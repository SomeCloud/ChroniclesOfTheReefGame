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
    public class LobbyPanel : APanel
    {

        private AEmptyPanel Header;
        private AEmptyPanel RoomInfo;
        private LobbyPlyersList PlyersList;

        public LobbyPanel(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = (Width - 20) * 3 / 4;

            Header = new AEmptyPanel(new ASize(dWidth, 100)) { Parent = this, Location = new APoint(10, 10) };
            RoomInfo = new AEmptyPanel(new ASize(dWidth, 100)) { Parent = this, Location = Header.Location + new APoint(0, Header.Height + 10) };
            PlyersList = new LobbyPlyersList(new ASize(dWidth, Height - 240)) { Parent = this, Location = RoomInfo.Location + new APoint(0, RoomInfo.Height + 10) };

            Header.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 16);
            RoomInfo.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

        }

        public void Update(ARoom room)
        {

            Header.Text = room.Name;
            RoomInfo.Text = "Количество игроков: " + room.Players.Count + "/" + room.PlayersCount;
            PlyersList.Update(room.Players);

        }

    }
}
