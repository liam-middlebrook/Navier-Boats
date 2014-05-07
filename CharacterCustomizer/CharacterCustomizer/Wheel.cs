using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//additional using statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Windows.Forms;

namespace CharacterCustomizer
{
    class Wheel : CustomizeElement
    {
        protected Texture2D rightButton, leftButton, display, currOption;
        protected Rectangle rBSize, lBSize, dispSize, currOptSize;
        protected int currIndex = 0;
        protected List<Texture2D> options;
        protected List<string> fileNames;
        protected MiscButton colorButton;
        protected Color color;

        //const string ImageLoc = "../../../../CharacterCustomizerContent/";

        //Displacements for each wheel item's x position
        protected int leftDisp,rightDisp,displayDisp,optionDisp;

        protected bool rBClicked = false, lBClicked = false;

        /// <summary>
        /// get
        /// returns the current texture on the wheel
        /// </summary>
        public Texture2D Current
        {
            get { return options[currIndex]; }
        }

        /// <summary>
        /// get
        /// </summary>
        public Color Color
        {
            get { return color; }
        }

        public Wheel(int s, string dir, ContentManager content, int x, int y) : base(s)
        {
            leftDisp = 0;
            displayDisp = leftDisp + 3;
            rightDisp = displayDisp + 3;
            optionDisp = displayDisp;
            int colorDisp = rightDisp + 3;

            options = new List<Texture2D>();
            fileNames = new List<string>();
            color = Color.White;
            int extraDisp = 0;
            LoadTextures(dir, content);
            Texture2D colorTexture = content.Load<Texture2D>("Buttons/Color");

            leftButton = content.Load<Texture2D>("Buttons/Left");
            leftDisp = ConvertPixelsToScale(leftDisp) + extraDisp;
            lBSize = new Rectangle(x + leftDisp, y, leftButton.Width * Scale, leftButton.Height * Scale);

            extraDisp += lBSize.Width;

            display = content.Load<Texture2D>("Buttons/Display");
            displayDisp = ConvertPixelsToScale(displayDisp) + extraDisp;
            dispSize = new Rectangle(x + displayDisp, y - ConvertPixelsToScale(leftButton.Height / 4), display.Width * Scale, display.Height * Scale);
            
            currOption = options[0];
            optionDisp = ConvertPixelsToScale(optionDisp) + extraDisp + dispSize.Width / 2 - currOption.Width * Scale / 4 / 2;
            currOptSize = new Rectangle(x + optionDisp, dispSize.Y + dispSize.Height / 2 - currOption.Height * Scale / 4 / 2, currOption.Width * Scale / 4, currOption.Height * Scale / 4);

            extraDisp += dispSize.Width;

            rightButton = content.Load<Texture2D>("Buttons/Right");
            rightDisp = ConvertPixelsToScale(rightDisp) + extraDisp;
            rBSize = new Rectangle(x + rightDisp, y, rightButton.Width * Scale, rightButton.Height * Scale);

            extraDisp += rBSize.Width;

            colorButton = new MiscButton(x + ConvertPixelsToScale(colorDisp) + extraDisp, y, colorTexture, s / 2);
        }

        /// <summary>
        /// Loads all the textures in the directory dir within the standard content hierarchy and stores them in options
        /// </summary>
        /// <param name="dir">The folder the specified textures are stored in</param>
        /// <param name="content">The content manager, passed in from Game1</param>
        public void LoadTextures(string dir, ContentManager content)
        {
            DirectoryInfo di = new DirectoryInfo(content.RootDirectory + "/" + dir);
            if (!di.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = di.GetFiles("*.*");

            foreach (FileInfo fi in files)
            {
                string file = Path.GetFileNameWithoutExtension(fi.Name);
                options.Add(content.Load<Texture2D>(dir + "/" + file));
                fileNames.Add(file);
            }
        }

        /// <summary>
        /// Checks if the mouse has clicked either button, calls the appropriate cycle if necessary
        /// </summary>
        /// <param name="mouseX">The current mouse x</param>
        /// <param name="mouseY">The current mouse y</param>
        public void ButtonClick(int mouseX, int mouseY)
        {
            if (rBSize.Contains(mouseX, mouseY))
            {
                CycleOption(true);
                rBClicked = true;
            }
            else if (lBSize.Contains(mouseX, mouseY))
            {
                CycleOption(false);
                lBClicked = true;
            }
            else
            {
                colorButton.ButtonClick(mouseX, mouseY);
                if (colorButton.Clicked)
                {
                    ColorDialog cd = new ColorDialog();
                    cd.Color = System.Drawing.Color.FromArgb(color.A,color.R,color.G,color.B);
                    cd.AllowFullOpen = true;
                    cd.FullOpen = true;

                    if (cd.ShowDialog() == DialogResult.OK)
                        color = new Color(cd.Color.R,cd.Color.G,cd.Color.B,cd.Color.A);

                    if (cd != null)
                        cd.Dispose();
                }
            }
        }

        /// <summary>
        /// "Unclicks" the buttons i.e. changes their clicked variable.
        /// </summary>
        public void ButtonUnClick()
        {
            if(rBClicked)
                rBClicked = false;
            if(lBClicked)
                lBClicked = false;
            colorButton.ButtonUnClick();
        }

        /// <summary>
        /// Cycles left or right through the list, changes the current index and option shown accordingly
        /// </summary>
        /// <param name="dir">The direction to travel in, true is right, false is left</param>
        public void CycleOption(bool dir)
        {
            currIndex += (dir) ? 1 : -1;
            if (dir && currIndex >= options.Count) //if going right and beyond the right bound of the list
                currIndex = 0; //go back to the beginning
            else if (!dir && currIndex < 0) //if going left and beyond the left bound of the list
                currIndex = options.Count - 1; //go to the end
            currOption = options[currIndex];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(leftButton, lBSize, lBClicked ? Color.CadetBlue : Color.White);
            spriteBatch.Draw(rightButton, rBSize, rBClicked ? Color.CadetBlue : Color.White);
            spriteBatch.Draw(display, dispSize, Color.White);
            spriteBatch.Draw(currOption, currOptSize, color);
            colorButton.Draw(spriteBatch);
        }

        public string Save()
        {
            return fileNames[currIndex] + "\n" + color.R + "," + color.G + "," + color.B + "," + color.A;
        }

        public void Load(string saveData)
        {
            try
            {
                string[] data = saveData.Split('\n');
                string fileName = data[0];
                currIndex = fileNames.IndexOf(fileName);
                currOption = options[currIndex];

                string[] rgba = data[1].Split(',');
                color = new Color(int.Parse(rgba[0]), int.Parse(rgba[1]), int.Parse(rgba[2]), int.Parse(rgba[3]));
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Feature Does not exist");
            }
        }
    }
}
