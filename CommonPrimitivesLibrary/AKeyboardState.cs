using System;
using System.Collections.Generic;
using System.Text;

namespace CommonPrimitivesLibrary
{

    public enum AKeyboardKey{
        None, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, 
        D1, D2, D3, D4, D5, D6, D7, D8, D9, D0, 
        Equally, Dash, Tilde, Slash, Dot, Comma, OemOpenBrackets, OemCloseBrackets, OemSemicolon, OemQuotes
    }

    public enum AKeyState
    {
        Undefined,
        Backspace,
        Key,
        Space,
        Enter,
        Exit
    }
    public enum AKeyboardLanguage
    {
        Eng,
        Rus
    }


    public class AKeyboardState
    {

        public AKeyboardKey KeyboardKey;
        public AKeyState KeyState;
        public string Text { get => CapsLook ? Localization().ToUpper() : Localization().ToLower(); }
        public AKeyboardLanguage KeyboardLanguage;
        public bool CapsLook;
        public bool Shift;

        public AKeyboardState(AKeyState keyState) => (KeyState, KeyboardKey, KeyboardLanguage, CapsLook, Shift) = (keyState, AKeyboardKey.None, AKeyboardLanguage.Eng, false, false);
        public AKeyboardState(AKeyState keyState, AKeyboardKey keyboardKey, AKeyboardLanguage keyboardLanguage, bool capsLook, bool shift): this(keyState) => (KeyboardKey, KeyboardLanguage, CapsLook, Shift) = (keyboardKey, keyboardLanguage, capsLook, shift);

        private string Localization()
        {

            Dictionary<AKeyboardKey, string> EngKeys = new Dictionary<AKeyboardKey, string>()
                {
                    { AKeyboardKey.A, "A" }, { AKeyboardKey.B, "B" }, { AKeyboardKey.C, "C" }, { AKeyboardKey.D, "D" },
                    { AKeyboardKey.E, "E" }, { AKeyboardKey.F, "F" }, { AKeyboardKey.G, "G" }, { AKeyboardKey.H, "H" },
                    { AKeyboardKey.I, "I" }, { AKeyboardKey.J, "J" }, { AKeyboardKey.K, "K" }, { AKeyboardKey.L, "L" },
                    { AKeyboardKey.M, "M" }, { AKeyboardKey.N, "N" }, { AKeyboardKey.O, "O" }, { AKeyboardKey.P, "P" },
                    { AKeyboardKey.Q, "Q" }, { AKeyboardKey.R, "R" }, { AKeyboardKey.S, "S" }, { AKeyboardKey.T, "T" },
                    { AKeyboardKey.U, "U" }, { AKeyboardKey.V, "V" }, { AKeyboardKey.W, "W" }, { AKeyboardKey.X, "X" },
                    { AKeyboardKey.Y, "Y" }, { AKeyboardKey.Z, "Z" },

                    { AKeyboardKey.D0, "0" }, { AKeyboardKey.D1, "1" }, { AKeyboardKey.D2, "2" }, { AKeyboardKey.D3, "3" },
                    { AKeyboardKey.D4, "4" }, { AKeyboardKey.D5, "5" }, { AKeyboardKey.D6, "6" }, { AKeyboardKey.D7, "7" },
                    { AKeyboardKey.D8, "8" }, { AKeyboardKey.D9, "9" },

                    { AKeyboardKey.Equally, "=" }, { AKeyboardKey.Dash, "-" }, { AKeyboardKey.Tilde, "`" },
                    { AKeyboardKey.Slash, "/" }, { AKeyboardKey.Dot, "." }, { AKeyboardKey.Comma, "," },
                    { AKeyboardKey.OemOpenBrackets, "[" }, { AKeyboardKey.OemCloseBrackets, "]" },
                    { AKeyboardKey.OemSemicolon, ";" }, { AKeyboardKey.OemQuotes, "'" }, { AKeyboardKey.None, "" }
                };

            Dictionary<AKeyboardKey, string> RuKeys = new Dictionary<AKeyboardKey, string>()
                {
                    { AKeyboardKey.A, "Ф" }, { AKeyboardKey.B, "И" }, { AKeyboardKey.C, "С" }, { AKeyboardKey.D, "В" },
                    { AKeyboardKey.E, "У" }, { AKeyboardKey.F, "А" }, { AKeyboardKey.G, "П" }, { AKeyboardKey.H, "Р" },
                    { AKeyboardKey.I, "Ш" }, { AKeyboardKey.J, "О" }, { AKeyboardKey.K, "Л" }, { AKeyboardKey.L, "Д" },
                    { AKeyboardKey.M, "Ь" }, { AKeyboardKey.N, "Т" }, { AKeyboardKey.O, "Щ" }, { AKeyboardKey.P, "З" },
                    { AKeyboardKey.Q, "Й" }, { AKeyboardKey.R, "К" }, { AKeyboardKey.S, "Ы" }, { AKeyboardKey.T, "Е" },
                    { AKeyboardKey.U, "Г" }, { AKeyboardKey.V, "М" }, { AKeyboardKey.W, "Ц" }, { AKeyboardKey.X, "Ч" },
                    { AKeyboardKey.Y, "Н" }, { AKeyboardKey.Z, "Я" },

                    { AKeyboardKey.D0, "0" }, { AKeyboardKey.D1, "1" }, { AKeyboardKey.D2, "2" }, { AKeyboardKey.D3, "3" },
                    { AKeyboardKey.D4, "4" }, { AKeyboardKey.D5, "5" }, { AKeyboardKey.D6, "6" }, { AKeyboardKey.D7, "7" },
                    { AKeyboardKey.D8, "8" }, { AKeyboardKey.D9, "9" },
                    
                    { AKeyboardKey.Equally, "=" }, { AKeyboardKey.Dash, "-" }, { AKeyboardKey.Tilde, "Ё" },
                    { AKeyboardKey.Slash, "." }, { AKeyboardKey.Dot, "Ю" }, { AKeyboardKey.Comma, "Б" },
                    { AKeyboardKey.OemOpenBrackets, "Х" }, { AKeyboardKey.OemCloseBrackets, "Ъ" },
                    { AKeyboardKey.OemSemicolon, "Ж" }, { AKeyboardKey.OemQuotes, "Э" }, { AKeyboardKey.None, "" }
                };

            Dictionary<AKeyboardKey, string> EngShiftKeys = new Dictionary<AKeyboardKey, string>()
                {
                    { AKeyboardKey.A, "A" }, { AKeyboardKey.B, "B" }, { AKeyboardKey.C, "C" }, { AKeyboardKey.D, "D" },
                    { AKeyboardKey.E, "E" }, { AKeyboardKey.F, "F" }, { AKeyboardKey.G, "G" }, { AKeyboardKey.H, "H" },
                    { AKeyboardKey.I, "I" }, { AKeyboardKey.J, "J" }, { AKeyboardKey.K, "K" }, { AKeyboardKey.L, "L" },
                    { AKeyboardKey.M, "M" }, { AKeyboardKey.N, "N" }, { AKeyboardKey.O, "O" }, { AKeyboardKey.P, "P" },
                    { AKeyboardKey.Q, "Q" }, { AKeyboardKey.R, "R" }, { AKeyboardKey.S, "S" }, { AKeyboardKey.T, "T" },
                    { AKeyboardKey.U, "U" }, { AKeyboardKey.V, "V" }, { AKeyboardKey.W, "W" }, { AKeyboardKey.X, "X" },
                    { AKeyboardKey.Y, "Y" }, { AKeyboardKey.Z, "Z" },

                    { AKeyboardKey.D0, ")" }, { AKeyboardKey.D1, "!" }, { AKeyboardKey.D2, "@" }, { AKeyboardKey.D3, "#" },
                    { AKeyboardKey.D4, "$" }, { AKeyboardKey.D5, "%" }, { AKeyboardKey.D6, "^" }, { AKeyboardKey.D7, "&" },
                    { AKeyboardKey.D8, "*" }, { AKeyboardKey.D9, "(" },

                    { AKeyboardKey.Equally, "+" }, { AKeyboardKey.Dash, "_" }, { AKeyboardKey.Tilde, "~" },
                    { AKeyboardKey.Slash, "?" }, { AKeyboardKey.Dot, ">" }, { AKeyboardKey.Comma, "<" },
                    { AKeyboardKey.OemOpenBrackets, "{" }, { AKeyboardKey.OemCloseBrackets, "}" },
                    { AKeyboardKey.OemSemicolon, ":" }, { AKeyboardKey.OemQuotes, '"'.ToString() }, { AKeyboardKey.None, "" }
                };

            Dictionary<AKeyboardKey, string> RuShiftKeys = new Dictionary<AKeyboardKey, string>()
                {
                    { AKeyboardKey.A, "Ф" }, { AKeyboardKey.B, "И" }, { AKeyboardKey.C, "С" }, { AKeyboardKey.D, "В" },
                    { AKeyboardKey.E, "У" }, { AKeyboardKey.F, "А" }, { AKeyboardKey.G, "П" }, { AKeyboardKey.H, "Р" },
                    { AKeyboardKey.I, "Ш" }, { AKeyboardKey.J, "О" }, { AKeyboardKey.K, "Л" }, { AKeyboardKey.L, "Д" },
                    { AKeyboardKey.M, "Ь" }, { AKeyboardKey.N, "Т" }, { AKeyboardKey.O, "Щ" }, { AKeyboardKey.P, "З" },
                    { AKeyboardKey.Q, "Й" }, { AKeyboardKey.R, "К" }, { AKeyboardKey.S, "Ы" }, { AKeyboardKey.T, "Е" },
                    { AKeyboardKey.U, "Г" }, { AKeyboardKey.V, "М" }, { AKeyboardKey.W, "Ц" }, { AKeyboardKey.X, "Ч" },
                    { AKeyboardKey.Y, "Н" }, { AKeyboardKey.Z, "Я" },

                    { AKeyboardKey.D0, ")" }, { AKeyboardKey.D1, "!" }, { AKeyboardKey.D2, '"'.ToString() }, { AKeyboardKey.D3, "№" },
                    { AKeyboardKey.D4, ";" }, { AKeyboardKey.D5, "%" }, { AKeyboardKey.D6, ":" }, { AKeyboardKey.D7, "?" },
                    { AKeyboardKey.D8, "*" }, { AKeyboardKey.D9, "(" },

                    { AKeyboardKey.Equally, "+" }, { AKeyboardKey.Dash, "_" }, { AKeyboardKey.Tilde, "~" },
                    { AKeyboardKey.Slash, "," }, { AKeyboardKey.Dot, ">" }, { AKeyboardKey.Comma, "<" },
                    { AKeyboardKey.OemOpenBrackets, "{" }, { AKeyboardKey.OemCloseBrackets, "}" },
                    { AKeyboardKey.OemSemicolon, ":" }, { AKeyboardKey.OemQuotes, '"'.ToString() }, { AKeyboardKey.None, "" }
                };

            switch (KeyboardLanguage)
            {
                case AKeyboardLanguage.Eng: return Shift ? EngShiftKeys[KeyboardKey] : EngKeys[KeyboardKey];
                case AKeyboardLanguage.Rus: return Shift ? RuShiftKeys[KeyboardKey] : RuKeys[KeyboardKey];
                default: return "";
            }
        }

    }



}
