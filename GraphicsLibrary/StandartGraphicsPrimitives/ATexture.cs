using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Reflection;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary
{
    public abstract class ATexture: Texture2D, ITexture, IDisposable
    {

        protected Texture2D _DarkenedTexture;

        public ASize Size { get => new ASize(Width, Height); }
        public Texture2D DarkenedTexture { get => _DarkenedTexture; }
        public Texture2D Texture { get => this; }

        public Texture2D RectangleTexture(ARectangle rectangle)
        {
            Color[] colors = new Color[Width * Height];
            Color[] newColors = new Color[Width * Height];

            Texture2D texture = new Texture2D(GraphicsDevice, Width, Height);

            GetData(colors);

            for (int i = rectangle.Location.Y; i < rectangle.EndPoint.Y; i++)
            {
                for (int j = rectangle.Location.X; j < rectangle.EndPoint.X; j++)
                {
                    newColors[i * Width + j] = colors[i * Width + j];
                }
            }

            texture.SetData(newColors);
            return texture;
        }

        public Texture2D RectangleDarkenedTexture(ARectangle rectangle)
        {
            Color[] colors = new Color[Width * Height];
            Color[] newColors = new Color[Width * Height];

            Texture2D texture = new Texture2D(GraphicsDevice, Width, Height);

            DarkenedTexture.GetData(colors); 
            
            for (int i = rectangle.Location.Y; i < rectangle.EndPoint.Y; i++)
            {
                for (int j = rectangle.Location.X; j < rectangle.EndPoint.X; j++)
                {
                    newColors[i * Width + j] = colors[i * Width + j];
                }
            }

            texture.SetData(newColors);
            return texture;
        }

        public Texture2D Clone()
        {
            return RectangleTexture(new ARectangle(new APoint(0, 0), new APoint(Width, Height)));
        }

        public void FillBySource(Texture2D texture)
        {
            Color[] colors = new Color[Width * Height];
            texture.GetData(colors);
            SetData(colors);
            DarkenTexture();
        }

        public ATexture(GraphicsDevice graphicsDevice, ASize size): base(graphicsDevice, size.Width, size.Height)
        {
            _DarkenedTexture = new Texture2D(graphicsDevice, Width, Height);
        }

        public void DrawLine(APoint A, APoint B, Color color) {
            Color[] colors = new Color[Width * Height];
            GetData(colors);

            //Изменения координат
            int dx = (B.X > A.X) ? (B.X - A.X) : (A.X - B.X);
            int dy = (B.Y > A.Y) ? (B.Y - A.Y) : (A.Y - B.Y);
            //Направление приращения
            int sx = (B.X >= A.X) ? (1) : (-1);
            int sy = (B.Y >= A.Y) ? (1) : (-1);

            if (dy < dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;
                colors[A.Y * Width + A.X] = color;
                int x = A.X + sx;
                int y = A.Y;
                for (int i = 1; i <= dx; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        y += sy;
                    }
                    else
                        d += d1;
                    colors[y * Width + x] = color;
                    x += sx;
                }
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;
                colors[A.Y * Width + A.X] = color;
                int x = A.X;
                int y = A.Y + sy;
                for (int i = 1; i <= dy; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        x += sx;
                    }
                    else
                        d += d1;
                    colors[y * Width + x] = color;
                    y += sy;
                }
            }

            SetData(colors);
        }

        public void FillPixels(APoint centerPoint, Color fillColor)
        {
            Color[] colors = new Color[Width * Height];
            GetData(colors);

            Stack<APoint> pixels = new Stack<APoint>();
            Color oldColor = colors[centerPoint.Y * Width + centerPoint.X];
            pixels.Push(centerPoint);

            while (pixels.Count > 0)
            {
                APoint a = pixels.Pop();
                if (a.X < Width && a.X > 0 && a.Y < Height && a.Y > 0)
                {
                    if (colors[a.Y * Width + a.X] == oldColor)
                    {
                        colors[a.Y * Width + a.X] = fillColor;
                        pixels.Push(new APoint(a.X - 1, a.Y));
                        pixels.Push(new APoint(a.X + 1, a.Y));
                        pixels.Push(new APoint(a.X, a.Y - 1));
                        pixels.Push(new APoint(a.X, a.Y + 1));
                    }
                }
            }
            SetData(colors);
        }

        protected void DarkenTexture()
        {
            _DarkenedTexture = new Texture2D(GraphicsDevice, Size.Width, Size.Height);
            Color[] rawData = new Color[Width * Height];
            GetData(rawData);
            for (int i = 0; i < rawData.Length; i++)
            {
                object Temp = rawData[i];
                foreach (string e in new[] { "R", "G", "B" })
                {
                    PropertyInfo prop = Temp.GetType().GetProperty(e);
                    var val = prop.GetValue(rawData[i]);
                    if ((byte)val > 32)
                    {
                        byte n = Convert.ToByte((byte)val - 32);
                        prop.SetValue(Temp, n, null);
                    }
                }
                rawData[i] = (Color)Temp;
            }
            _DarkenedTexture.SetData(rawData);
        }

        public void ReplacePixels(Color source, Color color)
        {
            Color[] rawData = new Color[Width * Height];
            GetData(rawData);

            for (int i = 0; i < rawData.Length; i++) if (rawData[i] == source) rawData[i] = color;

            SetData(rawData);
        }

        public new void Dispose()
        {
            if (!(_DarkenedTexture is null)) DarkenedTexture.Dispose();
            _DarkenedTexture = null;
            base.Dispose();
        }

    }
}
