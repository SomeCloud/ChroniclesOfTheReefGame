using System;
using System.Collections.Generic;
using System.Linq;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary.Player;

namespace GameLibrary.Map
{
    [Serializable]
    public class AMap
    {

        private ASize _Size;
        public Dictionary<APoint, AMapCell> Map;

        public ASize Size => _Size;
        public int Height => _Size.Height;
        public int Width => _Size.Width;
        public int Length => Height * Width - (Height + 1) / 2; 

        public bool IsContains(APoint point) => Map.ContainsKey(point);

        public AMapCell this[APoint point] => IsContains(point) ? Map[point] : null;

        public AMap()
        {
            Map = new Dictionary<APoint, AMapCell>();
            _Size = new ASize(0, 0);
        }

        public void RandomGeneration(ASize size)
        {

            Random random = new Random((int)DateTime.Now.Ticks);

            Map.Clear();
            _Size = size;

            List<ABiomeType> allBiomes = Enum.GetValues(typeof(ABiomeType)).OfType<ABiomeType>().Where(x => !x.Equals(ABiomeType.None)).ToList();
            List<AResourceType> allResources = Enum.GetValues(typeof(AResourceType)).OfType<AResourceType>().ToList();

            for (int y = 0; y < size.Height; y++)
                for (int x = 0; x < (y % 2 == 0 ? size.Width - 1 : size.Width); x++)
                {
                    APoint location = new APoint(x, y);
                    AMapCell mapCell = new AMapCell(location, allBiomes[random.Next(allBiomes.Count)], allResources[random.Next(allResources.Count)], null, random.Next(128, 256));
                    Map.Add(location, mapCell);
                }

            foreach (APoint location in Map.Keys) SetNeighbors(location);
        }

        public Dictionary<int, APoint> SetPlayers(List<IPlayer> players)
        {
            Dictionary<int, APoint> Settlements = new Dictionary<int, APoint>();
            Random random = new Random((int)DateTime.Now.Ticks);
            int count = 0;
            foreach (IPlayer player in players)
            {
                AMapCell cell = Map.Values.ToList()[random.Next(Map.Count)];
                while (!cell.IsEmpty || !cell.NeighboringCellsIsEmpty || cell.BiomeType == ABiomeType.Sea)
                {
                    cell = Map.Values.ToList()[random.Next(Map.Count)];
                }
                cell.SetOwner(player);
                cell.SetSettlement(player, random.Next(128, 256));
                Settlements.Add(player.Id, cell.Location);
                count++;
            }
            return Settlements;
        }

        private void SetNeighbors(APoint location)
        {
            // смещения для четной точки
            List<APoint> EvenPoints = new List<APoint>() { new APoint(0, -2), new APoint(0, -1), new APoint(0, 1), new APoint(1, -1), new APoint(1, 1), new APoint(0, 2) };
            // смещения для нечетной точки
            List<APoint> OddPoints = new List<APoint>() { new APoint(0, -2), new APoint(-1, -1), new APoint(0, -1), new APoint(-1, 1), new APoint(0, 1), new APoint(0, 2) };

            foreach (APoint point in location.Y % 2 == 0 ? EvenPoints : OddPoints)
            {
                if (location + point is APoint offset && Map.ContainsKey(offset)) Map[location].AddNeighboringCells(Map[offset]);
            }
        }

    }
}
