using Microsoft.Xna.Framework;
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
    public class MainMenu : GameState
    {
        KeyboardState keyState;
        KeyboardState prevKeyState;

        protected override void Init()
        {
            keyState = Keyboard.GetState();

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {

        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
            {
                StateManager.GetInstance().PushState(GameStates.GAMEPLAY);
            }
            if (keyState.IsKeyDown(Keys.C) && prevKeyState.IsKeyUp(Keys.C))
            {
                StateManager.GetInstance().PushState(GameStates.CREDITS);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }

        public override void DrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetInstance()["consolas"], "Main Menu!\nPress Spacebar to begin playing!\nPress C to visit the credits page!", Vector2.Zero, Color.Black);
        }
    }
}
