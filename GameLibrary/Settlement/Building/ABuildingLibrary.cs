using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingLibrary : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Library; }
        public override int BuildTime { get => 3; }
        public override bool IsSingle { get => false; }

        public ABuildingLibrary()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicScience(12),
                new ASettlementCharacteristicCulture(6),
                new ASettlementCharacteristicReligion(4)
              };
        }

    }
}
