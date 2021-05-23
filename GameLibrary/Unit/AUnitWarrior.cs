using System.Collections.Generic;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;

using AResourceType = GameLibrary.Map.AResourceType;
using APeople = GameLibrary.Map.APeople;

using GameLibrary.Unit.Main;

namespace GameLibrary.Unit
{
    public class AUnitWarrior : AUnit, IUnit
    {

        public override int Force { get => 3; }
        public override int ActionMaxValue { get => 2; }
        public override int ContentTax { get => 3; }
        public override int Cost { get => 50; }
        public override AResourceType RequiredResource { get => AResourceType.None; }
        public override AUnitType UnitType { get => AUnitType.Warrior; }
        public override string UnitTypeName { get => "Воин"; }

        public AUnitWarrior(int id, IPlayer player, APoint location, List<APeople> commoners, string name) : base(id, player, location, commoners, name) { }

        public override bool Act() { if (_Action - 1 > 0) { _Action--; return true; } else return false; }

    }
}