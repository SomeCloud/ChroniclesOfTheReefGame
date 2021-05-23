using System;
using System.Collections.Generic;
using System.Linq;

using AResourceType = GameLibrary.Map.AResourceType;

namespace GameLibrary.Settlement
{
    public class AStorage
    {

        private Dictionary<AResourceType, int> _Storage;

        public IReadOnlyDictionary<AResourceType, int> Total => _Storage;

        public AStorage()
        {
            _Storage = new Dictionary<AResourceType, int>();
            Enum.GetValues(typeof(AResourceType)).Cast<AResourceType>().Where(x => !x.Equals(AResourceType.None)).ToList().ForEach(x => _Storage.Add(x, 0));
        }

        public void Add(AResourceType resourceType, int count) => _Storage[resourceType] += count;
        public bool Remove(AResourceType resourceType, int count)
        {
            if (_Storage[resourceType] >= count)
            {
                _Storage[resourceType] -= count;
                return true;
            }
            return false;
        }

    }
}
