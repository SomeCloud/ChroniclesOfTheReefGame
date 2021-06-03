using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingWell : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Well; }
        public override int BuildTime { get => 1; }
        public override bool IsSingle { get => false; }

        public ABuildingWell()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicMedicine(3),
                new ASettlementCharacteristicFood(1)
              };
        }

    }
}
