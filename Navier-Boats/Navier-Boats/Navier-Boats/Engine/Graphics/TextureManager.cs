using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Graphics
{
    public class TextureManager
    {
        #region SINGLETON_MEMBERS

        private static TextureManager _instance;

        public static TextureManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TextureManager();
            }
            return _instance;
        }

        #endregion

        private Dictionary<string, Texture2D> loadedTextures;

        public Texture2D this[string index]
        {
            get
            {
                if (loadedTextures.ContainsKey(index))
                {
                    return loadedTextures[index];
                }
                return loadedTextures[""];
            }
            set { loadedTextures[index] = value; }
        }

        private TextureManager()
        {
            loadedTextures = new Dictionary<string, Texture2D>();
        }

        public void GenerateTextures(GraphicsDevice graphicsDevice)
        {
            loadedTextures[""] = new Texture2D(graphicsDevice, 1, 1);

            Texture2D CompassTexture;
            Texture2D HealthTexture;
            Texture2D HUDItemBoxTexture;

            #region Generate_Compass_Texture

            CompassTexture = new Texture2D(graphicsDevice, 5, 5, false, SurfaceFormat.Color);
            Color[] color = new Color[25];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.Purple;
            }
            CompassTexture.SetData(color);

            #endregion

            loadedTextures["CompassTexture"] = CompassTexture;

            #region Generate_ItemBoxTexture

            HUDItemBoxTexture = new Texture2D(graphicsDevice, 5, 5, false, SurfaceFormat.Color);
            Color[] colorOne = new Color[25];
            for (int i = 0; i < colorOne.Length; i++)
            {
                color[i] = Color.Black;
            }
            HUDItemBoxTexture.SetData(colorOne);

            #endregion

            loadedTextures["HUDItemBoxTexture"] = HUDItemBoxTexture;

            #region HealthTexture

            HealthTexture = new Texture2D(graphicsDevice, 5, 5, false, SurfaceFormat.Color);
            Color[] colorTwo = new Color[25];
            for (int i = 0; i < colorTwo.Length; i++)
            {
                color[i] = Color.Red;
            }
            HealthTexture.SetData(colorTwo);

            #endregion

            loadedTextures["HealthTexture"] = HealthTexture;
        }
    }
}
