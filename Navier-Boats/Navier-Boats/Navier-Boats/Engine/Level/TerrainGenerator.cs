using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Navier_Boats.Engine.Level
{
    /// <summary>
    /// Handles the data that will be passed to the Perlin functions as well as the type of generation (Countryside vs. City)
    /// </summary>
    public class TerrainGenerator
    {
        private PerlinGenerator perlinGen;
        private float groundLacuniarity;
        private float waterLacuniarity;
        private int octaves;
        private int gridWidth;

        private Dictionary<string, Texture2D> roadPatterns;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oct">Octaves of noise to use</param>
        /// <param name="lac">Rate of change of noise frequency</param>
        /// <param name="grid">Width of area to generate noise for</param>
        /// <param name="seed">Integer unique to each world</param>
        /// <param name="type">Type of generation to perform, defaults to Intracity if none is given</param>
        public TerrainGenerator(int oct, float gLac, float wLac, int grid, int seed)
        {
            this.octaves = oct;
            this.groundLacuniarity = gLac;
            this.waterLacuniarity = wLac;
            this.gridWidth = grid;

            
            perlinGen = new PerlinGenerator(seed);
        }

        public void SetRoadPatterns(Dictionary<string, Texture2D> patterns)
        {
            this.roadPatterns = patterns;
        }

        public List<RoadConnectors> GenerateConnections(int numConnections)
        {
            Random rand = CurrentLevel.GetRandom();
            byte cnctType = (byte)rand.Next(8);
            List<RoadConnectors> connections = new List<RoadConnectors>();

            if (cnctType % 2 == 0)
            {
                if (cnctType == 0 || cnctType == 4)
                {
                    connections.Add(RoadConnectors.East);
                    connections.Add(RoadConnectors.West);
                }
                else
                {
                    connections.Add(RoadConnectors.North);
                    connections.Add(RoadConnectors.South);
                }
            }
            else
            {
                if (cnctType == 1 || cnctType == 5)
                {
                    connections.Add(RoadConnectors.NorthEast);
                    connections.Add(RoadConnectors.SouthWest);
                }
                else
                {
                    connections.Add(RoadConnectors.NorthWest);
                    connections.Add(RoadConnectors.SouthEast);
                }
            }
            return connections;
        }

        /// <summary>
        /// Generates the color via noise value of particular tile
        /// using Tile's world coordinates (tileX + (chunkX*CHUNK_WIDTH), tileY + (chunkY*CHUNK_HEIGHT))
        /// </summary>
        /// <param name="xPos">Tile X-Coord</param>
        /// <param name="yPos">Tile Y-Coord</param>
        /// <param name="chunkPos">Chunk position vector</param>
        /// <returns></returns>
        public short GenerateGroundTile(int xPos, int yPos, Vector2 loc, TerrainType type)
        {
            int tileX = xPos + ((int)loc.X * Chunk.CHUNK_WIDTH);
            int tileY = yPos + ((int)loc.Y * Chunk.CHUNK_HEIGHT);

            
            float groundMap = 1 - Math.Abs(perlinGen.Perlin2D(tileX / ((float)Chunk.CHUNK_WIDTH * 2), tileY / ((float)Chunk.CHUNK_HEIGHT * 2)));
            float lakeMap = fRound(1 - Math.Abs(perlinGen.FBM2D(tileX, tileY, octaves, Chunk.CHUNK_WIDTH, waterLacuniarity)));

//             if (groundMap >= 0.95f)
//                 return (short)TileType.Road; //Placeholder roads
//             else 
            if (groundMap >= 0.7)
                return (short)TileType.Grass; //Bright green grass
            else if (lakeMap == 1)
                return (short)TileType.Water;
            else
                return 1;
                 
        }

       

        private short[,] GenerateRoadLayer(List<RoadConnectors> connections)
        {
            return null;
        }
    

        private float fRound(float f)
        {
            return f >= 0.9 ? 1 : 0;
        }
    }
}
