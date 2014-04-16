using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

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

        private LoadScreen loadScreen;

        public static ContentManager Content { get; set; }

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
            set
            {
                loadedTextures[index] = value;
                loadedTextures[index].Name = index;
            }
        }

        /// <summary>
        /// Creates the TextureManager instance
        /// </summary>
        private TextureManager()
        {
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            loadedTextures = new Dictionary<string, Texture2D>();

            loadedTextures[""] = new Texture2D(graphicsDevice, 1, 1);
            loadedTextures[""].SetData<Color>(new[] { Color.White });

            loadScreen = new LoadScreen(Content);
        }

        /// <summary>
        /// Generates textures into TextureManager
        /// </summary>
        /// <param name="graphicsDevice">The Graphics Device to use to generate the texture with</param>
        public void GenerateTextures(GraphicsDevice graphicsDevice)
        {
            
            Texture2D CompassTexture;
            Texture2D HealthTexture;
            Texture2D HUDItemBoxTexture;
            Texture2D MoneyTexture;

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

            #region MoneyTexture
            MoneyTexture = new Texture2D(graphicsDevice, 5, 5, false, SurfaceFormat.Color);
            Color[] colorMoney = new Color[25];
            for (int i = 0; i < colorMoney.Length; i++)
            {
                colorMoney[i] = Color.Gold;
            }
            MoneyTexture.SetData(colorMoney);
            #endregion

            loadedTextures["MoneyTexture"] = MoneyTexture;

        }

        //NOT RECURSIVE
        public void LoadAllTexturesInDirectory(string directoryLocation)
        {
            Task.Factory.StartNew(() =>
                   {
                       loadedTextures=loadedTextures.Merge(loadScreen.LoadContent<Texture2D>(directoryLocation));
                   });
        }

        public Texture2D LoadTexture(string location)
        {
            if (loadedTextures.ContainsKey(location))
            {
                return loadedTextures[location];
            }
            return this[location] = Content.Load<Texture2D>(location);
        }
    }
}
