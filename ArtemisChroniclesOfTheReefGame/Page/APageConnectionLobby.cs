using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using NetLibrary;

using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using ArtemisChroniclesOfTheReefGame.Interface;

namespace ArtemisChroniclesOfTheReefGame.Page
{

    public delegate void OnDisconnection();

    class APageConnectionLobby : APage, IPage
    {

        public delegate void OnStartGame(ARoom room, RPlayer player);

        public event OnStartGame StartGameEvent;
        public event OnBack BackEvent;

        public event OnDisconnection DisconnectionEvent;

        private AButton _Back;
        private LobbyPanel _LobbyPanel;

        private AServer Server;
        private AClient Client;

        bool IsDisconnect;

        private ARoom Room;
        private RPlayer Player;

        Thread Receiver;
        Thread Sender;

        private bool IsReceive;
        private bool ResetReceive;

        public APageConnectionLobby(IPrimitive primitive) : base(primitive)
        {

            Client = new AClient("224.0.0.0", 8001);
            Server = new AServer("224.0.0.0", 8002);

            _Back = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Отключиться", Location = new APoint(10, 10) };
            _LobbyPanel = new LobbyPanel(new ASize(Parent.Width - 20, Parent.Height - _Back.Height - 30)) { Location = _Back.Location + new APoint(0, _Back.Height + 10), IsCounting = true, DTimer = 1 };

            Add(_Back);
            Add(_LobbyPanel);

            _LobbyPanel.TimeEvent += () =>
            {
                if (IsDisconnect)
                {
                    Sender?.Abort();
                    Sender = new Thread(() => Server?.SendFrame(new AFrame(Room.Id, Player, AMessageType.Disconnection, "224.0.0.0", Client.LocalIPAddress()))) { Name = "Connection-Sender", IsBackground = true };
                    Sender.Start();
                    //Server?.SendFrame(new AFrame(Room.Id, Player, AMessageType.Disconnection, "224.0.0.0", Client.LocalIPAddress()));
                }
            };

            _LobbyPanel.TimeEvent += () =>
            {
                if (IsReceive || (ResetReceive && !Client.InReceive))
                {
                    Receiver?.Abort();
                    Receiver = new Thread(() => Client.ReceiveResult()) { Name = "Connection-Receiver", IsBackground = true };
                    Receiver.Start();
                    IsReceive = ResetReceive = false;
                }
                else
                {
                    ResetReceive = !IsReceive;
                }
            };

            _LobbyPanel.DrawEvent += () =>
            {
                if (Client.IsComleted) OnReceive(Client.Result);
            };

            _Back.MouseClickEvent += (state, mstate) => {
                IsDisconnect = true;
                BackEvent?.Invoke();
            };

        }

        private void OnReceive(AFrame frame)
        {
            switch (frame.MessageType)
            {
                case AMessageType.ServerDisconnection:
                    DisconnectionEvent?.Invoke();
                    break;
                case AMessageType.RoomInfo:
                    ARoom room = frame.Data as ARoom;
                    if (room is object)
                    {
                        if (!room.Players.Contains(Player))
                        {
                            IsDisconnect = false;
                            DisconnectionEvent?.Invoke();
                        }
                        else if (room.GameStatus.Equals(AGameStatus.Game))
                        {
                            StartGameEvent?.Invoke(room, Player);
                        }
                    }
                    _LobbyPanel.Update(room);
                    break;
            }
            IsReceive = true;
            ResetReceive = false;
        }

        public void Hide()
        {

            Client.StopReceive();

            Visible = false;
            IsReceive = false;
            ResetReceive = false;

        }

        public void Show(ARoom room, RPlayer player)
        {

            Visible = true;
            IsReceive = true;
            ResetReceive = false;
            Update();

            Room = room;
            Player = player;

            Client.StartReceive("Receiver");

            _LobbyPanel.Update(room);

        }

        public override void Update()
        {

            _Back.Location = new APoint(10, 10);
            _LobbyPanel.Location = _Back.Location + new APoint(0, _Back.Height + 10);

        }

    }

}
