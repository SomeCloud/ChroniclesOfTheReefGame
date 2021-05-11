using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingGarden : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Garden; }
        public override int BuildTime { get => 6; }
        public override bool IsSingle { get => false; }

        public ABuildingGarden()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicMedicine(3),
                new ASettlementCharacteristicFood(6)
              };
        }

    }
}
