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

        private int currentNode = 0;

        public Wanderer(Vector2 position)
            : base(100)
        {
            Position = position;
            initialSpeed = 50;
        }

        public override void Update(GameTime gameTime)
        {
            switch(currentState)
            {
                case AIState.Wandering:
                    if (Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position) <= 76800)
                    {
                        currentState = AIState.Following;
                        submitJob = false;
                        path = null;
                        currentNode = 0;
                        break;
                    }

                    timeUntilNewAccel -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeUntilNewAccel <= 0)
                    {
                        Velocity = new Vector2((float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 5.0f, (float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 5.0f);
                        timeUntilNewAccel = CurrentLevel.GetRandom().NextDouble() * 3.0;
                    }
                    break;

                case AIState.Following:
                    timeUntilNewAccel = 0f;

                    if (Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position) > 76800)
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
                                path = result;
                            };
                        path = null;
                        currentNode = 0;
                        PathThreadPool.GetInstance().SubmitJob(job);
                        submitJob = true;
                    }
                    else if (path != null)
                    {
                        if (path.Error != null)
                        {
                            path = null;
                            submitJob = false;
                            break;
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
