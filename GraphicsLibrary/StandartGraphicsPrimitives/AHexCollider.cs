using System;
using System.Collections.Generic;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;

namespace GraphicsLibrary.StandartGraphicsPrimitives
{
    public class AHexCollider: ACollider, ICollider
    {
        public AHexCollider(int radius) : base(radius % 2 == 0 ? new ASize(radius - 1, radius - 1) : new ASize(radius, radius)) { }

        public override void UpdateColliderPoints(List<APoint> points)
        {
            Points.Clear();
            for (int i = 0; i < 6; i++)
            {
                APoint point = CornerPoint(CenterPoint, Width / 2, i);
                Points.Add(point);
                //int x = point.X > VisibleArea.Location.X && point.X < VisibleArea.EndPoint.X ? point.X : point.X < VisibleArea.Location.X ? VisibleArea.Location.X : VisibleArea.EndPoint.X;
                //int y = point.Y > VisibleArea.Location.Y && point.Y < VisibleArea.EndPoint.Y ? point.Y : point.Y < VisibleArea.Location.Y ? VisibleArea.Location.Y : VisibleArea.EndPoint.Y;

                //Points.Add(new APoint(x, y));
            }
        }

        private APoint CornerPoint(APoint center, int size, int i)
        {
            var angle_deg = 60 * i + 60;
            var angle_rad = Math.PI / 180 * angle_deg;
            return new APoint(Convert.ToInt32(center.X + Math.Truncate(size * Math.Cos(angle_rad))), Convert.ToInt32(center.Y + Math.Truncate(size * Math.Sin(angle_rad))));
        }

    }
}
