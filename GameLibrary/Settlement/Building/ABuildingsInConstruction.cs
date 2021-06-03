using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Settlement.Building
{
    [Serializable]
    public class ABuildingsInConstruction
    {

        private IBuilding _Building;
        private int _TimeToComplete; 

        public IBuilding Building { get => _Building; }
        public int TimeToComplete { get => _TimeToComplete; }

        public ABuildingsInConstruction(IBuilding building)
        {
            _Building = building;
            _TimeToComplete = building.BuildTime;
        }

        public bool ReduceTimeToComplete()
        {
            if (_TimeToComplete - 1 <= 0)
            {
                _TimeToComplete--;
                return true;
            }
            else _TimeToComplete--;
            return false;
        }

    }
}
