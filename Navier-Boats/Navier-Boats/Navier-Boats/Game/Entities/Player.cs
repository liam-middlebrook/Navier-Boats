using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using libXNADeveloperConsole;
using Navier_Boats.Engine.Graphics;

namespace Navier_Boats.Game.Entities
{
    public class Player : LivingEntity, IInputControllable
    {
        public Player(Vector2 position)
            : base(100, 32)
        {
            Position = position;
            Speed = 1000;
        }

        public void HandleInput(KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState)
        {
            Vector2 vel = Vector2.Zero;
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (!ConsoleWindow.GetInstance().IsActive)
            {
                float mult = 1.0f;
                if (gamePadState.IsConnected)
                {
                    mult += gamePadState.Triggers.Right;
                    vel.X += gamePadState.ThumbSticks.Left.X;
                    vel.Y += -gamePadState.ThumbSticks.Left.Y;
                }
                else
                {
                    vel.X += keyState.IsKeyDown(Keys.A) ? -1 : 0;
                    vel.X += keyState.IsKeyDown(Keys.D) ? 1 : 0;
                    vel.Y += keyState.IsKeyDown(Keys.W) ? -1 : 0;
                    vel.Y += keyState.IsKeyDown(Keys.S) ? 1 : 0;
                }
                vel.Normalize();
                vel *= mult;
            }
            Velocity = vel;

            Vector2 headScreenPos = Camera.ConvertToScreenCoords(headSprite.Position);

            float angle = (float)Math.Atan2(mouseState.Y - headScreenPos.X, mouseState.X - headScreenPos.X);
            headSprite.Rotation = MathHelper.SmoothStep(headSprite.Rotation, angle, 0.97f);
        }
    }
}
