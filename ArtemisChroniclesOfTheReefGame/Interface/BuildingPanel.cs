using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;
using AKeyState = CommonPrimitivesLibrary.AKeyState;

using GameLibrary;
using GameLibrary.Settlement.Building;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Settlement;
using GameLibrary.Player;
using GameLibrary.Technology;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Interface
{
    public class BuildingPanel: AScrolleredPanel
    {


        public delegate void OnSelect(IBuilding building);
        public event OnSelect SelectEvent;

        private Dictionary<IBuilding, Func<IPlayer, bool>> ConstBuildings;
        private Dictionary<IBuilding, TripleFieldButton> BuildingList;

        private TripleFieldButton Header;
        private ISettlement Settlement;

        public BuildingPanel(ASize size) : base(AScrollbarAlign.Vertical, size)
        {

            BuildingList = new Dictionary<IBuilding, TripleFieldButton>();

            ConstBuildings = new Dictionary<IBuilding, Func<IPlayer, bool>>()
            {
                { new ABuildingAqueduct(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Mathematics, ATechnologyType.Masonry }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingArableLand(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Agriculture }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingArmory(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Handicraft }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingBank(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Banking }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingBarn(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Masonry }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingCityWarehouse(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Masonry }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingCraftWorkshop(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Handicraft }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingEarthenShaft(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.PrimitiveSociety }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingFarm(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.AnimalHusbandry }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingForge(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.CastingMetals }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingGarden(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.PrimitiveSociety }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingHospital(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Medicine }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingLibrary(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Alphabet }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingMilitaryPlatz(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.MilitaryTraditions }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingMill(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Construction }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingMoat(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.PrimitiveSociety }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingMonastery(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Priesthood }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingPalace(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Building }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingPalisade(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.WoodProcessing }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingResidentialQuarter(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.PrimitiveSociety }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingSchool(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Education }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingShoppingQuarter(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Сurrency }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingStoneWalls(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Masonry }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingTheatre(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.TheatricalArt }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingTownHall(), (player) => false },
                { new ABuildingTreasury(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.Banking }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) },
                { new ABuildingWell(), (player) => player.Technologies.Technologies.Where(x => new ATechnologyType[]{ ATechnologyType.PrimitiveSociety }.ToList().Contains(x.Key)).All(x => x.Value.IsCompleted) }
            };

        }

        public override void Initialize()
        {

            base.Initialize();

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            Header = new TripleFieldButton(new ASize(ContentSize.Width - 20, 50)) { Location = new APoint(10, 10), IsInteraction = false };

            Add(Header);

            Header.TitleFieldText = "Название";
            Header.TitleField.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            Header.TitleField.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

            Header.ExtraFieldText = "Характеристики";
            Header.ExtraField.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            Header.ExtraField.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 12);

            foreach (var e in ConstBuildings)
            {
                TripleFieldButton button = new TripleFieldButton(new ASize(ContentSize.Width - 20, 150));
                Add(button);

                button.ExtraFieldText = string.Join("\n", e.Key.Characteristics.Select(x => StringPad(GameLocalization.SettlementCharacteristicName[x.SettlementCharacteristicType], 10) + " (" + (x.Value > 0? "+" + x.Value: x.Value.ToString()) + ")"));
                button.ExtraField.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                button.ExtraField.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                button.TitleFieldText = GameLocalization.Buildings[e.Key.BuildingType];
                button.TitleField.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                button.TitleField.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

                button.DownFieldText = "Цена: " + GameExtension.BuildCost[e.Key.BuildingType] + "\nВремя строительства: " + GameExtension.BuildTime[e.Key.BuildingType];
                button.DownField.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                button.DownField.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                button.MouseClickEvent += (state, mstate) => { SelectEvent?.Invoke(e.Key); };

                BuildingList.Add(e.Key, button);
            }

        }

        public void Update(IPlayer player, ISettlement settlement)
        {

            Settlement = settlement;
            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = Header.Location + new APoint(0, -100);

            foreach (var building in ConstBuildings)
            {
                TripleFieldButton button = BuildingList[building.Key];
                button.Enabled = building.Value(player);

                if (BuildingList.ContainsKey(building.Key) && building.Value(player))
                {
                    button.ExtraFieldText = string.Join("\n", building.Key.Characteristics.Select(x => StringPad(GameLocalization.SettlementCharacteristicName[x.SettlementCharacteristicType], 10) + " (" + (x.Value > 0 ? "+" + x.Value : x.Value.ToString()) + ") => " + StringPad(Convert.ToInt32(Settlement.GetType().GetProperty(x.SettlementCharacteristicType.ToString()).GetValue(Settlement)) + x.Value, 5)));
                    button.Location = last + new APoint(0, button.Height + 10);
                    last = button.Location;
                }
            }

            ContentSize = new ASize(ContentSize.Width, BuildingList.Count > 0 ? last.Y + 160 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }

        public void Hide() => Enabled = false;
        public void Show(IPlayer player, ISettlement settlement)
        {
            Enabled = true;
            Update(player, settlement);
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
