using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingShoppingQuarter : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.ShoppingQuarter; }
        public override int BuildTime { get => 6; }
        public override bool IsSingle { get => false; }

        public ABuildingShoppingQuarter()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicIncome(8),
                new ASettlementCharacteristicScience(1),
                new ASettlementCharacteristicCulture(2),
                new ASettlementCharacteristicMedicine(-3),
                new ASettlementCharacteristicFood(-4)
              };
        }

    }
}
