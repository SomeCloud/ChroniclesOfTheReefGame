using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class APanel: APrimitive, IPrimitive
    {
        private IPrimitiveTexture Source;
        private Color _FillColor;
        private Color _BorderColor;

        private bool IsFilledConstructor;

        public Color FillColor
        {
            get => _FillColor;
            set {
                _FillColor = value;
                Source.FillColor = value;
            }
        }
        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                _BorderColor = value;
                Source.BorderColor = value;
            }
        }

        public APanel(ASize size): base(size)
        {

        }

        public APanel(): this(GraphicsExtension.DefaultPanelSize) { }
        public APanel(ASize size, Texture2D texture) : this(size)
        {
            Source = new ARectangleTexture(texture.GraphicsDevice, Size) { IsDraw = true, IsFill = true };
            Source.FillBySource(texture);
            Texture = Source;
        }

        public APanel(ASize size, Color fillColor) : this(size)
        {
            IsFilledConstructor = true;
            _FillColor = fillColor;
        }

        public APanel(ASize size, IPrimitiveTexture primitiveTexture) : this(size)
        {
            Texture = Source = primitiveTexture;
        }

        public override void Initialize()
        {

            Collider = new ARectangleCollider(Size);
            if (Source is null) Texture = Source = IsFilledConstructor ? new ARectangleTexture(GraphicsDevice, Size, _FillColor, _BorderColor) { IsDraw = true, IsFill = true } : new ARectangleTexture(GraphicsDevice, Size) { IsDraw = true, IsFill = true };

        }

        public override void OnLocationChangeProcess()
        {
            //nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }

        public void SetTexture(Texture2D texture) => Source.FillBySource(texture);

        public new void Dispose()
        {
            Source.Dispose();
            base.Dispose();
        }

    }
}
