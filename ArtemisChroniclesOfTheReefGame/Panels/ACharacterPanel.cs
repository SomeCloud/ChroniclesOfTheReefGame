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
using GameLibrary.Map;
using GameLibrary.Extension;
using GameLibrary.Character;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class ACharacterPanel: AEmptyPanel
    {

        public delegate void OnClick(ICharacter character);

        public event OnClick MarryEvent;
        public event OnClick AgreementEvent;
        public event OnClick HeirEvent;
        public event OnClick WarEvent;
        public event OnClick PeaceEvent;
        public event OnClick UnionEvent;
        public event OnClick BreakUnionEvent;

        private APanel HeaderPanel;
        private AEmptyPanel InfoPanel;
        private AEmptyPanel StatsPanel;

        private ACharacterControlPanel CharacterControlPanel;

        private ICharacter Character;

        public ACharacterPanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = Convert.ToInt32(Width * 5 / 8) - 50;

            HeaderPanel = new APanel(new ASize(Width, 50)) { Parent = this, Location = new APoint(0, 0) };
            InfoPanel = new AEmptyPanel(new ASize(dWidth, 120)) { Parent = this, Location = HeaderPanel.Location + new APoint(10, HeaderPanel.Height) };
            StatsPanel = new AEmptyPanel(new ASize(Width - dWidth, 120)) { Parent = this, Location = InfoPanel.Location + new APoint(InfoPanel.Width + 10, 0) };

            CharacterControlPanel = new ACharacterControlPanel(new ASize(Width - 20, Height - HeaderPanel.Height - InfoPanel.Height - 30)) { Parent = this, Location = InfoPanel.Location + new APoint(0, InfoPanel.Height + 10) };

            CharacterControlPanel.MarryEvent += () => MarryEvent?.Invoke(Character);
            CharacterControlPanel.AgreementEvent += () => AgreementEvent?.Invoke(Character);
            CharacterControlPanel.HeirEvent += () => HeirEvent?.Invoke(Character);
            CharacterControlPanel.WarEvent += () => WarEvent?.Invoke(Character);
            CharacterControlPanel.PeaceEvent += () => PeaceEvent?.Invoke(Character);
            CharacterControlPanel.UnionEvent += () => UnionEvent?.Invoke(Character);
            CharacterControlPanel.BreakUnionEvent += () => BreakUnionEvent?.Invoke(Character);

            HeaderPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            InfoPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            InfoPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            InfoPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            StatsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            StatsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            StatsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

        }

        public void Update(GameData gameData, ICharacter character)
        {

            Character = character;

            HeaderPanel.FillColor = TexturePack.Colors[character.OwnerId];

            HeaderPanel.Text = character.FullName + " (" + (character.IsOwned ? gameData.Players[character.OwnerId].Name : "Игрок отсутствует") + ")";
            InfoPanel.Text =
                "Статус: " + (character.IsAlive ? "жив" : "мертв") + (character.SexType.Equals(ASexType.Male) ? "" : "а") + "\n" +
                "Год рождения: " + character.BirthDate + " (" + character.Age(gameData.CurrentTurn) + ")" + "\n" +
                "Пол: " + GameLocalization.SexTypeName[character.SexType] + "\n" +
                "Супруг" + (character.SexType.Equals(ASexType.Male) ? "а" : "") + ": " + (character.IsMarried && gameData.GetCharacter(character.SpouseId) is ICharacter spouse ? spouse.FullName : "Отсутствует") + "\n" +
                "Отец: " + (character.FatherId > 0 ? gameData.GetCharacter(character.FatherId).FullName : "Неизвестно") + "\n" +
                "Мать: " + (character.MotherId > 0 ? gameData.GetCharacter(character.MotherId).FullName : "Неизвестно");
            StatsPanel.Text = string.Join("\n", typeof(ICharacterStats).GetProperties().Select(x => StringPad(GameLocalization.PlayerStatsName[x.Name] + ": ", 20) + x.GetValue(character)));
            CharacterControlPanel.Update(gameData, character);
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
