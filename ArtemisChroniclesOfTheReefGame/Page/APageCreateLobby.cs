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
    public class APageCreateLobby : APage, IPage
    {

        public event OnBack BackEvent;

        private AButton _Back;
        private CreateLobbyPanel _LobbyPanel;

        private AServer Server;
        private AClient Client;

        private AFrame Frame;

        private bool IsSend;
        private bool IsReceive;

        public APageCreateLobby(IPrimitive primitive) : base(primitive)
        {

            Client = new AClient("224.0.0.0", 8002);
            Server = new AServer("224.0.0.0", 8001);

            _Back = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Отключиться", Location = new APoint(10, 10) };
            _LobbyPanel = new CreateLobbyPanel(new ASize(Parent.Width - 20, Parent.Height - _Back.Height - 30)) { Location = _Back.Location + new APoint(0, _Back.Height + 10), IsCounting = true, DTimer = 1 };

            Add(_Back);
            Add(_LobbyPanel);

            _LobbyPanel.TimeEvent += () =>
            {
                if (IsReceive) Client.ReceiveFrame();
                if (IsSend) Server?.SendFrame(Frame);
            };

            _LobbyPanel.ChangeEvent += (room) =>
            {
                Frame = new AFrame(room.Id, room, AMessageType.RoomInfo, "224.0.0.0", Client.LocalIPAddress());
                IsSend = true;
            };

            Client.Receive += (frame) =>
            {
                _LobbyPanel.ProcessFrame(frame);
            };

            _Back.MouseClickEvent += (state, mstate) => {
                Frame = new AFrame(_LobbyPanel.Room.Id, _LobbyPanel.Room, AMessageType.ServerDisconnection, "224.0.0.0", Client.LocalIPAddress());
                BackEvent?.Invoke();
            };

            Frame = new AFrame(_LobbyPanel.Room.Id, _LobbyPanel.Room, AMessageType.RoomInfo, "224.0.0.0", Client.LocalIPAddress());

            IsSend = true;
            IsReceive = true;

        }

        public void Hide()
        {

            Client.StopReceive();
            Visible = false;

            IsSend = false;
            IsReceive = false;

        }

        public void Show(int id, string name)
        {

            Visible = true;
            Update();

            Client.StartReceive("Receiver");

            _LobbyPanel.Show(id, name);

            IsSend = true;
            IsReceive = true;

        }

        public override void Update()
        {

            _Back.Location = new APoint(10, 10);
            _LobbyPanel.Location = _Back.Location + new APoint(0, _Back.Height + 10);

        }

    }
}
