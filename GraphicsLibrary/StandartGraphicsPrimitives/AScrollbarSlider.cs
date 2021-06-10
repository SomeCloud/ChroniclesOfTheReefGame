using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary
{
    public class AScrollbarSlider: APrimitive, IPrimitive
    {

        public delegate void OnMoveEvent();

        public event OnMoveEvent MoveEvent;

        private IPrimitiveTexture Source;
        private Color _Color;

        public new readonly string Text;
        public new readonly bool DragAndDrop;

        private AScrollbarAlign ScrollbarAlign;

        public new readonly bool IsDarkened;

        public Color Color
        {
            get => _Color;
            set
            {
                _Color = value;
                Source.FillColor = value;
            }
        }

        public AScrollbarSlider(AScrollbarAlign scrollbarAlign) : base(scrollbarAlign.Equals(AScrollbarAlign.Horizontal)? GraphicsExtension.DefaultHorizontalScrollbarSliderSize: GraphicsExtension.DefaultVerticalScrollbarSliderSize) { 

            ScrollbarAlign = scrollbarAlign;

        }

        public override void Initialize()
        {
            base.IsDarkened = true;

            Collider = new ARectangleCollider(Size);
            Texture = Source = new ARectangleTexture(GraphicsDevice, Size) { IsDraw = true, IsFill = true };

            base.DragAndDrop = true;
            base.Text = "";

        }

        public override void OnLocationChangeProcess()
        {
            switch (ScrollbarAlign)
            {
                case AScrollbarAlign.Horizontal:
                    SetLocation(new APoint(X > 2 ? X + Width < Parent.Width - 2 ? X : Parent.Width - Width - 2 : 2, 2));
                    break;
                case AScrollbarAlign.Vertical:
                    SetLocation(new APoint(2, Y > 2 ? Y + Height < Parent.Height - 2 ? Y : Parent.Height - Height - 2 : 2));
                    break;
            }
            MoveEvent?.Invoke();
        }

        //public void ForciblySetLocation(APoint location) => SetLocation(location);
        public void ForciblySetLocation(APoint location)
        {
            switch (ScrollbarAlign)
            {
                case AScrollbarAlign.Horizontal:
                    SetLocation(new APoint(location.X > 2 ? location.X + Width < Parent.Width - 2 ? location.X: Parent.Width - Width - 2 : 2, 2));
                    break;
                case AScrollbarAlign.Vertical:
                    SetLocation(new APoint(2, location.Y > 2 ? location.Y + Height < Parent.Height - 2 ? location.Y : Parent.Height - Height - 2 : 2));
                    break;
            }
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }
    }
}
