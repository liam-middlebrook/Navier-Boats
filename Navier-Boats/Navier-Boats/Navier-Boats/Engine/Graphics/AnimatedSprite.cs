using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Navier_Boats.Engine.Graphics
{
    interface AnimatedSprite
    {
        private Rectangle[] animationFrameLocations;
        private int currentAnimationFrame;
        private Texture2D sourceTexture;

        protected Texture2D SourceTexture
        {
            get { return sourceTexture; }
            set { sourceTexture = value; }
        }

        protected int CurrentAnimationFrame
        {
            get { return currentAnimationFrame; }
            set { currentAnimationFrame = value; }
        }

        protected Rectangle[] AnimationFrameLocations
        {
            get { return animationFrameLocations; }
            set { animationFrameLocations = value; }
        }

    }
}
