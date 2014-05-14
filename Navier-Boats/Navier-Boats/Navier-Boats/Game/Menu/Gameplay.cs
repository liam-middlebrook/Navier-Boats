﻿using Microsoft.Xna.Framework;
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
using Navier_Boats.Engine.System;
using Navier_Boats.Engine.Graphics.PostProcessing;

namespace Navier_Boats.Game.Menu
{
    class Gameplay : GameState
    {
        InputStateHelper inputHelper;

        protected override void Init()
        {
            inputHelper = new InputStateHelper();

            ItemManager.GetInstance();

            Player player = EntityManager.GetInstance().Player;
            for (int i = 0; i < ItemManager.GetInstance().Items.Count; i++)
            {
            player.Items.AddItem(new ItemStack(ItemManager.GetInstance().Items[i], ItemManager.GetInstance().Items[i].MaxStack));
            }
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            ShaderManager.GetInstance()[0] = new ShaderQuery(content.Load<Effect>("Shaders/Ripple")){ Matrix=Camera.TransformMatrix};
        }

        public override void Update(GameTime gameTime)
        {
            inputHelper.Update();

            ShaderManager.GetInstance()[0].Shader.Parameters["time"].SetValue(gameTime.TotalGameTime.Seconds);

            EntityManager.GetInstance().GameTime = gameTime;
            CurrentLevel.GetInstance().Update(gameTime, inputHelper);
            PathThreadPool.GetInstance().Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentLevel.GetInstance().Draw(spriteBatch);

            EntityManager.GetInstance().Draw(spriteBatch);

            PathThreadPool.GetInstance().Draw(spriteBatch, TextureManager.GetInstance()["debugTextures/path"]);
            ShaderManager.GetInstance()[0].Matrix = Camera.TransformMatrix;
        }

        public override void DrawGUI(SpriteBatch spriteBatch)
        {
            CurrentLevel.GetInstance().DrawGUI(spriteBatch);
        }
    }
}