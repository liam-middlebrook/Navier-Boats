using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Level
{
    class CurrentLevel
    {
        Chunk[] chunks;
            //Chunks[0] = Quadrant 1 or Top Right
            //Chunks[1] = Quadrant 2 or Top Left
            //Chunks[2] = Quadrant 3 or Bottom Left
            //Chunks[3] = Quadrant 4 or Bottom Right

        public CurrentLevel()
        {
            chunks = new Chunk[4];

                chunks[0] = new Chunk(0, 0);
                chunks[1] = new Chunk(-1, 0);
                chunks[2] = new Chunk(-1, -1);
                chunks[3] = new Chunk(0, -1);
                foreach (Chunk chunk in chunks)
                {
                    Console.WriteLine(chunk.CHUNK_ID);
                }
        }
    }
}
