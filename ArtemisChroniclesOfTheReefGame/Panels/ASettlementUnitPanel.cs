using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Player;
using GameLibrary.Settlement;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class ASettlementUnitPanel: AEmptyPanel
    {

        public ASettlementUnitPanel(ASize size) : base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

        }

        public void Update(ISettlement settlement)
        {

        }

    }
}
