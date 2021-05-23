using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;
using AKeyState = CommonPrimitivesLibrary.AKeyState;

using GameLibrary;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class AExtendedButton: AEmptyPanel
    {

        public delegate void OnClick();

        public event OnClick ButtonClick;
        public event OnClick ExtraButtonClick;

        private AButton _Button;
        private AButton _ExtraButton;

        public Microsoft.Xna.Framework.Color FillColor
        {
            get => _Button.FillColor;
            set
            {
                _Button.FillColor = value;
            }
        }

        public AButton Button => _Button;
        public AButton ExtraButton => _ExtraButton;

        public new string Text
        {
            get => _Button.Text;
            set => _Button.Text = value;
        }

        public string ButtonText
        {
            get => _Button.Text;
            set => _Button.Text = value;
        }

        public string ExtraButtonText
        {
            get => _ExtraButton.Text;
            set => _ExtraButton.Text = value;
        }

        public AExtendedButton(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = Height / 2;

            _Button = new AButton(new ASize(Width - dWidth - 2, Height - 2)) { Parent = this, Location = new APoint(1, 1) };
            _ExtraButton = new AButton(new ASize(dWidth - 1, Height - 2)) { Parent = this, Location = _Button.Location + new APoint(_Button.Width - 1, 0) };

            _Button.MouseClickEvent += (state, mstate) => ButtonClick?.Invoke();
            _ExtraButton.MouseClickEvent += (state, mstate) => ExtraButtonClick?.Invoke();

        }

    }
}
