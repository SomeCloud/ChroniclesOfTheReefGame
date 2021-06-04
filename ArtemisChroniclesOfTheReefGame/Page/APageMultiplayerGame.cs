using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using NetLibrary;

using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using GameLibrary;
using GameLibrary.Unit.Main;
using GameLibrary.Technology;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Building;
using GameLibrary.Player;
using GameLibrary.Character;
using GameLibrary.Map;
using GameLibrary.Message;
using GameLibrary.Extension;

using CommonPrimitivesLibrary;

using ArtemisChroniclesOfTheReefGame.Panels;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    class APageMultiplayerGame : APage, IPage
    {

        private AServer CServer;
        private AClient CClient;

        private AServer SServer;
        private AClient SClient;

        private ARoom Room;
        private RPlayer Player;

        private GamePanel GamePanel;
        //private AEmptyPanel Menu;

        private bool IsServer;

        private AFrame ClientFrame;
        private AFrame ServerFrame;

        private AGame Game;

        private bool isSend;

        Thread ServerReceiver;
        Thread ClientReceiver;

        Thread ServerSender;
        Thread ClientSender;

        private bool IsClientReceive;
        private bool IsServerReceive;

        public APageMultiplayerGame(IPrimitive primitive) : base(primitive)
        {

            CServer = new AServer("224.0.0.0", 8002);
            CClient = new AClient("224.0.0.0", 8001);

            SServer = new AServer("224.0.0.0", 8001);
            SClient = new AClient("224.0.0.0", 8002);

            ServerReceiver = null;
            ClientReceiver = null;

            ServerSender = null;
            ClientSender = null;

            GamePanel = new GamePanel(Parent.Size) { Location = new APoint(0, 0), IsCounting = true, DTimer = 2 };
            //Menu = new AEmptyPanel(Parent.Size) { Location = new APoint(0, 0), IsInteraction = false, IsCounting = true, DTimer = 1 };

            Add(GamePanel);
            //Add(Menu);

            GamePanel.TurnClickEvent += () => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает завершение хода");
                ClientFrame = new AFrame( Room.Id, new AData(null, Player, ADataType.Turn), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };

            GamePanel.SelectUnitEvent += (unit) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает выделения юнита " + unit.Name);
                ClientFrame = new AFrame(Room.Id, new AData(unit, Player, ADataType.SelectUnit), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
            };

            GamePanel.SelectTechnologyEvent += (technology) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает выбора технологии " + GameLocalization.Technologies[technology.TechnologyType]);
                ClientFrame = new AFrame(Room.Id, new AData(technology, Player, ADataType.SelectTechnology), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };

            GamePanel.MarryEvent += (character) => { /* nothing */ };
            GamePanel.AgreementEvent += (character) => { /* nothing */ };
            GamePanel.HeirEvent += (character) => { /* nothing */ };
            GamePanel.WarEvent += (character) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает объявления войны персонажу " + character.FullName);
                ClientFrame = new AFrame(Room.Id, new AData(character, Player, ADataType.War), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };
            GamePanel.PeaceEvent += (character) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает заключения мира с персонажем " + character.FullName);
                ClientFrame = new AFrame(Room.Id, new AData(character, Player, ADataType.Peace), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };
            GamePanel.UnionEvent += (character) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает заключения союза с персонажем " + character.FullName);
                ClientFrame = new AFrame(Room.Id, new AData(character, Player, ADataType.Union), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };
            GamePanel.BreakUnionEvent += (character) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает разрыв союза с персонажем " + character.FullName);
                ClientFrame = new AFrame(Room.Id, new AData(character, Player, ADataType.BreakUnion), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };

            GamePanel.BuildingCreateEvent += (building) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает начало строительства здания " + GameLocalization.Buildings[building.BuildingType]);
                ClientFrame = new AFrame(Room.Id, new AData(building, Player, ADataType.BuildingCreate), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };
            GamePanel.UnitCreateEvent += (unitType) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает создания отряда юнитов " + GameLocalization.UnitName[unitType]);
                ClientFrame = new AFrame(Room.Id, new AData(unitType, Player, ADataType.UnitCreate), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };

            GamePanel.RenameEvent += (unit) => { };
            GamePanel.DestroyEvent += (unit) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает расформирования отряда юнитов " + unit.Name);
                ClientFrame = new AFrame(Room.Id, new AData(unit, Player, ADataType.DestroyUnit), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };
            GamePanel.GeneralEvent += (unit) => { };
            GamePanel.WorkEvent += (unit) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает использование отряда юнитов " + unit.Name);
                ClientFrame = new AFrame(Room.Id, new AData(unit, Player, ADataType.WorkUnit), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };
            GamePanel.EstablishEvent += (unit) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает основание поселения отрядом юнитов " + unit.Name);
                ClientFrame = new AFrame(Room.Id, new AData(unit, Player, ADataType.EstablishUnit), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };

            GamePanel.MapCellSelectEvent += (location) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает выделения клетки в координатах " + location);
                ClientFrame = new AFrame(Room.Id, new AData(location, Player, ADataType.MapCellSelect), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };
            GamePanel.MoveUnitEvent += (location) => {
                Console.WriteLine("CLIENT: Игрок " + Player.Name + " запрашивает перемещения активного юнита в координатах " + location);
                ClientFrame = new AFrame(Room.Id, new AData(location, Player, ADataType.MoveUnit), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                isSend = true;
            };

            GamePanel.TimeEvent += () =>
            {
                if (IsClientReceive)
                {
                    ClientReceiver?.Abort();
                    ClientReceiver = new Thread(() => CClient.ReceiveResult()) { Name = "Client-Receiver", IsBackground = true };
                    ClientReceiver.Start();
                    IsClientReceive = false;
                }
                //ClientReceiver.Join();
            };

            GamePanel.TimeEvent += () =>
            {
                if (IsServer && !SServer.InSend) Send(new AFrame(Room.Id, Room, AMessageType.RoomInfo, SClient.LocalIPAddress(), CClient.GroupIPAdress.ToString()));
            };

            GamePanel.TimeEvent += () =>
            {
                if (Room.Game.ActivePlayer.Name.Equals(Player.Name))
                {
                    if (IsServer)
                    {
                        OnServerReceive(ClientFrame);
                        ClientFrame = new AFrame(Room.Id, new AData(null, Player, ADataType.None), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                    }
                    else
                    {
                        ClientSender?.Abort();
                        ClientSender = new Thread(() => CServer?.SendFrame(ClientFrame)) { Name = "Client-Sender", IsBackground = true };
                        ClientSender.Start();
                    }
                    isSend = false;
                }
            };

            GamePanel.TimeEvent += () =>
            {
                if (IsServer && (IsServerReceive || !SClient.InReceive))
                {
                    ServerReceiver?.Abort();
                    SClient.Reset();
                    ServerReceiver = new Thread(() => SClient.ReceiveResult()) { Name = "Server-Receiver", IsBackground = true };
                    ServerReceiver.Start();
                    IsServerReceive = false;
                    //ServerReceiver.Join();
                }
            };

            GamePanel.DrawEvent += () =>
            {
                if (SClient.IsComleted) OnServerReceive(SClient.Result);
            };
            
            GamePanel.DrawEvent += () =>
            {
                if (CClient.IsComleted) OnClientReceive(CClient.Result);
            };

        }

        private void Send(AFrame frame)
        {
            ServerSender?.Abort();
            SServer.Reset();
            ServerSender = new Thread(() => SServer?.SendFrame(frame)) { Name = "Server-Sender", IsBackground = true };
            ServerSender.Start();
        }

        private void OnClientReceive(AFrame frame)
        {
            if (frame.MessageType.Equals(AMessageType.RoomInfo))
            {
                ARoom room = frame.Data as ARoom;
                if (room is object) GamePanel.Update(room.Game, Player.Name);
            }
            IsClientReceive = true;
        }

        private void OnServerReceive(AFrame frame)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
         
            if (frame.MessageType.Equals(AMessageType.Request) && IsServer)
            {
                AData request = frame.Data as AData;
                if (request is object && Game.ActivePlayer.Name.Equals(request.Player.Name))
                {
                    IUnit unit;
                    ITechnology technology;
                    IPlayer player;
                    ICharacter character;
                    APoint location;

                    bool IsChanged = true;

                    switch (request.PackageType)
                    {
                        case ADataType.Turn:
                            Game.Turn();
                            Console.WriteLine("SERVER: Переход хода к игроку" + Game.ActivePlayer.Name);
                            break;
                        case ADataType.SelectUnit:
                            unit = request.Package as IUnit;
                            if (unit is object) Game.GetMapCell(unit.Location).SetActiveUnit(unit);
                            Console.WriteLine("SERVER: Юнит " + unit.Name + " выделен");
                            break;
                        case ADataType.SelectTechnology:
                            technology = request.Package as ITechnology;
                            if (technology is object && Game.SelectedMapCell.Owner.Name.Equals(Player.Name)) Game.SelectedMapCell.Settlement?.SetInvestigatedTechnology(technology);
                            Console.WriteLine("SERVER: Выбрана технология " + GameLocalization.Technologies[technology.TechnologyType]);
                            break;
                        case ADataType.War:
                            character = request.Package as ICharacter;
                            if (character is object && Game.CharacterIsRuler(character, out player) && !player.Equals(Game.ActivePlayer) && new ARelationshipType[] { ARelationshipType.Neutrality, ARelationshipType.None }.Contains(player.Relationship(Game.ActivePlayer)) && character.IsAlive)
                            {
                                IMessage message = new AMessage(Game.ActivePlayer, player, "Объявление войны", "Игрок " + Game.ActivePlayer.Name + " объявляет войну игроку " + player.Name, () => { }, false);
                                player.SendMessage(message);
                                Game.SetRelationship(player, Game.ActivePlayer, ARelationshipType.War);
                                Console.WriteLine("SERVER: Объявление войны игроком " + Game.ActivePlayer.Name + " игроку " + player.Name);
                            }
                            break;
                        case ADataType.Peace:
                            character = request.Package as ICharacter;
                            if (character is object && Game.CharacterIsRuler(character, out player) && !player.Equals(Game.ActivePlayer) && player.Relationship(Game.ActivePlayer).Equals(ARelationshipType.War) && character.IsAlive)
                            {
                                IMessage message = new AMessage(Game.ActivePlayer, player, "Предложение мира", "Игрок " + Game.ActivePlayer.Name + " предлагает заключить мир игроку " + player.Name, () => Game.SetRelationship(player, Game.ActivePlayer, ARelationshipType.Neutrality), true);
                                player.SendMessage(message);
                                Console.WriteLine("SERVER: Заключение мира между игроком " + Game.ActivePlayer.Name + " игроку " + player.Name);
                            }
                            break;
                        case ADataType.Union:
                            character = request.Package as ICharacter;
                            if (character is object && Game.CharacterIsRuler(character, out player) && !player.Equals(Game.ActivePlayer) && new ARelationshipType[] { ARelationshipType.Neutrality, ARelationshipType.None }.Contains(player.Relationship(Game.ActivePlayer)) && character.IsAlive)
                            {
                                IMessage message = new AMessage(Game.ActivePlayer, player, "Предложение союза", "Игрок " + Game.ActivePlayer.Name + " предлагает заключить союз игроку " + player.Name, () => Game.SetRelationship(player, Game.ActivePlayer, ARelationshipType.Union), true);
                                player.SendMessage(message);
                                Console.WriteLine("SERVER: Заключение союза между игроком " + Game.ActivePlayer.Name + " игроку " + player.Name);
                            }
                            break;
                        case ADataType.BreakUnion:
                            character = request.Package as ICharacter;
                            if (character is object && Game.CharacterIsRuler(character, out player) && !player.Equals(Game.ActivePlayer) && player.Relationship(Game.ActivePlayer).Equals(ARelationshipType.Union) && character.IsAlive)
                            {
                                IMessage message = new AMessage(Game.ActivePlayer, player, "Разрыв союза", "Игрок " + Game.ActivePlayer.Name + " разрывает союз с игроком " + player.Name, () => { }, false);
                                player.SendMessage(message);
                                Game.SetRelationship(player, Game.ActivePlayer, ARelationshipType.Neutrality);
                                Console.WriteLine("SERVER: Разрыв союза между игроком " + Game.ActivePlayer.Name + " игроку " + player.Name);
                            }
                            break;
                        case ADataType.BuildingCreate:
                            IBuilding building = request.Package as IBuilding;
                            if (building is object && Game.SelectedMapCell.Owner.Name.Equals(Player.Name)) Game.SelectedMapCell.Settlement?.StartBuilding(building);
                            Console.WriteLine("SERVER: Начато строительство здания " + GameLocalization.Buildings[building.BuildingType]);
                            break;
                        case ADataType.UnitCreate:
                            AUnitType unitType = (AUnitType)request.Package;
                            if (Game.SelectedMapCell.IsSettlement)
                            {
                                List<APeople> squad = Game.GetMapCell(Game.SelectedMapCell.Location).Population.Subtract(100);
                                if (!Game.AddUnit(unitType, squad, GameLocalization.UnitName[unitType]))
                                {
                                    Game.GetMapCell(Game.SelectedMapCell.Location).Population.Add(squad);
                                    Console.WriteLine("SERVER: Создан отряд юнитов " + GameLocalization.UnitName[unitType]);
                                }
                            }
                            break;
                        case ADataType.RenameUnit:
                            break;
                        case ADataType.DestroyUnit:
                            unit = request.Package as IUnit;
                            if (Game.GetUnit(unit) is IUnit Unit && Unit is object)
                            {
                                Game.GetMapCell(Unit.Homeland).Population.Add(Unit.Squad.ToList());
                                AMapCell mapCell = Game.GetMapCell(Unit.Location);
                                Unit.Owner.RemoveUnit(Unit);
                                mapCell.SetActiveUnit(Game.GetUnits(mapCell.Location) is List<IUnit> units && units.Count > 0 ? units.First() : null);
                                Unit.Dispose();
                                Unit = null;
                                Console.WriteLine("SERVER: Расформирован отряд юнитов " + unit.Name);
                            }
                            break;
                        case ADataType.GeneralUnit:
                            break;
                        case ADataType.WorkUnit:
                            unit = request.Package as IUnit;
                            if (Game.GetUnit(unit) is IUnit e && e is object)
                            {
                                if (Game.GetMapCell(e.Location) is AMapCell mapCell && mapCell.IsResource)
                                {
                                    if (mapCell.IsMined) mapCell.UnsetActiveWorker();
                                    else mapCell.SetActiveWorker(e);
                                    Console.WriteLine("SERVER: Использован отряд юнитов " + unit.Name);
                                }
                            }
                            break;
                        case ADataType.EstablishUnit:
                            unit = request.Package as IUnit;
                            if (Game.GetUnit(unit) is IUnit dUnit && dUnit is object)
                            {
                                Game.AddSettlement(dUnit, GameExtension.SettlementName[random.Next(GameExtension.SettlementName.Count)]);
                                Console.WriteLine("SERVER: Отряд юнитов " + unit.Name + " основывает поселение");
                            }
                            break;
                        case ADataType.MapCellSelect:
                            location = (APoint)request.Package;
                            Game.SelectMapCell(location);
                            Console.WriteLine("SERVER: Выделена клетка в координатах " + location);
                            break;
                        case ADataType.MoveUnit:
                            location = (APoint)request.Package;
                            if (Game.SelectedMapCell.ActiveUnit is object && Game.MoveUnit(location))
                            {
                                Game.SelectMapCell(location);
                                Console.WriteLine("SERVER: Юнит перемещен в координаты " + location);
                            }
                            break;
                        default:
                            IsChanged = false;
                            Console.WriteLine("SERVER: Получен пустой или необрабатываемый запрос");
                            break;
                    }
                    if (IsChanged)
                    {
                        Room.UpdateGame(Game);

                        ServerFrame = new AFrame(
                            Room.Id,
                            Room,
                            AMessageType.RoomInfo,
                            SServer.LocalIPAddress(),
                            SServer.GroupIPAdress.ToString());

                        OnClientReceive(ServerFrame);

                        isSend = false;

                        Send(ServerFrame);

                    }
                }
                //SServer?.SendFrame(ServerFrame);

            }
            IsServerReceive = true;
        }

        public void Hide()
        {

            CClient.StopReceive();
            if (IsServer) SClient.StopReceive();

            Visible = false;
            IsClientReceive = false;
            IsServerReceive = false;

        }

        public void Show(ARoom room, RPlayer player, bool isServer)
        {

            IsServer = isServer;
            IsClientReceive = false;
            IsServerReceive = true;

            Room = room;
            Player = player;

            CClient.StartReceive("Receiver");
            if (isServer) SClient.StartReceive("Server Receiver");

            ClientFrame = new AFrame(Room.Id, new AData(null, Player, ADataType.None), AMessageType.Request, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());

            if (isServer)
            {
                Random random = new Random((int)DateTime.Now.Ticks);

                Game = new AGame();

                List<ICharacter> characters = new List<ICharacter>();

                for (int i = 0; i < Room.Players.Count; i++)
                {
                    ASexType sexType = new[] { ASexType.Female, ASexType.Male }[random.Next(2)];
                    ICharacter character = new ACharacter(GameExtension.CharacterName(sexType), GameExtension.DefaultFamily[random.Next(GameExtension.DefaultFamily.Count)], sexType, random.Next(-16, -5), i, i);
                    characters.Add(character);
                }

                Game.Initialize(Room.Players.Select(x => x.Name).ToList(), characters);

                if (Game.StartGame(Room.MapSize)) Room.StartGame(Game);

                //ServerSender?.Abort();
                AFrame frame = new AFrame(Room.Id, Room, AMessageType.RoomInfo, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString());
                ServerSender = new Thread(() => SServer?.SendFrame(frame)) { Name = "Server-Sender", IsBackground = true };
                ServerSender.Start();

                OnClientReceive(frame);

                //SServer?.SendFrame(new AFrame(Room.Id, Room.Game, AMessageType.RoomInfo, CClient.LocalIPAddress(), CClient.GroupIPAdress.ToString()));

            }
            else
            {
                ClientReceiver?.Abort();
                ClientReceiver = new Thread(() => CClient.ReceiveResult()) { Name = "Client-OnShowReceiver", IsBackground = true };
                ClientReceiver.Start();
                ClientReceiver.Join();
                if (CClient.IsComleted) OnClientReceive(CClient.Result);
            }

            Visible = true;
            Update();

        }

        public override void Update()
        {



        }
    }
}
