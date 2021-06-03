using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingMill : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Mill; }
        public override int BuildTime { get => 3; }
        public override bool IsSingle { get => false; }

        public ABuildingMill()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(3),
                new ASettlementCharacteristicFood(3)
              };
        }

    }
}
