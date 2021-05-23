using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;

using IPlayer = GameLibrary.Player.IPlayer;

using AMapCell = GameLibrary.Map.AMapCell;
using AResourceType = GameLibrary.Map.AResourceType;

using GameLibrary.Extension;

using GameLibrary.Settlement.Building;
using ASettlementCharacteristicType = GameLibrary.Settlement.Characteristic.ASettlementCharacteristicType;
using ISettlementCharacteristic = GameLibrary.Settlement.Characteristic.ISettlementCharacteristic;

using ITechnology = GameLibrary.Technology.ITechnology;

namespace GameLibrary.Settlement
{
    public class ASettlement : ISettlement
    {

        private string _Name;

        private IPlayer _Owner;
        private IPlayer _Ruler;

        private List<IBuilding> _Buildings { get; }
        private List<ABuildingsInConstruction> _BuildingsInConstruction { get; }
        public IReadOnlyDictionary<AResourceType, int> Resources
        {
            get
            {
                List<AResourceType> resources = Territories.Where(x => x.IsMined).Select(x => x.ResourceType).ToList();
                Dictionary<AResourceType, int> result = new Dictionary<AResourceType, int>();
                foreach (AResourceType resource in resources)
                {
                    if (result.ContainsKey(resource)) result[resource]++;
                    else result.Add(resource, 1);
                }
                return result;
            }
        }
        private List<AMapCell> _Territories;

        private ITechnology _InvestigatedTechnology;

        private int _Protection;
        private int _Income;
        private int _Science;
        private int _Culture;
        private int _Medicine;
        private int _Religion;
        private int _Food;

        public APoint Location { get; }
        public string Name { get => _Name; }

        public IPlayer Owner { get => _Owner; }
        public IPlayer Ruler { get => _Ruler; }

        public IReadOnlyList<IBuilding> Buildings { get => _Buildings; }
        public IReadOnlyList<ABuildingsInConstruction> BuildingsInConstruction { get => _BuildingsInConstruction; }
        public IReadOnlyList<AMapCell> Territories { get => _Territories; }

        public ITechnology InvestigatedTechnology => _InvestigatedTechnology;

        public int Protection { get => _Protection; }
        public int Income { get => _Income; }
        public int Science { get => _Science; }
        public int Culture { get => _Culture; }
        public int Medicine { get => _Medicine; }
        public int Religion { get => _Religion; }
        public int Food { get => _Food; }

        public bool IsOwned { get => _Owner is object; }

        public ASettlement(AMapCell mapCell, IPlayer owner) : this(mapCell, owner, "New Settlement") { }

        public ASettlement(AMapCell mapCell, IPlayer owner, string name)
        {

            _Buildings = new List<IBuilding>() { new ABuildingTownHall(), new ABuildingResidentialQuarter(), new ABuildingGarden(), new ABuildingWell() };
            _BuildingsInConstruction = new List<ABuildingsInConstruction>();
            _Territories = new List<AMapCell>();

            Location = mapCell.Location;
            _Owner = _Ruler = owner;
            _Name = name;

            _Protection = 0;

            RecalculateCharacteristics();

        }

        public void SetName(string name) => _Name = name;
        public void SetOwner(IPlayer player) => _Owner = player;
        public void SetRuler(IPlayer player) => _Ruler = player;

        public void UpdateBuilding()
        {
            for (int i = _BuildingsInConstruction.Count - 1; i >= 0; i--)
            {
                ABuildingsInConstruction e = _BuildingsInConstruction[i];
                if (e.ReduceTimeToComplete())
                {
                    AddBuilding(e.Building);
                    _BuildingsInConstruction.Remove(e);
                }
            }
        }

        public void AddBuilding(IBuilding building)
        {
            _Buildings.Add(building);
            RecalculateCharacteristics();
        }

        public void StartBuilding(IBuilding building)
        {
            if (_Owner.ChangeCoffers(-GameExtension.BuildCost[building.BuildingType])) _BuildingsInConstruction.Add(new ABuildingsInConstruction(building));
        }

        public void RemoveBuilding(IBuilding building)
        {
            if (_Buildings.Contains(building))
            {
                _Buildings.Remove(building);
                RecalculateCharacteristics();
            }
        }

        public void AddTerritory(AMapCell territory)
        {
            _Territories.Add(territory);
            if (!_Owner.Territories.Contains(territory)) Owner.AddTerritory(territory);
        }

        public void RemoveTerritory(AMapCell territory)
        {
            if (_Territories.Contains(territory)) _Territories.Remove(territory);
            if (_Owner.Territories.Contains(territory)) Owner.RemoveTerritory(territory);
        }

        private void RecalculateCharacteristics()
        {
            foreach (var e in Enum.GetValues(typeof(ASettlementCharacteristicType)))
            {
                GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "_" + e.ToString()).First().SetValue(this, 0);
            }

            foreach (IBuilding building in _Buildings) foreach (ISettlementCharacteristic characteristic in building.Characteristics)
                {
                    string Property = "_" + characteristic.SettlementCharacteristicType.ToString();
                    FieldInfo fieldInfo = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == Property).First();
                    fieldInfo.SetValue(this, Convert.ToInt32(fieldInfo.GetValue(this)) + characteristic.Value);
                }
        }

        public void SetInvestigatedTechnology(ITechnology technology) => _InvestigatedTechnology = technology;
        public void Turn()
        {
            UpdateBuilding();
            if (_InvestigatedTechnology is object)
            {
                _InvestigatedTechnology.Increase(_Science);
                if (_InvestigatedTechnology.IsCompleted) _InvestigatedTechnology = null;
            }
        }
    }
}
