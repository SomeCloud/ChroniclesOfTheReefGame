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

        private AFrame Frame;

        private bool IsSend;

        private int Counter;
        private int WaitCount = 10;

        private string Ip;
        private int Port;

        public APageWaitConnection(IPrimitive primitive) : base(primitive)
        {

            _Status = new AEmptyPanel(new ASize(Parent.Width - 20, (Parent.Height - 30 - GraphicsExtension.DefaultButtonSize.Height) * 3 / 4)) { Location = new APoint(10, 10), IsCounting = true, DTimer = 1 };
            _Back = new AButton(GraphicsExtension.DefaultButtonSize) { Text = "Отмена", Location = new APoint((Parent.Width - GraphicsExtension.DefaultButtonSize.Width) / 2, _Status.Y + _Status.Height + (Parent.Height - _Status.Height) / 2) };

            Add(_Status);
            Add(_Back);

            _Status.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 16);

            _Back.MouseClickEvent += (state, mstate) =>
            {
                IsSend = false;
                Client?.StopReceive();
                BackEvent?.Invoke();
            };

            Client = new AClient("224.0.0.0", 8001);

            _Status.TimeEvent += () =>
            {
                Client.ReceiveFrame();
                if (IsSend) Server.SendFrame(Frame);
            };

            Client.Receive += (frame) =>
            {
                if (frame.MessageType.Equals(AMessageType.RoomInfo))
                {
                    ARoom room = frame.Data as ARoom;
                    if (room is object && room.Players.Contains(PlayerInfo))
                    {
                        Client.StopReceive();
                        IsSend = false;
                        ConnectionEvent?.Invoke(room);
                    }
                    else
                    {
                        if (Counter >= WaitCount)
                        {
                            Client.StopReceive();
                            BackEvent?.Invoke();
                        }
                        else Counter++;

                        _Status.Text = "Ожидаем ответа от " + Ip + ":" + Port + " на подключение для игрока " + PlayerInfo.Name + "\nПопыток подключиться осталось: " + (WaitCount - Counter) + "/" + WaitCount;

                    }
                }
            };

        }

        public void Hide()
        {

            Client.StopReceive();
            IsSend = false;

            Visible = false;

        }

        public void Show(string ip, int port, string name)
        {

            Visible = true;

            _Status.Text = "Ожидаем ответа от " + ip + ":" + port + " на подключение для игрока " + name + "\nПопыток подключиться осталось: " + (WaitCount - Counter) + "/" + Counter;

            Server?.StopSending();
            Server = new AServer(ip, port);

            IsSend = true;

            Ip = ip;
            Port = port;
            PlayerInfo = new RPlayer(name, Client.LocalIPAddress());

            Counter = 0;

            Client.StartReceive("Receiver");

            Frame = new AFrame(-1, PlayerInfo, AMessageType.Connection, Client.LocalIPAddress(), ip);

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
