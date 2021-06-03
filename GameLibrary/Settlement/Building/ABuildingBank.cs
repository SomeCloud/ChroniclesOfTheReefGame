using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingBank : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Bank; }
        public override int BuildTime { get => 7; }
        public override bool IsSingle { get => false; }

        public ABuildingBank()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(1),
                new ASettlementCharacteristicIncome(12),
                new ASettlementCharacteristicScience(1)
              };
        }

    }
}
