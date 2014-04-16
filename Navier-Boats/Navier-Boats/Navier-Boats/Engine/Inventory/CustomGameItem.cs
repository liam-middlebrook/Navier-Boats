using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;

namespace Navier_Boats.Engine.Inventory
{
    public class CustomGameItem : IGameItem
    {
        private Texture2D inventoryTexture = null;
        private Texture2D itemTexture = null;
        private int maxStack = 1;

        public Microsoft.Xna.Framework.Graphics.Texture2D InventoryTexture
        {
            get
            {
                return inventoryTexture;
            }
        }

        public Microsoft.Xna.Framework.Graphics.Texture2D ItemTexture
        {
            get
            {
                return itemTexture;
            }
        }

        public int MaxStack
        {
            get
            {
                return maxStack;
            }
        }

        public int Cost
        {
            get;
            set;
        }
    }
}
