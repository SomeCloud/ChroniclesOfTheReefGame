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

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class CharacterInteractionPanel : AScrolleredPanel
    {

        public delegate void OnUpdate();

        public event OnUpdate UpdateEvent;

        private AGame Game;

        private AButton Marry;
        private AButton Agreement;
        private AButton Heir;
        private AButton Viceroy;
        private AButton War;
        private AButton Peace;
        private AButton Union;

        private List<Action<ICharacter>> Buttons;

        private ASize size;

        private IPlayer Player;

        public CharacterInteractionPanel(AGame game, ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            Game = game;
            Buttons = new List<Action<ICharacter>>();
        }

        public override void Initialize()
        {

            base.Initialize();

            size = new ASize(ContentSize.Width - 20, 50);

            Marry = new AButton(size) { Text = "Заключить брак" };
            Agreement = new AButton(size) { Text = "Заключить соглшаение" };
            Heir = new AButton(size) { Text = "Назначить наследника" };
            Viceroy = new AButton(size) { Text = "Назначить наместника" };
            War = new AButton(size) { Text = "Объявить войну" };
            Peace = new AButton(size) { Text = "Заключить мир" };
            Union = new AButton(size) { Text = "Заключить союз" };

            Player = null;

            Buttons.Add((ICharacter character) => Marry.Enabled = character.SpouseId == 0 && character.IsAlive ? true : false);
            Buttons.Add((ICharacter character) => Agreement.Enabled = Game.Players.Values.Select(x => x.Ruler).Contains(character) && !Game.ActivePlayer.Characters.Contains(character) && character.IsAlive ? true : false);
            Buttons.Add((ICharacter character) => Heir.Enabled = Game.ActivePlayer.Characters.Contains(character) && character.IsAlive ? true : false);
            Buttons.Add((ICharacter character) => Viceroy.Enabled = Game.ActivePlayer.Ruler.Equals(character) && character.IsAlive ? true : false);
            Buttons.Add((ICharacter character) => War.Enabled = Game.CharacterIsRuler(character, out Player) && !Player.Equals(Game.ActivePlayer) && new ARelationshipType[] { ARelationshipType.Neutrality, ARelationshipType.None }.Contains(Player.Relationship(Game.ActivePlayer)) && character.IsAlive ? true : false);
            Buttons.Add((ICharacter character) => Peace.Enabled = Game.CharacterIsRuler(character, out Player) && !Player.Equals(Game.ActivePlayer) && Player.Relationship(Game.ActivePlayer).Equals(ARelationshipType.War) && character.IsAlive ? true : false);
            Buttons.Add((ICharacter character) => Union.Enabled = Game.CharacterIsRuler(character, out Player) && !Player.Equals(Game.ActivePlayer) && new ARelationshipType[] { ARelationshipType.Neutrality, ARelationshipType.None }.Contains(Player.Relationship(Game.ActivePlayer)) && character.IsAlive ? true : false);

            Add(Marry);
            Add(Agreement);
            Add(Heir);
            Add(Viceroy);
            Add(War);
            Add(Peace);
            Add(Union);

            foreach (IPrimitive primitive in Content)
            {
                primitive.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
                primitive.Enabled = false;
            }

            War.MouseClickEvent += (state, mstate) => {
                if (Player is object)
                {
                    IPlayer player = Game.ActivePlayer;
                    IMessage message = new AMessage(Game.ActivePlayer, Player, "Объявление войны", "Игрок " + Game.ActivePlayer.Name + " объявляет войну игроку " + Player.Name, () => Game.SetRelationship(player, Player, ARelationshipType.War), false);
                    Player.SendMessage(message);
                    UpdateEvent?.Invoke();
                }
            };

            Union.MouseClickEvent += (state, mstate) => {
                if (Player is object)
                {
                    IPlayer player = Game.ActivePlayer;
                    IMessage message = new AMessage(Game.ActivePlayer, Player, "Предложение союза", "Игрок " + Game.ActivePlayer.Name + " предлагает заключить союз игроку " + Player.Name, () => Game.SetRelationship(player, Player, ARelationshipType.Union), true);
                    Player.SendMessage(message);
                    UpdateEvent?.Invoke();
                }
            };

            Peace.MouseClickEvent += (state, mstate) => {
                if (Player is object)
                {
                    IPlayer player = Game.ActivePlayer;
                    IMessage message = new AMessage(Game.ActivePlayer, Player, "Предложение мира", "Игрок " + Game.ActivePlayer.Name + " предлагает заключить мир игроку " + Player.Name, () => Game.SetRelationship(player, Player, ARelationshipType.Neutrality), true);
                    Player.SendMessage(message);
                    UpdateEvent?.Invoke();
                }
            };

        }

        public void Update(ICharacter character)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            foreach (Action<ICharacter> action in Buttons) action(character);

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
