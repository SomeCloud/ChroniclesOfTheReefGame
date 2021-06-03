using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;
using AKeyState = CommonPrimitivesLibrary.AKeyState;

using GameLibrary;
using GameLibrary.Message;
using GameLibrary.Player;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{
    public class AMessageListPanelForm: AForm
    {

        public delegate void OnSelect(IMessage message);

        public event OnSelect SelectEvent;

        private MessageListPanel MessageListPanel;

        private IPlayer Player;

        public AMessageListPanelForm(ASize size) : base(size)
        {

        }

        public AMessageListPanelForm(ASize size, IPrimitiveTexture primitiveTexture) : base(size, primitiveTexture)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            MessageListPanel = new MessageListPanel(Content.Size - 2) { Location = new APoint(1, 1) };

            MessageListPanel.ExtraButtonClickEvent += (message) => { Player.RemoveMessage(message); MessageListPanel.Update(Player.Messages); };
            MessageListPanel.SelectEvent += (message) => SelectEvent?.Invoke(message);

            Add(MessageListPanel);

        }

        public void Update() => Update(Player);

        public void Update(IPlayer player)
        {
            Player = player;
            MessageListPanel.Update(player.Messages);
        }

        public void Hide() => Enabled = false;

        public void Show(IPlayer player)
        {
            Enabled = true;
            Update(player);

            Text = "Почта правителя";
        }

    }
}
