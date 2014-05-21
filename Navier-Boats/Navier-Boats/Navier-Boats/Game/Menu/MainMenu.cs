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
    public class MainMenu : GameState
    {
        KeyboardState keyState;
        KeyboardState prevKeyState;

        MouseState prevMouseState;
        MouseState mouseState;

        Rectangle creditsLoc;
        Rectangle playLoc;

        protected override void Init()
        {
            TextureManager.GetInstance().LoadTexture("Credits");
            TextureManager.GetInstance().LoadTexture("Play");
            TextureManager.GetInstance().LoadTexture("menu");
            keyState = Keyboard.GetState();
            //creditsLoc = new Rectangle(ConsoleVars.GetInstance().WindowWidth / 3, ConsoleVars.GetInstance().WindowHeight / 2, ConsoleVars.GetInstance().WindowWidth / 3, ConsoleVars.GetInstance().WindowHeight / 5);
            //playLoc = new Rectangle(ConsoleVars.GetInstance().WindowWidth / 6, ConsoleVars.GetInstance().WindowHeight / 5, ConsoleVars.GetInstance().WindowWidth * 2 / 3, ConsoleVars.GetInstance().WindowHeight / 4);
            creditsLoc = new Rectangle(ConsoleVars.GetInstance().WindowWidth / 2, ConsoleVars.GetInstance().WindowHeight * 5 / 6
            , ConsoleVars.GetInstance().WindowWidth / 100 * TextureManager.GetInstance().LoadTexture("Credits").Width, ConsoleVars.GetInstance().WindowHeight / 100 * TextureManager.GetInstance().LoadTexture("Credits").Height);
            creditsLoc.X -= creditsLoc.Width / 2;
            playLoc = new Rectangle(ConsoleVars.GetInstance().WindowWidth / 2, ConsoleVars.GetInstance().WindowHeight * 4 / 6
            , ConsoleVars.GetInstance().WindowWidth / 100 * TextureManager.GetInstance().LoadTexture("Play").Width, ConsoleVars.GetInstance().WindowHeight / 100 * TextureManager.GetInstance().LoadTexture("Play").Height);
            playLoc.X -= playLoc.Width / 2;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            TextureManager.GetInstance().LoadTexture("menu");
            TextureManager.GetInstance().LoadTexture("Credits");
            TextureManager.GetInstance().LoadTexture("Play");
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            /*
            if (keyState.IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
            {
                StateManager.GetInstance().PushState(GameStates.GAMEPLAY);
            }
            if (keyState.IsKeyDown(Keys.C) && prevKeyState.IsKeyUp(Keys.C))
            {
                StateManager.GetInstance().PushState(GameStates.CREDITS);
            }
             * */
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && creditsLoc.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                StateManager.GetInstance().PushState(GameStates.CREDITS);
            }
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && playLoc.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                StateManager.GetInstance().PushState(GameStates.GAMEPLAY);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }

        public override void DrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.GetInstance()["menu"], new Rectangle(0, 0, ConsoleVars.GetInstance().WindowWidth, ConsoleVars.GetInstance().WindowHeight), Color.White);

            //spriteBatch.Draw(TextureManager.GetInstance()["HighlightTexture"], creditsLoc, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["Credits"], creditsLoc, Color.White);
            //spriteBatch.DrawString(FontManager.GetInstance()["consolas"], "Credits", new Vector2(creditsLoc.X, creditsLoc.Y), Color.Black);

            //spriteBatch.Draw(TextureManager.GetInstance()["HighlightTexture"], playLoc, Color.White);
            spriteBatch.Draw(TextureManager.GetInstance()["Play"], playLoc, Color.White);
            //spriteBatch.DrawString(FontManager.GetInstance()["consolas"], "Play Game", new Vector2(playLoc.X, playLoc.Y), Color.Black);

            
            //spriteBatch.DrawString(FontManager.GetInstance()["consolas"], "Main Menu!\nPress Spacebar to begin playing!\nPress C to visit the credits page!", Vector2.Zero, Color.Black);
            spriteBatch.Draw(TextureManager.GetInstance()["CursorTexture"], new Vector2(mouseState.X, mouseState.Y), Color.White);
           
        }
    }
}
