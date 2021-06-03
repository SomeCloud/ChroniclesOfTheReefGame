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
    public class ACharacterForm: AForm
    {

        public delegate void OnClick(ICharacter character);

        public event OnClick MarryEvent;
        public event OnClick AgreementEvent;
        public event OnClick HeirEvent;
        public event OnClick WarEvent;
        public event OnClick PeaceEvent;
        public event OnClick UnionEvent;
        public event OnClick BreakUnionEvent;

        private ACharacterPanel CharacterPanel;

        public ACharacterForm(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            CharacterPanel = new ACharacterPanel(Content.Size - 2) { Location = new APoint(1, 1) };

            CharacterPanel.MarryEvent += (character) => MarryEvent?.Invoke(character);
            CharacterPanel.AgreementEvent += (character) => AgreementEvent?.Invoke(character);
            CharacterPanel.HeirEvent += (character) => HeirEvent?.Invoke(character);
            CharacterPanel.WarEvent += (character) => WarEvent?.Invoke(character);
            CharacterPanel.PeaceEvent += (character) => PeaceEvent?.Invoke(character);
            CharacterPanel.UnionEvent += (character) => UnionEvent?.Invoke(character);
            CharacterPanel.BreakUnionEvent += (character) => BreakUnionEvent?.Invoke(character);

            Add(CharacterPanel);

        }

        public void Update(GameData gameData, ICharacter character)
        {

            CharacterPanel.Update(gameData, character);

        }

        public void Hide() => Enabled = false;

        public void Show(GameData gameData, ICharacter character)
        {

            Enabled = true;
            Update(gameData, character);

            Text = character.FullName;

        }

    }
}
