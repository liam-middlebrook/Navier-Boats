using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Navier_Boats.Engine.Graphics.PostProcessing
{
    public class ShaderManager
    {
        #region SINGLETON_MEMEBERS
        private static ShaderManager _instance;
        public static ShaderManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ShaderManager();
            }
            return _instance;
        }
        #endregion

        private List<ShaderQuery> queries;

        public ShaderQuery this[int index]
        {
            get { return queries[index]; }
            set
            {
                if (queries.Count <= index)
                {
                    queries.Add(value);
                }
                else
                {
                    queries[index] = value;
                }
            }
        }

        private ShaderManager()
        {
            queries = new List<ShaderQuery>();
        }

        public void PostProcess(SpriteBatch spriteBatch)
        {
            foreach(ShaderQuery sQuery in queries)
            {
                spriteBatch.Begin(0, null, null, null, null, sQuery.Shader, sQuery.Matrix);

                foreach (Tuple<Texture2D, Vector2> drawQuery in sQuery.DrawQueries)
                {
                    spriteBatch.Draw(drawQuery.Item1, drawQuery.Item2, Color.White);
                }
                sQuery.DrawQueries.Clear();
                spriteBatch.End();
            }
        }
    }
}
