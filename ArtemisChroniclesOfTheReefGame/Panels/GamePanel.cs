using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Unit.Main;
using GameLibrary.Technology;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Building;
using GameLibrary.Player;
using GameLibrary.Character;
using GameLibrary.Map;

using ArtemisChroniclesOfTheReefGame.Forms;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class GamePanel: AEmptyPanel
    {

        // delegates

        public delegate void OnAction();
        public delegate void OnClick();
        public delegate void OnCharacterClick(ICharacter character);
        public delegate void OnUnitClick(IUnit unit);

        public delegate void OnSelectUnit(IUnit unit);
        public delegate void OnSelectTechnology(ITechnology technology);
        public delegate void OnSettlementSelect(ISettlement settlement);
        public delegate void OnPlayerSelect(IPlayer player);

        public delegate void OnSelectBuilding(IBuilding building);
        public delegate void OnSelectInvestigatedTechnology(ISettlement settlement);
        public delegate void OnSelectUnitType(AUnitType unitType);
        
        public delegate void OnMapCellSelect(APoint point);

        // events

        public event OnAction TurnClickEvent;

        public event OnSelectUnit SelectUnitEvent;

        public event OnSelectTechnology SelectTechnologyEvent;

        public event OnCharacterClick MarryEvent;
        public event OnCharacterClick AgreementEvent;
        public event OnCharacterClick HeirEvent;
        public event OnCharacterClick WarEvent;
        public event OnCharacterClick PeaceEvent;
        public event OnCharacterClick UnionEvent;
        public event OnCharacterClick BreakUnionEvent;

        public event OnSelectBuilding BuildingCreateEvent;
        public event OnSelectUnitType UnitCreateEvent;

        public event OnUnitClick RenameEvent;
        public event OnUnitClick DestroyEvent;
        public event OnUnitClick GeneralEvent;
        public event OnUnitClick WorkEvent;
        public event OnUnitClick EstablishEvent;
        
        public event OnMapCellSelect MapCellSelectEvent;
        public event OnMapCellSelect MoveUnitEvent;

        // graphics

        private StatPanel StatPanel;

        private APanel MapField;
        private AMapView MapView;
        private ExtraSidePanel ExtraSidePanel;

        private ATechnologyTreeForm TechnologyTreePanel;
        private APlayerForm PlayerInfoForm;
        private ASettlementForm SettlementForm;
        private ABattleForm BattleForm;

        private AUnitForm UnitInfoForm;
        private ACharacterForm CharacterInfoForm;
        private AMessageListPanelForm MessageListForm;
        private AMessageForm MessageForm;

        private Dictionary<APoint, AMapCellView> Map;

        // data

        private GameData GameData;
        private string Name;
        private ASize MapSize;

        public GamePanel(ASize size): base(size)
        {

            Map = new Dictionary<APoint, AMapCellView>();

        }

        public override void Initialize()
        {

            base.Initialize();

            ASize dBigFormSize = new ASize(Convert.ToInt32(Width * (2 / 3f)), Convert.ToInt32(Height * (2 / 3f)));
            ASize dMiddleFormSize = new ASize(600, 600);

            APoint dBigFormLocation = (Size - dBigFormSize).ToAPoint() / 2;
            APoint dMiddleFormLocation = (Size - dMiddleFormSize).ToAPoint() / 2;

            StatPanel = new StatPanel(new ASize(Width - 20, GraphicsExtension.DefaultMiniButtonSize.Height + 20)) { Parent = this, Location = new APoint(10, 10) };

            MapField = new APanel(new ASize(Width - 20, Height - StatPanel.Height - 30)) { Parent = this, Location = StatPanel.Location + new APoint(0, StatPanel.Height + 10) };
            ExtraSidePanel = new ExtraSidePanel(new ASize(500, MapField.Height)) { Parent = this, Location = MapField.Location + new APoint(MapField.Width - 500, 0) };

            TechnologyTreePanel = new ATechnologyTreeForm(dBigFormSize) { Parent = this, Location = dBigFormLocation };
            PlayerInfoForm = new APlayerForm(dBigFormSize) { Parent = this, Location = dBigFormLocation };
            SettlementForm = new ASettlementForm(dBigFormSize) { Parent = this, Location = dMiddleFormLocation };
            BattleForm = new ABattleForm(dBigFormSize) { Parent = this, Location = dMiddleFormLocation };

            UnitInfoForm = new AUnitForm(dMiddleFormSize) { Parent = this, Location = dMiddleFormLocation };
            CharacterInfoForm = new ACharacterForm(dMiddleFormSize) { Parent = this, Location = dMiddleFormLocation };
            MessageListForm = new AMessageListPanelForm(dMiddleFormSize) { Parent = this, Location = dMiddleFormLocation };
            MessageForm = new AMessageForm(dMiddleFormSize) { Parent = this, Location = dMiddleFormLocation };

            StatPanel.PlayerClickEvent += () => PlayerInfoForm.Update(GameData, GameData.GetPlayer(Name));
            StatPanel.MessageClickEvent += () => MessageListForm.Update(GameData.GetPlayer(Name));
            StatPanel.TurnClickEvent += () => TurnClickEvent?.Invoke();

            ExtraSidePanel.SettlementPanelActivate += () =>
            {
                if (GameData.IsMapCellSelected && GameData.SelectedMapCell.IsSettlement) SettlementForm.Show(GameData.SelectedMapCell.Settlement);
            };
            ExtraSidePanel.CloseEvent += () => SettlementForm.Hide();
            ExtraSidePanel.SelectUnitEvent += (unit) => SelectUnitEvent?.Invoke(unit);
            ExtraSidePanel.ExtraUnitSelectEvent += (unit) => UnitInfoForm.Show(GameData, unit);

            TechnologyTreePanel.SelectEvent += (technology) => SelectTechnologyEvent?.Invoke(technology);

            PlayerInfoForm.MarryEvent += (character) => MarryEvent?.Invoke(character);
            PlayerInfoForm.AgreementEvent += (character) => AgreementEvent?.Invoke(character);
            PlayerInfoForm.HeirEvent += (character) => HeirEvent?.Invoke(character);
            PlayerInfoForm.WarEvent += (character) => WarEvent?.Invoke(character);
            PlayerInfoForm.PeaceEvent += (character) => PeaceEvent?.Invoke(character);
            PlayerInfoForm.UnionEvent += (character) => UnionEvent?.Invoke(character);
            PlayerInfoForm.BreakUnionEvent += (character) => BreakUnionEvent?.Invoke(character);

            PlayerInfoForm.SettlementSelectEvent += (settlement) => SettlementForm.Show(settlement);
            PlayerInfoForm.PlayerSelectEvent += (player) => CharacterInfoForm.Show(GameData, player.Ruler);
            
            SettlementForm.BuildingCreateEvent += (building) => BuildingCreateEvent?.Invoke(building);
            SettlementForm.TechnologySelectEvent += (settlement) => TechnologyTreePanel.Update(settlement.Owner);
            SettlementForm.UnitCreateEvent += (unit) => UnitCreateEvent?.Invoke(unit);

            UnitInfoForm.RenameEvent += (unit) => RenameEvent?.Invoke(unit);
            UnitInfoForm.DestroyEvent += (unit) => DestroyEvent?.Invoke(unit);
            UnitInfoForm.GeneralEvent += (unit) => GeneralEvent?.Invoke(unit);
            UnitInfoForm.WorkEvent += (unit) => WorkEvent?.Invoke(unit);
            UnitInfoForm.EstablishEvent += (unit) => EstablishEvent?.Invoke(unit);

            CharacterInfoForm.MarryEvent += (character) => MarryEvent?.Invoke(character);
            CharacterInfoForm.AgreementEvent += (character) => AgreementEvent?.Invoke(character);
            CharacterInfoForm.HeirEvent += (character) => HeirEvent?.Invoke(character);
            CharacterInfoForm.WarEvent += (character) => WarEvent?.Invoke(character);
            CharacterInfoForm.PeaceEvent += (character) => PeaceEvent?.Invoke(character);
            CharacterInfoForm.UnionEvent += (character) => UnionEvent?.Invoke(character);
            CharacterInfoForm.BreakUnionEvent += (character) => BreakUnionEvent?.Invoke(character);

            MessageListForm.SelectEvent += (message) => MessageForm.Show(message);

            HideAll();

        }

        public void UpdateGame(ASize mapSize)
        {

            HideAll();

        }

        private ASize CalculateMapViewSize(ASize MapSize)
        {
            return new ASize(26 + GraphicsExtension.DefaultMapCellRadius + Convert.ToInt32((MapSize.Width - 1) * GraphicsExtension.DefaultMapCellRadius * 1.55f + (MapSize.Height == 1 ? GraphicsExtension.DefaultMapCellRadius * 0.775f : 0)), GraphicsExtension.DefaultMapCellRadius + Convert.ToInt32((MapSize.Height - 1) * (GraphicsExtension.DefaultMapCellRadius / 2.2f)));
        }

        private APoint CalculateSpriteVector(APoint point)
        {
            return new APoint(13 + Convert.ToInt32(point.X * GraphicsExtension.DefaultMapCellRadius * 1.55f + (point.Y % 2 == 0 ? GraphicsExtension.DefaultMapCellRadius * 0.775f : 0)), Convert.ToInt32(point.Y * (GraphicsExtension.DefaultMapCellRadius / 2.2f)));
        }

        public void HideAll()
        {

            if (ExtraSidePanel.Enabled) TechnologyTreePanel.Hide();
            if (ExtraSidePanel.Enabled) PlayerInfoForm.Hide();
            if (ExtraSidePanel.Enabled) SettlementForm.Hide();
            if (ExtraSidePanel.Enabled) BattleForm.Hide();

            if (ExtraSidePanel.Enabled) UnitInfoForm.Hide();
            if (ExtraSidePanel.Enabled) CharacterInfoForm.Hide();
            if (ExtraSidePanel.Enabled) MessageListForm.Hide();
            if (ExtraSidePanel.Enabled) MessageForm.Hide();

        }

        private void UpdatePanels()
        {

            IPlayer player = GameData.GetPlayer(Name);

            StatPanel.Update(player, GameData.CurrentTurn, GameData.ActivePlayer.Name);

            if (ExtraSidePanel.Enabled && GameData.IsMapCellSelected) ExtraSidePanel.Update(GameData, GameData.SelectedMapCell);
            else ExtraSidePanel.Hide();

            if (ExtraSidePanel.Enabled) TechnologyTreePanel.Update(player); 
            else TechnologyTreePanel.Hide();
            if (ExtraSidePanel.Enabled) PlayerInfoForm.Update();
            else PlayerInfoForm.Hide();
            if (ExtraSidePanel.Enabled && GameData.IsMapCellSelected && GameData.SelectedMapCell.IsSettlement && GameData.SelectedMapCell.Settlement.Owner.Equals(player)) SettlementForm.Update(GameData.SelectedMapCell.Settlement);
            else SettlementForm.Hide();
            if (ExtraSidePanel.Enabled) BattleForm.Hide();

            if (ExtraSidePanel.Enabled) UnitInfoForm.Hide();
            if (ExtraSidePanel.Enabled) CharacterInfoForm.Hide();
            if (ExtraSidePanel.Enabled) MessageListForm.Update(player);
            if (ExtraSidePanel.Enabled) MessageForm.Hide();

        }

        public void Update(GameData gameData, string name)
        {

            GameData = gameData;

            if (Name is null || !name.Equals(Name)) Name = name;

            if (MapView is null || !gameData.GameMap.Size.Equals(MapSize)) {
                MapSize = gameData.GameMap.Size;
                MapView = new AMapView(CalculateMapViewSize(MapSize)) { Parent = MapField, Location = new APoint(10, 10), DragAndDrop = true };
            }

            UpdatePanels();

            foreach (var e in gameData.GameMap.Map)
            {

                APoint location = e.Key;
                AMapCell mapCell = e.Value;

                if (Map.ContainsKey(location))
                {

                    AMapCellView mapCellView = Map[location];
                    mapCellView.SetSourceMapCell(mapCell);

                }
                else
                {

                    APrimitiveTexture source = TexturePack.Hex[mapCell.OwnerId];
                    AMapCellView mapCellView = new AMapCellView(mapCell, GameData, source) { Parent = MapView, Location = CalculateSpriteVector(e.Key) };

                    mapCellView.MouseClickEvent += (state, mouseState) =>
                    {
                        if (mouseState.MouseButton.Equals(AMouseButton.Left))
                        {
                            MapCellSelectEvent?.Invoke(location);
                            ExtraSidePanel.Show(GameData, mapCellView.MapCell);
                        }
                        else if (mouseState.MouseButton.Equals(AMouseButton.Right))
                        {
                            if (GameData.SelectedMapCell.ActiveUnit is object) MoveUnitEvent?.Invoke(location);
                        }
                    };

                    mapCellView.KeyDownEvent += (state, kstate) =>
                    {
                        if (kstate.KeyboardKey.Equals(AKeyboardKey.E) && mapCell.IsSettlement && mapCell.Settlement.Owner.Equals(GameData.ActivePlayer))
                        {
                            SettlementForm.Show(mapCell.Settlement);
                            SettlementForm.Location = MapField.Location + location;
                        }
                        else SettlementForm.Hide();
                        if (kstate.KeyState.Equals(AKeyState.Exit)) ExtraSidePanel.Hide();
                    };

                    Map.Add(location, mapCellView);

                }

            }

        }

    }
}
