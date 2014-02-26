using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Navier_Boats.Engine.Level
{
    class Chunk
    {
        public const int CHUNK_WIDTH = 128;
        public const int CHUNK_HEIGHT = 128;

        private byte[,] chunkData;

        public readonly string CHUNK_ID; //format 1(xIndex-sign)1(yIndex-sign)15(xIndex-value)15(yIndex-value)
        public readonly int X, Y;
        public Chunk(int x, int y)
        {
            chunkData = new byte[CHUNK_WIDTH, CHUNK_HEIGHT];
            for (int yIndex = 0; yIndex < CHUNK_HEIGHT; yIndex++)
            {
                for (int xIndex = 0; xIndex < CHUNK_WIDTH; xIndex++)
                {
                    chunkData[xIndex, yIndex] = 65;
                }
            }

            CHUNK_ID = CoordsToChunkID(x, y);

            X = x;
            Y = y;
        }

        public Chunk(string fileName, string directory)
        {
            chunkData = new byte[CHUNK_WIDTH, CHUNK_HEIGHT];

            CHUNK_ID = fileName.Remove(fileName.Length-6);

            string[] location = CHUNK_ID.Split('_');

            X = int.Parse(location[0]);
            Y = int.Parse(location[1]);

            BinaryReader br = null;
            
            try
            {
                br = new BinaryReader(
                    File.OpenRead(
                    Path.Combine(directory, fileName)));

                //Read All Chunk Data From File - Line by Line
                for (int y = 0; y < CHUNK_HEIGHT; y++)
                {
                    for (int x = 0; x < CHUNK_WIDTH; x++)
                    {
                        chunkData[x, y] = br.ReadByte();
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
            }
        }

        public void Save(string directory)
        {
            BinaryWriter br = null;

            try
            {
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
            }
        }

        public static string CoordsToChunkID(int x, int y)
        {
            return string.Format("{0}_{1}", x, y);
        }
    }
}
