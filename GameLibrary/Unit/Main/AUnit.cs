using System;
using System.Collections.Generic;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;
using GameLibrary.Character;
using APeople = GameLibrary.Map.APeople;

namespace GameLibrary.Unit.Main
{
    public abstract class AUnit: IUnit
    {
        private int _Id;
        private IPlayer _Owner;
        private ICharacter _General;

        protected int _Action;

        private string _Name;

        private APoint _Location;
        private APoint _Homeland;
        private List<APeople> _Squad;

        public int Id { get => _Id; }
        public IPlayer Owner { get => _Owner; }
        public ICharacter General { get => _General; }
        public int Count { get => Squad.Sum(x => x.Count); }
        public abstract int Force { get; }
        public int Action { get => _Action; }
        public abstract int ActionMaxValue { get; }

        public abstract int ContentTax { get; }
        public abstract int Cost { get; }

        public string Name { get => _Name; }

        public APoint Location { get => _Location; }
        public APoint Homeland { get => _Homeland; }
        public abstract AUnitType UnitType { get; }
        public abstract string UnitTypeName { get; }
        public IReadOnlyList<APeople> Squad { get => _Squad; }

        public AUnit(int id, IPlayer player, APoint location, List<APeople> commoners, string name) => (_Id, _Owner, _Location, _Name, _Squad, _Homeland, _Action) = (id, player, location, name, commoners, location, ActionMaxValue);

        public void SetName(string name) => _Name = name;

        public void Turn() => _Action = ActionMaxValue;
        public void Attack(int count)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(_Squad.Count);
                if (_Squad[index].Count - 1 < 0) _Squad.RemoveAt(index);
                else _Squad[index].Count--;
            }
            _Action--;
        }
        public abstract bool Act();
        public bool Act(APoint newLocation) { if (_Action - 1 >= 0) { _Action--; _Location = newLocation; return true; } else return false; }
        public void SetOwner(IPlayer player) => _Owner = player;
        public void SetGeneral(ICharacter character) => _General = character;

        public override string ToString()
        {
            return UnitTypeName + " <" + _Name + ">\n" +
                "ID: " + _Id + " / C: " + Count + " / F: " + Force + " / A: " + _Action + "\n" +
                "Владелец: " + _Owner.Name + "\n" +
                "Полководец: " + (General is object ? General.FullName : "отсутствует") + "\n" +
                "Расположение: " + Location;
        }

    }
}
