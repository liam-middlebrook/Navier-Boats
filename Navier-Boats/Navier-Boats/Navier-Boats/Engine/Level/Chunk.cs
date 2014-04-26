using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Graphics;
using Navier_Boats.Engine.System;

namespace Navier_Boats.Engine.Level
{

    public class Chunk
    {
        public const int CHUNK_WIDTH = 48;
        public const int CHUNK_HEIGHT = 48;

        public const int TILE_WIDTH = 32;
        public const int TILE_HEIGHT = 32;


        private string chunkDir;
        private short[,] chunkDataGroundLayer;
        private short[,] chunkDataRoadLayer;
        private short[,] chunkDataOverLayer;
        private short[,] chunkDataCollision;

        public readonly string CHUNK_ID; //format 1(xIndex-sign)1(yIndex-sign)15(xIndex-value)15(yIndex-value)
        public readonly Vector2 Position;

        private bool fileInUse;

        Random rand;

        private readonly TerrainGenerator terrainGen;
        private List<RoadConnectors> connections;
        /// <summary>
        /// Creates a new Chunk with the specified Chunk Coords
        /// </summary>
        /// <param name="x">The X value of the chunk in chunk-coords</param>
        /// <param name="y">The Y value of the chunk in chunk-coords</param>
        private void CreateChunk(int x, int y)
        {
            connections = (x == 0 && y == 0) ? new List<RoadConnectors> {RoadConnectors.North, RoadConnectors.East, RoadConnectors.South, RoadConnectors.West } : terrainGen.GenerateConnections(CurrentLevel.NUM_ROAD_CONNECTIONS);

            //Fills chunk with tiles generated using Perlin Noise
            rand = CurrentLevel.GetRandom();
            short tileVal = (short)rand.Next(6);

            #region GroundLayer
            chunkDataGroundLayer = new short[CHUNK_WIDTH, CHUNK_HEIGHT];
            

            for (int yIndex = 0; yIndex < CHUNK_HEIGHT; yIndex++)
            {
                for (int xIndex = 0; xIndex < CHUNK_WIDTH; xIndex++)
                {
                    chunkDataGroundLayer[xIndex, yIndex] = terrainGen.GenerateGroundTile(xIndex, yIndex, Position, TerrainType.Country);
                }
            }

            for (int yIndex = 1; yIndex < CHUNK_HEIGHT - 1; yIndex++)
            {
                for (int xIndex = 1; xIndex < CHUNK_WIDTH - 1; xIndex++)
                {
                    if ((chunkDataGroundLayer[xIndex - 1, yIndex] == 1
                            || chunkDataGroundLayer[xIndex, yIndex - 1] == 1
                            || chunkDataGroundLayer[xIndex + 1, yIndex] == 1
                            || chunkDataGroundLayer[xIndex, yIndex + 1] == 1
                        )
                    && chunkDataGroundLayer[xIndex, yIndex] == (short)TileType.Water
                    )
                    {
                        chunkDataGroundLayer[xIndex, yIndex] = (short)TileType.Sand;
                    }
                }
            }

            #endregion



            #region OverLayer

            chunkDataOverLayer = new short[CHUNK_WIDTH, CHUNK_HEIGHT];
            for (int yIndex = 0; yIndex < CHUNK_HEIGHT; yIndex++)
            {
                for (int xIndex = 0; xIndex < CHUNK_WIDTH; xIndex++)
                {
                    chunkDataOverLayer[xIndex, yIndex] = (short)(rand.Next(6) + 1);
                }
            }

            #endregion

            #region CollisionLayer

            chunkDataCollision = new short[CHUNK_WIDTH, CHUNK_HEIGHT];
            for (int yIndex = 0; yIndex < CHUNK_HEIGHT; yIndex++)
            {
                for (int xIndex = 0; xIndex < CHUNK_WIDTH; xIndex++)
                {
                    chunkDataCollision[xIndex, yIndex] = chunkDataGroundLayer[xIndex, yIndex];
                }
            }

            #endregion

            Save(chunkDir);
        }

        /// <summary>
        /// Loads in a Chunk from the HDD
        /// </summary>
        /// <param name="fileName">The Chunks filename</param>
        /// <param name="directory">The Chunk Directory</param>
        public Chunk(string fileName, string directory, ref TerrainGenerator tg)
        {
            chunkDir = directory;

            chunkDataGroundLayer = new short[CHUNK_WIDTH, CHUNK_HEIGHT];
            chunkDataRoadLayer = new short[CHUNK_WIDTH, CHUNK_HEIGHT];
            chunkDataOverLayer = new short[CHUNK_WIDTH, CHUNK_HEIGHT];
            chunkDataCollision = new short[CHUNK_WIDTH, CHUNK_HEIGHT];

            //Parse data to position
            CHUNK_ID = fileName.Remove(fileName.Length - 6);
            string[] location = CHUNK_ID.Split('_');
            Position = new Vector2(int.Parse(location[0]), int.Parse(location[1]));
            terrainGen = tg;

            BinaryReader br = null;
            if (File.Exists(Path.Combine(directory, fileName)) && !fileInUse)
            {
                try
                {
                    fileInUse = true;
                    br = new BinaryReader(
                        File.OpenRead(
                        Path.Combine(directory, fileName)));
                    //Read All Chunk Data From File - Line by Line
                    for (int y = 0; y < CHUNK_HEIGHT; y++)
                    {
                        for (int x = 0; x < CHUNK_WIDTH; x++)
                        {
                            chunkDataGroundLayer[x, y] = br.ReadInt16();
                            chunkDataRoadLayer[x, y] = br.ReadInt16();
                            chunkDataOverLayer[x, y] = br.ReadInt16();
                            chunkDataCollision[x, y] = br.ReadInt16();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (br != null)
                    {
                        br.Close();
                    }
                    fileInUse = false;
                }
            }
            else
            {
                CreateChunk((int)Position.X, (int)Position.Y);
            }
        }

        /// <summary>
        /// Saves a chunk to the HDD
        /// </summary>
        /// <param name="directory">The directory to save the chunk in</param>
        public void Save(string directory)
        {
            BinaryWriter br = null;
            if (!fileInUse)
            {
                try
                {
                    fileInUse = true;
                    br = new BinaryWriter(
                        File.OpenWrite(
                        Path.Combine(directory, CHUNK_ID + ".chunk")));

                    //Write All Chunk Data To File - Line by Line
                    for (int y = 0; y < CHUNK_HEIGHT; y++)
                    {
                        for (int x = 0; x < CHUNK_WIDTH; x++)
                        {
                            br.Write(chunkDataGroundLayer[x, y]);
                            br.Write(chunkDataRoadLayer[x, y]);
                            br.Write(chunkDataOverLayer[x, y]);
                            br.Write(chunkDataCollision[x, y]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (br != null)
                    {
                        br.Close();
                    }

                    fileInUse = false;
                }
            }
        }

        /// <summary>
        /// Converts Chunk Coordinates to CHUNKID string format
        /// </summary>
        /// <param name="x">The X Coordinate of the Chunk in ChunkCoords</param>
        /// <param name="y">The Y Coordinate of the Chunk in ChunkCoords</param>
        /// <returns>The CHUNKID in string format</returns>
        private static string CoordsToChunkID(int x, int y)
        {
            return string.Format("{0}_{1}", x, y);
        }
        /// <summary>
        /// Converts Chunk Coordinates to CHUNKID string format
        /// </summary>
        /// <param name="pos">The Coordinates of the Chunk in ChunkCoords</param>
        /// <returns>The CHUNKID in string format</returns>
        public static string CoordsToChunkID(Vector2 pos)
        {
            return CoordsToChunkID((int)pos.X, (int)pos.Y);
        }

        /// <summary>
        /// Draws a chunk
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw the chunk to</param>
        /// <param name="tileTextures">The list of tile textures to draw the chunk to</param>
        /// <param name="chunkOffset">The offset of the chunk (based on which quadrant it is loaded into)</param>
        /// <param name="position">The position of the chunk in world coordinates</param>
        public void Draw(SpriteBatch spriteBatch, List<Texture2D> tileTextures, Vector2 chunkOffset, Vector2 position)
        {
            //TODO: make chunks only draw the tiles that appear on screen
            
            //Get the rectangle representing the size of the window
            Rectangle windowRect = ConsoleVars.GetInstance().WindowRect;
            //expand it so it will contain all tiles on screen
            windowRect.Inflate(TILE_WIDTH, TILE_HEIGHT);


            for (int y = 0; y < CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < CHUNK_WIDTH; x++)
                {
                    //Get the world position of the tile
                    Vector2 worldPos = new Vector2(x * TILE_WIDTH, y * TILE_HEIGHT)+ position;
                    //Get the screen position of the tile
                    Vector2 screenPos = Camera.ConvertToScreenCoords(worldPos);

                    //Is the tile on the screen?
                    if (windowRect.Contains(new Rectangle((int)screenPos.X, (int)screenPos.Y, 1, 1)))
                    {
                        //Draw the groundLayer tile
                        spriteBatch.Draw(tileTextures[chunkDataGroundLayer[x, y]], worldPos, Color.White);

                        //Draw the road layer tile
                        spriteBatch.Draw(tileTextures[chunkDataRoadLayer[x, y]], worldPos, new Color(255, 255, 255, 200));
                    }
                    //if (chunkDataOverLayer[x, y] > 0)
                    //{
                    //    spriteBatch.Draw(tileTextures[chunkDataOverLayer[x, y]], new Vector2(x * TILE_WIDTH, y * TILE_HEIGHT) + new Vector2(TILE_WIDTH, TILE_HEIGHT) * chunkOffset + position, new Color(255, 255, 255, 100));
                    //}
                }
            }
        }

        public short GetDataAtPosition(TileLayer tileLayer, Vector2 position)
        {
            switch (tileLayer)
            {
                case TileLayer.COLLISION_LAYER:
                    {
                        return chunkDataCollision[(int)position.X, (int)position.Y];
                    }
                case TileLayer.GROUND_LAYER:
                    {
                        return chunkDataGroundLayer[(int)position.X, (int)position.Y];
                    }
                case TileLayer.OVER_LAYER:
                    {
                        return chunkDataOverLayer[(int)position.X, (int)position.Y];
                    }
                case TileLayer.ROAD_LAYER:
                    {
                        return chunkDataRoadLayer[(int)position.X, (int)position.Y];
                    }
                default:
                    {
                        new Exception("Incorrect Layer Specified");
                        return -1;
                    }
            }
        }

    }
}
