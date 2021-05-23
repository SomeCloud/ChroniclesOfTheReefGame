using System;
using System.Collections.Generic;
using System.Text;

using AUnitType = GameLibrary.Unit.Main.AUnitType;
using GameLibrary.Map;
using GameLibrary.Character;
using GameLibrary.Player;
using GameLibrary.Technology;
using GameLibrary.Settlement.Building;
using GameLibrary.Settlement.Characteristic;

namespace GameLibrary.Extension
{
    public struct GameLocalization
    {
        
        public static IReadOnlyDictionary<ABiomeType, string> Biomes = new Dictionary<ABiomeType, string>() {
            {ABiomeType.None, "Непроходимая местность" } ,
            {ABiomeType.Desert, "Пустыни" } ,
            {ABiomeType.Forest, "Леса" } ,
            {ABiomeType.Mountain, "Горы" } ,
            {ABiomeType.Plain, "Равнины" } ,
            {ABiomeType.Sea, "Водоём" } 
        };

        public static IReadOnlyDictionary<AResourceType, string> Resources = new Dictionary<AResourceType, string>() {
            {AResourceType.None, "Ресурс отсутствует" } ,
            {AResourceType.Iron, "Железная руда" } ,
            {AResourceType.Copper, "Медная руда" } ,
            {AResourceType.Gold, "Золотая руда" } ,
            {AResourceType.Silver, "Серебрянная руда" } ,
            {AResourceType.Stone, "Камень" } ,
            {AResourceType.Marable, "Мрамор" } ,
            {AResourceType.Wood, "Древесина" } ,
            {AResourceType.Fish, "Рыба" } ,
            {AResourceType.Wheat,  "Злаки" } 
        };

        public static IReadOnlyDictionary<ABuildingType, string> Buildings = new Dictionary<ABuildingType, string>() {
            { ABuildingType.TownHall, "Ратуша" },
            { ABuildingType.Moat, "Ров" },
            { ABuildingType.EarthenShaft, "Землянной вал" },
            { ABuildingType.Palisade, "Частокол" },
            { ABuildingType.StoneWalls, "Каменные стены" },
            { ABuildingType.Barn, "Амбар" },
            { ABuildingType.MilitaryPlatz, "Воинский плац" },
            { ABuildingType.ResidentialQuarter, "Жилой квартал" },
            { ABuildingType.CraftWorkshop, "Ремесленная мастерская" },
            { ABuildingType.Forge, "Кузница" },
            { ABuildingType.Armory, "Оружейная" },
            { ABuildingType.CityWarehouse, "Городской склад" },
            { ABuildingType.ShoppingQuarter, "Торговый квартал" },
            { ABuildingType.Bank, "Банк" },
            { ABuildingType.Treasury, "Сокровищница" },
            { ABuildingType.School, "Школа" },
            { ABuildingType.Monastery, "Монастырь" },
            { ABuildingType.Hospital, "Госпиталь" },
            { ABuildingType.Theatre, "Театр" },
            { ABuildingType.Library, "Библиотека" },
            { ABuildingType.Palace, "Дворец" },
            { ABuildingType.Garden, "Сад" },
            { ABuildingType.Mill, "Мельница" },
            { ABuildingType.ArableLand, "Пашня" },
            { ABuildingType.Farm, "Ферма" },
            { ABuildingType.Aqueduct, "Акведук" },
            { ABuildingType.Well, "Колодец" }
        };

        public static IReadOnlyDictionary<ABuildingType, string> BuildingInfo = new Dictionary<ABuildingType, string>() {
            { ABuildingType.TownHall, "Ратуша" },
            { ABuildingType.Moat, "Ров" },
            { ABuildingType.EarthenShaft, "Землянной вал" },
            { ABuildingType.Palisade, "Частокол" },
            { ABuildingType.StoneWalls, "Каменные стены" },
            { ABuildingType.Barn, "Амбар" },
            { ABuildingType.MilitaryPlatz, "Воинский плац" },
            { ABuildingType.ResidentialQuarter, "Жилой квартал" },
            { ABuildingType.CraftWorkshop, "Ремесленная мастерская" },
            { ABuildingType.Forge, "Кузница" },
            { ABuildingType.Armory, "Оружейная" },
            { ABuildingType.CityWarehouse, "Городской склад" },
            { ABuildingType.ShoppingQuarter, "Торговый квартал" },
            { ABuildingType.Bank, "Банк" },
            { ABuildingType.Treasury, "Сокровищница" },
            { ABuildingType.School, "Школа" },
            { ABuildingType.Monastery, "Монастырь" },
            { ABuildingType.Hospital, "Госпиталь" },
            { ABuildingType.Theatre, "Театр" },
            { ABuildingType.Library, "Библиотека" },
            { ABuildingType.Palace, "Дворец" },
            { ABuildingType.Garden, "Сад" },
            { ABuildingType.Mill, "Мельница" },
            { ABuildingType.ArableLand, "Пашня" },
            { ABuildingType.Farm, "Ферма" },
            { ABuildingType.Aqueduct, "Акведук" },
            { ABuildingType.Well, "Колодец" }
        };

        public static IReadOnlyDictionary<AUnitType, string> UnitName = new Dictionary<AUnitType, string>()
        {
            { AUnitType.Colonist, "Поселенец" },
            { AUnitType.Spearman, "Копьеносец" },
            { AUnitType.Farmer, "Фермер" },
            { AUnitType.Swordsman, "Мечник" },
            { AUnitType.Archer, "Лучник" },
            { AUnitType.Missionary, "Миссионер" },
            { AUnitType.Axeman, "Секироносец" },
            { AUnitType.Warrior, "Воин" }
        };

        public static IReadOnlyDictionary<ASexType, string> SexTypeName = new Dictionary<ASexType, string>()
        {
            { ASexType.Male, "Мужчина" },
            { ASexType.Female, "Женщина" }
        };

        public static IReadOnlyDictionary<ASettlementCharacteristicType, string> SettlementCharacteristicName = new Dictionary<ASettlementCharacteristicType, string>()
        {
            { ASettlementCharacteristicType.Protection, "Защита" },
            { ASettlementCharacteristicType.Income, "Доход" },
            { ASettlementCharacteristicType.Science, "Наука" },
            { ASettlementCharacteristicType.Culture, "Культура" },
            { ASettlementCharacteristicType.Medicine, "Медицина" },
            { ASettlementCharacteristicType.Religion, "Религия" },
            { ASettlementCharacteristicType.Food, "Пища" }
        };

        public static IReadOnlyDictionary<string, string> PlayerStatsName = new Dictionary<string, string>()
        {
            { "Attractiveness", "Привлекательность" },
            { "Education", "Образованность" },
            { "MartialSkills", "Воинские навыки" },
            { "Health", "Здоровье" },
            { "Fertility", "Плодовитость" }
        };

        public static IReadOnlyDictionary<ARelationshipType, string> RelationshipName = new Dictionary<ARelationshipType, string>()
        {
            { ARelationshipType.None, "Отношений нет" },
            { ARelationshipType.War,"В состоянии войны" },
            { ARelationshipType.Neutrality,"Нейтральные отношения" },
            { ARelationshipType.Union, "Союзнические отношения" },
        };

        public static IReadOnlyDictionary<ATechnologyType, string> Technologies = new Dictionary<ATechnologyType, string>() {
            { ATechnologyType.PrimitiveSociety, "Первобытное общество" },
            { ATechnologyType.HuntingAndGathering, "Охота и собирательство" },
            { ATechnologyType.Paganism, "Язычество" },
            { ATechnologyType.Agriculture, "Сельское хозяйство" },
            { ATechnologyType.Fishing, "Рыболовство" },
            { ATechnologyType.Masonry, "Каменная кладка" },
            { ATechnologyType.Phytotherapy, "Фитотерапия" },
            { ATechnologyType.Priesthood, "Духовенство" },
            { ATechnologyType.WoodProcessing, "Обработка древесины" },
            { ATechnologyType.StoneProcessing, "Обработка камня" },
            { ATechnologyType.Mining, "Шахтерство" },
            { ATechnologyType.Medicine, "Медицина" },
            { ATechnologyType.AnimalHusbandry, "Животноводство" },
            { ATechnologyType.BronzeProcessing, "Обработка бронзы" },
            { ATechnologyType.Writing, "Письменность" },
            { ATechnologyType.Handicraft, "Ремесленничество" },
            { ATechnologyType.CastingMetals, "Литье металлов" },
            { ATechnologyType.Alphabet, "Алфавит" },
            { ATechnologyType.MilitaryTraditions, "Воинские традиции" },
            { ATechnologyType.Mathematics, "Математика" },
            { ATechnologyType.TheatricalArt, "Театральное искусство" },
            { ATechnologyType.Сurrency, "Деньги" },
            { ATechnologyType.Building, "Строительство" },
            { ATechnologyType.Banking, "Банковское дело" },
            { ATechnologyType.CodeOfLaws, "Свод законов" },
            { ATechnologyType.Construction, "Конструирование" },
            { ATechnologyType.Education, "Образование" }
        };
    }
}