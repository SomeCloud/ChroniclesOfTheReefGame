using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Settlement.Building
{
    public interface IBuilding
    {

        public string Name { get; }
        public bool IsSingle { get; }
        public int BuildTime { get; }
        public ABuildingType BuildingType { get; }
        public IReadOnlyList<ISettlementCharacteristic> Characteristics { get; }

    }
}
