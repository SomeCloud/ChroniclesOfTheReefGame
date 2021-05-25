using System;
using System.Collections.Generic;
using System.Linq;

using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Character;
using GameLibrary.Extension;

namespace NetLibrary
{
    [Serializable]
    public class ARoom
    {

        private int _Id;
        private string _Name;
        private int _PlayersCount;
        private List<RPlayer> _Players;

        private AGame _Game;

        private AGameStatus _GameStatus;

        private ASize _MapSize;

        public int Id => _Id;
        public string Name => _Name;
        public int PlayersCount => _PlayersCount;
        public IReadOnlyList<RPlayer> Players => _Players;

        public GameData Game => new GameData(_Game);

        public AGameStatus GameStatus => _GameStatus;

        private ASize MapSize => _MapSize;

        public ARoom(string name, int id, int playersCount)
        {
            _Name = name;
            _Id = id;
            _PlayersCount = playersCount;
            _Players = new List<RPlayer>();
            _GameStatus = AGameStatus.Wait;
        }

        public ARoom(ARoom room)
        {
            _Name = room._Name;
            _Id = room._Id;
            _PlayersCount = room._PlayersCount;
            _Players = room._Players;
            _GameStatus = room._GameStatus;
        }

        public void SetName(string name) => _Name = name;
        public void SetMapSize(ASize size) => _MapSize = size;
        public void SetPlayersCount(int count) => _PlayersCount = count;

        public void StartGame()
        {

            Random random = new Random((int)DateTime.Now.Ticks);

            _Game = new AGame();

            List<ICharacter> characters = new List<ICharacter>();

            for (int i = 0; i < _Players.Count; i++)
            {
                ASexType sexType = new[] { ASexType.Female, ASexType.Male }[random.Next(2)];
                ICharacter character = new ACharacter(GameExtension.CharacterName(sexType), GameExtension.DefaultFamily[random.Next(GameExtension.DefaultFamily.Count)], sexType, random.Next(-16, -5), i, i);
                characters.Add(character);
            }
            _Game.Initialize(_Players.Select(x => x.Name).ToList(), characters);
            _Game.StartGame(_MapSize);
            _GameStatus = AGameStatus.Game;

        }

        public void OverGame()
        {
            _GameStatus = AGameStatus.Over;
        }

        public bool Connect(RPlayer player)
        {
            if (_Players.Contains(player)) return false;
            else _Players.Add(player);
            return true;
        }

        public bool Disconnect(RPlayer player)
        {
            if (_Players.Contains(player)) _Players.Remove(player);
            else return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is ARoom room)
                return Id == room.Id && Name == room.Name && PlayersCount == room.PlayersCount && GameStatus == room.GameStatus && Players == room.Players;
            return false;
        }

    }
}
