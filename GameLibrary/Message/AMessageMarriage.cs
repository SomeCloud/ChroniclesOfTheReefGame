using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Player;
using GameLibrary.Character;

namespace GameLibrary.Message
{
    [Serializable]
    public class AMessageMarriage : AMessage, IMessage
    {

        private ICharacter Character;
        private ICharacter Spouse;
        private bool IsMatrilinearMarriage;

        public AMessageMarriage(IPlayer sender, IPlayer recipient, string header, string text, ICharacter character, ICharacter spouse, bool isMatrilinearMarriage) : base(sender, recipient, header, text, true) {
            Character = character;
            Spouse = spouse;
            IsMatrilinearMarriage = isMatrilinearMarriage;
        }

        public override void Done(AGame game)
        {
            game.Marry(Character, Spouse, IsMatrilinearMarriage);
        }

        public override void Renouncement(AGame game)
        {
            // nothing
        }

    }
}