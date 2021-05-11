using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Unit;
using GameLibrary.Unit.Main;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class UnitsPanel: APanel
    {

        public event MapCellPanel.OnUpdate UpdateEvent;

        private AGame Game;

        private APanel UnitsCreateHeaderPanel;
        private AScrolleredPanel UnitsInSettlementPanel;
        private AScrolleredPanel UnitsCreatePanel;

        private UnitInfoPanel UnitInfoPanel;

        private AButton CreateColonist;
        private AButton CreateSpearman;
        private AButton CreateFarmer;
        private AButton CreateSwordsman;
        private AButton CreateArcher;
        private AButton CreateMissionary;
        private AButton CreateAxeman;
        private AButton CreateWarrior;

        private Dictionary<IUnit, AButton> UnitButton;

        public UnitsPanel(AGame game, ASize size): base(size)
        {
            Game = game;
            UnitButton = new Dictionary<IUnit, AButton>();
        }

        public override void Initialize()
        {
            base.Initialize();

            int width = (Width - 30) / 2;

            UnitsCreatePanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(width, Height - 20)) { Parent = this, Location = new APoint(10, 10) };
            UnitsInSettlementPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(width, Height - 20)) { Parent = this, Location = UnitsCreatePanel.Location + new APoint(UnitsCreatePanel.Width + 10, 0) };

            UnitInfoPanel = new UnitInfoPanel(Game, new ASize(width, Height - 20)) { Parent = this, Location = new APoint(10, 10) };

            AUnitColonist unitColonist = new AUnitColonist(0, null, new APoint(0, 0), null, "");
            AUnitSpearman unitSpearman = new AUnitSpearman(0, null, new APoint(0, 0), null, "");
            AUnitFarmer unitFarmer = new AUnitFarmer(0, null, new APoint(0, 0), null, "");
            AUnitSwordsman unitSwordsman = new AUnitSwordsman(0, null, new APoint(0, 0), null, "");
            AUnitArcher unitArcher = new AUnitArcher(0, null, new APoint(0, 0), null, "");
            AUnitMissionary unitMissionary = new AUnitMissionary(0, null, new APoint(0, 0), null, "");
            AUnitAxeman unitAxeman = new AUnitAxeman(0, null, new APoint(0, 0), null, "");
            AUnitWarrior unitWarrior = new AUnitWarrior(0, null, new APoint(0, 0), null, "");

            UnitsCreateHeaderPanel = new APanel(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = new APoint(10, 10), Text = StringPad("Название", 10) + " | " + StringPad("Стоимость", 10) + " | " + StringPad("Атака", 10) + " | " + StringPad("Действие", 10) };

            CreateColonist = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = UnitsCreateHeaderPanel.Location + new APoint(0, UnitsCreateHeaderPanel.Height + 10), Text = StringPad(unitColonist.UnitTypeName, 10) + " | " + StringPad(unitColonist.Cost, 10) + " | " + StringPad(unitColonist.Force, 10) + " | " + StringPad(unitColonist.ActionMaxValue, 10) };
            CreateSpearman = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = CreateColonist.Location + new APoint(0, CreateColonist.Height + 10), Text = StringPad(unitSpearman.UnitTypeName, 10) + " | " + StringPad(unitSpearman.Cost, 10) + " | " + StringPad(unitSpearman.Force, 10) + " | " + StringPad(unitSpearman.ActionMaxValue, 10) };
            CreateFarmer = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = CreateSpearman.Location + new APoint(0, CreateSpearman.Height + 10), Text = StringPad(unitFarmer.UnitTypeName, 10) + " | " + StringPad(unitFarmer.Cost, 10) + " | " + StringPad(unitFarmer.Force, 10) + " | " + StringPad(unitFarmer.ActionMaxValue, 10) };
            CreateSwordsman = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = CreateFarmer.Location + new APoint(0, CreateFarmer.Height + 10), Text = StringPad(unitSwordsman.UnitTypeName, 10) + " | " + StringPad(unitSwordsman.Cost, 10) + " | " + StringPad(unitSwordsman.Force, 10) + " | " + StringPad(unitSwordsman.ActionMaxValue, 10) };
            CreateArcher = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = CreateSwordsman.Location + new APoint(0, CreateSwordsman.Height + 10), Text = StringPad(unitArcher.UnitTypeName, 10) + " | " + StringPad(unitArcher.Cost, 10) + " | " + StringPad(unitArcher.Force, 10) + " | " + StringPad(unitArcher.ActionMaxValue, 10) };
            CreateMissionary = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = CreateArcher.Location + new APoint(0, CreateArcher.Height + 10), Text = StringPad(unitMissionary.UnitTypeName, 10) + " | " + StringPad(unitMissionary.Cost, 10) + " | " + StringPad(unitMissionary.Force, 10) + " | " + StringPad(unitMissionary.ActionMaxValue, 10) };
            CreateAxeman = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = CreateMissionary.Location + new APoint(0, CreateMissionary.Height + 10), Text = StringPad(unitAxeman.UnitTypeName, 10) + " | " + StringPad(unitAxeman.Cost, 10) + " | " + StringPad(unitAxeman.Force, 10) + " | " + StringPad(unitAxeman.ActionMaxValue, 10) };
            CreateWarrior = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 50)) { Location = CreateAxeman.Location + new APoint(0, CreateAxeman.Height + 10), Text = StringPad(unitWarrior.UnitTypeName, 10) + " | " + StringPad(unitWarrior.Cost, 10) + " | " + StringPad(unitWarrior.Force, 10) + " | " + StringPad(unitWarrior.ActionMaxValue, 10) };

            UnitsCreatePanel.Add(UnitsCreateHeaderPanel);

            UnitsCreatePanel.Add(CreateColonist);
            UnitsCreatePanel.Add(CreateSpearman);
            UnitsCreatePanel.Add(CreateFarmer);
            UnitsCreatePanel.Add(CreateSwordsman);
            UnitsCreatePanel.Add(CreateArcher);
            UnitsCreatePanel.Add(CreateMissionary);
            UnitsCreatePanel.Add(CreateAxeman);
            UnitsCreatePanel.Add(CreateWarrior);

            UnitsCreatePanel.ContentSize = new ASize(UnitsCreatePanel.ContentSize.Width, CreateWarrior.Y + 60);
            UnitsCreatePanel.Scrollbar.MaxValue = UnitsCreatePanel.Height < UnitsCreatePanel.ContentSize.Height + 10 ? UnitsCreatePanel.ContentSize.Height - UnitsCreatePanel.Height + 10 : 10;
            
            UnitsCreateHeaderPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            CreateColonist.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            CreateSpearman.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            CreateFarmer.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            CreateSwordsman.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            CreateArcher.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            CreateMissionary.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            CreateAxeman.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            CreateWarrior.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            CreateColonist.TextLabel.VerticalAlign = ATextVerticalAlign.Center;
            CreateSpearman.TextLabel.VerticalAlign = ATextVerticalAlign.Center;
            CreateFarmer.TextLabel.VerticalAlign = ATextVerticalAlign.Center;
            CreateSwordsman.TextLabel.VerticalAlign = ATextVerticalAlign.Center;
            CreateArcher.TextLabel.VerticalAlign = ATextVerticalAlign.Center;
            CreateMissionary.TextLabel.VerticalAlign = ATextVerticalAlign.Center;
            CreateAxeman.TextLabel.VerticalAlign = ATextVerticalAlign.Center;
            CreateWarrior.TextLabel.VerticalAlign = ATextVerticalAlign.Center;

            UnitsCreateHeaderPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            UnitsInSettlementPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            CreateColonist.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            CreateSpearman.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            CreateFarmer.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            CreateSwordsman.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            CreateArcher.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            CreateMissionary.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            CreateAxeman.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            CreateWarrior.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            CreateColonist.MouseClickEvent += (state, mstate) => { SetUnit(AUnitType.Colonist, unitColonist); };
            CreateSpearman.MouseClickEvent += (state, mstate) => { SetUnit(AUnitType.Spearman, unitSpearman); };
            CreateFarmer.MouseClickEvent += (state, mstate) => { SetUnit(AUnitType.Farmer, unitFarmer); };
            CreateSwordsman.MouseClickEvent += (state, mstate) => { SetUnit(AUnitType.Swordsman, unitSwordsman); };
            CreateArcher.MouseClickEvent += (state, mstate) => { SetUnit(AUnitType.Archer, unitArcher); };
            CreateMissionary.MouseClickEvent += (state, mstate) => { SetUnit(AUnitType.Missionary, unitMissionary);  };
            CreateAxeman.MouseClickEvent += (state, mstate) => { SetUnit(AUnitType.Axeman, unitAxeman); };
            CreateWarrior.MouseClickEvent += (state, mstate) => { SetUnit(AUnitType.Warrior, unitWarrior); };

            UnitInfoPanel.UpdateEvent += () => Update(Game.SelectedMapCell.Location);

        }

        public void SetUnit(AUnitType unitType, IUnit unit)
        {
            Game.AddUnit(unitType, Game.SelectedMapCell.Population.Subtract(100), unit.UnitTypeName);
            Update(Game.SelectedMapCell.Location);
        }

        public void Update(APoint location)
        {

            APoint last = new APoint(10, -100);
            AButton button;
            foreach (AButton bt in UnitButton.Values) bt.Enabled = false;

            foreach (IUnit unit in Game.GetUnits(location))
            {
                if (UnitButton.ContainsKey(unit))
                {
                    button = UnitButton[unit];
                }
                else
                {
                    button = new AButton(new ASize(UnitsInSettlementPanel.ContentSize.Width - 20, 100));

                    button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 9);

                    button.MouseClickEvent += (state, mstate) => { Game.GetMapCell(unit.Location).SetActiveUnit(unit); UnitInfoPanel.Update(unit); };

                    UnitsInSettlementPanel.Add(button);
                    UnitButton.Add(unit, button);
                }
                string text = unit.ToString();
                if (!button.Text.Equals(text)) button.Text = text;
                button.Enabled = true;

                button.Location = last + new APoint(0, button.Height + 10);
                last = button.Location;
            }

            UnitInfoPanel.Enabled = false;

            UnitsInSettlementPanel.ContentSize = new ASize(UnitsInSettlementPanel.ContentSize.Width, UnitButton.Count > 0 ? last.Y + 110 : Height);
            UnitsInSettlementPanel.Scrollbar.MaxValue = Height < UnitsInSettlementPanel.ContentSize.Height ? UnitsInSettlementPanel.ContentSize.Height - Height + 10 : 0;
            
            //ShowUnits(location);
            UpdateEvent?.Invoke(Game.GetMapCell(location));
        }

        private void ShowUnits(APoint location)
        {

            //AMapCell mapCell = Game.GetMapCell(location);
            //string s = mapCell.IsUnit ? string.Join("\n", mapCell.Units.Select(x => x)) : "Юниты отсутствуют";
            string s = Game.GetUnits(location) is List<IUnit> units && units.Count > 0? string.Join("\n", units) : "Юниты отсутствуют";
            UnitsInSettlementPanel.ContentSize = new ASize(UnitsInSettlementPanel.ContentSize.Width, 40 + (s.Count(x => x == '\n') + 1) * UnitsInSettlementPanel.TextLabel.Font.Height);
            UnitsInSettlementPanel.Text = s;
            UnitsInSettlementPanel.Scrollbar.MaxValue = UnitsInSettlementPanel.Height < UnitsInSettlementPanel.ContentSize.Height ? UnitsInSettlementPanel.ContentSize.Height - UnitsInSettlementPanel.Height : 0;
        
        }

        public void ShowCreatePanel(bool status)
        {
            UnitsCreatePanel.Enabled = status;
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
