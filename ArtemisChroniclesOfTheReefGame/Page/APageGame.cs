using System;
using System.Collections.Generic;
using System.Linq;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Extension;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Character;
using GameLibrary.Unit;
using GameLibrary.Unit.Main;

using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;
using OnMouseEvent = GraphicsLibrary.StandartGraphicsPrimitives.APrimitive.OnMouseEvent;

using ArtemisChroniclesOfTheReefGame.Interface;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    public class APageGame : APage, IPage
    {

        private AGame COTR;

        private APanel MapField;

        private APanel PlayerStatisticPanel;
        private APanel ExtraPanel;
        private APanel GameTimeStatistic;

        private MapCellPanel mapCellPanel;
        private PlayerPanel PlayerPanel;
        private WorldPanel WorldPanel;

        private AButton Turn;

        private AMapView MapView;

        private APanel ButtonsPanel;
        private AScrolleredPanel ExtraStatisticPanel;
        private AScrolleredPanel UnitsPanel;

        private AContainer ButtonsPanelContainer;

        private APoint location;

        OnMouseEvent OnOverEvent;

        public APageGame(IPrimitive primitive) : base(primitive)
        {

            PlayerStatisticPanel = new APanel(new ASize(Parent.Width - 30 - 260, 50)) { Location = new APoint(10, 10) };
            GameTimeStatistic = new APanel(new ASize(200, 50)) { Location = new APoint(PlayerStatisticPanel.X + PlayerStatisticPanel.Width + 10, 10) };
            Turn = new AButton(new ASize(50, 50)) { Location = new APoint(GameTimeStatistic.X + GameTimeStatistic.Width + 10, 10) };
            MapField = new APanel(new ASize(Parent.Width - 20, Parent.Height - 80)) { Location = new APoint(10, PlayerStatisticPanel.Y + PlayerStatisticPanel.Height + 10) };
            ExtraPanel = new APanel(new ASize(Parent.Width - 20, 20)) { Location = MapField.Location + new APoint(0, MapField.Height - 20) };

            Add(MapField);
            Add(PlayerStatisticPanel);
            Add(GameTimeStatistic);
            Add(ExtraPanel);
            Add(Turn);

            COTR = new AGame();

            //
            ASize FormSize = new ASize(Convert.ToInt32(MapField.Width * (2f / 3)), Convert.ToInt32(MapField.Height * (2f / 3)));
            location = (MapField.Size - FormSize).ToAPoint() / 2;

            mapCellPanel = new MapCellPanel(COTR, FormSize) { Location = MapField.Location + location, DragAndDrop = true };
            PlayerPanel = new PlayerPanel(COTR, FormSize) { Location = MapField.Location + location, DragAndDrop = true };
            WorldPanel = new WorldPanel(COTR, FormSize) { Location = MapField.Location + location, DragAndDrop = true };

            Add(mapCellPanel);
            Add(PlayerPanel);
            Add(WorldPanel);

            MapField.FillColor = GraphicsExtension.DefaultDarkFillColor;

            ExtraPanel.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            ExtraPanel.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.DefaultFontFamilyName, 8);

            Turn.MouseClickEvent += (state, mstate) => { COTR.Turn(); Update(); };
            PlayerStatisticPanel.MouseClickEvent += (state, mstate) => { PlayerPanel.Update(COTR.ActivePlayer); PlayerPanel.Enabled = true; PlayerPanel.Location = MapField.Location + location; };
            GameTimeStatistic.MouseClickEvent += (state, mstate) => { WorldPanel.Update(); WorldPanel.Enabled = true; WorldPanel.Location = MapField.Location + location; };

        }

        private ASize CalculateMapViewSize(ASize MapSize)
        {
            return new ASize(26 + GraphicsExtension.DefaultMapCellRadius + Convert.ToInt32((MapSize.Width - 1) * GraphicsExtension.DefaultMapCellRadius * 1.55f + (MapSize.Height == 1 ? GraphicsExtension.DefaultMapCellRadius * 0.775f : 0)), GraphicsExtension.DefaultMapCellRadius + Convert.ToInt32((MapSize.Height - 1) * (GraphicsExtension.DefaultMapCellRadius / 2.2f)));
        }

        private APoint CalculateSpriteVector(APoint point)
        {
            return new APoint(13 + Convert.ToInt32(point.X * GraphicsExtension.DefaultMapCellRadius * 1.55f + (point.Y % 2 == 0 ? GraphicsExtension.DefaultMapCellRadius * 0.775f : 0)), Convert.ToInt32(point.Y * (GraphicsExtension.DefaultMapCellRadius / 2.2f)));
        }

        public void StartGame(List<string> players, List<ICharacter> characters, ASize mapSize)
        {

            COTR.Initialize(players, characters);

            COTR.StartGame(mapSize);

            PlayerStatisticPanel.Text = COTR.ActivePlayer.ToString();
            GameTimeStatistic.Text = "Ход: " + COTR.CurrentTurn;

            MapView = new AMapView(CalculateMapViewSize(mapSize)) { Parent = MapField, Location = new APoint(10, 10), DragAndDrop = true };

            MapView.MouseEnterEvent += OnOverEvent;

            //for (int y = 0; y < mapSize.Height; y++)
            //for (int x = 0; x < (y % 2 == 0 ? mapSize.Width - 1 : COTR.Size.Width); x++)
            foreach (var e in COTR.Map)
            {
                AMapCell mapCell = e.Value;
                if (mapCell.ActiveUnit is null && COTR.GetUnits(mapCell.Location) is List<IUnit> units && units.Count > 0) mapCell.SetActiveUnit(units.First());
                //AMapCell mapCell = COTR.GetMapCell(new APoint(x, y));
                APrimitiveTexture source = TexturePack.Hex[mapCell.OwnerId];
                AMapCellView mapCellView = new AMapCellView(mapCell, COTR, source) { Parent = MapView, Location = CalculateSpriteVector(/*new APoint(x, y)*/e.Key) };

                mapCellView.MouseClickEvent += (state, mouseState) =>
                {
                    if (mouseState.MouseButton.Equals(AMouseButton.Left))
                    {
                        COTR.SelectMapCell(mapCell.Location);
                        if (mapCellPanel.Enabled && (COTR.SelectedMapCell.IsSettlement || COTR.GetUnits().Count > 0))
                        {
                            mapCellPanel.Show();
                            mapCellPanel.Update(COTR.SelectedMapCell.Location);
                        }
                        else mapCellPanel.HideAll();
                    }
                    else if (mouseState.MouseButton.Equals(AMouseButton.Right))
                    {
                        if (/*COTR.IsSelectedUnit && */COTR.MoveUnit(mapCell.Location))
                        {
                            if (mapCellPanel.Enabled && (COTR.SelectedMapCell.IsSettlement || COTR.GetUnits().Count > 0))
                            {
                                mapCellPanel.Show();
                                mapCellPanel.Update(COTR.SelectedMapCell.Location);
                            }
                            else mapCellPanel.HideAll();
                            //UpdateMapCellInfo(mapCell.Location);
                            //UpdateUnitsButtonsInSelectedMaCell();
                            //UpdateActiveButtons();
                            //SelectActiveUnitButton(COTR.SelectedUnit);
                        }
                            //else UpdateUnitsButtonsInSelectedMaCell();
                        }
                };

                mapCellView.MouseEnterEvent += (state, mouseState) =>
                {
                    ExtraPanel.Text = "[" + mapCell.Location + "], " + GameLocalization.Biomes[mapCell.BiomeType] + ", " + GameLocalization.Resources[mapCell.ResourceType] + (mapCell.IsResource ? " (" + (mapCell.IsMined ? "Добывается" : "Не добывается") + ")" : "") + ", " + (mapCell.OwnerId == 0 ? "Пусто" : COTR.Players[mapCell.OwnerId].Name) + (mapCell.IsSettlement ? ", " + mapCell.Settlement.Name : "") + ", " + mapCell.Population.Total + " человек(а)";
                };

                mapCellView.KeyDownEvent += (state, kstate) =>
                {
                    if (kstate.KeyboardKey.Equals(AKeyboardKey.E) && (COTR.SelectedMapCell.IsSettlement || COTR.GetUnits(COTR.SelectedMapCell.Location).Count > 0))
                    {
                        mapCellPanel.Show();
                        mapCellPanel.Update(COTR.SelectedMapCell.Location);
                        mapCellPanel.Location = MapField.Location + location;
                    }
                    else mapCellPanel.HideAll();
                };

                mapCellView.MouseOverEvent += OnOverEvent;
            }

            mapCellPanel.Enabled = false;
            PlayerPanel.Enabled = false;
            WorldPanel.Enabled = false;

            WorldPanel.Update();

            MapView.MouseEnterEvent += (state, mstate) => { if (mstate.MouseButton.Equals(AMouseButton.Middle)) { mapCellPanel.HideAll(); PlayerPanel.Enabled = false; WorldPanel.Enabled = false; } };

            //foreach (IUnit unit in COTR.StartGame()) SetUnitButton(unit);

            //UpdatePlayerStatistic();

        }

        private void Update()
        {

            GameTimeStatistic.Text = "Ход: " + COTR.CurrentTurn;
            PlayerStatisticPanel.Text = COTR.ActivePlayer.ToString();

        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);
    }
}
