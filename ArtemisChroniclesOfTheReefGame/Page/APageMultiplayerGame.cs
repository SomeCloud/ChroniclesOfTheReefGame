using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Extension;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Character;
using GameLibrary.Unit;
using GameLibrary.Unit.Main;

using NetLibrary;

using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;
using OnMouseEvent = GraphicsLibrary.Interfaces.OnMouseEvent;

using ArtemisChroniclesOfTheReefGame.Interface;

namespace ArtemisChroniclesOfTheReefGame.Page
{

    public delegate void OnConnection(ARoom room);

    public class APageMultiplayerGame : APage, IPage
    {

        public delegate void OnCreateRoom(int id, string name);

        private AClient Client;

        public event OnConnection ConnectionEvent;
        public event OnResult ConnectEvent;

        public event OnCreateRoom CreateRoomEvent;

        private LobbyList _LobbyList;

        private ConcurrentDictionary<int, ARoom> rooms;

        private AButton _CreateNewRoom;
        private AButton _ConnectToGame;
        private AButton _Back;

        private ALabeledTextBox _PlayerName;

        public AButton CreateNewRoom { get => _CreateNewRoom; }
        public AButton ConnectToGame { get => _ConnectToGame; }
        public AButton Back { get => _Back; }

        private bool IsReceive;

        public string Name => _PlayerName.Text.Length.Equals(0) || _PlayerName.Text.Equals(" ") ? Environment.UserName : _PlayerName.Text;

        public APageMultiplayerGame(IPrimitive primitive) : base(primitive)
        {

            ASize lobbySize = new ASize((Parent.Width - 20) * 3 / 4, Parent.Height - GraphicsExtension.DefaultButtonSize.Height - 140);

            _Back = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Главное меню", Location = new APoint((Parent.Width - lobbySize.Width) / 2, 10) };
            _CreateNewRoom = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Создать новую игру", Location = _Back.Location + new APoint((lobbySize.Width - GraphicsExtension.DefaultMenuButtonSize.Width) / 2, 0) };
            _ConnectToGame = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Присоедениться к игре", Location = _Back.Location + new APoint(lobbySize.Width - GraphicsExtension.DefaultMenuButtonSize.Width, 0) };

            _PlayerName = new ALabeledTextBox(new ASize(lobbySize.Width, 100)) { Location = _Back.Location + new APoint(0, _Back.Height + 10) };

            _LobbyList = new LobbyList(lobbySize) { Location = _PlayerName.Location + new APoint(0, _PlayerName.Height + 10), IsCounting = true, DTimer = 1 };

            _LobbyList.DrawEvent += () =>
            {
                if (IsReceive) Client.ReceiveFrame();
            };

            Add(_CreateNewRoom);
            Add(_ConnectToGame);
            Add(_Back);

            Add(_PlayerName);

            Add(_LobbyList);

            _PlayerName.LabelText = "Имя игрока";
            _PlayerName.Text = Environment.UserName;

            rooms = new ConcurrentDictionary<int, ARoom>();

            Client = new AClient("224.0.0.0", 8001);

            _LobbyList.SelectLobbyEvent += (room) => {
                ConnectEvent?.Invoke("224.0.0.0", 8002, Name);
                ConnectionEvent?.Invoke(room);
            };

            Client.Receive += (frame) =>
            {
                if (frame.MessageType.Equals(AMessageType.RoomInfo))
                {
                    ARoom room = (ARoom)frame.Data;
                    if (rooms.ContainsKey(frame.Id)) rooms[frame.Id] = room;
                    else rooms.TryAdd(frame.Id, room);
                    _LobbyList.Update(rooms.Values);
                }
            };

            _CreateNewRoom.MouseClickEvent += (state, mstate) => {         
                CreateRoomEvent?.Invoke(rooms.Count > 0? rooms.Keys.Max() + 1: 1, Name);
            };

        }

        public void AddRoom(AFrame frame)
        {
            if (frame.MessageType.Equals(AMessageType.RoomInfo))
            {
                ARoom room = (ARoom)frame.Data;
                if (rooms.ContainsKey(frame.Id)) rooms[frame.Id] = room;
                else rooms.TryAdd(frame.Id, room);
                _LobbyList.Update(rooms.Values);
            }
        }

        public void Hide()
        {
            IsReceive = false;

            Client.StopReceive();

            Visible = false;

        }

        public void Show()
        {

            Visible = true;
            IsReceive = true;

            Client.StartReceive("Receiver");

            Update();

        }

        public override void Update()
        {

            ASize lobbySize = new ASize((Parent.Width - 20) * 3 / 4, Parent.Height - GraphicsExtension.DefaultButtonSize.Height - 30);

            _Back.Location = new APoint((Parent.Width - lobbySize.Width) / 2, 10) ;
            _CreateNewRoom.Location = _Back.Location + new APoint((lobbySize.Width - GraphicsExtension.DefaultMenuButtonSize.Width) / 2, 0);
            _ConnectToGame.Location = _Back.Location + new APoint(lobbySize.Width - GraphicsExtension.DefaultMenuButtonSize.Width, 0);

            _PlayerName.Location = _Back.Location + new APoint(0, _Back.Height + 10);

            _LobbyList.Location = _PlayerName.Location + new APoint(0, _PlayerName.Height + 10);

        }

    }
}
