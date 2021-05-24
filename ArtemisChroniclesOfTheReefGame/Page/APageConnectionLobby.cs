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

    public delegate void OnDisconnection();

    class APageConnectionLobby : APage, IPage
    {

        public event OnBack BackEvent;

        public event OnDisconnection DisconnectionEvent;

        private AButton _Back;
        private LobbyPanel _LobbyPanel;

        private AServer Server;
        private AClient Client;

        bool IsDisconnect;

        private ARoom Room;
        private RPlayer Player;

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
                Client.ReceiveFrame();
                if (IsDisconnect) Server?.SendFrame(new AFrame(Room.Id, Player, AMessageType.Disconnection, "224.0.0.0", Client.LocalIPAddress()));
            };

            _Back.MouseClickEvent += (state, mstate) => {
                IsDisconnect = true;
                BackEvent?.Invoke();
            };

            Client.Receive += (frame) =>
            {
                Server?.StopSending();
                switch (frame.MessageType)
                {
                    case AMessageType.ServerDisconnection:
                        DisconnectionEvent?.Invoke();
                        break;
                    case AMessageType.Confirm:
                        if (frame.Data is RPlayer player && player is object)
                        {
                            IsDisconnect = false;
                            DisconnectionEvent?.Invoke();
                        }
                        break;
                    case AMessageType.RoomInfo:
                        _LobbyPanel.Update(frame.Data as ARoom);
                        break;
                }
            };
        }

        public void Hide()
        {

            Client.StopReceive();
            Visible = false;

        }

        public void Show(ARoom room, RPlayer player)
        {

            Visible = true;
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
