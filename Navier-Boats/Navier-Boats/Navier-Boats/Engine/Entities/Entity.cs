using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Graphics;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Engine.Entities
{
    public abstract class Entity : Sprite
    {
        #region FIELDS

        private Vector2 acceleration;

        private Vector2 velocity;

        private float speed;

        protected float initialSpeed;

        #endregion

        #region Constants

        public const float WATER_SPEED_MULT = 0.25f;

        public const float SAND_SPEED_MULT = 0.6f;

        public const float GRASS_SPEED_MULT = 1.0f;

        public const float ROAD_SPEED_MULT = 1.05f;

        #endregion

        #region Properties

        public Rectangle BoundingRectangle()
        {
            // Use the Texture's Source Rectangle for Width and Height, if it's null just use the Texture's Bounding Rectangle
            Rectangle wh = SourceRectangle ?? Texture.Bounds;
            return new Rectangle((int)Position.X, (int)Position.Y, wh.Width, wh.Height);
        }

        public Vector2 Acceleration { get { return acceleration; } set { acceleration = value; } }

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

        public float Speed { get { return speed; } set { speed = value; } }

        public float InitialSpeed { get { return initialSpeed; } }

        #endregion

        public Entity()
            : base()
        {
            velocity = Vector2.Zero;
            speed = initialSpeed;
        }

        public virtual void Update(GameTime gameTime)
        {
            switch (CurrentLevel.GetInstance().GetTileDataAtPoint(TileLayer.GROUND_LAYER, Position))
            {
                case 3:
                    speed = initialSpeed * WATER_SPEED_MULT;
                    break;
                case 2:
                    speed = initialSpeed * SAND_SPEED_MULT;
                    break;
                case 0:
                    speed = initialSpeed * ROAD_SPEED_MULT;
                    break;
                default:
                    speed = initialSpeed * GRASS_SPEED_MULT;
                    break;
            }

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
