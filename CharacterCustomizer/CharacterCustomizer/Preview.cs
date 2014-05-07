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

namespace CharacterCustomizer
{
    class Preview
    {
        List<Texture2D> character;
        List<Color> colors;
        Texture2D blank;
        Rectangle paneSize, shadowSize;
        MiscButton okButton;

        /// <summary>
        /// get
        /// </summary>
        public bool Clicked
        {
            get { return okButton.Clicked; }
        }

        public Preview(Texture2D face, Color faceColor, Texture2D body, Color bodyColor, Texture2D legs, Color legColor, int screenWidth, int screenHeight, ContentManager content)
        {
            character = new List<Texture2D>();
            character.Add(legs);
            character.Add(body);
            character.Add(face);
            //character.Add(hair);

            colors = new List<Color>();
            colors.Add(legColor);
            colors.Add(bodyColor);
            colors.Add(faceColor);
            //colors.Add(hairColor);

            blank = content.Load<Texture2D>("Blank");

            Texture2D okTexture = content.Load<Texture2D>("Buttons/Ok");
            int s = 4;
            
            paneSize = new Rectangle(screenWidth / 8, screenHeight / 8, screenWidth / 8 * 6, screenHeight / 8 * 6);
            shadowSize = new Rectangle(0, 0, screenWidth, screenHeight);
            okButton = new MiscButton((paneSize.X + paneSize.Width - 20 - okTexture.Width * s), (paneSize.Y + paneSize.Height - 20 - okTexture.Height * s), okTexture, s);
        }

        /// <summary>
        /// Passes the click data to the button
        /// </summary>
        /// <param name="mouseX">mouse x</param>
        /// <param name="mouseY"> mouse y</param>
        public void ButtonClick(int mouseX, int mouseY)
        {
            okButton.ButtonClick(mouseX, mouseY);
        }

        /// <summary>
        /// "Unclicks" the buttons i.e. changes their clicked variable.
        /// </summary>
        public void ButtonUnClick()
        {
            okButton.ButtonUnClick();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int w0 = (int)(character[0].Width * 2.75 * 1.25);
            int h0 = (int)(character[0].Height * 2.75 * 1.25);
            int w1 = (int)(character[1].Width * 2.25 * 1.25);
            int h1 = (int)(character[1].Height * 2.25 * 1.25);
            int w2 = (int)(character[2].Width * 1.55 * 1.25);
            int h2 = (int)(character[2].Height * 1.45 * 1.25);
            //int w3 = (int)(character[3].Width);
            //int h3 = (int)(character[3].Height);
            spriteBatch.Draw(blank, shadowSize, Color.Black * 0.5f);
            spriteBatch.Draw(blank, paneSize, Color.MediumPurple);
            int disp = paneSize.Height / 8;
            spriteBatch.Draw(character[0], new Rectangle(paneSize.X + paneSize.Width / 2 - w0 / 2, paneSize.Y + paneSize.Height - h0 - disp, w0, h0), colors[0]);
            disp -= 35;
            spriteBatch.Draw(character[1], new Rectangle(paneSize.X + paneSize.Width / 2 - w1 / 2, paneSize.Y + paneSize.Height - h0 - h1 - disp, w1, h1), colors[1]);
            disp -= 13;
            spriteBatch.Draw(character[2], new Rectangle(paneSize.X + paneSize.Width / 2 - w2 / 2, paneSize.Y + paneSize.Height - h0 - h1 - h2 - disp, w2, h2), colors[2]);
            //disp -= 3;
            //spriteBatch.Draw(character[3], new Rectangle(paneSize.X + paneSize.Width / 2 - w3 / 2, paneSize.Y + paneSize.Height - h0 - h1 - h2 - disp, w3, h3), colors[3]);

            okButton.Draw(spriteBatch);

        }
    }
}
