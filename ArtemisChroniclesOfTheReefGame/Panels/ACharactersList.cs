using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary.Character;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class ACharactersList : AScrolleredPanel
    {

        public delegate void OnSelect(ICharacter character);
        public event OnSelect SelectEvent;

        private Dictionary<ICharacter, AButton> Buttons;
        public ACharactersList(ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            Buttons = new Dictionary<ICharacter, AButton>();
        }

        public override void Initialize()
        {

            base.Initialize();

            Text = "Список персонажей";

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

        }

        public new void Clear()
        {
            base.Clear();
            Buttons.Clear();
        }

        public void Update(IEnumerable<ICharacter> characters, int currentTurn)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, -30);

            foreach (AButton bt in Buttons.Values) bt.Enabled = false;

            foreach (ICharacter character in characters)
            {

                AButton button;

                if (Buttons.Keys.Where(x => x.Equals(character)).Count() != 0)
                {
                    button = Buttons[Buttons.Keys.Where(x => x.Equals(character)).First()];
                    string text = character.FullName + " (" + character.Age(currentTurn) + ")";
                    if (!button.Text.Equals(text)) button.Text = text;
                    button.Enabled = true;
                }
                else
                {
                    button = new AButton(new ASize(ContentSize.Width - 20, 80));
                    Add(button);
                    Buttons.Add(character, button);

                    button.Text = character.FullName + " (" + character.Age(currentTurn) + ")";

                    button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

                    button.MouseClickEvent += (state, mstate) => { SelectEvent?.Invoke(character); };
                }

                button.Location = last + new APoint(0, button.Height + 10);
                last = button.Location;

            }

            ContentSize = new ASize(ContentSize.Width, Buttons.Count > 0 ? last.Y + 90 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }
    }
}
