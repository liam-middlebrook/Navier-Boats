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
    class Player : LivingEntity, IInputControllable
    {
        public Player(Vector2 position)
            : base(100)
        {
            Position = position;
            Speed = 1000;
        }

        public void HandleInput(KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState)
        {
            Vector2 vel = Vector2.Zero;
            if (!ConsoleWindow.GetInstance().IsActive)
            {
                vel.X += keyState.IsKeyDown(Keys.A) ? -1 : 0;
                vel.X += keyState.IsKeyDown(Keys.D) ? 1 : 0;
                vel.Y += keyState.IsKeyDown(Keys.W) ? -1 : 0;
                vel.Y += keyState.IsKeyDown(Keys.S) ? 1 : 0;
            }
            Velocity = vel;


            headSprite.Rotation = (float)Math.Atan2(mouseState.Y - headSprite.Position.Y, mouseState.X - headSprite.Position.X);

        }
    }
}
