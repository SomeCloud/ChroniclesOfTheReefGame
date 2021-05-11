using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary
{
    public abstract class APrimitiveTexture: ATexture, IPrimitiveTexture, ITexture
    {

        private Color fillColor;
        private Color borderColor;
        private bool isDraw;
        private bool isFill;
        public Color FillColor
        {
            get => fillColor;
            set
            {
                if (!fillColor.Equals(value))
                {
                    FillColorReDraw(value);
                    fillColor = value;
                }
            }
        }
        public Color BorderColor
        {
            get => borderColor; 
            set { if (!borderColor.Equals(value)) { BorderColorReDraw(value); borderColor = value; } }
        }
        public bool IsDraw
        {
            get => isDraw;
            set { if (!isDraw.Equals(value)) { IsDrawChange(value); isDraw = value; } }
        }

        public bool IsFill
        {
            get => isFill;
            set { if (!isFill.Equals(value)) {  isFill = value; IsDrawChange(isDraw); } }
        }

        public APrimitiveTexture(GraphicsDevice graphicsDevice, ASize size): this(graphicsDevice, size, GraphicsExtension.DefaultFillColor, GraphicsExtension.DefaultBorderColor) { }
        public APrimitiveTexture(GraphicsDevice graphicsDevice, ASize size, Color fillColor, Color borderColor) : base(graphicsDevice, size) {

            this.fillColor = fillColor;
            this.borderColor = borderColor;

            IsDrawChange(IsDraw);

        }

        private void FillColorReDraw(Color color)
        {
            if (IsDraw)
            {
                DrawTextureBorder(BorderColor);
                if (isFill)
                {
                    FillTextureByColor(color);
                    DarkenTexture();
                }
            }
        }

        private void BorderColorReDraw(Color color)
        {
            if (IsDraw)
            {
                DrawTextureBorder(color);
                DarkenTexture();
            }
        }

        private void IsDrawChange(bool state)
        {
            if (state)
            {
                DrawTextureBorder(BorderColor);
                if (isFill)
                {
                    FillTextureByColor(FillColor);
                    DarkenTexture();
                }
            }
        }

        public abstract void FillTextureByColor(Color fillColor);
        public abstract void DrawTextureBorder(Color borderColor);

        public new void Dispose()
        {
            base.Dispose();
        }

    }
}
