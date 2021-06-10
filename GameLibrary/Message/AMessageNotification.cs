using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Player;

namespace GameLibrary.Message
{
    [Serializable]
    public class AMessageNotification : AMessage, IMessage
    {

        public AMessageNotification(IPlayer sender, IPlayer recipient, string header, string text) : base(sender, recipient, header, text, false) { }

        public override void Done(AGame game)
        {
            // nothing
        }

        public override void Renouncement(AGame game)
        {
            // nothing
        }

    }
}
