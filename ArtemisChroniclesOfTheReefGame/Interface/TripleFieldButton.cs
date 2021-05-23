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
    class TripleFieldButton: AButton
    {

        private AEmptyPanel _TitleField;
        private AEmptyPanel _DownField;
        private AEmptyPanel _ExtraField;

        public AEmptyPanel TitleField => _TitleField;
        public AEmptyPanel DownField => _DownField;
        public AEmptyPanel ExtraField => _ExtraField;

        public string TitleFieldText
        {
            get => _TitleField.Text;
            set => _TitleField.Text = value;
        }

        public string DownFieldText
        {
            get => _DownField.Text;
            set => _DownField.Text = value;
        }

        public string ExtraFieldText
        {
            get => _ExtraField.Text;
            set => _ExtraField.Text = value;
        }

        public TripleFieldButton(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = Width / 2;
            int dHeight = Convert.ToInt32(Height * 2 / 3f);

            _TitleField = new AEmptyPanel(new ASize(dWidth - 2, dHeight - 2)) { Parent = this, Location = new APoint(1, 1), IsInteraction = false };
            _DownField = new AEmptyPanel(new ASize(dWidth - 2, Height - _TitleField.Height)) { Parent = this, Location = _TitleField.Location + new APoint(0, _TitleField.Height), IsInteraction = false };
            _ExtraField = new AEmptyPanel(new ASize(dWidth - 1, Height - 2)) { Parent = this, Location = _TitleField.Location + new APoint(_TitleField.Width - 1, 0), IsInteraction = false };

        }


    }
}
