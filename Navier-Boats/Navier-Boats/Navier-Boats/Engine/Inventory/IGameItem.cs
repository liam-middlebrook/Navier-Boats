using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;

namespace Navier_Boats.Engine.Inventory
{
    public interface IGameItem
    {
        /// <summary>
        /// Texture in the inventory
        /// </summary>
        Texture2D InventoryTexture
        {
            get;
        }

        /// <summary>
        /// Texture when held
        /// </summary>
        Texture2D ItemTexture
        {
            get;
        }

        int MaxStack
        {
            get;
        }
    }
}
