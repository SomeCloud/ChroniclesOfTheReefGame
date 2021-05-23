using System;
using System.Collections.Generic;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;
using GameLibrary.Character;

using AResourceType = GameLibrary.Map.AResourceType;
using APeople = GameLibrary.Map.APeople;

namespace GameLibrary.Unit.Main
{

    public delegate void OnAction();

    public interface IUnit: IDisposable
    {

        public event OnAction MoveEvent;
        public event OnAction DestroyEvent;

        public int Id { get; }
        public IPlayer Owner { get; }
        public ICharacter General { get; }
        public int Count { get; }
        public int Force { get; }
        public int Action { get; }
        public int ActionMaxValue { get; }

        public int ContentTax { get; }
        public int Cost { get; }
        public AResourceType RequiredResource { get; }

        public string Name { get; }

        public APoint Location { get; }
        public APoint Homeland { get; }
        public AUnitType UnitType { get; }
        public string UnitTypeName { get; }
        public IReadOnlyList<APeople> Squad { get; }

        public bool IsGeneral { get; }

        public void SetName(string name);

        public void Turn();
        public void Attack(int count);
        public void UnderAttack(int count);
        public bool Act();
        public bool Act(APoint newLocation);
        public void Move(APoint newLocation);
        public void SetOwner(IPlayer player);
        public void SetGeneral(ICharacter character);

    }
}
