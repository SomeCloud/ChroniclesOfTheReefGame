using System.Collections.Generic;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;

using AResourceType = GameLibrary.Map.AResourceType;
using APeople = GameLibrary.Map.APeople;

using GameLibrary.Unit.Main;

namespace GameLibrary.Unit
{
    public class AUnitMissionary : AUnit, IUnit
    {

        public override int Force { get => 0; }
        public override int ActionMaxValue { get => 3; }
        public override int ContentTax { get => 5; }
        public override int Cost { get => 80; }
        public override AResourceType RequiredResource { get => AResourceType.None; }
        public override AUnitType UnitType { get => AUnitType.Missionary; }
        public override string UnitTypeName { get => "Миссионер"; }

        public AUnitMissionary(int id, IPlayer player, APoint location, List<APeople> commoners, string name) : base(id, player, location, commoners, name) { }

        public override bool Act() { if (_Action - 1 > 0) { _Action--; return true; } else return false; }

    }
}