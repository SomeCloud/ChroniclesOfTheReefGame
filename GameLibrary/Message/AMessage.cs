using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Player;

namespace GameLibrary.Message
{
    [Serializable]
    public abstract class AMessage: IMessage
    {

        public IPlayer Sender { get; }
        public IPlayer Recipient { get; }
        public string Header { get; }
        public string Text { get; }

        public bool IsRenouncement { get; }

        public AMessage(IPlayer sender, IPlayer recipient, string header, string text, bool iSRenouncement)
        {

            Sender = sender;
            Recipient = recipient;
            Header = header;
            Text = text;
            IsRenouncement = iSRenouncement;

        }

        public abstract void Done(AGame game);
        public abstract void Renouncement(AGame game);

    }
}
