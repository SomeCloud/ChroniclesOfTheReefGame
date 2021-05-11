using Microsoft.Xna.Framework;

using System;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class AVerticalScrollbar : AScrollbar, IPrimitive
    {

        private AScrollbarSlider.OnMoveEvent MoveEvent;

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

        public new readonly bool DragAndDrop;
        public new readonly bool IsDarkened;
        public new readonly string Text;

        public AVerticalScrollbar(ASize size) : base(size)
        {
            base.DragAndDrop = false;
            MoveEvent = new AScrollbarSlider.OnMoveEvent(() => { /*Value = ProcessValue();*/ SetValue(ProcessValue()); });
        }
        public AVerticalScrollbar() : this(new ASize(GraphicsExtension.DefaultVerticalScrollbarSize.Height, GraphicsExtension.DefaultVerticalScrollbarSize.Width)) { }

        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
            Texture = Source = new ARectangleTexture(GraphicsDevice, Size) { IsDraw = true, IsFill = true };

            ScrollbarSlider = new AScrollbarSlider(AScrollbarAlign.Vertical) { Parent = this, Location = new APoint(2, 0) };

            ScrollbarSlider.MoveEvent += MoveEvent;

            /*ValueChange += (value) => {
                ScrollbarSlider.ForciblySetLocation(new APoint(2, ProcessLocation(value)));
            };*/

            base.Text = "";

        }

        public override void OnLocationChangeProcess()
        {
            //if (!(ScrollbarSlider is null)) Value = ProcessValue();
            if (!(ScrollbarSlider is null)) SetValue(ProcessValue());
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }

        protected override int ProcessValue()
        {
            //return MinValue + (MinValue < 0 ? -1 : 0) + Convert.ToInt32(Convert.ToDouble(ScrollbarSlider.Y + ScrollbarSlider.Height / 2) / Convert.ToDouble(Height - 20) * (MaxValue - MinValue));
            return Convert.ToInt32(MinValue + (float)ScrollbarSlider.Y / (float)(Height - ScrollbarSlider.Height) * (MaxValue - MinValue));
        }

        protected override int ProcessLocation(int value) => Convert.ToInt32(MinValue - value * ((float)(Height - ScrollbarSlider.Height) / (MaxValue - MinValue)));

        public new void Dispose()
        {
            Source.Dispose();
            ScrollbarSlider.MoveEvent -= MoveEvent;
            base.Dispose();
        }
    }
}
