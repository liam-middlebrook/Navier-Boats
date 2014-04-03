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

namespace CharacterCustomizer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont sf;

        List<Wheel> characterParts;//all the character component wheels
        Die die;
        MouseState mouseState, prevMouseState;//current and previous mouse state

        const int WHEEL_SCALE = 5;
        const int DIE_SCALE = 4;

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
            this.IsMouseVisible = true;
            //graphics.PreferMultiSampling = false;
            //graphics.ApplyChanges();

            mouseState = Mouse.GetState();

            characterParts = new List<Wheel>();
            int x = 20;
            int y = 50;
            characterParts.Add(new Wheel(WHEEL_SCALE, "Faces", Content, x, y));
            y += 20 * WHEEL_SCALE;
            characterParts.Add(new LongWheel(WHEEL_SCALE, "Bodies", Content, x, y, 2, 1.5, 1));
            y += 20 * WHEEL_SCALE;
            characterParts.Add(new LongWheel(WHEEL_SCALE, "Legs", Content, x, y, 1.5, 1.5, 1.5));

            die = new Die(DIE_SCALE, 500, 20, Content);

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

            // TODO: use this.Content to load your game content here
            sf = Content.Load<SpriteFont>("SpriteFont1");
            die.StatText = sf;
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

            // TODO: Add your update logic here
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                foreach (Wheel characterPart in characterParts)
                    characterPart.ButtonClick(mouseState.X, mouseState.Y);
                die.ButtonClick(mouseState.X, mouseState.Y);
            }
            else if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                foreach (Wheel characterPart in characterParts)
                    characterPart.ButtonUnClick();
                die.ButtonUnClick();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumPurple);

            // TODO: Add your drawing code here
            
            //this overload turns off antialiasing
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

            
            foreach (Wheel characterPart in characterParts)
                characterPart.Draw(spriteBatch);
            die.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
