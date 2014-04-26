﻿using System;
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
using Navier_Boats.Engine.System;
using Lock = System.Object;

namespace Navier_Boats.Engine.Level
{
    public class CurrentLevel
    {
        #region SINGLETON_MEMBERS
        private static CurrentLevel instance;
        public static CurrentLevel GetInstance()
        {
            if (instance == null)
            {
                instance = new CurrentLevel();
            }
            return instance;
        }
        #endregion

        private static Random random = new Random();

        public static Random GetRandom()
        {
            return random;
        }

        public const int OCTAVES = 4;
        public const float GROUNDLAC = 2.145634563f;
        public const float WATERLAC = 2.17832f;
        public const int SEED = 2; //Not Implemented
        public const int GRID = Chunk.CHUNK_WIDTH;
        public const int NUM_ROAD_CONNECTIONS = 2; //This will change per chunk later

        private Chunk[,] chunks;

        private Lock chunkLock = new Lock();
        public Chunk[,] LoadedChunks
        {
            get
            {
                lock (chunkLock)
                {
                    return chunks;
                }
            }
            set
            {
                lock (chunkLock)
                {
                    chunks = value;
                }
            }
        }

        private List<Texture2D> tileTextures;

        private SpriteFont debugFont;

        private string chunkSaveDirectory = "./LevelData";

        private TerrainGenerator terrainGen;

        private CurrentLevel()
        {
            //EntityManager.GetInstance().EntitySaveDir = Path.Combine(chunkSaveDirectory, "entityData");
            EntityManager.GetInstance().AddEntity(new Player(new Vector2(0, 0)));

            terrainGen = new TerrainGenerator(OCTAVES, GROUNDLAC, WATERLAC, GRID, random.Next());
        }

        public void LoadContent(ContentManager Content)
        {
            EntityManager.GetInstance().Player.Texture = TextureManager.GetInstance().LoadTexture("playerTexture");
            EntityManager.GetInstance().Player.HeadTexture = TextureManager.GetInstance().LoadTexture("playerHeadTexture");

            ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand(
                    "spawn",
                    (args, logQueue)
                        =>
                    {
                        Wanderer wanderer;
                        wanderer = new Wanderer(new Vector2(0, 0));
                        wanderer.Texture = TextureManager.GetInstance().LoadTexture("playerTexture");
                        wanderer.HeadTexture = TextureManager.GetInstance().LoadTexture("playerHeadTexture");
                        EntityManager.GetInstance().AddEntity(wanderer);
                        return 0;
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

            ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand(
                    "clearleveldata",
                    (args, logQueue)
                        =>
                    {
                        LoadedChunks = null;
                        Directory.Delete(chunkSaveDirectory, true);
                        Environment.Exit(0);
                        return 0;
                    }));
            ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand(
                    "debugpaths",
                    (args, logQueue)
                        =>
                    {
                        ConsoleVars.GetInstance().DebugPathing = !ConsoleVars.GetInstance().DebugPathing;
                        return 0;
                    }));

            tileTextures = new List<Texture2D>();
            tileTextures.Add(Content.Load<Texture2D>("tiles\\road"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\green"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\sand"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\blue"));
            tileTextures.Add(Content.Load<Texture2D>("tiles\\clear"));

            terrainGen.SetRoadPatterns(new Dictionary<string, Texture2D> {
                {"Straight", Content.Load<Texture2D>("tiles\\Road_Straight")},
                {"Diag", Content.Load<Texture2D>("tiles\\Road_Diag")},
                {"Start", Content.Load<Texture2D>("tiles\\Road_Start")},            
            });

            FontManager.GetInstance().LoadFont(Content.Load<SpriteFont>("consolas"), "Console Font");

            debugFont = Content.Load<SpriteFont>("consolas");

            InitLevel();
        }

        public void Update(GameTime gameTime, KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState)
        {
            //Entity Update Loop
            EntityManager.GetInstance().Update(gameTime, keyState, prevKeyState, mouseState, prevMouseState);
            //Entity LateUpdate Loop
            EntityManager.GetInstance().LateUpdate(gameTime);

            UpdateChunks();

            //Center camera on player
            Camera.Focus(EntityManager.GetInstance().Player.Position);
        }

        private void UpdateChunks()
        {
            Vector2 playerPos = EntityManager.GetInstance().Player.Position;
            Vector2 upperLeftBound = GetChunkCenterWorldCoords(ChunkCoordsToWorldCoords(LoadedChunks[0, 0].Position));
            Vector2 lowerRightBound = GetChunkCenterWorldCoords(ChunkCoordsToWorldCoords(LoadedChunks[1, 1].Position));

            if (playerPos.X < upperLeftBound.X)
            {
                //Player has moved left
                //push left LoadedChunks over
                LoadedChunks[1, 0] = LoadedChunks[0, 0];
                LoadedChunks[1, 1] = LoadedChunks[0, 1];
                //unload left LoadedChunks and replace
                LoadedChunks[0, 0] = new Chunk(Chunk.CoordsToChunkID(LoadedChunks[0, 0].Position + new Vector2(-1, 0)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                //EntityManager.GetInstance().LoadChunk(new Vector2(0, 0), Chunk.CoordsToChunkID(LoadedChunks[0, 0].Position + new Vector2(-1, 0)));   
                LoadedChunks[0, 1] = new Chunk(Chunk.CoordsToChunkID(LoadedChunks[0, 1].Position + new Vector2(-1, 0)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                //EntityManager.GetInstance().LoadChunk(new Vector2(0, 1), Chunk.CoordsToChunkID(LoadedChunks[0, 1].Position + new Vector2(-1, 0)));   
            }
            else if (playerPos.X > lowerRightBound.X)
            {
                //Player has moved right                                                                                                               
                //push right LoadedChunks over                                                                                                         
                LoadedChunks[0, 0] = LoadedChunks[1, 0];
                LoadedChunks[0, 1] = LoadedChunks[1, 1];
                //unload right LoadedChunks and replace                                                                                                
                LoadedChunks[1, 0] = new Chunk(Chunk.CoordsToChunkID(LoadedChunks[1, 0].Position + new Vector2(1, 0)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                //EntityManager.GetInstance().LoadChunk(new Vector2(1, 0), Chunk.CoordsToChunkID(LoadedChunks[1, 0].Position + new Vector2(1, 0)));    
                LoadedChunks[1, 1] = new Chunk(Chunk.CoordsToChunkID(LoadedChunks[1, 1].Position + new Vector2(1, 0)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                //EntityManager.GetInstance().LoadChunk(new Vector2(1, 1), Chunk.CoordsToChunkID(LoadedChunks[1, 1].Position + new Vector2(1, 0)));    
            }
            if (playerPos.Y < upperLeftBound.Y)
            {
                //Player has moved up                                                                                                                  
                //push top LoadedChunks down                                                                                                           
                LoadedChunks[0, 1] = LoadedChunks[0, 0];
                LoadedChunks[1, 1] = LoadedChunks[1, 0];
                //unload top LoadedChunks and replace                                                                                                  
                LoadedChunks[0, 0] = new Chunk(Chunk.CoordsToChunkID(LoadedChunks[0, 0].Position + new Vector2(0, -1)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                //EntityManager.GetInstance().LoadChunk(new Vector2(0, 0), Chunk.CoordsToChunkID(LoadedChunks[0, 0].Position + new Vector2(0, -1)));   
                LoadedChunks[1, 0] = new Chunk(Chunk.CoordsToChunkID(LoadedChunks[1, 0].Position + new Vector2(0, -1)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                //EntityManager.GetInstance().LoadChunk(new Vector2(1, 0), Chunk.CoordsToChunkID(LoadedChunks[1, 0].Position + new Vector2(0, -1)));
            }
            if (playerPos.Y > lowerRightBound.Y)
            {
                //Player has moved up
                //push top LoadedChunks down
                LoadedChunks[0, 0] = LoadedChunks[0, 1];
                LoadedChunks[1, 0] = LoadedChunks[1, 1];
                //unload top LoadedChunks and replace
                LoadedChunks[0, 1] = new Chunk(Chunk.CoordsToChunkID(LoadedChunks[0, 1].Position + new Vector2(0, 1)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                //EntityManager.GetInstance().LoadChunk(new Vector2(0, 1), Chunk.CoordsToChunkID(LoadedChunks[0, 1].Position + new Vector2(0, 1)));
                LoadedChunks[1, 1] = new Chunk(Chunk.CoordsToChunkID(LoadedChunks[1, 1].Position + new Vector2(0, 1)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                //EntityManager.GetInstance().LoadChunk(new Vector2(1, 1), Chunk.CoordsToChunkID(LoadedChunks[1, 1].Position + new Vector2(0, 1)));
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
        private static Vector2 ChunkCoordsToWorldCoords(Vector2 chunkCoords)
        {
            return new Vector2(Chunk.TILE_WIDTH * Chunk.CHUNK_WIDTH, Chunk.TILE_HEIGHT * Chunk.CHUNK_HEIGHT) * chunkCoords;
        }
        private static Vector2 WorldCoordsToChunkCoords(Vector2 chunkCoords)
        {
            return new Vector2(1.0f / (Chunk.TILE_WIDTH * Chunk.CHUNK_WIDTH), 1.0f / (Chunk.TILE_HEIGHT * Chunk.CHUNK_HEIGHT)) * chunkCoords;
        }

        /// <summary>
        /// Gets the Chunk Coordinates of the Chunk enclosing the point
        /// </summary>
        /// <param name="point">The point to get the chunk coords for</param>
        /// <returns>The chunk coords of the point given.</returns>
        private static  Vector2 GetEnclosingChunk(Vector2 point)
        {
            Vector2 scaled = new Vector2((point.X / (Chunk.CHUNK_WIDTH * Chunk.TILE_WIDTH)), (point.Y / (Chunk.CHUNK_HEIGHT * Chunk.TILE_HEIGHT)));
            scaled.X = (int)Math.Floor(scaled.X);
            scaled.Y = (int)Math.Floor(scaled.Y);
            return scaled;
        }


        public static Vector2 WorldCoordsToTileCoords(Vector2 point, out Vector2 chunkCoord)
        {

            chunkCoord = GetEnclosingChunk(new Vector2((int)point.X, (int)point.Y));

            Vector2 chunkWorldCoord = ChunkCoordsToWorldCoords(chunkCoord);

            Vector2 pointChunkOffset = (point - chunkWorldCoord) / new Vector2(Chunk.TILE_WIDTH, Chunk.TILE_HEIGHT);

            pointChunkOffset.X += pointChunkOffset.X < 0 ? Chunk.CHUNK_WIDTH - 1 : 0;
            pointChunkOffset.Y += pointChunkOffset.Y < 0 ? Chunk.CHUNK_HEIGHT - 1 : 0;


            return pointChunkOffset;
        }
        public short GetTileDataAtPoint(TileLayer tileLayer, Vector2 point)
        {
            Vector2 chunkCoord;
            Vector2 pointChunkOffset = WorldCoordsToTileCoords(point, out chunkCoord);

            Chunk chunk = null;
            foreach (Chunk loadedChunk in LoadedChunks)
            {
                if (chunkCoord == loadedChunk.Position)
                {
                    chunk = loadedChunk;
                    break;
                }
            }
            if (chunk == null)
            {
                chunk = new Chunk(Chunk.CoordsToChunkID(chunkCoord) + ".chunk", chunkSaveDirectory, ref terrainGen);
            }
            return chunk.GetDataAtPosition(tileLayer, pointChunkOffset);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenWorldBounds = new Rectangle(0, 0, ConsoleVars.GetInstance().WindowWidth, ConsoleVars.GetInstance().WindowHeight);

            //Console.WriteLine("MIN: " + worldViewMin);
            //Console.WriteLine("MAX: " + worldViewMax);
            for (int y = 0; y < LoadedChunks.GetLength(0); y++)
            {
                for (int x = 0; x < LoadedChunks.GetLength(1); x++)
                {
                    Vector2 pos = Vector2.Transform(ChunkCoordsToWorldCoords(LoadedChunks[x, y].Position), Camera.TransformMatrix);
                    if (screenWorldBounds.Intersects(new Rectangle((int)pos.X, (int)pos.Y, Chunk.CHUNK_WIDTH*Chunk.TILE_WIDTH, Chunk.CHUNK_HEIGHT*Chunk.TILE_HEIGHT)))
                    {
                        LoadedChunks[x, y].Draw(spriteBatch, tileTextures, Vector2.Zero, ChunkCoordsToWorldCoords(LoadedChunks[x, y].Position));
                    }
                }
            }

            EntityManager.GetInstance().Draw(spriteBatch);
        }

        public void DrawGUI(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (ConsoleVars.GetInstance().DebugDraw)
            {
                string output = string.Format("Player Position {0}\n"
                                            + "ChunkData: {1}", EntityManager.GetInstance().Player.Position, GetTileDataAtPoint(TileLayer.GROUND_LAYER, EntityManager.GetInstance().Player.Position));
                spriteBatch.DrawString(debugFont, output, new Vector2(1024 - (debugFont.MeasureString(output).X + 10), 10), Color.Black);
            }

            EntityManager.GetInstance().DrawGUI(spriteBatch);
        }

        private void InitLevel()
        {
            if (!Directory.Exists(chunkSaveDirectory))
            {
                Directory.CreateDirectory(chunkSaveDirectory);
            }
            LoadedChunks = new Chunk[2, 2];
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    LoadedChunks[x, y] = new Chunk(Chunk.CoordsToChunkID(new Vector2(x - 1, y)) + ".chunk", chunkSaveDirectory, ref terrainGen);
                }
            }
            foreach (Chunk chunk in LoadedChunks)
            {
                Console.WriteLine(chunk.CHUNK_ID);
                chunk.Save(chunkSaveDirectory);
            }
        }

        public List<Chunk> GetAdjacentChunks(Chunk c)
        {
            return (from Chunk chunk in LoadedChunks.AsQueryable() where chunk != c select chunk).ToList();
        }

    }
}
