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

        public int SubtractAll(int age)
        {
            int count = _Population[age].Count;
            _Population[age].Count = 0;
            return count;
        }

        private static bool Probability(float value) => value > new Random((int)DateTime.Now.Ticks).NextDouble();

        /*public void Turn(int food, int medicine)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            int parents = _Population.Where(x => x.Key > 16 && x.Key < 45).Sum(x => x.Value.Count);
            int childs = parents > 0? random.Next(parents / 2 * food): 0;
            for (int i = 0; i < childs; i++)
            {
                if (Probability(_Population.Count / medicine))
                    if (_Population.ContainsKey(0)) _Population[0].Count += 1;
                    else _Population.Add(0, 0);
            }
        }*/

        public void Turn(float dConditions)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            _Population = _Population.OrderByDescending(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);

            foreach (APeople people in new List<APeople>(_Population.Values))
            {
                Add(people.Age + 1, SubtractAll(people.Age));
            }

            foreach (APeople people in new List<APeople>(_Population.Values)) 
                if (_Population[people.Age].Count <= 0) _Population.Remove(people.Age);

            //SummonDeath(dConditions);
            //SummonBirth(dConditions);

            for (int i = 0; i < _Population.Where(x => x.Key > 16 && x.Key < 45).Sum(x => x.Value.Count) / 2; i++)
                if (Probability(dConditions)) Add(1, random.Next(1, 3));

            int elder = _Population.Max(x => x.Key);

            foreach (APeople people in new List<APeople>(_Population.Values))
            {           
                if (Probability((1 - dConditions) * (people.Age / (float)elder)) || people.Age > 55)
                {
                    int count = people.Count > 1 ? random.Next(people.Count / 4, people.Count / 2) : people.Count;
                    if (people.Count - count > 0) people.Count -= count;
                    else _Population.Remove(people.Age);
                }
            }

        }

        /*public void SummonDeath(float deathRate)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 1; i < _Population.Count; i++)
            {
                if (random.Next(0, _Population.Count - i) == 0 || i > 55) 
                    if (i < 55) _Population[i].Count -= Convert.ToInt32(_Population[i].Count * deathRate) is int dt & dt <= 100 ? dt >= 0 ? dt : 0 : 100;
                    else if (_Population[i].Count > 0) _Population[i].Count -= _Population[i].Count > 1 ? random.Next(1, Convert.ToInt32(_Population[i].Count)) : 1;
            }
        }*/

        public void SummonDeath(float deathRate)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            foreach (APeople people in _Population.Select(x => x.Value))
            {
                if (random.Next(0, _Population.Max(x => x.Key) - people.Age) == 0 || people.Age > 55)
                    if (people.Age < 55) _Population[people.Age].Count -= Convert.ToInt32(_Population[people.Age].Count * deathRate) is int dt & dt <= 100 ? dt >= 0 ? dt : 0 : 100;
                    else if (people.Count > 0) _Population[people.Age].Count -= people.Count > 1 ? random.Next(1, people.Count) : 1;
            }
        }

        public void SummonBirth(float fertilityRate)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            foreach (APeople people in new List<APeople>(_Population.Where(x => x.Key > 15 && x.Key < 45).Select(x => x.Value)))
            {
                if (people.Count > 0 && random.Next(0, Convert.ToInt32(fertilityRate * 100)) == 0) Add(1, random.Next(Convert.ToInt32(people.Count / 2 * fertilityRate)));
            }
        }
    }
}
