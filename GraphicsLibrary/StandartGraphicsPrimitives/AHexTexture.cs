using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary
{
    public class AHexTexture : APrimitiveTexture, IPrimitiveTexture, IDisposable
    {
        
        private APoint CenterPoint { get => Size.ToAPoint() / 2; }

        public AHexTexture(GraphicsDevice graphicsDevice, int radius) : base(graphicsDevice, radius % 2 == 0 ? new ASize(radius - 1, radius - 1) : new ASize(radius, radius)) { }
        public AHexTexture(GraphicsDevice graphicsDevice, int radius, Color fillColor, Color borderColor) : base(graphicsDevice, radius % 2 == 0 ? new ASize(radius - 1, radius - 1) : new ASize(radius, radius), fillColor, borderColor) { }


        public override void FillTextureByColor(Color fillColor)
        {

            FillPixels(CenterPoint, fillColor);

        }

        public override void DrawTextureBorder(Color borderColor)
        {
            Color[] colors = new Color[Width * Height];

            GetData(colors);

            APoint Last = new APoint();

            for (int i = 0; i < 6; i++)
            {
                APoint point = CornerPoint(CenterPoint, Width / 2, i);
                if (i < 1) DrawLine(CornerPoint(CenterPoint, Width / 2, 0), CornerPoint(CenterPoint, Width / 2, 5), borderColor);
                else DrawLine(point, Last, borderColor);
                Last = point;
            }

        }

        private APoint CornerPoint(APoint center, int size, int i)
        {
            var angle_deg = 60 * i + 60;
            var angle_rad = Math.PI / 180 * angle_deg;
            return new APoint(Convert.ToInt32(center.X + Math.Truncate(size * Math.Cos(angle_rad))), Convert.ToInt32(center.Y + Math.Truncate(size * Math.Sin(angle_rad))));
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
