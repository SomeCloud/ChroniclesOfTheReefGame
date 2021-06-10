using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Character
{
    public interface ICharacterStats
    {
        public int Attractiveness { get; }
        public int Education { get; }
        public int MartialSkills { get; }
        public int Health { get; }
        public int Fertility { get; }
        public void SetStats(int attractiveness, int education, int martialSkills, int health, int fertility);
    }
}
