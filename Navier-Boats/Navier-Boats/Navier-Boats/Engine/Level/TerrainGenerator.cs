using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;

namespace Navier_Boats.Engine.Level
{
    public enum TileType
    {
        Road = 0,
        Grass = 1,
        Sand = 2,
        Water = 3,
        City = 4,
    }


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
        private PerlinGenerator perlinGen;
        private float groundLacuniarity;
        private float waterLacuniarity;
        private int octaves;
        private int gridWidth;
        


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

        /// <summary>
        /// Generates the color via noise value of particular tile
        /// using Tile's world coordinates (tileX + (chunkX*CHUNK_WIDTH), tileY + (chunkY*CHUNK_HEIGHT))
        /// </summary>
        /// <param name="xPos">Tile X-Coord</param>
        /// <param name="yPos">Tile Y-Coord</param>
        /// <param name="chunkPos">Chunk position vector</param>
        /// <returns></returns>
        public short GenerateTile(int xPos, int yPos, Vector2 chunkPos, TerrainType type)
        {
            int tileX = xPos + ((int)chunkPos.X * Chunk.CHUNK_WIDTH);
            int tileY = yPos + ((int)chunkPos.Y * Chunk.CHUNK_HEIGHT);

            switch (type)
            {
                case TerrainType.Country:
                    
                    //float groundMap = 1-Math.Abs(perlinGen.FBM2D(tileX, tileY, octaves, Chunk.CHUNK_WIDTH, groundLacuniarity));

                    float groundMap = 1-Math.Abs(perlinGen.Perlin2D(tileX/((float)Chunk.CHUNK_WIDTH*2), tileY/((float)Chunk.CHUNK_HEIGHT*2)));
                    float lakeMap = fRound(1-Math.Abs(perlinGen.FBM2D(tileX, tileY, octaves, Chunk.CHUNK_WIDTH, waterLacuniarity)));

                    if (groundMap >= 0.95f)
                        return (short)TileType.Road; //Placeholder roads
                    else if (groundMap < 0.95f && groundMap >= 0.7)
                        return (short)TileType.Grass; //Bright green grass
                    else if (lakeMap == 1)
                        return (short)TileType.Water;
                    else
                        return 1;





                case TerrainType.City:
                    //City generation goes here
                    return 4; //Placeholder
                    
            }
            //Should never reach this with a default in the 
            //constructor unless something goes horribly wrong
            return -1; //Something dun goofed 
            
        }

        private float fRound(float f)
        {
            return f >= 0.9 ? 1 : 0;
        }
    }
}
