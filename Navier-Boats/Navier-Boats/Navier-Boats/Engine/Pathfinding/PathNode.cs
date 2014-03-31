using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Pathfinding
{
    public class PathNode
    {
        public Vector2 Position
        {
            get;
            set;
        }

        public bool Walkable
        {
            get;
            set;
        }

        public float Resistance
        {
            get;
            set;
        }
    }
}
