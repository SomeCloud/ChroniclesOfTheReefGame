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
    public class LobbyList: AScrolleredPanel
    {

        public delegate void OnSelectLobby(ARoom room);
        public event OnSelectLobby SelectLobbyEvent;

        private Dictionary<ARoom, LobbyNote> LobbyButton;

        public LobbyList(ASize size) : base(AScrollbarAlign.Vertical, size)
        {

            LobbyButton = new Dictionary<ARoom, LobbyNote>();

        }

        public override void Initialize()
        {

            base.Initialize();

            Text = "Список доступных лобби";

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

        }

        public void Update(IEnumerable<ARoom> rooms)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, -50);

            foreach (AButton bt in LobbyButton.Values) bt.Enabled = false;

            foreach (ARoom room in rooms)
            {

                LobbyNote lobby;

                if (LobbyButton.ContainsKey(room))
                {
                    lobby = LobbyButton[room];
                    if (lobby.Room.Equals(room)) lobby.Update(room);
                    lobby.Enabled = true;
                }
                else
                {
                    lobby = new LobbyNote(new ASize(ContentSize.Width - 20, 100));
                    Add(lobby);
                    lobby.Update(room);
                    LobbyButton.Add(room, lobby);
                    lobby.MouseClickEvent += (state, mstate) => { SelectLobbyEvent?.Invoke(room); };
                }

                lobby.Location = last + new APoint(0, lobby.Height + 10);
                last = lobby.Location;

            }

        }

    }
}
