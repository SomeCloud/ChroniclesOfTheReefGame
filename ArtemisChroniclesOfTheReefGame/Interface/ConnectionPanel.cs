using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class ConnectionPanel: APanel
    {

        public delegate void OnResult(bool status, string ip, int port);

        public event OnResult ResultEvent;

        private AEmptyPanel MessagePanel;

        private ALabeledTextBox IPAdressTextBox;
        private ALabeledTextBox PortAdressTextBox;

        private AButton Done;
        private AButton Back;

        public ConnectionPanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            IPAdressTextBox = new ALabeledTextBox(new ASize(Width - 20, 100)) { Location = new APoint(10, 10) };
            PortAdressTextBox = new ALabeledTextBox(new ASize(Width - 20, 100)) { Location = IPAdressTextBox.Location + new APoint(0, IPAdressTextBox.Height + 10) };

            MessagePanel = new AEmptyPanel(new ASize(Width - 20, Height - 300)) { Location = PortAdressTextBox.Location + new APoint(0, PortAdressTextBox.Height + 10) };

            int dWidth = (Width - 30) / 2;

            Back = new AButton(new ASize(dWidth, 50)) { Location = MessagePanel.Location + new APoint(0, MessagePanel.Height + 10), Text = "Вернуться" };
            Done = new AButton(new ASize(dWidth, 50)) { Location = Back.Location + new APoint(Back.Width + 10, 0), Text = "Готово" };

            Done.MouseClickEvent += (state, mstate) =>
            {
                string text = "";
                if (IPAdressTextBox.Text.Equals(" ") || IPAdressTextBox.Text.Length.Equals(0)) text += "Ошибка: Неверно введен IP-адрес\n";
                if (PortAdressTextBox.Text.Equals(" ") || PortAdressTextBox.Text.Length.Equals(0) || !int.TryParse(PortAdressTextBox.Text, out int n)) text += "Ошибка: Неверно введен порт подключения";
                MessagePanel.Text = text.Length > 0? text: " ";
                if (text.Length.Equals(0)) ResultEvent?.Invoke(true, IPAdressTextBox.Text, int.Parse(PortAdressTextBox.Text));
            };
            Back.MouseClickEvent += (state, mstate) => ResultEvent?.Invoke(false, "", -1);

            Add(MessagePanel);

            Add(IPAdressTextBox);
            Add(PortAdressTextBox);

            MessagePanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            MessagePanel.TextLabel.TextColor = GraphicsExtension.ExtraColorRed;

            MessagePanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            IPAdressTextBox.LabelTextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            PortAdressTextBox.LabelTextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

            IPAdressTextBox.LabelText = "IP-Адрес для подключения";
            PortAdressTextBox.LabelText = "Порт для подключения";

            Add(Done);
            Add(Back);

            base.Initialize();

        }

        public void Update(string ip, string port)
        {
            IPAdressTextBox.Text = ip;
            PortAdressTextBox.Text = port;
            Enabled = true;
        }

    }
}
