using System;
using System.Collections.Generic;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary.Interfaces
{
    public interface ICollider: IDisposable
    {
        public ASize Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public APoint Location { get; set; }
        public int X { get; }
        public int Y { get; }
        public APoint CenterPoint { get; }
        public List<APoint> Points { get; }
        public ARectangle VisibleArea { get; set; }

        public abstract void UpdateColliderPoints(List<APoint> points);
        public bool InCollider(APoint cursorPosition, List<APoint> points);

    }
}
