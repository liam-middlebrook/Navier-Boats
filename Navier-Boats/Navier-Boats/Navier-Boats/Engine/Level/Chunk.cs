using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Navier_Boats.Engine.Level
{
    class Chunk
    {
        public const int CHUNK_WIDTH = 32;
        public const int CHUNK_HEIGHT = 32;

        public const int TILE_WIDTH = 32;
        public const int TILE_HEIGHT = 32;

        private string chunkDir;
        private short[,] chunkData;

        public readonly string CHUNK_ID; //format 1(xIndex-sign)1(yIndex-sign)15(xIndex-value)15(yIndex-value)
        public readonly Vector2 Position;

        private bool fileInUse;

        Random rand;
        private void CreateChunk(int x, int y)
        {
            //Currently fills a chunk with a random value between 1 and 4 (exclusive)
            //PLEASE FIX LATER - a nice grep friendly comment
            rand = new Random();
            short tileVal = (short)rand.Next(6);
            chunkData = new short[CHUNK_WIDTH, CHUNK_HEIGHT];
            for (int yIndex = 0; yIndex < CHUNK_HEIGHT; yIndex++)
            {
                for (int xIndex = 0; xIndex < CHUNK_WIDTH; xIndex++)
                {
                    chunkData[xIndex, yIndex] = (short)rand.Next(6);
                }
            }
            Save(chunkDir);
        }

        public Chunk(string fileName, string directory)
        {
            chunkDir = directory;
            chunkData = new short[CHUNK_WIDTH, CHUNK_HEIGHT];

            //Parse data to position
            CHUNK_ID = fileName.Remove(fileName.Length - 6);
            string[] location = CHUNK_ID.Split('_');
            Position = new Vector2(int.Parse(location[0]), int.Parse(location[1]));


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
                            chunkData[x, y] = br.ReadInt16();
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
                            br.Write(chunkData[x, y]);
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

        public void Draw(SpriteBatch spriteBatch, List<Texture2D> tileTextures, Vector2 chunkOffset, Vector2 position)
        {
            for (int y = 0; y < CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < CHUNK_WIDTH; x++)
                {
                    spriteBatch.Draw(tileTextures[chunkData[x, y]], new Vector2(x * TILE_WIDTH, y * TILE_HEIGHT) + new Vector2(TILE_WIDTH, TILE_HEIGHT) * chunkOffset + position, Color.White);
                }
            }
        }
    }
}
