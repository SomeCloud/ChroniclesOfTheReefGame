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

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{
    public class APlayerCreateForm: AForm
    {

        public delegate void OnDone(string playerName, string characterName, string characterFamilyName, int birthdate, ASexType sexType, int attractiveness, int education, int martialSkills, int health, int fertility);

        public event OnDone DoneEvent;

        private APlayerCreatePanel PlayerCreatePanel;

        public APlayerCreateForm(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            PlayerCreatePanel = new APlayerCreatePanel(Content.Size - 2);

            PlayerCreatePanel.DoneEvent += (playerName, characterName, characterFamilyName, age, sexType, attractiveness, education, martialSkills, health, fertility) =>
            {
                DoneEvent?.Invoke(playerName, characterName, characterFamilyName, age, sexType, attractiveness, education, martialSkills, health, fertility);
                Hide();
            };

            Add(PlayerCreatePanel);

        }

        public void Update(ICharacter character, string name)
        {

            PlayerCreatePanel.Update(character, name);

        }

        public void Hide() => Enabled = false;

        public void Show()
        {

            Enabled = true;
            PlayerCreatePanel.Update();

            Text = "Новый игрок";

        }

        public void Show(ICharacter character, string name)
        {

            Enabled = true;
            Update(character, name);

            Text = "Игрок " + name;

        }

        public void Show(ICharacter character)
        {

            Enabled = true;
            Update(character, Environment.UserName);

            Text = character.FullName;

            Text = "Новый игрок";

        }

    }
}
