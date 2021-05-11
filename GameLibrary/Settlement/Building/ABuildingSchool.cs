using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingSchool : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.School; }
        public override int BuildTime { get => 2; }
        public override bool IsSingle { get => false; }

        public ABuildingSchool()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(-2),
                new ASettlementCharacteristicScience(8),
                new ASettlementCharacteristicCulture(6),
                new ASettlementCharacteristicReligion(1),
                new ASettlementCharacteristicFood(-3)
              };
        }

    }
}
