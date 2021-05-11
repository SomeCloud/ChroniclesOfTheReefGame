using System.Collections.Generic;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;
using GameLibrary.Character;
using APeople = GameLibrary.Map.APeople;

namespace GameLibrary.Unit.Main
{
    public interface IUnit
    {

        public int Id { get; }
        public IPlayer Owner { get; }
        public ICharacter General { get; }
        public int Count { get; }
        public int Force { get; }
        public int Action { get; }
        public int ActionMaxValue { get; }

        public int ContentTax { get; }
        public int Cost { get; }

        public string Name { get; }

        public APoint Location { get; }
        public APoint Homeland { get; }
        public AUnitType UnitType { get; }
        public string UnitTypeName { get; }
        public IReadOnlyList<APeople> Squad { get; }

        public void SetName(string name);

        public void Turn();
        public void Attack(int count);
        public bool Act();
        public bool Act(APoint newLocation);
        public void SetOwner(IPlayer player);
        public void SetGeneral(ICharacter character);

    }
}
