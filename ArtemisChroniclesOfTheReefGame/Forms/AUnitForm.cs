using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Unit.Main;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{
    public class AUnitForm: AForm
    {

        public delegate void OnClick(IUnit unit);

        public event OnClick RenameEvent;
        public event OnClick DestroyEvent;
        public event OnClick GeneralEvent;
        public event OnClick WorkEvent;
        public event OnClick EstablishEvent;

        AUnitPanel UnitPanel;

        public AUnitForm(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            UnitPanel = new AUnitPanel(Content.Size - 2) { Location = new APoint(1, 1) };

            UnitPanel.RenameEvent += (unit) => RenameEvent?.Invoke(unit);
            UnitPanel.DestroyEvent += (unit) => DestroyEvent?.Invoke(unit);
            UnitPanel.GeneralEvent += (unit) => GeneralEvent?.Invoke(unit);
            UnitPanel.WorkEvent += (unit) => WorkEvent?.Invoke(unit);
            UnitPanel.EstablishEvent += (unit) => EstablishEvent?.Invoke(unit);

            Add(UnitPanel);

        }

        public void Update(GameData gameData, IUnit unit)
        {
            UnitPanel.Update(gameData, unit);
        }

        public void Hide() => Enabled = false;

        public void Show(GameData gameData, IUnit unit)
        {

            Enabled = true;
            Update(gameData, unit);

            Text = unit.Name + " (" + unit.UnitTypeName + ">)";

        }

    }
}
