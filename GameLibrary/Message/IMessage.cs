using System;
using System.Collections.Generic;
using System.Text;

using GameLibrary.Player;

namespace GameLibrary.Message
{
    public interface IMessage
    {

        public IPlayer Sender { get; }
        public IPlayer Recipient { get; }
        public string Header { get; }
        public string Text { get; }
        public Action Done { get; }
        public Action Renouncement { get; }

        public bool IsRenouncement { get; }

    }
}
