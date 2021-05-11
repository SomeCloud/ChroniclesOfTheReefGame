using System;
using System.Collections.Generic;
using System.Linq;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Extension;
using GameLibrary.Map;
using GameLibrary.Character;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class CharactersListPanel:AScrolleredPanel
    {

        public delegate void OnSelectCharacter(ICharacter character);
        public event OnSelectCharacter SelectCharacterEvent;

        private AGame Game;
        private Dictionary<ICharacter, AButton> CharacterButton;

        //private AScrolleredPanel CharactersList;

        public CharactersListPanel(AGame game, ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            Game = game;
            CharacterButton = new Dictionary<ICharacter, AButton>();
        }

        public override void Initialize()
        {

            base.Initialize();

            //CharactersList = new AScrolleredPanel(AScrollbarAlign.Vertical, Size) { Parent = this, Location = new APoint(10, 10) };

        }

        public void Update(List<ICharacter> characters)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, -50);

            foreach (AButton bt in CharacterButton.Values) bt.Enabled = false;

            foreach (ICharacter character in characters)
            {
                AButton button;

                if (CharacterButton.ContainsKey(character))
                {
                    button = CharacterButton[character];
                    string text = character.Name + " " + character.FamilyName + " (" + character.Age(Game.CurrentTurn) + "), " + (character.IsAlive ? "жив" : "мертв") + (character.SexType.Equals(ASexType.Male) ? "" : "а") + ", (" + (Game.GetMapCell(character.Location) is AMapCell mapCell && mapCell.IsSettlement ? mapCell.Settlement.Name : "не определенно") + ")";
                    if (!button.Text.Equals(text)) button.Text = text;
                    button.Enabled = true;
                }
                else
                {
                    button = new AButton(new ASize(ContentSize.Width - 20, 50));
                    button.Text = character.Name + " " + character.FamilyName + " (" + character.Age(Game.CurrentTurn) + "), " + (character.IsAlive ? "жив" : "мертв") + (character.SexType.Equals(ASexType.Male) ? "" : "а") + ", (" + (Game.GetMapCell(character.Location) is AMapCell mapCell && mapCell.IsSettlement ? mapCell.Settlement.Name : "не определенно") + ")";
                    Add(button);
                    CharacterButton.Add(character, button);
                    button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
                    button.MouseClickEvent += (state, mstate) => { SelectCharacterEvent?.Invoke(character); };
                }

                button.Location = last + new APoint(0, button.Height + 10);
                last = button.Location;
            }

            ContentSize = new ASize(ContentSize.Width, CharacterButton.Count > 0 ? last.Y + 60 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }
    }
}
