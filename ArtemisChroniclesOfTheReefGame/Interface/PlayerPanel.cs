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
using GameLibrary.Settlement;
using GameLibrary.Player;
using GameLibrary.Extension;
using GameLibrary.Technology;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class PlayerPanel: AForm
    {

        public delegate void OnSettlementSelect(ISettlement settlement);
        public delegate void OnPlayerCall(IPlayer player);
        public delegate void OnShow();

        public event OnSettlementSelect SettlementSelectEvent;
        public event OnPlayerCall PlayerCallEvent;
        public event OnShow ShowEvent;

        private CharacterMiniPanel CharacterPanel;
        private SettlementsListPanel SettlementsList;
        private RelationshipsListPanel RelationshipsList;

        private AGame Game;
        IPlayer Player;

        public PlayerPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
        }

        public PlayerPanel(AGame game, ASize size, IPrimitiveTexture primitiveTexture) : base(size, primitiveTexture)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = (Content.Width - 12) / 2;
            int dHeight = (Content.Height - 10) * 5 / 8;

            CharacterPanel = new CharacterMiniPanel(Game, new ASize(dWidth, dHeight)) { Location = new APoint(1, 1) };
            RelationshipsList = new RelationshipsListPanel(new ASize(dWidth, Content.Height - dHeight - 12)) { Location = CharacterPanel.Location + new APoint(0, CharacterPanel.Height + 10) };
            SettlementsList = new SettlementsListPanel(new ASize(dWidth, Content.Height - 2)) { Location = CharacterPanel.Location + new APoint(CharacterPanel.Width + 10, 0) };

            Add(CharacterPanel);
            Add(RelationshipsList);
            Add(SettlementsList);

            SettlementsList.SelectEvent += (settlement) => SettlementSelectEvent?.Invoke(settlement);
            RelationshipsList.SelectEvent += (player) => PlayerCallEvent?.Invoke(player);

        }

        public void Update()
        {
            if (Player is object) Update(Player);
        }

        public void Update(IPlayer player)
        {

            Player = player;

            CharacterPanel.Update(player.Ruler);
            RelationshipsList.Update(player.Relationships.Keys, player);
            SettlementsList.Update(player.Settlements);

        }

        public void Hide() => Enabled = false;

        public void Show(IPlayer player)
        {
            Enabled = true;
            Update(player);

            Text = "Первый советник игрока " + player.Name;

            ShowEvent?.Invoke();

        }


    }
}
