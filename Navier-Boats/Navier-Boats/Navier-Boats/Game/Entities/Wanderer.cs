using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Level;
using Navier_Boats.Engine.Pathfinding;
using Navier_Boats.Engine.Pathfinding.Threading;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Game.Entities
{
    public class Wanderer : HostileLivingEntity
    {
        private enum AIState
        {
            Wandering,
            Following
        }

        private double timeUntilNewAccel = 0f;

        private AIState currentState = AIState.Wandering;

        private bool submitJob = false;

        private PathResult path = null;

        private int currentNodeIndex = 0;

        public Wanderer(Vector2 position)
            : base(100)
        {
            Position = position;
            initialSpeed = 50;
        }

        public override void Update(GameTime gameTime)
        {
            timeUntilNewAccel -= gameTime.ElapsedGameTime.TotalSeconds;

            switch(currentState)
            {
                case AIState.Wandering:
                    if (Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position) <= 230400)
                    {
                        currentState = AIState.Following;
                        submitJob = false;
                        path = null;
                        currentNodeIndex = 0;
                        break;
                    }

                    if (timeUntilNewAccel <= 0)
                    {
                        Velocity = new Vector2((float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 5.0f, (float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 5.0f);
                        timeUntilNewAccel = CurrentLevel.GetRandom().NextDouble() * 3.0;
                    }
                    break;

                case AIState.Following:
                    if (Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position) > 230400)
                    {
                        currentState = AIState.Wandering;
                        break;
                    }

                    if (!submitJob)
                    {
                        PathJob job = new PathJob();
                        job.Start = this.Position;
                        job.End = EntityManager.GetInstance().Player.Position;
                        job.NodeSize = 32;
                        job.MaxTime = 10f;
                        job.Heuristic = Heuristics.Manhattan;
                        job.Callback = (result) =>
                            {
                                if (currentState != AIState.Following)
                                    return;
                                path = result;
                                currentNodeIndex = 0;
                                timeUntilNewAccel = 1f;
                            };
                        PathThreadPool.GetInstance().SubmitJob(job);
                        submitJob = true;
                    }

                    if (path != null)
                    {
                        if (path.Error == null)
                        {
                            if (currentNodeIndex < path.Path.Count)
                            {
                                Vector2 currentNode = path.Path[currentNodeIndex];
                                Vector2 modPos = new Vector2(Position.X - Position.X % 32, Position.Y - Position.Y % 32);
                                if (currentNode.Equals(modPos))
                                {
                                    currentNodeIndex++;
                                }
                                else
                                {
                                    Rotation = (float)Math.Atan2(currentNode.Y - Position.Y, currentNode.X - Position.X);
                                    Velocity = new Vector2((float)Math.Cos(Rotation) * 2, (float)Math.Sin(Rotation) * 2);
                                }
                            }
                            else
                            {
                                Velocity = Vector2.Zero;
                            }
                        }

                        if (timeUntilNewAccel <= 0)
                        {
                            submitJob = false;
                        }
                    }
                    break;
            }

            // Make the Head look at the mouse
            headSprite.Rotation = Rotation;

            base.Update(gameTime);
        }
    }
}
