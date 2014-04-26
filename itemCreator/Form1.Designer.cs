namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.itemDescriptionBox = new System.Windows.Forms.TextBox();
            this.saveInfo = new System.Windows.Forms.Button();
            this.itemCostBox = new System.Windows.Forms.TextBox();
            this.maxStackBox = new System.Windows.Forms.TextBox();
            this.inventoryTextureNameBox = new System.Windows.Forms.TextBox();
            this.textureNameBox = new System.Windows.Forms.TextBox();
            this.itemNameBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.HealBox = new System.Windows.Forms.TextBox();
            this.DmgBox = new System.Windows.Forms.TextBox();
            this.RangeBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(28, 157);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(68, 13);
            this.Label7.TabIndex = 33;
            this.Label7.Text = "Heal Amount";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(208, 223);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(60, 13);
            this.Label6.TabIndex = 32;
            this.Label6.Text = "Description";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(210, 172);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(58, 13);
            this.Label5.TabIndex = 31;
            this.Label5.Text = "Max Stack";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(295, 172);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(28, 13);
            this.Label4.TabIndex = 30;
            this.Label4.Text = "Cost";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(103, -15);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(58, 13);
            this.Label3.TabIndex = 29;
            this.Label3.Text = "Item Name";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(90, 101);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(121, 13);
            this.Label2.TabIndex = 28;
            this.Label2.Text = "Inventory Texture Name";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(137, 57);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(74, 13);
            this.Label1.TabIndex = 27;
            this.Label1.Text = "Texture Name";
            // 
            // itemDescriptionBox
            // 
            this.itemDescriptionBox.Location = new System.Drawing.Point(142, 239);
            this.itemDescriptionBox.Multiline = true;
            this.itemDescriptionBox.Name = "itemDescriptionBox";
            this.itemDescriptionBox.Size = new System.Drawing.Size(185, 66);
            this.itemDescriptionBox.TabIndex = 23;
            this.itemDescriptionBox.Text = "N/A";
            // 
            // saveInfo
            // 
            this.saveInfo.Location = new System.Drawing.Point(14, 332);
            this.saveInfo.Name = "saveInfo";
            this.saveInfo.Size = new System.Drawing.Size(309, 40);
            this.saveInfo.TabIndex = 22;
            this.saveInfo.Text = "Save Item";
            this.saveInfo.UseVisualStyleBackColor = true;
            this.saveInfo.Click += new System.EventHandler(this.saveInfo_Click);
            // 
            // itemCostBox
            // 
            this.itemCostBox.Location = new System.Drawing.Point(284, 188);
            this.itemCostBox.Name = "itemCostBox";
            this.itemCostBox.Size = new System.Drawing.Size(43, 20);
            this.itemCostBox.TabIndex = 21;
            this.itemCostBox.Text = "0";
            // 
            // maxStackBox
            // 
            this.maxStackBox.Location = new System.Drawing.Point(217, 188);
            this.maxStackBox.Name = "maxStackBox";
            this.maxStackBox.Size = new System.Drawing.Size(43, 20);
            this.maxStackBox.TabIndex = 20;
            this.maxStackBox.Text = "64";
            // 
            // inventoryTextureNameBox
            // 
            this.inventoryTextureNameBox.Location = new System.Drawing.Point(219, 98);
            this.inventoryTextureNameBox.Name = "inventoryTextureNameBox";
            this.inventoryTextureNameBox.Size = new System.Drawing.Size(108, 20);
            this.inventoryTextureNameBox.TabIndex = 19;
            this.inventoryTextureNameBox.Text = "otherImageName";
            // 
            // textureNameBox
            // 
            this.textureNameBox.Location = new System.Drawing.Point(221, 54);
            this.textureNameBox.Name = "textureNameBox";
            this.textureNameBox.Size = new System.Drawing.Size(106, 20);
            this.textureNameBox.TabIndex = 18;
            this.textureNameBox.Text = "imageName";
            // 
            // itemNameBox
            // 
            this.itemNameBox.Location = new System.Drawing.Point(221, 20);
            this.itemNameBox.Name = "itemNameBox";
            this.itemNameBox.Size = new System.Drawing.Size(108, 20);
            this.itemNameBox.TabIndex = 35;
            this.itemNameBox.Text = "GenericItem";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(141, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Item Name";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "./../../../Navier-Boats/Navier-Boats/Navier-Boats/bin/x86/Debug/Content/Items";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(141, 140);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Item Type";
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteCustomSource.AddRange(new string[] {
            "Items.Weapon",
            "Items.HealthPack"});
            this.comboBox1.Items.AddRange(new object[] {
            "Weapon",
            "Health Pack",
            "Type Three",
            "Type Four"});
            this.comboBox1.Location = new System.Drawing.Point(206, 137);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 38;
            this.comboBox1.Text = "Items.Weapon";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(39, 233);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 39;
            this.label10.Text = "Damage";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(40, 279);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 13);
            this.label11.TabIndex = 40;
            this.label11.Text = "Range";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 211);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 13);
            this.label12.TabIndex = 41;
            this.label12.Text = "Weapon Attributes";
            // 
            // HealBox
            // 
            this.HealBox.Location = new System.Drawing.Point(25, 172);
            this.HealBox.Name = "HealBox";
            this.HealBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.HealBox.Size = new System.Drawing.Size(71, 20);
            this.HealBox.TabIndex = 42;
            // 
            // DmgBox
            // 
            this.DmgBox.Location = new System.Drawing.Point(25, 249);
            this.DmgBox.Name = "DmgBox";
            this.DmgBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DmgBox.Size = new System.Drawing.Size(71, 20);
            this.DmgBox.TabIndex = 43;
            // 
            // RangeBox
            // 
            this.RangeBox.Location = new System.Drawing.Point(25, 295);
            this.RangeBox.Name = "RangeBox";
            this.RangeBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RangeBox.Size = new System.Drawing.Size(71, 20);
            this.RangeBox.TabIndex = 44;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 384);
            this.Controls.Add(this.RangeBox);
            this.Controls.Add(this.DmgBox);
            this.Controls.Add(this.HealBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.itemDescriptionBox);
            this.Controls.Add(this.saveInfo);
            this.Controls.Add(this.itemCostBox);
            this.Controls.Add(this.maxStackBox);
            this.Controls.Add(this.inventoryTextureNameBox);
            this.Controls.Add(this.textureNameBox);
            this.Controls.Add(this.itemNameBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox itemDescriptionBox;
        internal System.Windows.Forms.Button saveInfo;
        internal System.Windows.Forms.TextBox itemCostBox;
        internal System.Windows.Forms.TextBox maxStackBox;
        internal System.Windows.Forms.TextBox inventoryTextureNameBox;
        internal System.Windows.Forms.TextBox textureNameBox;
        internal System.Windows.Forms.TextBox itemNameBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        internal System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox1;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox HealBox;
        private System.Windows.Forms.TextBox DmgBox;
        private System.Windows.Forms.TextBox RangeBox;
    }
}

