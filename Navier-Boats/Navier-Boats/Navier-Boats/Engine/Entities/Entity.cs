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
        private Vector2 velocity;

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

        public Entity()
            : base()
        {
            velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            Position += velocity * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
        }
    }
}
