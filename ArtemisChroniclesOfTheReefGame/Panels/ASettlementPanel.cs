using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary.Settlement;
using GameLibrary.Settlement.Building;
using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Panels
{


    public class ASettlementPanel: AEmptyPanel
    {

        public delegate void OnBuildingSelect(IBuilding building);
        public delegate void OnTechnologySelect(ISettlement settlement);
        public delegate void OnUnitSelect(AUnitType unitType);

        public event OnBuildingSelect BuildingCreateEvent;
        public event OnTechnologySelect TechnologySelectEvent;
        public event OnUnitSelect UnitCreateEvent;

        private AButton Main;
        private AButton Building;
        private AButton Unit;

        private ASettlementBuildingPanel BuildingPanel;
        private ASettlementInfoPanel InfoPanel;
        private CreateUnitsPanel CreateUnitsPanel;

        ISettlement Settlement;

        public ASettlementPanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            Main = new AButton(GraphicsExtension.DefaultButtonSize) { Parent = this, Location = new APoint(10, 10), Text = "Поселение" };
            Building = new AButton(GraphicsExtension.DefaultButtonSize) { Parent = this, Location = Main.Location + new APoint(0, Main.Height + 10), Text = "Строительство" };
            Unit = new AButton(GraphicsExtension.DefaultButtonSize) { Parent = this, Location = Building.Location + new APoint(0, Building.Height + 10), Text = "Тренировать юнита" };

            BuildingPanel = new ASettlementBuildingPanel(new ASize(Width - Main.Height - 30, Height - 20)) { Parent = this, Location = Main.Location + new APoint(Main.Width + 10, 0) };
            InfoPanel = new ASettlementInfoPanel(new ASize(Width - Main.Height - 30, Height - 20)) { Parent = this, Location = Main.Location + new APoint(Main.Width + 10, 0) };
            CreateUnitsPanel = new CreateUnitsPanel(new ASize(Width - Main.Height - 30, Height - 20)) { Parent = this, Location = Main.Location + new APoint(Main.Width + 10, 0) };

            BuildingPanel.BuildingSelectEvent += (building) => BuildingCreateEvent?.Invoke(building);
            InfoPanel.TechnologySelectEvent += () => TechnologySelectEvent?.Invoke(Settlement);
            CreateUnitsPanel.SelectEvent += (unit) => UnitCreateEvent?.Invoke(unit);

            Main.MouseClickEvent += (state, mstate) => SetMain();

            Building.MouseClickEvent += (state, mstate) => SetBuilding();

            Unit.MouseClickEvent += (state, mstate) => SetUnit();

            InfoPanel.Enabled = true;
            BuildingPanel.Enabled = false;
            CreateUnitsPanel.Enabled = false;

        }

        private void SetMain()
        {

            HidePanels();

            InfoPanel.Enabled = true;

            Main.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
            Building.BorderColor = GraphicsExtension.DefaultBorderColor;
            Unit.BorderColor = GraphicsExtension.DefaultBorderColor;

        }
        
        private void SetBuilding()
        {

            HidePanels();

            BuildingPanel.Enabled = true;

            Main.BorderColor = GraphicsExtension.DefaultBorderColor;
            Building.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
            Unit.BorderColor = GraphicsExtension.DefaultBorderColor;

        }
        
        private void SetUnit()
        {

            HidePanels();

            CreateUnitsPanel.Enabled = true;

            Main.BorderColor = GraphicsExtension.DefaultBorderColor;
            Building.BorderColor = GraphicsExtension.DefaultBorderColor;
            Unit.BorderColor = GraphicsExtension.DefaultDarkBorderColor;

        }

        private void HidePanels()
        {
            InfoPanel.Enabled = false;
            BuildingPanel.Enabled = false;
            CreateUnitsPanel.Enabled = false;
        }

        public void Update(ISettlement settlement)
        {

            Settlement = settlement;

            InfoPanel.Update(settlement);
            BuildingPanel.Update(settlement);
            CreateUnitsPanel.Update(settlement.Owner);

        }

        public void Hide() => Enabled = false;
        public void Show(ISettlement settlement)
        {
            Enabled = true;
            Update(settlement);
        }

    }

}
