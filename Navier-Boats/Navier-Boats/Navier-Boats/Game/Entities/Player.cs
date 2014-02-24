using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Game.Entities
{
    class Player : LivingEntity
    {
        public Player(Vector2 position)
            : base(100)
        {
            Position = position;
            Speed = 100;
        }

        public void Update(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            // Change the player's velocity based on inputs
            Vector2 accel = Acceleration;
            accel.X += keyState.IsKeyDown(Keys.A) ? -1 : 0;
            accel.X += keyState.IsKeyDown(Keys.D) ? 1 : 0;
            accel.Y += keyState.IsKeyDown(Keys.W) ? -1 : 0;
            accel.Y += keyState.IsKeyDown(Keys.S) ? 1 : 0;
            Acceleration = accel;

            // Make the Head look at the mouse
            headSprite.Rotation = (float)Math.Atan2(mouseState.Y - headSprite.Position.Y, mouseState.X - headSprite.Position.X);

            base.Update(gameTime);
        }
    }
}
