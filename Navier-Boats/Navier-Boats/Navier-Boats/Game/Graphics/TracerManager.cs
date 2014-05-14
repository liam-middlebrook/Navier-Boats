using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Navier_Boats.Game.Graphics
{
    public class TracerManager
    {
        public const float TRACER_SPEED = 75f;

        private class Tracer
        {
            public Vector2 startPosition;
            public Vector2 position;
            public float rotation;
            public float range;
        }

        private static TracerManager instance = new TracerManager();

        public static TracerManager GetInstance()
        {
            return instance;
        }

        private Texture2D tracerTexture = null;

        private LinkedList<Tracer> tracers = new LinkedList<Tracer>();

        private TracerManager()
        {
        }

        public void Initialize(Texture2D tracerTex)
        {
            tracerTexture = tracerTex;
        }

        public void AddTracer(Vector2 start, float rotation, float range)
        {
            tracers.AddLast(new Tracer()
                {
                    startPosition = start,
                    position = start,
                    rotation = rotation,
                    range = range * range
                });
        }

        public void Update(GameTime gameTime)
        {
            LinkedListNode<Tracer> node = tracers.First;
            while(node != null)
            {
                Tracer tracer = node.Value;
                tracer.position.X += TRACER_SPEED * (float)Math.Cos(tracer.rotation);
                tracer.position.Y += TRACER_SPEED * (float)Math.Sin(tracer.rotation);
                
                if(Vector2.DistanceSquared(tracer.startPosition, tracer.position) > tracer.range)
                {
                    tracers.Remove(node);
                }

                node = node.Next;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tracer tracer in tracers)
            {
                spriteBatch.Draw(tracerTexture, tracer.position, null, Color.White, tracer.rotation, new Vector2(tracerTexture.Bounds.Center.X, tracerTexture.Bounds.Center.Y), 1f, SpriteEffects.None, 0);
            }
        }
    }
}
