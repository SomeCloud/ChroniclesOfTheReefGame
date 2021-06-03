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
using GameLibrary.Technology;
using GameLibrary.Player;
using GameLibrary.Map;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class TechnologyTreePanel: APanel
    {

        public delegate void OnSelect(ITechnology technology);
        public event OnSelect SelectEvent;

        private AScrolleredPanel TechnologiesInSettlementsList;
        //private TechnologiesListPanel TechnologiesList;

        public AButton _CloseButton;

        private AMapCell MapCell;
        private IPlayer Player;

        public AButton CloseButton => _CloseButton;

        public TechnologyTreePanel(ASize size): base(size)
        {

        }

        public TechnologyTreePanel(ASize size, IPrimitiveTexture primitiveTexture) : base(size, primitiveTexture)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            _CloseButton = new AButton(new ASize(40, 40)) { Parent = this, Location = new APoint(Width - 50, 10), Text = "×" };

            ASize size = new ASize((Width - 30) / 2, Height - _CloseButton.Height - 30);

            TechnologiesInSettlementsList = new AScrolleredPanel(AScrollbarAlign.Vertical, size) { Parent = this, Location = new APoint(10, _CloseButton.Y + _CloseButton.Height + 10) };
            //TechnologiesList = new TechnologiesListPanel(size) { Parent = this, Location = TechnologiesInSettlementsList.Location + new APoint(TechnologiesInSettlementsList.Width + 10, 0) };

            _CloseButton.MouseClickEvent += (state, mstate) => Hide();

            TechnologiesInSettlementsList.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TechnologiesInSettlementsList.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TechnologiesInSettlementsList.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TextLabel.VerticalAlign = ATextVerticalAlign.Top;

            Text = "Советник по науке";

            /*TechnologiesList.SelectEvent += (technology) => {
                if (MapCell is object && MapCell.IsSettlement)
                {
                    MapCell.Settlement.SetInvestigatedTechnology(technology);
                    Update(MapCell, Player);
                    SelectEvent?.Invoke(technology);
                }
            };*/

        }

        public void Update(AMapCell mapCell, IPlayer player)
        {

            MapCell = mapCell;
            Player = player;

            //TechnologiesList.Update(player.Technologies);

            string technologiesText = "Исследования в городах: \n\n" + string.Join("\n", player.Settlements.Select(x => x.Name + ": " + (x.InvestigatedTechnology is object? x.InvestigatedTechnology.Name + " " + x.InvestigatedTechnology.StudyPoints + "/" + x.InvestigatedTechnology.RequiredStudyPoints + " (" + (x.Science > 0 ? "+" + x.Science : x.Science.ToString()) + ")" : "исследование не выбрано")));
            
            TechnologiesInSettlementsList.ContentSize = new ASize(TechnologiesInSettlementsList.ContentSize.Width, 40 + (technologiesText.Count(x => x == '\n') + 1) * TechnologiesInSettlementsList.TextLabel.Font.Height);
            TechnologiesInSettlementsList.Text = technologiesText;
            TechnologiesInSettlementsList.Scrollbar.MaxValue = TechnologiesInSettlementsList.Height < TechnologiesInSettlementsList.ContentSize.Height ? TechnologiesInSettlementsList.ContentSize.Height - TechnologiesInSettlementsList.Height : 0;

        }

        public void Hide() => Enabled = false;
        public void Show(AMapCell mapCell, IPlayer player)
        {
            Enabled = true;
            Update(mapCell, player);
        }

    }
}
