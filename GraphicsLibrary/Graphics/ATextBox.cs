using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{

    public class ATextBox : APrimitive, IPrimitive
    {

        public delegate void OnTextChange(string text);

        public event OnTextChange TextChangeEvent;
        public event OnTextChange EndEditEvent;
        public event OnTextChange StartEditEvent;

        private IPrimitiveTexture Source;
        private Color _FillColor;
        private Color _BorderColor;

        protected bool isActive;
        protected bool isCursor;
        protected string sourceText;

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
        public ATextBox(ASize size) : base(size)
        {

            IsCounting = true;
            DTimer = 1;

            TimeEvent += () =>
            {
                if (isActive)
                {
                    if (isCursor)
                    {
                        isCursor = false;
                        if (Text.Length > 1) Text = Text.Remove(Text.Length - 1);
                        else Text = " ";
                    }
                    else
                    {
                        isCursor = true;
                        Text = Text += "|";
                    }
                }
            };

            MouseClickEvent += (state, mstate) => {
                isActive = true;
                sourceText = Text.Length > 0 && !Text.Equals(" ")? Text: "";
                StartEditEvent?.Invoke(Text);
            };

            KeyDownEvent += (state, kstate) => {
                if (kstate.KeyState.Equals(AKeyState.Exit))
                {
                    isActive = false;
                    isCursor = false;
                    Text = sourceText.Length > 0? sourceText: " ";
                    EndEditEvent?.Invoke(Text);
                }
                else if (kstate.KeyState.Equals(AKeyState.Enter))
                {
                    isActive = false;
                    if (isCursor)
                    {
                        isCursor = false;
                        Text = Text.Remove(Text.Length - 1);
                    }
                    EndEditEvent?.Invoke(Text);
                }
                else if (kstate.KeyState.Equals(AKeyState.Backspace)) RemoveChar();
                else if (kstate.KeyState.Equals(AKeyState.Space)) AddChar(" ");
                else if (kstate.KeyState.Equals(AKeyState.Key)) AddChar(kstate.Text);
            };

            MouseOverEvent += (state, mstate) => {
                isActive = false;
                if (isCursor)
                {
                    isCursor = false;
                    Text = Text.Remove(Text.Length - 1);
                }
                EndEditEvent?.Invoke(Text);
            };

        }

        public ATextBox() : this(GraphicsExtension.DefaultPanelSize) { }


        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
            Texture = Source = new ARectangleTexture(GraphicsDevice, Size) { IsDraw = true, IsFill = true };
            IsCounting = true;

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
        }

        public override void OnLocationChangeProcess()
        {
            //nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }

        public void SetTexture(Texture2D texture) => Source.FillBySource(texture);

        private void AddChar(string ch)
        {
            if (isCursor)
            {
                isCursor = false;
                Text = Text.Remove(Text.Length - 1);
            }
            else
            {
                if (Text.Equals(" ")) Text = "";
            }
            Text += ch;
            TextChangeEvent?.Invoke(Text);
        }
        private void RemoveChar()
        {
            if (isCursor)
            {
                isCursor = false;
                Text = Text.Remove(Text.Length - 1);
            }
            if (Text.Length > 0)
            {
                Text = Text.Remove(Text.Length - 1);
                TextChangeEvent?.Invoke(Text);
            }
        }

        public new void Dispose()
        {
            Source.Dispose();
            base.Dispose();
        }

    }
}
