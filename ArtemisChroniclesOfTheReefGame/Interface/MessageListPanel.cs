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
    public class MessageListPanel: AScrolleredPanel
    {

        public delegate void OnExtraButtonClick(IMessage message);
        public delegate void OnSelect(IMessage message);

        public event OnExtraButtonClick ExtraButtonClickEvent;
        public event OnSelect SelectEvent;

        private Dictionary<IMessage, AExtendedButton> MessagesList;

        public MessageListPanel(ASize size): base(AScrollbarAlign.Vertical, size)
        {
            MessagesList = new Dictionary<IMessage, AExtendedButton>();
        }

        public override void Initialize()
        {

            base.Initialize();

            Text = "Почта правителя";

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

        }

        public void UpdtaeContent(IEnumerable<IMessage> messages)
        {
            foreach (IMessage message in MessagesList.Keys.ToList())
                if (!messages.Contains(message))
                {
                    Remove(MessagesList[message]);
                    MessagesList.Remove(message);
                }
        }

        public void Update(IEnumerable<IMessage> messages)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, 0);

            foreach (var e in MessagesList) e.Value.Enabled = false;

            foreach (IMessage message in messages)
            {
                AExtendedButton button;

                if (MessagesList.ContainsKey(message))
                {
                    button = MessagesList[message];
                    button.Enabled = true;

                    string text = message.Header;
                    if (!button.Text.Equals(text)) button.Text = text;
                }
                else
                {
                    button = new AExtendedButton(new ASize(ContentSize.Width - 20, 50));

                    Add(button);

                    button.Text = message.Header;
                    button.ExtraButtonText = "x";

                    MessagesList.Add(message, button);

                    button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                    button.ButtonClick += () => { SelectEvent?.Invoke(message); };
                    button.ExtraButtonClick += () => { ExtraButtonClickEvent?.Invoke(message); };

                }
                if (button.Enabled)
                {
                    button.Location = last + new APoint(0, button.Height + 10);
                    last = button.Location;
                }
            }

            ContentSize = new ASize(ContentSize.Width, MessagesList.Count > 0 ? last.Y + 60 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;


        }

    }
}
