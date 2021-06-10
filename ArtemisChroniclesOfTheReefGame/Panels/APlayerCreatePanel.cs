using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using GameLibrary;
using GameLibrary.Character;
using GameLibrary.Extension;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class APlayerCreatePanel: AEmptyPanel
    {

        public delegate void OnDone(string playerName, string characterName, string characterFamilyName, int birthdate, ASexType sexType, int attractiveness, int education, int martialSkills, int health, int fertility);

        public event OnDone DoneEvent;

        private ALabeledTextBox PlayerName;

        private ALabeledTextBox CharacterName;
        private ALabeledTextBox CharacterFamilyName;

        private AButton Male;
        private AButton Female;

        private ALabeledScrollbar CharacterAge;

        private AButton RandomCharacter;
        private AButton Done;

        private ALabel Points;

        private ALabeledScrollbar CharacterAttractiveness;
        private ALabeledScrollbar CharacterEducation;
        private ALabeledScrollbar CharacterMartialSkills;
        private ALabeledScrollbar CharacterHealth;
        private ALabeledScrollbar CharacterFertility;

        private AListContainer StatsContainer;

        private int maxPoint;

        private int Attractiveness;
        private int Education;
        private int MartialSkills;
        private int Health;
        private int Fertility;

        private int freePoints;

        private ASexType SexType;

        public APlayerCreatePanel(ASize size): base(size)
        {

            Attractiveness = 1;
            Education = 1;
            MartialSkills = 1;
            Health = 1;
            Fertility = 1;

            maxPoint = GameExtension.PlayerDefautStatsValue - Attractiveness - Education - MartialSkills - Health - Fertility;

            freePoints = GameExtension.PlayerDefautStatsValue - Attractiveness - Education - MartialSkills - Health - Fertility;

            SexType = ASexType.Male;

        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = (Width - 30) / 2;

            PlayerName = new ALabeledTextBox(new ASize(dWidth, 100)) { Parent = this, Location = new APoint(10, 10), LabelText = "Имя игрока" };

            CharacterName = new ALabeledTextBox(new ASize(dWidth, 100)) { Parent = this, Location = PlayerName.Location + new APoint(0, PlayerName.Height + 10), LabelText = "Имя персонажа" };
            CharacterFamilyName = new ALabeledTextBox(new ASize(dWidth, 100)) { Parent = this, Location = CharacterName.Location + new APoint(0, CharacterName.Height + 10), LabelText = "Фамилия персонажа" };

            Male = new AButton(new ASize((dWidth - 10) / 2, 60)) { Parent = this, Location = CharacterFamilyName.Location + new APoint(0, CharacterFamilyName.Height + 10), Text = "Мужчина" };
            Female = new AButton(new ASize((dWidth - 10) / 2, 60)) { Parent = this, Location = Male.Location + new APoint(Male.Width + 10, 0), Text = "Женщина" };

            CharacterAge = new ALabeledScrollbar(new ASize(dWidth, 80)) { Parent = this, Location = Male.Location + new APoint(0, Male.Height + 10), Text = "Возраст: ", MinValue = 16, Value = 16, MaxValue = 55 };

            Done = new AButton(new ASize(dWidth - GraphicsExtension.DefaultMiniButtonSize.Width - 10, GraphicsExtension.DefaultMiniButtonSize.Height)) { Parent = this, Location = CharacterAge.Location + new APoint(0, CharacterAge.Height + 10), Text = "Готово" };
            RandomCharacter = new AButton(GraphicsExtension.DefaultMiniButtonSize, TexturePack.MiniButtons_Dice) { Parent = this, Location = Done.Location + new APoint(Done.Width + 10, 0) };

            Points = new ALabel(new ASize(dWidth, 100)) { Parent = this, Text = "Очков осталось: " + freePoints };

            CharacterAttractiveness = new ALabeledScrollbar(new ASize(dWidth, 80)) { Parent = this, Text = "Очки привлекательности: ", MinValue = 1, MaxValue = maxPoint };
            CharacterEducation = new ALabeledScrollbar(new ASize(dWidth, 80)) { Parent = this, Text = "Очки образованности: ", MinValue = 1, MaxValue = maxPoint };
            CharacterMartialSkills = new ALabeledScrollbar(new ASize(dWidth, 80)) { Parent = this, Text = "Очки военных навыков: ", MinValue = 1, MaxValue = maxPoint };
            CharacterHealth = new ALabeledScrollbar(new ASize(dWidth, 80)) { Parent = this, Text = "Очки здоровья: ", MinValue = 1, MaxValue = maxPoint };
            CharacterFertility = new ALabeledScrollbar(new ASize(dWidth, 80)) { Parent = this, Text = "Очки плодовитости: ", MinValue = 1, MaxValue = maxPoint };

            Male.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
            Female.BorderColor = GraphicsExtension.DefaultBorderColor;

            CharacterAttractiveness.ValueChange += (value) =>
            {
                if (value - Attractiveness <= freePoints && value >= 1)
                {
                    freePoints -= value - Attractiveness;
                    Attractiveness = value;
                    UpdateMaxValue();
                }
            };

            CharacterEducation.ValueChange += (value) =>
            {
                if (value - Education <= freePoints && value >= 1)
                {
                    freePoints -= value - Education;
                    Education = value;
                    UpdateMaxValue();
                }
            };

            CharacterMartialSkills.ValueChange += (value) =>
            {
                if (value - MartialSkills <= freePoints && value >= 1)
                {
                    freePoints -= value - MartialSkills;
                    MartialSkills = value;
                    UpdateMaxValue();
                }
            };

            CharacterHealth.ValueChange += (value) =>
            {
                if (value - Health <= freePoints && value >= 1)
                {
                    freePoints -= value - Health;
                    Health = value;
                    UpdateMaxValue();
                }
            };

            CharacterFertility.ValueChange += (value) =>
            {
                if (value - Fertility <= freePoints && value >= 1)
                {
                    freePoints -= value - Fertility;
                    Fertility = value;
                    UpdateMaxValue();
                }
            };

            Male.MouseClickEvent += (state, mstate) =>
            {
                SexType = ASexType.Male;
                Male.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
                Female.BorderColor = GraphicsExtension.DefaultBorderColor;
            };

            Female.MouseClickEvent += (state, mstate) =>
            {
                SexType = ASexType.Female;
                Male.BorderColor = GraphicsExtension.DefaultBorderColor;
                Female.BorderColor = GraphicsExtension.DefaultDarkBorderColor;
            };
            
            Done.MouseClickEvent += (state, mstate) =>
            {
                DoneEvent?.Invoke(PlayerName.Text, CharacterName.Text, CharacterFamilyName.Text, - CharacterAge.Value, SexType, Attractiveness, Education, MartialSkills, Health, Fertility);
            };
            
            RandomCharacter.MouseClickEvent += (state, mstate) =>
            {
                Update(CreateRandomCharacter(), PlayerName.Text);
            };

            StatsContainer = new AListContainer(PlayerName.Location + new APoint(PlayerName.Width + 10, 0), this);

            StatsContainer.Add(Points);
            StatsContainer.Add(CharacterAttractiveness);
            StatsContainer.Add(CharacterEducation);
            StatsContainer.Add(CharacterMartialSkills);
            StatsContainer.Add(CharacterHealth);
            StatsContainer.Add(CharacterFertility);

            StatsContainer.Update();

        }

        private void UpdateMaxValue()
        {

            CharacterAttractiveness.MaxValue = CharacterAttractiveness.Value + freePoints;
            CharacterEducation.MaxValue = CharacterEducation.Value + freePoints;
            CharacterMartialSkills.MaxValue = CharacterMartialSkills.Value + freePoints;
            CharacterHealth.MaxValue = CharacterHealth.Value + freePoints;
            CharacterFertility.MaxValue = CharacterFertility.Value + freePoints;

            Points.Text = "Свободные очки: " + freePoints;

        }

        private ICharacter CreateRandomCharacter()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return new ACharacter(GameExtension.CharacterName(SexType), GameExtension.DefaultFamily[random.Next(GameExtension.DefaultFamily.Count)], SexType, random.Next(-55, -16), 0, 0, 0, 0);
        }

        public void Update() => Update(CreateRandomCharacter(), Environment.UserName);

        public void Update(ICharacter character, string name)
        {

            PlayerName.Text = name;

            CharacterName.Text = character.Name;
            CharacterFamilyName.Text = character.FamilyName;

            CharacterAge.Value = character.Age(0);

            freePoints = 0;
            Points.Text = "Свободные очки: " + freePoints;

            Attractiveness = character.Attractiveness;
            Education = character.Education;
            MartialSkills = character.MartialSkills;
            Health = character.Health;
            Fertility = character.Fertility;
            /*
            CharacterAttractiveness.MaxValue = CharacterAttractiveness.MinValue + 1;
            CharacterEducation.MaxValue = CharacterEducation.MinValue + 1;
            CharacterMartialSkills.MaxValue = CharacterMartialSkills.MinValue + 1;
            CharacterHealth.MaxValue = CharacterHealth.MinValue + 1;
            CharacterFertility.MaxValue = CharacterFertility.MinValue + 1;
            */
            CharacterAttractiveness.Value = character.Attractiveness;
            CharacterEducation.Value = character.Education;
            CharacterMartialSkills.Value = character.MartialSkills;
            CharacterHealth.Value = character.Health;
            CharacterFertility.Value = character.Fertility;

            CharacterAttractiveness.MaxValue = character.Attractiveness;
            CharacterEducation.MaxValue = character.Education;
            CharacterMartialSkills.MaxValue = character.MartialSkills;
            CharacterHealth.MaxValue = character.Health;
            CharacterFertility.MaxValue = character.Fertility;

            StatsContainer.Update();

        }

    }
}
