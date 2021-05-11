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
namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class CharacterInteractionPanel: APanel
    {

        private AGame Game;

        private AScrolleredPanel _InteractionPanel;

        private AButton Marry;
        private AButton Agreement;
        private AButton Heir;
        private AButton Viceroy;

        private List<Action<ICharacter>> Buttons;

        private ASize size;

        public AScrolleredPanel InteractionPanel { get => _InteractionPanel; }

        public CharacterInteractionPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
            Buttons = new List<Action<ICharacter>>();
        }

        public override void Initialize()
        {

            base.Initialize();

            _InteractionPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, Size - 20) { Parent = this, Location = new APoint(10, 10) };

            size = new ASize(_InteractionPanel.ContentSize.Width - 20, 50);

            Marry = new AButton(size) { Text = "Заключить брак" };
            Agreement = new AButton(size) { Text = "Заключить соглшаение" };
            Heir = new AButton(size) { Text = "Назначить наследника" };
            Viceroy = new AButton(size) { Text = "Назначить наместника" };

            Buttons.Add((ICharacter character) => Marry.Enabled = character.SpouseId == 0 ? true : false);
            Buttons.Add((ICharacter character) => Agreement.Enabled = Game.Players.Values.Select(x => x.Ruler).Contains(character) && !Game.ActivePlayer.Characters.Contains(character)? true: false);
            Buttons.Add((ICharacter character) => Heir.Enabled = Game.ActivePlayer.Characters.Contains(character)? true: false);
            Buttons.Add((ICharacter character) => Viceroy.Enabled = Game.ActivePlayer.Ruler.Equals(character)? true: false);

            _InteractionPanel.Add(Marry);
            _InteractionPanel.Add(Agreement);
            _InteractionPanel.Add(Heir);
            _InteractionPanel.Add(Viceroy);

            Marry.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            Agreement.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            Heir.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
            Viceroy.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            Marry.Enabled = false;
            Agreement.Enabled = false;
            Heir.Enabled = false;
            Viceroy.Enabled = false;

        }

        public void Update(ICharacter character)
        {

            _InteractionPanel.Scrollbar.Value = _InteractionPanel.Scrollbar.MinValue;

            foreach (Action<ICharacter> action in Buttons) action(character);

            APoint last = new APoint(10, 10);

            foreach (IPrimitive primitive in _InteractionPanel.Content.Where(x => x.Enabled.Equals(true)))
            {
                primitive.Location = last;
                last.Y += primitive.Height + 10;
            }

            _InteractionPanel.ContentSize = new ASize(_InteractionPanel.ContentSize.Width, _InteractionPanel.Content.Where(x => x.Enabled).Count() > 0 ? last.Y : Height);
            _InteractionPanel.Scrollbar.MaxValue = Height < _InteractionPanel.ContentSize.Height ? _InteractionPanel.ContentSize.Height - Height + 10 : 0;

        }

    }
}
