using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary
{
    public class ARectangleTexture: APrimitiveTexture, IPrimitiveTexture, IDisposable
    {

        public ARectangleTexture(GraphicsDevice graphicsDevice, ASize size) : base(graphicsDevice, size) { }
        public ARectangleTexture(GraphicsDevice graphicsDevice, ASize size, Color fillColor, Color borderColor) : base(graphicsDevice, size, fillColor, borderColor) { }

        public override void FillTextureByColor(Color fillColor)
        {
            Color[] colors = new Color[Width * Height];

            GetData(colors);

            for (int i = 1; i < Height - 1; i++)
            {
                for (int j = 1; j < Width - 1; j++)
                {
                    colors[i * Width + j] = fillColor;
                }
            }

            SetData(colors);
        }

        public override void DrawTextureBorder(Color borderColor)
        {

            Color[] colors = new Color[Width * Height];

            GetData(colors);

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == 0 || i == Height - 1)
                    {
                        if (j > 0 && j < Width - 1) colors[i * Width + j] = borderColor;
                    }
                    else if ((i > 0 || i < Height - 1) && (j == 0 || j == Width - 1))
                    {
                        colors[i * Width + j] = borderColor;
                        colors[(i + 1) * Width - 1] = borderColor;
                    }
                    else
                    {
                        break;
                    }
                }

                SetData(colors);
            }
        }

        public new void Dispose()
        {
            base.Dispose();
        }

    }
}
