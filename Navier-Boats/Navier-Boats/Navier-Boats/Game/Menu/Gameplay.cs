using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Graphics;
using Navier_Boats.Engine.Inventory;
using Navier_Boats.Engine.Level;
using Navier_Boats.Engine.Menu;
using Navier_Boats.Engine.Pathfinding.Threading;
using Navier_Boats.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Game.Menu
{
    class Gameplay : GameState
    {
        KeyboardState keyState;
        KeyboardState prevKeyState;

        MouseState mouseState;
        MouseState prevMouseState;

        protected override void Init()
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            ItemManager.GetInstance();

            Player player = EntityManager.GetInstance().Player;
            for (int i = 0; i < ItemManager.GetInstance().Items.Count; i++)
            {
            player.Items.AddItem(new ItemStack(ItemManager.GetInstance().Items[i], ItemManager.GetInstance().Items[i].MaxStack));
            }
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            CurrentLevel.GetInstance().Update(gameTime, keyState, prevKeyState, mouseState, prevMouseState);
            PathThreadPool.GetInstance().Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentLevel.GetInstance().Draw(spriteBatch);

            PathThreadPool.GetInstance().Draw(spriteBatch, TextureManager.GetInstance()["debugTextures/path"]);
        }

        public override void DrawGUI(SpriteBatch spriteBatch)
        {
            CurrentLevel.GetInstance().DrawGUI(spriteBatch);
        }
    }
}