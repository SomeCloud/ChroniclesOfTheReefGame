using System;
using System.Collections.Generic;
using System.Text;

namespace NetLibrary
{
    [Serializable]
    public enum ADataType
    {
        None,
        Turn,
        SelectUnit,
        SelectTechnology,
        War,
        Peace,
        Union,
        BreakUnion,
        BuildingCreate,
        UnitCreate,
        RenameUnit,
        DestroyUnit,
        GeneralUnit,
        WorkUnit,
        EstablishUnit,
        MapCellSelect,
        MoveUnit,
    }

    [Serializable]
    public class AData
    {

        public object Package { get; }
        public RPlayer Player { get; }
        public ADataType PackageType { get; }

        public AData(object package, RPlayer player, ADataType packageType) => (Package, Player, PackageType) = (package, player, packageType);

    }
}
