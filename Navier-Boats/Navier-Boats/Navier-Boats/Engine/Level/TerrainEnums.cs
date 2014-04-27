using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Level
{
    public enum TileType
    {
        Road = 0,
        Grass = 1,
        Sand = 2,
        Water = 3,
        Clear = 4,
        City = 5,
        Debug = 6,
        
    }

    public enum TerrainType
    {
        Country, //For lack of a better term
        Road,
        City,
    }

    public enum TileLayer
    {
        GROUND_LAYER,
        ROAD_LAYER,
        OVER_LAYER,
        COLLISION_LAYER
    }

    public enum RoadConnectors
    {
        East = 0,
        NorthEast = 1,
        North = 2,
        NorthWest = 3,
        West = 4,
        SouthWest = 5,
        South = 6,
        SouthEast = 7,
    }
}
