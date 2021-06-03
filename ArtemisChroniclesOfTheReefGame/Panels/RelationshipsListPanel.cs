using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary.Player;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class RelationshipsListPanel : AScrolleredPanel
    {

        public delegate void OnSelect(IPlayer player);
        public event OnSelect SelectEvent;

        private Dictionary<IPlayer, AButton> RelationshipsList;

        public RelationshipsListPanel(ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            RelationshipsList = new Dictionary<IPlayer, AButton>();
        }

        public override void Initialize()
        {

            base.Initialize();

            Text = "Политический советник";

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

        }

        public void Update(IEnumerable<IPlayer> players, IPlayer mainPlayer)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, 0);

            foreach (var e in RelationshipsList) e.Value.Enabled = false;

            foreach (IPlayer player in players)
            {
                AButton button;

                if (RelationshipsList.ContainsKey(player))
                {
                    button = RelationshipsList[player];
                    button.Enabled = player.Status;

                    if (player.Status) button.FillColor = TexturePack.Colors[player.Id];

                    string text = player.Name + ": " + GameLocalization.RelationshipName[mainPlayer.Relationship(player)];
                    if (!button.Text.Equals(text)) button.Text = text;
                }
                else
                {
                    button = new AButton(new ASize(ContentSize.Width - 20, 50));
                    Add(button);

                    button.Text = player.Name + ": " + GameLocalization.RelationshipName[mainPlayer.Relationship(player)];

                    RelationshipsList.Add(player, button);

                    if (player.Status) button.FillColor = TexturePack.Colors[player.Id];

                    button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                    button.MouseClickEvent += (state, mstate) => { SelectEvent?.Invoke(player); };

                }
                if (button.Enabled)
                {
                    button.Location = last + new APoint(0, button.Height + 10);
                    last = button.Location;
                }
            }

            ContentSize = new ASize(ContentSize.Width, RelationshipsList.Count > 0 ? last.Y + 60 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }


    }
}
