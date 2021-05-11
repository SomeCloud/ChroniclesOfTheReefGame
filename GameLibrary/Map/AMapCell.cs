using System;
using System.Collections.Generic;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;

using IPlayer = GameLibrary.Player.IPlayer;

using GameLibrary.Unit.Main;
using GameLibrary.Settlement;
using GameLibrary.Extension;
using GameLibrary.Character;

namespace GameLibrary.Map
{
    public class AMapCell: ATerritory
    {

        private IPlayer _Owner;
        private bool _IsMined;

        private List<IUnit> _Units;
        private List<AMapCell> _NeighboringCells;

        private APopulation _Population;
        private ASettlement _Settlement;

        private IUnit _ActiveUnit;

        //public IReadOnlyList<IUnit> Units => _Units;
        public IReadOnlyList<AMapCell> NeighboringCells => _NeighboringCells;

        public APopulation Population => _Population;
        public ASettlement Settlement => _Settlement;

        public IUnit ActiveUnit => _ActiveUnit;

        public IPlayer Owner => _Owner;
        public int OwnerId => _Owner is object? _Owner.Id: 0;

        public bool IsMined => _IsMined;
        public bool IsResource => !ResourceType.Equals(AResourceType.None);
        public bool IsUnit => _Units.Count > 0;
        public bool IsSettlement => _Settlement is object;
        public bool IsOwned => _Owner is object;
        public bool IsEmpty { get => _Owner is null && !IsSettlement; }
        public bool NeighboringCellsIsEmpty { get => _NeighboringCells.Where(x => x.IsEmpty).Count() == _NeighboringCells.Count; }

        public AMapCell(APoint location, ABiomeType biomeType, AResourceType resourceType, IPlayer owner, int count) : base(location, biomeType, resourceType)
        {
            _Population = new APopulation(count);
            _Settlement = null;
            _NeighboringCells = new List<AMapCell>();
            _Units = new List<IUnit>();
            _Owner = owner;
        }

        public AMapCell(APoint location, ABiomeType biomeType, AResourceType resourceType, int count) : this(location, biomeType, resourceType, null, count) { }
        public AMapCell(APoint location, int count) : this(location, ABiomeType.None, AResourceType.None, null, count) { }
        public AMapCell(APoint location) : this(location, ABiomeType.None, AResourceType.None, null, new Random((int)DateTime.Now.Ticks).Next(128, 256)) { }

        public void AddNeighboringCells(AMapCell cell) { if (!_NeighboringCells.Contains(cell)) _NeighboringCells.Add(cell); }
        public void SetOwner(IPlayer player) => _Owner = player;
        public void SetUnit(IUnit unit) { if (!_Units.Contains(unit)) _Units.Add(unit); }
        public void SetActiveUnit(IUnit unit) => _ActiveUnit = unit;
        public void SetSettlement(IPlayer player, int count)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            _Settlement = new ASettlement(this, player, GameExtension.SettlementName[random.Next(GameExtension.SettlementName.Count)]);
            _Settlement.AddTerritory(this);
            _Population.Increase(count);
        }
    }
}
