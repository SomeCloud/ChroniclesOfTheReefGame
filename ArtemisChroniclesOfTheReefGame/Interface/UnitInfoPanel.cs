using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Extension;
using GameLibrary.Map;
using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Interface
{

    public class UnitInfoPanel : APanel
    {

        public delegate void OnUpdate();
        public event OnUpdate UpdateEvent;

        private IUnit Unit;
        private AGame Game;

        private APanel UnitIcon;
        private APanel UnitStats;
        private APanel UnitButtons;

        private AButton _CloseSettlementForm;

        private AButton Rename;
        private AButton Destroy;

        private TextBoxPanel RenameForm;

        public AButton CloseSettlementForm => _CloseSettlementForm;

        public UnitInfoPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            UnitIcon = new APanel(new ASize(195, 195)) { Parent = this, Location = new APoint(10, 50) };
            UnitStats = new APanel(new ASize(Width - UnitIcon.Width - 30, UnitIcon.Height)) { Parent = this, Location = UnitIcon.Location + new APoint(UnitIcon.Width + 10, 0) };
            UnitButtons = new APanel(new ASize(Width - 20, Height - UnitIcon.Height - 70)) { Parent = this, Location = UnitIcon.Location + new APoint(0, UnitIcon.Height + 10) };

            _CloseSettlementForm = new AButton(new ASize(40, 40)) { Parent = this, Location = new APoint(Width - 40, 0), Text = "×" };

            Rename = new AButton(new ASize((UnitButtons.Width - 30) / 2, 50)) { Parent = UnitButtons, Location = new APoint(10, 10), Text = "Переименовать" };
            Destroy = new AButton(new ASize((UnitButtons.Width - 30) / 2, 50)) { Parent = UnitButtons, Location = Rename.Location + new APoint(Rename.Width + 10, 0), Text = "Уничтожить" };

            RenameForm = new TextBoxPanel(new ASize(400, 250)) { Parent = this, Text = "Переименование" };

            RenameForm.Location = ((Size - RenameForm.Size) / 2).ToAPoint();

            _CloseSettlementForm.MouseClickEvent += (state, mstate) => { Enabled = false; RenameForm.Enabled = false; };

            Rename.MouseClickEvent += (state, mstate) =>
            {
                if (Unit is object) RenameForm.Update(Unit.Name);
                RenameForm.Location = ((Size - RenameForm.Size) / 2).ToAPoint();
            };

            Destroy.MouseClickEvent += (state, mstate) =>
            {
                if (Unit is object)
                {
                    Game.GetMapCell(Unit.Homeland).Population.Add(Unit.Squad.ToList());
                    Unit.Owner.RemoveUnit(Unit);
                    Unit = null;
                }
                UpdateEvent?.Invoke();
                Enabled = false;
            };

            RenameForm.ResultEvent += (state, text) =>
            {
                if (state || text.Length > 0)
                {
                    Unit.SetName(text);
                    UpdateEvent?.Invoke();
                    Update(Unit);
                    RenameForm.Enabled = false;
                }
            };

            RenameForm.Enabled = false;

        }

        public void Update(IUnit unit)
        {

            Unit = unit;

            RenameForm.Location = ((Size - RenameForm.Size) / 2).ToAPoint();
            RenameForm.Enabled = false;
            UnitIcon.SetTexture(TexturePack.Unit(unit.UnitType));
            UnitStats.Text = unit.Name + " (" + unit.UnitTypeName + ")\n" + "Количество: " + unit.Count + "\nВладелец: " + unit.Owner.Name + "\nАтака: " + unit.Force + "\nДвижения: " + unit.Action + " / " + unit.ActionMaxValue;

            Enabled = true;

        }

    }

}
