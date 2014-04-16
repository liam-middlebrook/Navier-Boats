using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void saveInfo_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = new DirectoryInfo(folderBrowserDialog1.SelectedPath).FullName;
            folderBrowserDialog1.ShowDialog();

            string folderLoc = folderBrowserDialog1.SelectedPath;

            

            saveInfo.Enabled = false;

            int tempStack;
            int tempCost;

            Item newItem = new Item();

            newItem.Folder = folderLoc;

            newItem.Name = itemNameBox.Text;
            newItem.Image = textureNameBox.Text;
            newItem.inventoryImage = inventoryTextureNameBox.Text;
            newItem.Description = itemDescriptionBox.Text;

            int.TryParse(maxStackBox.Text, out tempStack);
            newItem.Stack = tempStack;

            int.TryParse(itemCostBox.Text, out tempCost);
            newItem.Cost = tempCost;

            newItem.Type = DomainUpDown1.Text;


            newItem.Save();

            this.Close();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }


    }
}
