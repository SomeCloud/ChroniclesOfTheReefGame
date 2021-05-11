using GraphicsLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

using APoint = CommonPrimitivesLibrary.APoint;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    public class APageSingleplayerGameSettings: APage, IPage
    {


        private AButton _StartNewGame;
        private AButton _LoadGame;
        private AButton _Back;

        public AButton StartNewGame { get => _StartNewGame; }
        public AButton LoadGame { get => _LoadGame; }
        public AButton Back { get => _Back; }

        public APageSingleplayerGameSettings(IPrimitive primitive) : base(primitive)
        {

            _StartNewGame = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Новая игра", Location = new APoint((Parent.Width - GraphicsExtension.DefaultMenuButtonSize.Width) / 2, (Parent.Height - GraphicsExtension.DefaultMenuButtonSize.Height) / 2) };
            _LoadGame = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Загрузить игру", Location = _StartNewGame.Location + new APoint(0, _StartNewGame.Height + 10) };
            _Back = new AButton(GraphicsExtension.DefaultMenuButtonSize) { Text = "Главное меню", Location = _LoadGame.Location + new APoint(0, _LoadGame.Height + 10) };

            Add(_StartNewGame);
            Add(_LoadGame);
            Add(_Back);

        }

    }
}
