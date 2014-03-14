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
                if (gamePadState.IsConnected)
                {
                    float mult = 1.0f + gamePadState.Triggers.Right;
                    vel.X += gamePadState.ThumbSticks.Left.X * mult;
                    vel.Y += -gamePadState.ThumbSticks.Left.Y * mult;
                }
                else
                {
                    bool a = keyState.IsKeyDown(Keys.A);
                    bool d = keyState.IsKeyDown(Keys.D);
                    bool w = keyState.IsKeyDown(Keys.W);
                    bool s = keyState.IsKeyDown(Keys.S);

                    vel.X +=  a ? -1 : 0;
                    vel.X +=  d ? 1 : 0;
                    vel.Y +=  w ? -1 : 0;
                    vel.Y +=  s ? 1 : 0;

                    //Simplified from (a && w) || (w && d) || (d && s) || (s && a), yay boolean algebra
                    if ((a||d) && (s||w))
                    {
                        vel.Normalize();
                    }
                    
                }
            }
            Velocity = vel;

            Vector2 headScreenPos = Camera.ConvertToScreenCoords(headSprite.Position);

            float angle = (float)Math.Atan2(mouseState.Y - headScreenPos.X, mouseState.X - headScreenPos.X);
            headSprite.Rotation = MathHelper.SmoothStep(headSprite.Rotation, angle, 0.97f);
        }
    }
}
