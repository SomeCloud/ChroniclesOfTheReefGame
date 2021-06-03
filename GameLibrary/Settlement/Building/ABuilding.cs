using System;
using System.Collections.Generic;

using GameLibrary.Extension;
using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public abstract class ABuilding: IBuilding
    {

        protected List<ISettlementCharacteristic> _Characteristics;

        public string Name { get => GameLocalization.Buildings[BuildingType]; }
        public abstract bool IsSingle { get; }
        public abstract int BuildTime { get; }
        public abstract ABuildingType BuildingType { get; }
        public IReadOnlyList<ISettlementCharacteristic> Characteristics { get => _Characteristics; }

        /*
           
                new ASettlementCharacteristicProtection(1),
                new ASettlementCharacteristicIncome(1),
                new ASettlementCharacteristicScience(1),
                new ASettlementCharacteristicCulture(1),
                new ASettlementCharacteristicMedicine(1),
                new ASettlementCharacteristicReligion(1),
                new ASettlementCharacteristicFood(1)
             
         */

        public ABuilding() { }

    }
}
