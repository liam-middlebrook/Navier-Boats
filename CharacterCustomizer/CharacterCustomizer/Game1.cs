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

//additional using statements
using System.Windows.Forms;
using System.IO;

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
        MiscButton previewButton, saveButton, loadButton;
        MouseState mouseState, prevMouseState;//current and previous mouse state

        Preview preview = null;
        Texture2D saving, save;
        string loadedFile;

        const int WHEEL_SCALE = 6;
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
            characterParts.Add(new Wheel(WHEEL_SCALE, "Faces", Content, x + 50, y));
            y += 20 * WHEEL_SCALE;
            characterParts.Add(new LongWheel(WHEEL_SCALE, "Bodies", Content, x + 20, y - 20, 2, 1.5, 1));
            y += 20 * WHEEL_SCALE;
            characterParts.Add(new LongWheel(WHEEL_SCALE, "Legs", Content, x + 35, y, 1.5, 1.5, 1.5));

            die = new Die(DIE_SCALE, 500, 100, Content);

            previewButton = new MiscButton(500, 300, "Buttons/Preview", Content, 3);
            saveButton = new MiscButton(500, 350, "Buttons/Save", Content, 3);
            save = saveButton.Button;
            loadButton = new MiscButton(500, 400, "Buttons/Load", Content, 3);

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
            saving = Content.Load<Texture2D>("Buttons/Saving");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                if (preview != null)
                    preview.ButtonClick(mouseState.X, mouseState.Y);
                else
                {
                    foreach (Wheel characterPart in characterParts)
                        characterPart.ButtonClick(mouseState.X, mouseState.Y);
                    die.ButtonClick(mouseState.X, mouseState.Y);
                    previewButton.ButtonClick(mouseState.X, mouseState.Y);
                    saveButton.ButtonClick(mouseState.X, mouseState.Y);
                    loadButton.ButtonClick(mouseState.X, mouseState.Y);
                }
            }
            else if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && prevMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (preview != null)
                    preview.ButtonUnClick();
                else
                {
                    foreach (Wheel characterPart in characterParts)
                        characterPart.ButtonUnClick();
                    die.ButtonUnClick();
                    previewButton.ButtonUnClick();
                    saveButton.ButtonUnClick();
                    loadButton.ButtonUnClick();
                }
            }
            if (previewButton.Clicked)
            {
                preview = new Preview(characterParts[0].Current, characterParts[0].Color, characterParts[1].Current, characterParts[1].Color, characterParts[2].Current, characterParts[2].Color, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content);
                previewButton.ButtonUnClick();
            }
            if (saveButton.Clicked)
            {
                saveButton.Button = saving;
                Save();
                saveButton.ButtonUnClick();
                saveButton.Button = save;
            }
            if (loadButton.Clicked)
            {
                Load();
                loadButton.ButtonUnClick();
            }
            if (preview != null && preview.Clicked)
                preview = null;
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
            previewButton.Draw(spriteBatch);
            saveButton.Draw(spriteBatch);
            loadButton.Draw(spriteBatch);

            if (preview != null)
            {
                preview.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Save()
        {
            SaveFileDialog saver = new SaveFileDialog();

            saver.AddExtension = true;
            saver.CheckPathExists = true;
            saver.DefaultExt = "dat";
            saver.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Saves\\";
            saver.OverwritePrompt = true;
            saver.RestoreDirectory = true;
            saver.Title = "Save Character As";
            if (loadedFile != null)
                saver.FileName = loadedFile;

            if (saver.ShowDialog() == DialogResult.OK)
            {
                using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(saver.FileName)))
                {
                    try
                    {
                        foreach (Wheel w in characterParts)
                            bw.Write(w.Save());
                        foreach (int stat in die.Save())
                            bw.Write(stat);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        if (bw != null)
                            bw.Dispose();
                    }
                }
            }

            loadedFile = saver.FileName;

            if (saver != null)
                saver.Dispose();
        }

        public void Load()
        {
            OpenFileDialog loader = new OpenFileDialog();

            loader.CheckPathExists = true;
            loader.CheckFileExists = true;
            //loader.DefaultExt = "dat";
            loader.Filter = "Data files (.dat)|*.dat|All files|*.*";
            loader.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Saves\\";
            loader.RestoreDirectory = true;
            loader.Title = "Load Character";

            if (loader.ShowDialog() == DialogResult.OK)
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead(loader.FileName)))
                {
                    try
                    {
                        foreach (Wheel w in characterParts)
                            w.Load(br.ReadString());
                        List<int> stats = new List<int>();
                        foreach (int stat in die.Save())
                            stats.Add(br.ReadInt32());
                        die.Load(stats);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        if (br != null)
                            br.Dispose();
                    }
                }
            }

            loadedFile = loader.FileName;

            if (loader != null)
                loader.Dispose();
        }

    }
}
