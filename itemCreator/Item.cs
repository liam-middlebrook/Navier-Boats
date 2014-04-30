using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WindowsFormsApplication1
{
    [JsonObject(MemberSerialization.OptIn)]
    class Item
    {
        string itemName;
        string imgName;
        string type;
        string inventoryImgName;
        string descripton;
        int maxStackValue;
        int cost;
        string locationFolder;
        double damage;
        double range;
        double heal;

        public string Folder
        {
            get { return locationFolder; }
            set { locationFolder = value; }
        }

        [JsonProperty]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        [JsonProperty]
        public string Name
        {
            get{return itemName;}
            set{itemName = value;}
        }
        [JsonProperty]
        public string Image
        {
            get { return imgName; }
            set { imgName = value; }
        }
        [JsonProperty]
        public string inventoryImage
        {
            get { return inventoryImgName; }
            set { inventoryImgName = value; }
        }
        [JsonProperty]
        public string Description
        {
            get { return descripton; }
            set { descripton = value; }
        }
        [JsonProperty]
        public int Stack
        {
            get { return maxStackValue; }
            set { maxStackValue = value; }
        }
        [JsonProperty]
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        [JsonProperty]
        public double Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        [JsonProperty]
        public double Heal
        {
            get { return heal; }
            set { heal = value; }
        }

        [JsonProperty]
        public double Range
        {
            get { return range; }
            set { range = value; }
        }

        public Item()
        {
            locationFolder = null;
            cost = 0;
            maxStackValue = 64;
            range = 0;
            heal = 0;
            damage = 0;
        }

        public void Save()
        {
            StreamWriter infoDump = null;
            try
            {
                infoDump = new StreamWriter( Folder + "/" + itemName + ".itm");
                string output = JsonConvert.SerializeObject(this, Formatting.Indented);

                infoDump.WriteLine(output);

            }
            catch (Exception e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message);
            }
            finally
            {
                infoDump.Close();
            }
        }
    }
}
