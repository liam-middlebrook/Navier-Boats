using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Level
{
    class CurrentLevel
    {
        Chunk[] chunks;
            //Chunks[0] = Quadrant 1 or Top Right
            //Chunks[1] = Quadrant 2 or Top Left
            //Chunks[2] = Quadrant 3 or Bottom Left
            //Chunks[3] = Quadrant 4 or Bottom Right

        private string chunkSaveDirectory = "./LevelData";

        public CurrentLevel()
        {
            if (!Directory.Exists(chunkSaveDirectory))
            {
                Directory.CreateDirectory(chunkSaveDirectory);
            }
            chunks = new Chunk[4];

            if (File.Exists(Path.Combine(chunkSaveDirectory, "0_0.chunk"))
                && File.Exists(Path.Combine(chunkSaveDirectory, "0_-1.chunk"))
                && File.Exists(Path.Combine(chunkSaveDirectory, "-1_0.chunk"))
                && File.Exists(Path.Combine(chunkSaveDirectory, "-1_-1.chunk")))
            {
                chunks[0] = new Chunk("0_0.chunk", chunkSaveDirectory);
                chunks[1] = new Chunk("0_-1.chunk", chunkSaveDirectory);
                chunks[2] = new Chunk("-1_0.chunk", chunkSaveDirectory);
                chunks[3] = new Chunk("-1_-1.chunk", chunkSaveDirectory);
            }
            else
            {
                chunks[0] = new Chunk(0, 0);
                chunks[1] = new Chunk(-1, 0);
                chunks[2] = new Chunk(-1, -1);
                chunks[3] = new Chunk(0, -1);
            }
            foreach (Chunk chunk in chunks)
                {
                    Console.WriteLine(chunk.CHUNK_ID);
                    SaveChunk(chunk);
                }
        }

        private void SaveChunk(Chunk chunkToSave)
        {
            chunkToSave.Save(chunkSaveDirectory);
        }

        public void UpdateChunks(Vector2 playerPosition)
        {
            Vector2 chunkOffset = Vector2.Zero;
            Vector2 centerPosition = new Vector2(chunks[0].X * Chunk.CHUNK_WIDTH, chunks[0].Y * Chunk.CHUNK_HEIGHT);
            
            // Is the Player Outside of a Reasonable Range - What direction?
            if (Math.Abs(playerPosition.X - centerPosition.X) > Chunk.CHUNK_WIDTH / 2)
            {
                chunkOffset.X = (playerPosition.X > centerPosition.X) ? 1 : -1;
            }

            // Is the Player Outside of a Reasonable Range - What direction?
            if (Math.Abs(playerPosition.Y - centerPosition.Y) > Chunk.CHUNK_HEIGHT / 2)
            {
                chunkOffset.Y = (playerPosition.Y > centerPosition.Y) ? 1 : -1;
            }

            if (chunkOffset.X > 0)
            {
                Console.WriteLine("Removing Chunks in Quadrants 1 and 4");
                chunks[0] = chunks[1];
                chunks[3] = chunks[2];
            }
            else
            {
                Console.WriteLine("Removing Chunks in Quadrants 2 and 3");
                chunks[1] = chunks[0];
                chunks[2] = chunks[3];
            }
            if (chunkOffset.Y > 0)
            {
                Console.WriteLine("Removing Chunks in Quadrants 1 and 2");
                chunks[0] = chunks[3];
                chunks[1] = chunks[2];
            }
            else
            {
                Console.WriteLine("Removing Chunks in Quadrants 3 and 4");
                chunks[3] = chunks[0];
                chunks[2] = chunks[1];
            }
            
        }
    }
}
