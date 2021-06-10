using Microsoft.Xna.Framework;

using GraphicsLibrary.Interfaces;
using CommonPrimitivesLibrary;

namespace GraphicsLibrary.StandartGraphicsPrimitives
{

    public enum AScrollbarAlign
    {
        Vertical,
        Horizontal
    }

    public abstract class AScrollbar: APrimitive, IPrimitive
    {

        public AScrollbarSlider ScrollbarSlider;

        public delegate void OnValueChange(int value);

        public event OnValueChange MaxValueChange;
        public event OnValueChange MinValueChange;
        public event OnValueChange ValueChange;

        private int _MaxValue;
        private int _MinValue;
        protected int _Value;

        public int MaxValue
        {
            get => _MaxValue;
            set
            {
                _MaxValue = value;
                //SetValue(ProcessValue());
                MaxValueChange?.Invoke(_MaxValue);
            }
        }
        public int MinValue
        {
            get => _MinValue;
            set
            {
                _MinValue = value;
                //SetValue(ProcessValue());
                MinValueChange?.Invoke(_MinValue);
            }
        }
        public int Value
        {
            get => _Value;
            set {
                _Value = value;
                ValueChange?.Invoke(_Value);
                //ScrollbarSlider.ForciblySetLocation(new APoint(2, ProcessLocation(value)));
            }
        }

        public AScrollbar(ASize size) : base(size)
        {

            _MaxValue = 1;
            _MinValue = 0;
            _Value = _MinValue;
        }

        protected new void SetLocation(APoint point) => base.SetLocation(point);

        protected abstract int ProcessValue();
        protected abstract int ProcessLocation(int value);
        protected void SetValue(int value) {
            _Value = value;
            ValueChange?.Invoke(_Value);
        }
    }
}
