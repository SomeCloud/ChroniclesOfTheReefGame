using Microsoft.Xna.Framework;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class ALabel : APrimitive, IPrimitive
    {
        private IPrimitiveTexture Source;

        public ALabel(ASize size) : base(size)
        {

        }

        public ALabel() : this(GraphicsExtension.DefaultTextLabelSize) { }

        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
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
