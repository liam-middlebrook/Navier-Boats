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

/**
 * Add your names here once you have completed the Git/SourceTree seminar:
 * 
 * Liam Middlebrook
 * 
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
        Random randy;

        List<LivingEntity> entities;

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
            entities = new List<LivingEntity>();

            entities.Add(new Player(new Vector2(100, 100)));

            randy = new Random();

            keyHelper = new KeyboardHelper();

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

            entities[0].Texture = Content.Load<Texture2D>("playerTexture");
            entities[0].HeadTexture = Content.Load<Texture2D>("playerHeadTexture");

            /*for (int i = 1; i < 1; i++)
            {
            entities.Add(new Wanderer(new Vector2(30*i,30*i), randy.Next(int.MaxValue)));
            entities[i].Texture = Content.Load<Texture2D>("playerTexture");
            entities[i].HeadTexture = Content.Load<Texture2D>("playerHeadTexture");
                
            }//*/

            ConsoleWindow.GetInstance().ConsoleFont = Content.Load<SpriteFont>("consolas");

            ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand(
                    "spawn",
                    (args, logQueue) 
                        =>
                     {
                         int i = entities.Count;
                     entities.Add(new Wanderer(new Vector2(250, 250), randy.Next(int.MaxValue)));
                     entities[i].Texture = Content.Load<Texture2D>("playerTexture");
                     entities[i].HeadTexture = Content.Load<Texture2D>("playerHeadTexture");
                       ; return 0;
                 }));
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
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

            ConsoleWindow.GetInstance().Update(keyHelper);

            foreach (LivingEntity entity in entities)
            {
                if (entity is Player)
                {
                    ((Player)entity).Update(gameTime, keyHelper, Mouse.GetState());
                }
                else
                {
                    entity.Update(gameTime);
                }
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

            spriteBatch.Begin();

            foreach (LivingEntity entity in entities)
            {
                entity.Draw(spriteBatch);
            }

            ConsoleWindow.GetInstance().Draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
