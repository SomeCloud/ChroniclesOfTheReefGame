using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Player;

namespace GameLibrary.Message
{
    [Serializable]
    public class AMessageUnion : AMessage, IMessage
    {

        public AMessageUnion(IPlayer sender, IPlayer recipient, string header, string text) : base(sender, recipient, header, text, true) { }

        public override void Done(AGame game)
        {
            game.SetRelationship(game.GetPlayer(Sender.Name), game.GetPlayer(Recipient.Name), ARelationshipType.Union);
        }

        public override void Renouncement(AGame game)
        {
            // nothing
        }

    }
}
