using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Graphics;

namespace Navier_Boats.Engine.Entities
{
    class Entity : Sprite
    {
        private Vector2 acceleration;

        private Vector2 velocity;

        private float speed;

        public Vector2 Acceleration { get { return acceleration; } set { acceleration = value; } }

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

        public float Speed { get { return speed; } set { speed = value; } }

        public Entity()
            : base()
        {
            velocity = Vector2.Zero;
        }

        public virtual void Update(GameTime gameTime)
        {
            //Update Position based on Velocity * deltaTime
            velocity += acceleration * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            Position += velocity * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f) * speed;

            //Set rotation to point in direction of velocity
            if (velocity != Vector2.Zero)
            {
                Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            }
            acceleration = Vector2.Zero;
            velocity *= 0.9975f;
        }
    }
}
