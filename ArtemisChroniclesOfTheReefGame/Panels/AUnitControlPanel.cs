using System;
using System.Linq;
using System.Collections.Generic;

using NetLibrary;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;
using GraphicsLibrary.Interfaces;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

using GameLibrary;
using GameLibrary.Unit.Main;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class AUnitControlPanel : AScrolleredPanel
    {

        public delegate void OnClick(IUnit unit);

        public event OnClick RenameEvent;
        public event OnClick DestroyEvent;
        public event OnClick GeneralEvent;
        public event OnClick WorkEvent;
        public event OnClick EstablishEvent;

        private AButton _Rename;
        private AButton _Destroy;
        private AButton _General;
        private AButton _Work;
        private AButton _Establish;

        public AButton Rename => _Rename;
        public AButton Destroy => _Destroy;
        public AButton General => _General;
        public AButton Work => _Work;
        public AButton Establish => _Establish;

        private List<Action<AGame, IUnit>> Buttons;

        private IUnit Unit;

        public AUnitControlPanel(ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            Buttons = new List<Action<AGame, IUnit>>();
        }

        public override void Initialize()
        {

            base.Initialize();

            _Rename = new AButton(new ASize((ContentSize.Width - 20) / 2, 50)) { Text = "Переименовать" };
            _Destroy = new AButton(new ASize((ContentSize.Width - 20) / 2, 50)) { Text = "Уничтожить" };
            _General = new AButton(new ASize((ContentSize.Width - 20) / 2, 50)) { Text = "Полководец" };
            _Work = new AButton(new ASize((ContentSize.Width - 20) / 2, 50)) { Text = "Добыча ресурсов" };
            _Establish = new AButton(new ASize((ContentSize.Width - 20) / 2, 50)) { Text = "Основать поселение" };

            _Rename.MouseClickEvent += (state, mstate) =>
            {
                RenameEvent?.Invoke(Unit);
            };

            _Destroy.MouseClickEvent += (state, mstate) =>
            {
                DestroyEvent?.Invoke(Unit);
            };

            _General.MouseClickEvent += (state, mstate) =>
            {
                GeneralEvent?.Invoke(Unit);
            };

            _Work.MouseClickEvent += (state, mstate) =>
            {
                WorkEvent?.Invoke(Unit);
            };

            _Establish.MouseClickEvent += (state, mstate) =>
            {
                EstablishEvent?.Invoke(Unit);
            };

            Add(_Rename);
            Add(_Destroy);
            Add(_General);
            Add(_Work);
            Add(_Establish);

            foreach (IPrimitive primitive in Content)
            {
                primitive.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);
                primitive.Enabled = false;
            }

            Buttons.Add((AGame game, IUnit unit) => _Rename.Enabled = true);
            Buttons.Add((AGame game, IUnit unit) => _Destroy.Enabled = true);
            Buttons.Add((AGame game, IUnit unit) => _General.Enabled = new List<AUnitType>() { AUnitType.Spearman, AUnitType.Swordsman, AUnitType.Archer, AUnitType.Axeman, AUnitType.Warrior }.Contains(unit.UnitType));
            Buttons.Add((AGame game, IUnit unit) => _Work.Enabled = unit.Owner.IsResource(game.GetMapCell(unit.Location).ResourceType) && game.GetMapCell(unit.Location).IsResource && unit.UnitType.Equals(AUnitType.Farmer));
            Buttons.Add((AGame game, IUnit unit) => _Establish.Enabled = game.GetMapCell(unit.Location).NeighboringCells.All(x => x.Owner is null) && unit.UnitType.Equals(AUnitType.Colonist));

        }

        public void Update(AGame game, IUnit unit)
        {

            Unit = unit;

            foreach (Action<AGame, IUnit> action in Buttons) action(game, unit);

            APoint last = new APoint(10, 10);

            foreach (IPrimitive primitive in Content.Where(x => x.Enabled.Equals(true)))
            {
                primitive.Location = last;
                last.Y += primitive.Height + 10;
            }

            ContentSize = new ASize(ContentSize.Width, Content.Where(x => x.Enabled).Count() > 0 ? last.Y : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }


    }

}
