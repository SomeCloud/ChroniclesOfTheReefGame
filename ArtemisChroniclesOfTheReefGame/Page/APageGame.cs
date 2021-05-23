using System;
using System.Collections.Generic;
using System.Linq;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Extension;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Character;
using GameLibrary.Unit;
using GameLibrary.Unit.Main;

using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;
using OnMouseEvent = GraphicsLibrary.Interfaces.OnMouseEvent;

using ArtemisChroniclesOfTheReefGame.Interface;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    public class APageGame : APage, IPage
    {

        private AGame COTR;

        private APanel MapField;

        private APanel PlayerStatisticPanel;
        private APanel ExtraPanel;
        private APanel GameTimeStatistic;

        private PlayerPanel PlayerPanel;
        private WorldPanel WorldPanel;
        private TechnologyTreePanel TechnologyTreePanel;
        private ExtraSidePanel ExtraSidePanel;
        private UnitPanel UnitInfoPanel;
        private CharacterPanel CharacterPanel;
        private MessageListPanelForm MessageListPanel;
        private MessageForm MessagePanel;

        private SettlementPanel SettlementPanel;

        private BattleForm BattleForm;

        private AButton Turn;
        private AButton Messages;

        private AMapView MapView;

        private APanel ButtonsPanel;
        private AScrolleredPanel ExtraStatisticPanel;
        private AScrolleredPanel UnitsPanel;

        private AContainer ButtonsPanelContainer;

        private APoint location;

        OnMouseEvent OnOverEvent;

        public APageGame(IPrimitive primitive) : base(primitive)
        {

            PlayerStatisticPanel = new APanel(new ASize(Parent.Width - 30 - (GraphicsExtension.DefaultMiniButtonSize.Width * 2) - 220, GraphicsExtension.DefaultMiniButtonSize.Height)) { Location = new APoint(10, 10) };
            Messages = new AButton(GraphicsExtension.DefaultMiniButtonSize, TexturePack.MiniButtons_Message) { Location = new APoint(PlayerStatisticPanel.X + PlayerStatisticPanel.Width + 10, 10) };
            GameTimeStatistic = new APanel(new ASize(200, GraphicsExtension.DefaultMiniButtonSize.Height)) { Location = new APoint(Messages.X + Messages.Width + 10, 10) };
            Turn = new AButton(GraphicsExtension.DefaultMiniButtonSize, TexturePack.MiniButtons_General_Turn) { Location = new APoint(GameTimeStatistic.X + GameTimeStatistic.Width + 10, 10), IsCounting = false };
            MapField = new APanel(new ASize(Parent.Width - 20, Parent.Height - GraphicsExtension.DefaultMiniButtonSize.Height - 30), GraphicsExtension.DefaultDarkFillColor) { Location = new APoint(10, PlayerStatisticPanel.Y + PlayerStatisticPanel.Height + 10) };
            ExtraPanel = new APanel(new ASize(Parent.Width - 20, 20)) { Location = MapField.Location + new APoint(0, MapField.Height - 20) };

            Add(MapField);
            Add(PlayerStatisticPanel);
            Add(GameTimeStatistic);
            Add(ExtraPanel);
            Add(Messages);
            Add(Turn);

            COTR = new AGame();

            //
            ASize BigFormSize = new ASize(Convert.ToInt32(MapField.Width * (2f / 3)), Convert.ToInt32(MapField.Height * (2.5f / 3)));
            ASize MiddleFormSize = new ASize(600, 600);
            location = (MapField.Size - BigFormSize).ToAPoint() / 2;

            ARectangleTexture bigRectangleTexture = new ARectangleTexture(Parent.GraphicsDevice, BigFormSize) { IsDraw = true, IsFill = true };
            ARectangleTexture MiddlerectangleTexture = new ARectangleTexture(Parent.GraphicsDevice, MiddleFormSize) { IsDraw = true, IsFill = true };

            PlayerPanel = new PlayerPanel(COTR, BigFormSize, bigRectangleTexture) { Location = MapField.Location + location, DragAndDrop = true };
            WorldPanel = new WorldPanel(COTR, BigFormSize, bigRectangleTexture) { Location = MapField.Location + location, DragAndDrop = true };
            TechnologyTreePanel = new TechnologyTreePanel(BigFormSize, bigRectangleTexture) { Location = MapField.Location + location, DragAndDrop = true };
            ExtraSidePanel = new ExtraSidePanel(COTR, new ASize(500, MapField.Height - ExtraPanel.Height)) { Location = new APoint(Parent.Width - 510, MapField.Y + 1) };

            UnitInfoPanel = new UnitPanel(COTR, MiddleFormSize, MiddlerectangleTexture);
            UnitInfoPanel.Location = MapField.Location + (MapField.Size - UnitInfoPanel.Size).ToAPoint() / 2;
            
            CharacterPanel = new CharacterPanel(COTR, MiddleFormSize, MiddlerectangleTexture);
            CharacterPanel.Location = MapField.Location + (MapField.Size - CharacterPanel.Size).ToAPoint() / 2;

            MessageListPanel = new MessageListPanelForm(MiddleFormSize, MiddlerectangleTexture);
            MessageListPanel.Location = MapField.Location + (MapField.Size - MessageListPanel.Size).ToAPoint() / 2;

            MessagePanel = new MessageForm(BigFormSize);
            MessagePanel.Location = MapField.Location + (MapField.Size - MessagePanel.Size).ToAPoint() / 2;

            SettlementPanel = new SettlementPanel(BigFormSize) { DragAndDrop = true };
            SettlementPanel.Location = MapField.Location + (MapField.Size - SettlementPanel.Size).ToAPoint() / 2;

            SettlementPanel.ShowEvent += () => SettlementPanel.Location = MapField.Location + (MapField.Size - SettlementPanel.Size).ToAPoint() / 2;
            PlayerPanel.ShowEvent += () => PlayerPanel.Location = MapField.Location + (MapField.Size - SettlementPanel.Size).ToAPoint() / 2;

            SettlementPanel.TechnologyTreeSelectEvent += (settlement) =>
            {
                TechnologyTreePanel.Show(COTR.GetMapCell(settlement.Location), settlement.Owner);
            };

            SettlementPanel.BuildingSelectEvent += () =>
            {
                PlayerStatisticPanel.Text = COTR.ActivePlayer.ToString();
                if (ExtraSidePanel.Enabled) ExtraSidePanel.Update(COTR.SelectedMapCell);
            };

            BattleForm = new BattleForm(new ASize(850, 450));
            BattleForm.Location = MapField.Location + (MapField.Size - BattleForm.Size).ToAPoint() / 2;

            Add(ExtraSidePanel);
            Add(PlayerPanel);
            Add(WorldPanel);

            Add(SettlementPanel);

            Add(TechnologyTreePanel);
            Add(UnitInfoPanel);
            Add(CharacterPanel);
            Add(MessageListPanel);
            Add(MessagePanel);

            Add(BattleForm);

            ExtraPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            ExtraPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.DefaultFontFamilyName, 8);

            Messages.MouseClickEvent += (state, mstate) => {
                MessageListPanel.Show(COTR.ActivePlayer);
            };

            Turn.MouseClickEvent += (state, mstate) => { 
                COTR.Turn(); 
                UpdateInterface();
            };

            PlayerStatisticPanel.MouseClickEvent += (state, mstate) => { 
                PlayerPanel.Show(COTR.ActivePlayer);
                PlayerPanel.Location = MapField.Location + (MapField.Size - SettlementPanel.Size).ToAPoint() / 2;
            };

            GameTimeStatistic.MouseClickEvent += (state, mstate) => { WorldPanel.Update(); WorldPanel.Enabled = true; WorldPanel.Location = MapField.Location + location; };

            //mapCellPanel.UpdateEvent += (state) => { PlayerStatisticPanel.Text = COTR.ActivePlayer.ToString(); };

            COTR.OnAttackResultEvent += (dUnit, aUnit, dPower, aPower, dResult, aResult) =>
            {
                BattleForm.Update(dUnit, aUnit, dPower, aPower, dResult, aResult);
                BattleForm.Location = MapField.Location + (MapField.Size - BattleForm.Size).ToAPoint() / 2;
            };

            ExtraSidePanel.SelectUnitEvent += (unit) =>
            {
                if (unit.Owner.Equals(COTR.ActivePlayer)) COTR.SelectedMapCell.SetActiveUnit(unit);
            };

            ExtraSidePanel.ExtraUnitSelectEvent += (unit) =>
            {
                UnitInfoPanel.Show(unit);
                UnitInfoPanel.Location = MapField.Location + (MapField.Size - UnitInfoPanel.Size).ToAPoint() / 2;
            };

            ExtraSidePanel.SettlementPanelActivate += () =>
            {
                if (COTR.IsMapCellSelected && COTR.SelectedMapCell.IsSettlement) SettlementPanel.Update(COTR.SelectedMapCell.Settlement);
            };

            SettlementPanel.UnitSelectEvent += (unitType, location) => {
                List<APeople> squad = COTR.GetMapCell(location).Population.Subtract(100);
                if (!COTR.AddUnit(unitType, squad, GameLocalization.UnitName[unitType])) COTR.GetMapCell(location).Population.Add(squad);
                else
                {
                    AMapCell mapCell = COTR.GetMapCell(location);
                    if (mapCell.IsSettlement) SettlementPanel.Update(mapCell.Settlement);
                    else if (SettlementPanel.Enabled) SettlementPanel.Update();
                    ExtraSidePanel.Update(mapCell);
                    PlayerStatisticPanel.Text = COTR.ActivePlayer.ToString();
                }
            };

            UnitInfoPanel.UpdateEvent += () => { if (ExtraSidePanel.Enabled) ExtraSidePanel.Update(COTR.SelectedMapCell); };

            TechnologyTreePanel.SelectEvent += (technology) => { if (SettlementPanel.Enabled) SettlementPanel.Update(); if (ExtraSidePanel.Enabled) ExtraSidePanel.Update(COTR.SelectedMapCell); };

            PlayerPanel.SettlementSelectEvent += (settlement) => SettlementPanel.Show(settlement);
            PlayerPanel.PlayerCallEvent += (player) => { CharacterPanel.Show(player.Ruler); };

            CharacterPanel.UpdateEvent += (player) => { if (PlayerPanel.Enabled) PlayerPanel.Update(); };

            MessageListPanel.SelectEvent += (message) => { MessagePanel.Show(message); };

            MessagePanel.DoneEvent += () => { if (MessageListPanel.Enabled) MessageListPanel.Update(); };
            MessagePanel.RenouncementEvent += () => { if (MessageListPanel.Enabled) MessageListPanel.Update(); };

            Turn.TimeEvent += () => {
                if (COTR.Status) COTR.Turn();
                UpdateInterface();
            };

        }

        private ASize CalculateMapViewSize(ASize MapSize)
        {
            return new ASize(26 + GraphicsExtension.DefaultMapCellRadius + Convert.ToInt32((MapSize.Width - 1) * GraphicsExtension.DefaultMapCellRadius * 1.55f + (MapSize.Height == 1 ? GraphicsExtension.DefaultMapCellRadius * 0.775f : 0)), GraphicsExtension.DefaultMapCellRadius + Convert.ToInt32((MapSize.Height - 1) * (GraphicsExtension.DefaultMapCellRadius / 2.2f)));
        }

        private APoint CalculateSpriteVector(APoint point)
        {
            return new APoint(13 + Convert.ToInt32(point.X * GraphicsExtension.DefaultMapCellRadius * 1.55f + (point.Y % 2 == 0 ? GraphicsExtension.DefaultMapCellRadius * 0.775f : 0)), Convert.ToInt32(point.Y * (GraphicsExtension.DefaultMapCellRadius / 2.2f)));
        }

        public void StartGame(List<string> players, List<ICharacter> characters, ASize mapSize)
        {

            COTR.Initialize(players, characters);

            COTR.StartGame(mapSize);

            PlayerStatisticPanel.Text = COTR.ActivePlayer.ToString();
            GameTimeStatistic.Text = "Ход: " + COTR.CurrentTurn;

            MapView = new AMapView(CalculateMapViewSize(mapSize)) { Parent = MapField, Location = new APoint(10, 10), DragAndDrop = true };

            //MapView.MouseEnterEvent += OnOverEvent;

            //for (int y = 0; y < mapSize.Height; y++)
            //for (int x = 0; x < (y % 2 == 0 ? mapSize.Width - 1 : COTR.Size.Width); x++)
            foreach (var e in COTR.Map)
            {
                AMapCell mapCell = e.Value;
                if (mapCell.ActiveUnit is null && COTR.GetUnits(mapCell.Location) is List<IUnit> units && units.Count > 0) mapCell.SetActiveUnit(units.First());
                //AMapCell mapCell = COTR.GetMapCell(new APoint(x, y));
                APrimitiveTexture source = TexturePack.Hex[mapCell.OwnerId];
                AMapCellView mapCellView = new AMapCellView(mapCell, COTR, source) { Parent = MapView, Location = CalculateSpriteVector(/*new APoint(x, y)*/e.Key) };

                mapCellView.MouseClickEvent += (state, mouseState) =>
                {
                    if (mouseState.MouseButton.Equals(AMouseButton.Left))
                    {
                        COTR.SelectMapCell(mapCell.Location);
                        ExtraSidePanel.Show(mapCell);
                        if (SettlementPanel.Enabled && COTR.SelectedMapCell.IsSettlement)
                        {
                            SettlementPanel.Show(mapCell.Settlement);
                        }
                        else SettlementPanel.Hide();
                    }
                    else if (mouseState.MouseButton.Equals(AMouseButton.Right))
                    {
                        if (COTR.SelectedMapCell.ActiveUnit is object && COTR.MoveUnit(mapCell.Location))
                        {
                            COTR.SelectMapCell(mapCell.Location);
                            if (ExtraSidePanel.Enabled) ExtraSidePanel.Update(COTR.SelectedMapCell);
                            if (PlayerPanel.Enabled) PlayerPanel.Update();
                            //COTR.ActivePlayer.ExploreTerritories(COTR.SelectedMapCell.NeighboringCells.Select(x => x.Location));
                            if (SettlementPanel.Enabled && COTR.SelectedMapCell.IsSettlement)
                            {
                                SettlementPanel.Show(mapCell.Settlement);
                            }
                            else SettlementPanel.Hide();
                            //UpdateMapCellInfo(mapCell.Location);
                            //UpdateUnitsButtonsInSelectedMaCell();
                            //UpdateActiveButtons();
                            //SelectActiveUnitButton(COTR.SelectedUnit);
                        }
                        //else UpdateUnitsButtonsInSelectedMaCell();
                    }
                    PlayerStatisticPanel.Text = COTR.ActivePlayer.ToString();
                };

                mapCellView.MouseEnterEvent += (state, mouseState) =>
                {
                    ExtraPanel.Text = "[" + mapCell.Location + "], " + GameLocalization.Biomes[mapCell.BiomeType] + ", " + (COTR.ActivePlayer.IsResource(mapCell.ResourceType) ? GameLocalization.Resources[mapCell.ResourceType] + (mapCell.IsResource ? " (" + (mapCell.IsMined ? "Добывается" : "Не добывается") + ")" : "") : "Ресурс отсутствует") + ", " + (mapCell.OwnerId == 0 ? "Пусто" : COTR.Players[mapCell.OwnerId].Name) + (mapCell.IsSettlement ? ", " + mapCell.Settlement.Name : "") + ", " + mapCell.Population.Total + " человек(а), " + mapCell.Culture + " ед.";
                };

                mapCellView.KeyDownEvent += (state, kstate) =>
                {
                    if (kstate.KeyboardKey.Equals(AKeyboardKey.E) && mapCell.IsSettlement && mapCell.Settlement.Owner.Equals(COTR.ActivePlayer))
                    {
                        SettlementPanel.Show(mapCell.Settlement);
                        SettlementPanel.Location = MapField.Location + location;
                    }
                    else SettlementPanel.Hide();
                    if (kstate.KeyState.Equals(AKeyState.Exit)) ExtraSidePanel.Hide();
                    if (kstate.KeyboardKey.Equals(AKeyboardKey.S)) TechnologyTreePanel.Show(mapCell, COTR.ActivePlayer);
                    if (kstate.KeyboardKey.Equals(AKeyboardKey.T)) Turn.IsCounting = !Turn.IsCounting;
                    //if (kstate.KeyboardKey.Equals(AKeyboardKey.Q) && mapCell.IsSettlement) CreateUnitsPanel.Show(mapCell.Settlement);
                };

                //mapCellView.MouseOverEvent += OnOverEvent;
            }

            PlayerPanel.Enabled = false;
            WorldPanel.Enabled = false;
            TechnologyTreePanel.Enabled = false;
            ExtraSidePanel.Enabled = false;
            UnitInfoPanel.Enabled = false;
            CharacterPanel.Enabled = false;
            MessageListPanel.Enabled = false;
            MessagePanel.Enabled = false;

            SettlementPanel.Enabled = false;

            BattleForm.Enabled = false;

            WorldPanel.Update();

            MapView.MouseEnterEvent += (state, mstate) => {
                if (mstate.MouseButton.Equals(AMouseButton.Middle))
                {
                    //mapCellPanel.HideAll(); 
                    //PlayerPanel.Enabled = false; 
                    //WorldPanel.Enabled = false;
                }
                ExtraPanel.Text = "Неизведанные территории";
            };

            //foreach (IUnit unit in COTR.StartGame()) SetUnitButton(unit);

            //UpdatePlayerStatistic();

        }

        private void UpdateInterface()
        {

            GameTimeStatistic.Text = "Ход: " + COTR.CurrentTurn;
            PlayerStatisticPanel.Text = COTR.ActivePlayer.ToString();
            SettlementPanel.Hide();
            if (ExtraSidePanel.Enabled && COTR.IsMapCellSelected && COTR.ActivePlayer.ExploredTerritories.Contains(COTR.SelectedMapCell.Location)) ExtraSidePanel.Update(COTR.SelectedMapCell);
            else ExtraSidePanel.Hide();
            PlayerPanel.Update(COTR.ActivePlayer);
        }

        public override void Update()
        {

            PlayerStatisticPanel.Size = new ASize(Parent.Width - 30 - GraphicsExtension.DefaultMiniButtonSize.Width - 210, GraphicsExtension.DefaultMiniButtonSize.Height);
            PlayerStatisticPanel.Location = new APoint(10, 10);

            GameTimeStatistic.Size = new ASize(200, GraphicsExtension.DefaultMiniButtonSize.Height);
            GameTimeStatistic.Location = new APoint(PlayerStatisticPanel.X + PlayerStatisticPanel.Width + 10, 10);

            Turn.Size = GraphicsExtension.DefaultMiniButtonSize;
            Turn.Location = new APoint(GameTimeStatistic.X + GameTimeStatistic.Width + 10, 10);

            MapField.Size = new ASize(Parent.Width - 20, Parent.Height - GraphicsExtension.DefaultMiniButtonSize.Height - 30);
            MapField.Location = new APoint(10, PlayerStatisticPanel.Y + PlayerStatisticPanel.Height + 10);

            ExtraPanel.Size = new ASize(Parent.Width - 20, 20);
            ExtraPanel.Location = MapField.Location + new APoint(0, MapField.Height - 20);

        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
