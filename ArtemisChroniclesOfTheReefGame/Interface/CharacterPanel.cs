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
using GameLibrary.Player;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class CharacterPanel : AForm
    {

        public delegate void OnUpdate(ICharacter character);

        public event OnUpdate UpdateEvent;

        private CharacterMiniPanel CharacterMiniPanel;

        private AGame Game;
        private ICharacter Character;

        public CharacterPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
        }

        public CharacterPanel(AGame game, ASize size, IPrimitiveTexture primitiveTexture) : base(size, primitiveTexture)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            CharacterMiniPanel = new CharacterMiniPanel(Game, Content.Size - 2) { Location = new APoint(1, 1) };

            CharacterMiniPanel.UpdateEvent += () =>
            {
                if (Character is object)
                {
                    UpdateEvent?.Invoke(Character);
                    Update();
                }
            };

            Add(CharacterMiniPanel);

        }

        public void Update()
        {
            if (Character is object) Update(Character);
        }

        public void Update(ICharacter character)
        {
            Character = character;
            CharacterMiniPanel.Update(character);
        }

        public void Hide() => Enabled = false;

        public void Show(ICharacter character)
        {
            Enabled = true;
            Update(character);

            Text = character.FullName;
        }


    }
}
