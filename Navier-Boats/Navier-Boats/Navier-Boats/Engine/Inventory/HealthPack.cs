using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;

namespace Navier_Boats.Engine.Inventory
{
    [Serializable]
    class HealthPack : IGameItem
    {
        private Texture2D invTex;
        private Texture2D itemTex;
        private string name;
        private string description;
        private int stack;
        private int cost;
        private int heal;
        private string type;
        

        /// <summary>
        /// Texture in the inventory
        /// </summary>
        public Texture2D InventoryTexture
        {
            get { return invTex; }
            set { invTex = value; }
        }

        /// <summary>
        /// Texture when held
        /// </summary>
        public Texture2D ItemTexture
        {
            get { return itemTex; }
            set { itemTex = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int MaxStack
        {
            get { return stack; }
            set { stack = value; }
        }

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public void OnAction(LivingEntity executor)
        {
            executor.TakeDamage(-heal);
        }

        public string getItemType()
        {
            return type;
        }
    }
}
