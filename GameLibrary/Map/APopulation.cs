using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLibrary.Map
{
    public class APopulation
    {

        private Dictionary<int, APeople> _Population;

        public IReadOnlyDictionary<int, APeople> Population => _Population;
        public int Total => _Population.Values.Sum(x => x.Count);

        public APopulation(int count) {
            Random random = new Random((int)DateTime.Now.Ticks);
            _Population = new Dictionary<int, APeople>();
            for (int i = 0; i < count; i++) Add(random.Next(5, 50), 1);
        }

        public void Add(List<APeople> peoples)
        {
            foreach (APeople people in peoples)
                if (_Population.ContainsKey(people.Age)) _Population[people.Age].Count += people.Count;
                else _Population.Add(people.Age, people);
        }

        public void Add(int age, int count)
        {
            if (_Population.ContainsKey(age)) _Population[age].Count += count;
            else _Population.Add(age, new APeople(age, count));
        }

        public void Increase(int count) { }
        public List<APeople> Subtract(int count)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            Dictionary<int, APeople> pepoples = new Dictionary<int, APeople>();
            List<int> ages = _Population.Select(x => x.Key).ToList();
            int age = 0;
            for (int i = 0; i < Math.Min(count, Total); i++)
            {
                age = ages[random.Next(ages.Count)];
                if (age >= 16)
                {
                    if (_Population[age].Count > 0)
                    {
                        if (pepoples.ContainsKey(age)) pepoples[age].Count++;
                        else pepoples.Add(age, new APeople(age, 1));
                        _Population[age].Count--;
                        if (_Population[age].Count <= 0)
                        {
                            _Population.Remove(age);
                            ages.Remove(age);
                        }
                    }
                    else
                    {
                        _Population.Remove(age);
                        ages.Remove(age);
                        i--;
                    }
                }
            }
            return pepoples.Select(x => x.Value).ToList();
        }

    }
}
