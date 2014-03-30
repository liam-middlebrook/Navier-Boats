﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Pathfinding.Threading
{
    public class PathResult
    {
        public List<Vector2> Path
        {
            get;
            set;
        }

        public PathException Error
        {
            get;
            set;
        }

        public TimeSpan ExecuteTime
        {
            get;
            set;
        }

        public PathResult()
        {
            Path = null;
            Error = null;
        }
    }
}
