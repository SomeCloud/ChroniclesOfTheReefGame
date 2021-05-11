using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingAqueduct : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.Aqueduct; }
        public override int BuildTime { get => 3; }
        public override bool IsSingle { get => false; }

        public ABuildingAqueduct()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicMedicine(6),
                new ASettlementCharacteristicFood(2)
             };
        }

    }
}
