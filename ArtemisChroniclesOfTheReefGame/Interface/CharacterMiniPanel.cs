using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;
using AKeyState = CommonPrimitivesLibrary.AKeyState;

using GameLibrary;
using GameLibrary.Character;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class CharacterMiniPanel : APanel
    {

        public delegate void OnUpdate();

        public event OnUpdate UpdateEvent;

        private APanel HeaderPanel;
        private AEmptyPanel InfoPanel;
        private AEmptyPanel StatsPanel;

        private CharacterInteractionPanel CharacterInteractionPanel;

        private AGame Game;
        private ICharacter Character;

        public CharacterMiniPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = Convert.ToInt32(Width * 5 / 8) - 50;

            HeaderPanel = new APanel(new ASize(Width, 50)) { Parent = this, Location = new APoint(0, 0) };
            InfoPanel = new AEmptyPanel(new ASize(dWidth, 120)) { Parent = this, Location = HeaderPanel.Location + new APoint(10, HeaderPanel.Height) };
            StatsPanel = new AEmptyPanel(new ASize(Width - dWidth, 120)) { Parent = this, Location = InfoPanel.Location + new APoint(InfoPanel.Width + 10, 0) };

            CharacterInteractionPanel = new CharacterInteractionPanel(Game, new ASize(Width - 20, Height - 190)) { Parent = this, Location = InfoPanel.Location + new APoint(0, InfoPanel.Height + 10) };

            CharacterInteractionPanel.UpdateEvent += () =>
            {
                UpdateEvent?.Invoke();
                Update();
            };

            HeaderPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            InfoPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            InfoPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            InfoPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            StatsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            StatsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            StatsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

        }

        public void Update()
        {
            if (Character is object) Update(Character);
        }

        public void Update(ICharacter character)
        {

            Character = character;

            HeaderPanel.FillColor = TexturePack.Colors[character.OwnerId];

            HeaderPanel.Text = character.FullName + " (" + (character.IsOwned? Game.Players[character.OwnerId].Name: "Игрок отсутствует") + ")";
            InfoPanel.Text = 
                "Статус: " + (character.IsAlive? "жив": "мертв") + (character.SexType.Equals(ASexType.Male) ? "" : "а") + "\n" +
                "Год рождения: " + character.BirthDate + " (" + character.Age(Game.CurrentTurn) + ")" + "\n" +
                "Пол: " + GameLocalization.SexTypeName[character.SexType] + "\n" + 
                "Супруг" + (character.SexType.Equals(ASexType.Male) ? "а" : "") + ": " + (character.IsMarried && Game.GetCharacter(character.SpouseId) is ICharacter spouse? spouse.FullName : "Отсутствует") + "\n" +
                "Отец: " + (character.FatherId > 0 ? Game.GetCharacter(character.FatherId).FullName : "Неизвестно") + "\n" +
                "Мать: " + (character.MotherId > 0 ? Game.GetCharacter(character.MotherId).FullName : "Неизвестно");
            StatsPanel.Text = string.Join("\n", typeof(ICharacterStats).GetProperties().Select(x => StringPad(GameLocalization.PlayerStatsName[x.Name] + ": ", 20) + x.GetValue(character)));
            CharacterInteractionPanel.Update(character);
        }

        public void Hide() => Enabled = false;
        public void Show(ICharacter character)
        {
            Enabled = true;
            Update(character);
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);



    }
}
