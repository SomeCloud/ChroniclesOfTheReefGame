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
using GameLibrary.Settlement;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class SettlementMiniPanel: APanel
    {

        public delegate void OnClick();
        public event OnClick SettlementPanelActivate;

        private APanel _HeaderPanel;
        private AEmptyPanel ContentPanel;

        public APanel HeaderPanel => _HeaderPanel;

        public SettlementMiniPanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            _HeaderPanel = new APanel(new ASize(Width, 50)) { Parent = this, Location = new APoint(0, 0) };
            ContentPanel = new AEmptyPanel(new ASize(Width, Height - 50)) { Parent = this, Location = _HeaderPanel.Location + new APoint(0, _HeaderPanel.Height) };

            ContentPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            ContentPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            ContentPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            _HeaderPanel.MouseClickEvent += (state, mastate) =>
            {
                SettlementPanelActivate?.Invoke();
            };

        }

        public void Update(ISettlement settlement, bool isShowDetails)
        {
            _HeaderPanel.Text = settlement.Name + " (" + settlement.Owner.Name + ")";
            ContentPanel.Text = isShowDetails ? "Характеристики: \n\n" +
                string.Join("\n", Enum.GetValues(typeof(ASettlementCharacteristicType)).OfType<ASettlementCharacteristicType>().Select(x => StringPad(GameLocalization.SettlementCharacteristicName[x] + ":", 10) + " " + settlement.GetType().GetProperty(x.ToString()).GetValue(settlement))) + "\n\n" +
                "Изучается: " + (settlement.InvestigatedTechnology is object? settlement.InvestigatedTechnology.Name + " (" + settlement.InvestigatedTechnology.StudyPoints + " / " + settlement.InvestigatedTechnology.RequiredStudyPoints + ")": "не выбрано") + "\n" +
                "Проекты: " + (settlement.BuildingsInConstruction.Count > 0 ? settlement.BuildingsInConstruction.First().Building.Name + " (" + settlement.BuildingsInConstruction.First().TimeToComplete + ")" : "отсутствуют"): "Информация не досутпна";
        }

        public void Hide() => Enabled = false;
        public void Show(ISettlement settlement, bool isShowDetails)
        {
            Enabled = true;
            Update(settlement, isShowDetails);
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
