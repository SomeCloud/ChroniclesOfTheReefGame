using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingForge : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Forge; }
        public override int BuildTime { get => 3; }
        public override bool IsSingle { get => false; }

        public ABuildingForge()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(6),
                new ASettlementCharacteristicScience(2),
                new ASettlementCharacteristicMedicine(-6)
              };
        }

    }
}
