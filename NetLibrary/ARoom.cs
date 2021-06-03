﻿using System;
using System.Collections.Generic;
using System.Linq;

using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Character;
using GameLibrary.Extension;

namespace NetLibrary
{
    [Serializable]
    public class ARoom
    {

        private int _Id;
        private string _Name;
        private int _PlayersCount;
        private List<RPlayer> _Players;

        private GameData _Game;

        private AGameStatus _GameStatus;

        private ASize _MapSize;

        public int Id => _Id;
        public string Name => _Name;
        public int PlayersCount => _PlayersCount;
        public IReadOnlyList<RPlayer> Players => _Players;

        public GameData Game => _Game;

        public AGameStatus GameStatus => _GameStatus;

        public ASize MapSize => _MapSize;

        public ARoom(string name, int id, int playersCount)
        {
            _Name = name;
            _Id = id;
            _PlayersCount = playersCount;
            _Players = new List<RPlayer>();
            _GameStatus = AGameStatus.Wait;
        }

        public ARoom(ARoom room)
        {
            _Name = room._Name;
            _Id = room._Id;
            _PlayersCount = room._PlayersCount;
            _Players = room._Players;
            _GameStatus = room._GameStatus;
        }

        public void SetName(string name) => _Name = name;
        public void SetMapSize(ASize size) => _MapSize = size;
        public void SetPlayersCount(int count) => _PlayersCount = count;

        public void StartGame(AGame game)
        {

            _Game = new GameData(game);
            _GameStatus = AGameStatus.Game;

        }
        public void UpdateGame(AGame game) => _Game = new GameData(game);


        public void OverGame()
        {
            _GameStatus = AGameStatus.Over;
        }

        public bool Connect(RPlayer player)
        {
            if (_Players.Contains(player)) return false;
            else _Players.Add(player);
            return true;
        }

        public bool Disconnect(RPlayer player)
        {
            if (_Players.Contains(player)) _Players.Remove(player);
            else return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is ARoom room)
                return Id == room.Id && Name == room.Name && PlayersCount == room.PlayersCount && GameStatus == room.GameStatus && Players == room.Players;
            return false;
        }

    }
}
