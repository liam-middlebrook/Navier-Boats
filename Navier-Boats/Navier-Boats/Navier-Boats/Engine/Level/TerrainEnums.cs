using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Level
{
    public enum TileType
    {
        Road = 4,
        Grass = 1,
        Sand = 2,
        Water = 3,
        Clear = 0,
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
        North = 1,  
        West = 2,
        South = 3,
    }
}
