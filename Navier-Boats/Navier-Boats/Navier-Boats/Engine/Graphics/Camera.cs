using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Graphics
{
    class Camera
    {
        private static Matrix transformMatrix = new Matrix();
        private static Rectangle screenSize = new Rectangle(0, 0, 1024, 1024);

        public static Matrix TransformMatrix { get { return transformMatrix; } }

        public static void Focus(Vector2 focalPoint)
        {
            Focus(focalPoint.X, focalPoint.Y);
        }

        public static void Focus(float x, float y)
        {
            float cameraX = -x + screenSize.Width / 2.0f;
            float cameraY = -y + screenSize.Height / 2.0f;
            transformMatrix = Matrix.CreateTranslation(cameraX, cameraY, 1.0f);
        }

        public static Vector2 ConvertToScreenCoords(Vector2 coords)
        {
            Vector3 newCoords = Vector3.Transform(new Vector3(coords,1), transformMatrix);
            return new Vector2(newCoords.X, newCoords.Y);
        }

    }
}
