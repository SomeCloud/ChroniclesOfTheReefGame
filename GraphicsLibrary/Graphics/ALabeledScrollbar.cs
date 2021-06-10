using System;
using System.Collections.Generic;
using System.Text;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class ALabeledScrollbar : AEmptyPanel
    {

        public event AScrollbar.OnValueChange ValueChange;

        private ALabel _Label;
        private AHorizontalScrollbar _Scrollbar;

        private string _Text;

        public ALabel Label => _Label;
        public AHorizontalScrollbar Scrollbar => _Scrollbar;

        public new ATextLabel TextLabel
        {
            get => _Label.TextLabel;
        }

        public string LabelText
        {
            get => Label.Text;
            set => Label.Text = value;
        }

        public new string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                if (_Label is object) _Label.Text = _Text + Value;
            }
        }

        public int MinValue {
            set => _Scrollbar.MinValue = value;
            get => _Scrollbar.MinValue;
        }

        public int MaxValue
        {
            set
            {
                if (!_Scrollbar.MaxValue.Equals(value)) _Scrollbar.MaxValue = value;
            }
            get => _Scrollbar.MaxValue;
        }

        public int Value
        {
            set => _Scrollbar.Value = value;
            get => _Scrollbar.Value;
        }

        public ALabeledScrollbar(ASize size) : base(size)
        {      

        }

        public override void Initialize()
        {

            base.Initialize();

            _Scrollbar = new AHorizontalScrollbar(new ASize(Width - 2, GraphicsExtension.DefaultHorizontalScrollbarSize.Height)) { Parent = this, Location = new APoint(1, 1) };
            _Label = new ALabel(new ASize(Width - 2, Height - _Scrollbar.Height - 12)) { Parent = this, Location = _Scrollbar.Location + new APoint(0, _Scrollbar.Height + 10) };

            _Scrollbar.ValueChange += (value) => _Label.Text = _Text + value.ToString();

            Label.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            _Scrollbar.ValueChange += (value) => ValueChange?.Invoke(value);

        }

    }
}
