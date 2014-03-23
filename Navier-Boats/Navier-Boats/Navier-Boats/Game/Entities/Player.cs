using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using libXNADeveloperConsole;
using Navier_Boats.Engine.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace Navier_Boats.Game.Entities
{
    public class Player : LivingEntity, IInputControllable
    {
        #region HUD Textures and Rectangles
        //Textures for the HUD elements
        private Texture2D CompassTexture;
        private Texture2D HealthTexture;
        private Texture2D HUDItemBoxTexture;

        //Locations for the HUD elements
        private Rectangle HUDItemBoxRectOne;
        private Rectangle HUDItemBoxRectTwo;
        private Rectangle HUDItemBoxRectThree;
        private Rectangle HUDItemBoxRectFour;
        private Rectangle HUDItemBoxRectFive;
        private Rectangle HealthRect;
        private Rectangle CompassRect;
        #endregion

        //Playes current health
        private int health;

        public Player(Vector2 position)
            : base(100, 32)
        {
            Position = position;
            initialSpeed = 1000;

            health = 100;

            #region HUD Rectangle Initiation\
            HUDItemBoxRectOne = new Rectangle(100, 50, 100, 100);
            HUDItemBoxRectTwo = new Rectangle(220, 50, 100, 100);
            HUDItemBoxRectThree = new Rectangle(340, 50, 100, 100);
            HUDItemBoxRectFour = new Rectangle(410, 50, 100, 100);
            HUDItemBoxRectFive = new Rectangle(52, 50, 100, 100);
            CompassRect = new Rectangle(900, 50, 100, 100);
            #endregion
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
                if (vel.LengthSquared() != 0)
                {
                    vel.X = vel.X / vel.Length();
                    vel.Y = vel.Y / vel.Length();
                }

                vel *= mult;
            }
            Velocity = vel;

            Vector2 headScreenPos = Camera.ConvertToScreenCoords(headSprite.Position);

            float angle = (float)Math.Atan2(mouseState.Y - headScreenPos.X, mouseState.X - headScreenPos.X);
            headSprite.Rotation = MathHelper.SmoothStep(headSprite.Rotation, angle, 0.97f);
        }

        public void DrawGUI(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            CompassTexture = new Texture2D(graphicsDevice, 5, 5, false, SurfaceFormat.Color);
            Color[] color = new Color[25];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.Purple;
            }
            CompassTexture.SetData(color);

            HUDItemBoxTexture = new Texture2D(graphicsDevice, 5, 5, false, SurfaceFormat.Color);
            Color[] colorOne = new Color[25];
            for (int i = 0; i < colorOne.Length; i++)
            {
                color[i] = Color.Black;
            }
            HUDItemBoxTexture.SetData(colorOne);

            HealthTexture = new Texture2D(graphicsDevice, 5, 5, false, SurfaceFormat.Color);
            Color[] colorTwo = new Color[25];
            for (int i = 0; i < colorTwo.Length; i++)
            {
                color[i] = Color.Red;
            }
            HealthTexture.SetData(colorTwo);

            spriteBatch.Draw(HUDItemBoxTexture, HUDItemBoxRectOne, Color.White);
            spriteBatch.Draw(HUDItemBoxTexture, HUDItemBoxRectTwo, Color.White);
            spriteBatch.Draw(HUDItemBoxTexture, HUDItemBoxRectThree, Color.White);
            spriteBatch.Draw(HUDItemBoxTexture, HUDItemBoxRectFour, Color.White);
            spriteBatch.Draw(HUDItemBoxTexture, HUDItemBoxRectFive, Color.White);
            spriteBatch.Draw(HealthTexture, new Rectangle(10, health, 100, 100), Color.White);
            spriteBatch.Draw(CompassTexture, CompassRect, Color.White);


        }
    }
}
