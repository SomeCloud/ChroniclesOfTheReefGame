using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingCityWarehouse : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.CityWarehouse; }
        public override int BuildTime { get => 3; }
        public override bool IsSingle { get => true; }

        public ABuildingCityWarehouse()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(3),
                new ASettlementCharacteristicIncome(3)
              };
        }

    }
}
