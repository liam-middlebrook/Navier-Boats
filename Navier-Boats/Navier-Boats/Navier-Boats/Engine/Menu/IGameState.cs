﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Navier_Boats.Engine.Menu
{
    public enum GameStates
    {
        MAIN_MENU,
        GAMEPLAY,
        CREDITS,
        GAME_OVER,
    }
    public interface IGameState
    {
        void Initialize();

        void LoadContent(ContentManager content);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

        void DrawGUI(SpriteBatch spriteBatch);
    }
}
