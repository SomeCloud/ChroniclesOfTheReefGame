using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Map;
using GameLibrary.Unit;
using GameLibrary.Unit.Main;
using GameLibrary.Settlement;
using GameLibrary.Extension;
using GameLibrary.Character;
using GameLibrary.Player;
using GameLibrary.Technology;
using GameLibrary.Message;

namespace GameLibrary
{

    [Serializable]
    public class GameData
    {

        public IReadOnlyDictionary<int, IPlayer> Players;
        public IReadOnlyDictionary<int, List<AMapCell>> Territories;
        public IReadOnlyDictionary<int, List<ISettlement>> Settlements;
        public IReadOnlyDictionary<int, List<IUnit>> Units;
        public IReadOnlyDictionary<int, List<ICharacter>> Characters;
        public IReadOnlyDictionary<int, ATechnologyTree> TechnologyTrees;

        public AMap GameMap;

        public int CurrentTurn;
        public IPlayer ActivePlayer;
        public AMapCell SelectedMapCell;

        public GameData(AGame game)
        {

            Players = game.Players;
            Territories = game.Territories;
            Settlements = game.Settlements;
            Units = game.Units;
            Characters = game.Characters;
            TechnologyTrees = game.Technologies;

            GameMap = game.GameMap;

            CurrentTurn = game.CurrentTurn;
            ActivePlayer = game.ActivePlayer;
            SelectedMapCell = game.SelectedMapCell;

        }

    }
}
