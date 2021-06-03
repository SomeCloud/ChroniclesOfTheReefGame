using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;
using GameLibrary;
using GameLibrary.Message;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{
    public class AMessageForm: AForm
    {

        public delegate void OnDone();
        public delegate void OnRenouncement();

        public event OnDone DoneEvent;
        public event OnRenouncement RenouncementEvent;

        private MessagePanel MessagePanel;

        private IMessage Message;

        public AMessageForm(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            MessagePanel = new MessagePanel(Content.Size - 2) { Location = new APoint(1, 1) };

            MessagePanel.DoneEvent += () => {
                Message.Recipient.RemoveMessage(Message);
                DoneEvent?.Invoke();
                Hide();
            };

            MessagePanel.RenouncementEvent += () => {
                Message.Recipient.RemoveMessage(Message);
                RenouncementEvent?.Invoke();
                Hide();
            };

            Add(MessagePanel);

        }

        public void Update(IMessage message)
        {
            MessagePanel.Update(message);
        }

        public void Hide() => Enabled = false;

        public void Show(IMessage message)
        {
            Enabled = true;
            Update(message);

            Text = message.Header + ": " + message.Sender.Name;

            Message = message;

        }

    }
}
