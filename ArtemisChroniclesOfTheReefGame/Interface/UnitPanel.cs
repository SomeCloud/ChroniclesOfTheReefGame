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
using GameLibrary.Unit.Main;
using GameLibrary.Player;
using GameLibrary.Map;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class UnitPanel: AForm
    {

        public delegate void OnUpdate();

        public event OnUpdate UpdateEvent;

        private UnitInfoPanel UnitInfoPanel;

        private AGame Game;

        public UnitPanel(AGame game, ASize size) : base(size)
        {
            Game = game;
        }

        public UnitPanel(AGame game, ASize size, IPrimitiveTexture primitiveTexture) : base(size, primitiveTexture)
        {
            Game = game;
        }

        public override void Initialize()
        {

            base.Initialize();

            UnitInfoPanel = new UnitInfoPanel(Game, Content.Size - 2) { Location = new APoint(1, 1) };

            UnitInfoPanel.DestroyEvent += () =>
            {
                UpdateEvent?.Invoke();
                Hide();
            };

            UnitInfoPanel.UpdateEvent += () => UpdateEvent?.Invoke();

            Add(UnitInfoPanel);

        }

        public void Update(IUnit unit)
        {
            UnitInfoPanel.Update(unit);
        }

        public void Hide() => Enabled = false;

        public void Show(IUnit unit)
        {
            Enabled = true;
            Update(unit);

            Text = "Отряд " + unit.Name;

        }


    }
}
