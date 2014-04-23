using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;

namespace Navier_Boats.Engine.Inventory
{
    public class ItemManager
    {
        private static ItemManager instance = new ItemManager();

        private List<IGameItem> items = new List<IGameItem>();

        public static ItemManager GetInstance()
        {
            return instance;
        }

        private ItemManager()
        {
        }

        /*
        public void LoadItems()
        {
            //path searched for .itm files, for now
            string path = "./Content/Items";
            string [] files =  Directory.GetFiles(path, "*.itm");

            foreach (string file in files)
            {
                
                string output;

                StreamReader reader = new StreamReader(file);
                output = reader.ReadToEnd();
                
                // SEAN: no longer using item factories. just store the item itself.
                CustomGameItem item = JsonConvert.DeserializeObject<CustomGameItem>(output);
                items.Add(item);
            }
        }
         * */
    }
}
