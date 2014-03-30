using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Pathfinding
{
    public static class Heuristics
    {
        // manhattan distance
        public static float Manhattan(Vector2 a, Vector2 b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        // real distance
        // WARNING: very slow due to square root
        public static float Distance(Vector2 a, Vector2 b)
        {
            return Vector2.Distance(a, b);
        }

        // squared distance
        public static float DistanceSquared(Vector2 a, Vector2 b)
        {
            return Vector2.DistanceSquared(a, b);
        }

        // Dijkstra's, or: let's be lazy and return 0
        // Do not actually use this one, as it simply flood-fills everything until it finds the end node.
        // There are uses for this heuristic, but the current pathfinder implementation does not lend itself
        // to them.
        public static float Dijkstras(Vector2 a, Vector2 b)
        {
            return 0;
        }
    }
}
