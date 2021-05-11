using System.Collections.Generic;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;
using APeople = GameLibrary.Map.APeople;
using GameLibrary.Unit.Main;

namespace GameLibrary.Unit
{
    public class AUnitSpearman : AUnit, IUnit
    {

        public override int Force { get => 5; }
        public override int ActionMaxValue { get => 2; }
        public override int ContentTax { get => 5; }
        public override int Cost { get => 80; }
        public override AUnitType UnitType { get => AUnitType.Spearman; }
        public override string UnitTypeName { get => "Копейщик"; }

        public AUnitSpearman(int id, IPlayer player, APoint location, List<APeople> commoners, string name) : base(id, player, location, commoners, name) { }

        public override bool Act() { if (_Action - 1 > 0) { _Action--; return true; } else return false; }

    }
}
