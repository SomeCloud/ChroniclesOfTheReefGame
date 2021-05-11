using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public class ABuildingEarthenShaft : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.EarthenShaft; }
        public override int BuildTime { get => 3; }
        public override bool IsSingle { get => false; }

        public ABuildingEarthenShaft()
        {
            _Characteristics = new List<ISettlementCharacteristic>() {
                new ASettlementCharacteristicProtection(4)
               };
        }

    }
}
