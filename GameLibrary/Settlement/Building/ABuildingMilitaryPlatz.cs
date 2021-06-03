using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingMilitaryPlatz : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.MilitaryPlatz; }
        public override int BuildTime { get => 3; }
        public override bool IsSingle { get => false; }

        public ABuildingMilitaryPlatz()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(6),
                new ASettlementCharacteristicIncome(-3),
                new ASettlementCharacteristicScience(1),
                new ASettlementCharacteristicCulture(1),
                new ASettlementCharacteristicReligion(1),
                new ASettlementCharacteristicFood(-2)
              };
        }

    }
}
