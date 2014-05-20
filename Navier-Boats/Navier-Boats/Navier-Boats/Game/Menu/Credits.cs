using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Navier_Boats.Engine.Graphics;
using Navier_Boats.Engine.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.System;


namespace Navier_Boats.Game.Menu
{
    public class Credits : GameState
    {
        KeyboardState keyState;
        KeyboardState prevKeyState;

        MouseState prevMouseState;
        MouseState mouseState;

        Rectangle menuLoc;
        protected override void Init()
        {
            keyState = Keyboard.GetState();
            menuLoc = new Rectangle(ConsoleVars.GetInstance().WindowWidth / 3, ConsoleVars.GetInstance().WindowHeight * 4 / 5, ConsoleVars.GetInstance().WindowWidth / 3, ConsoleVars.GetInstance().WindowHeight / 6);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {

        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

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
            spriteBatch.Draw(TextureManager.GetInstance()["HighlightTexture"], menuLoc, Color.White);

            spriteBatch.DrawString(FontManager.GetInstance()["consolas"], "Zombie Fiesta\n\n\nCreated By:\n\nSam Bloomberg\nMichael Cohen\nTom Landi\nLiam Middlebrook\nSean Maraia\nSam Willis\n\nSpecial Thanks to our Backer(s)!\n\nBlack Lotus Into Storm Crow\n\nOut-Sourced Art By:\n\nAdvisable Robin\n\n\nPress Escape to return to Main Menu", Vector2.Zero, Color.Black);

            spriteBatch.Draw(TextureManager.GetInstance()["CursorTexture"], new Vector2(mouseState.X, mouseState.Y), Color.White);
        }
    }
}
