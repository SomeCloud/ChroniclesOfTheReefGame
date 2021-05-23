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
using GameLibrary.Character;
using GameLibrary.Map;
using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Interface
{

    public class UnitInfoPanel : APanel
    {

        public delegate void OnUpdate();
        public delegate void OnDestroy();

        public event OnUpdate UpdateEvent;
        public event OnDestroy DestroyEvent;

        private IUnit Unit;
        private AGame Game;

        private APanel UnitIcon;
        private APanel UnitStats;
        private APanel UnitButtons;

        private AButton Rename;
        private AButton Destroy;
        private AButton General;
        private AButton Work;
        private AButton Establish;

        private TextBoxPanel RenameForm;
        private TextBoxPanel SettlementNameForm;
        private CharactersListPanel CharactersListPanel;

        private AContainer ButtonsContainer;
        private List<Action<IUnit>> Buttons;

        public UnitInfoPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            UnitIcon = new APanel(new ASize(195, 195)) { Parent = this, Location = new APoint(10, 10) };
            UnitStats = new APanel(new ASize(Width - UnitIcon.Width - 30, UnitIcon.Height)) { Parent = this, Location = UnitIcon.Location + new APoint(UnitIcon.Width + 10, 0) };
            UnitButtons = new APanel(new ASize(Width - 20, Height - UnitIcon.Height - 30)) { Parent = this, Location = UnitIcon.Location + new APoint(0, UnitIcon.Height + 10) };

            Rename = new AButton(new ASize((UnitButtons.Width - 30) / 2, 50)) { Parent = UnitButtons, Location = new APoint(10, 10), Text = "Переименовать" };
            Destroy = new AButton(new ASize((UnitButtons.Width - 30) / 2, 50)) { Parent = UnitButtons, Location = Rename.Location + new APoint(Rename.Width + 10, 0), Text = "Уничтожить" };
            General = new AButton(new ASize((UnitButtons.Width - 30) / 2, 50)) { Parent = UnitButtons, Location = Rename.Location + new APoint(0, Rename.Height + 10), Text = "Полководец" };
            Work = new AButton(new ASize((UnitButtons.Width - 30) / 2, 50)) { Parent = UnitButtons, Location = Rename.Location + new APoint(General.Width + 10, 0), Text = "Добыча" };
            Establish = new AButton(new ASize((UnitButtons.Width - 30) / 2, 50)) { Parent = UnitButtons, Location = Work.Location + new APoint(0, Work.Height + 10), Text = "Поселиться" };

            RenameForm = new TextBoxPanel(new ASize(400, 250)) { Parent = this, Text = "Переименование" };
            SettlementNameForm = new TextBoxPanel(new ASize(400, 250)) { Parent = this, Text = "Новое поселение" };
            CharactersListPanel = new CharactersListPanel(Game, Size) { Parent = this, Location = new APoint(0, 0) };

            RenameForm.Location = ((Size - RenameForm.Size) / 2).ToAPoint();

            ButtonsContainer = new AContainer(UnitButtons.Size - 10, UnitButtons, new APoint(10, 10));

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
                    AMapCell mapCell = Game.GetMapCell(Unit.Location);
                    Unit.Owner.RemoveUnit(Unit);
                    mapCell.SetActiveUnit(Game.GetUnits(mapCell.Location) is List<IUnit> units && units.Count > 0? units.First(): null);
                    Unit.Dispose();
                    Unit = null;
                }
                DestroyEvent.Invoke();
                Enabled = false;
            };

            Work.MouseClickEvent += (state, mstate) =>
            {
                if (Game.GetMapCell(Unit.Location) is AMapCell mapCell && mapCell.IsResource)
                {
                    if (mapCell.IsMined)
                    {
                        mapCell.UnsetActiveWorker();
                        Work.Text = "Добыча";
                    }
                    else
                    {
                        mapCell.SetActiveWorker(Unit);
                        Work.Text = "Не добывать";
                    }
                    Update(Unit);
                    UpdateEvent?.Invoke();
                }
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

            SettlementNameForm.ResultEvent += (state, text) =>
            {
                if (state || text.Length > 0)
                {
                    if (Game.AddSettlement(Unit, text)) DestroyEvent.Invoke();
                }
            };

            Buttons = new List<Action<IUnit>>();

            Buttons.Add((IUnit unit) => Rename.Enabled = true);
            Buttons.Add((IUnit unit) => Destroy.Enabled = true);
            Buttons.Add((IUnit unit) => General.Enabled = new List<AUnitType>() { AUnitType.Spearman, AUnitType.Swordsman, AUnitType.Archer, AUnitType.Axeman, AUnitType.Warrior }.Contains(unit.UnitType));
            Buttons.Add((IUnit unit) => Work.Enabled = unit.Owner.IsResource(Game.GetMapCell(Unit.Location).ResourceType) && Game.GetMapCell(Unit.Location).IsResource && unit.UnitType.Equals(AUnitType.Farmer));
            Buttons.Add((IUnit unit) => Establish.Enabled = Game.GetMapCell(Unit.Location).NeighboringCells.All(x => x.Owner is null) && unit.UnitType.Equals(AUnitType.Colonist));

            ButtonsContainer.Add(Rename);
            ButtonsContainer.Add(Destroy);
            ButtonsContainer.Add(General);
            ButtonsContainer.Add(Work);
            ButtonsContainer.Add(Establish);

            CharactersListPanel.SelectCharacterEvent += (character) => { Unit.SetGeneral(character); CharactersListPanel.Enabled = false; UpdateEvent?.Invoke(); };
            CharactersListPanel.KeyDownEvent += (state, kstate) => { if (kstate.KeyState.Equals(AKeyState.Exit)) CharactersListPanel.Enabled = false; };

            General.MouseClickEvent += (state, mstate) =>
            {
                List<ICharacter> characters = Game.Characters.Values.SelectMany(x => x).Where(x => Game.ActivePlayer.Territories.Select(x => x.Location).Contains(x.Location) && x.Age(Game.CurrentTurn) >= 16 && !Game.Players.Values.Where(x => !x.Equals(Game.ActivePlayer)).SelectMany(x => x.Characters).Contains(x)).ToList();
                if (characters.Count > 0)
                {
                    CharactersListPanel.Update(characters);
                    CharactersListPanel.Enabled = true;
                }
            };

            Establish.MouseClickEvent += (state, mstate) =>
            {
                SettlementNameForm.Update("Поселение");
                SettlementNameForm.Location = ((Size - SettlementNameForm.Size) / 2).ToAPoint();
            };

            SettlementNameForm.Enabled = false;
            RenameForm.Enabled = false;
            CharactersListPanel.Enabled = false;

        }

        public void Update(IUnit unit)
        {

            Unit = unit;

            foreach (Action<IUnit> action in Buttons) action(unit);

            UnitButtons.Enabled = Game.ActivePlayer.Equals(unit.Owner);

            RenameForm.Location = ((Size - RenameForm.Size) / 2).ToAPoint();
            RenameForm.Enabled = false;
            UnitIcon.SetTexture(TexturePack.Unit(unit.UnitType));
            UnitStats.Text = unit.Name + " (" + unit.UnitTypeName + ")\n" + "Количество: " + unit.Count + "\nВладелец: " + unit.Owner.Name + "\nАтака: " + unit.Force + "\nДвижения: " + unit.Action + " / " + unit.ActionMaxValue;

            if (Game.GetMapCell(unit.Location) is AMapCell mapCell && mapCell.IsResource)
                if (mapCell.IsMined) Work.FillColor = GraphicsExtension.ExtraColorRed;
                else Work.FillColor = GraphicsExtension.ExtraColorGreen;
            else Work.FillColor = GraphicsExtension.DefaultFillColor;

            Enabled = true;

        }

        public void Close()
        {
            RenameForm.Enabled = false; 
        }

    }

}
