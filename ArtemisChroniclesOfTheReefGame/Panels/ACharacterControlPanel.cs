using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;
using GraphicsLibrary.Interfaces;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Extension;
using GameLibrary.Character;
using GameLibrary.Player;
using GameLibrary.Message;


namespace ArtemisChroniclesOfTheReefGame.Panels
{

    public class ACharacterControlPanel: AScrolleredPanel
    {

        public delegate void OnClick();

        public event OnClick MarryEvent;
        public event OnClick DivorceEvent;
        public event OnClick AgreementEvent;
        public event OnClick HeirEvent;
        public event OnClick WarEvent;
        public event OnClick PeaceEvent;
        public event OnClick UnionEvent;
        public event OnClick BreakUnionEvent;

        private AButton Marry;
        private AButton Divorce;
        private AButton Agreement;
        private AButton Heir;
        private AButton War;
        private AButton Peace;
        private AButton Union;
        private AButton BreakUnion;

        private List<Action<GameData, ICharacter>> Buttons;

        private IPlayer Player;

        public ACharacterControlPanel(ASize size): base(AScrollbarAlign.Vertical, size)
        {
            Buttons = new List<Action<GameData, ICharacter>>();
        }

        public override void Initialize()
        {

            base.Initialize();

            ASize size = new ASize(ContentSize.Width - 20, 50);

            Marry = new AButton(size) { Text = "Заключить брак" };
            Divorce = new AButton(size) { Text = "Расторгнуть брак" };
            Agreement = new AButton(size) { Text = "Заключить соглшаение" };
            Heir = new AButton(size) { Text = "Назначить наследника" };
            War = new AButton(size) { Text = "Объявить войну" };
            Peace = new AButton(size) { Text = "Заключить мир" };
            Union = new AButton(size) { Text = "Заключить союз" };
            BreakUnion = new AButton(size) { Text = "Разорвать союз" };

            Marry.MouseClickEvent += (state, mstate) =>
            {
                MarryEvent?.Invoke();
            };

            Divorce.MouseClickEvent += (state, mstate) =>
            {
                DivorceEvent?.Invoke();
            };

            Agreement.MouseClickEvent += (state, mstate) =>
            {
                AgreementEvent?.Invoke();
            };

            Heir.MouseClickEvent += (state, mstate) =>
            {
                HeirEvent?.Invoke();
            };
            
            War.MouseClickEvent += (state, mstate) =>
            {
                WarEvent?.Invoke();
            };

            Peace.MouseClickEvent += (state, mstate) =>
            {
                PeaceEvent?.Invoke();
            };

            Union.MouseClickEvent += (state, mstate) =>
            {
                UnionEvent?.Invoke();
            };

            BreakUnion.MouseClickEvent += (state, mstate) =>
            {
                BreakUnionEvent?.Invoke();
            };

            Add(Marry);
            Add(Divorce);
            Add(Agreement);
            Add(Heir);
            Add(War);
            Add(Peace);
            Add(Union);
            Add(BreakUnion);

            foreach (IPrimitive primitive in Content)
            {
                primitive.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
                primitive.Enabled = false;
            }

            Player = null;

            Buttons.Add((GameData gameData, ICharacter character) => Marry.Enabled = character.Age(gameData.CurrentTurn) > 15 && !character.IsMarried && character.IsAlive ? true : false);
            Buttons.Add((GameData gameData, ICharacter character) => Divorce.Enabled = character.IsOwned && gameData.Players[character.OwnerId].Equals(gameData.ActivePlayer) && character.IsMarried && character.IsAlive ? true : false);
            Buttons.Add((GameData gameData, ICharacter character) => Agreement.Enabled = gameData.Players.Values.Select(x => x.Ruler).Contains(character) && !gameData.ActivePlayer.Characters.Contains(character) && character.IsAlive ? true : false);
            Buttons.Add((GameData gameData, ICharacter character) => Heir.Enabled = gameData.ActivePlayer.Characters.Contains(character) && character.IsAlive ? true : false);
            Buttons.Add((GameData gameData, ICharacter character) => War.Enabled = gameData.CharacterIsRuler(character, out Player) && !Player.Equals(gameData.ActivePlayer) && new ARelationshipType[] { ARelationshipType.Neutrality, ARelationshipType.None }.Contains(Player.Relationship(gameData.ActivePlayer)) && character.IsAlive ? true : false);
            Buttons.Add((GameData gameData, ICharacter character) => Peace.Enabled = gameData.CharacterIsRuler(character, out Player) && !Player.Equals(gameData.ActivePlayer) && Player.Relationship(gameData.ActivePlayer).Equals(ARelationshipType.War) && character.IsAlive ? true : false);
            Buttons.Add((GameData gameData, ICharacter character) => Union.Enabled = gameData.CharacterIsRuler(character, out Player) && !Player.Equals(gameData.ActivePlayer) && new ARelationshipType[] { ARelationshipType.Neutrality, ARelationshipType.None }.Contains(Player.Relationship(gameData.ActivePlayer)) && character.IsAlive ? true : false);
            Buttons.Add((GameData gameData, ICharacter character) => BreakUnion.Enabled = gameData.CharacterIsRuler(character, out Player) && !Player.Equals(gameData.ActivePlayer) && Player.Relationship(gameData.ActivePlayer).Equals(ARelationshipType.Union) && character.IsAlive ? true : false);

        }

        public void Update(GameData gameData, ICharacter character)
        {

            foreach (Action<GameData, ICharacter> action in Buttons) action(gameData, character);

            APoint last = new APoint(10, 10);

            foreach (IPrimitive primitive in Content.Where(x => x.Enabled.Equals(true)))
            {
                primitive.Location = last;
                last.Y += primitive.Height + 10;
            }

            ContentSize = new ASize(ContentSize.Width, Content.Where(x => x.Enabled).Count() > 0 ? last.Y : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }

    }
}
