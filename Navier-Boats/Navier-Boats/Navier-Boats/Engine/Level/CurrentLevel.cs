using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Game.Entities;
using libXNADeveloperConsole;
using Microsoft.Xna.Framework.Input;
using Navier_Boats.Engine.Graphics;

namespace Navier_Boats.Engine.Level
{
    class CurrentLevel
    {
        Chunk[,] chunks;
        List<Texture2D> tileTextures;

        Random randy;

        Camera drawCamera;
        List<Entity> entities;

        SpriteFont debugFont;

        private string chunkSaveDirectory = "./LevelData";

        public CurrentLevel(Camera drawCamera)
        {
            this.drawCamera = drawCamera;
            entities = new List<Entity>();

            entities.Add(new Player(new Vector2(300, 300)));
            randy = new Random();
        }

        public void LoadContent(ContentManager Content)
        {
            entities[0].Texture = Content.Load<Texture2D>("playerTexture");
            ((LivingEntity)entities[0]).HeadTexture = Content.Load<Texture2D>("playerHeadTexture");

            ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand(
                    "spawn",
                    (args, logQueue)
                        =>
                    {
                        int i = entities.Count;
                        entities.Add(new Wanderer(new Vector2(250, 250), randy.Next(int.MaxValue)));
                        entities[i].Texture = Content.Load<Texture2D>("playerTexture");
                        ((LivingEntity)entities[i]).HeadTexture = Content.Load<Texture2D>("playerHeadTexture");
                        ; return 0;
                    }));

            ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand(
                    "debugmode",
                    (args, logQueue)
                        =>
                    {
                        ConsoleVars.GetInstance().DebugDraw = !ConsoleVars.GetInstance().DebugDraw;
                        return 0;
                    }));

            tileTextures = new List<Texture2D>();
            tileTextures.Add(Content.Load<Texture2D>("tiles\\abbyblue"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\blue"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\green"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\navylight"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\pink"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\pruplelight"));

            debugFont = Content.Load<SpriteFont>("consolas");

            InitLevel();
        }

        private void SaveChunk(Chunk chunkToSave)
        {
            chunkToSave.Save(chunkSaveDirectory);
        }

        public void Update(GameTime gameTime, KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState)
        {
            //Entity Update Loop
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
                if (entities[i] is IInputControllable)
                {
                    ((IInputControllable)entities[i]).HandleInput(keyState, prevKeyState, mouseState, prevMouseState);
                }
                if (entities[i] is IInteractable)
                {
                    ((IInteractable)entities[i]).CheckInteractions(entities);
                }
                if (((LivingEntity)entities[i]).Health < 0)
                {
                    entities.RemoveAt(i);
                    --i;
                }
            }
            //Entity LateUpdate Loop
            foreach (Entity entity in entities)
            {
                if (entity is ILateUpdateable)
                {
                    (entity as ILateUpdateable).LateUpdate(gameTime);
                }
            }


            UpdateChunks();

            //Center camera on player
            drawCamera.Focus(entities[0].Position);
        }

        private void UpdateChunks()
        {
            Vector2 playerPos = entities[0].Position;
            Vector2 upperLeftBound = GetChunkCenterWorldCoords(ChunkCoordsToWorldCoords(chunks[0, 0].Position));
            Vector2 lowerRightBound = GetChunkCenterWorldCoords(ChunkCoordsToWorldCoords(chunks[1, 1].Position));

            if (playerPos.X < upperLeftBound.X)
            {
                //Player has moved left
                //push left chunks over
                chunks[1, 0] = chunks[0, 0];
                chunks[1, 1] = chunks[0, 1];
                //unload left chunks and replace
                chunks[0, 0] = new Chunk(Chunk.CoordsToChunkID(chunks[0, 0].Position + new Vector2(-1, 0)) + ".chunk", chunkSaveDirectory);
                chunks[0, 1] = new Chunk(Chunk.CoordsToChunkID(chunks[0, 1].Position + new Vector2(-1, 0)) + ".chunk", chunkSaveDirectory);
            }
            else if (playerPos.X > lowerRightBound.X)
            {
                //Player has moved right
                //push right chunks over
                chunks[0, 0] = chunks[1, 0];
                chunks[0, 1] = chunks[1, 1];
                //unload right chunks and replace
                chunks[1, 0] = new Chunk(Chunk.CoordsToChunkID(chunks[1, 0].Position + new Vector2(1, 0)) + ".chunk", chunkSaveDirectory);
                chunks[1, 1] = new Chunk(Chunk.CoordsToChunkID(chunks[1, 1].Position + new Vector2(1, 0)) + ".chunk", chunkSaveDirectory);
            }
            if (playerPos.Y < upperLeftBound.Y)
            {
                //Player has moved up
                //push top chunks down
                chunks[0, 1] = chunks[0, 0];
                chunks[1, 1] = chunks[1, 0];
                //unload top chunks and replace
                chunks[0, 0] = new Chunk(Chunk.CoordsToChunkID(chunks[0, 0].Position + new Vector2(0, -1)) + ".chunk", chunkSaveDirectory);
                chunks[1, 0] = new Chunk(Chunk.CoordsToChunkID(chunks[1, 0].Position + new Vector2(0, -1)) + ".chunk", chunkSaveDirectory);
            }
            if (playerPos.Y > lowerRightBound.Y)
            {
                //Player has moved up
                //push top chunks down
                chunks[0, 0] = chunks[0, 1];
                chunks[1, 0] = chunks[1, 1];
                //unload top chunks and replace
                chunks[0, 1] = new Chunk(Chunk.CoordsToChunkID(chunks[0, 1].Position + new Vector2(0, 1)) + ".chunk", chunkSaveDirectory);
                chunks[1, 1] = new Chunk(Chunk.CoordsToChunkID(chunks[1, 1].Position + new Vector2(0, 1)) + ".chunk", chunkSaveDirectory);
            }
        }

        /// <summary>
        /// Adds the offset to a the world coordinates of a chunk to get it's center
        /// </summary>
        /// <param name="chunkPos">The chunk's position in world coords</param>
        /// <returns>The Center of the Chunk in World Coords</returns>
        private Vector2 GetChunkCenterWorldCoords(Vector2 chunkPos)
        {
            return chunkPos + new Vector2(Chunk.TILE_WIDTH * Chunk.CHUNK_WIDTH / 2, Chunk.TILE_HEIGHT * Chunk.CHUNK_HEIGHT / 2);
        }

        /// <summary>
        /// Converts from Chunk coordinates (0,-1) to World Coordinates (0, -1024)
        /// </summary>
        /// <param name="chunkCoords">The coordinates of the chunk in chunkspace</param>
        /// <returns>The coordinates of the chunk in worldspace</returns>
        private Vector2 ChunkCoordsToWorldCoords(Vector2 chunkCoords)
        {
            return new Vector2(Chunk.TILE_WIDTH * Chunk.CHUNK_WIDTH, Chunk.TILE_HEIGHT * Chunk.CHUNK_HEIGHT) * chunkCoords;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the tilemap
            for (int y = 0; y < chunks.GetLength(0); y++)
            {
                for (int x = 0; x < chunks.GetLength(1); x++)
                {
                    chunks[x, y].Draw(spriteBatch, tileTextures, Vector2.Zero, ChunkCoordsToWorldCoords(chunks[x, y].Position));
                }
            }

            //Draw all entities
            foreach (LivingEntity entity in entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        public void DrawGUI(SpriteBatch spriteBatch)
        {
            if (ConsoleVars.GetInstance().DebugDraw)
            {
                string output = string.Format("Player Position {0}", entities[0].Position);
                spriteBatch.DrawString(debugFont, output, new Vector2(1024 - (debugFont.MeasureString(output).X + 10), 10), Color.Black);
            }
        }

        private void InitLevel()
        {
            if (!Directory.Exists(chunkSaveDirectory))
            {
                Directory.CreateDirectory(chunkSaveDirectory);
            }
            chunks = new Chunk[2, 2];
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    chunks[x, y] = new Chunk(Chunk.CoordsToChunkID(new Vector2(x - 1, y)) + ".chunk", chunkSaveDirectory);
                }
            }
            foreach (Chunk chunk in chunks)
            {
                Console.WriteLine(chunk.CHUNK_ID);
                SaveChunk(chunk);
            }
        }

    }
}
