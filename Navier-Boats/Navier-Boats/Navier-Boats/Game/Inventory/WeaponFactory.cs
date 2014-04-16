using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Inventory;

namespace Navier_Boats.Game.Inventory
{
    public class WeaponFactory : IItemFactory
    {
        public float Damage
        {
            get;
            set;
        }

        public float Range
        {
            get;
            set;
        }

        public Texture2D InventoryTexture
        {
            get;
            set;
        }

        public Texture2D WorldTexture
        {
            get;
            set;
        }

        public IGameItem CreateItem()
        {
            throw new NotImplementedException();
        }
    }
}
