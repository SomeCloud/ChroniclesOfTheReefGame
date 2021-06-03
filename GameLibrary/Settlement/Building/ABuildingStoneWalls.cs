using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingStoneWalls : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.StoneWalls; }
        public override int BuildTime { get => 7; }
        public override bool IsSingle { get => false; }

        public ABuildingStoneWalls()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(10)
              };
        }

    }
}
