using System;
using System.Collections.Generic;
using System.Linq;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Extension;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Character;
using GameLibrary.Player;
using GameLibrary.Unit.Main;

using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using OnMouseEvent = GraphicsLibrary.Interfaces.OnMouseEvent;

using ArtemisChroniclesOfTheReefGame.Forms;
using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    public class APageCreateSinglePCGame : APage, IPage
    {

        public delegate void OnStartGame(List<string> players, List<ICharacter> characters, ASize mapSize);

        public event OnStartGame StartGameEvent;

        private Dictionary<string, ICharacter> Players;

        APlayerCreateForm PlayerCreateForm;
        APlayersList PlyersList;
        AButton AddPlayer;
        AButton StartGame;

        ALabeledScrollbar MapHeight;
        ALabeledScrollbar MapWidth;

        ASize MapSize;

        private string SelectedPlyaer;

        public APageCreateSinglePCGame(IPrimitive primitive) : base(primitive)
        {

            Players = new Dictionary<string, ICharacter>();

            ASize dSize = new ASize(Parent.Width / 2, Convert.ToInt32(Parent.Height * 2.25f / 3f));
            int dWidth = (Parent.Width - 30) / 2;

            PlayerCreateForm = new APlayerCreateForm(dSize) { Location = ((Parent.Size - dSize) / 2).ToAPoint()};
            PlyersList = new APlayersList(new ASize(dWidth, Parent.Height - 90)) { Location = new APoint(10, 10) };
            AddPlayer = new AButton(new ASize(dWidth, 60)) { Location = PlyersList.Location + new APoint(0, PlyersList.Height + 10), Text = "Добавить нового игрока" };

            MapHeight = new ALabeledScrollbar(new ASize(dWidth, 80)) { Location = PlyersList.Location + new APoint(PlyersList.Width + 10, 0) };
            MapWidth = new ALabeledScrollbar(new ASize(dWidth, 80)) { Location = MapHeight.Location + new APoint(0, MapHeight.Height + 10) };

            StartGame = new AButton(new ASize(dWidth, 60)) { Location = AddPlayer.Location + new APoint(AddPlayer.Width + 10, 0), Text = "Начать игру" };

            AddPlayer.MouseClickEvent += (state, mstate) =>
            {
                PlayerCreateForm.Show();
            };

            StartGame.MouseClickEvent += (state, mstate) =>
            {
                StartGameEvent?.Invoke(Players.Keys.ToList(), Players.Values.ToList(), MapSize);
            };

            PlyersList.SelectEvent += (player) =>
            {
                if (Players.ContainsKey(player))
                {
                    PlayerCreateForm.Show(Players[player], player);
                    SelectedPlyaer = player;
                }
            };

            PlyersList.ExtraSelectEvent += (player) =>
            {
                if (Players.ContainsKey(player)) Players.Remove(player);
                PlyersList.Update(Players.Keys);
            };

            PlayerCreateForm.DoneEvent += (playerName, characterName, characterFamilyName, age, sexType, attractiveness, education, martialSkills, health, fertility) =>
            {
                ICharacter character = new ACharacter(characterName, characterFamilyName, sexType, age, Players.Count + 1, Players.Count + 1);
                character.SetStats(attractiveness, education, martialSkills, health, fertility);
                if (SelectedPlyaer is object)
                {
                    if (!SelectedPlyaer.Equals(playerName))
                    {
                        Players.Remove(SelectedPlyaer);
                        Players[playerName] = character;
                    }
                    else
                    {
                        Players[SelectedPlyaer] = character;
                    }
                    SelectedPlyaer = null;
                }
                else Players.Add(Players.ContainsKey(playerName) ? playerName + Players.Keys.Where(x => x.Contains(playerName)).Count() : playerName, character);
                PlyersList.Update(Players.Keys);
            };

            MapHeight.ValueChange += (value) => MapSize.Height = value;
            MapWidth.ValueChange += (value) => MapSize.Width = value;

            Add(PlyersList);
            Add(AddPlayer);
            Add(StartGame);
            
            Add(MapHeight);
            Add(MapWidth);

            Add(PlayerCreateForm);

            MapHeight.Text = "Высота карты: ";
            MapHeight.MinValue = 5;
            MapHeight.MaxValue = 15;
            MapHeight.Value = MapHeight.MinValue;

            MapWidth.Text = "Ширина карты: ";
            MapWidth.MinValue = 8;
            MapWidth.MaxValue = 30;
            MapWidth.Value = MapWidth.MinValue;

        }

        public void Show()
        {

            Visible = true;
            Update();
            PlayerCreateForm.Hide();
            MapSize = new ASize(5, 8);

        }

        public void Hide()
        {

            Players.Clear();
            PlyersList.Clear();

            Visible = false;

        }

        public override void Update()
        {


        }

    }
}
