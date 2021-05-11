using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingMoat : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Moat; }
        public override int BuildTime { get => 2; }
        public override bool IsSingle { get => false; }

        public ABuildingMoat()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(2)
              };
        }

    }
}
