using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingResidentialQuarter : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.ResidentialQuarter; }
        public override int BuildTime { get => 6; }
        public override bool IsSingle { get => false; }

        public ABuildingResidentialQuarter()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(6),
                new ASettlementCharacteristicScience(1),
                new ASettlementCharacteristicCulture(1),
                new ASettlementCharacteristicMedicine(-3),
                new ASettlementCharacteristicReligion(2),
                new ASettlementCharacteristicFood(-4)
              };
        }

    }
}
