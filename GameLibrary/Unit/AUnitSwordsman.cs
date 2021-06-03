using System.Collections.Generic;
using System;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Player;

using AResourceType = GameLibrary.Map.AResourceType;
using APeople = GameLibrary.Map.APeople;

using GameLibrary.Unit.Main;

namespace GameLibrary.Unit
{
    [Serializable]
    public class AUnitSwordsman : AUnit, IUnit
    {

        public override int Force { get => 10; }
        public override int ActionMaxValue { get => 1; }
        public override int ContentTax { get => 10; }
        public override int Cost { get => 120; }
        public override AResourceType RequiredResource { get => AResourceType.Iron; }
        public override AUnitType UnitType { get => AUnitType.Swordsman; }
        public override string UnitTypeName { get => "Мечник"; }

        public AUnitSwordsman(int id, IPlayer player, APoint location, List<APeople> commoners, string name) : base(id, player, location, commoners, name) { }

        public override bool Act() { if (_Action - 1 > 0) { _Action--; return true; } else return false; }

    }
}