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
using GameLibrary.Player;
using GameLibrary.Settlement;
using GameLibrary.Character;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{
    class APlayerForm: AForm
    {

        public delegate void OnSettlementSelect(ISettlement settlement);
        public delegate void OnPlayerSelect(IPlayer player);

        public delegate void OnClick(ICharacter character);

        public event OnSettlementSelect SettlementSelectEvent;
        public event OnPlayerSelect PlayerSelectEvent;

        public event OnClick MarryEvent;
        public event OnClick AgreementEvent;
        public event OnClick HeirEvent;
        public event OnClick WarEvent;
        public event OnClick PeaceEvent;
        public event OnClick UnionEvent;
        public event OnClick BreakUnionEvent;

        private ACharacterPanel CharacterPanel;
        private SettlementsListPanel SettlementsList;
        private RelationshipsListPanel RelationshipsList;

        private GameData GameData;
        IPlayer Player;

        public APlayerForm(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = (Content.Width - 12) / 2;
            int dHeight = (Content.Height - 10) * 5 / 8;

            CharacterPanel = new ACharacterPanel(new ASize(dWidth, dHeight)) { Location = new APoint(1, 1) };
            RelationshipsList = new RelationshipsListPanel(new ASize(dWidth, Content.Height - dHeight - 12)) { Location = CharacterPanel.Location + new APoint(0, CharacterPanel.Height + 10) };
            SettlementsList = new SettlementsListPanel(new ASize(dWidth, Content.Height - 2)) { Location = CharacterPanel.Location + new APoint(CharacterPanel.Width + 10, 0) };

            CharacterPanel.MarryEvent += (character) => MarryEvent?.Invoke(character);
            CharacterPanel.AgreementEvent += (character) => AgreementEvent?.Invoke(character);
            CharacterPanel.HeirEvent += (character) => HeirEvent?.Invoke(character);
            CharacterPanel.WarEvent += (character) => WarEvent?.Invoke(character);
            CharacterPanel.PeaceEvent += (character) => PeaceEvent?.Invoke(character);
            CharacterPanel.UnionEvent += (character) => UnionEvent?.Invoke(character);
            CharacterPanel.BreakUnionEvent += (character) => BreakUnionEvent?.Invoke(character);

            Add(CharacterPanel);
            Add(RelationshipsList);
            Add(SettlementsList);

            SettlementsList.SelectEvent += (settlement) => SettlementSelectEvent?.Invoke(settlement);
            RelationshipsList.SelectEvent += (player) => PlayerSelectEvent?.Invoke(player);

        }

        public void Update()
        {
            if (Player is object) Update(GameData, Player);
        }

        public void Update(GameData gameData, IPlayer player)
        {

            Player = player;
            GameData = gameData;

            CharacterPanel.Update(gameData, player.Ruler);
            RelationshipsList.Update(player.Relationships.Keys, player);
            SettlementsList.Update(player.Settlements);

        }

        public void Hide() => Enabled = false;

        public void Show(GameData gameData, IPlayer player)
        {

            Enabled = true;
            Update(gameData, player);

            Text = "Первый советник игрока " + player.Name;

        }

    }
}
