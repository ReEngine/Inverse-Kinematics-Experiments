using System;
using SFML.Graphics;
using System.Collections.Generic;
using System.Text;

namespace Perlin_Noise
{
    class InverseKinematicsReacher
    {
        public readonly Segment[] segments;
        public Vector2F Base;
        float Length;
        public readonly float MaxLength;
        public Vector2F Target;
        public Vector2F TransferFrom;
        public Vector2F TransferTo;
        public Vector2F TransferPos;


        public InverseKinematicsReacher(float x, float y, float length, int segNum)
        {
            Length = length;
            segments = new Segment[segNum];
            Base = new Vector2F(x, y);
            segments[0] = new Segment(300, 200, Length);
            MaxLength = Length;
            for (int i = 1; i < segments.Length; i++)
            {
                segments[i] = new Segment(segments[i - 1], Length);
                MaxLength += Length;
            }


        }

        public void Transfer()
        {
            Vector2F transferDir;

            if (TransferTo != Target)
            {
                TransferTo = Target;
            }
            if ((Target - segments[segments.Length - 1].End).Length > 0.5)
            {
                TransferFrom = segments[segments.Length - 1].End;
                transferDir = (TransferFrom - TransferTo).Normalized;
                TransferPos = segments[segments.Length - 1].End;
                TransferPos -= transferDir * 1;
            }

        }

        public void Update()
        {
            int total = segments.Length;
            Segment end = segments[total - 1];
            Transfer();
            end.Follow(TransferPos.X, TransferPos.Y);
            end.Update();

            for (int i = total - 2; i >= 0; i--)
            {
                segments[i].Follow(segments[i + 1]);
                segments[i].Update();
            }
            segments[0].SetStart(Base);

            for (int i = 1; i < total; i++)
            {
                segments[i].SetStart(segments[i - 1].End);
            }


        }
        public void Show()
        {
            for (int i = 0; i < segments.Length - 1; i++)
                segments[i].Show(Color.Black);
            segments[segments.Length - 1].Show(Color.Red);
        }

    }
}
