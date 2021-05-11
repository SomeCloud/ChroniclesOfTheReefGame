using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using CommonPrimitivesLibrary;

using AUnitType = GameLibrary.Unit.Main.AUnitType;
using ABiomeType = GameLibrary.Map.ABiomeType;
using AResourceType = GameLibrary.Map.AResourceType;

namespace ArtemisChroniclesOfTheReefGame
{
    public struct TexturePack
    {

        private static Dictionary<int, Color> _Colors;
        private static Dictionary<int, AHexTexture> _Hex;
        private static Dictionary<int, Texture2D> _Banners;

        public static IReadOnlyDictionary<int, Color> Colors { get => _Colors; }
        public static IReadOnlyDictionary<int, AHexTexture> Hex { get => _Hex; }
        public static IReadOnlyDictionary<int, Texture2D> Banners { get => _Banners; }

        public static Texture2D Empty;

        // Biome

        public static Texture2D Biome_Mountain_Triple;
        public static Texture2D Biome_Mountain_Double;
        public static Texture2D Biome_Mountain_Single;

        public static Texture2D Biome_Forest_Triple;
        public static Texture2D Biome_Forest_Double;
        public static Texture2D Biome_Forest_Single;

        public static Texture2D Biome_Desert_Triple;
        public static Texture2D Biome_Desert_Double;
        public static Texture2D Biome_Desert_Single;

        public static Texture2D Biome_Sea;

        // Construction

        public static Texture2D Construction_Village_Triple;
        public static Texture2D Construction_Village_Double;
        public static Texture2D Construction_Village_Single;
        public static Texture2D Construction_Village_Center;

        // Unit

        public static Texture2D Unit_Colonist;
        public static Texture2D Unit_Spearman;
        public static Texture2D Unit_Farmer;
        public static Texture2D Unit_Swordsman;
        public static Texture2D Unit_Archer;
        public static Texture2D Unit_Missionary;
        public static Texture2D Unit_Axeman;
        public static Texture2D Unit_Warrior;

        public static Texture2D Unit_Banner;

        // Resource

        public static Texture2D Resource_Copper_Ingot_Mined;
        public static Texture2D Resource_Iron_Ingot_Mined;
        public static Texture2D Resource_Gold_Ingot_Mined;
        public static Texture2D Resource_Silver_Ingot_Mined;
        public static Texture2D Resource_Marable_Mined;
        public static Texture2D Resource_Stone_Mined;
        public static Texture2D Resource_Wood_Mined;
        public static Texture2D Resource_Wheat_Mined;
        public static Texture2D Resource_Fish_Mined;

        public static Texture2D Resource_Copper_Ingot_NotMined;
        public static Texture2D Resource_Iron_Ingot_NotMined;
        public static Texture2D Resource_Gold_Ingot_NotMined;
        public static Texture2D Resource_Silver_Ingot_NotMined;
        public static Texture2D Resource_Marable_NotMined;
        public static Texture2D Resource_Stone_NotMined;
        public static Texture2D Resource_Wood_NotMined;
        public static Texture2D Resource_Wheat_NotMined;
        public static Texture2D Resource_Fish_NotMined;

        // Inteface

        public static Texture2D MiniButtons_General_Atack;
        public static Texture2D MiniButtons_General_Destroy;
        public static Texture2D MiniButtons_General_Expansion;
        public static Texture2D MiniButtons_General_Farming;
        public static Texture2D MiniButtons_General_FoundVillage;
        public static Texture2D MiniButtons_General_Resource_Extraction;
        public static Texture2D MiniButtons_General_Technology_Tree;
        public static Texture2D MiniButtons_General_Turn;

        public static Texture2D MiniButtons_Unit_Colonist;
        public static Texture2D MiniButtons_Unit_Spearman;
        public static Texture2D MiniButtons_Unit_Farmer;
        public static Texture2D MiniButtons_Unit_Swordsman;
        public static Texture2D MiniButtons_Unit_Archer;
        public static Texture2D MiniButtons_Unit_Missionary;
        public static Texture2D MiniButtons_Unit_Axeman;
        public static Texture2D MiniButtons_Unit_Warrior;

        public TexturePack(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {

            _Colors = new Dictionary<int, Color>() { };

            int dt = 64;

            /*for (int r = 16, k = 1; r < 224; r += dt)
                for (int g = 16; g < 224; g += dt)
                    for (int b = 16; b < 224; b += dt)
                    {
                        if (r != g && r != b && g != b)
                        {
                            _Colors.Add(k, new Color(r, g, b, 255));
                            k++;
                        }
                    }
            */

            List<Color> temp = new List<Color>();

            for (int r = 16; r < 224; r += dt)
                for (int g = 16; g < 224; g += dt)
                    for (int b = 16; b < 224; b += dt)
                    {
                        if (r != g && r != b && g != b)
                        {
                            temp.Add(new Color(r, g, b, 255));
                        }
                    }

            List<int> _index = Enumerable.Range(1, temp.Count).ToList();

            Random random = new Random((int)DateTime.Now.Ticks);

            foreach (Color color in temp)
            {
                int index = random.Next(0, _index.Count);
                _Colors.Add(_index[index], color);
                _index.RemoveAt(index);
            }

            _Colors.Add(0, Color.LightGray);

            _Hex = new Dictionary<int, AHexTexture>();
            _Banners = new Dictionary<int, Texture2D>();

            Empty = new Texture2D(graphicsDevice, GraphicsExtension.DefaultMapCellRadius, GraphicsExtension.DefaultMapCellRadius);

            // Biome

            Biome_Mountain_Triple = contentManager.Load<Texture2D>("Biome/Biome_Mountain_Triple");
            Biome_Mountain_Double = contentManager.Load<Texture2D>("Biome/Biome_Mountain_Double");
            Biome_Mountain_Single = contentManager.Load<Texture2D>("Biome/Biome_Mountain_Single");

            Biome_Forest_Triple = contentManager.Load<Texture2D>("Biome/Biome_Forest_Triple");
            Biome_Forest_Double = contentManager.Load<Texture2D>("Biome/Biome_Forest_Double");
            Biome_Forest_Single = contentManager.Load<Texture2D>("Biome/Biome_Forest_Single");

            Biome_Desert_Triple = contentManager.Load<Texture2D>("Biome/Biome_Desert_Triple");
            Biome_Desert_Double = contentManager.Load<Texture2D>("Biome/Biome_Desert_Double");
            Biome_Desert_Single = contentManager.Load<Texture2D>("Biome/Biome_Desert_Single");

            Biome_Sea = contentManager.Load<Texture2D>("Biome/Biome_Sea");

            // Construction

            Construction_Village_Triple = contentManager.Load<Texture2D>("Construction/Construction_Village_Triple");
            Construction_Village_Double = contentManager.Load<Texture2D>("Construction/Construction_Village_Double");
            Construction_Village_Single = contentManager.Load<Texture2D>("Construction/Construction_Village_Single");
            Construction_Village_Center = contentManager.Load<Texture2D>("Construction/Construction_Village_Center");

            // Unit

            Unit_Colonist = contentManager.Load<Texture2D>("Unit/Unit_Colonist");
            Unit_Spearman = contentManager.Load<Texture2D>("Unit/Unit_Spearman");
            Unit_Farmer = contentManager.Load<Texture2D>("Unit/Unit_Farmer");
            Unit_Swordsman = contentManager.Load<Texture2D>("Unit/Unit_Swordsman");
            Unit_Archer = contentManager.Load<Texture2D>("Unit/Unit_Archer");
            Unit_Missionary = contentManager.Load<Texture2D>("Unit/Unit_Missionary");
            Unit_Axeman = contentManager.Load<Texture2D>("Unit/Unit_Axeman");
            Unit_Warrior = contentManager.Load<Texture2D>("Unit/Unit_Warrior");

            Unit_Banner = contentManager.Load<Texture2D>("Unit/Unit_Banner");

            // Resource

            Resource_Copper_Ingot_Mined = contentManager.Load<Texture2D>("Resource/Resource_Copper_Ingot_Mined");
            Resource_Iron_Ingot_Mined = contentManager.Load<Texture2D>("Resource/Resource_Iron_Ingot_Mined");
            Resource_Gold_Ingot_Mined = contentManager.Load<Texture2D>("Resource/Resource_Gold_Ingot_Mined");
            Resource_Silver_Ingot_Mined = contentManager.Load<Texture2D>("Resource/Resource_Silver_Ingot_Mined");
            Resource_Marable_Mined = contentManager.Load<Texture2D>("Resource/Resource_Marable_Mined");
            Resource_Stone_Mined = contentManager.Load<Texture2D>("Resource/Resource_Stone_Mined");
            Resource_Wood_Mined = contentManager.Load<Texture2D>("Resource/Resource_Wood_Mined");
            Resource_Wheat_Mined = contentManager.Load<Texture2D>("Resource/Resource_Wheat_Mined");
            Resource_Fish_Mined = contentManager.Load<Texture2D>("Resource/Resource_Fish_Mined");

            Resource_Copper_Ingot_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Copper_Ingot_NotMined");
            Resource_Iron_Ingot_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Iron_Ingot_NotMined");
            Resource_Gold_Ingot_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Gold_Ingot_NotMined");
            Resource_Silver_Ingot_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Silver_Ingot_NotMined");
            Resource_Marable_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Marable_NotMined");
            Resource_Stone_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Stone_NotMined");
            Resource_Wood_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Wood_NotMined");
            Resource_Wheat_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Wheat_NotMined");
            Resource_Fish_NotMined = contentManager.Load<Texture2D>("Resource/Resource_Fish_NotMined");

            // Inteface

            MiniButtons_General_Atack = contentManager.Load<Texture2D>("Inteface/MiniButtons_General_Atack");
            MiniButtons_General_Destroy = contentManager.Load<Texture2D>("Inteface/MiniButtons_General_Destroy");
            MiniButtons_General_Expansion = contentManager.Load<Texture2D>("Inteface/MiniButtons_General_Expansion");
            MiniButtons_General_Farming = contentManager.Load<Texture2D>("Inteface/MiniButtons_General_Farming");
            MiniButtons_General_FoundVillage = contentManager.Load<Texture2D>("Inteface/MiniButtons_General_FoundVillage");
            MiniButtons_General_Resource_Extraction = contentManager.Load<Texture2D>("Inteface/MiniButtons_General_Resource_Extraction");
            MiniButtons_General_Technology_Tree = contentManager.Load<Texture2D>("Inteface/MiniButtons_General_Technology_Tree");
            MiniButtons_General_Turn = contentManager.Load<Texture2D>("Inteface/MiniButtons_General_Turn");

            MiniButtons_Unit_Colonist = contentManager.Load<Texture2D>("Inteface/MiniButtons_Unit_Colonist");
            MiniButtons_Unit_Spearman = contentManager.Load<Texture2D>("Inteface/MiniButtons_Unit_Spearman");
            MiniButtons_Unit_Farmer = contentManager.Load<Texture2D>("Inteface/MiniButtons_Unit_Farmer");
            MiniButtons_Unit_Swordsman = contentManager.Load<Texture2D>("Inteface/MiniButtons_Unit_Swordsman");
            MiniButtons_Unit_Archer = contentManager.Load<Texture2D>("Inteface/MiniButtons_Unit_Archer");
            MiniButtons_Unit_Missionary = contentManager.Load<Texture2D>("Inteface/MiniButtons_Unit_Missionary");
            MiniButtons_Unit_Axeman = contentManager.Load<Texture2D>("Inteface/MiniButtons_Unit_Axeman");
            MiniButtons_Unit_Warrior = contentManager.Load<Texture2D>("Inteface/MiniButtons_Unit_Warrior");

            // Extra

            ATexture texture = new ARectangleTexture(graphicsDevice, new ASize(GraphicsExtension.DefaultMapCellRadius, GraphicsExtension.DefaultMapCellRadius)) { IsDraw = true, IsFill = true };

            foreach (var value in _Colors)
            {
                AHexTexture hexTexture = new AHexTexture(graphicsDevice, GraphicsExtension.DefaultMapCellRadius, value.Value, GraphicsExtension.DefaultDarkBorderColor) { IsDraw = true, IsFill = true };

                _Hex.Add(value.Key, hexTexture);

                texture.FillBySource(Unit_Banner);
                texture.ReplacePixels(Color.White, value.Value);
                Color[] colors = new Color[texture.Height * texture.Width];
                texture.GetData(colors);
                _Banners.Add(value.Key, texture.Clone());
            }
        }

        public static Texture2D Biome(ABiomeType biome, bool isUnit, bool isConstruction)
        {
            switch (biome)
            {
                case ABiomeType.Forest:
                    return isUnit ? isConstruction ? Biome_Mountain_Single : Biome_Forest_Double : isConstruction ? Biome_Forest_Double : Biome_Forest_Triple;
                case ABiomeType.Mountain:
                    return isUnit ? isConstruction ? Biome_Mountain_Single : Biome_Mountain_Double : isConstruction ? Biome_Mountain_Double : Biome_Mountain_Triple;
                case ABiomeType.Desert:
                    return isUnit ? isConstruction ? Biome_Desert_Single : Biome_Desert_Double : isConstruction ? Biome_Desert_Double : Biome_Desert_Triple;
                case ABiomeType.Sea:
                    return Biome_Sea;
                default: return Empty;
            }
        }

        public static Texture2D Unit(AUnitType unitType)
        {
            switch (unitType)
            {
                case AUnitType.Colonist:
                    return Unit_Colonist;
                case AUnitType.Spearman:
                    return Unit_Spearman;
                case AUnitType.Farmer:
                    return Unit_Farmer;
                case AUnitType.Swordsman:
                    return Unit_Swordsman;
                case AUnitType.Archer:
                    return Unit_Archer;
                case AUnitType.Missionary:
                    return Unit_Missionary;
                case AUnitType.Axeman:
                    return Unit_Axeman;
                case AUnitType.Warrior:
                    return Unit_Warrior;
                default:
                    return Empty;
            }
        }

        public static Texture2D Construction(/*AConstructionType constructionType, */ABiomeType biome, bool isUnit)
        {
            /*
            switch (constructionType)
            {
                case AConstructionType.Village:
                    return isUnit ? biome.Equals(ABiomeType.Plain) ? Construction_Village_Double : Construction_Village_Single : biome.Equals(ABiomeType.Plain) ? Construction_Village_Triple : Construction_Village_Center;
                default:
                    return Empty;
            }*/
            return isUnit ? biome.Equals(ABiomeType.Plain) ? Construction_Village_Double : Construction_Village_Single : biome.Equals(ABiomeType.Plain) ? Construction_Village_Triple : Construction_Village_Center;
        }

        public static Texture2D Resource(AResourceType resource, bool isActive)
        {
            switch (resource)
            {
                case AResourceType.Iron:
                    return isActive ? Resource_Iron_Ingot_Mined : Resource_Iron_Ingot_NotMined;
                case AResourceType.Copper:
                    return isActive ? Resource_Copper_Ingot_Mined : Resource_Copper_Ingot_NotMined;
                case AResourceType.Gold:
                    return isActive ? Resource_Gold_Ingot_Mined : Resource_Gold_Ingot_NotMined;
                case AResourceType.Silver:
                    return isActive ? Resource_Silver_Ingot_Mined : Resource_Silver_Ingot_NotMined;
                case AResourceType.Stone:
                    return isActive ? Resource_Stone_Mined : Resource_Stone_NotMined;
                case AResourceType.Marable:
                    return isActive ? Resource_Marable_Mined : Resource_Marable_NotMined;
                case AResourceType.Wood:
                    return isActive ? Resource_Wood_Mined : Resource_Wood_NotMined;
                case AResourceType.Fish:
                    return isActive ? Resource_Fish_Mined : Resource_Fish_NotMined;
                case AResourceType.Wheat:
                    return isActive ? Resource_Wheat_Mined : Resource_Wheat_NotMined;
                default:
                    return Empty;
            }
        }



    }

}
