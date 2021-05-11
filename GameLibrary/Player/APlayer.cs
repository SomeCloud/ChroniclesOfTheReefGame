using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Map;
using GameLibrary.Unit.Main;
using GameLibrary.Settlement;
using GameLibrary.Character;

namespace GameLibrary.Player
{
    public class APlayer: IPlayer
    {

        private string _Name;

        private int _Income;
        private int _Dues;
        private int _Coffers;

        private ICharacter _Ruler;

        private List<AMapCell> _Territories;
        private List<ISettlement> _Settlements;
        private List<IUnit> _Units;

        private Dictionary<IPlayer, ARelationshipType> _Relationships;

        private List<ICharacter> _Characters;

        public int Id { get; }
        public string Name { get => _Name; }

        public int Income { get => _Income; }
        public int Dues { get => _Dues; }
        public int Coffers { get => _Coffers; }

        public ICharacter Ruler { get => _Ruler; }

        public IReadOnlyList<AMapCell> Territories { get => _Territories; }
        public IReadOnlyList<ISettlement> Settlements { get => _Settlements; }
        public IReadOnlyList<IUnit> Units { get => _Units; }

        public IReadOnlyDictionary<IPlayer, ARelationshipType> Relationships { get => _Relationships; }

        public IReadOnlyList<ICharacter> Characters { get => _Characters; }

        public APlayer(int id, string name, ICharacter character)
        {
            Id = id;
            _Name = name;
            _Ruler = character;
            _Territories = new List<AMapCell>();
            _Settlements = new List<ISettlement>();
            _Units = new List<IUnit>();
            _Relationships = new Dictionary<IPlayer, ARelationshipType>();
            _Characters = new List<ICharacter>();
        }

        public APlayer(int id, string name, ICharacter character, List<AMapCell> territories, List<ISettlement> settlements, List<IUnit> units, List<ICharacter> characters) :this(id, name, character)
        {
            _Territories = territories;
            _Settlements = settlements;
            _Units = units;
            _Characters = characters;
            _Characters.Add(character);

            RecalculateIncome();
            RecalculateDues();
        }

        public void SetName(string name) => _Name = name;
        public void SetRuler(ICharacter character) { if (_Characters.Contains(character)) _Ruler = character; }
        public void SetRelationship(IPlayer player, ARelationshipType relationship) { if (_Relationships.ContainsKey(player)) _Relationships[player] = relationship; else _Relationships.Add(player, relationship); }

        public void AddTerritory(AMapCell territory) { if (!_Territories.Contains(territory)) _Territories.Add(territory); RecalculateIncome(); }
        public void AddSettlement(ISettlement settlement) { if (!_Settlements.Contains(settlement)) _Settlements.Add(settlement); foreach (AMapCell territory in settlement.Territories) AddTerritory(territory); RecalculateIncome(); }
        public void AddUnit(IUnit unit) { if (!_Units.Contains(unit)) _Units.Add(unit); RecalculateDues(); }
        public void AddCharacter(ICharacter character) { if (!_Characters.Contains(character)) _Characters.Add(character); }

        public void RemoveTerritory(AMapCell territory) { if (_Territories.Contains(territory)) _Territories.Remove(territory); RecalculateIncome(); }
        public void RemoveSettlement(ISettlement settlement) { if (_Settlements.Contains(settlement)) _Settlements.Remove(settlement); RecalculateIncome(); }
        public void RemoveUnit(IUnit unit) { if (_Units.Contains(unit)) _Units.Remove(unit); RecalculateDues(); }
        public void RemoveCharacter(ICharacter character) { if (_Characters.Contains(character)) _Characters.Remove(character); }

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
        public bool IsOpenBorders(IPlayer player) => _Relationships[player] == ARelationshipType.Union || _Relationships[player] == ARelationshipType.War;

        public ARelationshipType Relationship(IPlayer player) { if (_Relationships.ContainsKey(player)) return _Relationships[player]; return ARelationshipType.None; }

        private void RecalculateIncome() { _Income = 0; foreach (ISettlement settlement in _Settlements) _Income += settlement.Income; }
        private void RecalculateDues() {  _Dues = 0; float temp = 0; foreach (IUnit unit in _Units) temp += unit.ContentTax * unit.Count * 0.01f; _Dues = Convert.ToInt32(temp); } 
        private void RecalculateCoffers() { _Coffers += _Income - _Dues; }

        public override string ToString()
        {
            return _Name + " | Казна: " + _Coffers + " ( " + (_Income - _Dues is int income && income > 0 ? "+" + income : income.ToString()) + " ) | Отряды: " + _Units.Count + " | Территории: " + _Territories.Count + " | Поселения: " + _Settlements.Count;
        }

    }
}
