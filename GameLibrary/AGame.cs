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
using GameLibrary.Technology;
using GameLibrary.Message;

namespace GameLibrary
{

    public delegate void OnTurn();

    public delegate void OnGameOver(IPlayer player);

    [Serializable]
    public class AGame
    {

        public event OnTurn TurnEvent;
        public event OnGameOver GameOverEvent;

        public delegate void OnAttack(IUnit dUnit, IUnit aUnit, int dPower, int aPower, int dResult, int aResult);
        public event OnAttack OnAttackResultEvent;

        private AMap _GameMap;

        private bool _Status;

        private Dictionary<int, IPlayer> _Players;

        private Dictionary<int, List<AMapCell>> _Territories;
        private Dictionary<int, List<ISettlement>> _Settlements;
        private Dictionary<int, List<IUnit>> _Units;
        private Dictionary<int, List<ICharacter>> _Characters;
        private Dictionary<int, ATechnologyTree> _TechnologyTrees;

        private Dictionary<APoint, AMapCell> _Map;

        private AMapCell _SelectedMapCell;
        private IPlayer _ActivePlayer;
        private int _CurrentTurn;

        private int NewCharacterId => _Characters.Sum(x => x.Value.Count) + 1;

        public AMap GameMap => _GameMap;

        public IReadOnlyDictionary<int, IPlayer> Players => _Players;
        public IReadOnlyDictionary<int, List<AMapCell>> Territories => _Territories;
        public IReadOnlyDictionary<int, List<ISettlement>> Settlements => _Settlements;
        public IReadOnlyDictionary<int, List<IUnit>> Units => _Units;
        public IReadOnlyDictionary<int, List<ICharacter>> Characters => _Characters;
        public IReadOnlyDictionary<int, ATechnologyTree> Technologies => _TechnologyTrees;

        public IReadOnlyDictionary<APoint, AMapCell> Map { get => _Map; }

        public AMapCell SelectedMapCell => _SelectedMapCell;
        public IPlayer ActivePlayer => _ActivePlayer;
        public int CurrentTurn => _CurrentTurn;
        public ASize Size => _GameMap.Size;

        public bool IsMapCellSelected => _SelectedMapCell is object;

        public bool Status => _Status;

        public AGame()
        {

            _GameMap = new AMap();

            _Map = _GameMap.Map;

            _Territories = new Dictionary<int, List<AMapCell>>();
            _Players = new Dictionary<int, IPlayer>();
            _Settlements = new Dictionary<int, List<ISettlement>>();
            _Units = new Dictionary<int, List<IUnit>>();
            _Characters = new Dictionary<int, List<ICharacter>>();
            _TechnologyTrees = new Dictionary<int, ATechnologyTree>();

        }

        public void Initialize(List<string> names, List<ICharacter> characters)
        {

            Random random = new Random((int)DateTime.Now.Ticks);

            _Territories.Clear();
            _Players.Clear();
            _Settlements.Clear();
            _Units.Clear();
            _Characters.Clear();
            _TechnologyTrees.Clear();

            foreach (string name in names)
            {

                int id = _Players.Count + 1;

                _Territories.Add(id, new List<AMapCell>());
                _Settlements.Add(id, new List<ISettlement>());
                _Units.Add(id, new List<IUnit>());
                _Characters.Add(id, new List<ICharacter>());
                _TechnologyTrees.Add(id, new ATechnologyTree());


                IPlayer newPlayer = new APlayer(id, name, characters[_Players.Count], _Territories[id], _Settlements[id], _Units[id], _Characters[id], _TechnologyTrees[id]);

                int gift = random.Next(50, 60);

                newPlayer.SendMessage(new AMessage(newPlayer, newPlayer, "Да здравствует правитель!", 
                    "Спустя столетия безраздельного правления старейшин, народ решил избрать единого правителя,\n" +
                    "способного возглавить их на пути к процветанию. Этим правителем стали вы, " + newPlayer.Ruler.FullName + ".\n" +
                    "Сможете ли вы оправдать возложенные на вас надежды и обессмертить свое имя в истории,\n" +
                    "или же вашему роду суждено быть преданным забвению - все это, зависит от вас и ваших решений,\n" +
                    "совершайте их обдумано!\n\n" +
                    "В качестве дара народ преподносит вам " + gift + " золота", 
                    () => newPlayer.ChangeCoffers(gift), false));

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
            _Status = true;

            if (size.Count > 0 && _Players.Count > 0)
            {
                _GameMap.RandomGeneration(size);

                foreach (var e in _GameMap.SetPlayers(_Players.Select(x => x.Value).ToList()))
                {
                    _Players[e.Key].AddSettlement(GetMapCell(e.Value).Settlement);
                    _Players[e.Key].ExploreTerritories(GetMapCell(e.Value).NeighboringCells.Select(x => x.Location));
                    _Players[e.Key].ExploreTerritories(GetMapCell(e.Value).Settlement.Territories.Select(x => x.Location));
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

                foreach (AMapCell mapCell in Map.Values) mapCell.ExpansionEvent += (neighborCell) => {
                    //GetUnits(neighborCell.Location).ForEach(x => neighborCell.Deportation(x)); 
                    List<IUnit> units = GetUnits(neighborCell.Location).Where(x => !x.Owner.Id.Equals(neighborCell.Owner.Id) || !x.Owner.IsOpenBorders(neighborCell.Owner)).ToList();
                    foreach (IUnit unit in units)
                        if (neighborCell.Owner.Territories.SelectMany(x => x.NeighboringCells).Where(x => x.Owner.Equals(unit.Owner) || x.Owner is null || x.Owner.IsOpenBorders(unit.Owner)) is List<AMapCell> cells && cells.Count > 0)
                        {
                            AMapCell newLocation = cells.First();
                            unit.Move(newLocation.Location);
                            if (neighborCell.ActiveUnit.Equals(unit)) neighborCell.SetActiveUnit(null);
                            if (newLocation.ActiveUnit is null) newLocation.SetActiveUnit(unit);
                        }
                        else if (unit.Owner.Settlements.Count > 0)
                        {
                            AMapCell newLocation = GetMapCell(unit.Owner.Settlements.First().Location);
                            unit.Move(newLocation.Location);
                            if (neighborCell.ActiveUnit.Equals(unit)) neighborCell.SetActiveUnit(null);
                            if (newLocation.ActiveUnit is null) newLocation.SetActiveUnit(unit);
                        }
                        else
                        {
                            neighborCell.Population.Add(unit.Squad.ToList());
                            unit.Owner.RemoveUnit(unit);
                            unit.Dispose();
                        }
                    if (neighborCell.ActiveUnit is null)
                    {
                        if (GetUnits(neighborCell.Location) is List<IUnit> newUnits && newUnits.Count > 0) neighborCell.SetActiveUnit(newUnits.First());
                    }
                };

                _CurrentTurn = 0;
                _ActivePlayer = _Players.Values.First();

                return true;
            }
            return false;
        }
        private static bool Probability(float value) => value > new Random((int)DateTime.Now.Ticks).NextDouble();
        private static bool Probability(int value) => (float)value / 2 > new Random((int)DateTime.Now.Ticks).Next(value);
        private static bool Probability(int value, int extrValue) => new Random((int)DateTime.Now.Ticks).Next(value) > new Random((int)DateTime.Now.Ticks).Next(extrValue);

        public void Turn()
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            int elder = _Characters.SelectMany(x => x.Value).Max(x => x.Age(CurrentTurn));

            List<IPlayer> alive = _Players.Select(x => x.Value).Where(x => x.Status).ToList();

            if (_Status) 
                if (_Players.Keys.ToList().IndexOf(_ActivePlayer.Id) is int id && id + 1 < _Players.Keys.Count()) _ActivePlayer = _Players[_Players.Keys.ToList()[id + 1]];
                else
                {
                    _ActivePlayer = _Players[_Players.Keys.ToList()[0]];
                    _CurrentTurn++;
                    _Units.SelectMany(x => x.Value).ToList().ForEach(x => x.Turn());
                    _Players.Values.ToList().ForEach(x => x.Turn());
                    _Settlements.SelectMany(x => x.Value).ToList().ForEach(x => x.Turn());

                    foreach (ICharacter character in _Characters.SelectMany(x => x.Value).Where(x => x.IsAlive))
                    {
                        if (character.SexType.Equals(ASexType.Female))
                        {
                            if (character.IsMarried && Probability(character.Fertility, GetCharacter(character.SpouseId).Fertility))
                            {
                                ICharacter father = GetCharacter(character.SpouseId);
                                ASexType sexType = Probability(0.5f) ? ASexType.Male : ASexType.Female;
                                ICharacter child = new ACharacter(GameExtension.CharacterName(sexType), character.IsMatrilinearMarriage ? character.FamilyName : father.FamilyName, sexType, CurrentTurn, _Characters.SelectMany(x => x.Value).Max(x => x.Id) + 1, character.Id, father.Id, character.Id);
                                character.AddChild(child.Id);
                                father.AddChild(child.Id);
                                _Characters[character.IsMatrilinearMarriage ? character.OwnerId : father.OwnerId].Add(child);
                            }
                        }
                        if (Probability(0.5f) && random.Next(Math.Max(elder - character.Age(CurrentTurn), 1)) == random.Next(character.Health))
                        {
                            character.Kill(CurrentTurn);
                            IPlayer player;
                            if (CharacterIsRuler(character, out player) && player.Status)
                            {
                                player.SendMessage(new AMessage(player, player, "Король умер, да здравствует король!", "Прежний правитель " + player.Ruler + " скончался на " + player.Ruler.Age(CurrentTurn) + " году своей жизни.\n Народ с нетерпением ожидает вступления в права следующего монарха,\n в надежде что тот преумножит свершения своего предшественника.", () => { }, false));
                                if (player.Characters.Where(x => x.IsAlive).Count() > 0) player.UpdateRulerByHeir(CurrentTurn);
                                else player.SetStatus(false);
                            }
                        }
                    }

                    Map.Values.ToList().ForEach(x => x.Turn());
                }
            if (!_ActivePlayer.Status) Turn();

            if (_Status && alive.Count <= 1)
            {
                GameOverEvent?.Invoke(alive.FirstOrDefault());
                _Status = false;
            }

        }

        public bool MoveUnit(APoint location)
        {
            AMapCell mapCell = GetMapCell(location);
            List<IUnit> enemies = GetEnemies(location);
            IUnit unit = SelectedMapCell.ActiveUnit;
            if (mapCell.BiomeType != ABiomeType.Sea && mapCell.NeighboringCells.Contains(_SelectedMapCell) && unit is object && unit.Owner.Equals(_ActivePlayer) && unit.Action > 0)
                if (enemies.Count > 0)
                {
                    IUnit defender = enemies.First();
                    enemies.Remove(defender);
                    bool isWin = Attack(defender, unit);
                    if (isWin && enemies.Count == 0)
                    {
                        unit.Move(location);
                        if (_Players.Values.Where(x => x.Relationship(_ActivePlayer).Equals(ARelationshipType.War)).Contains(mapCell.Owner) && mapCell.IsSettlement)
                        {
                            ISettlement settlement = mapCell.Settlement;
                            mapCell.Owner.RemoveSettlement(settlement);
                            unit.Owner.AddSettlement(settlement);
                            unit.Owner.ExploreTerritories(settlement.Territories.Select(x => x.Location));
                            unit.Owner.ExploreTerritories(mapCell.NeighboringCells.Select(x => x.Location));
                        }
                        mapCell.SetActiveUnit(unit);
                        SetActiveUnit(_SelectedMapCell.Location);
                        return true;
                    }
                    else if (isWin) mapCell.SetActiveUnit(enemies.First());
                }
                else if (_Players.Values.Where(x => x.Relationship(_ActivePlayer).Equals(ARelationshipType.War)).Contains(mapCell.Owner) && mapCell.IsSettlement)
                {
                    unit.Act(location);
                    mapCell.SetActiveUnit(unit);
                    SetActiveUnit(_SelectedMapCell.Location);
                    ISettlement settlement = mapCell.Settlement;
                    mapCell.Owner.RemoveSettlement(settlement);
                    unit.Owner.AddSettlement(settlement);
                    unit.Owner.ExploreTerritories(settlement.Territories.Select(x => x.Location));
                    unit.Owner.ExploreTerritories(mapCell.NeighboringCells.Select(x => x.Location));
                    return true;
                }
                else if (mapCell.Owner is null || mapCell.Owner.Equals(_ActivePlayer) || mapCell.Owner.IsOpenBorders(_ActivePlayer))
                {
                    unit.Act(location);
                    mapCell.SetActiveUnit(unit);
                    SetActiveUnit(_SelectedMapCell.Location);
                    _ActivePlayer.ExploreTerritories(mapCell.NeighboringCells.Select(x => x.Location));
                    return true;
                }

            return false;

        }

        public bool Attack(IUnit defender, IUnit attacker)
        {
            bool isWin = false;
            Random random = new Random((int)DateTime.Now.Ticks);

            int defenderPower = defender.Force * defender.Count * (defender.IsGeneral? defender.General.MartialSkills: 1);
            int attackerPower = attacker.Force * attacker.Count * (defender.IsGeneral ? defender.General.MartialSkills : 1);

            int dDefender = Convert.ToInt32((float)defenderPower / attackerPower * attacker.Count);
            int dAttacker = Convert.ToInt32((float)attackerPower / defenderPower * defender.Count);

            dDefender -= random.Next(Convert.ToInt32(dDefender * (3 / 4f)));
            dAttacker -= random.Next(Convert.ToInt32(dAttacker * (3 / 4f)));

            OnAttackResultEvent?.Invoke(defender, attacker, defenderPower, attackerPower, Math.Min(dAttacker, defender.Count), Math.Min(dDefender, attacker.Count));

            if (defender.Count - dAttacker > 0)
            {
                defender.UnderAttack(dAttacker);
                isWin = false;
            }
            else
            {
                defender.Owner.RemoveUnit(defender);
                isWin = true;
            }

            if (attacker.Count - dDefender > 0)
            {
                attacker.Attack(dDefender);
                isWin = isWin? true: false;
            }
            else
            {
                attacker.Owner.RemoveUnit(attacker);
                isWin = false;
            }

            return isWin;
        }

        public void SetActiveUnit(APoint location)
        {
            AMapCell mapCell = GetMapCell(location);
            List<IUnit> units = GetUnits(location);
            if (units.Count > 0 && units.Where(x => x.Owner.Equals(_ActivePlayer)) is List<IUnit> playerUnits && playerUnits.Count > 0) mapCell.SetActiveUnit(playerUnits.First());
            else if (units.Count > 0) mapCell.SetActiveUnit(units.First());
            else mapCell.SetActiveUnit(null);
        }
        public void SelectMapCell(APoint location) => _SelectedMapCell = GetMapCell(location);
        public AMapCell GetMapCell(APoint location) => _GameMap[location];
        public IPlayer GetPlayer(string name) => Players.Select(x => x.Value).Where(x => x.Name == name).First();
        public List<IUnit> GetUnits(APoint location) => _Units.Values.SelectMany(x => x).Where(x => x.Location.Equals(location)).ToList();
        public IUnit GetUnit(IUnit unit) => _Units.Values.SelectMany(x => x).Where(x => x.Equals(unit)).FirstOrDefault();
        public List<IUnit> GetEnemies(APoint location) => _Units.Values.SelectMany(x => x).Where(x => x.Location.Equals(location) && x.Owner.Relationship(_ActivePlayer).Equals(ARelationshipType.War)).ToList();
        public List<IUnit> GetUnits() => _Units.Values.SelectMany(x => x).Where(x => x.Location.Equals(_SelectedMapCell.Location)).ToList();
        public ICharacter GetCharacter(int id) => _Characters.SelectMany(x => x.Value).Where(x => x.Id == id).First();
        public List<ICharacter> GetCharacters(APoint location) => _Characters.SelectMany(x => x.Value).Where(x => x.Location.Equals(location)).ToList();
        public bool AddUnit(AUnitType unitType, List<APeople> commoners, string name)
        {
            IUnit unit = null;
            int id = _Units.Values.SelectMany(x => x).ToList() is List<IUnit> units && units.Count > 0? units.OrderBy(x => x.Id).Last().Id + 1: 1;
            if (commoners.Sum(x => x.Count) > 0) switch (unitType)
                {
                    case AUnitType.Colonist:
                        unit = new AUnitColonist(id, _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Spearman:
                        unit = new AUnitSpearman(id, _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Farmer:
                        unit = new AUnitFarmer(id, _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Swordsman:
                        unit = new AUnitSwordsman(id, _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Archer:
                        unit = new AUnitArcher(id, _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Missionary:
                        unit = new AUnitMissionary(id, _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Axeman:
                        unit = new AUnitAxeman(id, _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                    case AUnitType.Warrior:
                        unit = new AUnitWarrior(id, _SelectedMapCell.Owner, _SelectedMapCell.Location, commoners, name);
                        break;
                }
            if (unit is object && unit.Owner.ChangeCoffers(-unit.Cost))
            {
                _Players[unit.Owner.Id].AddUnit(unit);
                AMapCell mapCell = GetMapCell(unit.Location);
                if (mapCell.ActiveUnit is null) mapCell.SetActiveUnit(unit);
                return true;
            }
            return false;
        }
        public bool AddSettlement(IUnit unit, string name)
        {
            AMapCell mapCell = GetMapCell(unit.Location);
            List<AMapCell> cells = mapCell.NeighboringCells.SelectMany(x => x.NeighboringCells).ToList();
            if (unit.UnitType.Equals(AUnitType.Colonist) && cells.All(x => !x.IsSettlement) && !mapCell.IsOwned)
            {           
                IPlayer player = unit.Owner;
                ISettlement settlement = new ASettlement(mapCell, player, name);
                mapCell.Population.Add(unit.Squad.ToList());
                player.AddSettlement(settlement);
                mapCell.SetSettlement(settlement);
                player.RemoveUnit(unit);
                return true;
            }
            return false;
        }

        public void SetRelationship(IPlayer player, IPlayer otherPlayer, ARelationshipType relationshipType)
        {
            player.SetRelationship(otherPlayer, relationshipType);
            otherPlayer.SetRelationship(player, relationshipType);
            if (relationshipType.Equals(ARelationshipType.War))
            {
                List<IPlayer> playerAllies = player.Relationships.Where(x => x.Value.Equals(ARelationshipType.Union)).Select(x => x.Key).ToList();
                List<IPlayer> otherPlayerAllies = otherPlayer.Relationships.Where(x => x.Value.Equals(ARelationshipType.Union)).Select(x => x.Key).ToList();
                foreach (IPlayer pl in new List<IPlayer>(playerAllies)) if (otherPlayerAllies.Contains(pl))
                    {
                        pl.SetRelationship(player, ARelationshipType.Neutrality);
                        pl.SetRelationship(otherPlayer, ARelationshipType.Neutrality);
                        player.SetRelationship(pl, ARelationshipType.Neutrality);
                        otherPlayer.SetRelationship(pl, ARelationshipType.Neutrality);
                        playerAllies.Remove(pl);
                        otherPlayerAllies.Remove(pl);
                    }
                foreach (IPlayer pl in playerAllies) pl.SetRelationship(otherPlayer, ARelationshipType.War);
                foreach (IPlayer pl in otherPlayerAllies) pl.SetRelationship(player, ARelationshipType.War);
            }
        }

        public void SetRelationship(IPlayer player, ARelationshipType relationshipType) => SetRelationship(player, _ActivePlayer, relationshipType);

        public bool CharacterIsRuler(ICharacter character, out IPlayer player)
        {
            Dictionary<ICharacter, IPlayer> rulers = _Players.ToDictionary(x => x.Value.Ruler, x => x.Value);
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
