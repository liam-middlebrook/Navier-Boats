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
    class MiscButton : CustomizeElement
    {
        protected Texture2D button;
        protected Rectangle buttonSpace;
        protected bool clicked = false;

        /// <summary>
        /// get or set
        /// </summary>
        public Texture2D Button
        {
            get { return button; }

            set { button = value; }
        }

        /// <summary>
        /// get
        /// </summary>
        public bool Clicked
        {
            get { return clicked; }
        }

        public MiscButton(int x, int y, string fileName, ContentManager content, int s)
            : base(s)
        {
            button = content.Load<Texture2D>(fileName);
            buttonSpace = new Rectangle(x, y, button.Width * s, button.Height * s);
        }

        public MiscButton(int x, int y, Texture2D texture, int s)
            : base(s)
        {
            button = texture;
            buttonSpace = new Rectangle(x, y, button.Width * s, button.Height * s);
        }

        /// <summary>
        /// Checks if the mouse has clicked either button, calls the appropriate cycle if necessary
        /// </summary>
        /// <param name="mouseX">The current mouse x</param>
        /// <param name="mouseY">The current mouse y</param>
        public virtual void ButtonClick(int mouseX, int mouseY)
        {
            if (buttonSpace.Contains(mouseX, mouseY))
            {
                clicked = true;
            }
        }

        /// <summary>
        /// "Unclicks" the buttons i.e. changes their clicked variable.
        /// </summary>
        public virtual void ButtonUnClick()
        {
            if (clicked)
                clicked = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(button, buttonSpace, clicked ? Color.CadetBlue : Color.White);
        }
    }
}
