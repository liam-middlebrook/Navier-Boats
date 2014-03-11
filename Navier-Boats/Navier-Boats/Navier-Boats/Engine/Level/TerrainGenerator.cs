using System;
using Microsoft.Xna.Framework;


namespace Navier_Boats.Engine.Level
{
    
    enum TerrainType
    {
        Country, //For lack of a better term
        City,
    }

    /// <summary>
    /// Handles the data that will be passed to the Perlin functions as well as the type of generation (Countryside vs. City)
    /// </summary>
    class TerrainGenerator
    {
        private TerrainType type;
        private PerlinGenerator perlinGen;
        private float lacuniarity;
        private float octaves;
        private int gridWidth;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oct">Octaves of noise to use</param>
        /// <param name="lac">Rate of change of noise frequency</param>
        /// <param name="grid">Width of area to generate noise for</param>
        /// <param name="seed">Integer unique to each world</param>
        /// <param name="type">Type of generation to perform, defaults to Intracity if none is given</param>
        public TerrainGenerator(float oct, float lac, int grid, int seed, TerrainType type = TerrainType.Country)
        {
            this.type = type;
            this.octaves = oct;
            this.lacuniarity = lac;
            this.gridWidth = grid;
            perlinGen = new PerlinGenerator(seed);
        }

        /// <summary>
        /// Generates the noise value for a particular tile
        /// using Tile's world coordinates (tileX + (chunkX*CHUNK_WIDTH), tileY + (chunkY*CHUNK_HEIGHT))
        /// </summary>
        /// <param name="xPos">Tile X-Coord</param>
        /// <param name="yPos">Tile Y-Coord</param>
        /// <param name="chunkPos">Chunk position vector</param>
        /// <returns></returns>
        public float GenerateTile(int xPos, int yPos, Vector2 chunkPos)
        {
            switch (type)
            {
                case TerrainType.City:
                    //City tile generation goes here
                    return 1.0f;
                 
                case TerrainType.Country:
                    //Road and ground generation goes here
                    //I figured out a way to do this with a single noisemap
                    return 0.0f;
                    
            }
            //Should never reach this with a default in the 
            //constructor unless something goes horribly wrong
            return 1.0f; 
            
        }
    }
}
