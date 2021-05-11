using System;
using System.Collections.Generic;
using System.Linq;

using ASize = CommonPrimitivesLibrary.ASize;
using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Player;
using GameLibrary.Character;
using GameLibrary.Unit;
using GameLibrary.Extension;
using GameLibrary.Settlement;

namespace GameLibraryTester
{
    class ProgramOld
    {


        static void Main(string[] args)
        {

            Dictionary<int, IPlayer> Players = new Dictionary<int, IPlayer>();
            Dictionary<IPlayer, List<AMapCell>> Map = new Dictionary<IPlayer, List<AMapCell>>();
            Dictionary<IPlayer, List<ISettlement>> Settlements = new Dictionary<IPlayer, List<ISettlement>>();

            IPlayer player = null;
            List<AMapCell> territories = new List<AMapCell>() { new AMapCell(new APoint(1, 1)), new AMapCell(new APoint(2, 2)), new AMapCell(new APoint(3, 3)) };
            List<ISettlement> settlements = new List<ISettlement>() { new ASettlement(territories[0], player), new ASettlement(territories[1], player), new ASettlement(territories[2], player) };

            player = new APlayer(1, "Player", null, territories, settlements);

            foreach (ISettlement settlement in settlements) settlement.SetOwner(player);

            foreach (AMapCell mapCell in territories) Console.WriteLine(mapCell.Location);

            player.RemoveTerritory(territories[0]);

            Console.WriteLine("territories: ");
            foreach (AMapCell mapCell in territories) Console.WriteLine(mapCell.Location);
            Console.WriteLine("player.Territories: ");
            foreach (AMapCell mapCell in player.Territories) Console.WriteLine(mapCell.Location);

            Console.WriteLine("player.Settlements: ");
            foreach (ISettlement settlement in player.Settlements) Console.WriteLine(settlement.Owner.Name);
        }

    }
}
