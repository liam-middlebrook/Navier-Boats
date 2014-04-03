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

namespace CharacterCustomizer
{
    class LongWheel : Wheel
    {
        //meant for longer parts of the character

        public LongWheel(int s, string dir, ContentManager content, int x, int y, double width, double height)
            : base(s, dir, content, x, y)
        {
            dispSize = new Rectangle(x + displayDisp, y - ConvertPixelsToScale(leftButton.Height / 4), (int)(display.Width * Scale * width), (int)(display.Height * Scale * height));

            optionDisp += (int)(display.Width * Scale * width / 30);
            currOptSize = new Rectangle(x + optionDisp, y + (int)(display.Height * Scale * height / 10), currOption.Width * Scale / 4, currOption.Height * Scale / 4);

            rightDisp += (int)(display.Width * Scale * width / 2);
            rBSize = new Rectangle(x + rightDisp, y, rightButton.Width * Scale, rightButton.Height * Scale);
        }
    }
}
