using System;
using System.Collections.Generic;
using System.Text;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

namespace GraphicsLibrary.Graphics
{
    public class ALabeledTextBox: AEmptyPanel
    {

        private ALabel Label;
        private ATextBox TextBox;

        public string LabelText 
        {
            get => Label.Text;
            set => Label.Text = value;
        }

        public ATextLabel LabelTextLabel
        {
            get => Label.TextLabel;
        }

        public new string Text
        {
            get => TextBox.Text;
            set => TextBox.Text = value;
        }

        public new ATextLabel TextLabel
        {
            get => TextBox.TextLabel;
        }

        public ALabeledTextBox(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();
            Label = new ALabel(new ASize(Width - 2, Height - 62)) { Parent = this, Location = new APoint(1, 1) };
            TextBox = new ATextBox(new ASize(Width - 2, 50)) { Parent = this, Location = Label.Location + new APoint(0, Label.Height + 10) };

            Label.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

        }

    }
}
