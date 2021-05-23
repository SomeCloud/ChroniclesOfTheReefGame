using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;
using AKeyState = CommonPrimitivesLibrary.AKeyState;

using GameLibrary;
using GameLibrary.Settlement;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class SettlementsListPanel: AScrolleredPanel
    {

        public delegate void OnSelect(ISettlement settlement);

        public event OnSelect SelectEvent;

        private Dictionary<ISettlement, AButton> SettlementsList;

        public SettlementsListPanel(ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            SettlementsList = new Dictionary<ISettlement, AButton>();
        }

        public override void Initialize()
        {

            base.Initialize();

            Text = "Советник по инфраструктуре";

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

        }


        public void Update(IEnumerable<ISettlement> settlements)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, 0);

            foreach (var e in SettlementsList) e.Value.Enabled = false;

            foreach (ISettlement settlement in settlements)
            {
                AButton button;

                if (SettlementsList.ContainsKey(settlement))
                {
                    button = SettlementsList[settlement];

                    button.Enabled = true;

                    string text = "[" + settlement.Location + "] город " + settlement.Name + ", доход (" + (settlement.Income > 0? "+": "") + settlement.Income + ")";
                    if (!button.Text.Equals(text)) button.Text = text;
                }
                else
                {
                    button = new AButton(new ASize(ContentSize.Width - 20, 50));
                    Add(button);

                    button.Text = "[" + settlement.Location + "] город " + settlement.Name + ", доход (" + (settlement.Income > 0 ? "+" : "") + settlement.Income + ")";

                    SettlementsList.Add(settlement, button);

                    button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                    button.MouseClickEvent += (state, mstate) => { SelectEvent?.Invoke(settlement); };

                }

                button.Location = last + new APoint(0, button.Height + 10);
                last = button.Location;
            }

            ContentSize = new ASize(ContentSize.Width, SettlementsList.Count > 0 ? last.Y + 60 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }

    }
}
