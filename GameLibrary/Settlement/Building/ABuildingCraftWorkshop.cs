using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingCraftWorkshop : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.CraftWorkshop; }
        public override int BuildTime { get => 3; }
        public override bool IsSingle { get => false; }

        public ABuildingCraftWorkshop()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(4),
                new ASettlementCharacteristicScience(4),
                new ASettlementCharacteristicCulture(1),
                new ASettlementCharacteristicMedicine(-3),
              };
        }

    }
}
