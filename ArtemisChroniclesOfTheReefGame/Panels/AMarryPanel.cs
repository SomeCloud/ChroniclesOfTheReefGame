using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary.Player;
using GameLibrary.Character;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class AMarryPanel : AEmptyPanel
    {

        public delegate void OnSelect();
        public delegate void OnDone(ICharacter firstCandidate, ICharacter secondCandidate, bool isMatrilinearMarriage);

        public event OnSelect FirstCandidateSelectEvent;
        public event OnSelect SecondCandidateSelectEvent;

        public event OnDone DoneEvent;

        AButton FirstCandidateButton;
        AButton SecondCandidateButton;

        ALabeledCheckbox IsMatrilinearMarriage;

        private ICharacter _FirstCandidate;
        private ICharacter _SecondCandidate;

        public ICharacter FirstCandidate => _FirstCandidate;
        public ICharacter SecondCandidate => _SecondCandidate;

        AButton Done;

        public AMarryPanel(ASize size) : base(size)
        {

        }


        public override void Initialize()
        {

            base.Initialize();

            FirstCandidateButton = new AButton(new ASize(Width - 20, 60)) { Parent = this, Location = new APoint(10, 10), Text = "Никто не выбран" };
            SecondCandidateButton = new AButton(new ASize(Width - 20, 60)) { Parent = this, Location = FirstCandidateButton.Location + new APoint(0, FirstCandidateButton.Height + 10), Text = "Никто не выбран" };

            IsMatrilinearMarriage = new ALabeledCheckbox(Width - 20, TexturePack.Checkbox_Active, TexturePack.Checkbox_NotActive) { Parent = this, Location = SecondCandidateButton.Location + new APoint(0, SecondCandidateButton.Height + 10), Text = "Матрилинейный брак" };

            Done = new AButton(GraphicsExtension.DefaultButtonSize) { Parent = this, Location = new APoint((Width - GraphicsExtension.DefaultButtonSize.Width) / 2, IsMatrilinearMarriage.Y + IsMatrilinearMarriage.Height + 10), Text = "Подтвердить" };

            FirstCandidateButton.MouseClickEvent += (state, mstate) => FirstCandidateSelectEvent?.Invoke();
            SecondCandidateButton.MouseClickEvent += (state, mstate) => SecondCandidateSelectEvent?.Invoke();

            Done.MouseClickEvent += (state, mstate) =>
            {
                if (_FirstCandidate is object && _SecondCandidate is object) DoneEvent?.Invoke(_FirstCandidate, _SecondCandidate, IsMatrilinearMarriage.Value);
            };

        }

        public void SetFirstCandidate(ICharacter character)
        {
            _FirstCandidate = character;
            FirstCandidateButton.Text = character.FullName;
            if (_FirstCandidate is object && _SecondCandidate is object) Done.IsInteraction = true;
        }
        
        public void SetSecondCandidate(ICharacter character)
        {
            _SecondCandidate = character;
            SecondCandidateButton.Text = character.FullName;
            if (_FirstCandidate is object && _SecondCandidate is object) Done.IsInteraction = true;
        }

        public void Update()
        {
            FirstCandidateButton.Text = "Никто не выбран";
            SecondCandidateButton.Text = "Никто не выбран";
            _FirstCandidate = _SecondCandidate = null;
            IsMatrilinearMarriage.Value = false;
            Done.IsInteraction = false;
        }

    }
}
