using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingFarm : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Farm; }
        public override int BuildTime { get => 5; }
        public override bool IsSingle { get => false; }

        public ABuildingFarm()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(5),
                new ASettlementCharacteristicMedicine(-2),
                new ASettlementCharacteristicFood(8)
              };
        }

    }
}
