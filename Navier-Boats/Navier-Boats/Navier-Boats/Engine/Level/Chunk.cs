using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Level
{
    class Chunk
    {
        const int CHUNK_WIDTH = 128;
        const int CHUNK_HEIGHT = 128;

        public readonly string CHUNK_ID; //format 1(x-sign)1(y-sign)15(x-value)15(y-value)

        public Chunk(int x, int y)
        {
            CHUNK_ID = string.Format("{0}_{1}", x, y);
        }
    }
}
