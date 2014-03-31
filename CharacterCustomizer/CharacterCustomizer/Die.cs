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
        Vector2 dieLoc;
        Rectangle rollButtonLoc;
        Random roller = new Random();
        int rollsLeft = 0;// this is just for the graphical rolling effect

        public Die(int s, int x, int y, ContentManager content) : base(s)
        {
            rolls = new List<Texture2D>();
            for (int i = 1; i < 7; i++)
                rolls.Add(content.Load<Texture2D>("Buttons/Die/" + i));
            currRoll = rolls[0];
            rollButton = content.Load<Texture2D>("Buttons/Die/Roll");

            dieLoc = new Vector2(x, y);
            rollButtonLoc = new Rectangle(x, y + 20, rollButton.Width, rollButton.Height);
        }

        /// <summary>
        /// Uses the roller to simulate a dice roll, updates the current roll accordingly
        /// </summary>
        public void Roll()
        {
            int roll = roller.Next(1, 6);
            currRoll = rolls[roll];
        }

        public void ButtonClick(int mouseX, int mouseY)
        {
            if (rollButtonLoc.Contains(mouseX, mouseY))
            {
                rollsLeft = 5;
                Roll();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currRoll, dieLoc, Color.White);
            spriteBatch.Draw(rollButton, rollButtonLoc, Color.White);
            if (rollsLeft > 0)
            {
                rollsLeft--;
                Roll();
            }
        }
    }
}
