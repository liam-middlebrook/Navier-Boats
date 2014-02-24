using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Navier_Boats.Engine.Graphics
{
    class Sprite
    {
        #region Fields

        private Texture2D texture;

        private Vector2 position;

        private Rectangle? sourceRectangle;

        private Color tintColor;

        private float rotation;

        private Vector2 rotationOrigin;

        private Vector2 scale;

        private SpriteEffects effects;

        private float depthLayer;

        #endregion

        #region Properties

        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                rotationOrigin = Center();
            }
        }

        public Vector2 Position { get { return position; } set { position = value; } }

        public Rectangle? SourceRectangle { get { return sourceRectangle; } set { sourceRectangle = value; } }

        public Color TintColor { get { return tintColor; } set { tintColor = value; } }

        public float Rotation { get { return rotation; } set { rotation = value; } }

        public Vector2 RotationOrigin { get { return rotationOrigin; } set { rotationOrigin = value; } }

        public Vector2 Scale { get { return scale; } set { scale = value; } }

        public SpriteEffects SpriteFX { get { return effects; } set { effects = value; } }

        public float DepthLayer { get { return depthLayer; } set { depthLayer = value; } }

        #endregion

        public Sprite()
        {
            this.texture = null;

            this.position = Vector2.Zero;

            this.sourceRectangle = null;

            this.tintColor = Color.White;

            this.rotation = 0.0f;

            this.rotationOrigin = Vector2.Zero;

            this.scale = Vector2.One;

            this.effects = SpriteEffects.None;

            this.depthLayer = 1.0f;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, tintColor, rotation, rotationOrigin, scale, effects, depthLayer);
        }

        public Vector2 Center()
        {
            Rectangle rect = sourceRectangle ?? texture.Bounds;

            return new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
    }
}
