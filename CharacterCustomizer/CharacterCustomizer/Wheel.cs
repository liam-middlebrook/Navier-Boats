using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace CharacterCustomizer
{
    class Wheel
    {
        Texture2D rightButton, leftButton, display, currOption;
        Rectangle rBSize, lBSize, dispSize, currOptSize;
        int currIndex = 0;
        List<Texture2D> options;

        //const string ImageLoc = "../../../../CharacterCustomizerContent/";

        //Displacements for each wheel item's x position
        const int Scale = 8;
        const int LeftDisp = 0;
        const int DisplayDisp = LeftDisp + 25 * Scale / 2;
        const int RightDisp = DisplayDisp + 25 * Scale / 2;
        const int OptionDisp = (int)(DisplayDisp + Scale * 1.85);

        public Wheel(string dir, ContentManager content, int x, int y)
        {
            options = new List<Texture2D>();
            rightButton = content.Load<Texture2D>("Buttons/Right");
            rBSize = new Rectangle(x + RightDisp, y, rightButton.Width * Scale, rightButton.Height * Scale);
            leftButton = content.Load<Texture2D>("Buttons/Left");
            lBSize = new Rectangle(x + LeftDisp, y, leftButton.Width * Scale, leftButton.Height * Scale);
            display = content.Load<Texture2D>("Buttons/Display");
            dispSize = new Rectangle(x + DisplayDisp, y - leftButton.Height * Scale / 6, display.Width * Scale, display.Height * Scale);
            LoadTextures(dir, content);
            currOption = options[0];
            currOptSize = new Rectangle(x + OptionDisp, y - leftButton.Height * Scale / 6 + Scale * 2, currOption.Width * Scale / 4, currOption.Height * Scale / 4);
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
                CycleOption(true);
            else if (lBSize.Contains(mouseX, mouseY))
                CycleOption(false);
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
            spriteBatch.Draw(leftButton, lBSize, Color.White);
            spriteBatch.Draw(rightButton, rBSize, Color.White);
            spriteBatch.Draw(display, dispSize, Color.White);
            spriteBatch.Draw(currOption, currOptSize, Color.White);
        }
    }
}
