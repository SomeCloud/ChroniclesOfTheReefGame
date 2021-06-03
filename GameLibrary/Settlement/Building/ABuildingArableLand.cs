using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingArableLand : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.ArableLand; }
        public override int BuildTime { get => 2; }
        public override bool IsSingle { get => false; }

        public ABuildingArableLand()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(1),
                new ASettlementCharacteristicMedicine(-1),
                new ASettlementCharacteristicFood(3)
              };
        }

    }
}
