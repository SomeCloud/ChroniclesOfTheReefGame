using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class AButton: APrimitive, IPrimitive
    {

        private IPrimitiveTexture Source;
        private Color _FillColor;
        private Color _BorderColor;
        public Color FillColor
        {
            get => _FillColor;
            set
            {
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

        public AButton(ASize size) : base(size)
        {
            Text = "";
            DragAndDrop = false;
            IsDarkened = true;
        }

        public AButton() : this(GraphicsExtension.DefaultButtonSize) { }
        public AButton(ASize size, Texture2D texture) : this(size) {
            Source = new ARectangleTexture(texture.GraphicsDevice, Size) { IsDraw = true, IsFill = true };
            Source.FillBySource(texture);
            Texture = Source;
        }

        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
            if (Source is null) Texture = Source = new ARectangleTexture(GraphicsDevice, Size) { IsDraw = true, IsFill = true };
        }

        public override void OnLocationChangeProcess()
        {
            //nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }


    }
}
