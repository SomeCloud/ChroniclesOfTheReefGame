using System.Collections.Generic;

using APoint = CommonPrimitivesLibrary.APoint;

using GameLibrary.Map;
using GameLibrary.Unit.Main;
using GameLibrary.Settlement;
using GameLibrary.Character;
using GameLibrary.Technology;
using GameLibrary.Message;

namespace GameLibrary.Player
{
    public interface IPlayer
    {

        public int Id { get; }
        public string Name { get; }

        public int Income { get; }
        public int Dues { get; }
        public int Coffers { get; }

        public bool Status { get; }

        public ICharacter Ruler { get; }
        public ICharacter Heir { get; }

        public IReadOnlyList<AMapCell> Territories { get; }
        public IReadOnlyList<APoint> ExploredTerritories { get; }
        public IReadOnlyList<ISettlement> Settlements { get; }
        public IReadOnlyList<IUnit> Units { get; }
        public ATechnologyTree Technologies { get; }

        public IReadOnlyList<IMessage> Messages { get; }

        public IReadOnlyDictionary<AResourceType, int> Resources { get; }

        public IReadOnlyDictionary<IPlayer, ARelationshipType> Relationships { get; }

        public IReadOnlyList<ICharacter> Characters { get; }

        public void SetStatus(bool status);

        public void SetName(string name);
        public void SetRuler(ICharacter character);
        public void SetHeir(ICharacter character);
        public void SetRelationship(IPlayer player, ARelationshipType relationship);

        public void UpdateRulerByHeir(int currentTurn);

        public void RemoveMessage(IMessage message);
        public void SendMessage(IMessage message);

        public void AddTerritory(AMapCell territory);
        public void AddSettlement(ISettlement settlement);
        public void AddUnit(IUnit unit);
        public void AddCharacter(ICharacter character);

        public void RemoveTerritory(AMapCell territory);
        public void RemoveSettlement(ISettlement settlement);
        public void RemoveUnit(IUnit unit);
        public void RemoveCharacter(ICharacter character);
        public void ExploreTerritories(IEnumerable<APoint> territories);

        public bool IsResource(AResourceType resourceType);

        public void Turn();

        public bool IsTerritory(AMapCell territory);
        public bool IsSettlement(ISettlement settlement);
        public bool IsUnit(IUnit unit);
        public bool IsCharacter(ICharacter character);
        public bool IsOpenBorders(IPlayer player);

        public ARelationshipType Relationship(IPlayer player);

        public bool ChangeCoffers(int count);
    }
}
