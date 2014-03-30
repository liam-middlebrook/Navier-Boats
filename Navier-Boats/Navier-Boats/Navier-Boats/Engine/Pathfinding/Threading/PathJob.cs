using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Pathfinding.Threading
{
    public class PathJob
    {
        public Vector2 Start
        {
            get;
            set;
        }

        public Vector2 End
        {
            get;
            set;
        }

        public Pathfinder.Heuristic Heuristic
        {
            get;
            set;
        }

        public float NodeSize
        {
            get;
            set;
        }

        public float MaxTime
        {
            get;
            set;
        }

        public PathThreadPool.JobCallback Callback
        {
            get;
            set;
        }
    }
}
