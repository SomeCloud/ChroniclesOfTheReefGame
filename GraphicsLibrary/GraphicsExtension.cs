using Microsoft.Xna.Framework;
using Font = System.Drawing.Font;
using System;
using CommonPrimitivesLibrary;

namespace GraphicsLibrary
{
    public struct GraphicsExtension
    {
        public static string DefaultFontFamilyName = "Tahoma"; // "Courier New"
        public static string ExtraFontFamilyName = "Courier New";
        public static Font DefaultFont = new Font(DefaultFontFamilyName, 12, System.Drawing.FontStyle.Regular);
        public static Font DefaultConstructionFont = new Font(DefaultFontFamilyName, 8, System.Drawing.FontStyle.Regular);

        public static Color DefaultFillColor = new Color(188, 187, 198, 255);
        public static Color DefaultBorderColor = new Color(134, 136, 159, 255);
        public static Color DefaultMiddleFillColor = new Color(134, 136, 159, 255);
        public static Color DefaultDarkFillColor = new Color(66, 67, 83, 255);
        public static Color DefaultDarkBorderColor = new Color(43, 44, 55, 255);
        public static Color DefaultTextColor = Color.Black;

        public static Color ExtraColorGreen = new Color(93, 149, 70, 255);
        public static Color ExtraColorYellow = new Color(166, 154, 78, 255);
        public static Color ExtraColorRed = new Color(166, 103, 78, 255);

        public static Color BackgroundColor = new Color(66, 67, 83, 255); //new Color(7, 12, 46);

        public static ASize DefaultStandartPrimititveSize = new ASize(64, 64);

        public static ASize DefaultTextLabelSize = new ASize(250, 60);
        public static ASize DefaultPanelSize = new ASize(500, 300);
        public static ASize DefaultButtonSize = new ASize(250, 60);
        public static ASize DefaultMenuButtonSize = new ASize(300, 60);
        public static ASize DefaultFormSize = new ASize(600, 480);
        public static ASize DefaultCheckboxSize = new ASize(20, 20);

        public static ASize DefaultMiniButtonSize = new ASize(60, 60);
        public static ASize DefaultInterfaceUnitButtonSize = new ASize(60, 92);
        public static ASize DefaultResourceIconSize = new ASize(195, 28);

        public static ASize DefaultMapViewSize = new ASize(1280, 720);
        public static int DefaultMapCellRadius = 195;

        public static ASize DefaultHorizontalScrollbarSize = new ASize(400, 20);
        public static ASize DefaultVerticalScrollbarSize = new ASize(20, 400);
        public static ASize DefaultHorizontalScrollbarSliderSize = new ASize(40, 16);
        public static ASize DefaultVerticalScrollbarSliderSize = new ASize(16, 40);

        public ASize CurrentHexSize(int radius) => new ASize(radius, radius - Convert.ToInt32(radius / 7.5f));

    }
}
