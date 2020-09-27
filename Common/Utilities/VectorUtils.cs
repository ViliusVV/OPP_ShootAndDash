using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utilities
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

        public static Vector2f ToUnit(Vector2f vec)
        {
            float magnitude = (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);

            return new Vector2f(vec.X / magnitude, vec.Y / magnitude);
        }

        public static Vector2f ConstantLenVector(Vector2f vec, float len)
        {
            return ToUnit(vec) * len;
        }


        // Get intersection point between two lines
        public static Vector2f GetIntersection(Vector2f line1Start, Vector2f line1End, Vector2f line2Start, Vector2f line2End)
        {
            var x1 = line2Start.X;
            var y1 = line2Start.Y;
            var x2 = line2End.X;
            var y2 = line2End.Y;

            var x3 = line1Start.X;
            var y3 = line1Start.Y;
            var x4 = line1End.X;
            var y4 = line1End.Y;

            var den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (den == 0)
            {
                return new Vector2f(float.NaN, float.NaN);
            }

            var t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
            var u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;
            if (t > 0 && t < 1 && u > 0)
            {
                return new Vector2f(x1 + t * (x2 - x1), y1 + t * (y2 - y1));
            }
            else
            {
                return new Vector2f(float.NaN, float.NaN);
            }
        }

        public static Vector2f AngleDegToUnitVector(float degrees)
        {
            return AngleRadToUnitVector(RadToDeg(degrees));
        }

        public static Vector2f AngleRadToUnitVector(float radians)
        {
            return new Vector2f(MathF.Cos(radians), MathF.Sin(radians));
        }

        public static float RadToDeg(float radians)
        {
            return radians * (180 / MathF.PI);
        }

        public static float GetSquaredDistance(Vector2f start, Vector2f end)
        {
            return (float)(Math.Pow(start.X - end.X, 2) + Math.Pow(end.Y - end.Y, 2));
        }

        public static float GetSquaredDistance(float x1, float y1, float x2, float y2)
        {
            return (float)(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}
