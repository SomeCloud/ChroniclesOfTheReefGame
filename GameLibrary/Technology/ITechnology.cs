using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Technology
{

    public delegate void OnComplete();

    public interface ITechnology
    {

        public event OnComplete CompleteEvent;

        public IReadOnlyList<ATechnologyType> RequiredTechnologies { get; }

        public int RequiredStudyPoints { get; }
        public int StudyPoints { get; }

        public ATechnologyType TechnologyType { get; }

        public bool IsCompleted { get; }

        public string Name { get; }

        //public void SetRequiredTechnologies(IEnumerable<ITechnology> technologies);
        public void Increase(int count);

    }
}
