using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Pathfinding
{
    public class PathException : Exception
    {
        public PathException()
            : base()
        {
        }

        public PathException(string message)
            : base(message)
        {
        }

        public PathException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
