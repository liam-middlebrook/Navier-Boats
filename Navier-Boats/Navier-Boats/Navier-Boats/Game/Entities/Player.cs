﻿using System;
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
using Navier_Boats.Engine.System;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Game.Entities
{
    public class Player : LivingEntity, IInputControllable, IDrawableGUI
    {
        public enum InventoryState
        {
            nothing,
            clicking,
            dragging,
            releasing
        }
        public enum PlayerState
        {
            playing,
            inventory,
            dead
        }

        private int selectedItemIndex;

        private int secondSelectedItemIndex;

        private List<Rectangle> invItemRects;

        private ItemStack tempItemStack;

        public ItemStack TempItemStack
        {
            get { return tempItemStack; }
            set { tempItemStack = value; }
        }

        private InventoryState curInvState;

        public InventoryState CurInvState
        {
            get { return curInvState; }
            set { curInvState = value; }
        }

        private PlayerState curState;

        public PlayerState CurState
        {
            get { return curState; }
            set { curState = value; }
        }

        #region HUD Element Rectangles
        //Locations for the HUD elements
        private Rectangle HUDItemBoxRectOne;
        private Rectangle HUDItemBoxRectTwo;
        private Rectangle HUDItemBoxRectThree;
        private Rectangle HUDItemBoxRectFour;
        private Rectangle HUDItemBoxRectFive;
        private Rectangle CompassRect;
        private Rectangle HighlightRect;
        #endregion

        private int previousMouseWheelValue = 0;
        private bool clickLastFrame = false;
        private bool firstFrameI = true;

        public Player(Vector2 position)
            : base(100, 32)
        {
            curState = PlayerState.playing;
            curInvState = InventoryState.nothing;
            Position = position;
            initialSpeed = 300;

            invItemRects = new List<Rectangle>();

            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    invItemRects.Add(new Rectangle(((ConsoleVars.GetInstance().WindowWidth * 117) / 990) + ((i * ConsoleVars.GetInstance().WindowWidth * 49) / 495), ((ConsoleVars.GetInstance().WindowHeight * 4) / 10) + ((ConsoleVars.GetInstance().WindowHeight * k) / 8), (ConsoleVars.GetInstance().WindowHeight) / 11, (ConsoleVars.GetInstance().WindowHeight) / 11));
                }
            }

            #region HUD Rectangle Initiation
            HUDItemBoxRectOne = new Rectangle(ConsoleVars.GetInstance().WindowWidth - 724,ConsoleVars.GetInstance().WindowHeight - 124, 75, 75);
            HUDItemBoxRectTwo = new Rectangle(ConsoleVars.GetInstance().WindowWidth - 604,ConsoleVars.GetInstance().WindowHeight - 124, 75, 75);
            HUDItemBoxRectThree = new Rectangle(ConsoleVars.GetInstance().WindowWidth - 484,ConsoleVars.GetInstance().WindowHeight - 124, 75, 75);
            HUDItemBoxRectFour = new Rectangle(ConsoleVars.GetInstance().WindowWidth - 364,ConsoleVars.GetInstance().WindowHeight - 124, 75, 75);
            HUDItemBoxRectFive = new Rectangle(ConsoleVars.GetInstance().WindowWidth - 244,ConsoleVars.GetInstance().WindowHeight - 124, 75, 75);
            CompassRect = new Rectangle(ConsoleVars.GetInstance().WindowWidth - 124, 50, 75, 75);
            #endregion
        }

        public override void OnDeath()
        {
            base.OnDeath();
            curState = PlayerState.dead;
        }

        public void HandleInput(KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState)
        {
            if (curState == PlayerState.playing)
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

                        if (keyState.IsKeyDown(Keys.I))
                        {
                            if (!firstFrameI)
                            {
                                firstFrameI = true;
                                curState = PlayerState.inventory;
                            }
                        }
                        else
                        {
                            firstFrameI = false;
                        }

                        if (keyState.IsKeyDown(Keys.D1))
                        {
                            Items.SelectedItemIndex = 0;
                        }
                        else if (keyState.IsKeyDown(Keys.D2))
                        {
                            Items.SelectedItemIndex = 1;
                        }
                        else if (keyState.IsKeyDown(Keys.D3))
                        {
                            Items.SelectedItemIndex = 2;
                        }
                        else if (keyState.IsKeyDown(Keys.D4))
                        {
                            Items.SelectedItemIndex = 3;
                        }
                        else if (keyState.IsKeyDown(Keys.D5))
                        {
                            Items.SelectedItemIndex = 4;
                        }
                        if (mouseState.ScrollWheelValue < previousMouseWheelValue)
                        {
                            Items.SelectedItemIndex = (Items.SelectedItemIndex + 1) % 5;
                        }
                        if (mouseState.ScrollWheelValue > previousMouseWheelValue)
                        {
                            if (Items.SelectedItemIndex - 1 < 0)
                            {
                                Items.SelectedItemIndex = 4;
                            }
                            else
                            {
                                Items.SelectedItemIndex -= 1;
                            }
                        }
                        if (mouseState.LeftButton == ButtonState.Pressed && clickLastFrame == false && Items.SelectedItem != null && Items.SelectedItem.Item != null && Items.SelectedItem.Amount > 0)
                        {
                            Items.SelectedItem.Item.OnAction(this);
                            clickLastFrame = true;
                        }
                        else if (mouseState.LeftButton == ButtonState.Released)
                        {
                            clickLastFrame = false;
                        }

                        previousMouseWheelValue = mouseState.ScrollWheelValue;

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
            else if (curState == PlayerState.inventory)
            {
                Velocity = Vector2.Zero;
                if (curInvState == InventoryState.nothing)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        bool indexSelected = false;
                        foreach (Rectangle temp in invItemRects)
                        {
                            if (temp.Contains(new Point(mouseState.X, mouseState.Y)))
                            {
                                selectedItemIndex = invItemRects.IndexOf(temp);
                                indexSelected = true;
                            }
                        }
                        if (indexSelected)
                        {
                            tempItemStack = Items.Items[selectedItemIndex];
                            Items.Items[selectedItemIndex] = null;
                            curInvState = InventoryState.dragging;
                        }
                    }
                }
                if (curInvState == InventoryState.dragging)
                {
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        bool secondItemSelected = false;
                        foreach (Rectangle temp in invItemRects)
                        {
                            if (temp.Contains(new Point(mouseState.X, mouseState.Y)))
                            {
                                secondItemSelected = true;
                                secondSelectedItemIndex = invItemRects.IndexOf(temp);
                            }
                        }
                        if (secondItemSelected)
                        {
                            if (secondSelectedItemIndex == selectedItemIndex)
                            {
                                Items.Items[selectedItemIndex] = tempItemStack;
                                tempItemStack = null;
                                curInvState = InventoryState.nothing;
                            }
                            else
                            {
                                if (Items.Items[secondSelectedItemIndex] == null)
                                {
                                    Items.Items[secondSelectedItemIndex] = tempItemStack;
                                    selectedItemIndex = -1;
                                    secondSelectedItemIndex = -1;
                                    curInvState = InventoryState.nothing;
                                }
                                else if (Items.Items[secondSelectedItemIndex].Item == tempItemStack.Item)
                                {
                                    Items.Items[secondSelectedItemIndex].Amount += Items.Items[selectedItemIndex].Amount;
                                    if (Items.Items[secondSelectedItemIndex].Amount > 32)
                                    {
                                        Items.Items[secondSelectedItemIndex].Amount = 32;
                                    }
                                    tempItemStack = null;
                                    secondSelectedItemIndex = -1;
                                    selectedItemIndex = -1;
                                    curInvState = InventoryState.nothing;
                                }
                                else
                                {
                                    Items.Items[selectedItemIndex] = Items.Items[secondSelectedItemIndex];
                                    Items.Items[secondSelectedItemIndex] = tempItemStack;
                                    tempItemStack = null;
                                    secondSelectedItemIndex = -1;
                                    selectedItemIndex = -1;
                                    curInvState = InventoryState.nothing;
                                }
                            }
                        }
                        else
                        {
                            Items.Items[selectedItemIndex] = tempItemStack;
                            tempItemStack = null;
                            selectedItemIndex = -1;
                            secondSelectedItemIndex = -1;
                            curInvState = InventoryState.nothing;
                        }
                    }
                }
                if (keyState.IsKeyDown(Keys.I))
                {
                    if (!firstFrameI)
                    {
                        curState = PlayerState.playing;
                        firstFrameI = true;
                    }
                }
                else
                {
                    firstFrameI = false;
                }
            }
        }

        public void DrawGUI(SpriteBatch spriteBatch)
        {
            if (Items.SelectedItemIndex == 0)
            {
                HighlightRect = new Rectangle(HUDItemBoxRectOne.X - 5, HUDItemBoxRectOne.Y - 5, HUDItemBoxRectOne.Width + 10, HUDItemBoxRectOne.Height + 10);
            } 
            if (Items.SelectedItemIndex == 1)
            {
                HighlightRect = new Rectangle(HUDItemBoxRectTwo.X - 5, HUDItemBoxRectTwo.Y - 5, HUDItemBoxRectTwo.Width + 10, HUDItemBoxRectTwo.Height + 10);
            } 
            if (Items.SelectedItemIndex == 2)
            {
                HighlightRect = new Rectangle(HUDItemBoxRectThree.X - 5, HUDItemBoxRectThree.Y - 5, HUDItemBoxRectThree.Width + 10, HUDItemBoxRectThree.Height + 10);
            } 
            if (Items.SelectedItemIndex == 3)
            {
                HighlightRect = new Rectangle(HUDItemBoxRectFour.X - 5, HUDItemBoxRectFour.Y - 5, HUDItemBoxRectFour.Width + 10, HUDItemBoxRectFour.Height + 10);
            } 
            if (Items.SelectedItemIndex == 4)
            {
                HighlightRect = new Rectangle(HUDItemBoxRectFive.X - 5, HUDItemBoxRectFive.Y - 5, HUDItemBoxRectFive.Width + 10, HUDItemBoxRectFive.Height + 10);
            }

            spriteBatch.Draw(TextureManager.GetInstance()["HighlightTexture"], HighlightRect, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectOne, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectTwo, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectThree, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectFour, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HUDItemBoxTexture"], HUDItemBoxRectFive, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["HealthTexture"], new Rectangle(ConsoleVars.GetInstance().WindowWidth - 974, ConsoleVars.GetInstance().WindowHeight - 99, 200, 50), Color.Black);
            spriteBatch.Draw(TextureManager.GetInstance()["HealthTexture"], new Rectangle(ConsoleVars.GetInstance().WindowWidth - 973, ConsoleVars.GetInstance().WindowHeight - 98, (int)(2 * Health) - 2, 48), Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["MoneyTexture"], new Rectangle(ConsoleVars.GetInstance().WindowWidth - 974, ConsoleVars.GetInstance().WindowHeight - 134, 200 , 35), Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["CompassTexture"], CompassRect, Color.White);

            //Draw text indicating precise value of player health
            SpriteFont drawFont = FontManager.GetInstance()["Console Font"];

            // Get how large the text will be when drawn
            Vector2 fontSize = drawFont.MeasureString(string.Format("{0:00.00}", Health));
            
            //Get the position to draw the text to
            Vector2 healthTextPos = new Vector2(ConsoleVars.GetInstance().WindowWidth - 874, ConsoleVars.GetInstance().WindowHeight - 76) - fontSize / 2;
            spriteBatch.DrawString(drawFont, string.Format("{0:00.00}", Health), healthTextPos, Color.White);

            Vector2 moneyTextPos = new Vector2(ConsoleVars.GetInstance().WindowWidth - 874, ConsoleVars.GetInstance().WindowHeight - 111) - fontSize / 2;
            spriteBatch.DrawString(drawFont, this.Money.ToString(), moneyTextPos, Color.Black);

            //Draw text indicating the number of items in the stack of items this appears over
            SpriteFont itemFont = FontManager.GetInstance()["Console Font"];


            //Get the position to draw the text for the five on screen item stacks
            Vector2 itemTextPosOne = new Vector2(ConsoleVars.GetInstance().WindowWidth - 674, ConsoleVars.GetInstance().WindowHeight - 84);
            Vector2 itemTextPosTwo = new Vector2(ConsoleVars.GetInstance().WindowWidth - 554, ConsoleVars.GetInstance().WindowHeight - 84);
            Vector2 itemTextPosThree = new Vector2(ConsoleVars.GetInstance().WindowWidth - 434, ConsoleVars.GetInstance().WindowHeight - 84);
            Vector2 itemTextPosFour = new Vector2(ConsoleVars.GetInstance().WindowWidth - 314, ConsoleVars.GetInstance().WindowHeight - 84);
            Vector2 itemTextPosFive = new Vector2(ConsoleVars.GetInstance().WindowWidth - 194, ConsoleVars.GetInstance().WindowHeight - 84);
            #region HUD Item Number Drawing
            if (Items.Items[0] == null || Items.Items[0].Item == null || Items.Items[0].Item.ItemTexture == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosOne, Color.White);
            }
            else
            {
                spriteBatch.Draw(Items.Items[0].Item.ItemTexture, HUDItemBoxRectOne, Color.White);
                spriteBatch.DrawString(itemFont, Items.Items[0].Amount.ToString(), itemTextPosOne, Color.White);
            }


            if (Items.Items[1] == null || Items.Items[1].Item == null || Items.Items[1].Item.ItemTexture == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosTwo, Color.White);
            }
            else
            {
                spriteBatch.Draw(Items.Items[1].Item.ItemTexture, HUDItemBoxRectTwo, Color.White);
                spriteBatch.DrawString(itemFont, Items.Items[1].Amount.ToString(), itemTextPosTwo, Color.White);
            }


            if (Items.Items[2] == null || Items.Items[2].Item == null || Items.Items[2].Item.ItemTexture == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosThree, Color.White);
            }
            else
            {
                spriteBatch.Draw(Items.Items[2].Item.ItemTexture, HUDItemBoxRectThree, Color.White);
                spriteBatch.DrawString(itemFont, Items.Items[2].Amount.ToString(), itemTextPosThree, Color.White);
            }


            if (Items.Items[3] == null || Items.Items[3].Item == null || Items.Items[3].Item.ItemTexture == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosFour, Color.White);
            }
            else
            {
                spriteBatch.Draw(Items.Items[3].Item.ItemTexture, HUDItemBoxRectFour, Color.White);
                spriteBatch.DrawString(itemFont, Items.Items[3].Amount.ToString(), itemTextPosFour, Color.White);
            }


            if (Items.Items[4] == null || Items.Items[4].Item == null || Items.Items[4].Item.ItemTexture == null)
            {
                spriteBatch.DrawString(itemFont, "0", itemTextPosFive, Color.White);
            }
            else
            {
                spriteBatch.Draw(Items.Items[4].Item.ItemTexture, HUDItemBoxRectFive, Color.White);
                spriteBatch.DrawString(itemFont, Items.Items[4].Amount.ToString(), itemTextPosFive, Color.White);
            }
            #endregion

            //Draws the inventory when I is pressed
            if (curState == PlayerState.inventory)
            {
                spriteBatch.Draw(TextureManager.GetInstance()["HighlightTexture"], new Rectangle(ConsoleVars.GetInstance().WindowWidth / 10, ConsoleVars.GetInstance().WindowHeight / 10, (ConsoleVars.GetInstance().WindowWidth * 8) / 10, (ConsoleVars.GetInstance().WindowHeight * 8) / 10), Color.Gray);
                foreach (Rectangle temp in invItemRects)
                {
                    spriteBatch.Draw(TextureManager.GetInstance()["HighlightTexture"], temp, Color.DarkGray);
                }
                for (int i = 0; i < 8; i++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //spriteBatch.Draw(TextureManager.GetInstance()["HighlightTexture"], new Rectangle(((ConsoleVars.GetInstance().WindowWidth * 117) / 990) + ((i * ConsoleVars.GetInstance().WindowWidth * 49) / 495), ((ConsoleVars.GetInstance().WindowHeight * 4) / 10)  + ((ConsoleVars.GetInstance().WindowHeight * k) / 8), (ConsoleVars.GetInstance().WindowHeight) / 11, (ConsoleVars.GetInstance().WindowHeight) / 11), Color.DarkGray);
                        int itemIndex = i + (8 * k);
                        if(Items.Items[itemIndex] != null && Items.Items[itemIndex].Item != null && Items.Items[itemIndex].Item.InventoryTexture != null)
                        {
                            spriteBatch.Draw(Items.Items[itemIndex].Item.ItemTexture, new Rectangle(((ConsoleVars.GetInstance().WindowWidth * 117) / 990) + ((i * ConsoleVars.GetInstance().WindowWidth * 49) / 495), ((ConsoleVars.GetInstance().WindowHeight * 4) / 10) + ((ConsoleVars.GetInstance().WindowHeight * k) / 8), (ConsoleVars.GetInstance().WindowHeight) / 11, (ConsoleVars.GetInstance().WindowHeight) / 11), Color.White);
                            spriteBatch.DrawString(drawFont, Items.Items[itemIndex].Amount.ToString(), new Vector2((float)(((ConsoleVars.GetInstance().WindowWidth * 117) / 990) + ((i * ConsoleVars.GetInstance().WindowWidth * 49) / 495) + 45), (float)(((ConsoleVars.GetInstance().WindowHeight * 4) / 10) + (((ConsoleVars.GetInstance().WindowHeight * k) / 8))) + 50), Color.White);
                        }
                    }
                }

                if (tempItemStack != null && curInvState == InventoryState.dragging)
                {
                    spriteBatch.Draw(tempItemStack.Item.ItemTexture, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, (ConsoleVars.GetInstance().WindowHeight) / 11, (ConsoleVars.GetInstance().WindowHeight) / 11), Color.White);
                }
            }
            else if (curState == PlayerState.dead)
            {
                //Draw death screen here
            }



            // Draw Mouse Cursor
            Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            spriteBatch.Draw(TextureManager.GetInstance()["cursor"], mousePos, Color.White);
        }

        public override void Interact(IInteractable interactor)
        {
            //not yet implemented
        }
    }
}
