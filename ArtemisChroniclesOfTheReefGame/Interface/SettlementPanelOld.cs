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
    public class SettlementPanelOld: APanel
    {

        public event MapCellPanel.OnUpdate UpdateEvent;

        private AGame Game;

        private APanel SettlementCharacteristic;

        private AScrolleredPanel SettlementBuildingsPanel;
        //private AScrolleredPanel SettlementStoragePanel;
        private AScrolleredPanel SettlementBuildingsInConstructionPanel;
        private AScrolleredPanel SettlementNewConstructionPanel;

        public SettlementPanelOld(AGame game, ASize size): base(size)
        {

            Game = game;

        }

        public override void Initialize()
        {
            base.Initialize();

            int width = (Width - 30) / 2;

            SettlementCharacteristic = new APanel(new ASize(200, 200)) { Parent = this, Location = new APoint(10, 10) };

            int extraWidt = Width - SettlementCharacteristic.Width - 30/*) / 2*/;

            SettlementBuildingsPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(extraWidt, SettlementCharacteristic.Height)) { Parent = this, Location = SettlementCharacteristic.Location + new APoint(SettlementCharacteristic.Width + 10, 0) };
            //SettlementStoragePanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(extraWidt, SettlementCharacteristic.Height)) { Parent = this, Location = SettlementBuildingsPanel.Location + new APoint(SettlementBuildingsPanel.Width + 10, 0) };

            SettlementBuildingsInConstructionPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(width, Height - SettlementCharacteristic.Height - 30)) { Parent = this, Location = SettlementCharacteristic.Location + new APoint(0, SettlementCharacteristic.Height + 10) };
            SettlementNewConstructionPanel = new AScrolleredPanel(AScrollbarAlign.Vertical, new ASize(width,Height - SettlementCharacteristic.Height - 30)) { Parent = this, Location = SettlementBuildingsInConstructionPanel.Location + new APoint(SettlementBuildingsInConstructionPanel.Width + 10, 0) };

            SettlementCharacteristic.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            SettlementBuildingsPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            SettlementBuildingsInConstructionPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            //SettlementStoragePanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;

            SettlementBuildingsPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            SettlementBuildingsInConstructionPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            //SettlementStoragePanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;

            SettlementCharacteristic.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            SettlementBuildingsPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
            //SettlementStoragePanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);
        }

        public void Update(APoint location)
        {
            AMapCell mapCell = Game.GetMapCell(location);
            SettlementCharacteristic.Text = "Статистика: \n\n" + string.Join("\n", Enum.GetValues(typeof(ASettlementCharacteristicType)).OfType<ASettlementCharacteristicType>().Select(x => StringPad(GameLocalization.SettlementCharacteristicName[x] + ":", 10) + " " + mapCell.Settlement.GetType().GetProperty(x.ToString()).GetValue(mapCell.Settlement)));
            SettlementBuildingsPanel.Text = "Городские строения: \n\n" + string.Join("\n", mapCell.Settlement.Buildings.Select(x => x.Name));

            string buildingsText = "Городские строения: \n\n" + string.Join("\n", mapCell.Settlement.Buildings.Select(x => x.Name));
            SettlementBuildingsPanel.ContentSize = new ASize(SettlementBuildingsPanel.ContentSize.Width, 40 + (buildingsText.Count(x => x == '\n') + 1) * SettlementBuildingsPanel.TextLabel.Font.Height);
            SettlementBuildingsPanel.Text = buildingsText;
            SettlementBuildingsPanel.Scrollbar.MaxValue = SettlementBuildingsPanel.Height < SettlementBuildingsPanel.ContentSize.Height ? SettlementBuildingsPanel.ContentSize.Height - SettlementBuildingsPanel.Height : 0;

            /*
            string storageText = "Городской склад: \n\n" + string.Join("\n", mapCell.Settlement.Storage.Total.Select(x => GameLocalization.Resources[x.Key] + ": " + x.Value + " ед."));
            SettlementStoragePanel.ContentSize = new ASize(SettlementStoragePanel.ContentSize.Width, 40 + (storageText.Count(x => x == '\n') + 1) * SettlementStoragePanel.TextLabel.Font.Height);
            SettlementStoragePanel.Text = storageText;
            SettlementStoragePanel.Scrollbar.MaxValue = SettlementStoragePanel.Height < SettlementStoragePanel.ContentSize.Height ? SettlementStoragePanel.ContentSize.Height - SettlementStoragePanel.Height : 0;
            */

            SettlementBuildingsInConstructionPanel.Text = string.Join("\n", mapCell.Settlement.BuildingsInConstruction.Select(x => x.Building.Name));
            UpdateEvent?.Invoke(mapCell);
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
