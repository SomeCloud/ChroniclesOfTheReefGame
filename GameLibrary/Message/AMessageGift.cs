using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Player;

namespace GameLibrary.Message
{
    [Serializable]
    public class AMessageGift: AMessage, IMessage
    {

        private int Gift;

        public AMessageGift(IPlayer sender, IPlayer recipient, string header, string text, bool iSRenouncement, int gift): base(sender, recipient, header, text, iSRenouncement)
        {
            Gift = gift;
        }

        public override void Done(AGame game)
        {
            game.GetPlayer(Recipient.Name).ChangeCoffers(Gift);
        }

        public override void Renouncement(AGame game)
        {
            game.GetPlayer(Sender.Name).ChangeCoffers(Gift);
        }

    }
}
