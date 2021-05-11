using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingMonastery : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Monastery; }
        public override int BuildTime { get => 8; }
        public override bool IsSingle { get => false; }

        public ABuildingMonastery()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(3),
                new ASettlementCharacteristicIncome(6),
                new ASettlementCharacteristicScience(-2),
                new ASettlementCharacteristicCulture(4),
                new ASettlementCharacteristicMedicine(6),
                new ASettlementCharacteristicReligion(12),
                new ASettlementCharacteristicFood(-4)
              };
        }

    }
}
