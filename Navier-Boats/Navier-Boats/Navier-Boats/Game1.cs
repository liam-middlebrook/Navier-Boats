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

// DEBUGGING PATHFINDER, REMOVE ONCE IT WORKS
using Navier_Boats.Engine.Pathfinding;

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
        PathThread pathThread = null;
        bool submitPathing = false;
        bool showError = false;
        Texture2D pixel;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 1024;
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

            CurrentLevel.GetInstance().LoadContent(Content);
            /*for (int i = 1; i < 1; i++)
            {
            entities.Add(new Wanderer(new Vector2(30*i,30*i), randy.Next(int.MaxValue)));
            entities[i].Texture = Content.Load<Texture2D>("playerTexture");
            entities[i].HeadTexture = Content.Load<Texture2D>("playerHeadTexture");
                
            }//*/

            ConsoleWindow.GetInstance().ConsoleFont = Content.Load<SpriteFont>("consolas");

            // DEBUGGING PATHFINDER, REMOVE ONCE IT WORKS
            pixel = Content.Load<Texture2D>("1pix");
            Pathfinder pathfinder = new Pathfinder(CurrentLevel.GetInstance(), Heuristics.Manhattan);
            this.pathThread = new PathThread(pathfinder);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (this.pathThread.Running)
                this.pathThread.Stop();
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

            // DEBUGGING PATHFINDER, REMOVE ONCE IT WORKS
            if (!this.submitPathing)
            {
                this.submitPathing = true;
                this.pathThread.Run(new Vector2(0, 0), new Vector2(1300, -1300), 10);
            }

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
            spriteBatch.End();
            spriteBatch.Begin();
            ConsoleWindow.GetInstance().Draw(spriteBatch);
            CurrentLevel.GetInstance().DrawGUI(spriteBatch);

            // DEBUGGING PATHFINDER, REMOVE ONCE IT WORKS
            if (this.pathThread.Done)
            {
                Console.WriteLine("DONE PATHING");
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
