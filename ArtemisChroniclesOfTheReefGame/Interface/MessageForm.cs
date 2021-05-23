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
using GameLibrary.Message;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class MessageForm: AForm
    {

        public delegate void OnDone();
        public delegate void OnRenouncement();

        public event OnDone DoneEvent;
        public event OnRenouncement RenouncementEvent;

        private MessagePanel MessagePanel;

        private IMessage Message;

        public MessageForm(ASize size) : base(size)
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
