using System.Collections.Generic;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;

namespace GraphicsLibrary.StandartGraphicsPrimitives
{
    public class ARectangleCollider: ACollider, ICollider
    {

        public ARectangleCollider(ASize size) : base(size) { }

        public override void UpdateColliderPoints(List<APoint> points)
        {
            points.Clear();
            points.Add(VisibleArea.Location);
            points.Add(new APoint(VisibleArea.EndPoint.X, VisibleArea.Location.Y));
            points.Add(VisibleArea.EndPoint - 1);
            points.Add(new APoint(VisibleArea.Location.X, VisibleArea.EndPoint.Y));
        }

    }
}
