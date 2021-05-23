using System;
using System.Collections.Generic;
using System.Text;

using APoint = CommonPrimitivesLibrary.APoint;

namespace GameLibrary.Character
{
    public interface ICharacter: ICharacterStats
    {

        public string Name { get; }
        public string FamilyName { get; }
        public string FullName { get; }

        public ASexType SexType { get; }

        public int Id { get; }
        public int OwnerId { get; }
        public int FatherId { get; }
        public int MotherId { get; }
        public int SpouseId { get; }

        public IReadOnlyList<int> ChildId { get; }

        public bool IsAlive { get; }
        public bool IsMarried { get; }
        public bool IsChild { get; }
        public bool IsOwned { get; }
        public bool IsMatrilinearMarriage { get; }

        public int BirthDate { get; }

        public APoint Location { get; }

        public int Age(int currentDate);
        public void Kill(int currentDate);
        public void SetLocation(APoint location);

        public void Marry(int spouse, bool isMatrilinearMarriage);

        public void AddChild(int childId);

    }
}
