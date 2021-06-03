using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Building;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class ASettlementBuildingPanel: AEmptyPanel
    {

        public delegate void OnBuildingSelect(IBuilding building);

        public event OnBuildingSelect BuildingSelectEvent;

        private ABuildingPanel BuildingPanel;
        private AScrolleredPanel BuildingsInConstruction;

        public ASettlementBuildingPanel(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            BuildingsInConstruction = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(Width - 20, 200)) { Parent = this, Location = new APoint(10, 10) };
            BuildingPanel = new ABuildingPanel(new ASize(Width - 20, Height - BuildingsInConstruction.Height - 20)) { Parent = this, Location = BuildingsInConstruction.Location + new APoint(0, BuildingsInConstruction.Height + 10) };

            BuildingPanel.SelectEvent += (building) =>  BuildingSelectEvent?.Invoke(building);

            BuildingsInConstruction.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            BuildingsInConstruction.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            BuildingsInConstruction.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

        }

        public void Update(ISettlement settlement)
        {

            if (settlement is object) BuildingsInConstruction.Text = "Проекты в разработке: \n\n" + string.Join("\n", settlement.BuildingsInConstruction.Select(x => x.Building.Name + " (" + x.TimeToComplete + ")"));
            BuildingPanel.Update(settlement.Owner, settlement);

        }

        public void Hide() => Enabled = false;
        public void Show(ISettlement settlement)
        {
            Enabled = true;
            Update(settlement);
        }

    }
}
