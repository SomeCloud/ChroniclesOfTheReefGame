﻿using System;
using System.Linq;
using System.Collections.Generic;

using NetLibrary;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Player;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class StatPanel : AEmptyPanel
    {

        public delegate void OnAction();

        public event OnAction MessageClickEvent;
        public event OnAction TurnClickEvent;
        public event OnAction PlayerClickEvent;

        public APanel PlayerName;
        public APanel PlayerCoffers;

        public APanel CurrntTurn;

        public AButton Turn;
        public AButton Mesages;

        public StatPanel(ASize size) : base(size)
        {

        }


        public override void Initialize()
        {

            base.Initialize();

            int dWidth = (Width - 180) / 3;

            PlayerName = new APanel(new ASize(dWidth, Height - 20)) { Parent = this, Location = new APoint(10, 10) };
            PlayerCoffers = new APanel(new ASize(dWidth, Height - 20)) { Parent = this, Location = PlayerName.Location + new APoint(PlayerName.Width + 10, 0) };
            CurrntTurn = new APanel(new ASize(dWidth, Height - 20)) { Parent = this, Location = PlayerCoffers.Location + new APoint(PlayerCoffers.Width + 10, 0) };

            Turn = new AButton(new ASize(60, 60), TexturePack.MiniButtons_General_Turn) { Parent = this, Location = CurrntTurn.Location + new APoint(CurrntTurn.Width + 10, 0) };
            Mesages = new AButton(new ASize(60, 60), TexturePack.MiniButtons_Message) { Parent = this, Location = Turn.Location + new APoint(Turn.Width + 10, 0) };

            PlayerName.MouseClickEvent += (state, mstate) =>
            {
                PlayerClickEvent?.Invoke();
            };
            
            Turn.MouseClickEvent += (state, mstate) =>
            {
                TurnClickEvent?.Invoke();
            };

            Mesages.MouseClickEvent += (state, mstate) =>
            {
                MessageClickEvent?.Invoke();
            };

        }

        public void Update(IPlayer player, int currentTurn, string activePlayerName)
        {

            PlayerName.Text = player.Name;
            PlayerCoffers.Text = "Казна: " + player.Coffers + " ( " + (player.Income - player.Dues is int income && income > 0 ? "+" + income : income.ToString()) + " )";
            CurrntTurn.Text = "Ход (" + activePlayerName + "): " + currentTurn;

        }

    }
}
