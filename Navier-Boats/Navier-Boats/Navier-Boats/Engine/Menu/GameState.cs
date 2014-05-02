using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Menu
{
    public abstract class GameState : IGameState
    {
        private bool initialized;
        
        public void Initialize()
        {
            if (!initialized)
            {
                Init();
            }
            initialized = true;
        }

        protected abstract void Init();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void DrawGUI(SpriteBatch spriteBatch);
    }
}
