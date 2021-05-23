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
using GameLibrary.Unit.Main;
using GameLibrary.Extension;
using GameLibrary.Technology;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class SettlementPanel : AForm
    {

        public delegate void OnTechnologySelect(ISettlement settlement);
        public delegate void OnBuildingSelect();
        public delegate void OnUnitSelect(AUnitType unitType, APoint location);
        public delegate void OnShow();

        public event OnUnitSelect UnitSelectEvent;
        public event OnBuildingSelect BuildingSelectEvent;
        public event OnTechnologySelect TechnologyTreeSelectEvent;
        public event OnShow ShowEvent;

        private SettlementInfoPanel SettlementInfoPanel;
        private BuildingPanel BuildingPanel;
        private CreateUnitsPanel UnitsPanel;

        private AScrolleredPanel BuildingsInConstruction;

        private AButton Main;
        private AButton Building;
        private AButton Units;

        private ISettlement Settlement;

        public SettlementPanel(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            Main = new AButton(new ASize(250, 50)) { Location = new APoint(1, 1), Text = "Поселение" };
            Building = new AButton(new ASize(250, 50)) { Location = Main.Location + new APoint(0, Main.Height + 10), Text = "Строительство" };
            Units = new AButton(new ASize(250, 50)) { Location = Building.Location + new APoint(0, Building.Height + 10), Text = "Юниты" };

            SettlementInfoPanel = new SettlementInfoPanel(new ASize(Content.Width - Main.Width - 12, Content.Height - 2)) { Location = Main.Location + new APoint(Main.Width + 10, 0) };

            BuildingsInConstruction = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(Content.Width - Main.Width - 12, 200)) { Location = Main.Location + new APoint(Main.Width + 10, 0) };

            BuildingPanel = new BuildingPanel(new ASize(Content.Width - Main.Width - 12, Content.Height - 210 - 2)) { Location = BuildingsInConstruction.Location + new APoint(0, BuildingsInConstruction.Height + 10) };
            UnitsPanel = new CreateUnitsPanel(new ASize(Content.Width - Main.Width - 12, Content.Height - 2)) { Location = Main.Location + new APoint(Main.Width + 10, 0) };

            Add(Main);
            Add(Building);
            Add(Units);

            Add(SettlementInfoPanel);
            Add(BuildingsInConstruction);
            Add(BuildingPanel);
            Add(UnitsPanel);

            Main.MouseClickEvent += (state, mstate) => {
                HideChilds();

                Main.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                Building.BorderColor = GraphicsExtension.DefaultBorderColor;
                Units.BorderColor = GraphicsExtension.DefaultBorderColor;

                SettlementInfoPanel.Show(Settlement);
            };

            Building.MouseClickEvent += (state, mstate) =>
            {
                HideChilds();

                Main.BorderColor = GraphicsExtension.DefaultBorderColor;
                Building.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                Units.BorderColor = GraphicsExtension.DefaultBorderColor;

                BuildingsInConstruction.Enabled = true;

                BuildingPanel.Show(Settlement.Owner, Settlement);
            };

            Units.MouseClickEvent += (state, mstate) =>
            {
                HideChilds();

                Main.BorderColor = GraphicsExtension.DefaultBorderColor;
                Building.BorderColor = GraphicsExtension.DefaultBorderColor;
                Units.BorderColor = GraphicsExtension.DefaultDarkBorderColor;

                UnitsPanel.Show(Settlement.Owner);
            };

            SettlementInfoPanel.TechnologySelectEvent += (settlement) =>
            {
                TechnologyTreeSelectEvent?.Invoke(settlement);
                SettlementInfoPanel.Show(Settlement);
            };

            UnitsPanel.SelectEvent += (unit) =>
            {
                UnitSelectEvent?.Invoke(unit, Settlement.Location);
                UnitsPanel.Show(Settlement.Owner);
            };

            BuildingPanel.SelectEvent += (building) =>
            {
                Settlement.StartBuilding(building);
                UpdateBuildingsInConstruction(Settlement);
                BuildingPanel.Show(Settlement.Owner, Settlement);
                BuildingSelectEvent?.Invoke();
            };

            BuildingsInConstruction.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            BuildingsInConstruction.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            BuildingsInConstruction.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

        }

        public void Update(ISettlement settlement)
        {
            Settlement = settlement;

            Main.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
            Building.BorderColor = GraphicsExtension.DefaultBorderColor;
            Units.BorderColor = GraphicsExtension.DefaultBorderColor;

            UpdateBuildingsInConstruction(settlement);

            SettlementInfoPanel.Update(settlement);
            BuildingPanel.Update(Settlement.Owner, settlement);
            UnitsPanel.Update(Settlement.Owner);

            HideChilds();

            SettlementInfoPanel.Show(settlement);
        }

        public void Update() => Update(Settlement);

        //private void UpdateBuildingsInConstruction(ISettlement settlement) => BuildingsInConstruction.Text = "Проекты в разработке: \n\n" + string.Join("\n", settlement.BuildingsInConstruction.Select(x => x.Building.Name + " (" + x.TimeToComplete + ")"));
        private void UpdateBuildingsInConstruction(ISettlement settlement)
        {
            
            if (settlement is object) BuildingsInConstruction.Text = "Проекты в разработке: \n\n" + string.Join("\n", settlement.BuildingsInConstruction.Select(x => x.Building.Name + " (" + x.TimeToComplete + ")"));
        }

        public void Hide() => Enabled = false;

        private void HideChilds() {
            BuildingsInConstruction.Enabled = false;
            SettlementInfoPanel.Enabled = false;
            BuildingPanel.Enabled = false;
            UnitsPanel.Enabled = false;
        }
        public void Show(ISettlement settlement)
        {
            Enabled = true;
            Update(settlement);

            Text = "Наместник города " + settlement.Name;

            ShowEvent?.Invoke();
        }

    }
}
