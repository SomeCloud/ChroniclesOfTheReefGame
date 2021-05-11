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
using GameLibrary.Character;
using GameLibrary.Map;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class WorldPanel: APanel
    {

        private AGame Game;

        private AButton _CloseSettlementForm;
        private CharacterPanel _CharacterPanel;
        private CharactersListPanel _CharactersListPanel;

        public CharacterPanel CharacterPanel { get => _CharacterPanel; }
        public CharactersListPanel CharactersListPanel { get => _CharactersListPanel; }
        public AButton CloseSettlementForm { get => _CloseSettlementForm; }

        public WorldPanel(AGame game, ASize size): base(size)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            int width = (Width - 30) / 2;

            _CloseSettlementForm = new AButton(new ASize(40, 40)) { Parent = this, Location = new APoint(Width - 50, 10), Text = "×" };

            _CharacterPanel = new CharacterPanel(Game, new ASize(width, Height - 80)) { Parent = this, Location = new APoint(10, 70) };
            _CharactersListPanel = new CharactersListPanel(Game, new ASize(width, Height - 80)) { Parent = this, Location = _CharacterPanel.Location + new APoint(_CharacterPanel.Width + 10, 0) };

            _CloseSettlementForm.MouseClickEvent += (state, mstate) => { Enabled = false; };
            _CharactersListPanel.SelectCharacterEvent += (character) => { _CharacterPanel.Update(character); _CharacterPanel.Enabled = true; };

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;

            Text = "Советник по обществу";

        }

        public void Update()
        {

            _CharacterPanel.Enabled = false;
            _CharactersListPanel.Update(Game.Characters.SelectMany(x => x.Value).ToList());

        }

    }
}
