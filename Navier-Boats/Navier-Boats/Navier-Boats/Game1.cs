using System;
using System.Collections.Generic;
using System.Linq;
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
using Navier_Boats.Engine.System;
using Navier_Boats.Engine.Inventory;

// DEBUGGING PATHFINDER, REMOVE ONCE IT WORKS
using Navier_Boats.Engine.Pathfinding;
using Navier_Boats.Engine.Pathfinding.Threading;

/**
 * Add your names here once you have completed the Git/SourceTree seminar:
 * 
 * Liam Middlebrook - Completed
 * Sam Willis - Completed
 * Sean Maraia - Completed
 * Thomas Landi - Completed
 * Michael Cohen - Completed
 * Sam Bloomberg - Completed
 */

namespace Navier_Boats
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardHelper keyHelper;

        MouseState mouseState;
        MouseState prevMouseState;

        // DEBUGGING PATHFINDER, REMOVE ONCE IT WORKS
        Texture2D pathSquare = null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            keyHelper = new KeyboardHelper();
            mouseState = Mouse.GetState();

            ConsoleVars.GetInstance().WindowWidth = 1024;
            ConsoleVars.GetInstance().WindowHeight = 768;

            graphics.PreferredBackBufferWidth = ConsoleVars.GetInstance().WindowWidth;
            graphics.PreferredBackBufferHeight = ConsoleVars.GetInstance().WindowHeight;
            graphics.ApplyChanges();

            IsMouseVisible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.Content = Content;
            TextureManager.GetInstance().Initialize(GraphicsDevice);
            TextureManager.GetInstance().GenerateTextures(GraphicsDevice);

            CurrentLevel.GetInstance().LoadContent(Content);
            //Load boat mouse cursor
            TextureManager.GetInstance()["cursor"] = Content.Load<Texture2D>("cursor");
            
            /*for (int i = 1; i < 1; i++)
            {
            entities.Add(new Wanderer(new Vector2(30*i,30*i), randy.Next(int.MaxValue)));
            entities[i].Texture = Content.Load<Texture2D>("playerTexture");
            entities[i].HeadTexture = Content.Load<Texture2D>("playerHeadTexture");
                
            }//*/

            ConsoleWindow.GetInstance().ConsoleFont = Content.Load<SpriteFont>("consolas");

            ItemManager.GetInstance();

            //TextureManager.GetInstance()["debugTextures/path"] = Content.Load<Texture2D>("debugTextures/path");
            TextureManager.GetInstance().LoadAllTexturesInDirectory("debugTextures");
            // TODO: use this.Content to load your game content here

            Wanderer test = new Wanderer(new Vector2(0, 0));
            test.Texture = Content.Load<Texture2D>("playerTexture");
            test.HeadTexture = Content.Load<Texture2D>("playerHeadTexture");
            //EntityManager.GetInstance().SaveEntities("test.ent", test);

            Player player = EntityManager.GetInstance().Player;
            player.Items.AddItem(new ItemStack(ItemManager.GetInstance().Items[0], 64));
            player.Items.AddItem(new ItemStack(ItemManager.GetInstance().Items[1], 64));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            PathThreadPool.GetInstance().CancelAll();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyHelper.UpdateKeyStates();
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            ConsoleWindow.GetInstance().Update(keyHelper);

            CurrentLevel.GetInstance().Update(gameTime, keyHelper.KeyState, keyHelper.PrevKeyState, mouseState, prevMouseState);

            PathThreadPool.GetInstance().Update();

            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.TransformMatrix);

            CurrentLevel.GetInstance().Draw(spriteBatch);

            PathThreadPool.GetInstance().Draw(spriteBatch, TextureManager.GetInstance()["debugTextures/path"]);

            spriteBatch.End();
            spriteBatch.Begin();
            ConsoleWindow.GetInstance().Draw(spriteBatch);
            CurrentLevel.GetInstance().DrawGUI(spriteBatch, GraphicsDevice);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
