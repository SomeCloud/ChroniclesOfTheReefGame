using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Character;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{
    public class ACharactersForm : AForm
    {

        public delegate void OnSelect(ICharacter character);
        public event OnSelect SelectEvent;

        private ACharactersList CharactersList;

        public ACharactersForm(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            CharactersList = new ACharactersList(Content.Size - 2) { Location = new APoint(1, 1) };

            CharactersList.SelectEvent += (character) => SelectEvent?.Invoke(character);

            Add(CharactersList);

        }

        public void Update(IEnumerable<ICharacter> characters, int currentTurn)
        {
            CharactersList.Update(characters, currentTurn);
        }

        public void Hide() => Enabled = false;

        public void Show(IEnumerable<ICharacter> characters, int currentTurn)
        {

            Enabled = true;
            Update(characters, currentTurn);

            Text = "Персонажи";

        }

    }
}
