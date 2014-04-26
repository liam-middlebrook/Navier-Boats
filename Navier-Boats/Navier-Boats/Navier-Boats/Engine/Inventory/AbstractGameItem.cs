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
    public class BaseGameItem : IGameItem
    {
        /// <summary>
        /// Texture in the inventory
        /// </summary>
        public Texture2D InventoryTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Texture when held
        /// </summary>
        public Texture2D ItemTexture
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int MaxStack
        {
            get;
            set;
        }

        public int Cost
        {
            get;
            set;
        }

        public BaseGameItem()
        {
        }

        public BaseGameItem(SerializationInfo info, StreamingContext context)
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

        public virtual void ImportItem(ItemInfo info)
        {
            this.InventoryTexture = TextureManager.GetInstance().LoadTexture(info.inventoryImage);
            this.ItemTexture = TextureManager.GetInstance().LoadTexture(info.Image);
            this.Description = info.Description;
            this.MaxStack = info.Stack;
            this.Cost = info.Cost;
        }
    }
}
