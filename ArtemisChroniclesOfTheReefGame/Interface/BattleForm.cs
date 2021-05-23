using System;
using System.Linq;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;
using AKeyState = CommonPrimitivesLibrary.AKeyState;

using GameLibrary;
using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class BattleForm: AForm
    {

        private AEmptyPanel DefenderInfo;
        private AEmptyPanel AttackerInfo;
        private AEmptyPanel ResultInfo;

        private AButton Done;

        public BattleForm(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            ASize size = new ASize((Content.Width - 30) / 2, Content.Height - 190);

            DefenderInfo = new AEmptyPanel(size) { Location = new APoint(10, 10) };
            AttackerInfo = new AEmptyPanel(size) { Location = DefenderInfo.Location + new APoint(DefenderInfo.Width + 10, 0) };
            ResultInfo = new AEmptyPanel(new ASize(Width - 20, 100)) { Location = DefenderInfo.Location + new APoint(0, DefenderInfo.Height + 10) };

            Done = new AButton(new ASize(200, 50)) { Location = ResultInfo.Location + new APoint((ResultInfo.Width - 200) / 2, ResultInfo.Height + 10), Text = "Готово" };

            Done.MouseClickEvent += (state, mstate) => Enabled = false;

            KeyDownEvent += (state, kstate) => { if (kstate.KeyState.Equals(AKeyState.Exit)) Enabled = false; };

            Add(DefenderInfo);
            Add(AttackerInfo);
            Add(ResultInfo);
            Add(Done);

            DefenderInfo.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            AttackerInfo.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            ResultInfo.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

            Text = "Итоги битвы";

        }

        public void Update(IUnit dUnit, IUnit aUnit, int dPower, int aPower, int dResult, int aResult)
        {
            int allPower = dPower + aPower;

            DefenderInfo.Text =
                "Игрок: " + dUnit.Owner.Name + "\n" +
                dUnit.UnitTypeName + " <" + dUnit.Name + ">" + "\n" +
                "Полководец: " + (dUnit.IsGeneral ? dUnit.General.FullName : "отсутствует") + "\n" +
                "Отряд: " + dUnit.Count + "\n" +
                "Сила: " + dUnit.Force + "\n" +
                (dUnit.IsGeneral ? "Навык полководца: " + dUnit.General.MartialSkills: "") + "\n" +
                "Расстановка сил: " + Convert.ToInt32((dPower / (float)allPower) * 100) + "%";

            AttackerInfo.Text =
                "Игрок: " + aUnit.Owner.Name + "\n" +
                aUnit.UnitTypeName + " <" + aUnit.Name + ">" + "\n" +
                "Полководец: " + (aUnit.IsGeneral ? aUnit.General.FullName : "отсутствует") + "\n" +
                "Отряд: " + aUnit.Count + "\n" +
                "Сила: " + aUnit.Force + "\n" +
                (aUnit.IsGeneral ? "Навык полководца: " + aUnit.General.MartialSkills: "") + "\n" +
                "Расстановка сил: " + Convert.ToInt32((aPower / (float)allPower) * 100) + "%";

            ResultInfo.Text = "Потери: \n" + 
                dUnit.Owner.Name + " (" + dResult + ")" + (dUnit.Count - dResult == 0? " - уничтожен": "") +"\n" +
                aUnit.Owner.Name + " (" + aResult + ")" + (aUnit.Count - aResult == 0 ? " - уничтожен" : "");

            Enabled = true;

        }

    }
}
