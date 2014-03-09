using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Inventory
{
    public class ItemStack
    {
        private int amount = 1;

        public IGameItem Item
        {
            get;
            set;
        }

        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Stack size must be >= 0 (got {0})", value));
                }

                if (this.Item != null && value > this.Item.MaxStack)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Maximum stack size for item {0} is {1} (tried to use stack of {2})", this.Item.GetType().Name, Item.MaxStack, value));
                }

                this.amount = value;
            }
        }

        public ItemStack()
        {
            this.Amount = 0;
        }

        public ItemStack(IGameItem item, int amount = 1)
        {
            this.Item = item;
            this.Amount = amount;
        }
    }
}
