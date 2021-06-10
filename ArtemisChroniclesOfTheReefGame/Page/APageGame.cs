using System;
using System.Collections.Generic;
using System.Linq;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Extension;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Characteristic;
using GameLibrary.Character;
using GameLibrary.Message;
using GameLibrary.Player;
using GameLibrary.Unit;
using GameLibrary.Unit.Main;

using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using OnMouseEvent = GraphicsLibrary.Interfaces.OnMouseEvent;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    public class APageGame : APage, IPage
    {

        public delegate void OnBack();

        public event OnBack BackEvent;
        private OnGameOver GameOverEvent;

        private AGame Game;

        private GamePanel GamePanel;

        private AGameOverPanel GameOverPanel;

        public APageGame(IPrimitive primitive) : base(primitive)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            GamePanel = new GamePanel(Parent.Size) { Location = new APoint(0, 0), IsCounting = true, DTimer = 3 };
            GameOverPanel = new AGameOverPanel(Parent.Size) { Location = new APoint(0, 0) };

            Add(GamePanel);
            Add(GameOverPanel);

            GameOverPanel.BackEvent += () =>
            {
                GameOverPanel.Hide();
                BackEvent?.Invoke();
            };

            GamePanel.TurnClickEvent += () => {
                Game.Turn();
                //Console.WriteLine("SERVER: Переход хода к игроку " + Game.ActivePlayer.Name);
                GamePanel.Update(Game, Game.ActivePlayer.Name);
            };

            GamePanel.SelectUnitEvent += (unit) => {
                if (unit is object)
                {
                    Game.GetMapCell(unit.Location).SetActiveUnit(unit);
                    Console.WriteLine("SERVER: Юнит " + unit.Name + " выделен");
                    GamePanel.Update(Game, Game.ActivePlayer.Name);
                }
            };

            GamePanel.SelectTechnologyEvent += (technology) => {
                if (technology is object && Game.SelectedMapCell.Owner.Name.Equals(Game.ActivePlayer.Name))
                {
                    Game.SelectedMapCell.Settlement?.SetInvestigatedTechnology(technology);
                    Console.WriteLine("SERVER: Выбрана технология " + GameLocalization.Technologies[technology.TechnologyType]);
                    GamePanel.Update(Game, Game.ActivePlayer.Name);
                }
            };

            GamePanel.MarryEvent += (character) => { /* nothing */ };
            GamePanel.DivorceEvent += (character) => {
                Game.Divorce(character);
            };
            GamePanel.AgreementEvent += (character) => { /* nothing */ };
            GamePanel.HeirEvent += (character) => { /* nothing */ };
            GamePanel.WarEvent += (character) =>
            {
                if (Game.CharacterIsRuler(character, out IPlayer player) && !player.Equals(Game.ActivePlayer) && new ARelationshipType[] { ARelationshipType.Neutrality, ARelationshipType.None }.Contains(player.Relationship(Game.ActivePlayer)) && character.IsAlive)
                {
                    IMessage message = new AMessageNotification(Game.ActivePlayer, player, "Объявление войны", "Игрок " + Game.ActivePlayer.Name + " объявляет войну игроку " + player.Name);
                    player.SendMessage(message);
                    Game.SetRelationship(player, Game.ActivePlayer, ARelationshipType.War);
                    Console.WriteLine("SERVER: Объявление войны игроком " + Game.ActivePlayer.Name + " игроку " + player.Name);
                    GamePanel.Update(Game, Game.ActivePlayer.Name);
                }
            };

            GamePanel.PeaceEvent += (character) => {
                if (Game.CharacterIsRuler(character, out IPlayer player) && !player.Equals(Game.ActivePlayer) && player.Relationship(Game.ActivePlayer).Equals(ARelationshipType.War) && character.IsAlive)
                {
                    IMessage message = new AMessagePeace(Game.ActivePlayer, player, "Предложение мира", "Игрок " + Game.ActivePlayer.Name + " предлагает заключить мир игроку " + player.Name);
                    player.SendMessage(message);
                    Console.WriteLine("SERVER: Заключение мира между игроком " + Game.ActivePlayer.Name + " игроку " + player.Name);
                    GamePanel.Update(Game, Game.ActivePlayer.Name);
                }
            };

            GamePanel.UnionEvent += (character) => {
                if (Game.CharacterIsRuler(character, out IPlayer player) && !player.Equals(Game.ActivePlayer) && new ARelationshipType[] { ARelationshipType.Neutrality, ARelationshipType.None }.Contains(player.Relationship(Game.ActivePlayer)) && character.IsAlive)
                {
                    IMessage message = new AMessageUnion(Game.ActivePlayer, player, "Предложение союза", "Игрок " + Game.ActivePlayer.Name + " предлагает заключить союз игроку " + player.Name);
                    player.SendMessage(message);
                    Console.WriteLine("SERVER: Заключение союза между игроком " + Game.ActivePlayer.Name + " игроку " + player.Name);
                    GamePanel.Update(Game, Game.ActivePlayer.Name);
                }
            };

            GamePanel.BreakUnionEvent += (character) => {
                if (Game.CharacterIsRuler(character, out IPlayer player) && !player.Equals(Game.ActivePlayer) && player.Relationship(Game.ActivePlayer).Equals(ARelationshipType.Union) && character.IsAlive)
                {
                    IMessage message = new AMessageNotification(Game.ActivePlayer, player, "Разрыв союза", "Игрок " + Game.ActivePlayer.Name + " разрывает союз с игроком " + player.Name);
                    player.SendMessage(message);
                    Game.SetRelationship(player, Game.ActivePlayer, ARelationshipType.Neutrality);
                    Console.WriteLine("SERVER: Разрыв союза между игроком " + Game.ActivePlayer.Name + " игроку " + player.Name);
                    GamePanel.Update(Game, Game.ActivePlayer.Name);
                }
            };

            GamePanel.BuildingCreateEvent += (building) => {
                if (building is object && Game.SelectedMapCell.Owner.Name.Equals(Game.ActivePlayer.Name)) Game.SelectedMapCell.Settlement?.StartBuilding(building);
                Console.WriteLine("SERVER: Начато строительство здания " + GameLocalization.Buildings[building.BuildingType]);
                GamePanel.Update(Game, Game.ActivePlayer.Name);
            };
            GamePanel.UnitCreateEvent += (unitType) => {
                if (Game.SelectedMapCell.IsSettlement)
                {
                    List<APeople> squad = Game.GetMapCell(Game.SelectedMapCell.Location).Population.Subtract(100);
                    if (!Game.AddUnit(unitType, squad, GameLocalization.UnitName[unitType]))
                    {
                        Game.GetMapCell(Game.SelectedMapCell.Location).Population.Add(squad);
                        Console.WriteLine("SERVER: Создан отряд юнитов " + GameLocalization.UnitName[unitType]);
                        GamePanel.Update(Game, Game.ActivePlayer.Name);
                    }
                }
            };

            GamePanel.RenameEvent += (unit) => { };
            GamePanel.DestroyEvent += (unit) => {
                if (Game.GetUnit(unit) is IUnit Unit && Unit is object)
                {
                    Game.GetMapCell(Unit.Homeland).Population.Add(Unit.Squad.ToList());
                    AMapCell mapCell = Game.GetMapCell(Unit.Location);
                    Unit.Owner.RemoveUnit(Unit);
                    mapCell.SetActiveUnit(Game.GetUnits(mapCell.Location) is List<IUnit> units && units.Count > 0 ? units.First() : null);
                    Unit.Dispose();
                    Unit = null;
                    Console.WriteLine("SERVER: Расформирован отряд юнитов " + unit.Name);
                    GamePanel.Update(Game, Game.ActivePlayer.Name);
                }
            };
            GamePanel.GeneralEvent += (unit) => { };
            GamePanel.WorkEvent += (unit) => {
                if (Game.GetUnit(unit) is IUnit e && e is object)
                {
                    if (Game.GetMapCell(e.Location) is AMapCell mapCell && mapCell.IsResource)
                    {
                        if (mapCell.IsMined) mapCell.UnsetActiveWorker();
                        else mapCell.SetActiveWorker(e);
                        Console.WriteLine("SERVER: Использован отряд юнитов " + unit.Name);
                        GamePanel.Update(Game, Game.ActivePlayer.Name);
                    }
                }
            };
            GamePanel.EstablishEvent += (unit) => {
                if (Game.GetUnit(unit) is IUnit dUnit && dUnit is object)
                {
                    Random random = new Random((int)DateTime.Now.Ticks);
                    Game.AddSettlement(dUnit, GameExtension.SettlementName[random.Next(GameExtension.SettlementName.Count)]);
                    Console.WriteLine("SERVER: Отряд юнитов " + unit.Name + " основывает поселение");
                    GamePanel.Update(Game, Game.ActivePlayer.Name);
                }
            };

            GamePanel.MapCellSelectEvent += (location) => {
                Game.SelectMapCell(location);
                Console.WriteLine("SERVER: Выделена клетка в координатах " + location);
                GamePanel.Update(Game, Game.ActivePlayer.Name);
            };
            GamePanel.MoveUnitEvent += (location) => {
                if (Game.SelectedMapCell.ActiveUnit is object && Game.MoveUnit(location))
                {
                    Game.SelectMapCell(location);
                    Console.WriteLine("SERVER: Юнит перемещен в координаты " + location);
                }
                GamePanel.Update(Game, Game.ActivePlayer.Name);
            };
            GamePanel.DoneMessageEvent += (message) =>
            {
                message.Done(Game);
                GamePanel.Update(Game, Game.ActivePlayer.Name);
            };
            GamePanel.RenouncementMessageEvent += (message) =>
            {
                message.Renouncement(Game);
                GamePanel.Update(Game, Game.ActivePlayer.Name);
            };
            GamePanel.MarriageEvent += (character, spouse, isMatrilinearMarriage) =>
            {
                IPlayer characterOwner = null;
                IPlayer spouseOwner = null;

                if (Game.CharacterIsRuler(character, out characterOwner) && Game.CharacterIsRuler(spouse, out spouseOwner))
                {
                    if (characterOwner.Equals(Game.ActivePlayer) && spouseOwner.Equals(Game.ActivePlayer))
                    {
                        Game.Marry(character, spouse, isMatrilinearMarriage);
                    }
                    else if (characterOwner.Equals(Game.ActivePlayer))
                    {
                        IMessage message = new AMessageMarriage(Game.ActivePlayer, spouseOwner, "Заключение брака", "Игрок " + Game.ActivePlayer.Name + " предалагает игроку " + spouseOwner.Name + " заключить брак между персонажами: \n" + character.FullName + "(" + characterOwner.Name + ") и " + spouse.FullName + "(" + spouseOwner.Name + ")\nс условием наследования по линии " + (isMatrilinearMarriage? "матери": "отца"), character, spouse, isMatrilinearMarriage);
                        spouseOwner.SendMessage(message);
                    }
                    else if (spouseOwner.Equals(Game.ActivePlayer))
                    {
                        IMessage message = new AMessageMarriage(Game.ActivePlayer, characterOwner, "Заключение брака", "Игрок " + Game.ActivePlayer.Name + " предалагает игроку " + characterOwner.Name + " заключить брак между персонажами: \n" + character.FullName + "(" + characterOwner.Name + ") и " + spouse.FullName + "(" + spouseOwner.Name + ")\nс условием наследования по линии " + (isMatrilinearMarriage ? "матери" : "отца"), character, spouse, isMatrilinearMarriage);
                        characterOwner.SendMessage(message);
                    }
                    else
                    {
                        characterOwner.SendMessage(new AMessageMarriage(Game.ActivePlayer, characterOwner, "Заключение брака", "Игрок " + Game.ActivePlayer.Name + " предалагает игроку " + characterOwner.Name + " заключить брак между персонажами: \n" + character.FullName + "(" + characterOwner.Name + ") и " + spouse.FullName + "(" + spouseOwner.Name + ")\nс условием наследования по линии " + (isMatrilinearMarriage ? "матери" : "отца"), character, spouse, isMatrilinearMarriage));
                        spouseOwner.SendMessage(new AMessageMarriage(Game.ActivePlayer, spouseOwner, "Заключение брака", "Игрок " + Game.ActivePlayer.Name + " предалагает игроку " + spouseOwner.Name + " заключить брак между персонажами: \n" + character.FullName + "(" + characterOwner.Name + ") и " + spouse.FullName + "(" + spouseOwner.Name + ")\nс условием наследования по линии " + (isMatrilinearMarriage ? "матери" : "отца"), character, spouse, isMatrilinearMarriage));
                    }
                }
                else
                {
                    if (Game.CharacterIsRuler(character, out characterOwner))
                    {
                        if (characterOwner.Equals(Game.ActivePlayer))
                        {
                            Game.Marry(character, spouse, isMatrilinearMarriage);
                        }
                        else
                        {
                            IMessage message = new AMessageMarriage(Game.ActivePlayer, characterOwner, "Заключение брака", "Игрок " + Game.ActivePlayer.Name + " предалагает игроку " + characterOwner.Name + " заключить брак между персонажами: \n" + character.FullName + "(" + characterOwner.Name + ") и " + spouse.FullName + "\nс условием наследования по линии " + (isMatrilinearMarriage ? "матери" : "отца"), character, spouse, isMatrilinearMarriage);
                            characterOwner.SendMessage(message);
                        }
                    }
                    else if (Game.CharacterIsRuler(spouse, out spouseOwner))
                    {
                        if (spouseOwner.Equals(Game.ActivePlayer))
                        {
                            Game.Marry(character, spouse, isMatrilinearMarriage);
                        }
                        else
                        {
                            IMessage message = new AMessageMarriage(Game.ActivePlayer, spouseOwner, "Заключение брака", "Игрок " + Game.ActivePlayer.Name + " предалагает игроку " + spouseOwner.Name + " заключить брак между персонажами: \n" + character.FullName + " и " + spouse.FullName + "(" + spouseOwner.Name + ")\n с условием наследования по линии " + (isMatrilinearMarriage ? "матери" : "отца"), character, spouse, isMatrilinearMarriage);
                            spouseOwner.SendMessage(message);
                        }
                    }
                    else if (Game.ActivePlayer.Characters.Contains(character) || Game.ActivePlayer.Characters.Contains(spouse))
                    {
                        Game.Marry(character, spouse, isMatrilinearMarriage);
                    }
                    else if (Math.Abs(character.Attractiveness - spouse.Attractiveness) is int dt && (dt.Equals(0) || random.Next().Equals(0))) Game.Marry(character, spouse, isMatrilinearMarriage);
                }
                GamePanel.Update(Game, Game.ActivePlayer.Name);
            };
        }

        public void StartGame(List<string> players, List<ICharacter> characters, ASize mapSize)
        {

            if (Game is object) Game.GameOverEvent -= GameOverEvent;

            Game = new AGame();

            GameOverEvent = new OnGameOver((winner) => {
                GameOverPanel.Show(winner, Game.Players.Values.ToList());
            });

            Game.GameOverEvent += GameOverEvent;

            Game.Initialize(players, characters);
            Game.StartGame(mapSize);
            GamePanel.Update(Game, Game.ActivePlayer.Name);
        }

        public void Show()
        {

            Visible = true;
            Update();
            GameOverPanel.Hide();

        }

        public void Hide()
        {

            Visible = false;

        }

        public override void Update()
        {

           

        }

    }
}
