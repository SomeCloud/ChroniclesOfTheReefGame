using Microsoft.Xna.Framework;

using System;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class AHorizontalScrollbar: AScrollbar, IPrimitive
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

        public AHorizontalScrollbar(ASize size) : base(size)
        {
            base.DragAndDrop = false;
            MoveEvent = new AScrollbarSlider.OnMoveEvent(() => { /*Value = ProcessValue();*/ SetValue(ProcessValue()); });
        }
        public AHorizontalScrollbar() : this(GraphicsExtension.DefaultHorizontalScrollbarSize) { }

        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
            Texture = Source = new ARectangleTexture(GraphicsDevice, Size) { IsDraw = true, IsFill = true };

            ScrollbarSlider = new AScrollbarSlider(AScrollbarAlign.Horizontal) { Parent = this, Location = new APoint(0, 2) };

            ScrollbarSlider.MoveEvent += MoveEvent;

            base.Text = "";

        }

        public override void OnLocationChangeProcess()
        {
            if (!(ScrollbarSlider is null)) SetValue(ProcessValue());
            // nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }

        protected override int ProcessValue() {
            return Convert.ToInt32(MinValue + (float)ScrollbarSlider.X / (float)(Width - ScrollbarSlider.Width) * (MaxValue - MinValue));
        }

        protected override int ProcessLocation(int value) => Convert.ToInt32((value - MinValue) * (float)(Width - ScrollbarSlider.Width) / (MaxValue - MinValue));
        public new void Dispose()
        {
            Source.Dispose();
            ScrollbarSlider.MoveEvent -= MoveEvent;
            base.Dispose();
        }

    }
}
