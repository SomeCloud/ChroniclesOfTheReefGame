using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingPalisade : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Palisade; }
        public override int BuildTime { get => 4; }
        public override bool IsSingle { get => false; }

        public ABuildingPalisade()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(6)
              };
        }

    }
}
