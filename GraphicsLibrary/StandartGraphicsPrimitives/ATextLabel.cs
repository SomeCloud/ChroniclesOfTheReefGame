using Microsoft.Xna.Framework;

using Font = System.Drawing.Font;

using System;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary
{

    /// <summary>  
    /// Список возможных расположений текста по горизонтали
    /// </summary>  
    public enum ATextHorizontalAlign
    {
        Right,
        Center,
        Left
    }

    /// <summary>  
    /// Список возможных расположений текста по вертикали
    /// </summary>  
    public enum ATextVerticalAlign
    {
        Top,
        Center,
        Bottom
    }

    public class ATextLabel: IDisposable
    {

        public delegate void OnChangeEvent(ATextLabel obj);

        public event OnChangeEvent ChangeEvent;

        private Font font;
        private ATextHorizontalAlign horizontalAlign;
        private ATextVerticalAlign verticalAlign;
        private Color textColor;
        private bool visible;

        public Font Font { get => font; set { font = value; ChangeEvent?.Invoke(this); } }
        public ATextHorizontalAlign HorizontalAlign { get => horizontalAlign; set { horizontalAlign = value; ChangeEvent?.Invoke(this); } }
        public ATextVerticalAlign VerticalAlign { get => verticalAlign; set { verticalAlign = value; ChangeEvent?.Invoke(this); } }
        public Color TextColor { get => textColor; set { textColor = value; ChangeEvent?.Invoke(this); } }
        public bool Visible { get => visible; set { visible = value; ChangeEvent?.Invoke(this); } }

        public ATextLabel()
        {
            Font = GraphicsExtension.DefaultFont;
            HorizontalAlign = ATextHorizontalAlign.Center;
            VerticalAlign = ATextVerticalAlign.Center;
            TextColor = GraphicsExtension.DefaultTextColor;
            Visible = true;
        }

        public void Dispose()
        {
            ChangeEvent = null;
        }

    }

}
