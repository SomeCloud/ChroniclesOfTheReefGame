using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingPalace : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Palace; }
        public override int BuildTime { get => 15; }
        public override bool IsSingle { get => false; }

        public ABuildingPalace()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(5),
                new ASettlementCharacteristicIncome(9),
                new ASettlementCharacteristicCulture(14),
                new ASettlementCharacteristicFood(-6)
              };
        }

    }
}
