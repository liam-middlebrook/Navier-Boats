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

        /// <summary>
        /// The instance of TextureManager
        /// </summary>
        private static TextureManager _instance;

        /// <summary>
        /// Gets the instance of TextureManager
        /// </summary>
        /// <returns>The instance of TextureManager</returns>
        public static TextureManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TextureManager();
            }
            return _instance;
        }

        #endregion

        /// <summary>
        /// A dictionary containg the textures loaded into TextureManager
        /// </summary>
        private Dictionary<string, Texture2D> loadedTextures;

        /// <summary>
        /// An Indexer Property that exposes loaded textures in a dictionary like format
        /// </summary>
        /// <param name="index">The name of the texture</param>
        /// <returns>The texture at the specified name</returns>
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

        /// <summary>
        /// Creates the TextureManager instance
        /// </summary>
        private TextureManager()
        {
            loadedTextures = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Generates textures into TextureManager
        /// </summary>
        /// <param name="graphicsDevice">The Graphics Device to use to generate the texture with</param>
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
                colorOne[i] = Color.Black;
            }
            HUDItemBoxTexture.SetData(colorOne);

            #endregion

            loadedTextures["HUDItemBoxTexture"] = HUDItemBoxTexture;

            #region HealthTexture

            HealthTexture = new Texture2D(graphicsDevice, 5, 5, false, SurfaceFormat.Color);
            Color[] colorTwo = new Color[25];
            for (int i = 0; i < colorTwo.Length; i++)
            {
                colorTwo[i] = Color.Red;
            }
            HealthTexture.SetData(colorTwo);

            #endregion

            loadedTextures["HealthTexture"] = HealthTexture;
        }
    }
}
