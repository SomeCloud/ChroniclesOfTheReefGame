using System;
using System.Collections.Generic;
using System.Text;

namespace CommonPrimitivesLibrary
{

    public enum AMouseButtonState
    {
        Up,
        Down,
        Pressed,
        None
    }
    public enum AMouseButton
    {
        Left,
        Right,
        Middle,
        None
    }

    public class AMouseState
    {

        public APoint CursorPosition;
        public AMouseButton MouseButton;
        public AMouseButtonState MouseButtonState;

        public AMouseState(APoint cursorPosition, AMouseButton mouseButton, AMouseButtonState mouseButtonState) => (CursorPosition, MouseButton, MouseButtonState) = (cursorPosition, mouseButton, mouseButtonState);

    }
}
