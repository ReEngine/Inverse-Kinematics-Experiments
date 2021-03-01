using System;
using SFML.Graphics;
using System.Collections.Generic;
using System.Text;

namespace Perlin_Noise
{
    class Segment
    {
        Vector2F Start;
        public Vector2F End;
        float Angle;
        public float Length;

        public Segment(float x, float y, float length)
        {
            Start = new Vector2F(x, y);
            Length = length;
            CalculateEnd();
        }
        public Segment(Segment parent, float length)
        {
            Start = parent.End;
            Length = length;
            CalculateEnd();
        }
        public void Follow(Segment child)
        {
            float targetX = child.Start.X;
            float targetY = child.Start.Y;
            Follow(targetX, targetY);
        }
        public void Follow(float tx, float ty)
        {
            Vector2F target = new Vector2F(tx, ty);
            Vector2F dir = target - Start;
            Angle = dir.Heading();
            dir.SetLength(Length);
            dir *= -1;
            Start = target + dir;
        }

        public void SetStart(Vector2F position)
        {
            Start = position;
            CalculateEnd();
        }
        public void CalculateEnd()
        {
            float dx = (float)(Length * Math.Cos(Angle));
            float dy = (float)(Length * Math.Sin(Angle));
            End = new Vector2F(Start.X + dx, Start.Y + dy);
        }
        public void Update() => CalculateEnd();
        public void Show(Color color)
        {
            Program.DrawLine((int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, color);
            Program.DrawLine((int)Start.X + 1, (int)Start.Y, (int)End.X, (int)End.Y, color);
            Program.DrawLine((int)Start.X - 1, (int)Start.Y, (int)End.X, (int)End.Y, color);
            Program.DrawLine((int)Start.X, (int)Start.Y + 1, (int)End.X, (int)End.Y, color);
            Program.DrawLine((int)Start.X, (int)Start.Y - 1, (int)End.X, (int)End.Y, color);
        }
    }
}
