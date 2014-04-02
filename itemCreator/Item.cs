using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WindowsFormsApplication1
{
    class Item
    {
        string itemName;
        string imgName;
        string inventoryImgName;
        string descripton;
        int maxStackValue;
        double cost;

        public string Name
        {
            get{return itemName;}
            set{itemName = value;}
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

        public int Stack
        {
            get { return maxStackValue; }
            set { maxStackValue = value; }
        }

        public double Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public Item()
        {

        }

        public void Save()
        {
            StreamWriter infoDump = null;
            try
            {
                infoDump = new StreamWriter( itemName + ".itm");
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
