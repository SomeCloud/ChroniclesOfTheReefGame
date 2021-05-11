using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingTheatre : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Theatre; }
        public override int BuildTime { get => 4; }
        public override bool IsSingle { get => false; }

        public ABuildingTheatre()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(4),
                new ASettlementCharacteristicCulture(12),
                new ASettlementCharacteristicReligion(2)
              };
        }

    }
}
