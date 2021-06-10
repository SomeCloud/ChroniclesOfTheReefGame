using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

using CommonPrimitivesLibrary;

using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class AScrolleredPanel: APrimitive, IPrimitive
    {

        private IPrimitiveTexture Source;
        private Color _FillColor;
        private Color _BorderColor;
        private AScrollbar _Scrollbar;
        protected AEmptyPanel _Content;

        public ALabel InfoLabel;

        private AScrollbarAlign ScrollbarAlign;

        public new string Text { get => _Content.Text; set => _Content.Text = value; }
        public ASize ContentSize { get => _Content.Size; set => _Content.Size = value; }
        public new ATextLabel TextLabel { get => _Content.TextLabel; }
        public AScrollbar Scrollbar { get => _Scrollbar; }

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

        public List<IPrimitive> Content { get => _Content.Controls; }

        public AScrolleredPanel(AScrollbarAlign scrollbarAlign, ASize size) : base(size)
        {
            ScrollbarAlign = scrollbarAlign;
        }

        public AScrolleredPanel(AScrollbarAlign scrollbarAlign) : this(scrollbarAlign, GraphicsExtension.DefaultPanelSize) { }

        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
            Texture = Source = new ARectangleTexture(GraphicsDevice, Size) { IsDraw = true, IsFill = true };

            if (_Scrollbar is null && _Content is null)
            {
                if (ScrollbarAlign.Equals(AScrollbarAlign.Horizontal))
                {
                    _Content = new AEmptyPanel(new ASize(Width, Height - GraphicsExtension.DefaultHorizontalScrollbarSize.Height - 20)) { Parent = this, Location = new APoint(10, 10) };
                    _Scrollbar = new AHorizontalScrollbar(new ASize(Width, GraphicsExtension.DefaultHorizontalScrollbarSize.Height)) { Parent = this, Location = new APoint(0, Height - GraphicsExtension.DefaultHorizontalScrollbarSize.Height), MinValue = -10, MaxValue = 10 };
                }
                else
                {
                    _Content = new AEmptyPanel(new ASize(Width - GraphicsExtension.DefaultVerticalScrollbarSize.Width - 20, Height)) { Parent = this, Location = new APoint(10, 10) };
                    _Scrollbar = new AVerticalScrollbar(new ASize(GraphicsExtension.DefaultVerticalScrollbarSize.Width, Height)) { Parent = this, Location = new APoint(Width - GraphicsExtension.DefaultVerticalScrollbarSize.Width, 0), MinValue = -10, MaxValue = 10 };
                }

                _Scrollbar.ScrollbarSlider.Color = GraphicsExtension.DefaultDarkFillColor;
                InfoLabel = new ALabel(GraphicsExtension.DefaultTextLabelSize) { Parent = this, Location = new APoint(10, 10), Text = "Value: " + _Scrollbar.MinValue + "/" + _Scrollbar.Value + "/" + _Scrollbar.MaxValue, Enabled = false };

                _Scrollbar.ValueChange += (value) =>
                {
                    if (ScrollbarAlign.Equals(AScrollbarAlign.Horizontal)) _Content.X = -value;
                    else _Content.Y = -value;
                    InfoLabel.Text = "Value: " + _Scrollbar.MinValue + "/" + value + "/" + _Scrollbar.MaxValue;
                };
            }
        }

        public override void OnLocationChangeProcess()
        {
            //nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }

        public new void Add(IPrimitive primitive)
        {
            _Content.Add(primitive);
            //_Content.Height = 10;
            /*foreach (IPrimitive e in _Content.Controls.Where(x => x.Enabled)) _Content.Height += e.Height + 10;
            _Scrollbar.MaxValue = _Content.Height / 2;
            _Scrollbar.MinValue = - _Content.Height / 2;*/
        }

        public new void Remove(IPrimitive primitive)
        {
            _Content.Remove(primitive);
            //_Content.Height = 10;
            //foreach (IPrimitive e in _Content.Controls.Where(x => x.Enabled)) _Content.Height += e.Height + 10;
            //_Scrollbar.MaxValue = _Content.Height / 2;
            //_Scrollbar.MinValue = -_Content.Height / 2;
        }

        public new void Clear()
        {
            _Content.Clear();
            _Content.Height = Height - 20;
        }

        public new void Dispose()
        {
            Source.Dispose();
            base.Dispose();
        }

    }
}
