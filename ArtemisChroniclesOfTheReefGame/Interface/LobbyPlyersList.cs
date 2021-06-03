using System;
using System.Linq;
using System.Collections.Generic;

using NetLibrary;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Interface
{

    public class LobbyPlyersList : AScrolleredPanel
    {

        public delegate void OnSelect(RPlayer plryer);
        public event OnSelect SelectEvent;
        public event OnSelect ExtraSelectEvent;

        private Dictionary<RPlayer, AExtendedButton> PlayerButton;

        public LobbyPlyersList(ASize size) : base(AScrollbarAlign.Vertical, size)
        {

            PlayerButton = new Dictionary<RPlayer, AExtendedButton>();

        }

        public override void Initialize()
        {

            base.Initialize();

            Text = "Список игроков";

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

        }

        public void Update(IEnumerable<RPlayer> players)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, -30);

            foreach (AExtendedButton bt in PlayerButton.Values) bt.Enabled = false;

            foreach (RPlayer player in players)
            {

                AExtendedButton button;

                if (PlayerButton.Keys.Where(x => x.Equals(player)).Count() != 0)
                {
                    button = PlayerButton[PlayerButton.Keys.Where(x => x.Equals(player)).First()];
                    string text = player.Name + " (" + player.IPAdress + ")";
                    if (!button.Text.Equals(text)) button.Text = text;
                    button.Enabled = true;
                }
                else
                {
                    button = new AExtendedButton(new ASize(ContentSize.Width - 20, 80));
                    Add(button);
                    PlayerButton.Add(player, button);

                    button.Text = player.Name + " (" + player.IPAdress + ")";
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

        }


    }
}
