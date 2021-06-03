using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Player;

namespace GameLibrary.Message
{
    [Serializable]
    public class AMessage: IMessage
    {

        public IPlayer Sender { get; }
        public IPlayer Recipient { get; }
        public string Header { get; }
        public string Text { get; }
        //public Action Done { get; }
        //public Action Renouncement { get; }

        public bool IsRenouncement { get; }

        public AMessage(IPlayer sender, IPlayer recipient, string header, string text, Action done, bool iSRenouncement)
        {

            Sender = sender;
            Recipient = recipient;
            Header = header;
            Text = text;
            //Done = done;
            IsRenouncement = iSRenouncement;

        }

        public AMessage(IPlayer sender, IPlayer recipient, string header, string text, Action done, bool iSRenouncement, Action renouncement) : this(sender, recipient, header, text, done, iSRenouncement)
        {
            //Renouncement = renouncement;
        }

    }
}
