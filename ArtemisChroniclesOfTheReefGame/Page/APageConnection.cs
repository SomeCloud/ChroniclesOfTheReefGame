using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using NetLibrary;

using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using ArtemisChroniclesOfTheReefGame.Interface;

namespace ArtemisChroniclesOfTheReefGame.Page
{

    public delegate void OnBack();

    public class APageConnection : APage, IPage
    {

        public delegate void OnResult(string ip, int port, string name);

        public event OnBack BackEvent;
        public event OnResult ConnectEvent;

        private ConnectionPanel _ConnectionPanel;
        private ALabeledTextBox _PlayerName;

        public APageConnection(IPrimitive primitive) : base(primitive)
        {

            ASize dSize = new ASize(Parent.Width / 2, Parent.Height / 2);

            _ConnectionPanel = new ConnectionPanel(dSize) { Location = ((Parent.Size - dSize) / 2).ToAPoint() };
            _PlayerName = new ALabeledTextBox(new ASize(dSize.Width, 100)) { Location = _ConnectionPanel.Location + new APoint(0, -110) };

            Add(_ConnectionPanel);
            Add(_PlayerName);

            _PlayerName.LabelText = "Имя игрока";
            _PlayerName.Text = Environment.UserName;

            _ConnectionPanel.ResultEvent += (status, ip, port) =>
            {
                if (_PlayerName.Text.Length.Equals(0) || _PlayerName.Text.Equals(" ")) _PlayerName.Text = Environment.UserName;
                if (status) ConnectEvent?.Invoke(ip, port, _PlayerName.Text);
                else BackEvent?.Invoke();
            };

        }
        public void Hide() => Visible = false;

        public void Show()
        {

            Visible = true;
            Update();

        }

        public override void Update()
        {

            ASize dSize = new ASize(Parent.Width / 2, Parent.Height / 2);

            if (_PlayerName.Text.Length.Equals(0) || _PlayerName.Text.Equals(" ")) _PlayerName.Text = Environment.UserName;
            _ConnectionPanel.Location = ((Parent.Size - dSize) / 2).ToAPoint();

        }


    }
}
