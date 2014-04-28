using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Engine.Inventory
{
    public class ItemManager
    {
        private static ItemManager instance = new ItemManager();

        private List<IGameItem> items = new List<IGameItem>();

        private Dictionary<string, Type> baseItems = new Dictionary<string, Type>();

        private int totalCost;

        public static ItemManager GetInstance()
        {
            return instance;
        }

        private ItemManager()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IGameItem).IsAssignableFrom(type) && typeof(IGameItem) != type)
                    {
                        IGameItem item = (IGameItem)Activator.CreateInstance(type);
                        baseItems.Add(item.getItemType(), type);
                    }
                }
                
            }

            LoadItems();
        }

        
        private void LoadItems()
        {
            //path searched for .itm files, for now
            string path = "./Content/Items";
            if (!Directory.Exists(path))
            {
                return;
            }

            string [] files =  Directory.GetFiles(path, "*.itm");

            foreach (string file in files)
            {
                
                string output;

                StreamReader reader = new StreamReader(file);
                output = reader.ReadToEnd();

                ItemInfo info = JsonConvert.DeserializeObject<ItemInfo>(output);
                Type itemType = null;
                if (!baseItems.TryGetValue(info.Type, out itemType))
                {
                    throw new NotImplementedException("No item of type " + info.Type + " implemented");
                }

                IGameItem item = (IGameItem)Activator.CreateInstance(itemType);
                item.ImportItem(info);
                items.Add(item);
                totalCost += item.Cost;
            }
            
         
        }

        public IGameItem GetRandomItem()
        {
            int diceRoll = CurrentLevel.GetRandom().Next(0, totalCost);
            int index = 0;

            while (diceRoll > 0)
            {
                diceRoll -= items[index].Cost;
                index++;
            }
            return items[diceRoll];
        }
    }
}
