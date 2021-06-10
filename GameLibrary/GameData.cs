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

        public void Update(GameData game)
        {
            Players = game.Players;
            Territories = game.Territories;
            Settlements = game.Settlements;
            Units = game.Units;
            Characters = game.Characters;
            TechnologyTrees = game.TechnologyTrees;

            GameMap = game.GameMap;

            CurrentTurn = game.CurrentTurn;
            ActivePlayer = game.ActivePlayer;
            SelectedMapCell = game.SelectedMapCell;
        }

        public void Update(AGame game)
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
        public IPlayer GetPlayer(string name) => Players.Select(x => x.Value).Where(x => x.Name.Equals(name)).FirstOrDefault();
        public AMapCell GetMapCell(APoint location) => GameMap[location];
        public List<IUnit> GetUnits(APoint location) => Units.Values.SelectMany(x => x).Where(x => x.Location.Equals(location)).ToList();

        public bool IsRelative(ICharacter character, ICharacter otherCharacter)
        {
            if (character is null || otherCharacter is null || character.Equals(otherCharacter)) return false;

            Dictionary<int, List<int>> characterParent = new Dictionary<int, List<int>>() { { 0, new List<int>() { character.FatherId, character.MotherId } } };
            Dictionary<int, List<int>> otherCharacterParent = new Dictionary<int, List<int>>() { { 0, new List<int>() { otherCharacter.FatherId, otherCharacter.MotherId } } };

            for (int i = 0; i < 2; i++)
            {
                List<int> temp = new List<int>();
                foreach (int id in new List<int>(characterParent[i]))
                {
                    if (id > 0)
                    {
                        ICharacter parent = GetCharacter(id);
                        temp.AddRange(new int[] { parent.MotherId, parent.FatherId });
                    }
                    else characterParent[i].Remove(id);
                }
                characterParent.Add(i + 1, temp);
                temp.Clear();
                foreach (int id in new List<int>(otherCharacterParent[i]))
                {
                    if (id > 0)
                    {
                        ICharacter parent = GetCharacter(id);
                        temp.AddRange(new int[] { parent.MotherId, parent.FatherId });
                    }
                    else otherCharacterParent[i].Remove(id);
                }
                otherCharacterParent.Add(i + 1, temp);
            }

            return characterParent.Values.SelectMany(x => x).Where(x => otherCharacterParent.Values.SelectMany(y => y).Contains(x)).Count() > 0;

        }

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
