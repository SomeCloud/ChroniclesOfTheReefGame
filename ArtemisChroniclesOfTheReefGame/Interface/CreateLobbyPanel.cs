using System;
using System.Linq;
using System.Collections.Generic;

using NetLibrary;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class CreateLobbyPanel : APanel
    {

        public delegate void OnProcess(AFrame frame);
        public delegate void OnChange(ARoom room);
        public delegate void OnPlayerSelect(RPlayer player);

        public event OnProcess ProcessEvent;
        public event OnChange ChangeEvent;
        public event OnPlayerSelect PlayerSelectEvent;

        private ATextBox Header;
        private AEmptyPanel RoomInfo;
        private LobbyPlyersList PlyersList;

        private ARoom _Room;

        public ARoom Room => _Room;

        public CreateLobbyPanel(ASize size): base(size)
        {
            _Room = new ARoom("", 1, 2);
        }

        public override void Initialize()
        {

            base.Initialize();

            Header = new ATextBox(new ASize(Width - 20, 100)) { Parent = this, Text = _Room.Name, Location = new APoint(10, 10) };
            RoomInfo = new AEmptyPanel(new ASize(Width - 20, 100)) { Parent = this, Text = "Количество игроков: " + _Room.Players.Count + "/" + _Room.PlayersCount, Location = Header.Location + new APoint(0, Header.Height + 10) };
            PlyersList = new LobbyPlyersList(new ASize(Width - 20, Height - 240)) { Parent = this, Location = RoomInfo.Location + new APoint(0, RoomInfo.Height + 10) };

            Header.EndEditEvent += (text) => {
                if (text.Length > 0 && !text.Equals(" ") && !_Room.Name.Equals(text))
                {
                    _Room.SetName(text);
                    ChangeEvent?.Invoke(_Room);
                } };

            PlyersList.ExtraSelectEvent += (player) => {
                PlayerSelectEvent?.Invoke(player);
            };

        }

        public void ProcessFrame(AFrame frame)
        {
            RPlayer player;
            switch (frame.MessageType)
            {
                case AMessageType.Connection:
                    player = frame.Data as RPlayer;
                    if (Room.Connect(player)) ProcessEvent?.Invoke(new AFrame(Room.Id, Room, AMessageType.Confirm, frame.DestinationAdress, frame.SourceAdress)); 
                    else ProcessEvent?.Invoke(new AFrame(Room.Id, player, AMessageType.Renouncement, frame.DestinationAdress, frame.SourceAdress));
                    break;
                case AMessageType.Disconnection:
                    player = frame.Data as RPlayer;
                    if (Room.Disconnect(player)) ProcessEvent?.Invoke(new AFrame(Room.Id, player, AMessageType.Confirm, frame.DestinationAdress, frame.SourceAdress));
                    else ProcessEvent?.Invoke(new AFrame(Room.Id, Room, AMessageType.Renouncement, frame.DestinationAdress, frame.SourceAdress));
                    break;
                default:
                    ProcessEvent?.Invoke(new AFrame(Room.Id, Room, AMessageType.RoomInfo, "224.0.0.0", frame.SourceAdress));
                    break;
            }
            PlyersList.Update(_Room.Players);
            RoomInfo.Text = "Количество игроков: " + _Room.Players.Count + "/" + _Room.PlayersCount;
        }

        public void Update()
        {

            PlyersList.Update(_Room.Players);

        }

        public void Hide() => Enabled = false;

        public void Show(int id, string name)
        {

            Enabled = true;
            _Room = new ARoom(name + "'s Game", id, 2);
            Header.Text = _Room.Name;
            ChangeEvent?.Invoke(_Room);
        }

    }
}
