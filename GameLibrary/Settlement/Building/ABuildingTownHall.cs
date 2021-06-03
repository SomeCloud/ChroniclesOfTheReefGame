using System;
using System.Collections.Generic;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingTownHall : ABuilding, IBuilding
    {
        public override ABuildingType BuildingType { get => ABuildingType.TownHall; }
        public override int BuildTime { get => 0; }
        public override bool IsSingle { get => true; }

        public ABuildingTownHall()
        {
            _Characteristics = new List<ISettlementCharacteristic>() { 
                new ASettlementCharacteristicIncome(3), 
                new ASettlementCharacteristicCulture(3), 
                new ASettlementCharacteristicProtection(1),
                new ASettlementCharacteristicScience(2)
            };
        }

    }
}
