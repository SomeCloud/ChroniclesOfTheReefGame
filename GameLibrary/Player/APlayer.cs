using System;
using System.Collections.Generic;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Map;
using GameLibrary.Unit.Main;
using GameLibrary.Settlement;
using GameLibrary.Character;
using GameLibrary.Technology;
using GameLibrary.Extension;
using GameLibrary.Message;

namespace GameLibrary.Player
{
    public class APlayer: IPlayer
    {

        private string _Name;

        private int _Income;
        private int _Dues;
        private int _Coffers;

        private bool _Status;

        private ICharacter _Ruler;
        private ICharacter _Heir;

        private List<AMapCell> _Territories;
        private List<APoint> _ExploredTerritories;
        private List<ISettlement> _Settlements;
        private List<IUnit> _Units;
        private ATechnologyTree _Technologies;
        private List<IMessage> _Messages;

        public IReadOnlyDictionary<AResourceType, int> Resources
        {
            get
            {
                return _Territories.Where(x => IsResource(x.ResourceType)).Where(x => x.IsMined).Select(x => x.ResourceType).GroupBy(res => res).OrderByDescending(g => g.Count()).Select(g => new { key = g.Key, count = g.Count() }).ToDictionary(e => e.key, x => x.count);
                //List<AResourceType> resources = _Settlements.SelectMany(x => x.Territories).Where(x => x.IsMined).Select(x => x.ResourceType).ToList();
                /*Dictionary<AResourceType, int> result = new Dictionary<AResourceType, int>();
                foreach (var resource in _Settlements.SelectMany(x => x.Resources))
                {
                    if (result.ContainsKey(resource.Key)) result[resource.Key]+= resource.Value;
                    else result.Add(resource.Key, resource.Value);
                }
                return result;*/
            }
        }
        private Dictionary<IPlayer, ARelationshipType> _Relationships;

        private List<ICharacter> _Characters;

        public int Id { get; }
        public string Name { get => _Name; }

        public int Income { get => _Income; }
        public int Dues { get => _Dues; }
        public int Coffers { get => _Coffers; }

        public bool Status { get => _Status; }

        public ICharacter Ruler { get => _Ruler; }
        public ICharacter Heir { get => _Heir; }

        public IReadOnlyList<AMapCell> Territories { get => _Territories; }
        public IReadOnlyList<APoint> ExploredTerritories => _ExploredTerritories;
        public IReadOnlyList<ISettlement> Settlements { get => _Settlements; }
        public IReadOnlyList<IUnit> Units { get => _Units; }

        public ATechnologyTree Technologies => _Technologies;

        public IReadOnlyList<IMessage> Messages { get => _Messages; }

        public IReadOnlyDictionary<IPlayer, ARelationshipType> Relationships { get => _Relationships; }

        public IReadOnlyList<ICharacter> Characters { get => _Characters; }

        public APlayer(int id, string name, ICharacter character)
        {
            _Coffers = 50;
            _Status = true;
            Id = id;
            _Name = name;
            _Ruler = character;
            _Territories = new List<AMapCell>();
            _Settlements = new List<ISettlement>();
            _Units = new List<IUnit>();
            _Relationships = new Dictionary<IPlayer, ARelationshipType>();
            _Characters = new List<ICharacter>();
            _Technologies = new ATechnologyTree();
            _Messages = new List<IMessage>();
        }

        public APlayer(int id, string name, ICharacter character, List<AMapCell> territories, List<ISettlement> settlements, List<IUnit> units, List<ICharacter> characters, ATechnologyTree technologies) :this(id, name, character)
        {
            _Territories = territories;
            _Settlements = settlements;
            _Units = units;
            _Characters = characters;
            _Characters.Add(character);

            _Technologies = technologies;
            _ExploredTerritories = new List<APoint>();

            RecalculateIncome();
            RecalculateDues();
        }

        public void SetStatus(bool status) => _Status = status;

        public void SetName(string name) => _Name = name;
        public void SetRuler(ICharacter character) { if (_Characters.Contains(character)) _Ruler = character; }
        public void SetHeir(ICharacter character) { if (_Characters.Contains(character)) _Heir = character; }
        public void SetRelationship(IPlayer player, ARelationshipType relationship) { 
            if (_Relationships.ContainsKey(player)) _Relationships[player] = relationship; 
            else _Relationships.Add(player, relationship);
        }

        public void UpdateRulerByHeir(int currentTurn)
        {
            if (_Heir is object) SetRuler(_Heir);
            else SetRuler(_Characters.Where(x => x.IsAlive).OrderByDescending(i => i.Age(currentTurn)).FirstOrDefault());
        }

        public void SendMessage(IMessage message)
        {
            _Messages.Add(message);
        }

        public void RemoveMessage(IMessage message)
        {
            if (_Messages.Contains(message)) _Messages.Remove(message);
        }

        public void AddTerritory(AMapCell territory) { if (!_Territories.Contains(territory)) _Territories.Add(territory); RecalculateIncome(); }
        public void AddSettlement(ISettlement settlement)
        {
            if (!_Settlements.Contains(settlement))
            {
                _Settlements.Add(settlement);
                settlement.SetOwner(this);
                settlement.Territories.ToList().ForEach(x => x.SetOwner(this));
                settlement.Territories.ToList().ForEach(x => AddTerritory(x));
            } 
            RecalculateIncome();
        }
        public void AddUnit(IUnit unit) { if (!_Units.Contains(unit)) _Units.Add(unit); RecalculateDues(); }
        public void AddCharacter(ICharacter character) { if (!_Characters.Contains(character)) _Characters.Add(character); }

        public void RemoveTerritory(AMapCell territory) { 
            if (_Territories.Contains(territory)) _Territories.Remove(territory);
            if (_Settlements.Count.Equals(0)) _Status = false;
            RecalculateIncome(); 
        }
        public void RemoveSettlement(ISettlement settlement) { 
            if (_Settlements.Contains(settlement)) _Settlements.Remove(settlement);
            if (_Settlements.Count.Equals(0)) _Status = false;
            settlement.Territories.Where(x => _Territories.Contains(x)).ToList().ForEach(x => RemoveTerritory(x)); 
            RecalculateIncome();
        }
        public void RemoveUnit(IUnit unit) { if (_Units.Contains(unit)) _Units.Remove(unit); RecalculateDues(); }
        public void RemoveCharacter(ICharacter character) { if (_Characters.Contains(character)) _Characters.Remove(character); }
        public void ExploreTerritories(IEnumerable<APoint> territories) => territories.Where(x => !_ExploredTerritories.Contains(x)).ToList().ForEach(x => _ExploredTerritories.Add(x));

        public bool IsResource(AResourceType resourceType) => Technologies.Technologies.Where(x => GameExtension.ResourceRequiredTechnologies[resourceType].Equals(x.Key)).All(x => x.Value.IsCompleted);

        public void Turn()
        {
            RecalculateIncome();
            RecalculateDues();
            RecalculateCoffers();
        }

        public bool IsTerritory(AMapCell territory) => _Territories.Contains(territory);
        public bool IsSettlement(ISettlement settlement) => _Settlements.Contains(settlement);
        public bool IsUnit(IUnit unit) => _Units.Contains(unit);
        public bool IsCharacter(ICharacter character) => _Characters.Contains(character);
        public bool IsOpenBorders(IPlayer player) => _Relationships.ContainsKey(player) && (_Relationships[player] == ARelationshipType.Union || _Relationships[player] == ARelationshipType.War);

        public ARelationshipType Relationship(IPlayer player) { if (_Relationships.ContainsKey(player)) return _Relationships[player]; return ARelationshipType.None; }

        private void RecalculateIncome() { _Income = 0; foreach (ISettlement settlement in _Settlements) _Income += settlement.Income; }
        private void RecalculateDues() {  _Dues = 0; float temp = 0; foreach (IUnit unit in _Units) temp += unit.ContentTax * unit.Count * 0.01f; _Dues = Convert.ToInt32(temp); } 
        private void RecalculateCoffers() { _Coffers += _Income - _Dues; }
        public bool ChangeCoffers(int count)
        {
            if (count > 0 || _Coffers + count >= 0) _Coffers += count;
            else return false;
            return true;
        }

        public override string ToString()
        {
            return _Name + " | Казна: " + _Coffers + " ( " + (_Income - _Dues is int income && income > 0 ? "+" + income : income.ToString()) + " ) | Отряды: " + _Units.Count + " | Территории: " + _Territories.Count + " | Поселения: " + _Settlements.Count;
        }

    }
}
