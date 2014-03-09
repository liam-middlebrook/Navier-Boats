using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;

namespace Navier_Boats.Engine.Inventory
{
    public interface IGameItem : IInteractable
    {
        Texture2D InventoryTexture
        {
            get;
            set;
        }

        Texture2D ItemTexture
        {
            get;
            set;
        }

        int MaxStack
        {
            get;
            set;
        }
    }
}
