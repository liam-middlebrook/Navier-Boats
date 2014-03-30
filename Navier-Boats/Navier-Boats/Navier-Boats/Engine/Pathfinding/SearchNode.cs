using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Pathfinding
{
    public class SearchNode
    {
        public PathNode Node
        {
            get;
            set;
        }

        public SearchNode Parent
        {
            get;
            set;
        }

        public bool InOpenList
        {
            get;
            set;
        }

        public bool InClosedList
        {
            get;
            set;
        }

        public bool InFinalPath
        {
            get;
            set;
        }

        public float DistanceToGoal
        {
            get;
            set;
        }

        public float DistanceTraveled
        {
            get;
            set;
        }

        public bool Equals(SearchNode obj)
        {
            return this.Node.Position.Equals(obj.Node.Position);
        }
    }
}
