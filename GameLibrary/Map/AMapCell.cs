using System;
using System.Collections.Generic;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;

using IPlayer = GameLibrary.Player.IPlayer;

using GameLibrary.Unit.Main;
using GameLibrary.Unit;
using GameLibrary.Settlement;
using GameLibrary.Extension;
using GameLibrary.Character;

namespace GameLibrary.Map
{

    public delegate void OnExpansion(AMapCell mapCell);
    [Serializable]
    public class AMapCell: ATerritory
    {

        public event OnExpansion ExpansionEvent;

        private OnAction OnMoveHandler;
        private OnAction OnDestroyHandler;

        private IPlayer _Owner;
        private bool _IsMined;

        //private List<IUnit> _Units;
        private List<AMapCell> _NeighboringCells;

        private APopulation _Population;
        private ISettlement _Settlement;

        private int _Culture;

        private IUnit _ActiveUnit;
        private IUnit _ActiveWorker;

        //public IReadOnlyList<IUnit> Units => _Units;
        public IReadOnlyList<AMapCell> NeighboringCells => _NeighboringCells;

        public APopulation Population => _Population;
        public ISettlement Settlement => _Settlement;

        public int Culture { get => _Culture; }

        public IUnit ActiveUnit => _ActiveUnit;
        public IUnit ActiveWorker => _ActiveWorker;

        public IPlayer Owner => _Owner;
        public int OwnerId => _Owner is object? _Owner.Id: 0;

        public bool IsMined => _IsMined;
        public bool IsResource => !ResourceType.Equals(AResourceType.None);
        //public bool IsUnit => _Units.Count > 0;
        public bool IsSettlement => _Settlement is object && _Settlement.Location.Equals(Location);
        public bool IsOwned => _Owner is object;
        public bool IsEmpty { get => _Owner is null && !IsSettlement; }
        public bool NeighboringCellsIsEmpty { get => _NeighboringCells.Where(x => x.IsEmpty).Count() == _NeighboringCells.Count; }

        public AMapCell(APoint location, ABiomeType biomeType, AResourceType resourceType, IPlayer owner, int count) : base(location, biomeType, resourceType)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            _Population = new APopulation(count);
            _Settlement = null;
            _NeighboringCells = new List<AMapCell>();
            //_Units = new List<IUnit>();
            _Owner = owner;
            //_Culture = random.Next(128, 256);
            _Culture = random.Next(8, 16);

            OnMoveHandler = new OnAction(() =>
            {
                _ActiveWorker.MoveEvent -= OnMoveHandler;
                _IsMined = false;
                _ActiveWorker = null;
            });

            OnDestroyHandler = new OnAction(() =>
            {
                _ActiveWorker.DestroyEvent -= OnDestroyHandler;
                _IsMined = false;
                _ActiveWorker = null;
            });
        }

        public AMapCell(APoint location, ABiomeType biomeType, AResourceType resourceType, int count) : this(location, biomeType, resourceType, null, count) { }
        public AMapCell(APoint location, int count) : this(location, ABiomeType.None, AResourceType.None, null, count) { }
        public AMapCell(APoint location) : this(location, ABiomeType.None, AResourceType.None, null, new Random((int)DateTime.Now.Ticks).Next(128, 256)) { }

        public void AddNeighboringCells(AMapCell cell) { if (!_NeighboringCells.Contains(cell)) _NeighboringCells.Add(cell); }
        public void SetOwner(IPlayer player) => _Owner = player;
        //public void SetUnit(IUnit unit) { if (!_Units.Contains(unit)) _Units.Add(unit); }
        public void SetActiveUnit(IUnit unit) => _ActiveUnit = unit;
        public void SetActiveWorker(IUnit unit)
        {
            _ActiveWorker = unit;
            unit.MoveEvent += OnMoveHandler;
            unit.DestroyEvent += OnDestroyHandler;
            _IsMined = true;
        }
        public void UnsetActiveWorker()
        {
            if (_ActiveWorker is object)
            {
                _ActiveWorker.DestroyEvent -= OnDestroyHandler;
                _ActiveWorker.MoveEvent -= OnMoveHandler;
                _ActiveWorker = null;
                _IsMined = false;
            }
        }
        public void SetSettlement(IPlayer player, int count)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            _Settlement = new ASettlement(this, player, GameExtension.SettlementName[random.Next(GameExtension.SettlementName.Count)]);
            _Settlement.AddTerritory(this);
            _Population.Increase(count);
        }

        public void SetSettlement(ISettlement settlement)
        {
            _Settlement = settlement;
            _Settlement.AddTerritory(this);
            _Owner = settlement.Owner;
        }


        /*public void Turn()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            if (IsOwned)
            {
                if (IsSettlement)
                {
                    _Culture += 1;
                    int count = _Settlement.Culture - 1;
                    while (count > 0)
                    {
                        _Settlement.Territories[random.Next(_Settlement.Territories.Count)]._Culture += 1;
                        count--;
                    }
                }
                //if (IsSettlement) _Culture += _Settlement.Culture;
                //else _Culture += _Settlement.Culture;
            }
            else
            {
                AMapCell mapCell = _NeighboringCells.Where(x => x.IsOwned && x.Culture > _Culture).FirstOrDefault();
                if (mapCell is object)
                {
                    SetOwner(mapCell.Owner);
                    _Settlement = mapCell.Settlement;
                    _Settlement.AddTerritory(this);
                    _Culture = _Culture / 2 + mapCell.Culture / 2;
                    mapCell._Culture /= 2;
                }
                else foreach (AMapCell e in _NeighboringCells.Where(x => x.IsOwned))
                    {
                        int dCulture = random.Next(e.Culture);
                        dCulture = _Culture - dCulture > 0 ? dCulture : _Culture;
                        _Culture -= dCulture;
                        e._Culture -= dCulture;
                    }
            }
        }*/


        public void Turn()
        {
            if (IsOwned)
            {
                if (IsSettlement) _Culture += _Settlement.Culture;
                foreach (AMapCell mapCell in Settlement.Territories.ToList())
                {
                    int dCulture = mapCell.Culture / (mapCell.NeighboringCells.Count + 1);
                    foreach (AMapCell neighbor in mapCell.NeighboringCells)
                    {
                        if ((neighbor.Owner is null || !neighbor.Owner.Equals(mapCell.Owner)) && neighbor.Culture - dCulture <= 0)
                        {
                            if (neighbor.IsSettlement)
                            {
                                ISettlement settlement = neighbor.Settlement;
                                settlement.Owner.RemoveSettlement(settlement);
                                mapCell.Owner.AddSettlement(settlement);
                                //Owner.ExploreTerritories(settlement.Territories.Select(x => x.Location));
                                //Owner.ExploreTerritories(neighbor.NeighboringCells.Select(x => x.Location));
                                Owner.ExploreTerritories(settlement.Territories.SelectMany(x => x.NeighboringCells).Select(x => x.Location));
                                ExpansionEvent?.Invoke(neighbor);
                                //if (!neighbor.ActiveUnit.Owner.IsOpenBorders(Owner)) Deportation(neighbor.ActiveUnit);
                            }
                            else
                            {
                                if (neighbor.Settlement is null)
                                {
                                    neighbor.SetOwner(mapCell.Owner);
                                    _Settlement.AddTerritory(neighbor);
                                    neighbor._Settlement = mapCell.Settlement;
                                    Owner.ExploreTerritories(neighbor.NeighboringCells.Select(x => x.Location));
                                    ExpansionEvent?.Invoke(neighbor);
                                    //if (!neighbor.ActiveUnit.Owner.IsOpenBorders(Owner)) Deportation(neighbor.ActiveUnit);
                                }
                                else
                                {
                                    neighbor.SetOwner(mapCell.Owner);
                                    neighbor.Settlement.RemoveTerritory(neighbor);
                                    _Settlement.AddTerritory(neighbor);
                                    neighbor._Settlement = mapCell.Settlement;
                                    Owner.ExploreTerritories(neighbor.NeighboringCells.Select(x => x.Location));
                                    ExpansionEvent?.Invoke(neighbor);
                                    //if (!neighbor.ActiveUnit.Owner.IsOpenBorders(Owner)) Deportation(neighbor.ActiveUnit);
                                }
                            }
                        }
                        neighbor._Culture += _Settlement.Territories.Contains(neighbor) ? dCulture : neighbor._Culture -dCulture >= 0? - dCulture: -neighbor._Culture;
                        mapCell._Culture -= dCulture;
                    }
                }
            }

            _Population.Turn(IsSettlement ? ((Math.Min(Math.Max(Settlement.Food, -100), 100) + 100f) / 200f) * ((Math.Min(Math.Max(Settlement.Medicine, 100), 100) + 100f) / 200f) : 0.2f);

        }

        public void Deportation(IUnit unit)
        {
            List<AMapCell> cells = _Owner.Territories.SelectMany(x => x.NeighboringCells).Where(x => x.Owner.Equals(unit.Owner) || x.Owner.IsOpenBorders(unit.Owner) || x.Owner is null).ToList();
            if (cells.Count > 0)
            {
                AMapCell mapCell = cells.First();
                unit.Move(mapCell.Location);
                if (mapCell.ActiveUnit is null) mapCell.SetActiveUnit(unit);
                if (_ActiveUnit.Equals(unit)) _ActiveUnit = null;
            }
            else if (unit.Owner.Territories.Select(x => x.Location).Contains(unit.Homeland))
            {
                AMapCell mapCell = unit.Owner.Territories.Where(x => x.Location.Equals(unit.Homeland)).First();
                unit.Move(mapCell.Location);
                if (mapCell.ActiveUnit is null) mapCell.SetActiveUnit(unit);
                if (_ActiveUnit.Equals(unit)) _ActiveUnit = null;
            }
            else
            {
                Population.Add(unit.Squad.ToList());
                unit.Owner.RemoveUnit(unit);
                unit.Dispose();
                unit = null;
            }
        }


    }
}
