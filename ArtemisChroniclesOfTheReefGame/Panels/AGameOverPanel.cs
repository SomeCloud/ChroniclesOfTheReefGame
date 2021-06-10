using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using CommonPrimitivesLibrary;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Unit.Main;
using GameLibrary.Technology;
using GameLibrary.Settlement;
using GameLibrary.Settlement.Building;
using GameLibrary.Player;
using GameLibrary.Character;
using GameLibrary.Map;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class AGameOverPanel: APanel
    {

        public delegate void OnBack();

        public event OnBack BackEvent;

        private ALabel Header;
        private ALabelList Players;
        private AButton BackToMenu;

        public AGameOverPanel(ASize size): base(size)
        {

        }

        public override void Initialize()
        {

            base.Initialize();

            int dWidth = Width / 2;

            Header = new ALabel(new ASize(dWidth, 100)) { Parent = this, Location = new APoint((Width - dWidth) / 2, 10) };
            Players = new ALabelList(new ASize(Width / 2, Height - 200)) { Parent = this, Location = Header.Location + new APoint(0, Header.Height + 10), Text = "Список игроков" };
            BackToMenu = new AButton(new ASize(dWidth, 60)) { Parent = this, Location = Players.Location + new APoint(0, Players.Height + 10), Text = "Главное меню" };

            BackToMenu.MouseClickEvent += (state, mstate) => BackEvent?.Invoke();

        }

        public void Update(IPlayer winner, List<IPlayer> players)
        {
            Header.Text = winner is object? "Игрок " + winner.Name + " победил!": "Все персонажи мертвы";
            int maxLength = players.Max(x => x.Name.Length);
            Players.Update(players.OrderBy(x => x.Status).Select(x => StringPad(x.Name, maxLength) + " - " + (!x.Equals(winner) ?(x.Status? "Союзник" : !x.Ruler.IsAlive? "Персонаж мертв": "Власть утеряна"): "Победил")));
        }

        public void Show(IPlayer winner, List<IPlayer> players)
        {
            Enabled = true;
            Update(winner, players);
        }

        public void Hide()
        {
            Enabled = false;
        }

        private string StringPad(string value, int width) => value + new String(' ', width - value.Length);
        private string StringPad(int value, int width) => StringPad(value.ToString(), width);

    }
}
