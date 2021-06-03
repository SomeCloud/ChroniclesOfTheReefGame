using System;
using System.Linq;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Technology;
using GameLibrary.Player;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{
    public class ATechnologyTreeForm: AForm
    {

        public delegate void OnSelect(ITechnology technology);
        public event OnSelect SelectEvent;

        private AScrolleredPanel TechnologiesInSettlementsList;
        private TechnologiesListPanel TechnologiesList;

        public ATechnologyTreeForm(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            ASize size = new ASize((Content.Width - 32) / 2, Content.Height - 22);

            TechnologiesInSettlementsList = new AScrolleredPanel(AScrollbarAlign.Vertical, size) { Parent = this, Location = new APoint(10, 10) };
            TechnologiesList = new TechnologiesListPanel(size) { Parent = this, Location = TechnologiesInSettlementsList.Location + new APoint(TechnologiesInSettlementsList.Width + 10, 0) };

            TechnologiesInSettlementsList.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TechnologiesInSettlementsList.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TechnologiesInSettlementsList.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            Text = "Советник по науке";

            TechnologiesList.SelectEvent += (technology) => SelectEvent?.Invoke(technology);

        }

        public void Update(IPlayer player)
        {

            TechnologiesList.Update(player.Technologies);

            string technologiesText = "Исследования в городах: \n\n" + string.Join("\n", player.Settlements.Select(x => x.Name + ": " + (x.InvestigatedTechnology is object ? x.InvestigatedTechnology.Name + " " + x.InvestigatedTechnology.StudyPoints + "/" + x.InvestigatedTechnology.RequiredStudyPoints + " (" + (x.Science > 0 ? "+" + x.Science : x.Science.ToString()) + ")" : "исследование не выбрано")));

            TechnologiesInSettlementsList.ContentSize = new ASize(TechnologiesInSettlementsList.ContentSize.Width, 40 + (technologiesText.Count(x => x == '\n') + 1) * TechnologiesInSettlementsList.TextLabel.Font.Height);
            TechnologiesInSettlementsList.Text = technologiesText;
            TechnologiesInSettlementsList.Scrollbar.MaxValue = TechnologiesInSettlementsList.Height < TechnologiesInSettlementsList.ContentSize.Height ? TechnologiesInSettlementsList.ContentSize.Height - TechnologiesInSettlementsList.Height : 0;
        }

        public void Hide() => Enabled = false;
        public void Show(IPlayer player)
        {
            Enabled = true;
            Update(player);
        }

    }
}
