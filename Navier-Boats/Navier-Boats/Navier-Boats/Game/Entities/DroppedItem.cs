using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using libXNADeveloperConsole;
using Navier_Boats.Engine.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Inventory;
using Navier_Boats.Engine.System;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Game.Entities
{
    class DroppedItem : Entity
    {
        private ItemStack item;

        public ItemStack Item
        {
            get { return item; }
            set { item = value; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(item.Item.ItemTexture, this.Position, Color.White);
            base.Draw(spriteBatch);
        }

    }
}
