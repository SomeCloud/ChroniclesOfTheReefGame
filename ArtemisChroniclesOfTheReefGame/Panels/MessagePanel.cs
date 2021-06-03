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

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class MessagePanel: APanel
    {

        public delegate void OnDone();
        public delegate void OnRenouncement();

        public event OnDone DoneEvent;
        public event OnRenouncement RenouncementEvent;

        private APanel _Content;
        private AButton _Done;
        private AButton _Renouncement;

        private Action _DoneAction;
        private Action _RenouncementAction;

        public MessagePanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int dHeight = (Height - 20) * 3 / 4;

            _Content = new APanel(new ASize(Width - 20, dHeight)) { Parent = this, Location = new APoint(10, 10) };

            int dY = _Content.Height + (Height - dHeight - 110) / 2;

            _Done = new AButton(new ASize(_Content.Width / 2, 50)) { Parent = this, Text = "Принять", Location = _Content.Location + new APoint((_Content.Width - _Content.Width / 2) / 2, dY) };
            _Renouncement = new AButton(new ASize(_Content.Width / 2, 50)) { Parent = this, Text = "Отказаться", Location = _Done.Location + new APoint(0, _Done.Height + 10) };

            _Done.MouseClickEvent += (state, mstate) =>
            {
                if (_DoneAction is object)
                {
                    _DoneAction();
                }
                DoneEvent?.Invoke();
            };

            _Renouncement.MouseClickEvent += (state, mstate) =>
            {
                if (_RenouncementAction is object)
                {
                    _RenouncementAction();
                }
                RenouncementEvent?.Invoke();
            };

        }

        public void Update(IMessage message)
        {

            _Content.Text = message.Text;
            //_DoneAction = message.Done;
            //_RenouncementAction = message.Renouncement;
            _Renouncement.Enabled = message.IsRenouncement;
        }

    }
}
