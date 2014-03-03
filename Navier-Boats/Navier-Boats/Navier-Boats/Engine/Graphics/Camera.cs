using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Graphics
{
    class Camera
    {
        private Matrix transformMatrix;
        private static Rectangle screenSize = new Rectangle(0, 0, 1024, 1024);
        public Camera()
        {
            transformMatrix = new Matrix();
        }

        public Matrix TransformMatrix { get { return transformMatrix; } }
        
        public void Focus(Vector2 focalPoint)
        {
            Focus(focalPoint.X, focalPoint.Y);
        }

        public void Focus(float x, float y)
        {
            float cameraX = -x + screenSize.Width / 2.0f;
            float cameraY = -y + screenSize.Height / 2.0f;
            transformMatrix = Matrix.CreateTranslation(cameraX, cameraY, 1.0f);
        }


    }
}
