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
            this.NumericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.itemDescriptionBox = new System.Windows.Forms.TextBox();
            this.saveInfo = new System.Windows.Forms.Button();
            this.itemCostBox = new System.Windows.Forms.TextBox();
            this.maxStackBox = new System.Windows.Forms.TextBox();
            this.inventoryTextureNameBox = new System.Windows.Forms.TextBox();
            this.textureNameBox = new System.Windows.Forms.TextBox();
            this.itemNameBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(9, 174);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(97, 13);
            this.Label7.TabIndex = 33;
            this.Label7.Text = "Other input option?";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(155, 180);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(60, 13);
            this.Label6.TabIndex = 32;
            this.Label6.Text = "Description";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(157, 129);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(58, 13);
            this.Label5.TabIndex = 31;
            this.Label5.Text = "Max Stack";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(235, 129);
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
            this.Label2.Location = new System.Drawing.Point(39, 101);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(121, 13);
            this.Label2.TabIndex = 28;
            this.Label2.Text = "Inventory Texture Name";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(86, 57);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(74, 13);
            this.Label1.TabIndex = 27;
            this.Label1.Text = "Texture Name";
            // 
            // NumericUpDown3
            // 
            this.NumericUpDown3.Location = new System.Drawing.Point(12, 242);
            this.NumericUpDown3.Name = "NumericUpDown3";
            this.NumericUpDown3.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown3.TabIndex = 26;
            // 
            // NumericUpDown2
            // 
            this.NumericUpDown2.Location = new System.Drawing.Point(12, 190);
            this.NumericUpDown2.Name = "NumericUpDown2";
            this.NumericUpDown2.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown2.TabIndex = 25;
            // 
            // NumericUpDown1
            // 
            this.NumericUpDown1.Location = new System.Drawing.Point(12, 216);
            this.NumericUpDown1.Name = "NumericUpDown1";
            this.NumericUpDown1.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown1.TabIndex = 24;
            // 
            // itemDescriptionBox
            // 
            this.itemDescriptionBox.Location = new System.Drawing.Point(89, 196);
            this.itemDescriptionBox.Multiline = true;
            this.itemDescriptionBox.Name = "itemDescriptionBox";
            this.itemDescriptionBox.Size = new System.Drawing.Size(185, 66);
            this.itemDescriptionBox.TabIndex = 23;
            this.itemDescriptionBox.Text = "N/A";
            // 
            // saveInfo
            // 
            this.saveInfo.Location = new System.Drawing.Point(12, 289);
            this.saveInfo.Name = "saveInfo";
            this.saveInfo.Size = new System.Drawing.Size(260, 23);
            this.saveInfo.TabIndex = 22;
            this.saveInfo.Text = "Save Item";
            this.saveInfo.UseVisualStyleBackColor = true;
            this.saveInfo.Click += new System.EventHandler(this.saveInfo_Click);
            // 
            // itemCostBox
            // 
            this.itemCostBox.Location = new System.Drawing.Point(229, 145);
            this.itemCostBox.Name = "itemCostBox";
            this.itemCostBox.Size = new System.Drawing.Size(43, 20);
            this.itemCostBox.TabIndex = 21;
            this.itemCostBox.Text = "0";
            // 
            // maxStackBox
            // 
            this.maxStackBox.Location = new System.Drawing.Point(164, 145);
            this.maxStackBox.Name = "maxStackBox";
            this.maxStackBox.Size = new System.Drawing.Size(43, 20);
            this.maxStackBox.TabIndex = 20;
            this.maxStackBox.Text = "64";
            // 
            // inventoryTextureNameBox
            // 
            this.inventoryTextureNameBox.Location = new System.Drawing.Point(166, 98);
            this.inventoryTextureNameBox.Name = "inventoryTextureNameBox";
            this.inventoryTextureNameBox.Size = new System.Drawing.Size(108, 20);
            this.inventoryTextureNameBox.TabIndex = 19;
            this.inventoryTextureNameBox.Text = "otherImageName";
            // 
            // textureNameBox
            // 
            this.textureNameBox.Location = new System.Drawing.Point(166, 54);
            this.textureNameBox.Name = "textureNameBox";
            this.textureNameBox.Size = new System.Drawing.Size(106, 20);
            this.textureNameBox.TabIndex = 18;
            this.textureNameBox.Text = "imageName";
            // 
            // itemNameBox
            // 
            this.itemNameBox.Location = new System.Drawing.Point(166, 20);
            this.itemNameBox.Name = "itemNameBox";
            this.itemNameBox.Size = new System.Drawing.Size(108, 20);
            this.itemNameBox.TabIndex = 35;
            this.itemNameBox.Text = "GenericItem";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(86, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Item Name";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 328);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.NumericUpDown3);
            this.Controls.Add(this.NumericUpDown2);
            this.Controls.Add(this.NumericUpDown1);
            this.Controls.Add(this.itemDescriptionBox);
            this.Controls.Add(this.saveInfo);
            this.Controls.Add(this.itemCostBox);
            this.Controls.Add(this.maxStackBox);
            this.Controls.Add(this.inventoryTextureNameBox);
            this.Controls.Add(this.textureNameBox);
            this.Controls.Add(this.itemNameBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown1)).EndInit();
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
        internal System.Windows.Forms.NumericUpDown NumericUpDown3;
        internal System.Windows.Forms.NumericUpDown NumericUpDown2;
        internal System.Windows.Forms.NumericUpDown NumericUpDown1;
        internal System.Windows.Forms.TextBox itemDescriptionBox;
        internal System.Windows.Forms.Button saveInfo;
        internal System.Windows.Forms.TextBox itemCostBox;
        internal System.Windows.Forms.TextBox maxStackBox;
        internal System.Windows.Forms.TextBox inventoryTextureNameBox;
        internal System.Windows.Forms.TextBox textureNameBox;
        internal System.Windows.Forms.TextBox itemNameBox;
        private System.Windows.Forms.Label label8;
    }
}

