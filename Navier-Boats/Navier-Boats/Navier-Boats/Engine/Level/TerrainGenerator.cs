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
        public TerrainGenerator(int oct, float gLac, float wLac, int grid)
        {
            this.octaves = oct;
            this.groundLacuniarity = gLac;
            this.waterLacuniarity = wLac;
            this.gridWidth = grid;
        }

        public void Init()
        {
            perlinGen = new PerlinGenerator();
        }

        public void SetRoadPatterns(Dictionary<string, Texture2D> patterns)
        {
            this.roadPatterns = patterns;
        }

        public List<RoadConnector> GenerateConnections(Chunk c, int numConnections, ref byte mask)
        {
            Random rand = CurrentLevel.GetRandom();
            List<Chunk> adjChunks = CurrentLevel.GetInstance().GetAdjacentChunks(c);
            List<RoadConnector> rc = new List<RoadConnector>();
            
            

            foreach (Chunk c2 in adjChunks)
            {
                if (c2.Connections == null || c2.Connections.Count == 0)
                    continue;

                int xDiff = (int)(c.Position.X - c2.Position.X);
                int yDiff = (int)(c.Position.Y - c2.Position.Y);

                if (xDiff == 1 && yDiff == 0) //Left
                {
                    if ((mask & (byte)RoadCombination.EastAndWest) == (byte)RoadConnector.East || rc.Count == 0)
                    {
                        rc.Add(RoadConnector.West);
                        mask |= (byte)RoadConnector.West;
                    }
                }
                else if (xDiff == -1 && yDiff == 0) //Right
                {
                    if ((mask & (byte)RoadCombination.EastAndWest) == (byte)RoadConnector.West || rc.Count == 0)
                    {
                        rc.Add(RoadConnector.East);
                        mask |= (byte)RoadConnector.East;
                    }
                }
                else if (xDiff == 0 && yDiff == -1) //Up
                {
                    if ((mask & (byte)RoadCombination.NorthAndSouth) == (byte)RoadConnector.South || rc.Count == 0)
                    {
                        rc.Add(RoadConnector.North);
                        mask |= (byte)RoadConnector.North;
                    }
                }
                else if (xDiff == 0 && yDiff == 1) //Down
                {
                    if ((mask & (byte)RoadCombination.NorthAndSouth) == (byte)RoadConnector.North || rc.Count == 0)
                    {
                        rc.Add(RoadConnector.South);
                        mask |= (byte)RoadConnector.South;
                    }
                }
            }

            numConnections--;
            while (numConnections > 0)
            {
                RoadConnector cn;
                while (rc.Contains(cn = (RoadConnector)Enum.GetValues(typeof(RoadConnector)).GetValue(rand.Next(4))));
                rc.Add(cn);
                mask |= (byte)cn;
                numConnections--;
            }

            return rc;
            
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

       

        public short[,] GenerateRoadLayer(List<RoadConnector> connections)
        {
            short[,] layer = new short[Chunk.CHUNK_WIDTH, Chunk.CHUNK_HEIGHT];
            if (connections.Count == 0)
            {
                for (int x = 0; x < Chunk.CHUNK_WIDTH; x++)
                {
                    for (int y = 0; y < Chunk.CHUNK_HEIGHT; y++)
                    {
                        layer[x, y] = (short)TileType.Clear;
                    }
                }
            }
            else
            {
                layer = GenRoadPattern(connections);
                
            }
            return layer;
        }

        private short[,] GenRoadPattern(List<RoadConnector> rc)
        {
            short[,] pattern = new short[Chunk.CHUNK_WIDTH, Chunk.CHUNK_HEIGHT];
            foreach (RoadConnector c in rc)
            {
                string name = Enum.GetName(typeof(RoadConnector), c);
                MergeArrays(ToArray(roadPatterns[Enum.GetName(typeof(RoadConnector), c)]), ref pattern);
            }
            return pattern;
        }

        private short[,] ToArray(Texture2D tex)
        {
            Color roadColor = new Color(72, 77, 62, 255);
            Color edgeColor = new Color(0, 255, 0, 255);
            short[,] chunk = new short[Chunk.CHUNK_WIDTH, Chunk.CHUNK_HEIGHT];

            for (int x = 0; x < Chunk.CHUNK_WIDTH; x++)
            {
                for (int y = 0; y < Chunk.CHUNK_HEIGHT; y++)
                {
                    Color[] c = new Color[1];
                    tex.GetData<Color>(0, new Rectangle(x, y, 1, 1), c, 0, 1);
                    if (c[0] == roadColor)
                    {
                        chunk[x, y] = (short)TileType.Road;
                    }
                    else if (c[0] == edgeColor)
                    {
                        chunk[x, y] = (short)TileType.RoadSide;
                    }
                    else
                    {
                        chunk[x, y] = (short)TileType.Clear;
                    }
                    
                }
            }
            
            return chunk;
        }

        public short IsCorner(int x, int y, short[,] arr, List<RoadConnector> cn)
        {
            if (x == 0 || x == arr.GetLength(0) - 1 || y == 0 || y == arr.GetLength(0) - 1 || cn == null) return -1;
            short[] res = new short[4];

            res[0] = arr[x, y + 1];
            res[1] = arr[x - 1, y];
            res[2] = arr[x + 1, y];
            res[3] = arr[x, y - 1];

            int C = 0;
            bool r = false;
            foreach (short s in res)
            {
                if (s == (short)TileType.RoadSide)
                {
                    C++;
                }
                if (s == (short)TileType.Road)
                {
                    r = true;
                }
            }

            if (C == 2 && r)
            {
                if (res[0] == (short)TileType.RoadSide && res[1] == (short)TileType.RoadSide)
                {
                    if (cn.Contains(RoadConnector.West) && cn.Contains(RoadConnector.South))
                        return (short)TileType.RoadCornerInside;
                    else
                        return (short)TileType.RoadCornerOutside;
                }
                else if (res[0] == (short)TileType.RoadSide && res[2] == (short)TileType.RoadSide)
                {
                    if (cn.Contains(RoadConnector.East) && cn.Contains(RoadConnector.South))
                        return (short)TileType.RoadCornerInside;
                    else
                        return (short)TileType.RoadCornerOutside;
                }
                else if (res[3] == (short)TileType.RoadSide && res[1] == (short)TileType.RoadSide)
                {
                    if (cn.Contains(RoadConnector.West) && cn.Contains(RoadConnector.North))
                        return (short)TileType.RoadCornerInside;
                    else
                        return (short)TileType.RoadCornerOutside;
                }
                else if (res[3] == (short)TileType.RoadSide && res[2] == (short)TileType.RoadSide)
                {
                    if (cn.Contains(RoadConnector.East) && cn.Contains(RoadConnector.North))
                        return (short)TileType.RoadCornerInside;
                    else
                        return (short)TileType.RoadCornerOutside;
                }
            }

            return -1;
        }

        public float GetTileRotation(int x, int y, short type, short[,] arr, List<RoadConnector> cn)
        {
            if (x == 22 && y == 22)
            {
                if (type == (short)TileType.RoadCornerInside)
                    return (float)Math.PI;
                else if (type == (short)TileType.RoadCornerOutside)
                    return 0;
                else
                    return (float)Math.PI;
            }
            else if (x == 25 && y == 22)
            {
                if (type == (short)TileType.RoadCornerInside)
                    return (float)-Math.PI / 2f;
                else if (type == (short)TileType.RoadCornerOutside)
                    return (float)-Math.PI / 2f;
                else return (float)Math.PI;
            }
            else if (x == 22 && y == 25)
            {
                if (type == (short)TileType.RoadCornerInside)
                    return (float)Math.PI / 2f;
                else if (type == (short)TileType.RoadCornerOutside)
                    return (float)-Math.PI / 2f;
                else return (float)Math.PI / 2f;
            }
            else if (x == 25 && y == 25)
            {
                if (type == (short)TileType.RoadCornerInside)
                    return 0;
                else if (type == (short)TileType.RoadCornerOutside)
                    return (float)Math.PI;
                else return (float)-Math.PI / 2f;
            }

            if (IsCorner(x, y, arr, cn) == -1)
            {
                if (x == 22) return (float)(Math.PI / 2f);
                if (y == 22) return (float)(Math.PI);
                if (y == 25) return 0;
                else if (x == 25) return (float)(-Math.PI / 2f);
            }
            
            
            return 0;
        }

        private void MergeArrays(short[,] from, ref short[,] to)
        {
            for (int x = 0; x < to.GetLength(0); x++)
            {
                for (int y = 0; y < to.GetLength(0); y++)
                {
                    if (from[x, y] != (short)TileType.Clear && to[x, y] != (short)TileType.Road)
                    {
                        to[x, y] = from[x, y];
                    }
                }
            }
        }


        private float fRound(float f)
        {
            return f >= 0.9 ? 1 : 0;
        }
    }
}
