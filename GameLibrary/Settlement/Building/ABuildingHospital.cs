using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingHospital : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Hospital; }
        public override int BuildTime { get => 5; }
        public override bool IsSingle { get => false; }

        public ABuildingHospital()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(1),
                new ASettlementCharacteristicIncome(-2),
                new ASettlementCharacteristicScience(4),
                new ASettlementCharacteristicMedicine(12),
                new ASettlementCharacteristicFood(-2)
              };
        }

    }
}
