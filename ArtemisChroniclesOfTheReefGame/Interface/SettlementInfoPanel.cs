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
using GameLibrary.Map;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Extension;
using GameLibrary.Technology;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class SettlementInfoPanel: APanel
    {

        public delegate void OnTechnologySelect(ISettlement settlement);
        public event OnTechnologySelect TechnologySelectEvent;

        public APanel HeaderTitle;
        public APanel HeaderExtraInfo;
        public AExtendedButton InvestigatedTechnologyInfo;
        public APanel StatsPanel;
        private AScrolleredPanel SettlementResources;
        public AScrolleredPanel BuildingsPanel;

        ISettlement Settlement;

        public SettlementInfoPanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int titleWidth = Width / 4;
            HeaderTitle = new APanel(new ASize(titleWidth, 50)) { Parent = this, Location = new APoint(10, 10) };
            HeaderExtraInfo = new APanel(new ASize(Width - titleWidth - 30, 50)) { Parent = this, Location = HeaderTitle.Location + new APoint(HeaderTitle.Width + 10, 0) };
            InvestigatedTechnologyInfo = new AExtendedButton(new ASize(Width - 20, 50)) { Parent = this, Location = HeaderTitle.Location + new APoint(0, HeaderTitle.Height + 10) };
            StatsPanel = new APanel(new ASize(titleWidth, Height - HeaderTitle.Height - InvestigatedTechnologyInfo.Height - 40)) { Parent = this, Location = InvestigatedTechnologyInfo.Location + new APoint(0, InvestigatedTechnologyInfo.Height + 10) };

            int dHeight = (StatsPanel.Height - 10) / 2;

            SettlementResources = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(Width - titleWidth - 30, dHeight)) { Parent = this, Location = StatsPanel.Location + new APoint(StatsPanel.Width + 10, 0) };
            BuildingsPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(Width - titleWidth - 30, dHeight)) { Parent = this, Location = SettlementResources.Location + new APoint(0, SettlementResources.Height + 10) };

            //HeaderTitle.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            HeaderTitle.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            HeaderExtraInfo.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            HeaderExtraInfo.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            InvestigatedTechnologyInfo.Button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            InvestigatedTechnologyInfo.Button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            StatsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            StatsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            StatsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            SettlementResources.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            SettlementResources.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            SettlementResources.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            BuildingsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            BuildingsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            BuildingsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            InvestigatedTechnologyInfo.ExtraButtonClick += () => TechnologySelectEvent?.Invoke(Settlement);
            InvestigatedTechnologyInfo.ExtraButtonText = "...";
        }

        public void Update(ISettlement settlement)
        {
            Settlement = settlement;
            HeaderTitle.Text = settlement.Name;
            HeaderExtraInfo.Text = "[" + settlement.Location + "] : " + settlement.Owner.Name;
            InvestigatedTechnologyInfo.Text = "Изучаемая технология: " + (settlement.InvestigatedTechnology is ITechnology technology? StringPad(technology.Name, 40) + technology.StudyPoints + "/" + technology.RequiredStudyPoints + " ("+ (settlement.Science > 0? "+": "") + settlement.Science + ")" : "технология не выбрана");
            StatsPanel.Text = "Характеристики: \n\n" + string.Join("\n", Enum.GetValues(typeof(ASettlementCharacteristicType)).OfType<ASettlementCharacteristicType>().Select(x => StringPad(GameLocalization.SettlementCharacteristicName[x] + ":", 10) + " " + settlement.GetType().GetProperty(x.ToString()).GetValue(settlement)));
            //BuildingsPanel.Text = "Городские строения: \n\n" + string.Join("\n", settlement.Buildings.Select(x => x.Name));

            List<AMapCell> resources = settlement.Territories.Where(x => x.IsResource).ToList();

            string resourcesText = "Ресурсы на территориях поселения: \n\n" + (resources.Count > 0? string.Join("\n", resources.Where(x => settlement.Owner.IsResource(x.ResourceType)).Where(x => x.IsMined).Select(x => GameLocalization.Resources[x.ResourceType]).GroupBy(str => str).OrderByDescending(g => g.Count()).Select(g => StringPad(g.Key, 20) + " (" + g.Count() + ")")): "ресурсы отсутствуют");
            SettlementResources.ContentSize = new ASize(BuildingsPanel.ContentSize.Width, 40 + (resourcesText.Count(x => x == '\n') + 1) * BuildingsPanel.TextLabel.Font.Height);
            SettlementResources.Text = resourcesText;
            SettlementResources.Scrollbar.MaxValue = BuildingsPanel.Height < BuildingsPanel.ContentSize.Height ? BuildingsPanel.ContentSize.Height - BuildingsPanel.Height : 0;
            
            string buildingsText = "Городские строения: \n\n" + string.Join("\n", settlement.Buildings.Select(x => x.Name).GroupBy(str => str).OrderByDescending(g => g.Count()).Select(g => StringPad(g.Key, 15) + " (" + g.Count() + ")"));
            BuildingsPanel.ContentSize = new ASize(BuildingsPanel.ContentSize.Width, 40 + (buildingsText.Count(x => x == '\n') + 1) * BuildingsPanel.TextLabel.Font.Height);
            BuildingsPanel.Text = buildingsText;
            BuildingsPanel.Scrollbar.MaxValue = BuildingsPanel.Height < BuildingsPanel.ContentSize.Height ? BuildingsPanel.ContentSize.Height - BuildingsPanel.Height : 0;

        }

        public void Hide() => Enabled = false;
        public void Show(ISettlement settlement)
        {
            Enabled = true;
            Update(settlement);
        }

        private string StringPad(string value, int width) => value + (width - value.Length > 0? new String(' ', width - value.Length): "");
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
