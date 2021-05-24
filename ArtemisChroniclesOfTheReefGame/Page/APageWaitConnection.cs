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
    public class APageWaitConnection : APage, IPage
    {

        public event OnBack BackEvent;
        public event OnConnection ConnectionEvent;

        private AServer Server;
        private AClient Client;

        private RPlayer PlayerInfo;

        public RPlayer Player => PlayerInfo;

        private AEmptyPanel _Status;
        private AButton _Back;

        public APageWaitConnection(IPrimitive primitive) : base(primitive)
        {

            _Status = new AEmptyPanel(new ASize(Parent.Width - 20, (Parent.Height - 30 - GraphicsExtension.DefaultButtonSize.Height) * 3 / 4)) { Location = new APoint(10, 10), IsCounting = true, DTimer = 1 };
            _Back = new AButton(GraphicsExtension.DefaultButtonSize) { Text = "Отмена", Location = new APoint((Parent.Width - GraphicsExtension.DefaultButtonSize.Width) / 2, _Status.Y + _Status.Height + (Parent.Height - _Status.Height) / 2) };

            Add(_Status);
            Add(_Back);

            _Status.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 16);

            _Back.MouseClickEvent += (state, mstate) =>
            {
                Server?.StopSending();
                Client?.StopReceive();
                BackEvent?.Invoke();
            };

            Client = new AClient("224.0.0.0", 8001);

            _Status.TimeEvent += () =>
            {
                Client.ReceiveFrame();
            };

            Client.Receive += (frame) =>
            {
                if (frame.DestinationAdress.Equals(Client.LocalIPAddress()))
                {
                    switch (frame.MessageType)
                    {
                        case AMessageType.Confirm:
                            ARoom room = frame.Data as ARoom;
                            if (room.Players.Contains(PlayerInfo))
                            {
                                Server?.StopSending();
                                ConnectionEvent?.Invoke(room);
                            }
                            break;
                        case AMessageType.Renouncement:
                            RPlayer player = frame.Data as RPlayer;
                            if (player.Equals(PlayerInfo))
                            {
                                Server?.StopSending();
                                BackEvent?.Invoke();
                                Client.StopReceive();
                            }
                            break;
                    }
                }
            };

        }

        public void Hide()
        {

            Client.StopReceive();
            Server.StopSending();

            Visible = false;

        }

        public void Show(string ip, int port, string name)
        {

            Visible = true;

            _Status.Text = "Ожидаем ответа от " + ip + ":" + port + " на подключение для игрока " + name;

            Server?.StopSending();
            Server = new AServer(ip, port);

            PlayerInfo = new RPlayer(name, Client.LocalIPAddress());

            Client.StartReceive("Receiver");

            Server.StartSending(new AFrame(-1, PlayerInfo, AMessageType.Connection, Client.LocalIPAddress(), ip), false, "Connection");

            //Client.StartReceive("Receiver");

            Update();

        }

        public override void Update()
        {

            _Status.Size = Parent.Size;
            _Status.Location = new APoint(0, 0);

        }


    }
}
