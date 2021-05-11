using System;
using System.Collections.Generic;
using System.Linq;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Character;
using GameLibrary.Player;
using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class MapCellPanel: APanel
    {

        public delegate void OnUpdate(AMapCell mapCell);

        private AGame Game;

        private AButton _CloseSettlementForm;
        private AButton _ShowSettlementPanel;
        private AButton _ShowUnitsPanel;
        private AButton _ShowCharactersPanel;

        private SettlementPanel _SettlementPanel;
        private UnitsPanel _UnitsPanel;
        private CharacterPanel _CharacterPanel;
        private CharactersListPanel _CharactersListPanel;

        private OnUpdate OnUpdateHandler;

        public AButton CloseSettlementForm { get => _CloseSettlementForm; }
        public AButton ShowSettlementPanel { get => _ShowSettlementPanel; }
        public AButton ShowUnitsPanel { get => _ShowUnitsPanel; }
        public AButton ShowCharactersPanel { get => _ShowCharactersPanel; }

        public SettlementPanel SettlementPanel { get => _SettlementPanel; }
        public UnitsPanel UnitsPanel { get => _UnitsPanel; }
        public CharacterPanel CharactersPanel { get => _CharacterPanel; }
        public CharactersListPanel CharactersListPanel { get => _CharactersListPanel; }

        public MapCellPanel(AGame game, ASize size) : base(size)
        {
            Game = game;

            OnUpdateHandler = new OnUpdate((mapCell) => { if (mapCell.IsSettlement) Text = mapCell.Settlement.Name + " (" + mapCell.Settlement.Owner.Name + "), " + mapCell.Population.Total + " человек(а)"; else Text = mapCell.Location.ToString() + " (" + (mapCell.Owner is null? "владелец отсутствует": mapCell.Owner.Name) + "), " + mapCell.Population.Total + " человек(а)"; });

        }

        public override void Initialize()
        {
            base.Initialize();

            int width = (Width - 70) / 3;

            _CloseSettlementForm = new AButton(new ASize(40, 40)) { Parent = this, Location = new APoint(Width - 50, 10), Text = "×" };

            _ShowSettlementPanel = new AButton(new ASize(width + 10, 50)) { Parent = this, Location = new APoint(10, 59), Text = "Поселение" };
            _ShowUnitsPanel = new AButton(new ASize(width + 10, 50)) { Parent = this, Location = _ShowSettlementPanel.Location + new APoint(_ShowSettlementPanel.Width + 10, 0), Text = "Юниты" };
            _ShowCharactersPanel = new AButton(new ASize(width + 10, 50)) { Parent = this, Location = _ShowUnitsPanel.Location + new APoint(_ShowUnitsPanel.Width + 10, 0), Text = "Персонажи" };

            _SettlementPanel = new SettlementPanel(Game, new ASize(Width - 20, Height - _ShowSettlementPanel.Height - 79)) { Parent = this, Location = _ShowSettlementPanel.Location + new APoint(0, _ShowSettlementPanel.Height + 10) };
            _UnitsPanel = new UnitsPanel(Game, new ASize(Width - 20, Height - _ShowSettlementPanel.Height - 79)) { Parent = this, Location = _ShowSettlementPanel.Location + new APoint(0, _ShowSettlementPanel.Height + 10) };
            _CharacterPanel = new CharacterPanel(Game, new ASize((Width - 30) / 2, Height - _ShowSettlementPanel.Height - 79)) { Parent = this, Location = _ShowSettlementPanel.Location + new APoint(0, _ShowSettlementPanel.Height + 10) };
            _CharactersListPanel = new CharactersListPanel(Game, new ASize((Width - 30) / 2, Height - _ShowSettlementPanel.Height - 79)) { Parent = this, Location = _CharacterPanel.Location + new APoint(_CharacterPanel.Width + 10, 0) };

            _CharactersListPanel.SelectCharacterEvent += (character) => { _CharacterPanel.Update(character); _CharacterPanel.Enabled = true; };

            _SettlementPanel.UpdateEvent += OnUpdateHandler;
            _UnitsPanel.UpdateEvent += OnUpdateHandler;

            _CloseSettlementForm.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.DefaultFontFamilyName, 18);

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;

            _CloseSettlementForm.MouseClickEvent += (state, mstate) => { 
                HideAll();
                _ShowSettlementPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
                _ShowUnitsPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
                _ShowCharactersPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
            };

            _ShowSettlementPanel.MouseClickEvent += (state, mstate) =>
            {
                _SettlementPanel.Enabled = true;
                _UnitsPanel.Enabled = false;
                _CharacterPanel.Enabled = false;
                _CharactersListPanel.Enabled = false;
                _ShowSettlementPanel.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                _ShowUnitsPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
                _ShowCharactersPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
            };

            _ShowUnitsPanel.MouseClickEvent += (state, mstate) =>
            {
                _UnitsPanel.Enabled = true;
                _SettlementPanel.Enabled = false;
                _CharacterPanel.Enabled = false;
                _CharactersListPanel.Enabled = false;
                _ShowUnitsPanel.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                _ShowSettlementPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
                _ShowCharactersPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
            };

            _ShowCharactersPanel.MouseClickEvent += (state, mstate) =>
            {
                _CharactersListPanel.Enabled = true;
                _CharacterPanel.Enabled = false;
                _UnitsPanel.Enabled = false;
                _SettlementPanel.Enabled = false;
                _ShowCharactersPanel.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                _ShowUnitsPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
                _ShowSettlementPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
            };

            _SettlementPanel.Enabled = true;
            _UnitsPanel.Enabled = false;
            _CharacterPanel.Enabled = false;
            _CharactersListPanel.Enabled = false;

        }

        public void Update(APoint location)
        {
            AMapCell mapCell = Game.GetMapCell(location);

            _ShowCharactersPanel.Enabled = false;
            _ShowUnitsPanel.Enabled = false;
            _ShowSettlementPanel.Enabled = false;

            if (Game.GetCharacters(mapCell.Location) is List<ICharacter> characters && characters.Count > 0)
            {
                _CharactersListPanel.Update(characters);

                Enabled = true;

                _SettlementPanel.Enabled = false;
                _UnitsPanel.Enabled = false;
                _CharacterPanel.Enabled = true;
                _CharactersListPanel.Enabled = true;

                _ShowCharactersPanel.Enabled = true;

                ShowCharactersPanel.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                ShowUnitsPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
                ShowCharactersPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
            }
            if (Game.GetUnits(location) is List<IUnit> units && units.Count > 0 || mapCell.IsSettlement)
            {
                UnitsPanel.Update(location);

                Enabled = true;

                _SettlementPanel.Enabled = false;
                _UnitsPanel.Enabled = true;
                _CharacterPanel.Enabled = false;
                _CharactersListPanel.Enabled = false;

                UnitsPanel.ShowCreatePanel(mapCell.IsSettlement);

                ShowUnitsPanel.Enabled = true;

                ShowUnitsPanel.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                ShowSettlementPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
                ShowCharactersPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
            }
            if (mapCell.IsSettlement)
            {
                SettlementPanel.Update(location);

                Enabled = true;

                _SettlementPanel.Enabled = true;
                _UnitsPanel.Enabled = false;
                _CharacterPanel.Enabled = false;
                _CharactersListPanel.Enabled = false;

                ShowSettlementPanel.Enabled = true;

                Text = mapCell.Settlement.Name + " (" + mapCell.Settlement.Owner.Name + "), " + mapCell.Population.Total + " человек(а)";

                ShowSettlementPanel.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                ShowUnitsPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
                ShowCharactersPanel.BorderColor = GraphicsExtension.DefaultBorderColor;
            }
        }

        public void Show() => Enabled = true;

        public void HideAll()
        {
            Enabled = false;
            _SettlementPanel.Enabled = false;
            _UnitsPanel.Enabled = false;
            _CharacterPanel.Enabled = false;
            _CharactersListPanel.Enabled = false;
        }

    }
}
