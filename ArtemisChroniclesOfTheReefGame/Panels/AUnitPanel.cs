using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Unit.Main;
using GameLibrary.Map;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class AUnitPanel: AEmptyPanel
    {

        public delegate void OnClick(IUnit unit);

        public event OnClick RenameEvent;
        public event OnClick DestroyEvent;
        public event OnClick GeneralEvent;
        public event OnClick WorkEvent;
        public event OnClick EstablishEvent;

        private APanel UnitIcon;
        private APanel UnitStats;

        private AUnitControlPanel UnitControlPanel;

        public AUnitPanel(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            UnitIcon = new APanel(new ASize(195, 195)) { Parent = this, Location = new APoint(10, 10) };
            UnitStats = new APanel(new ASize(Width - UnitIcon.Width - 30, UnitIcon.Height)) { Parent = this, Location = UnitIcon.Location + new APoint(UnitIcon.Width + 10, 0) };
            UnitControlPanel = new AUnitControlPanel(new ASize(Width - 20, Height - UnitIcon.Height - 30)) { Parent = this, Location = UnitIcon.Location + new APoint(0, UnitIcon.Height + 10) };

            UnitControlPanel.RenameEvent += (unit) => RenameEvent?.Invoke(unit);
            UnitControlPanel.DestroyEvent += (unit) => DestroyEvent?.Invoke(unit);
            UnitControlPanel.GeneralEvent += (unit) => GeneralEvent?.Invoke(unit);
            UnitControlPanel.WorkEvent += (unit) => WorkEvent?.Invoke(unit);
            UnitControlPanel.EstablishEvent += (unit) => EstablishEvent?.Invoke(unit);

        }

        public void Update(GameData gameData, IUnit unit)
        {

            UnitIcon.SetTexture(TexturePack.Unit(unit.UnitType));
            UnitStats.Text = unit.Name + " (" + unit.UnitTypeName + ")\n" + "Количество: " + unit.Count + "\nВладелец: " + unit.Owner.Name + "\nАтака: " + unit.Force + "\nДвижения: " + unit.Action + " / " + unit.ActionMaxValue;

            if (gameData.GetMapCell(unit.Location) is AMapCell mapCell && mapCell.IsResource)
                if (mapCell.IsMined) UnitControlPanel.Work.FillColor = GraphicsExtension.ExtraColorRed;
                else UnitControlPanel.Work.FillColor = GraphicsExtension.ExtraColorGreen;
            else UnitControlPanel.Work.FillColor = GraphicsExtension.DefaultFillColor;

            Enabled = true;
        }

    }
}
