using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Inventory
{
    public class ItemManager
    {
        private static ItemManager instance = new ItemManager();

        public static ItemManager GetInstance()
        {
            return instance;
        }

        private ItemManager()
        {
        }

        public void RegisterFactory(IItemFactory factory)
        {
            throw new NotImplementedException();
        }

        public void LoadItems()
        {
        }
    }
}
