using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Extension;
using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class ExtraSidePanel: APanel
    {

        public delegate void OnClick();
        public delegate void OnClose();
        public delegate void OnSelectUnit(IUnit unit);

        public event OnClick SettlementPanelActivate;
        public event OnClose CloseEvent;
        public event OnSelectUnit SelectUnitEvent;
        public event OnSelectUnit ExtraUnitSelectEvent;

        private APanel HeaderPanel;
        private APanel InfoPanel;
        private UnitsListPanel UnitsList;
        private SettlementMiniPanel SettlementMiniPanel;
        private AButton CloseButton;

        public ExtraSidePanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            HeaderPanel = new APanel(new ASize(Width - 20, 50)) { Parent = this, Location = new APoint(10, 10) };
            InfoPanel = new APanel(new ASize(Width - 20, 120)) { Parent = this, Location = HeaderPanel.Location + new APoint(10, HeaderPanel.Height + 10) };
            UnitsList = new UnitsListPanel(new ASize(Width - 20, 300)) { Parent = this, Location = InfoPanel.Location + new APoint(0, InfoPanel.Height + 10), Text = "Юниты на клетке" };
            SettlementMiniPanel = new SettlementMiniPanel(new ASize(Width - 20, 300)) { Parent = this, Location = UnitsList.Location + new APoint(0, UnitsList.Height + 10) };
            CloseButton = new AButton(new ASize(Width - 20, 50)) { Parent = this, Location = SettlementMiniPanel.Location + new APoint(0, SettlementMiniPanel.Height + 10), Text = "Закрыть" };

            InfoPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            InfoPanel.TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            InfoPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            UnitsList.SelectEvent += (unit) => SelectUnitEvent?.Invoke(unit);
            UnitsList.ExtraSelectEvent += (unit) => ExtraUnitSelectEvent?.Invoke(unit);

            SettlementMiniPanel.SettlementPanelActivate += () => SettlementPanelActivate?.Invoke();

            CloseButton.MouseClickEvent += (state, mstate) =>
            {
                CloseEvent?.Invoke();
                Hide();
            };

        }

        public void Update(GameData gameData, AMapCell mapCell)
        {

            APoint location = new APoint(10, 10);

            if (mapCell.IsSettlement)
            {
                SettlementMiniPanel.Show(mapCell.Settlement, mapCell.Settlement.Owner is object && gameData.ActivePlayer.Name.Equals(mapCell.Settlement.Owner.Name));
                SettlementMiniPanel.Location = location;
                SettlementMiniPanel.HeaderPanel.FillColor = mapCell.IsOwned ? TexturePack.Colors[mapCell.Owner.Id] : GraphicsExtension.DefaultFillColor;
                location.Y += SettlementMiniPanel.Height + 10;
                HeaderPanel.Enabled = false;
            }
            else
            {
                SettlementMiniPanel.Hide();
                HeaderPanel.Enabled = true;
                HeaderPanel.Text = mapCell.IsSettlement ? mapCell.Settlement.Name + " (" + mapCell.Settlement.Owner.Name + ")" : mapCell.Location + " (" + (mapCell.IsOwned ? mapCell.Owner.Name : "владелец отсутствует") + ")";
                HeaderPanel.FillColor = mapCell.IsOwned ? TexturePack.Colors[mapCell.Owner.Id] : GraphicsExtension.DefaultFillColor;
                location.Y += HeaderPanel.Height + 10;
            }

            InfoPanel.Location = location;

            InfoPanel.Text = "Данные о клетке: \nМестность: " + GameLocalization.Biomes[mapCell.BiomeType] + "\n" +
                "Ресурс: " + (gameData.ActivePlayer.IsResource(mapCell.ResourceType) ? (GameLocalization.Resources[mapCell.ResourceType] + (mapCell.IsResource ? " (" + (mapCell.IsMined ? "Добывается" : "Не добывается") + ")" : "")): "Ресурс отсутствует") + "\n" +
                "Население: " + mapCell.Population.Total + " человек(а)" + "\n" +
                "Культурный уровень: " + mapCell.Culture + " ед.";

            location.Y += InfoPanel.Height + 10;

            if (gameData.GetUnits(mapCell.Location) is List<IUnit> units && units.Count > 0)
            {
                UnitsList.Show(units);
                UnitsList.Location = location;
                location.Y += UnitsList.Height + 10;
            }
            else UnitsList.Hide();

            CloseButton.Location = location;

        }

        public void Hide() => Enabled = false;
        public void Show(GameData gameData, AMapCell mapCell)
        {
            Enabled = true;
            Update(gameData, mapCell);
        }

    }
}
