using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Level;
using Navier_Boats.Engine.Pathfinding;
using Navier_Boats.Engine.Pathfinding.Threading;
using Navier_Boats.Engine.Inventory;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Game.Entities
{
    [Serializable]
    public class Wanderer : HostileLivingEntity
    {
        private static Pathfinder.Heuristic Heuristic = Heuristics.ResistanceRandomness(-0.1, 0.1, Heuristics.Distance);

        private enum AIState
        {
            None,
            Wandering,
            SubmitPathing,
            Pathing,
            Following,
            Attacking
        }

        private double timeUntilNewAccel = 0f;

        private double timeUntilNextPath = 1f;

        private AIState currentState = AIState.Wandering;

        private PathResult path = null;
        
        private PathJob currentJob = null;

        private int currentNodeIndex = 0;

        private bool canSubmit = false;

        public Wanderer(Vector2 position)
            : base(100)
        {
            Position = position;
            initialSpeed = 50;
            if(CurrentLevel.GetRandom().Next(2) == 1)
                this.Items.AddItem(new ItemStack(ItemManager.GetInstance().GetRandomItem(), CurrentLevel.GetRandom().Next(1, 5)));
        }

        protected Wanderer(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case AIState.Wandering:
                    {
                        timeUntilNewAccel -= gameTime.ElapsedGameTime.TotalSeconds;

                        if (currentJob != null)
                        {
                            currentJob.Cancelled = true;
                            currentJob = null;
                        }

                        float distanceToPlayer = Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position);
                        if (distanceToPlayer <= Math.Pow(Chunk.TILE_WIDTH * 2f, 2))
                        {
                            Velocity = Vector2.Zero;
                            currentState = AIState.Attacking;
                            break;
                        }
                        else if (distanceToPlayer <= 360000)
                        {
                            Velocity = Vector2.Zero;
                            currentState = AIState.SubmitPathing;
                            break;
                        }

                        if (timeUntilNewAccel <= 0)
                        {
                            Velocity = new Vector2((float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 5.0f, (float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 5.0f);
                            timeUntilNewAccel = CurrentLevel.GetRandom().NextDouble() * 3.0;
                        }
                        break;
                    }

                case AIState.SubmitPathing:
                    {
                        float distanceToPlayer = Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position);
                        if (distanceToPlayer <= Math.Pow(Chunk.TILE_WIDTH * 2f, 2))
                        {
                            Velocity = Vector2.Zero;
                            currentState = AIState.Attacking;
                            break;
                        }
                        else if (distanceToPlayer > 360000)
                        {
                            currentState = AIState.Wandering;
                            break;
                        }

                        Velocity = Vector2.Zero;
                        path = null;
                        currentNodeIndex = 0;
                        currentJob = new PathJob()
                        {
                            Start = this.Position,
                            End = EntityManager.GetInstance().Player.Position,
                            NodeSize = 32,
                            MaxTime = 0.5f,
                            Heuristic = Heuristic,
                            Callback = (result) =>
                                {
                                    if (currentState != AIState.Pathing)
                                        return;

                                    path = result;
                                    currentNodeIndex = 0;
                                    currentState = AIState.Following;
                                    timeUntilNextPath = 1f;
                                    canSubmit = true;
                                }
                        };
                        currentState = AIState.Pathing;
                        PathThreadPool.GetInstance().SubmitJob(currentJob);
                        break;
                    }

                case AIState.Pathing:
                    {
                        float distanceToPlayer = Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position);
                        if (distanceToPlayer <= Math.Pow(Chunk.TILE_WIDTH * 2f, 2))
                        {
                            Velocity = Vector2.Zero;
                            currentState = AIState.Attacking;
                            break;
                        }
                        else if (distanceToPlayer > 360000)
                        {
                            currentState = AIState.Wandering;
                            currentJob.Cancelled = true;
                            currentJob = null;
                            break;
                        }

                        Velocity = Vector2.Zero;
                        break;
                    }

                case AIState.Following:
                    {
                        float distanceToPlayer = Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position);
                        if (distanceToPlayer <= Math.Pow(Chunk.TILE_WIDTH * 2f, 2))
                        {
                            Velocity = Vector2.Zero;
                            currentState = AIState.Attacking;
                            break;
                        }
                        else if (distanceToPlayer > 360000)
                        {
                            currentState = AIState.Wandering;
                            currentJob.Cancelled = true;
                            currentJob = null;
                            break;
                        }

                        if (path != null)
                        {
                            if (path.Error == null)
                            {
                                if (currentNodeIndex < path.Path.Count)
                                {
                                    Vector2 currentNode = path.Path[currentNodeIndex] + new Vector2(16, 16);
                                    Rectangle validRect = new Rectangle((int)this.Position.X - 20, (int)this.Position.Y - 20, 40, 40);
                                    if (validRect.Contains((int)currentNode.X, (int)currentNode.Y))
                                    {
                                        currentNodeIndex++;
                                    }

                                    Rotation = (float)Math.Atan2(currentNode.Y - Position.Y, currentNode.X - Position.X);
                                    Velocity = new Vector2((float)Math.Cos(Rotation) * 2, (float)Math.Sin(Rotation) * 2);
                                }
                                else
                                {
                                    Velocity = Vector2.Zero;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Path error: " + path.Error);
                                path = null;
                            }
                        }

                        if (canSubmit)
                            timeUntilNextPath -= gameTime.ElapsedGameTime.TotalSeconds;

                        if (timeUntilNextPath <= 0 && canSubmit)
                        {
                            currentJob = new PathJob()
                            {
                                Start = this.Position,
                                End = EntityManager.GetInstance().Player.Position,
                                NodeSize = 32,
                                MaxTime = 0.5f,
                                Heuristic = Heuristic,
                                Callback = (result) =>
                                {
                                    if (currentState != AIState.Following)
                                        return;

                                    path = result;
                                    currentNodeIndex = 0;
                                    currentState = AIState.Following;
                                    canSubmit = true;
                                }
                            };
                            canSubmit = false;
                            timeUntilNextPath = 1f;
                            PathThreadPool.GetInstance().SubmitJob(currentJob);
                        }
                        break;
                    }

                case AIState.Attacking:
                    {
                        float distanceToPlayer = Vector2.DistanceSquared(this.Position, EntityManager.GetInstance().Player.Position);
                        if (distanceToPlayer > Math.Pow(Chunk.TILE_WIDTH * 2f, 2))
                        {
                            Velocity = Vector2.Zero;
                            currentState = AIState.Wandering;
                            break;
                        }
                        break;
                    }
            }

            // Make the Head look at the mouse
            headSprite.Rotation = Rotation;

            base.Update(gameTime);
        }

        public override void OnDeath()
        {
            if (this.currentJob != null)
                this.currentJob.Cancelled = true;

            base.OnDeath();
        }

        public override void Unload()
        {
            base.Unload();

            this.currentState = AIState.None;
            if (currentJob != null)
            {
                currentJob.Cancelled = true;
            }
        }
    }
}
