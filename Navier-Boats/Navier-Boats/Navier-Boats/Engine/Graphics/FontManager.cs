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
using libXNADeveloperConsole;
using Navier_Boats.Game.Entities;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Level;
using Navier_Boats.Engine.Graphics;

namespace Navier_Boats.Engine.Graphics
{
    class FontManager
    {
        private static FontManager _instance;

        public static FontManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FontManager();
            }
            return _instance;
        }

        private Dictionary<string, SpriteFont> loadedFonts;

        public SpriteFont this[string index]
        {
            get
            {
                if (loadedFonts.ContainsKey(index))
                {
                    return loadedFonts[index];
                }
                return null;
            }
            set { loadedFonts[index] = value; }
        }

        private FontManager()
        {
            loadedFonts = new Dictionary<string, SpriteFont>();
        }

        public void LoadFont(SpriteFont spriteFont, string fontName)
        {
            loadedFonts.Add(fontName, spriteFont);
        }
    }
}
