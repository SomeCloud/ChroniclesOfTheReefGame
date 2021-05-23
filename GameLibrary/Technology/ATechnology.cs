using System;
using System.Collections.Generic;
using System.Linq;

using GameLibrary.Extension;

namespace GameLibrary.Technology
{
    public class ATechnology: ITechnology
    {

        public event OnComplete CompleteEvent;

        //private List<ITechnology> _RequiredTechnologies;

        private int _StudyPoints;
        private bool _IsCompleted;

        public IReadOnlyList<ATechnologyType> RequiredTechnologies => GameExtension.TechnologiesRequiredTechnologies[TechnologyType].ToList();
        public int RequiredStudyPoints => GameExtension.TechnologiesScienceValue[TechnologyType];
        public int StudyPoints  => _StudyPoints;

        public ATechnologyType TechnologyType { get; }

        public bool IsCompleted => _IsCompleted;
        public string Name => GameLocalization.Technologies[TechnologyType];

        public ATechnology(ATechnologyType technologyType)
        {
            _StudyPoints = 0;
            //_RequiredTechnologies = new List<ITechnology>();
            TechnologyType = technologyType;
        }

        /*public void SetRequiredTechnologies(IEnumerable<ITechnology> technologies)
        {
            _RequiredTechnologies = new List<ITechnology>(technologies);
        }*/

        public void Increase(int count)
        {
            if (_StudyPoints + count >= RequiredStudyPoints)
            {
                _StudyPoints = RequiredStudyPoints;
                _IsCompleted = true;
                CompleteEvent?.Invoke();
            }
            else if (count > 0) _StudyPoints += count;
        }

    }
}
