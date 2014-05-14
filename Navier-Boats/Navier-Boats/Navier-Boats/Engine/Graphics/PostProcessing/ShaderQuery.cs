using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Navier_Boats.Engine.Graphics.PostProcessing
{
    public class ShaderQuery
    {
        private Effect shader;
        private List<Tuple<Texture2D, Vector2>> drawQueries;

        public Effect Shader { get { return shader; } }

        public Matrix Matrix { get; set; }

        public List<Tuple<Texture2D, Vector2>> DrawQueries { get { return drawQueries; } }

        public Tuple<Texture2D,Vector2> this[int index]
        {
            get { return drawQueries[index]; }
            set { drawQueries[index] = value; }
        }

        public ShaderQuery(Effect shader)
        {
            this.drawQueries = new List<Tuple<Texture2D, Vector2>>();
            this.shader = shader;
            this.Matrix = Matrix.Identity;
        }
    }
}
