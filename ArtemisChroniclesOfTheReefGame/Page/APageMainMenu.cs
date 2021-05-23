using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    public class APageMainMenu: APage, IPage
    {

        private AButton _SingleplayerGame;
        private AButton _MultiplayerGame;
        private AButton _Settings;
        private AButton _Exit;

        public AButton SingleplayerGame { get => _SingleplayerGame; }
        public AButton MultiplayerGame { get => _MultiplayerGame; }
        public AButton Settings { get => _Settings; }
        public AButton Exit { get => _Exit; }

        public APageMainMenu(IPrimitive primitive): base(primitive)
        {

            _SingleplayerGame = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Одиночная игра", Location = new APoint((Parent.Width - GraphicsExtension.DefaultMenuButtonSize.Width) / 2, (Parent.Height - GraphicsExtension.DefaultMenuButtonSize.Height) / 2) };
            _MultiplayerGame = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Многопользовательская игра", Location = _SingleplayerGame.Location + new APoint(0, _SingleplayerGame.Height + 10) };
            _Settings = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Настройки", Location = _MultiplayerGame.Location + new APoint(0, _MultiplayerGame.Height + 10) };
            _Exit = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Выход", Location = _Settings.Location + new APoint(0, _Settings.Height + 10) };

            Add(_SingleplayerGame);
            Add(_MultiplayerGame);
            Add(_Settings);
            Add(_Exit);

        }

        public override void Update()
        {
            _SingleplayerGame.Location = new APoint((Parent.Width - GraphicsExtension.DefaultMenuButtonSize.Width) / 2, (Parent.Height - GraphicsExtension.DefaultMenuButtonSize.Height) / 2);
            _MultiplayerGame.Location = _SingleplayerGame.Location + new APoint(0, _SingleplayerGame.Height + 10);
            _Settings.Location = _MultiplayerGame.Location + new APoint(0, _MultiplayerGame.Height + 10);
            _Exit.Location = _Settings.Location + new APoint(0, _Settings.Height + 10);
        }
    }
}
