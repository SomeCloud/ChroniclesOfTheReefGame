﻿using System;
using System.Collections.Generic;
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

        private AClient Client;

        public event OnConnection ConnectionEvent;

        private LobbyList _LobbyList;

        private Dictionary<int, ARoom> rooms;

        private AButton _CreateNewRoom;
        private AButton _ConnectToGame;
        private AButton _Back;

        public AButton CreateNewRoom { get => _CreateNewRoom; }
        public AButton ConnectToGame { get => _ConnectToGame; }
        public AButton Back { get => _Back; }

        public APageMultiplayerGame(IPrimitive primitive) : base(primitive)
        {

            ASize lobbySize = new ASize((Parent.Width - 20) * 3 / 4, Parent.Height - GraphicsExtension.DefaultButtonSize.Height - 30);

            _Back = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Главное меню", Location = new APoint((Parent.Width - lobbySize.Width) / 2, 10) };
            _CreateNewRoom = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Создать новую игру", Location = _Back.Location + new APoint((lobbySize.Width - GraphicsExtension.DefaultMenuButtonSize.Width) / 2, 0) };
            _ConnectToGame = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Присоедениться к игре", Location = _Back.Location + new APoint(lobbySize.Width - GraphicsExtension.DefaultMenuButtonSize.Width, 0) };

            _LobbyList = new LobbyList(lobbySize) { Location = _Back.Location + new APoint(0, _Back.Height + 10) };

            Add(_CreateNewRoom);
            Add(_ConnectToGame);
            Add(_Back);

            Add(_LobbyList);

            rooms = new Dictionary<int, ARoom>();

            Client = new AClient("224.0.0.0", 8001);

            Client.Receive += (frame) =>
            {
                /*if (frame.MessageType.Equals(AMessageType.RoomInfo))
                {
                    ARoom room = (ARoom)frame.Data;
                    if (rooms.ContainsKey(frame.Id)) rooms[frame.Id] = room;
                    else rooms.Add(frame.Id, room);
                    _LobbyList.Update(rooms.Values);
                }*/
                test(frame);
            };

        }

        public void AddRoom(AFrame frame)
        {
            if (frame.MessageType.Equals(AMessageType.RoomInfo))
            {
                ARoom room = (ARoom)frame.Data;
                if (rooms.ContainsKey(frame.Id)) rooms[frame.Id] = room;
                else rooms.Add(frame.Id, room);
                _LobbyList.Update(rooms.Values);
            }
        }

        private async void test(AFrame frame)
        {
            await Task.Run(() => AddRoom(frame));
        }

        public void Hide()
        {

            Client.StopReceive();
            Visible = false;

        }

        public void Show()
        {

            Visible = true;

            Client.StartReceive("Receiver");

            Update();

        }

        public override void Update()
        {

            ASize lobbySize = new ASize((Parent.Width - 20) * 3 / 4, Parent.Height - GraphicsExtension.DefaultButtonSize.Height - 30);

            _Back.Location = new APoint((Parent.Width - lobbySize.Width) / 2, 10) ;
            _CreateNewRoom.Location = _Back.Location + new APoint((lobbySize.Width - GraphicsExtension.DefaultMenuButtonSize.Width) / 2, 0);
            _ConnectToGame.Location = _Back.Location + new APoint(lobbySize.Width - GraphicsExtension.DefaultMenuButtonSize.Width, 0);

            _LobbyList.Location = _Back.Location + new APoint(0, _Back.Height + 10);

        }

    }
}
