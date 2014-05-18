using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Level
{
    public enum TileType
    {
        Clear = 0,
        Grass = 1,
        Sand = 2,
        Water = 3,
        DeepWater = 4,
        Road = 5,
        RoadCornerInside = 6,
        RoadCornerOutside = 7,
        RoadSide = 8,
        Debug = 9,
        
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

    

    public enum RoadConnector
    {
        East = 1,   
        North = 2,
        West = 4,
        South = 8,
    }

    public enum RoadCombination
    {
        NorthAndEast = 3,
        NorthAndWest = 6,
        SouthAndEast = 9,
        SouthAndWest = 12,

        NorthAndSouth = 10,
        EastAndWest = 5,
    }
}
