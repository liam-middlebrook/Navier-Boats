﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Engine.Pathfinding
{
    public static class Heuristics
    {
        // manhattan distance
        public static float Manhattan(Vector2 current, Vector2 end, float resistance)
        {
            float inverse = resistance == 0 ? 1 : 1f / resistance;
            return (Math.Abs(current.X - end.X) + Math.Abs(current.Y - end.Y)) * inverse;
        }

        // real distance
        // WARNING: very slow due to square root
        public static float Distance(Vector2 current, Vector2 end, float resistance)
        {
            float inverse = resistance == 0 ? 1 : 1f / resistance;
            return Vector2.Distance(current, end) * inverse;
        }

        // squared distance
        public static float DistanceSquared(Vector2 current, Vector2 end, float resistance)
        {
            float inverse = resistance == 0 ? 1 : 1f / resistance;
            return Vector2.DistanceSquared(current, end) * inverse;
        }

        // Dijkstra's, or: let's be lazy and return 0
        // Do not actually use this one, as it simply flood-fills everything until it finds the end node.
        // There are uses for this heuristic, but the current pathfinder implementation does not lend itself
        // to them.
        public static float Dijkstras(Vector2 current, Vector2 end, float resistance)
        {
            return 0;
        }

        public static Pathfinder.Heuristic MultiplyResistance(float multiplier, Pathfinder.Heuristic heuristic)
        {
            return (current, end, resistance) =>
                {
                    return heuristic(current, end, resistance * multiplier);
                };
        }

        public static Pathfinder.Heuristic ResistanceRandomness(double min, double max, Pathfinder.Heuristic heuristic)
        {
            return (current, end, resistance) =>
                {
                    float res = (float)(CurrentLevel.GetRandom().NextDouble() * (max - min) + min) + resistance;
                    return heuristic(current, end, res);
                };
        }
    }
}
