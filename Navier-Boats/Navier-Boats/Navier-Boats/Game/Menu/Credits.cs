﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Navier_Boats.Engine.Graphics;
using Navier_Boats.Engine.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Game.Menu
{
    public class Credits : GameState
    {
        KeyboardState keyState;
        KeyboardState prevKeyState;

        protected override void Init()
        {
            keyState = Keyboard.GetState();

        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape) && prevKeyState.IsKeyUp(Keys.Escape))
            {
                StateManager.GetInstance().PopState(GameStates.MAIN_MENU);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }

        public override void DrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetInstance()["consolas"], "Zombie Fiesta\n\n\nCreated By:\n\nSam Bloomberg\nMichael Cohen\nTom Landi\nLiam Middlebrook\nSean Maraia\nSam Willis\n\nSpecial Thanks to our Backer(s)!\n\nBlack Lotus Into Storm Crow\n\n\nPress Escape to return to Main Menu", Vector2.Zero, Color.Black);
        }
    }
}
