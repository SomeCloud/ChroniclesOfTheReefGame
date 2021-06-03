using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingArmory : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Armory; }
        public override int BuildTime { get => 2; }
        public override bool IsSingle { get => true; }

        public ABuildingArmory()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(5)
              };
        }

    }
}
