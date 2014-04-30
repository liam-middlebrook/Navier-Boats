using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;
using System.Runtime.Serialization;

namespace Navier_Boats.Engine.Inventory
{
    public interface IGameItem : ISerializable
    {
        /// <summary>
        /// Texture in the inventory
        /// </summary>
        Texture2D InventoryTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Texture when held
        /// </summary>
        Texture2D ItemTexture
        {
            get;
            set;
        }

        string Description
        {
            get;
            set;
        }

        int MaxStack
        {
            get;
            set;
        }

        int Cost
        {
            get;
            set;
        }

        void OnAction(LivingEntity executor);

        string getItemType();

        void ImportItem(ItemInfo info);
    }
}
