using Microsoft.Xna.Framework.Graphics;

using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

namespace GraphicsLibrary.Graphics
{
    public class ALabeledCheckbox : AEmptyPanel
    {

        public delegate void OnValueChange(bool value);

        public event OnValueChange ValueChange;

        private Texture2D ActiveTexture;
        private Texture2D NotActiveTexture;

        private ALabel _Label;
        private ACheckbox _Checkbox;

        private string _Text;

        public ALabel Label => _Label;
        public ACheckbox Checkbox => _Checkbox;

        public string LabelText
        {
            get => Label.Text;
            set => Label.Text = value;
        }

        public new string Text
        {
            get => _Label is object ? _Label.Text : "";
            set
            {
                _Text = value;
                if (_Label is object) _Label.Text = _Text;
            }
        }

        public ALabeledCheckbox(int width, Texture2D active, Texture2D notActive) : base(new ASize(width, GraphicsExtension.DefaultCheckboxSize.Height + 2))
        {
            ActiveTexture = active;
            NotActiveTexture = notActive;
        }

        public bool Value
        {
            set
            {
                if (value) _Checkbox.Activate();
                else _Checkbox.Deactivate();
            }
            get => _Checkbox.Value;
        }

        public override void Initialize()
        {

            base.Initialize();


            _Label = new ALabel(new ASize(Width - 12 - GraphicsExtension.DefaultCheckboxSize.Width, GraphicsExtension.DefaultCheckboxSize.Height)) { Parent = this, Location = new APoint(1, 1) };
            _Checkbox = new ACheckbox(ActiveTexture, NotActiveTexture) { Parent = this, Location = _Label.Location + new APoint(_Label.Width + 10, 0) };

            _Checkbox.ValueChange += (value) => ValueChange?.Invoke(value);

            Label.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            Label.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

        }

    }
}
