using System;
using System.Linq;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Player;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class PlayerPanel : APanel
    {

        private AGame Game;
        private AButton _CloseSettlementForm;

        private CharacterPanel _CharacterPanel;
        private AScrolleredPanel _RelationshipsPanel;
        private AScrolleredPanel _SettlementsPanel;

        public CharacterPanel CharacterPanel { get => _CharacterPanel; }
        public AButton CloseSettlementForm { get => _CloseSettlementForm; }

        public AScrolleredPanel RelationshipsPanel { get => _RelationshipsPanel; }
        public AScrolleredPanel SettlementsPanel { get => _SettlementsPanel; }

        public PlayerPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            int width = (Width - 30) / 2;

            _CharacterPanel = new CharacterPanel(Game, new ASize(width, Height - 80)) { Parent = this, Location = new APoint(10, 70) };
            _CloseSettlementForm = new AButton(new ASize(40, 40)) { Parent = this, Location = new APoint(Width - 50, 10), Text = "×" };

            int height = (_CharacterPanel.Height - 10) / 2;

            _RelationshipsPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(_CharacterPanel.Width, height)) { Parent = this, Location = _CharacterPanel.Location + new APoint(_CharacterPanel.Width + 10, 0) };
            _SettlementsPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, _RelationshipsPanel.Size) { Parent = this, Location = _RelationshipsPanel.Location + new APoint(0, _RelationshipsPanel.Height + 10) };

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            _RelationshipsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            _SettlementsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            _RelationshipsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            _SettlementsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            _RelationshipsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            _SettlementsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

            _CloseSettlementForm.MouseClickEvent += (state, mstate) => { Enabled = false; };

        }

        public void Update(IPlayer player)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            Text = player.Name;
            //_CharacterPanel.Update(Game.ActivePlayer.Ruler);
            _CharacterPanel.Update(player.Ruler);
            _RelationshipsPanel.Text = "Политический советник: \n\n" + string.Join("\n", player.Relationships.Select(x => x.Key.Name + ": " + GameLocalization.RelationshipName[x.Value]));
            _SettlementsPanel.Text = "Поселения игрока: \n\n" + string.Join("\n", player.Settlements.Select(x => "[" + x.Location + "] " + x.Name + " (" + x.Income + ")"));
        }

    }
}
