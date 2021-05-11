using System;
using System.Collections.Generic;
using System.Text;

using GraphicsLibrary.Interfaces;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

namespace GraphicsLibrary.Graphics
{
    public class AForm: APanel
    {

        public delegate void OnClose();
        public event OnClose CloseEvent;

        private AEmptyPanel _Content;
        protected AEmptyPanel Content => _Content;
        protected AButton CloseButton;

        public AForm(ASize size): base(new ASize(Math.Max(size.Width, 100), Math.Max(size.Height, 100)))
        {
            Text = "New Form";
        }

        public override void Initialize()
        {

            base.Initialize();

            _Content = new AEmptyPanel(new ASize(Width - 20, Height - 70)) { Parent = this, Location = new APoint(10, 60) };
            CloseButton = new AButton(new ASize(40, 40)) { Parent = this, Location = new APoint(Width - 40, 0), Text = "×" };

            CloseButton.MouseClickEvent += (state, mstate) =>
            {
                CloseEvent?.Invoke();
                Enabled = false;
            };

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

        }

        public new void Add(IPrimitive primitive)
        {
            _Content.Add(primitive);
        }

        public new void Remove(IPrimitive primitive)
        {
            _Content.Remove(primitive);
        }

        public new void Clear()
        {
            _Content.Clear();
            _Content.Height = Height - 20;
        }

        public new void Dispose()
        {
            base.Dispose();
        }

    }
}
