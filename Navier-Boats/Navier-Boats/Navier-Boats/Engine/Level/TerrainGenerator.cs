using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;


namespace Navier_Boats.Engine.Level
{
    
    public enum TerrainType
    {
        Country, //For lack of a better term
        City,
    }

    /// <summary>
    /// Handles the data that will be passed to the Perlin functions as well as the type of generation (Countryside vs. City)
    /// </summary>
    public class TerrainGenerator
    {
        private TerrainType type;
        private PerlinGenerator perlinGen;
        private float lacuniarity;
        private int octaves;
        private int gridWidth;
        ColorConverter colorConv;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oct">Octaves of noise to use</param>
        /// <param name="lac">Rate of change of noise frequency</param>
        /// <param name="grid">Width of area to generate noise for</param>
        /// <param name="seed">Integer unique to each world</param>
        /// <param name="type">Type of generation to perform, defaults to Intracity if none is given</param>
        public TerrainGenerator(int oct, float lac, int grid, int seed, TerrainType type = TerrainType.Country)
        {
            this.type = type;
            this.octaves = oct;
            this.lacuniarity = lac;
            this.gridWidth = grid;
            perlinGen = new PerlinGenerator(seed);
            colorConv = new ColorConverter();
        }

        /// <summary>
        /// Generates the color via noise value of particular tile
        /// using Tile's world coordinates (tileX + (chunkX*CHUNK_WIDTH), tileY + (chunkY*CHUNK_HEIGHT))
        /// </summary>
        /// <param name="xPos">Tile X-Coord</param>
        /// <param name="yPos">Tile Y-Coord</param>
        /// <param name="chunkPos">Chunk position vector</param>
        /// <returns></returns>
        public short GenerateTile(int xPos, int yPos, Vector2 chunkPos)
        {
            int tileX = xPos + ((int)chunkPos.X * Chunk.CHUNK_WIDTH);
            int tileY = yPos + ((int)chunkPos.Y * Chunk.CHUNK_HEIGHT);

            switch (type)
            {
                case TerrainType.Country:
                    
                    float p = 1-Math.Abs(perlinGen.FBM2D(tileX/3, tileY/3, octaves, Chunk.CHUNK_WIDTH, lacuniarity));
                    
                    if (p >= 0.999f)
                        return 0; //Placeholder roads
                    else if (p < 0.999f && p >= 0.85f)
                        return 1; //Bright green grass
                    else if (p < 0.85f && p >= 0.815f)
                        return 2; //sand around lakes
                    else
                        return 3; //lakes & small bodies of water
                    
                 
                case TerrainType.City:
                    //City generation goes here
                    return 4; //Placeholder
                    
            }
            //Should never reach this with a default in the 
            //constructor unless something goes horribly wrong
            return -1; //Something dun goofed 
            
        }

        


        
    }
}
