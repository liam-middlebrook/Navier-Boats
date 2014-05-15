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
using Navier_Boats.Engine.System;
using Navier_Boats.Engine.Graphics.PostProcessing;
using Navier_Boats.Game.Graphics;

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
                if (ItemManager.GetInstance().Items[i].getItemType() == "Items.Weapon")
                {
                    player.Items.AddItem(new ItemStack(ItemManager.GetInstance().Items[i], ItemManager.GetInstance().Items[i].MaxStack));
                    break;
                }
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
            spriteBatch.Begin(0, null, null, null, null, null, Camera.TransformMatrix);

            CurrentLevel.GetInstance().Draw(spriteBatch);

            spriteBatch.End();

            ShaderManager.GetInstance()[0].Matrix = Camera.TransformMatrix;
            ShaderManager.GetInstance().PostProcess(spriteBatch);

            spriteBatch.Begin(0, null, null, null, null, null, Camera.TransformMatrix);

            EntityManager.GetInstance().Draw(spriteBatch);

            TracerManager.GetInstance().Draw(spriteBatch);

            PathThreadPool.GetInstance().Draw(spriteBatch, TextureManager.GetInstance()["debugTextures/path"]);

            spriteBatch.End();
        }

        public override void DrawGUI(SpriteBatch spriteBatch)
        {
            CurrentLevel.GetInstance().DrawGUI(spriteBatch);
        }
    }
}