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
//using System.IO;

namespace CharacterCustomizer
{
    class Die : CustomizeElement
    {
        List<Texture2D> rolls;
        Texture2D currRoll;
        Texture2D rollButton;
        Rectangle dieLoc;
        Rectangle rollButtonLoc;
        Random roller = new Random();
        int rollsLeft = 0;// this is just for the graphical rolling effect
        bool rollClicked = false;

        SpriteFont statText;
        List<string> statNames;
        List<int> stats;

        public SpriteFont StatText
        {
            set { statText = value; }
        }

        public Die(int s, int x, int y, ContentManager content) : base(s)
        {
            rolls = new List<Texture2D>();
            for (int i = 1; i < 7; i++)
                rolls.Add(content.Load<Texture2D>("Buttons/Die/" + i));
            currRoll = rolls[0];
            rollButton = content.Load<Texture2D>("Buttons/Die/Roll");

            statNames = new List<string>();
            stats = new List<int>();
            statNames.Add("Health");
            stats.Add(0);
            statNames.Add("Strength");
            stats.Add(0);
            statNames.Add("Speed");
            stats.Add(0);
            statNames.Add("Stamina");
            stats.Add(0);

            dieLoc = new Rectangle(x, y, currRoll.Width * Scale, currRoll.Height * Scale);
            rollButtonLoc = new Rectangle(x - ConvertPixelsToScale(1), y + dieLoc.Height + ConvertPixelsToScale(10 * stats.Count), rollButton.Width * Scale, rollButton.Height * Scale);
        }

        /// <summary>
        /// Uses the roller to simulate a dice roll, updates the current roll accordingly
        /// </summary>
        public void Roll()
        {
            int roll = roller.Next(1, 6);
            currRoll = rolls[roll];

            for (int i = 0; i < stats.Count; i++)
                stats[i] = roller.Next(10, 51);
        }

        public void ButtonClick(int mouseX, int mouseY)
        {
            if (rollButtonLoc.Contains(mouseX, mouseY))
            {
                rollsLeft = 5;
                Roll();
                rollClicked = true;
            }
        }

        /// <summary>
        /// "Unclicks" the buttons i.e. changes their clicked variable.
        /// </summary>
        public void ButtonUnClick()
        {
            rollClicked = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currRoll, dieLoc, Color.White);
            for (int i = 0; i < stats.Count; i++)
            {
                spriteBatch.DrawString(statText, statNames[i] + ":", new Vector2(dieLoc.X, dieLoc.Y + dieLoc.Height + ConvertPixelsToScale(10 * i)), Color.Blue);
                spriteBatch.DrawString(statText, stats[i] + "", new Vector2(dieLoc.X + ConvertPixelsToScale(30), dieLoc.Y + dieLoc.Height + ConvertPixelsToScale(10 * i)), Color.Blue);
            }
            spriteBatch.Draw(rollButton, rollButtonLoc, rollClicked ? Color.CadetBlue : Color.White);
            if (rollsLeft > 0)
            {
                rollsLeft--;
                Roll();
            }
        }
    }
}
