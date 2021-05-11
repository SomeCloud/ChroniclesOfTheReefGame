using Microsoft.Xna.Framework;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class AEmptyPanel : APrimitive, IPrimitive
    {
        private IPrimitiveTexture Source;
        private Color _Color;
        public Color Color
        {
            get => _Color;
            set
            {
                _Color = value;
                Source.FillColor = value;
            }
        }

        public AEmptyPanel(ASize size) : base(size)
        {

        }

        public AEmptyPanel() : this(GraphicsExtension.DefaultPanelSize) { }

        public override void Initialize()
        {
            if (Collider is null) Collider = new ARectangleCollider(Size);
            else Collider.Size = Size;
            Texture = Source = new ARectangleTexture(GraphicsDevice, Size) { IsDraw = false, IsFill = false };
        }

        public override void OnLocationChangeProcess()
        {
            //nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }

        public new void Dispose()
        {
            Source.Dispose();
            base.Dispose();
        }

    }
}

