using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Game.Inventory;

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
            string path = "./Content/Items";
            string [] files =  Directory.GetFiles(path, "*.itm");

            foreach (string file in files)
            {
                
                string output;

                StreamReader reader = new StreamReader(file);
                output = reader.ReadToEnd();
                
                CustomGameItem temp = JsonConvert.DeserializeObject<CustomGameItem>(output);
                if (temp.Type == "Weapon")
                {
                    WeaponFactory Factory = new WeaponFactory();
                    Factory.CreateItem();
                }
            }
        }
    }
}
