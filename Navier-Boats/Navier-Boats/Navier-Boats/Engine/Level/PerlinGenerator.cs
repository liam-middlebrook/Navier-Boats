using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Level
{
    
    class PerlinGenerator
    {
       
        //Scalar look-up list. Will be randomized with a seed later
        private byte[] p; /*= {151,160,137,91,90,15,
             131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
             190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
             88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
             77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
             102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
             135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
             5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
             223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
             129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
             251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
             49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
             138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180, 
             151,160,137,91,90,15, 
             131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
             190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
             88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
             77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
             102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
             135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
             5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
             223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
             129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
             251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
             49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
             138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };*/
        
        //Array of vectors evenly distributed around a unit square
        private Vector2[] grads = {
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1),
            new Vector2(-1,1),
            new Vector2(-1,0),
            new Vector2(-1,-1),
            new Vector2(0,-1),
            new Vector2(1,-1)
        };



        public PerlinGenerator()
        {
            p = GenerateLookupTable();

        }


        /// <summary>
        /// Perlin's Curve function (6t^5 + 15t^4 + 10t^3)
        /// </summary>
        /// <param name="t"></param>
        /// <returns>t smoothed with Perlin's Curve function</returns>
        float Curve(float t)
        {
            return t * t * t * ((t * t * 6) - (t * 15) + 10);
        }

        /// <summary>
        /// Not actually a dot product, just scalar multiplication for 1D noise
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        float dot(Vector2 g, float x)
        {
            return g.X * x + g.Y * x;
        }

        /// <summary>
        /// Standard dot product of a Vector2 and two points
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        private float dot(Vector2 g, float x2, float y2)
        {
            return (g.X * x2) + (g.Y * y2);
        }

        

        /// <summary>
        /// 1D Perlin Noise algorithm
        /// </summary>
        /// <param name="x"></param>
        /// <returns>Scalar value between -1 and 1 at x</returns>
        private float Perlin1D(float x)
        {
            float xF = x - (float)Math.Floor(x);
            int xW = (int)x & 255;
            float xC = Curve(xF);

            int g1 = p[xW] % 8;
            int g2 = p[xW + 1] % 8;

            float d1 = dot(grads[g1], xF);
            float d2 = dot(grads[g2], xF-1);

            
            return MathHelper.Lerp(d1, d2, xF);

        }

        /// <summary>
        /// 2D Perlin Noise algorithm
        /// </summary>
        /// <param name="x">X-Coord, must have a decimal component</param>
        /// <param name="y">X-Coord, must have a decimal component</param>
        /// <returns>Scalar value between -1 and 1 at (x,y)</returns>
        public float Perlin2D(float x, float y)
        {
            float xN = x; 
            float yN = y; 

            int xW = (int)Math.Floor(xN);
            int yW = (int)Math.Floor(yN);

            float xF = xN - xW;
            float yF = yN - yW;

            float xC = Curve(xF);
            float yC = Curve(yF);

            xW &= 255;
            yW &= 255;

            int g1 = p[xW + p[yW]] % 8;
            int g2 = p[xW + p[yW + 1]] % 8;
            int g3 = p[xW + 1 + p[yW]] % 8;
            int g4 = p[xW + 1 + p[yW + 1]] % 8;

            float d1 = dot(grads[g1], xF, yF);
            float d2 = dot(grads[g2], xF, yF - 1);
            float d3 = dot(grads[g3], xF - 1, yF);
            float d4 = dot(grads[g4], xF - 1, yF - 1);

            float xL1 = MathHelper.SmoothStep(d1, d3, xC);
            float xL2 = MathHelper.SmoothStep(d2, d4, xC);

            return MathHelper.SmoothStep(xL1, xL2, yC);
        }

        /// <summary>
        /// Fractional Brownian Motion applied to 1D Perlin Noise
        /// </summary>
        /// <param name="x">Coordinate along x-axis</param>
        /// <param name="octs">Octaves of noise to generate</param>
        /// <param name="grid">Width of heightmap (powers of 2 work best)</param>
        /// <param name="lac">Change in frequency per octave</param>
        /// <returns></returns>
        public float FBM1D(float x, int octs, int grid, float lac)
        {
            float frq = 1.0f / grid;
            float gain = 1.0f / lac;
            float amp = gain;

            float total = 0.0f;
            for (int i = 0; i < octs; i++)
            {
                total += Perlin1D(x * frq) * amp;
                amp *= gain;
                frq *= lac;
            }
            
            return total;
        }

        /// <summary>
        /// Fractional Brownian Motion applied to 2D Perlin Noise
        /// </summary>
        /// <param name="x">X-Coord</param>
        /// <param name="y">Y-Coord</param>
        /// <param name="octs">Octaves of noise to generate</param>
        /// <param name="grid">Width of heightmap (powers of 2 work best)</param>
        /// <param name="lac">Change in frequency per octave</param>
        /// <returns></returns>
        public float FBM2D(int x, int y, int octs, int grid, float lac)
        {
            float frq = 1.0f / grid;
            float gain = 1.0f / lac;
            float amp = gain;

            float total = 0.0f;
            for (int i = 0; i < octs; i++)
            {
                total += Perlin2D(x * frq, y * frq) * amp;
                amp *= gain;
                frq *= lac;
            }
            return total;
        }

        private byte[] GenerateLookupTable()
        {
            byte[] arr = new byte[255];
            Random rnd = CurrentLevel.GetRandom();
            rnd.NextBytes(arr);

            byte[] arr2 = new byte[512];
            arr.CopyTo(arr2, 0);
            arr.CopyTo(arr2, 255);

            return arr2;
        }
    }
}
