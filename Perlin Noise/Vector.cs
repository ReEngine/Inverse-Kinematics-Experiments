using System;

namespace Perlin_Noise
{
    class Vector2F
    {
        public float X, Y;
        public Vector2F(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }


        public static Vector2F operator +(Vector2F vector1, Vector2F vector2) => new Vector2F(vector1.X + vector2.X, vector1.Y + vector2.Y);
        public static Vector2F operator -(Vector2F vector1, Vector2F vector2) => new Vector2F(vector1.X - vector2.X, vector1.Y - vector2.Y);
        public static Vector2F operator *(Vector2F vector, float scalar) => new Vector2F(vector.X * scalar, vector.Y * scalar);
        public static Vector2F operator /(Vector2F vector, float scalar) => new Vector2F(vector.X / scalar, vector.Y / scalar);
        public static float operator *(Vector2F vector1, Vector2F vector2) => (vector1.X * vector2.X + vector1.Y * vector2.Y);


        public float Length => (float)Math.Sqrt(X * X + Y * Y);

        //
        // Сводка:
        //     Returns sin of angles between this vector and another.
        public float Angle(Vector2F vector2) => (this * vector2) / ((this.Length) * (vector2.Length));
        public float Angle(Vector2F vector1, Vector2F vector2) => (vector1 * vector2) / ((vector1.Length) * (vector2.Length));
        public float Heading() => (float)-(Math.Atan2(0, 1) - Math.Atan2(Y, X));
        public Vector2F Normalized => this * (1 / Length);
        public void SetLength(float Length)
        {
            Vector2F tmp = this.Normalized;
            this.X = tmp.X *= Length;
            this.Y = tmp.Y *= Length;
        }


        public void Normalize()
        {
            this.X = this.Normalized.X;
            this.Y = this.Normalized.Y;
        }


    }

}
