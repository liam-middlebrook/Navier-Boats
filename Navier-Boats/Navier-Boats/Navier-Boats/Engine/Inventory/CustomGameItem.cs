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
        private string itemName;
        private string imgName;
        private string inventoryImgName;
        private string descripton;
        private int maxStackValue;
        private int cost;
        private string type;

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

        public string Name
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public string Image
        {
            get { return imgName; }
            set { imgName = value; }
        }
        public string inventoryImage
        {
            get { return inventoryImgName; }
            set { inventoryImgName = value; }
        }
        public string Description
        {
            get { return descripton; }
            set { descripton = value; }
        }
        public int MaxStack
        {
            get { return maxStackValue; }
            set { maxStackValue = value; }
        }
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        public string Type
        {
            get { return type;}
        }
    }
}
