using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Unit.Main;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Forms
{

    public class ABattleForm: AForm
    {

        private ABattlePanel BattlePanel;

        public ABattleForm(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            BattlePanel = new ABattlePanel(Content.Size - 2) { Location = new APoint(1, 1) };

            Add(BattlePanel);

        }

        public void Update(IUnit dUnit, IUnit aUnit, int dPower, int aPower, int dResult, int aResult)
        {

            BattlePanel.Update(dUnit, aUnit, dPower, aPower, dResult, aResult);

        }

        public void Hide() => Enabled = false;

        public void Show(IUnit dUnit, IUnit aUnit, int dPower, int aPower, int dResult, int aResult)
        {

            Enabled = true;

            Update(dUnit, aUnit, dPower, aPower, dResult, aResult);

            Text = "Итоги битвы";

        }


    }

}
