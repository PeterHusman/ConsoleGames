using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderEngine
{
    public class Vector2
    {
        public float X;
        public float Y;

        public Vector2()
        {

        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2 operator *(float left, Vector2 right)
        {
            return new Vector2(left * right.X, left * right.Y);
        }

        public static Vector2 operator *(Vector2 left, float right)
        {
            return right * left;
        }

        public static Vector2 operator -(Vector2 a)
        {
            return -1 * a;
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }

        public static float DotProduct(Vector2 left, Vector2 right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        public static Vector2 Zero => new Vector2(0, 0);

        public static Vector2 UnitX => new Vector2(1, 0);

        public static Vector2 UnitY => new Vector2(0, 1);

        public float SquareMagnitude => X * X + Y * Y;

        public bool IsNear(Vector2 point, float distance)
        {
            return (this - point).SquareMagnitude < distance * distance;
        }

        public Vector2 Copy => new Vector2(X, Y);
    }
}
