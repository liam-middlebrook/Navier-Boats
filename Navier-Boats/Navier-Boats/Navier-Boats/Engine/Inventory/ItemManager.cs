using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

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
            //path searched for .itm files, for now
            string path = "../../../../../../itemCreator/bin/debug";
            string [] files =  Directory.GetFiles(path, "*.itm");

            foreach (string file in files)
            {
                Console.WriteLine(file);
                string output;

                StreamReader reader = new StreamReader(file);
                output = reader.ReadToEnd();
                Console.WriteLine(output);
                CustomGameItem temp = JsonConvert.DeserializeObject<CustomGameItem>(output);
            }
        }
    }
}
