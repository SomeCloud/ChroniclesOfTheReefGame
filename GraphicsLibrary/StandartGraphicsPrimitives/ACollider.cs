using System;
using System.Collections.Generic;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary.StandartGraphicsPrimitives
{
    public abstract class ACollider: IDisposable
    {

        private ASize _Size;
        private APoint _Location;
        private List<APoint> _Points;
        private ARectangle _VisibleArea;

        public ASize Size
        {
            get => _Size;
            set
            {
                _Size = value;
                UpdateColliderPoints(_Points);
            }
        }
        public int Width
        {
            get => _Size.Width;
            set
            {
                _Size.Width = value;
                UpdateColliderPoints(_Points);
            }
        }
        public int Height
        {
            get => _Size.Height;
            set
            {
                _Size.Height = value;
                UpdateColliderPoints(_Points);
            }
        }
        public APoint Location
        {
            get => _Location;
            set
            {
                _Location = value;
                UpdateColliderPoints(_Points);
            }
        }
        public int X
        {
            get => _Location.X;
            set
            {
                _Location.X = value;
                UpdateColliderPoints(_Points);
            }
        }
        public int Y
        {
            get => _Location.Y;
            set
            {
                _Location.Y = value;
                UpdateColliderPoints(_Points);
            }
        }
        public APoint CenterPoint { get => Location + (Size / 2).ToAPoint(); }
        public List<APoint> Points { get => _Points; }
        public ARectangle VisibleArea
        {
            get => _VisibleArea;
            set
            {
                _VisibleArea = value;
                UpdateColliderPoints(_Points);
            }
        }

        public ACollider(ASize size)
        {

            _Points = new List<APoint>();

            Location = new APoint();
            Size = size;

        }

        public abstract void UpdateColliderPoints(List<APoint> points);

        public bool InCollider(APoint cursorPosition, List<APoint> points)
        {
            bool c = false;
            for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
            {
                if ((((points[i].Y <= cursorPosition.Y) && (cursorPosition.Y < points[j].Y)) || ((points[j].Y <= cursorPosition.Y) && (cursorPosition.Y < points[i].Y))) &&
                  (((points[j].Y - points[i].Y) != 0) && (cursorPosition.X > ((points[j].X - points[i].X) * (cursorPosition.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))))
                    c = !c;
            }
            return c;
        }

        public void Dispose()
        {
            _Points.Clear();
        }

    }
}
