using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class APlayersList : AScrolleredPanel
    {

        public delegate void OnSelect(string player);
        public event OnSelect SelectEvent;
        public event OnSelect ExtraSelectEvent;

        private Dictionary<string, AExtendedButton> PlayerButton;
        public APlayersList(ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            PlayerButton = new Dictionary<string, AExtendedButton>();
        }

        public override void Initialize()
        {

            base.Initialize();

            Text = "Список игроков";

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

        }

        public new void Clear()
        {
            base.Clear();
            PlayerButton.Clear();
        }

        public void Update(IEnumerable<string> players)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, -30);

            foreach (AExtendedButton bt in PlayerButton.Values) bt.Enabled = false;

            foreach (string player in players)
            {

                AExtendedButton button;

                if (PlayerButton.Keys.Where(x => x.Equals(player)).Count() != 0)
                {
                    button = PlayerButton[PlayerButton.Keys.Where(x => x.Equals(player)).First()];
                    string text = player;
                    if (!button.Text.Equals(text)) button.Text = text;
                    button.Enabled = true;
                }
                else
                {
                    button = new AExtendedButton(new ASize(ContentSize.Width - 20, 80));
                    Add(button);
                    PlayerButton.Add(player, button);

                    button.Text = player;
                    button.ExtraButtonText = "x";

                    button.Button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.Button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

                    button.ExtraButton.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 16);

                    button.ButtonClick += () => { SelectEvent?.Invoke(player); };
                    button.ExtraButtonClick += () => { ExtraSelectEvent?.Invoke(player); };
                }

                button.Location = last + new APoint(0, button.Height + 10);
                last = button.Location;

            }

            ContentSize = new ASize(ContentSize.Width, PlayerButton.Count > 0 ? last.Y + 90 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }

    }
}
