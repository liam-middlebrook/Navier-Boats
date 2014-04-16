using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Navier_Boats.Engine.Graphics
{
    public class Camera
    {
        public const float FollowMultiplier = 0.25f;

        private static Matrix transformMatrix = new Matrix();
        private static Matrix inverseTransformMatrix = new Matrix();
        private static Rectangle screenSize = new Rectangle(0, 0, 1024, 1024);
        private static bool followMouse = true;

        public static Matrix TransformMatrix { get { return transformMatrix; } }

        public static Matrix InverseTransformMatrix { get { return inverseTransformMatrix; } }

        public static bool FollowMouse { get { return followMouse; } set { followMouse = value; } }

        public static void Focus(Vector2 focalPoint)
        {
            Focus(focalPoint.X, focalPoint.Y);
        }

        public static void Focus(float x, float y)
        {
            float cameraX = -x + screenSize.Width / 2.0f;
            float cameraY = -y + screenSize.Height / 2.0f;

            if (FollowMouse)
            {
                MouseState mouse = Mouse.GetState();
                Vector2 mouseOffset = ConvertToScreenCoords(new Vector2(MathHelper.Clamp(mouse.X, 0, screenSize.Width), MathHelper.Clamp(mouse.Y, 0, screenSize.Width)));
                mouseOffset -= new Vector2(cameraX, cameraY);
                mouseOffset *= FollowMultiplier;

                cameraX += -mouseOffset.X + screenSize.Width / 8;
                cameraY += -mouseOffset.Y;
            }

            transformMatrix = Matrix.CreateTranslation(cameraX, cameraY, 1.0f);
            inverseTransformMatrix = Matrix.Invert(transformMatrix);
        }

        public static Vector2 ConvertToScreenCoords(Vector2 coords)
        {
            Vector3 newCoords = Vector3.Transform(new Vector3(coords,1), transformMatrix);
            return new Vector2(newCoords.X, newCoords.Y);
        }

    }
}
