using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Extension;
using GameLibrary.Map;
using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class TextBoxPanel: AForm
    {

        public delegate void OnResult(bool status, string text);
        public event OnResult ResultEvent;

        private ATextBox TextBox;
        private AButton Done;

        public TextBoxPanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            TextBox = new ATextBox(new ASize(Content.Width - 20, 50)) { Location = new APoint(10, (Content.Height - 110) / 2) };
            Done = new AButton(new ASize(200, 50)) { Location = TextBox.Location + new APoint((TextBox.Width - 200) / 2, TextBox.Height + 10), Text = "Готово" };

            Done.MouseClickEvent += (state, mstate) => ResultEvent?.Invoke(true, TextBox.Text);
            CloseEvent += () => ResultEvent?.Invoke(false, TextBox.Text);

            Add(TextBox);
            Add(Done);

        }

        public void Update(string text)
        {
            TextBox.Text = text;
            Enabled = true;
        }

    }
}
