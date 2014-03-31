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
using Navier_Boats.Engine.Inventory;

namespace Navier_Boats.Game.Entities
{
    public class Player : LivingEntity, IInputControllable, IDrawableGUI
    {
        #region HUD Element Rectangles
        //Locations for the HUD elements
        private Rectangle HUDItemBoxRectOne;
        private Rectangle HUDItemBoxRectTwo;
        private Rectangle HUDItemBoxRectThree;
        private Rectangle HUDItemBoxRectFour;
        private Rectangle HUDItemBoxRectFive;
        private Rectangle CompassRect;
        #endregion

        Inventory playerInv;

        public Player(Vector2 position)
            : base(100, 32)
        {
            Position = position;
            initialSpeed = 300;

            playerInv = new Inventory(64);

            #region HUD Rectangle Initiation
            HUDItemBoxRectOne = new Rectangle(300, 900, 75, 75);
            HUDItemBoxRectTwo = new Rectangle(420, 900, 75, 75);
            HUDItemBoxRectThree = new Rectangle(540, 900, 75, 75);
            HUDItemBoxRectFour = new Rectangle(660, 900, 75, 75);
            HUDItemBoxRectFive = new Rectangle(780, 900, 75, 75);
            CompassRect = new Rectangle(900, 50, 75, 75);
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

        public void DrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectOne, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectTwo, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectThree, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectFour, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectFive, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HealthTexture"], new Rectangle(50, 900, 200, 75), Color.Black);
            spriteBatch.Draw(TextureManager.GetInstance()["HealthTexture"], new Rectangle(51, 901, (int)(2 * Health) - 2, 73), Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["CompassTexture"], CompassRect, Color.White);

            //Draw text indicating precise value of player health
            SpriteFont drawFont = FontManager.GetInstance()["Console Font"];

            // Get how large the text will be when drawn
            Vector2 fontSize = drawFont.MeasureString(string.Format("{0:00.00}", Health));
            
            //Get the position to draw the text to
            Vector2 healthTextPos = new Vector2(150, 938) - fontSize / 2;
            spriteBatch.DrawString(drawFont, string.Format("{0:00.00}", Health), healthTextPos, Color.White);



            //Draw text indicating the number of items in the stack of items this appears over
            SpriteFont itemFont = FontManager.GetInstance()["Console Font"];


            //Get the position to draw the text for the five on screen item stacks
            Vector2 itemTextPosOne = new Vector2(350, 940);
            Vector2 itemTextPosTwo = new Vector2(470, 940);
            Vector2 itemTextPosThree = new Vector2(590, 940);
            Vector2 itemTextPosFour = new Vector2(710, 940);
            Vector2 itemTextPosFive = new Vector2(830, 940);
            #region HUD Item Number Drawing
            if (playerInv.Items[0] == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosOne, Color.White);
            }
            else
            {
                spriteBatch.DrawString(itemFont, playerInv.Items[0].Amount.ToString(), itemTextPosOne, Color.White);
            }
            if (playerInv.Items[1] == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosTwo, Color.White);
            }
            else
            {
                spriteBatch.DrawString(itemFont, playerInv.Items[1].Amount.ToString(), itemTextPosTwo, Color.White);
            } 
            if (playerInv.Items[2] == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosThree, Color.White);
            }
            else
            {
                spriteBatch.DrawString(itemFont, playerInv.Items[2].Amount.ToString(), itemTextPosThree, Color.White);
            } 
            if (playerInv.Items[3] == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosFour, Color.White);
            }
            else
            {
                spriteBatch.DrawString(itemFont, playerInv.Items[3].Amount.ToString(), itemTextPosFour, Color.White);
            } 
            if (playerInv.Items[4] == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosFive, Color.White);
            }
            else
            {
                spriteBatch.DrawString(itemFont, playerInv.Items[4].Amount.ToString(), itemTextPosFive, Color.White);
            }
            #endregion

        }
    }
}
