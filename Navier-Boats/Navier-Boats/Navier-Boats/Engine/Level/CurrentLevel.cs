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
    public class CurrentLevel
    {
        public const int OCTAVES = 4;
        public const float LAC = 2.145634563f;
        public const int SEED = 2; //Not Implemented
        public const int GRID = 32;

        private Chunk[,] chunks;

        private List<Texture2D> tileTextures;

        private Random randy;

        private List<Entity> entities;

        private SpriteFont debugFont;

        private string chunkSaveDirectory = "./LevelData";

        private EntityManager entityManager;

        private TerrainGenerator terrainGen = new TerrainGenerator(OCTAVES, LAC, GRID, SEED, TerrainType.Country);

        public CurrentLevel()
        {
            entities = new List<Entity>();
            entityManager = new EntityManager(Path.Combine(chunkSaveDirectory,"entityData"));
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
            tileTextures.Add(Content.Load<Texture2D>("tiles\\road"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\green"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\sand"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\blue"));
            

            debugFont = Content.Load<SpriteFont>("consolas");

            InitLevel();
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
            Camera.Focus(entities[0].Position);
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
                chunks[0, 0] = new Chunk(Chunk.CoordsToChunkID(chunks[0, 0].Position + new Vector2(-1, 0)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                entityManager.LoadChunk(new Vector2(0, 0), Chunk.CoordsToChunkID(chunks[0, 0].Position + new Vector2(-1, 0)));
                chunks[0, 1] = new Chunk(Chunk.CoordsToChunkID(chunks[0, 1].Position + new Vector2(-1, 0)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                entityManager.LoadChunk(new Vector2(0, 1), Chunk.CoordsToChunkID(chunks[0, 1].Position + new Vector2(-1, 0)));
            }
            else if (playerPos.X > lowerRightBound.X)
            {
                //Player has moved right
                //push right chunks over
                chunks[0, 0] = chunks[1, 0];
                chunks[0, 1] = chunks[1, 1];
                //unload right chunks and replace
                chunks[1, 0] = new Chunk(Chunk.CoordsToChunkID(chunks[1, 0].Position + new Vector2(1, 0)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                entityManager.LoadChunk(new Vector2(1, 0), Chunk.CoordsToChunkID(chunks[1, 0].Position + new Vector2(1, 0)));
                chunks[1, 1] = new Chunk(Chunk.CoordsToChunkID(chunks[1, 1].Position + new Vector2(1, 0)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                entityManager.LoadChunk(new Vector2(1, 1), Chunk.CoordsToChunkID(chunks[1, 1].Position + new Vector2(1, 0)));
            }
            if (playerPos.Y < upperLeftBound.Y)
            {
                //Player has moved up
                //push top chunks down
                chunks[0, 1] = chunks[0, 0];
                chunks[1, 1] = chunks[1, 0];
                //unload top chunks and replace
                chunks[0, 0] = new Chunk(Chunk.CoordsToChunkID(chunks[0, 0].Position + new Vector2(0, -1)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                entityManager.LoadChunk(new Vector2(0, 0), Chunk.CoordsToChunkID(chunks[0, 0].Position + new Vector2(0, -1)));
                chunks[1, 0] = new Chunk(Chunk.CoordsToChunkID(chunks[1, 0].Position + new Vector2(0, -1)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                entityManager.LoadChunk(new Vector2(1, 0), Chunk.CoordsToChunkID(chunks[1, 0].Position + new Vector2(0, -1)));
            }
            if (playerPos.Y > lowerRightBound.Y)
            {
                //Player has moved up
                //push top chunks down
                chunks[0, 0] = chunks[0, 1];
                chunks[1, 0] = chunks[1, 1];
                //unload top chunks and replace
                chunks[0, 1] = new Chunk(Chunk.CoordsToChunkID(chunks[0, 1].Position + new Vector2(0, 1)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                entityManager.LoadChunk(new Vector2(0, 1), Chunk.CoordsToChunkID(chunks[0, 1].Position + new Vector2(0, 1)));
                chunks[1, 1] = new Chunk(Chunk.CoordsToChunkID(chunks[1, 1].Position + new Vector2(0, 1)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                entityManager.LoadChunk(new Vector2(1, 1), Chunk.CoordsToChunkID(chunks[1, 1].Position + new Vector2(0, 1)));
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
        private Vector2 WorldCoordsToChunkCoords(Vector2 chunkCoords)
        {
            return new Vector2(1.0f/(Chunk.TILE_WIDTH * Chunk.CHUNK_WIDTH), 1.0f/(Chunk.TILE_HEIGHT * Chunk.CHUNK_HEIGHT)) * chunkCoords;
        }

        /// <summary>
        /// Gets the Chunk Coordinates of the Chunk enclosing the point
        /// </summary>
        /// <param name="point">The point to get the chunk coords for</param>
        /// <returns>The chunk coords of the point given.</returns>
        private Vector2 GetEnclosingChunk(Vector2 point)
        {
            Vector2 scaled = new Vector2((point.X/(Chunk.CHUNK_WIDTH*Chunk.TILE_WIDTH)), (point.Y/(Chunk.CHUNK_HEIGHT*Chunk.TILE_HEIGHT)));
            scaled.X = (int)Math.Floor(scaled.X);
            scaled.Y = (int)Math.Floor(scaled.Y);
            return scaled;
        }


        public short GetTileDataAtPoint(TileLayer tileLayer, Vector2 point)
        {
            //Gets the chunk the point is in
            Vector2 chunkCoord = GetEnclosingChunk(point);

            Vector2 chunkWorldCoord = ChunkCoordsToWorldCoords(chunkCoord);

            Vector2 pointChunkOffset = (point - chunkWorldCoord) / new Vector2(Chunk.TILE_WIDTH, Chunk.TILE_HEIGHT);

            pointChunkOffset.X += pointChunkOffset.X < 0 ? Chunk.CHUNK_WIDTH : 0;
            pointChunkOffset.Y += pointChunkOffset.Y < 0 ? Chunk.CHUNK_WIDTH : 0;


            Chunk chunk = null;
            foreach (Chunk loadedChunk in chunks)
            {
                if(chunkCoord == loadedChunk.Position)
                {
                    chunk = loadedChunk;
                    break;
                }
            }
            if(chunk == null)
            {
                chunk = new Chunk(Chunk.CoordsToChunkID(chunkCoord) + ".chunk", chunkSaveDirectory, ref terrainGen);
            }
            return chunk.GetDataAtPosition(tileLayer, pointChunkOffset);
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
                string output = string.Format("Player Position {0}\n"
                                            + "ChunkData: {1}", entities[0].Position, GetTileDataAtPoint(TileLayer.GROUND_LAYER, entities[0].Position));
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
                    chunks[x, y] = new Chunk(Chunk.CoordsToChunkID(new Vector2(x - 1, y)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                }
            }
            foreach (Chunk chunk in chunks)
            {
                Console.WriteLine(chunk.CHUNK_ID);
                chunk.Save(chunkSaveDirectory);
            }
        }

    }
}
