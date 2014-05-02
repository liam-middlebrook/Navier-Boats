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
    public class GameOver : GameState
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

            if (keyState.IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
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
            spriteBatch.DrawString(FontManager.GetInstance()["consolas"], "You died!\nUnfortunately Zombie Fiesta does not currently support multiple playthoughs after death.\n\nPress Space to return to MainMenu", Vector2.Zero, Color.Black);
        }
    }
}
