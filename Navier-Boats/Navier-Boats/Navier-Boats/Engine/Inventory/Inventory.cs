using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Navier_Boats.Engine.Inventory
{
    [Serializable]
    public class Inventory : ISerializable
    {
        private int selectedItemIndex = 0;

        public ItemStack[] Items
        {
            get;
            protected set;
        }

        public int SelectedItemIndex
        {
            get
            {
                return selectedItemIndex;
            }

            set
            {
                if (value < 0 || value > this.Items.Length)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Tried to set selected item out of range ({0})", value));
                }

                selectedItemIndex = value;
            }
        }

        public ItemStack SelectedItem
        {
            get
            {
                return this.Items[SelectedItemIndex];
            }
        }

        public Inventory(int maxSize)
        {
            this.Items = new ItemStack[maxSize];
        }

        public Inventory(SerializationInfo info, StreamingContext context)
        {
            this.Items = (ItemStack[])info.GetValue("items", typeof(ItemStack[]));
            this.selectedItemIndex = info.GetInt32("selectedItemIndex");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("items", Items);
            info.AddValue("selectedItemIndex", selectedItemIndex);
        }

        public void AddItem(ItemStack item)
        {
            for(int i = 0; i < this.Items.Length; i++)
            {
                if (this.Items[i] == null || this.Items[i].Item == null)
                {
                    this.Items[i] = item;
                    return;
                }
            }

            throw new InventoryOutOfSpaceException("Trying to Inventory.AddItem to a full inventory");
        }

        /// <summary>
        /// Remove a single item (decrement the first stack found, removing if <= 0)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool RemoveItem<T>() where T : IGameItem
        {
            return RemoveItem(typeof(T));
        }

        /// <summary>
        /// Remove a single item (decrement the first stack found, removing if <= 0)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool RemoveItem(Type t)
        {
            int i = Find(t);
            if (i < 0)
                return false;

            this.Items[i].Amount--;
            if (this.Items[i].Amount <= 0)
                this.Items[i] = null;

            return true;
        }

        public void RemoveAll<T>() where T : IGameItem
        {
            RemoveAll(typeof(T));
        }

        public void RemoveAll(Type t)
        {
            List<int> items = FindAll(t);
            foreach (int i in items)
            {
                this.Items[i] = null;
            }
        }

        public bool RemoveStack(ItemStack item)
        {
            int i = Find(item);
            if (i < 0)
                return false;

            this.Items[i] = null;
            return true;
        }

        public bool RemoveStack<T>() where T : IGameItem
        {
            return RemoveStack(typeof(T));
        }

        public bool RemoveStack(Type t)
        {
            int i = Find(t);
            if (i < 0)
                return false;

            this.Items[i] = null;
            return true;
        }

        /// <summary>
        /// Find an itemstack. This is not a pure equality operation (it does not compare references). Instead,
        /// it checks the item type and amount.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The index of the itemstack, or -1 if not found.</returns>
        public int Find(ItemStack item)
        {
            for (int i = 0; i < this.Items.Length; i++)
            {
                ItemStack stack = this.Items[i];
                if (stack == null || stack.Item == null)
                {
                    if (item == null || item.Item == null)
                        return i;
                    continue;
                }

                if (item.Amount == stack.Amount && item.Item.GetType().IsAssignableFrom(stack.Item.GetType()))
                    return i;
            }

            return -1;
        }

        public int Find<T>() where T : IGameItem
        {
            return Find(typeof(T));
        }

        public int Find(Type t)
        {
            for (int i = 0; i < this.Items.Length; i++ )
            {
                ItemStack stack = this.Items[i];
                if (t.IsAssignableFrom(stack.Item.GetType()))
                    return i;
            }

            return -1;
        }

        public List<int> FindAll(ItemStack item)
        {
            List<int> found = new List<int>();
            for (int i = 0; i < this.Items.Length; i++)
            {
                ItemStack stack = this.Items[i];
                if (stack == null || stack.Item == null)
                {
                    if (item == null || item.Item == null)
                        found.Add(i);
                    continue;
                }

                if (item.Amount == stack.Amount && item.Item.GetType().IsAssignableFrom(stack.Item.GetType()))
                    found.Add(i);
            }

            return found;
        }

        public List<int> FindAll<T>() where T : IGameItem
        {
            return FindAll(typeof(T));
        }

        public List<int> FindAll(Type t)
        {
            List<int> found = new List<int>();
            for (int i = 0; i < this.Items.Length; i++)
            {
                ItemStack stack = this.Items[i];
                if (stack != null && t.IsAssignableFrom(stack.Item.GetType()))
                    found.Add(i);
            }

            return found;
        }
    }
}
