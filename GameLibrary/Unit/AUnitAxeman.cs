using System.Collections.Generic;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;
using APeople = GameLibrary.Map.APeople;
using GameLibrary.Unit.Main;

namespace GameLibrary.Unit
{
    public class AUnitAxeman : AUnit, IUnit
    {

        public override int Force { get => 8; }
        public override int ActionMaxValue { get => 1; }
        public override int ContentTax { get => 8; }
        public override int Cost { get => 100; }
        public override AUnitType UnitType { get => AUnitType.Axeman; }
        public override string UnitTypeName { get => "Секирщик"; }

        public AUnitAxeman(int id, IPlayer player, APoint location, List<APeople> commoners, string name) : base(id, player, location, commoners, name) { }

        public override bool Act() { if (_Action - 1 > 0) { _Action--; return true; } else return false; }

    }
}