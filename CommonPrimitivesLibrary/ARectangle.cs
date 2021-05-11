using System;
using System.Collections.Generic;
using System.Text;

namespace CommonPrimitivesLibrary
{
    public struct ARectangle
    {

        public APoint Location;
        public APoint EndPoint;
        public ASize Size { get => (EndPoint - Location).ToASize(); }
        public ARectangle(APoint location, APoint endPoint) => (Location, EndPoint) = (location, endPoint);

        public ARectangle Intersect(ARectangle child)
        {

            int startX = Math.Max(child.Location.X, Location.X);
            int startY = Math.Max(child.Location.Y, Location.Y);
            int EndX = Math.Min(child.EndPoint.X, EndPoint.X);
            int EndY = Math.Min(child.EndPoint.Y, EndPoint.Y);

            return new ARectangle(new APoint(startX + (startX == Location.X ? 1 : 0), startY + (startY == Location.Y ? 1 : 0)), new APoint(EndX + (EndX == EndPoint.X ? -1 : 0), EndY + (EndY == EndPoint.Y ? -1 : 0)));

        }

    }
}
