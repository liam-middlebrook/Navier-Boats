using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Menu
{
    public enum GameStates
    {
        MAIN_MENU,
        GAMEPLAY,
        CREDITS,
        GAME_OVER,
        INSTRUCTIONS,
    }
    public interface IGameState
    {
        void Initialize();

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

        void DrawGUI(SpriteBatch spriteBatch);
    }
}
