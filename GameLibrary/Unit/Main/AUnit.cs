using System;
using System.Collections.Generic;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;
using GameLibrary.Character;

using AResourceType = GameLibrary.Map.AResourceType;
using APeople = GameLibrary.Map.APeople;

namespace GameLibrary.Unit.Main
{
    [Serializable]
    public abstract class AUnit: IUnit, IDisposable
    {
        public event OnAction MoveEvent;
        public event OnAction DestroyEvent;

        private int _Id;
        private IPlayer _Owner;
        private ICharacter _General;

        protected int _Action;

        private string _Name;

        protected APoint _Location;
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
        public abstract AResourceType RequiredResource { get; }

        public string Name { get => _Name; }

        public APoint Location { get => _Location; }
        public APoint Homeland { get => _Homeland; }
        public abstract AUnitType UnitType { get; }
        public abstract string UnitTypeName { get; }
        public IReadOnlyList<APeople> Squad { get => _Squad; }

        public bool IsGeneral { get => General is object; }

        public AUnit(int id, IPlayer player, APoint location, List<APeople> commoners, string name) => (_Id, _Owner, _Location, _Name, _Squad, _Homeland, _Action) = (id, player, location, name, commoners, location, ActionMaxValue);

        public void SetName(string name) => _Name = name;

        public void Turn() => _Action = ActionMaxValue;
        public void Attack(int count)
        {
            UnderAttack(count);
            _Action--;
        }
        public void UnderAttack(int count)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(_Squad.Count);
                if (_Squad[index].Count - 1 <= 0) _Squad.RemoveAt(index);
                else _Squad[index].Count--;
            }
        }
        public abstract bool Act();
        public bool Act(APoint newLocation) { if (_Action - 1 >= 0 && !Location.Equals(newLocation)) { _Action--; _Location = newLocation; MoveEvent?.Invoke(); return true; } else return false; }
        public void Move(APoint newLocation) => _Location = newLocation;
        public void SetOwner(IPlayer player) => _Owner = player;
        public void SetGeneral(ICharacter character) => _General = character;

        public override string ToString()
        {
            return UnitTypeName + " <" + _Name + ">\n" +
                "ID: " + _Id + " / C: " + Count + " / F: " + Force + " / A: " + _Action + "\n" +
                "Владелец: " + _Owner.Name + "\n" +
                "Полководец: " + (General is object ? General.FullName + "(" + General.MartialSkills + ")" : "отсутствует") + "\n" +
                "Расположение: " + Location;
        }

        public void Dispose()
        {
            DestroyEvent?.Invoke();
            if (_Owner is object) _Owner = null;
            if (_General is object) _General = null;
        }

        ~AUnit()
        {
            Dispose();
        }

    }
}
