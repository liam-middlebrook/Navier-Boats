using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Graphics;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Engine.Entities
{
    [Serializable]
    public abstract class Entity : Sprite
    {
        #region FIELDS

        /// <summary>
        /// The Acceleration of the Entity
        /// </summary>
        private Vector2 acceleration;

        /// <summary>
        /// The Velocity of the Entits
        /// </summary>
        private Vector2 velocity;

        /// <summary>
        /// The speed multiplier of the player
        /// </summary>
        private float speed;

        /// <summary>
        /// The default/initial speed of the player
        /// </summary>
        protected float initialSpeed;

        #endregion

        #region Constants

        /// <summary>
        /// The Speed Multiplier of an Entity while moving in WATER
        /// </summary>
        public const float WATER_SPEED_MULT = 0.25f;

        /// <summary>
        /// The Speed Multiplier of an Entity while moving in SAND
        /// </summary>
        public const float SAND_SPEED_MULT = 0.6f;

        /// <summary>
        /// The Speed Multiplier of an Entity while moving in GRASS
        /// </summary>
        public const float GRASS_SPEED_MULT = 1.0f;

        /// <summary>
        /// The Speed Multiplier of an Entity while moving in ROAD
        /// </summary>
        public const float ROAD_SPEED_MULT = 1.05f;

        #endregion

        #region Properties

        /// <summary>
        /// The bounding rectangle of the player (for collisions)
        /// </summary>
        /// <returns>The bounding rectangle of the player (for collisions)</returns>
        public Rectangle BoundingRectangle()
        {
            // Use the Texture's Source Rectangle for Width and Height, if it's null just use the Texture's Bounding Rectangle
            Rectangle wh = SourceRectangle ?? Texture.Bounds;
            return new Rectangle((int)Position.X, (int)Position.Y, wh.Width, wh.Height);
        }
        /// <summary>
        /// The Acceleration of the Entity
        /// </summary>
        public Vector2 Acceleration { get { return acceleration; } set { acceleration = value; } }

        /// <summary>
        /// The Speed of the Entity
        /// </summary>
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

        /// <summary>
        /// The speed multiplier of the player
        /// </summary>
        public float Speed { get { return speed; } set { speed = value; } }

        /// <summary>
        /// The default/initial speed of the player
        /// </summary>
        public float InitialSpeed { get { return initialSpeed; } }

        #endregion

        /// <summary>
        /// An entity that can move around in world space.
        /// </summary>
        public Entity()
            : base()
        {
            velocity = Vector2.Zero;
            speed = initialSpeed;
        }

        protected Entity(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.acceleration = (Vector2)info.GetValue("acceleration", typeof(Vector2));
            this.velocity = (Vector2)info.GetValue("velocity", typeof(Vector2));
            this.speed = info.GetSingle("speed");
            this.initialSpeed = info.GetSingle("initialSpeed");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("acceleration", acceleration);
            info.AddValue("velocity", velocity);
            info.AddValue("speed", speed);
            info.AddValue("initialSpeed", initialSpeed);
        }

        /// <summary>
        /// Updates the entity relative to time
        /// </summary>
        /// <param name="gameTime">Data about the time between update cycles for our game</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!CurrentLevel.GetInstance().IsChunkLoadedAtPoint(Position))
            {
                return;
            }

            //Change the speed of an entity realative to the type of tile it's walking on
            if (CurrentLevel.GetInstance().GetTileDataAtPoint(TileLayer.ROAD_LAYER, Position) != (short)TileType.Road)
            {
                switch (CurrentLevel.GetInstance().GetTileDataAtPoint(TileLayer.GROUND_LAYER, Position))
                {
                    case 3:
                        speed = initialSpeed * WATER_SPEED_MULT;
                        break;
                    case 2:
                        speed = initialSpeed * SAND_SPEED_MULT;
                        break;
                    default:
                        speed = initialSpeed * GRASS_SPEED_MULT;
                        break;
                }
            }
            else
            {
                speed = initialSpeed * ROAD_SPEED_MULT;
            }
            //Update Position based on Velocity * deltaTime
            velocity += acceleration * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            Position += velocity * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f) * speed;

            //Set rotation to point in direction of velocity
            if (velocity != Vector2.Zero)
            {
                Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            }

            //reset acceleration and dampen velocity
            acceleration = Vector2.Zero;
            velocity *= 0.9975f;
        }

        /// <summary>
        /// Called when the entity is in a chunk that is being unloaded.
        /// </summary>
        public virtual void Unload()
        {
        }
    }
}
