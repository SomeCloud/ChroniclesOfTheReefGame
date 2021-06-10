using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GameLibrary.Extension;

using APoint = CommonPrimitivesLibrary.APoint;

namespace GameLibrary.Character
{
    [Serializable]
    public class ACharacter: ICharacter, ICharacterStats
    {

        private string _Name;
        private string _FamilyName;

        private int _Id;
        private int _OwnerId;
        private int _FatherId;
        private int _MotherId;
        private int _SpouseId;

        private bool _Alive;
        private bool _IsMatrilinearMarriage;

        private APoint _Location;

        private int _Attractiveness;
        private int _Education;
        private int _MartialSkills;
        private int _Health;
        private int _Fertility;

        private int DeathDate;

        private List<int> _ChildId;

        public string Name { get => _Name; }
        public string FamilyName { get => _FamilyName; }
        public string FullName { get => _Name + " " + _FamilyName; }

        public ASexType SexType { get; }

        public int Id { get => _Id; }
        public int OwnerId { get => _OwnerId; }
        public int FatherId { get => _FatherId; }
        public int MotherId { get => _MotherId; }
        public int SpouseId { get => _SpouseId; }

        public IReadOnlyList<int> ChildId { get => _ChildId; }

        public bool IsAlive { get => _Alive; }
        public bool IsMarried { get => _SpouseId > 0; }
        public bool IsChild { get => _ChildId.Count > 0; }
        public bool IsOwned { get => _OwnerId > 0; }
        public bool IsMatrilinearMarriage { get => _IsMatrilinearMarriage; }

        public APoint Location { get => _Location; }

        public int BirthDate { get; }

        public int Attractiveness { get => _Attractiveness; }
        public int Education { get => _Education; }
        public int MartialSkills { get => _MartialSkills; }
        public int Health { get => _Health; }
        public int Fertility { get => _Fertility; }

        public ACharacter(string name, string familyName, ASexType sexType, int birthDate, int id, int ownerId, int fatherId, int motherId) {
            _Name = name;
            _FamilyName = familyName;
            SexType = sexType;
            BirthDate = birthDate;
            _Id = id;
            _FatherId = fatherId;
            _MotherId = motherId;
            _OwnerId = ownerId;
            _ChildId = new List<int>();
            _Alive = true;
            _IsMatrilinearMarriage = false;
            SetRandomStats(GameExtension.PlayerDefautStatsValue);
        }

        public ACharacter(string name, string familyName, ASexType sexType, int birthDate, int id, int ownerId): this(name, familyName, sexType, birthDate, id, ownerId, 0, 0) { }

        public int Age(int currentDate) => IsAlive? currentDate - BirthDate: DeathDate - BirthDate;
        public void Kill(int currentDate) => (_Alive, DeathDate) = (false, currentDate);
        public void SetLocation(APoint location) => _Location = location;

        public void Marry(int spouse, bool isMatrilinearMarriage)
        {
            _SpouseId = spouse;
            _IsMatrilinearMarriage = isMatrilinearMarriage;
        }
        public void Divorce()
        {
            _SpouseId = 0;
            _IsMatrilinearMarriage = false;
        }

        public void SetStats(int attractiveness, int education, int martialSkills, int health, int fertility)
        {
            _Attractiveness = attractiveness;
            _Education = education;
            _MartialSkills = martialSkills;
            _Health = health;
            _Fertility = fertility;
        }

        private void SetRandomStats(int value)
        {

            Random random = new Random((int)DateTime.Now.Ticks);

            List<string> stats = typeof(ICharacterStats).GetProperties().Select(x => "_" + x.Name).ToList();

            foreach (string stat in stats)
            {
                GetType().GetField(stat, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, 5);
                value -= 5;
            }

            for (int i = 0; i < value; i++)
            {
                string stat = stats[random.Next(stats.Count)];
                var field = GetType().GetField(stat, BindingFlags.NonPublic | BindingFlags.Instance);
                field.SetValue(this, Convert.ToInt32(field.GetValue(this)) + 1);
            }
        }

        public void AddChild(int childId)
        {
            if (!_ChildId.Contains(childId)) _ChildId.Add(childId);
        }

    }
}
