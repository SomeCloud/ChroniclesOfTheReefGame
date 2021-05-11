using System;
using System.Collections.Generic;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary.Map;
using GameLibrary.Unit;
using GameLibrary.Unit.Main;
using GameLibrary.Settlement;
using GameLibrary.Extension;
using GameLibrary.Character;
using GameLibrary.Player;

namespace GameLibrary
{
    public class AGame
    {

        private AMap GameMap;

        private Dictionary<int, IPlayer> _Players;

        private Dictionary<int, List<AMapCell>> _Territories;
        private Dictionary<int, List<ISettlement>> _Settlements;
        private Dictionary<int, List<IUnit>> _Units;
        private Dictionary<int, List<ICharacter>> _Characters;

        private Dictionary<APoint, AMapCell> _Map;

        private AMapCell _SelectedMapCell;
        private IPlayer _ActivePlayer;
        private int _CurrentTurn;

        private int NewCharacterId => _Characters.Sum(x => x.Value.Count) + 1;

        public IReadOnlyDictionary<int, IPlayer> Players => _Players;
        public IReadOnlyDictionary<int, List<AMapCell>> Territories => _Territories;
        public IReadOnlyDictionary<int, List<ISettlement>> Settlements => _Settlements;
        public IReadOnlyDictionary<int, List<IUnit>> Units => _Units;
        public IReadOnlyDictionary<int, List<ICharacter>> Characters => _Characters;

        public IReadOnlyDictionary<APoint, AMapCell> Map { get => _Map; }

        public AMapCell SelectedMapCell => _SelectedMapCell;
        public IPlayer ActivePlayer => _ActivePlayer;
        public int CurrentTurn => _CurrentTurn;
        public ASize Size => GameMap.Size;

        public AGame()
        {

            GameMap = new AMap();

            _Map = GameMap.Map;

            _Territories = new Dictionary<int, List<AMapCell>>();
            _Players = new Dictionary<int, IPlayer>();
            _Settlements = new Dictionary<int, List<ISettlement>>();
            _Units = new Dictionary<int, List<IUnit>>();
            _Characters = new Dictionary<int, List<ICharacter>>();

        }

        public void Initialize(List<string> names, List<ICharacter> characters)
        {

            _Territories.Clear();
            _Players.Clear();
            _Settlements.Clear();
            _Units.Clear();
            _Characters.Clear();

            foreach (string name in names)
            {

                int id = _Players.Count + 1;

                _Territories.Add(id, new List<AMapCell>());
                _Settlements.Add(id, new List<ISettlement>());
                _Units.Add(id, new List<IUnit>());
                _Characters.Add(id, new List<ICharacter>());

                IPlayer newPlayer = new APlayer(id, name, characters[_Players.Count], _Territories[id], _Settlements[id], _Units[id], _Characters[id]);

                foreach (IPlayer player in _Players.Values)
                {
                    player.SetRelationship(newPlayer, ARelationshipType.None);
                    newPlayer.SetRelationship(player, ARelationshipType.None);
                }

                _Players.Add(newPlayer.Id, newPlayer);

            }

        }

        public bool StartGame(ASize size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            _Characters.Add(0, new List<ICharacter>());

            if (size.Count > 0 && _Players.Count > 0)
            {
                GameMap.RandomGeneration(size);

                foreach (var e in GameMap.SetPlayers(_Players.Select(x => x.Value).ToList()))
                {
                    _Players[e.Key].AddSettlement(GetMapCell(e.Value).Settlement);
                    foreach (ICharacter character in _Players[e.Key].Characters) character.SetLocation(e.Value);
                }

                List<APoint> locations = _Settlements.Values.SelectMany(x => x).Select(x => x.Location).ToList();

                for (int i = 0; i < GameExtension.CharactersDefautStartValue * _Settlements.Values.Sum(x => x.Count); i++)
                {
                    ASexType sexType = new[] { ASexType.Female, ASexType.Male }[random.Next(2)];
                    ICharacter character = new ACharacter(GameExtension.CharacterName(sexType), GameExtension.DefaultFamily[random.Next(GameExtension.DefaultFamily.Count)], sexType, random.Next(-45, -5), NewCharacterId, 0);
                    character.SetLocation(locations[random.Next(locations.Count)]);
                    _Characters[0].Add(character);
                }

                _CurrentTurn = 0;
                _ActivePlayer = _Players.Values.First();         
                return true;
            }
            return false;
        }

        public void Turn()
        {
            if (_Players.Keys.ToList().IndexOf(_ActivePlayer.Id) is int id && id + 1 < _Players.Keys.Count()) _ActivePlayer = _Players[_Players.Keys.ToList()[id + 1]];
            else
            {
                _ActivePlayer = _Players[_Players.Keys.ToList()[0]];
                _CurrentTurn++;
                _Units.SelectMany(x => x.Value).ToList().ForEach(x => x.Turn());
                _Players.Values.ToList().ForEach(x => x.Turn());
                //Map.UpdateCells();
            }
        }

        public bool MoveUnit(APoint location)
        {
            if (GetMapCell(location) is AMapCell mapCell && mapCell.BiomeType != ABiomeType.Sea && mapCell.NeighboringCells.Contains(_SelectedMapCell) && _SelectedMapCell.ActiveUnit is IUnit unit && unit.Owner.Equals(_ActivePlayer))
            {
                if (mapCell.Owner is null || mapCell.Owner == _ActivePlayer || mapCell.Owner.IsOpenBorders(_ActivePlayer))
                {
                    unit.Act(location);
                    var locationMapCellUnits = GetUnits(mapCell.Location);
                    var selectedMapCellUnits = GetUnits(_SelectedMapCell.Location);

                    if (selectedMapCellUnits.Count > 0) _SelectedMapCell.SetActiveUnit(selectedMapCellUnits.First());
                    else _SelectedMapCell.SetActiveUnit(null);

                    if (mapCell.ActiveUnit is null)
                    {
                        if (locationMapCellUnits.Count > 0) mapCell.SetActiveUnit(locationMapCellUnits.First());
                        else mapCell.SetActiveUnit(null);
                    }
                    SelectMapCell(location);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void SelectMapCell(APoint location) => _SelectedMapCell = GetMapCell(location);
        public AMapCell GetMapCell(APoint location) => GameMap[location];
        public List<IUnit> GetUnits(APoint location) => _Units.Values.SelectMany(x => x).Where(x => x.Location.Equals(location)).ToList();
        public List<IUnit> GetUnits() => _Units.Values.SelectMany(x => x).Where(x => x.Location.Equals(_SelectedMapCell.Location)).ToList();
        public ICharacter GetCharacter(int id) => _Characters.SelectMany(x => x.Value).Where(x => x.Id == id).First();
        public List<ICharacter> GetCharacters(APoint location) => _Characters.SelectMany(x => x.Value).Where(x => x.Location.Equals(location)).ToList();
        public void AddUnit(AUnitType unitType, List<APeople> commoners, string name)
        {
            IUnit unit = null;
            if (commoners.Sum(x => x.Count) > 0) switch (unitType)
                {
                    case AUnitType.Colonist:
                        unit = new AUnitColonist(_Units.Values.Sum(x => x.Count + 1), _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Spearman:
                        unit = new AUnitSpearman(_Units.Values.Sum(x => x.Count + 1), _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Farmer:
                        unit = new AUnitFarmer(_Units.Values.Sum(x => x.Count + 1), _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Swordsman:
                        unit = new AUnitSwordsman(_Units.Values.Sum(x => x.Count + 1), _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Archer:
                        unit = new AUnitArcher(_Units.Values.Sum(x => x.Count + 1), _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Missionary:
                        unit = new AUnitMissionary(_Units.Values.Sum(x => x.Count + 1), _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Axeman:
                        unit = new AUnitAxeman(_Units.Values.Sum(x => x.Count + 1), _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Warrior:
                        unit = new AUnitWarrior(_Units.Values.Sum(x => x.Count + 1), _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                }
            if (unit is object)
            {
                _Players[unit.Owner.Id].AddUnit(unit);
                AMapCell mapCell = GetMapCell(unit.Location);
                if (mapCell.ActiveUnit is null) mapCell.SetActiveUnit(unit);
            }
        }
    }
}
