using System;
using System.Collections.Generic;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;

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

        public bool IsMapCellSelected => SelectedMapCell is object;

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

        public ICharacter GetCharacter(int id) => Characters.SelectMany(x => x.Value).Where(x => x.Id == id).First();
        public IPlayer GetPlayer(string name) => Players.Select(x => x.Value).Where(x => x.Name == name).First();
        public AMapCell GetMapCell(APoint location) => GameMap[location];
        public List<IUnit> GetUnits(APoint location) => Units.Values.SelectMany(x => x).Where(x => x.Location.Equals(location)).ToList();

        public bool CharacterIsRuler(ICharacter character, out IPlayer player)
        {
            Dictionary<ICharacter, IPlayer> rulers = Players.ToDictionary(x => x.Value.Ruler, x => x.Value);
            player = null;
            if (rulers.ContainsKey(character))
            {
                player = rulers[character];
                return true;
            }
            return false;
        }

    }
}
