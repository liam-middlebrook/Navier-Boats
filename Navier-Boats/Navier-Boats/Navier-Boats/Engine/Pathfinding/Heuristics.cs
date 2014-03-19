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
        public static float Real(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X + b.X, 2) + Math.Pow(a.Y + b.Y, 2));
        }
    }
}
