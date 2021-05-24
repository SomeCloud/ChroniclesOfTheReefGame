using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;
using GraphicsLibrary.Interfaces;

using GameLibrary.Character;

using CommonPrimitivesLibrary;
using ArtemisChroniclesOfTheReefGame.Page;

namespace ArtemisChroniclesOfTheReefGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private bool IsKeyButtonDown;
        private bool IsMouseLeftButtonDown;
        private bool IsMouseRightButtonDown;
        private bool IsMouseMiddleButtonDown;

        private Keys LastKey;
        private AMouseState _mouseState;
        private AKeyboardState _keyboardState;

        TexturePack TexturePack;

        AGraphics Graphics;

        APageMainMenu PageMenu;
        APageSingleplayerGameSettings PageSingleplayerGameSettings;
        APageMultiplayerGame PageMultiplayerGame;
        APageConnection PageConnection;
        APageWaitConnection PageWaitConnection;
        APageConnectionLobby PageConnectionLobby;
        APageCreateLobby PageCreateLobby;
        //APageGame PageGame;

        private Dictionary<Keys, AKeyboardKey> keys = new Dictionary<Keys, AKeyboardKey>()
                {
                    { Keys.A, AKeyboardKey.A }, { Keys.B, AKeyboardKey.B }, { Keys.C, AKeyboardKey.C }, { Keys.D, AKeyboardKey.D },
                    { Keys.E, AKeyboardKey.E }, { Keys.F, AKeyboardKey.F }, { Keys.G, AKeyboardKey.G }, { Keys.H, AKeyboardKey.H },
                    { Keys.I, AKeyboardKey.I }, { Keys.J, AKeyboardKey.J }, { Keys.K, AKeyboardKey.K }, { Keys.L, AKeyboardKey.L },
                    { Keys.M, AKeyboardKey.M }, { Keys.N, AKeyboardKey.N }, { Keys.O, AKeyboardKey.O }, { Keys.P, AKeyboardKey.P },
                    { Keys.Q, AKeyboardKey.Q }, { Keys.R, AKeyboardKey.R }, { Keys.S, AKeyboardKey.S }, { Keys.T, AKeyboardKey.T },
                    { Keys.U, AKeyboardKey.U }, { Keys.V, AKeyboardKey.V }, { Keys.W, AKeyboardKey.W }, { Keys.X, AKeyboardKey.X },
                    { Keys.Y, AKeyboardKey.Y }, { Keys.Z, AKeyboardKey.Z },

                    { Keys.D0, AKeyboardKey.D0 }, { Keys.D1, AKeyboardKey.D1 }, { Keys.D2, AKeyboardKey.D2 }, { Keys.D3, AKeyboardKey.D3 },
                    { Keys.D4, AKeyboardKey.D4 }, { Keys.D5, AKeyboardKey.D5 }, { Keys.D6, AKeyboardKey.D6 }, { Keys.D7, AKeyboardKey.D7 },
                    { Keys.D8, AKeyboardKey.D8 }, { Keys.D9, AKeyboardKey.D9 },

                    { Keys.OemPlus, AKeyboardKey.Equally }, { Keys.OemMinus, AKeyboardKey.Dash }, { Keys.OemTilde, AKeyboardKey.Tilde },
                    { Keys.OemQuestion, AKeyboardKey.Slash }, { Keys.OemPeriod, AKeyboardKey.Dot }, { Keys.OemComma, AKeyboardKey.Comma },
                    { Keys.OemOpenBrackets, AKeyboardKey.OemOpenBrackets }, { Keys.OemCloseBrackets, AKeyboardKey.OemCloseBrackets },
                    { Keys.OemSemicolon, AKeyboardKey.OemSemicolon }, { Keys.OemQuotes, AKeyboardKey.OemQuotes }
                };

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            if (false)
            {
                _graphics.IsFullScreen = true;
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                _graphics.IsFullScreen = false;
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 50;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 150;
            }
            _graphics.ApplyChanges();

            TexturePack = new TexturePack(Content, GraphicsDevice);

            _mouseState = new AMouseState(new APoint(), AMouseButton.Left, AMouseButtonState.None);
            _keyboardState = new AKeyboardState(AKeyState.Undefined);

            Graphics = new AGraphics(new ASize(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), GraphicsDevice);

            PageMenu = new APageMainMenu(Graphics) { Visible = true };
            PageSingleplayerGameSettings = new APageSingleplayerGameSettings(Graphics) { Visible = false };
            PageMultiplayerGame = new APageMultiplayerGame(Graphics) { Visible = false };
            PageConnection = new APageConnection(Graphics) { Visible = false };
            PageWaitConnection = new APageWaitConnection(Graphics) { Visible = false };
            PageConnectionLobby = new APageConnectionLobby(Graphics) { Visible = false };
            PageCreateLobby = new APageCreateLobby(Graphics) { Visible = false };
            //PageGame = new APageGame(Graphics) { Visible = false };

            Graphics.SizeChangeEvent += (state, value) => {
                PageMenu.Update();
                PageSingleplayerGameSettings.Update();
                //PageGame.Update();
            };

            PageMenu.SingleplayerGame.MouseClickEvent += (state, mstate) => {
                PageMenu.Visible = false;
                PageSingleplayerGameSettings.Visible = true; 
            };
            
            PageMenu.MultiplayerGame.MouseClickEvent += (state, mstate) => {
                PageMenu.Visible = false;
                PageMultiplayerGame.Show(); 
            };

            PageMultiplayerGame.ConnectToGame.MouseClickEvent += (state, mstate) => {
                PageMultiplayerGame.Hide();
                PageConnection.Show();
            };

            PageConnection.BackEvent += () => {
                PageConnection.Hide();
                PageMultiplayerGame.Show();
            };

            PageConnection.ConnectEvent += (ip, port, name) => {
                PageConnection.Hide();
                PageWaitConnection.Show(ip, port, name);
            };

            PageWaitConnection.BackEvent += () => {
                PageWaitConnection.Hide();
                PageMultiplayerGame.Show();
            };

            PageSingleplayerGameSettings.StartNewGame.MouseClickEvent += (state, mstate) => {
                PageSingleplayerGameSettings.Visible = false;
                //PageGame.Visible = true;
                /*PageGame.StartGame(
                    new List<string>() { "Player Tom", "Player Alex", "Player Mark", "Player Malin" }, 
                    new List<ICharacter>() { 
                        new ACharacter("Том", "Питерсон", ASexType.Male, -16, 1, 1),
                        //new ACharacter("Эмбрел", "Филистин", ASexType.Female, -16, 1, 1),
                        new ACharacter("Александра", "Эриксон", ASexType.Female, -16, 2, 2),
                        new ACharacter("Марк", "Эллианел", ASexType.Male, -16, 3, 3),
                        new ACharacter("Лин", "Берггрен", ASexType.Female, -16, 4, 4) },
                    new ASize(8, 17));*/
            };
            
            PageSingleplayerGameSettings.Back.MouseClickEvent += (state, mstate) => {
                PageSingleplayerGameSettings.Visible = false;
                PageMenu.Visible = true;
            };

            PageMultiplayerGame.Back.MouseClickEvent += (state, mstate) => {
                PageMultiplayerGame.Hide();
                PageMenu.Visible = true;
            };
            
            PageMultiplayerGame.CreateRoomEvent += (id, name) => {
                PageMultiplayerGame.Hide();
                PageCreateLobby.Show(id, name);
            };

            PageMultiplayerGame.ConnectEvent += (ip, port, name) => {
                PageMultiplayerGame.Hide();
                PageWaitConnection.Show(ip, port, name);
            };

            PageWaitConnection.ConnectionEvent += (room) => {
                PageWaitConnection.Hide();
                PageConnectionLobby.Show(room, PageWaitConnection.Player);
            };

            PageConnectionLobby.DisconnectionEvent += () =>
            {
                PageConnectionLobby.Hide();
                PageMultiplayerGame.Show();
            };

            PageCreateLobby.BackEvent += () =>
            {
                PageCreateLobby.Hide();
                PageMultiplayerGame.Show();
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            var mstate = Mouse.GetState();
            var kstate = Keyboard.GetState();

            _keyboardState.KeyboardLanguage = System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.TwoLetterISOLanguageName == "en" ? AKeyboardLanguage.Eng : AKeyboardLanguage.Rus;

            if (!IsKeyButtonDown && Keyboard.GetState().GetPressedKeyCount() > 0)
            {
                IsKeyButtonDown = true;
                LastKey = kstate.GetPressedKeys().ToList().Where(x => x != Keys.RightShift && x != Keys.LeftShift).ToList().FirstOrDefault();
                switch (LastKey)
                {
                    case Keys.Back:
                        _keyboardState.KeyboardKey = AKeyboardKey.None;
                        _keyboardState.KeyState = AKeyState.Backspace;
                        break;
                    case Keys.F1:
                        if (_graphics.IsFullScreen)
                        {
                            _graphics.IsFullScreen = false;
                            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 50;
                            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 150;
                        }
                        else
                        {
                            _graphics.IsFullScreen = true;
                            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                        }
                        _graphics.ApplyChanges();
                        Graphics.Size = new ASize(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                        break;
                    case Keys.Space:
                        _keyboardState.KeyboardKey = AKeyboardKey.None;
                        _keyboardState.KeyState = AKeyState.Space;
                        break;
                    case Keys.Escape:
                        _keyboardState.KeyboardKey = AKeyboardKey.None;
                        _keyboardState.KeyState = AKeyState.Exit;
                        break;
                    case Keys.Enter:
                        _keyboardState.KeyboardKey = AKeyboardKey.None;
                        _keyboardState.KeyState = AKeyState.Enter;
                        break;
                    case Keys.CapsLock:
                        _keyboardState.KeyboardKey = AKeyboardKey.None;
                        _keyboardState.CapsLook = !_keyboardState.CapsLook;
                        break;
                    case Keys.LeftShift:
                        _keyboardState.KeyboardKey = AKeyboardKey.None;
                        _keyboardState.Shift = !_keyboardState.Shift;
                        break;
                    default:
                        if (keys.ContainsKey(LastKey)) {
                            _keyboardState.KeyboardKey = keys[LastKey];
                            _keyboardState.KeyState = AKeyState.Key;
                        }
                        break;
                }
            }

            if (IsKeyButtonDown && Keyboard.GetState().IsKeyUp(LastKey))
            {
                Graphics.ProcessKey(_keyboardState);
                _keyboardState.KeyState = AKeyState.Undefined;
                IsKeyButtonDown = false;
            }

            _mouseState.MouseButtonState = AMouseButtonState.Pressed;
            _mouseState.MouseButton = AMouseButton.None;

            _mouseState.CursorPosition = new APoint(mstate.X, mstate.Y);

            Graphics.ProcessClick(_mouseState);
            if (!IsKeyButtonDown) Graphics.ProcessKey(_keyboardState);

            switch (mstate.LeftButton)
            {
                case ButtonState.Pressed:
                    _mouseState.MouseButton = AMouseButton.Left;
                    if (!IsMouseLeftButtonDown)
                    {
                        IsMouseLeftButtonDown = true;
                        _mouseState.MouseButtonState = AMouseButtonState.Down;
                    }
                    else
                    {
                        _mouseState.MouseButtonState = AMouseButtonState.Pressed;
                    }
                    Graphics.ProcessClick(_mouseState);
                    break;
                case ButtonState.Released:
                    if (IsMouseLeftButtonDown)
                    {
                        _mouseState.MouseButton = AMouseButton.Left;
                        IsMouseLeftButtonDown = false;
                        _mouseState.MouseButtonState = AMouseButtonState.Up;
                        Graphics.ProcessClick(_mouseState);
                    }
                    break;
                default: break;
            }

            switch (mstate.RightButton)
            {
                case ButtonState.Pressed:
                    _mouseState.MouseButton = AMouseButton.Right;
                    if (!IsMouseRightButtonDown)
                    {
                        IsMouseRightButtonDown = true;
                        _mouseState.MouseButtonState = AMouseButtonState.Down;
                    }
                    else
                    {
                        _mouseState.MouseButtonState = AMouseButtonState.Pressed;
                    }
                    Graphics.ProcessClick(_mouseState);
                    break;
                case ButtonState.Released:
                    if (IsMouseRightButtonDown)
                    {
                        _mouseState.MouseButton = AMouseButton.Right;
                        IsMouseRightButtonDown = false;
                        _mouseState.MouseButtonState = AMouseButtonState.Up;
                        Graphics.ProcessClick(_mouseState);
                    }
                    break;
                default: break;
            }

            switch (mstate.MiddleButton)
            {
                case ButtonState.Pressed:
                    _mouseState.MouseButton = AMouseButton.Middle;
                    if (!IsMouseMiddleButtonDown)
                    {
                        IsMouseMiddleButtonDown = true;
                        _mouseState.MouseButtonState = AMouseButtonState.Down;
                    }
                    else
                    {
                        _mouseState.MouseButtonState = AMouseButtonState.Pressed;
                    }
                    Graphics.ProcessClick(_mouseState);
                    break;
                case ButtonState.Released:
                    if (IsMouseMiddleButtonDown)
                    {
                        _mouseState.MouseButton = AMouseButton.Middle;
                        IsMouseMiddleButtonDown = false;
                        _mouseState.MouseButtonState = AMouseButtonState.Up;
                        Graphics.ProcessClick(_mouseState);
                    }
                    break;
                default: break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(GraphicsExtension.DefaultFillColor);

            _spriteBatch.Begin();

            Graphics.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
