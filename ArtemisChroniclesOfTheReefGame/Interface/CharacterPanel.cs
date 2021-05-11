using System;
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
    public class CharacterPanel: APanel
    {

        private AGame Game;
        private ICharacter _Character;
        public ICharacter Character { get => _Character; }


        private AScrolleredPanel _MainInfo;
        private APanel _Portrait;
        private APanel _Stats;

        private AButton _InteractionButton;

        private CharacterInteractionPanel _CharacterInteractionPanel;

        public AScrolleredPanel MainInfo { get => _MainInfo; }
        public APanel Portrait { get => _Portrait; }
        public APanel Stats { get => _Stats; }

        public AButton InteractionButton { get => _InteractionButton; }

        public CharacterInteractionPanel CharacterInteractionPanel { get => _CharacterInteractionPanel; }

        public CharacterPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            int width = (Height - 30) / 2;

            _Portrait = new APanel(new ASize(width, width)) { Parent = this, Location = new APoint(10, 10) };
            _Stats = new APanel(new ASize(Width - width - 30, width)) { Parent = this, Location = _Portrait.Location + new APoint(_Portrait.Width + 10, 0) };
            _MainInfo = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(Width - 20, Height - _Portrait.Height - 30)) { Parent = this, Location = _Portrait.Location + new APoint(0, _Portrait.Height + 10) };

            _InteractionButton = new AButton(new ASize(50, 50)) { Parent = this, Location = _Portrait.Location + new APoint(_Portrait.Width - 50, _Portrait.Height - 50), Text = "..." };
            _CharacterInteractionPanel = new CharacterInteractionPanel(Game, _Stats.Size) { Parent = this, Location = _InteractionButton.Location };

            _MainInfo.TextLabel.VerticalAlign = ATextVerticalAlign.Top;

            _MainInfo.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            _Stats.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            _MainInfo.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            _InteractionButton.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

            _InteractionButton.MouseClickEvent += (state, mstate) => { _CharacterInteractionPanel.Enabled = true; };
            _CharacterInteractionPanel.MouseOverEvent += (state, mstate) => { _CharacterInteractionPanel.Enabled = false; };

            CharacterInteractionPanel.Enabled = false;

        }

        public void Update(ICharacter character)
        {
            if (_Character is null || !_Character.Equals(character)) _Character = character;
            _MainInfo.Scrollbar.Value = _MainInfo.Scrollbar.MinValue;
            SetInfo();
            _CharacterInteractionPanel.Update(character);
            _CharacterInteractionPanel.Enabled = false;
        }

        private void SetInfo()
        {
            _MainInfo.Text = _Character.FullName + " (" + (_Character.IsAlive? "Жив": "Мертв") + (_Character.SexType.Equals(ASexType.Male)? "": "а") + ")" + "\n" +
                "Год рождения: " + _Character.BirthDate + " (" + _Character.Age(Game.CurrentTurn) + ")" + "\n" +
                "Игрок: " + (Game.Players.ContainsKey(_Character.OwnerId)? Game.Players[_Character.OwnerId].Name: "Отсутствует") + "\n" +
                "Пол: " + GameLocalization.SexTypeName[_Character.SexType] + "\n" +
                "Расположение: " + (Game.GetMapCell(_Character.Location) is AMapCell mapCell && mapCell.IsSettlement? mapCell.Settlement.Name: "(" + _Character.Location + ")") + "\n" +
                "Супруг" + (_Character.SexType.Equals(ASexType.Male) ? "а" : "") + ": " + (_Character.IsMarried && Game.GetCharacter(_Character.SpouseId) is ICharacter character? character.Name + " " + character.FamilyName : "Отсутствует") + "\n" +
                "Отец: " + (_Character.FatherId > 0? Game.GetCharacter(_Character.FatherId).FullName : "Неизвестно") + "\n" +
                "Мать: " + (_Character.MotherId > 0? Game.GetCharacter(_Character.MotherId).FullName : "Неизвестно") + "\n" +
                "Дети: " + (_Character.IsChild ? string.Join(",\n", _Character.ChildId.Select(x => Game.GetCharacter(x)).Select(x => x.Name + " " + x.FamilyName)) : "Отсутствуют");
            _Stats.Text = string.Join("\n", typeof(ICharacterStats).GetProperties().Select(x => StringPad(GameLocalization.PlayerStatsName[x.Name] + ": ", 20) + x.GetValue(_Character)));
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);
    }
}
