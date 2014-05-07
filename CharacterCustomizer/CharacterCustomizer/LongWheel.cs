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

        public LongWheel(int s, string dir, ContentManager content, int x, int y, double width, double height, double contentScale)
            : base(s, dir, content, x, y)
        {
            dispSize = new Rectangle(x + displayDisp, y - ConvertPixelsToScale(leftButton.Height / 4), (int)(display.Width * Scale * width), (int)(display.Height * Scale * height));

            optionDisp = (int)(displayDisp + dispSize.Width / 2 - currOption.Width * Scale / 4 * contentScale / 2);
            currOptSize = new Rectangle(x + optionDisp, (int)(y + (dispSize.Height - currOption.Height * Scale / 3 * contentScale) / 2), (int)(currOption.Width * Scale / 4 * contentScale), (int)(currOption.Height * Scale / 4 * contentScale));

            rightDisp += dispSize.Width - displayDisp + ConvertPixelsToScale(3);
            rBSize = new Rectangle(x + rightDisp, y, rightButton.Width * Scale, rightButton.Height * Scale);

            int colorDisp = rightDisp + rBSize.Width + ConvertPixelsToScale(3);
            colorButton = new MiscButton(x + colorDisp, y, colorButton.Button, s / 2);
        }
    }
}
