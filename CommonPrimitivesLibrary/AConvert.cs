using System;
using System.Collections.Generic;
using System.Text;

namespace CommonPrimitivesLibrary
{
    public class AConvert
    {

        public APoint ToAPoint(ASize size) => new APoint(size.Width, size.Height);
        public ASize ToASize(APoint point) => new ASize(point.X, point.Y);

    }
}
