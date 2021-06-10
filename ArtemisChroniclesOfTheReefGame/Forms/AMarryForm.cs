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
    public class AMarryForm: AForm
    {

        public delegate void OnSelect();
        public delegate void OnDone(ICharacter firstCandidate, ICharacter secondCandidate, bool isMatrilinearMarriage);

        public event OnSelect FirstCandidateSelectEvent;
        public event OnSelect SecondCandidateSelectEvent;

        public event OnDone DoneEvent;

        private AMarryPanel MarryPanel;

        private ACharactersForm _CharactersForm;
        public ACharactersForm CharactersForm => _CharactersForm;
        private GameData GameData;

        private bool isFirstCandidateSelect;
        private bool isSecondCandidateSelect;

        public bool CharactersFormIsActive => isFirstCandidateSelect || isSecondCandidateSelect;

        private ACharactersForm.OnSelect CharacterSelect;

        public AMarryForm(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            MarryPanel = new AMarryPanel(Content.Size - 2) { Location = new APoint(1, 1) };

            CloseEvent += () => {

                isFirstCandidateSelect = false;
                isSecondCandidateSelect = false;

            };

            MarryPanel.DoneEvent += (firstCandidate, secondCandidate, isMatrilinearMarriage) =>
            {
                DoneEvent?.Invoke(firstCandidate, secondCandidate, isMatrilinearMarriage);
                isFirstCandidateSelect = false;
                isSecondCandidateSelect = false;
                _CharactersForm.Hide();
                Hide();
            };

            MarryPanel.FirstCandidateSelectEvent += () =>
            {
                _CharactersForm.Show(GameData.Characters.Values.SelectMany(x => x).OrderBy(x => x.OwnerId.Equals(GameData.ActivePlayer.Id)).Where(x => GameData.IsRelative(x, MarryPanel.SecondCandidate).Equals(false) && !MarryPanel.SecondCandidate.SexType.Equals(x.SexType) && x.Age(GameData.CurrentTurn) > 15 && !x.IsMarried), GameData);
                isFirstCandidateSelect = true;
                isSecondCandidateSelect = false;
                FirstCandidateSelectEvent?.Invoke();
            };

            MarryPanel.SecondCandidateSelectEvent += () =>
            {
                _CharactersForm.Show(GameData.Characters.Values.SelectMany(x => x).OrderBy(x => x.OwnerId.Equals(GameData.ActivePlayer.Id)).Where(x => GameData.IsRelative(MarryPanel.FirstCandidate, x).Equals(false) && !MarryPanel.FirstCandidate.SexType.Equals(x.SexType) && x.Age(GameData.CurrentTurn) > 15 && !x.IsMarried), GameData);
                isFirstCandidateSelect = false;
                isSecondCandidateSelect = true;
                SecondCandidateSelectEvent?.Invoke();
            };

            CharacterSelect = new ACharactersForm.OnSelect(
                (character) =>
                {
                    if (isFirstCandidateSelect)
                    {
                        MarryPanel.SetFirstCandidate(character);
                        _CharactersForm.Hide();
                    }
                    else if (isSecondCandidateSelect)
                    {
                        MarryPanel.SetSecondCandidate(character);
                        _CharactersForm.Hide();
                    }
                }
                );

            Add(MarryPanel);

        }

        public void SetCharactersForm(ACharactersForm charactersForm)
        {
            if (_CharactersForm is object) _CharactersForm.SelectEvent -= CharacterSelect;
            _CharactersForm = charactersForm;
            _CharactersForm.SelectEvent += CharacterSelect;
        }

        public void Update(ICharacter character, GameData gameData)
        {
            GameData = gameData;
            MarryPanel.Update();
            MarryPanel.SetFirstCandidate(character);
        }

        public void Hide()
        {

            isFirstCandidateSelect = false;
            isSecondCandidateSelect = false;

            Enabled = false;
        }

        public void Show(ICharacter character, GameData gameData)
        {
            Enabled = true;
            Update(character, gameData);

            Text = "Заключение брачаного союза";

        }

    }
}
