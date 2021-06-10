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
using GameLibrary.Message;
using GameLibrary.Map;

using ArtemisChroniclesOfTheReefGame.Forms;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class GamePanel : AEmptyPanel
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

        public delegate void OnMessage(IMessage message);

        public delegate void OnMarriage(ICharacter character, ICharacter spouse, bool isMatrilinearMarriage);

        // events

        public event OnAction TurnClickEvent;

        public event OnSelectUnit SelectUnitEvent;

        public event OnSelectTechnology SelectTechnologyEvent;

        public event OnCharacterClick MarryEvent;
        public event OnCharacterClick DivorceEvent;
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

        public event OnMessage DoneMessageEvent;
        public event OnMessage RenouncementMessageEvent;

        public event OnMarriage MarriageEvent;

        // graphics

        private StatPanel StatPanel;

        private APanel MapField;
        private AMapView MapView;
        private ExtraSidePanel ExtraSidePanel;

        private ATechnologyTreeForm TechnologyTreePanel;
        private APlayerForm PlayerInfoForm;
        private ASettlementForm SettlementForm;
        private ABattleForm BattleForm;
        private AMessageForm MessageForm;

        private AUnitForm UnitInfoForm;
        private AMarryForm MarryForm;
        private ACharacterForm CharacterInfoForm;
        private AMessageListPanelForm MessageListForm;
        private ACharactersForm CharactersList;

        private Dictionary<APoint, AMapCellView> Map;

        // data

        private GameData GameData;
        private string Name;
        private ASize MapSize;

        public GamePanel(ASize size) : base(size)
        {

            Map = new Dictionary<APoint, AMapCellView>();

        }

        public override void Initialize()
        {

            base.Initialize();

            ASize dBigFormSize = new ASize(Convert.ToInt32(Width * (2.5 / 3f)), Convert.ToInt32(Height * (2.5 / 3f)));
            ASize dSmallFormSize = new ASize(Convert.ToInt32(Height * (2.5 / 3f)), Convert.ToInt32(Height * (2.5 / 3f)));
            ASize dMiddleFormSize = new ASize(Convert.ToInt32(Width * (7 / 9f)), Convert.ToInt32(Height * (2.5 / 3f)));

            APoint dBigFormLocation = (Size - dBigFormSize).ToAPoint() / 2;
            APoint dSmallFormLocation = (Size - dSmallFormSize).ToAPoint() / 2;
            APoint dMiddleFormLocation = (Size - dMiddleFormSize).ToAPoint() / 2;

            StatPanel = new StatPanel(new ASize(Width - 20, GraphicsExtension.DefaultMiniButtonSize.Height + 20)) { Parent = this, Location = new APoint(10, 10) };

            MapField = new APanel(new ASize(Width - 20, Height - StatPanel.Height - 30)) { Parent = this, Location = StatPanel.Location + new APoint(0, StatPanel.Height + 10) };
            ExtraSidePanel = new ExtraSidePanel(new ASize(500, MapField.Height)) { Parent = this, Location = MapField.Location + new APoint(MapField.Width - 500, 0) };

            TechnologyTreePanel = new ATechnologyTreeForm(dBigFormSize) { Parent = this, Location = dBigFormLocation };
            PlayerInfoForm = new APlayerForm(dBigFormSize) { Parent = this, Location = dBigFormLocation };
            SettlementForm = new ASettlementForm(dBigFormSize) { Parent = this, Location = dSmallFormLocation };
            BattleForm = new ABattleForm(dBigFormSize) { Parent = this, Location = dSmallFormLocation };

            UnitInfoForm = new AUnitForm(dSmallFormSize) { Parent = this, Location = dSmallFormLocation };
            MarryForm = new AMarryForm(dSmallFormSize) { Parent = this, Location = dSmallFormLocation };
            CharactersList = new ACharactersForm(dSmallFormSize) { Parent = this, Location = dSmallFormLocation };
            CharacterInfoForm = new ACharacterForm(dSmallFormSize) { Parent = this, Location = dSmallFormLocation };
            MessageListForm = new AMessageListPanelForm(dSmallFormSize) { Parent = this, Location = dSmallFormLocation };
            MessageForm = new AMessageForm(dMiddleFormSize) { Parent = this, Location = dMiddleFormLocation };

            MarryForm.SetCharactersForm(CharactersList);

            StatPanel.PlayerClickEvent += () =>
            {
                if (PlayerInfoForm.Enabled) PlayerInfoForm.Update(GameData, /*GameData.GetPlayer(Name)*/GameData.ActivePlayer);
                else PlayerInfoForm.Show(GameData, /*GameData.GetPlayer(Name)*/GameData.ActivePlayer);
            };

            StatPanel.MessageClickEvent += () =>
            {
                if (MessageListForm.Enabled) MessageListForm.Update(/*GameData.GetPlayer(Name)*/GameData.ActivePlayer);
                else MessageListForm.Show(/*GameData.GetPlayer(Name)*/GameData.ActivePlayer);
            };

            StatPanel.TurnClickEvent += () => TurnClickEvent?.Invoke();

            StatPanel.CharactersClickEvent += () => CharactersList.Show(GameData.Characters.Values.SelectMany(x => x), GameData);

            ExtraSidePanel.SettlementPanelActivate += () =>
            {
                if (GameData.IsMapCellSelected && GameData.SelectedMapCell.IsSettlement) SettlementForm.Show(GameData.SelectedMapCell.Settlement);
            };
            ExtraSidePanel.CloseEvent += () => SettlementForm.Hide();
            ExtraSidePanel.SelectUnitEvent += (unit) => SelectUnitEvent?.Invoke(unit);
            ExtraSidePanel.ExtraUnitSelectEvent += (unit) => UnitInfoForm.Show(GameData, unit);

            TechnologyTreePanel.SelectEvent += (technology) => SelectTechnologyEvent?.Invoke(technology);

            PlayerInfoForm.MarryEvent += (character) =>
            {
                MarryForm.Show(character, GameData);
                //MarryEvent?.Invoke(character);
            };
            PlayerInfoForm.DivorceEvent += (character) => DivorceEvent?.Invoke(character);
            PlayerInfoForm.AgreementEvent += (character) => AgreementEvent?.Invoke(character);
            PlayerInfoForm.HeirEvent += (character) => HeirEvent?.Invoke(character);
            PlayerInfoForm.WarEvent += (character) => WarEvent?.Invoke(character);
            PlayerInfoForm.PeaceEvent += (character) => PeaceEvent?.Invoke(character);
            PlayerInfoForm.UnionEvent += (character) => UnionEvent?.Invoke(character);
            PlayerInfoForm.BreakUnionEvent += (character) => BreakUnionEvent?.Invoke(character);
            PlayerInfoForm.SelectRelativeEvent += (character) =>
            {
                CharacterInfoForm.Show(GameData, character);
            };

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

            CharacterInfoForm.MarryEvent += (character) =>
            {
                MarryForm.Show(character, GameData);
                //MarryEvent?.Invoke(character);
            };
            CharacterInfoForm.AgreementEvent += (character) => AgreementEvent?.Invoke(character);
            CharacterInfoForm.HeirEvent += (character) => HeirEvent?.Invoke(character);
            CharacterInfoForm.WarEvent += (character) => WarEvent?.Invoke(character);
            CharacterInfoForm.PeaceEvent += (character) => PeaceEvent?.Invoke(character);
            CharacterInfoForm.UnionEvent += (character) => UnionEvent?.Invoke(character);
            CharacterInfoForm.BreakUnionEvent += (character) => BreakUnionEvent?.Invoke(character);
            CharacterInfoForm.SelectRelativeEvent += (character) =>
            {
                CharacterInfoForm.Show(GameData, character);
            };

            MessageListForm.SelectEvent += (message) => MessageForm.Show(message);

            MessageForm.DoneEvent += (message) => DoneMessageEvent?.Invoke(message);
            MessageForm.RenouncementEvent += (message) => RenouncementMessageEvent?.Invoke(message);

            CharactersList.SelectEvent += (character) =>
            {
                if (!MarryForm.CharactersFormIsActive) CharacterInfoForm.Show(GameData, character);
            };

            MarryForm.DoneEvent += (character, spouse, isMatrilinearMarriage) => MarriageEvent?.Invoke(character, spouse, isMatrilinearMarriage);

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

            if (TechnologyTreePanel.Enabled) TechnologyTreePanel.Hide();
            if (PlayerInfoForm.Enabled) PlayerInfoForm.Hide();
            if (SettlementForm.Enabled) SettlementForm.Hide();
            if (BattleForm.Enabled) BattleForm.Hide();

            if (UnitInfoForm.Enabled) UnitInfoForm.Hide();
            if (CharacterInfoForm.Enabled) CharacterInfoForm.Hide();
            if (MessageListForm.Enabled) MessageListForm.Hide();
            if (MessageForm.Enabled) MessageForm.Hide();
            if (CharactersList.Enabled) CharactersList.Hide();
            if (MarryForm.Enabled) MarryForm.Hide();

        }

        private void UpdatePanels()
        {

            IPlayer player = GameData.GetPlayer(Name);

            StatPanel.Update(player, GameData.CurrentTurn, GameData.ActivePlayer.Name);

            if (ExtraSidePanel.Enabled && GameData.IsMapCellSelected) ExtraSidePanel.Update(GameData, GameData.SelectedMapCell);
            else ExtraSidePanel.Hide();

            if (TechnologyTreePanel.Enabled) TechnologyTreePanel.Update(player);
            else TechnologyTreePanel.Hide();
            if (PlayerInfoForm.Enabled) PlayerInfoForm.Update();
            else PlayerInfoForm.Hide();
            if (SettlementForm.Enabled && GameData.IsMapCellSelected && GameData.SelectedMapCell.IsSettlement && GameData.SelectedMapCell.Settlement.Owner.Equals(player)) SettlementForm.Update(GameData.SelectedMapCell.Settlement);
            else SettlementForm.Hide();
            if (BattleForm.Enabled) BattleForm.Hide();

            if (UnitInfoForm.Enabled) UnitInfoForm.Hide();
            if (CharacterInfoForm.Enabled) CharacterInfoForm.Hide();
            if (MessageListForm.Enabled) MessageListForm.Update(player);
            if (MessageForm.Enabled) MessageForm.Hide();
            CharactersList.Update(GameData.Characters.Values.SelectMany(x => x), GameData);
            if (MarryForm.Enabled) MarryForm.Hide();

        }

        public void Update(AGame game, string name)
        {

            if (GameData is null) GameData = new GameData(game);
            else GameData.Update(game);

            if (Name is null || !name.Equals(Name)) Name = name;

            if (MapView is null || !game.GameMap.Size.Equals(MapSize))
            {
                MapSize = game.GameMap.Size;
                MapView = new AMapView(CalculateMapViewSize(MapSize)) { Parent = MapField, Location = new APoint(10, 10), DragAndDrop = true };
            }

            UpdatePanels();

            foreach (var e in game.GameMap.Map)
            {

                APoint location = e.Key;
                AMapCell mapCell = e.Value;

                if (Map.ContainsKey(location))
                {
                    Map[location].SetSourceMapCell(mapCell);
                }
                else
                {

                    APrimitiveTexture source = TexturePack.Hex[mapCell.OwnerId];
                    AMapCellView mapCellView = new AMapCellView(mapCell, GameData, source) { Parent = MapView, Location = CalculateSpriteVector(e.Key) };

                    mapCellView.MouseClickEvent += (state, mouseState) =>
                    {
                        if (mouseState.MouseButton.Equals(AMouseButton.Left))
                        {
                            ExtraSidePanel.Show(GameData, mapCellView.MapCell);
                            MapCellSelectEvent?.Invoke(location);
                        }
                        else if (mouseState.MouseButton.Equals(AMouseButton.Right))
                        {
                            if (GameData.SelectedMapCell.ActiveUnit is object) MoveUnitEvent?.Invoke(location);
                        }
                    };

                    mapCellView.KeyDownEvent += (state, kstate) =>
                    {
                        if (kstate.KeyboardKey.Equals(AKeyboardKey.E) && mapCell.IsSettlement && mapCell.Settlement.Owner.Name.Equals(GameData.ActivePlayer.Name))
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

        public void Update(GameData gameData, string name)
        {

            if (GameData is null) GameData = gameData;
            else GameData.Update(gameData);

            if (Name is null || !name.Equals(Name)) Name = name;

            if (MapView is null || !gameData.GameMap.Size.Equals(MapSize))
            {
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
                    Map[location].SetSourceMapCell(mapCell);
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
