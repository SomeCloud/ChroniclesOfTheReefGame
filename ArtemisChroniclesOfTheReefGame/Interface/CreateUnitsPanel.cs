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
using GameLibrary.Unit;
using GameLibrary.Map;
using GameLibrary.Player;
using GameLibrary.Technology;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class CreateUnitsPanel: AScrolleredPanel
    {

        public delegate void OnSelect(AUnitType unitType);
        public event OnSelect SelectEvent;

        private Dictionary<IUnit, Func<IPlayer, bool>> ConstUnits;
        private Dictionary<IUnit, AButton> UnitList;

        private AEmptyPanel Header;

        public CreateUnitsPanel(ASize size) : base(AScrollbarAlign.Vertical, size)
        {

            UnitList = new Dictionary<IUnit, AButton>();

            ConstUnits = new Dictionary<IUnit, Func<IPlayer, bool>>()
            {
                { new AUnitColonist(0, null, new APoint(0, 0), null, ""), (player) => player.Technologies.Technologies[ATechnologyType.PrimitiveSociety].IsCompleted },
                { new AUnitSpearman(0, null, new APoint(0, 0), null, ""), (player) => player.Technologies.Technologies[ATechnologyType.StoneProcessing].IsCompleted },
                { new AUnitFarmer(0, null, new APoint(0, 0), null, ""), (player) => player.Technologies.Technologies[ATechnologyType.PrimitiveSociety].IsCompleted },
                { new AUnitSwordsman(0, null, new APoint(0, 0), null, ""), (player) => player.Technologies.Technologies[ATechnologyType.MilitaryTraditions].IsCompleted },
                { new AUnitArcher(0, null, new APoint(0, 0), null, ""), (player) => player.Technologies.Technologies[ATechnologyType.HuntingAndGathering].IsCompleted },
                { new AUnitMissionary(0, null, new APoint(0, 0), null, ""), (player) => player.Technologies.Technologies[ATechnologyType.Priesthood].IsCompleted },
                { new AUnitAxeman(0, null, new APoint(0, 0), null, ""), (player) => player.Technologies.Technologies[ATechnologyType.BronzeProcessing].IsCompleted },
                { new AUnitWarrior(0, null, new APoint(0, 0), null, ""), (player) => player.Technologies.Technologies[ATechnologyType.PrimitiveSociety].IsCompleted }
            };

        }

        public override void Initialize()
        {

            base.Initialize();

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            Header = new AEmptyPanel(new ASize(ContentSize.Width - 20, 50)) { Location = new APoint(10, 10) };

            Add(Header);

            Header.Text = StringPad("Название", 15) + " | " + StringPad("Стоимость", 10) + " | " + StringPad("Атака", 10) + " | " + StringPad("Действие", 10) + " | " + StringPad("Содержание", 10) + " | " + StringPad("Ресурс", 15);

            Header.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            Header.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            foreach (var e in ConstUnits)
            {
                AButton button = new AButton(new ASize(ContentSize.Width - 20, 50));
                Add(button);

                button.Text = StringPad(e.Key.UnitTypeName, 15) + " | " + StringPad(e.Key.Cost, 10) + " | " + StringPad(e.Key.Force, 10) + " | " + StringPad(e.Key.ActionMaxValue, 10) + " | " + StringPad(e.Key.ContentTax, 10) + " | " + StringPad(e.Key.RequiredResource.Equals(AResourceType.None)? "Не требуется": GameLocalization.Resources[e.Key.RequiredResource], 15);
                button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                button.MouseClickEvent += (state, mstate) => { SelectEvent?.Invoke(e.Key.UnitType); };

                UnitList.Add(e.Key, button);
            }

        }

        public void Update(IPlayer player)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = Header.Location + new APoint(0, 0);

            foreach (var unit in ConstUnits)
            {
                AButton button = UnitList[unit.Key];
                button.Enabled = unit.Value(player);

                if (UnitList.ContainsKey(unit.Key) && unit.Value(player))
                {
                    button.Location = last + new APoint(0, button.Height + 10);
                    last = button.Location;
                    button.FillColor = player.Resources.Keys.Contains(unit.Key.RequiredResource) || unit.Key.RequiredResource.Equals(AResourceType.None) ? GraphicsExtension.ExtraColorGreen : GraphicsExtension.ExtraColorRed;
                }
            }

            ContentSize = new ASize(ContentSize.Width, UnitList.Count > 0 ? last.Y + 60 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }

        public void Hide() => Enabled = false;
        public void Show(IPlayer player)
        {
            Enabled = true;
            Update(player);
        }

        private string StringPad(string value, int width) => value + (width - value.Length > 0 ? new String(' ', width - value.Length) : "");
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
