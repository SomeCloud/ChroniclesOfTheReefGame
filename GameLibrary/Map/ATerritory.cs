using System;
using System.Collections.Generic;
using System.Text;

using APoint = CommonPrimitivesLibrary.APoint;

namespace GameLibrary.Map
{
    public class ATerritory
    {
        protected ABiomeType _BiomeType;
        protected AResourceType _ResourceType;

        public APoint Location { get; }
        public ABiomeType BiomeType { get => _BiomeType; }
        public AResourceType ResourceType { get => _ResourceType; }

        public ATerritory(APoint location, ABiomeType biomeType, AResourceType resourceType) => (Location, _BiomeType, _ResourceType) = (location, biomeType, resourceType);

    }
}
