using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Navier_Boats.Engine.Graphics;

namespace Navier_Boats.Engine.System
{
    class LoadScreen
    {
        private ContentManager content;
        private Texture2D pixelTex;
        public LoadScreen(ContentManager content)
        {
            this.content = content;
            pixelTex = TextureManager.GetInstance()[""];
        }

        public Dictionary<string, T> LoadContent<T>(string directoryToLoadFrom)
        {
            // Get file list
            List<string> files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/" + content.RootDirectory + "/" + directoryToLoadFrom).ToList<string>();

            // Check for valid .XNB files
            files.RemoveAll((f) => { return !f.EndsWith(".xnb"); });

            Dictionary<string, T> contentList = new Dictionary<string, T>();

            for (int i = 0; i < files.Count; ++i)
            {
                string name = Path.GetFileName(files[i]);

                // Trim File Extensions
                name = name.Substring(0, name.Length - 4);
                string file = directoryToLoadFrom + "/" + name;
                contentList.Add(file, content.Load<T>(file));
            }
            return contentList;
        }
    }
}
