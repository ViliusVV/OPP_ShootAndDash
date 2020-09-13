using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Utilities
{
    static class VectorUtils
    {
        // Calculate and return rotation between two vectors
        public static float GetAngleBetweenVectors(Vector2f center, Vector2f target)
        {
            double dx = target.X - center.X;
            double dy = target.Y - center.Y;

            return (float)((Math.Atan2(dy, dx)) * 180 / Math.PI);
        }

        // Convert unit vector to heading angle
        public static float VectorToAngle(Vector2f headingVector)
        {
            return VectorToAngle(headingVector.X, headingVector.Y);
        }

        public static float VectorToAngle(float x, float y)
        {
            return MathF.Atan2(y, x) * 180 / MathF.PI;
        }

        // Calculate and return middle point/vector between to vectors
        public static Vector2f GetMiddlePoint(Vector2f firstPoint, Vector2f secondPoint)
        {
            return (firstPoint + secondPoint) / 2.0f;
        }
    }
}
