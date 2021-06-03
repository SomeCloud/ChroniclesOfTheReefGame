using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Building;
using GameLibrary.Unit.Main;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{
    public class ASettlementForm: AForm
    {

        public delegate void OnBuildingSelect(IBuilding building);
        public delegate void OnTechnologySelect(ISettlement settlement);
        public delegate void OnUnitSelect(AUnitType unitType);

        public event OnBuildingSelect BuildingCreateEvent;
        public event OnTechnologySelect TechnologySelectEvent;
        public event OnUnitSelect UnitCreateEvent;

        private ASettlementPanel SettlementPanel;

        public ASettlementForm(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            SettlementPanel = new ASettlementPanel(Content.Size - 2) { Location = new APoint(1, 1) };

            SettlementPanel.BuildingCreateEvent += (building) => BuildingCreateEvent?.Invoke(building);
            SettlementPanel.TechnologySelectEvent += (settlement) => TechnologySelectEvent?.Invoke(settlement);
            SettlementPanel.UnitCreateEvent += (unit) => UnitCreateEvent?.Invoke(unit);

            Add(SettlementPanel);

        }

        public void Update(ISettlement settlement)
        {

            SettlementPanel.Update(settlement);

        }

        public void Hide() => Enabled = false;
        public void Show(ISettlement settlement)
        {
            Enabled = true;
            Update(settlement);
        }

    }
}
