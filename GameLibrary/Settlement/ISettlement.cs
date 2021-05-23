using System.Collections.Generic;

using APoint = CommonPrimitivesLibrary.APoint;

using IPlayer = GameLibrary.Player.IPlayer;

using AMapCell = GameLibrary.Map.AMapCell;
using AResourceType = GameLibrary.Map.AResourceType;

using IBuilding = GameLibrary.Settlement.Building.IBuilding;
using ABuildingsInConstruction = GameLibrary.Settlement.Building.ABuildingsInConstruction;
using ASettlementCharacteristicType = GameLibrary.Settlement.Characteristic.ASettlementCharacteristicType;
using ISettlementCharacteristic = GameLibrary.Settlement.Characteristic.ISettlementCharacteristic;

using ITechnology = GameLibrary.Technology.ITechnology;

namespace GameLibrary.Settlement
{
    public interface ISettlement
    {
        public APoint Location { get; }
        public string Name { get; }

        public IPlayer Owner { get; }
        public IPlayer Ruler { get; }

        public IReadOnlyList<IBuilding> Buildings { get; }
        public IReadOnlyList<ABuildingsInConstruction> BuildingsInConstruction { get; }
        public IReadOnlyDictionary<AResourceType, int> Resources { get; }
        public IReadOnlyList<AMapCell> Territories { get; }

        public ITechnology InvestigatedTechnology { get; }

        public int Protection { get; }
        public int Income { get; }
        public int Science { get; }
        public int Culture { get; }
        public int Medicine { get; }
        public int Religion { get; }
        public int Food { get; }

        public void SetName(string name);
        public void SetOwner(IPlayer player);
        public void SetRuler(IPlayer player);
        public void UpdateBuilding();
        public void AddBuilding(IBuilding building);
        public void StartBuilding(IBuilding building);
        public void RemoveBuilding(IBuilding building);
        public void AddTerritory(AMapCell territory);
        public void RemoveTerritory(AMapCell territory);

        public void SetInvestigatedTechnology(ITechnology technology);
        public void Turn();

    }
}
