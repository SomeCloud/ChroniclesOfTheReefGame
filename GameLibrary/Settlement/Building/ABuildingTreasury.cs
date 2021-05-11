using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingTreasury : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Treasury; }
        public override int BuildTime { get => 5; }
        public override bool IsSingle { get => false; }

        public ABuildingTreasury()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(6),
                new ASettlementCharacteristicIncome(8)
              };
        }

    }
}
