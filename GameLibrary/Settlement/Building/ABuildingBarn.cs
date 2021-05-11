using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingBarn : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Barn; }
        public override int BuildTime { get => 2; }
        public override bool IsSingle { get => false; }

        public ABuildingBarn()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(1),
                new ASettlementCharacteristicIncome(2),
                new ASettlementCharacteristicMedicine(2),
                new ASettlementCharacteristicFood(3)
              };
        }

    }
}
