using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class UnitsListPanel : AScrolleredPanel
    {

        public delegate void OnSelect(IUnit unit);
        public event OnSelect SelectEvent;
        public event OnSelect ExtraSelectEvent;

        private Dictionary<IUnit, AExtendedButton> UnitsList;

        public UnitsListPanel(ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            UnitsList = new Dictionary<IUnit, AExtendedButton>();
        }

        public override void Initialize()
        {

            base.Initialize();

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

        }

        public void Update(IEnumerable<IUnit> units)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, -70);

            foreach (AExtendedButton bt in UnitsList.Values) bt.Enabled = false;

            foreach (IUnit unit in units)
            {
                AExtendedButton button;

                if (UnitsList.ContainsKey(unit))
                {
                    button = UnitsList[unit];
                    string text = unit.Name + "\n" +
                        unit.UnitTypeName + "\n" +
                        "F: " + unit.Force + ", A: " + unit.Action + "/" + unit.ActionMaxValue + ", C: " + unit.Count + "\n" +
                        (unit.IsGeneral ? "Полководец: " + unit.General.FullName : "Полководец отсутствует") + "\n" +
                        "Владелец: " + unit.Owner.Name + "\n";
                    if (!button.ButtonText.Equals(text)) button.ButtonText = text;
                    button.FillColor = TexturePack.Colors[unit.Owner.Id];
                }
                else
                {
                    button = new AExtendedButton(new ASize(ContentSize.Width - 20, 120));
                    Add(button);

                    button.ButtonText = unit.Name + "\n" +
                        unit.UnitTypeName + "\n" +
                        "F: " + unit.Force + ", A: " + unit.Action + "/" + unit.ActionMaxValue + ", C: " + unit.Count + "\n" +
                        (unit.IsGeneral ? "Полководец: " + unit.General.FullName : "Полководец отсутствует") + "\n" +
                        "Владелец: " + unit.Owner.Name + "\n";
                    button.ExtraButtonText = "...";

                    UnitsList.Add(unit, button);

                    button.FillColor = TexturePack.Colors[unit.Owner.Id];

                    button.Button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.Button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                    button.ButtonClick += () => { SelectEvent?.Invoke(unit); };
                    button.ExtraButtonClick += () => { ExtraSelectEvent?.Invoke(unit); };

                }

                button.Enabled = true;
                button.Location = last + new APoint(0, button.Height + 10);
                last = button.Location;
            }

            ContentSize = new ASize(ContentSize.Width, UnitsList.Count > 0 ? last.Y + 130 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }

        public void Hide() => Enabled = false;
        public void Show(IEnumerable<IUnit> units)
        {
            Enabled = true;
            Update(units);
        }

    }
}
