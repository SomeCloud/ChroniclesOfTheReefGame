using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using GameLibrary;
using GameLibrary.Character;
using GameLibrary.Extension;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;


namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class ACharacterInfoPanel : AScrolleredPanel
    {

        public delegate void OnSelect(ICharacter character);

        public event OnSelect SelectEvent;

        private AEmptyPanel StatsPanel;

        private Dictionary<int, AButton> Buttons;

        private AButton NullFatherButton;
        private AButton NullMotherButton;
        private AButton NullSpouseButton;

        private AButton FatherButton;
        private AButton MotherButton;
        private AButton SpouseButton;

        public ACharacterInfoPanel(ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            Buttons = new Dictionary<int, AButton>();
        }

        public override void Initialize()
        {

            base.Initialize();

            StatsPanel = new AEmptyPanel(new ASize(ContentSize.Width - 20, 120)) { Location = new APoint(10, 10) };

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

            StatsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            StatsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            StatsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            ASize bthSize = new ASize(ContentSize.Width - 20, 50);

            NullFatherButton = new AButton(bthSize) { Location = StatsPanel.Location + new APoint(0, StatsPanel.Height + 5), Text = "Отец: неизвестно", IsInteraction = false };
            NullMotherButton = new AButton(bthSize) { Location = NullFatherButton.Location + new APoint(0, NullFatherButton.Height + 5), Text = "Мать: неизвестно", IsInteraction = false };
            NullSpouseButton = new AButton(bthSize) { Location = NullMotherButton.Location + new APoint(0, NullMotherButton.Height + 5), Text = "Супруг: неизвестно", IsInteraction = false };

            Add(StatsPanel);

            Add(NullFatherButton);
            Add(NullMotherButton);
            Add(NullSpouseButton);

            NullFatherButton.BorderColor = GraphicsExtension.DefaultFillColor;
            NullMotherButton.BorderColor = GraphicsExtension.DefaultFillColor;
            NullSpouseButton.BorderColor = GraphicsExtension.DefaultFillColor;

            NullFatherButton.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            NullFatherButton.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            NullMotherButton.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            NullMotherButton.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            NullSpouseButton.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            NullSpouseButton.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

        }

        public new void Clear()
        {
            base.Clear();
            Buttons.Clear();
        }

        public void Update(GameData gameData, ICharacter character)
        {

            NullFatherButton.Enabled = false;
            NullMotherButton.Enabled = false;
            NullSpouseButton.Enabled = false;

            foreach (AButton bt in Buttons.Values) bt.Enabled = false;

            Scrollbar.Value = Scrollbar.MinValue;

            string statText =
                "Статус: " + (character.IsAlive ? "жив" : "мертв") + (character.SexType.Equals(ASexType.Male) ? "" : "а") + "\n" +
                "Год рождения: " + character.BirthDate + " (" + character.Age(gameData.CurrentTurn) + ")" + "\n" +
                "Пол: " + GameLocalization.SexTypeName[character.SexType] + "\n" +
                string.Join("\n", typeof(ICharacterStats).GetProperties().Select(x => StringPad(GameLocalization.PlayerStatsName[x.Name] + ": ", 20) + x.GetValue(character)));
            StatsPanel.Height = 20 + (statText.Count(x => x == '\n') + 1) * StatsPanel.TextLabel.Font.Height;
            StatsPanel.Text = statText;

            ASize bthSize = new ASize(ContentSize.Width - 20, 50);

            ICharacter father = character.FatherId > 0? gameData.GetCharacter(character.FatherId) : null;
            ICharacter mother = character.MotherId > 0 ? gameData.GetCharacter(character.MotherId) : null;
            ICharacter spouse = character.SpouseId > 0 ? gameData.GetCharacter(character.SpouseId) : null;

            if (father is object && Buttons.ContainsKey(character.FatherId))
            {
                FatherButton = Buttons[character.FatherId];
                FatherButton.Location = StatsPanel.Location + new APoint(0, StatsPanel.Height + 5);
                string text = "Отец: " + father.FullName;
                if (!FatherButton.Text.Equals(text)) FatherButton.Text = text;
                FatherButton.IsInteraction = true;
                FatherButton.Enabled = true;
            }
            else if (father is null)
            {
                FatherButton = NullFatherButton;
                FatherButton.Location = StatsPanel.Location + new APoint(0, StatsPanel.Height + 5);
                NullFatherButton.Enabled = true;
            }
            else
            {
                FatherButton = new AButton(bthSize) { Location = StatsPanel.Location + new APoint(0, StatsPanel.Height + 5), Text = "Отец: " + father.FullName, IsInteraction = true };
                FatherButton.MouseClickEvent += (state, mstate) => SelectEvent.Invoke(father);

                Add(FatherButton);

                FatherButton.BorderColor = GraphicsExtension.DefaultFillColor;

                FatherButton.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                FatherButton.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                Buttons.Add(character.FatherId, FatherButton);
            }

            if (mother is object && Buttons.ContainsKey(character.MotherId))
            {
                MotherButton = Buttons[character.MotherId];
                MotherButton.Location = FatherButton.Location + new APoint(0, FatherButton.Height + 5);
                string text = "Мать: " + mother.FullName;
                if (!MotherButton.Text.Equals(text)) MotherButton.Text = text;
                MotherButton.IsInteraction = true;
                MotherButton.Enabled = true;
            }
            else if (mother is null)
            {
                MotherButton = NullMotherButton;
                MotherButton.Location = FatherButton.Location + new APoint(0, FatherButton.Height + 5);
                MotherButton.Enabled = true;
            }
            else
            {
                MotherButton = new AButton(bthSize) { Location = FatherButton.Location + new APoint(0, FatherButton.Height + 5), Text = "Мать: " + mother.FullName, IsInteraction = true };
                MotherButton.MouseClickEvent += (state, mstate) => SelectEvent.Invoke(father);

                Add(MotherButton);

                MotherButton.BorderColor = GraphicsExtension.DefaultFillColor;

                MotherButton.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                MotherButton.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                Buttons.Add(character.MotherId, MotherButton);
            }

            if (spouse is object && Buttons.ContainsKey(character.SpouseId))
            {
                SpouseButton = Buttons[character.SpouseId];
                SpouseButton.Location = Location = MotherButton.Location + new APoint(0, MotherButton.Height + 5);
                string text = "Супруг" + (character.SexType.Equals(ASexType.Male)? "а": "") + ": " + spouse.FullName;
                if (!SpouseButton.Text.Equals(text)) SpouseButton.Text = text;
                SpouseButton.IsInteraction = true;
                SpouseButton.Enabled = true;
            }
            else if (spouse is null)
            {
                SpouseButton = NullSpouseButton;
                SpouseButton.Location = MotherButton.Location + new APoint(0, MotherButton.Height + 5);
                SpouseButton.Enabled = true;
            }
            else
            {
                SpouseButton = new AButton(bthSize) { Location = MotherButton.Location + new APoint(0, MotherButton.Height + 5), Text = "Супруг" + (character.SexType.Equals(ASexType.Male) ? "а" : "") + ": " + spouse.FullName, IsInteraction = true };
                SpouseButton.MouseClickEvent += (state, mstate) => SelectEvent.Invoke(father);

                Add(SpouseButton);

                SpouseButton.BorderColor = GraphicsExtension.DefaultFillColor;

                SpouseButton.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                SpouseButton.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                Buttons.Add(character.SpouseId, SpouseButton);
            }

            APoint last = SpouseButton.Location;

            foreach (ICharacter child in character.ChildId.Select(x => gameData.GetCharacter(x)))
            {

                AButton button;

                if (Buttons.Keys.Where(x => x.Equals(child.Id)).Count() != 0)
                {
                    button = Buttons[Buttons.Keys.Where(x => x.Equals(child.Id)).First()];
                    string text = (child.SexType.Equals(ASexType.Male) ? "Сын" : "Дочь") + ": " + child.FullName;
                    if (!button.Text.Equals(text)) button.Text = text;
                    button.Enabled = true;
                }
                else
                {
                    button = new AButton(bthSize);
                    Add(button);
                    Buttons.Add(child.Id, button);

                    button.Text = (child.SexType.Equals(ASexType.Male) ? "Сын" : "Дочь") + ": " + child.FullName;

                    button.BorderColor = GraphicsExtension.DefaultFillColor;

                    button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                    button.MouseClickEvent += (state, mstate) => SelectEvent?.Invoke(child);

                }

                button.Location = last + new APoint(0, button.Height + 5);
                last = button.Location;

            }

            ContentSize = new ASize(ContentSize.Width, last.Y + bthSize.Height + 10);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 5 : 0;

        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
