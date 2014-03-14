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
        Texture2D rightButton, leftButton;
        List<Texture2D> options;

        public Wheel(string dir, ContentManager content)
        {
            LoadTextures(dir, content);
        }

        public void LoadTextures(string dir, ContentManager content)
        {
            content.
        }
    }
}
