using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using libXNADeveloperConsole;

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

        public void Update(GameTime gameTime, KeyboardHelper keyHelper, MouseState mouseState)
        { 
            // Change the player's velocity based on inputs
            Vector2 vel = Vector2.Zero;
            if (!ConsoleWindow.GetInstance().IsActive)
            {
                vel.X += keyHelper.KeyState.IsKeyDown(Keys.A) ? -1 : 0;
                vel.X += keyHelper.KeyState.IsKeyDown(Keys.D) ? 1 : 0;
                vel.Y += keyHelper.KeyState.IsKeyDown(Keys.W) ? -1 : 0;
                vel.Y += keyHelper.KeyState.IsKeyDown(Keys.S) ? 1 : 0;
            }
            Velocity = vel;

            // Make the Head look at the mouse
            headSprite.Rotation = (float)Math.Atan2(mouseState.Y - headSprite.Position.Y, mouseState.X - headSprite.Position.X);

            base.Update(gameTime);
        }
    }
}
