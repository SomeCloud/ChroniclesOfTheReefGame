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
    public class LobbyNote: AButton
    {

        private AEmptyPanel Header;
        private AEmptyPanel Status;
        private AEmptyPanel Info;

        public ARoom Room;

        public LobbyNote(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = Width * 5 / 8;
            int dHeight = Height / 2;

            Header = new AEmptyPanel(new ASize(dWidth, Height) - 2) { Parent = this, Location = new APoint(1, 1), IsInteraction = false };
            Info = new AEmptyPanel(new ASize(Width - dWidth, dHeight - 2)) { Parent = this, Location = Header.Location + new APoint(Header.Width, 0), IsInteraction = false };
            Status = new AEmptyPanel(new ASize(Info.Width, Height - dHeight)) { Parent = this, Location = Info.Location + new APoint(0, Info.Height), IsInteraction = false };

            Header.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            Header.TextLabel.TextColor = Microsoft.Xna.Framework.Color.White;
            Header.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 20);

            Status.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            Status.TextLabel.TextColor = Microsoft.Xna.Framework.Color.White;
            Status.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            Info.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            Info.TextLabel.TextColor = Microsoft.Xna.Framework.Color.White;
            Info.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 16);

        }

        public void Update(ARoom room)
        {

            Room = room;

            Header.Text = room.Name;
            Status.Text = room.GameStatus.Equals(AGameStatus.Wait) ? "Ожидает подключения игроков": room.GameStatus.Equals(AGameStatus.Game)? "Игра идет": "Игра окончена";

            FillColor = room.GameStatus.Equals(AGameStatus.Wait) ? GraphicsExtension.ExtraColorGreen : GraphicsExtension.ExtraColorRed;

            Info.Text = room.Players.Count + "/" + room.PlayersCount;

        }

    }
}
