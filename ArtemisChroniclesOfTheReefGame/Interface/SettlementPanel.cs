using System;
using System.Linq;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Extension;
using GameLibrary.Settlement.Characteristic;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class SettlementPanel: APanel
    {

        public event MapCellPanel.OnUpdate UpdateEvent;

        private AGame Game;

        private APanel SettlementCharacteristic;

        private AScrolleredPanel SettlementBuildingsPanel;
        private AScrolleredPanel SettlementBuildingsInConstructionPanel;
        private AScrolleredPanel SettlementNewConstructionPanel;

        public SettlementPanel(AGame game, ASize size): base(size)
        {

            Game = game;

        }

        public override void Initialize()
        {
            base.Initialize();

            int width = (Width - 30) / 2;

            SettlementCharacteristic = new APanel(new ASize(200, 200)) { Parent = this, Location = new APoint(10, 10) };
            SettlementBuildingsPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(Width - SettlementCharacteristic.Width - 30, SettlementCharacteristic.Height)) { Parent = this, Location = SettlementCharacteristic.Location + new APoint(SettlementCharacteristic.Width + 10, 0) };

            SettlementBuildingsInConstructionPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(width, Height - SettlementCharacteristic.Height - 30)) { Parent = this, Location = SettlementCharacteristic.Location + new APoint(0, SettlementCharacteristic.Height + 10) };
            SettlementNewConstructionPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(width,Height - SettlementCharacteristic.Height - 30)) { Parent = this, Location = SettlementBuildingsInConstructionPanel.Location + new APoint(SettlementBuildingsInConstructionPanel.Width + 10, 0) };

            SettlementCharacteristic.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            SettlementBuildingsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            SettlementBuildingsInConstructionPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            SettlementBuildingsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            SettlementBuildingsInConstructionPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;

            SettlementCharacteristic.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            SettlementBuildingsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
        }

        public void Update(APoint location)
        {
            AMapCell mapCell = Game.GetMapCell(location);
            SettlementCharacteristic.Text = string.Join("\n", Enum.GetValues(typeof(ASettlementCharacteristicType)).OfType<ASettlementCharacteristicType>().Select(x => StringPad(GameLocalization.SettlementCharacteristicName[x] + ":", 10) + " " + mapCell.Settlement.GetType().GetProperty(x.ToString()).GetValue(mapCell.Settlement)));
            SettlementBuildingsPanel.Text = string.Join("\n", mapCell.Settlement.Buildings.Select(x => x.Name));
            SettlementBuildingsInConstructionPanel.Text = string.Join("\n", mapCell.Settlement.BuildingsInConstruction.Select(x => x.Building.Name));
            UpdateEvent?.Invoke(mapCell);
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
