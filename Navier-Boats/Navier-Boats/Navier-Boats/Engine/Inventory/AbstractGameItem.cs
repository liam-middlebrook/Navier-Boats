using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Graphics;

namespace Navier_Boats.Engine.Inventory
{
    [Serializable]
    public class AbstractGameItem : IGameItem
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

        public AbstractGameItem(SerializationInfo info, StreamingContext context)
        {
            this.InventoryTexture = TextureManager.GetInstance().LoadTexture(info.GetString("inventoryTexture"));
            this.ItemTexture = TextureManager.GetInstance().LoadTexture(info.GetString("itemTexture"));
            this.Description = info.GetString("description");
            this.MaxStack = info.GetInt32("maxStack");
            this.Cost = info.GetInt32("cost");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("inventoryTexture", this.InventoryTexture.Name);
            info.AddValue("itemTexture", this.ItemTexture.Name);
            info.AddValue("description", Description);
            info.AddValue("maxStack", MaxStack);
            info.AddValue("cost", Cost);
        }
    }
}
