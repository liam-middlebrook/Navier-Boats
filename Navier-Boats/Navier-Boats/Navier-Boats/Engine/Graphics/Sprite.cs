using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Navier_Boats.Engine.Graphics
{
    /// <summary>
    /// A Sprite That Will Be Drawn to the Screen
    /// </summary>
    class Sprite
    {
        #region Fields

        /// <summary>
        /// The Texture of the Sprite to draw to the screen
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// The Position to draw the Sprite to on the screen
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The Rectangle representing the texture coordinates (texels) to use while drawing the sprite to the screen
        /// <remarks>If null the entire texture will be drawn the the screen</remarks>
        /// </summary>
        private Rectangle? sourceRectangle;

        /// <summary>
        /// The color to tint the texture that will be drawn the the screen
        /// </summary>
        private Color tintColor;

        /// <summary>
        /// The angle of rotation in radians to rotate the Sprite
        /// </summary>
        private float rotation;

        /// <summary>
        /// The Position in Texture Coordinates (texels) to rotate the Sprite around
        /// </summary>
        private Vector2 rotationOrigin;

        /// <summary>
        /// The Amount to scale the Sprite by when drawing it to the screen
        /// </summary>
        private Vector2 scale;

        /// <summary>
        /// The SpriteEffects to drawn the Sprite to the screen with (Flip Horizontally AND OR Vertically)
        /// </summary>
        private SpriteEffects effects;

        /// <summary>
        /// The Depth or z-buffer layer to draw the sprite to the screen to
        /// </summary>
        private float depthLayer;

        #endregion

        #region Properties

        /// <summary>
        /// The Texture of the Sprite to draw to the screen
        /// <remarks>When setting the Texture the RotationOrigin of a Sprite is set to the Sprite's Center()</remarks>
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                rotationOrigin = Center();
            }
        }

        /// <summary>
        /// The Position to draw the Sprite to on the screen
        /// </summary>
        public Vector2 Position { get { return position; } set { position = value; } }

        /// <summary>
        /// The Rectangle representing the texture coordinates (texels) to use while drawing the sprite to the screen
        /// <remarks>If null the entire texture will be drawn the the screen</remarks>
        /// </summary>
        public Rectangle? SourceRectangle { get { return sourceRectangle; } set { sourceRectangle = value; } }

        /// <summary>
        /// The color to tint the texture that will be drawn the the screen
        /// </summary>
        public Color TintColor { get { return tintColor; } set { tintColor = value; } }

        /// <summary>
        /// The angle of rotation in radians to rotate the Sprite
        /// </summary>
        public float Rotation { get { return rotation; } set { rotation = value; } }

        /// <summary>
        /// The Position in Texture Coordinates (texels) to rotate the Sprite around
        /// </summary>
        public Vector2 RotationOrigin { get { return rotationOrigin; } set { rotationOrigin = value; } }

        /// <summary>
        /// The Amount to scale the Sprite by when drawing it to the screen
        /// </summary>
        public Vector2 Scale { get { return scale; } set { scale = value; } }

        /// <summary>
        /// The SpriteEffects to drawn the Sprite to the screen with (Flip Horizontally AND OR Vertically)
        /// </summary>
        public SpriteEffects SpriteFX { get { return effects; } set { effects = value; } }

        /// <summary>
        /// The Depth or z-buffer layer to draw the sprite to the screen to
        /// </summary>
        public float DepthLayer { get { return depthLayer; } set { depthLayer = value; } }

        #endregion

        /// <summary>
        /// Creates a new Sprite Object for Drawing A Texture to the Screen
        /// </summary>
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

        /// <summary>
        /// Draws the Sprite to the Screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw the Sprite to the screen with</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, tintColor, rotation, rotationOrigin, scale, effects, depthLayer);
        }

        /// <summary>
        /// The Position of the center point of the Texture in Texture Coordinates (texels)
        /// </summary>
        /// <returns>The Position of the center point of the Texture in Texture Coordinates (texels)</returns>
        public Vector2 Center()
        {
            Rectangle rect = sourceRectangle ?? texture.Bounds;

            return new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
    }
}
