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

        public delegate void OnChange(ARoom room);

        public event OnChange ChangeEvent;
        public event OnChange StartGameEvent;

        private ATextBox Header;
        private AEmptyPanel RoomInfo;

        private ALabeledScrollbar MapHeight;
        private ALabeledScrollbar MapWidth;

        private LobbyPlyersList PlyersList;

        private AButton StartGame;

        private ARoom _Room;

        public ARoom Room => _Room;

        public CreateLobbyPanel(ASize size): base(size)
        {
            _Room = new ARoom("", 1, 2);
        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = Width * 3 / 4;

            Header = new ATextBox(new ASize(Width - 20, 80)) { Parent = this, Text = _Room.Name, Location = new APoint(10, 10) };

            PlyersList = new LobbyPlyersList(new ASize(dWidth - 30, Height - 110)) { Parent = this, Location = Header.Location + new APoint(0, Header.Height + 10) };

            RoomInfo = new AEmptyPanel(new ASize(Width - dWidth, 80)) { Parent = this, Text = "Количество игроков: " + _Room.Players.Count + "/" + _Room.PlayersCount, Location = PlyersList.Location + new APoint(PlyersList.Width + 10, 0) };

            MapHeight = new ALabeledScrollbar(new ASize(Width - dWidth, 80)) { Parent = this, Location = RoomInfo.Location + new APoint(0, RoomInfo.Height + 10), Text = "Высота карты: ", MinValue = 5, MaxValue = 15 };
            MapWidth = new ALabeledScrollbar(new ASize(Width - dWidth, 80)) { Parent = this, Location = MapHeight.Location + new APoint(0, MapHeight.Height + 10), Text = "Ширина карты: ", MinValue = 8, MaxValue = 30 };

            StartGame = new AButton(new ASize(Width - dWidth, 50)) { Parent = this, Location = MapWidth.Location + new APoint(0, MapWidth.Height + 10), Text = "Начать игру" };

            Header.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 16);
            RoomInfo.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 16);

            Header.EndEditEvent += (text) => {
                if (text.Length > 0 && !text.Equals(" ") && !_Room.Name.Equals(text))
                {
                    _Room.SetName(text);
                    ChangeEvent?.Invoke(_Room);
                } 
            };

            MapHeight.ValueChange += (value) =>
            {
                _Room.SetMapSize(new ASize(MapWidth.Value, value));
                ChangeEvent?.Invoke(_Room);
            };

            MapWidth.ValueChange += (value) =>
            {
                _Room.SetMapSize(new ASize(value, MapHeight.Value));
                ChangeEvent?.Invoke(_Room);
            };

            StartGame.MouseClickEvent += (state, mstate) => {
                //_Room.StartGame();
                StartGameEvent?.Invoke(_Room);
            };

            PlyersList.ExtraSelectEvent += (player) => {
                Room.Disconnect(player);
                ChangeEvent?.Invoke(_Room);
                PlyersList.Update(_Room.Players);
                RoomInfo.Text = "Количество игроков: " + _Room.Players.Count + "/" + _Room.PlayersCount;
            };

        }

        public void ProcessFrame(AFrame frame)
        {
            RPlayer player;
            switch (frame.MessageType)
            {
                case AMessageType.Connection:
                    player = frame.Data as RPlayer;
                    Room.Connect(player);
                    ChangeEvent?.Invoke(_Room);
                    break;
                case AMessageType.Disconnection:
                    player = frame.Data as RPlayer;
                    Room.Disconnect(player);
                    ChangeEvent?.Invoke(_Room);
                    break;
                default:
                    break;
            }
            PlyersList.Update(_Room.Players);
            RoomInfo.Text = "Количество игроков: " + _Room.Players.Count + "/" + _Room.PlayersCount;
        }

        public void Update()
        {

            PlyersList.Update(_Room.Players);
            RoomInfo.Text = "Количество игроков: " + _Room.Players.Count + "/" + _Room.PlayersCount;

        }

        public void Hide() => Enabled = false;

        public void Show(int id, string name)
        {

            Enabled = true;

            _Room = new ARoom(name + "'s Game", id, 2);
            _Room.SetMapSize(new ASize(MapWidth.MinValue, MapHeight.MinValue));

            Header.Text = _Room.Name;
            ChangeEvent?.Invoke(_Room);
        }

    }
}
