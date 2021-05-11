using System;
using System.Collections.Generic;
using System.Linq;

using GraphicsLibrary.Interfaces;
using CommonPrimitivesLibrary;

namespace GraphicsLibrary.Graphics
{
    public class AContainer
    {

        private List<IPrimitive> _Controls;

        private int _Width;
        private int _Height;
        public APoint _Location;

        public int Width { get => _Width; set => _Width = value; }
        public int Height { get => _Height; set => _Height = value; }
        public APoint Location { get => _Location; set => _Location = value; }
        public IPrimitive Parent;

        public IReadOnlyList<IPrimitive> Controls { get => _Controls; }

        public AContainer(ASize size, IPrimitive parent, APoint location)
        {
            _Width = size.Width;
            _Height = size.Height;

            Parent = parent;
            _Location = location;

            _Controls = new List<IPrimitive>();
        }

        public void Add(IPrimitive primitive)
        {
            _Controls.Add(primitive);
            if (!Parent.Controls.Contains(primitive)) Parent.Add(primitive);
        }

        public void AddRange(List<IPrimitive> primitives)
        {
            foreach (IPrimitive primitive in primitives) Add(primitive);
        }

        public void Remove(IPrimitive primitive)
        {
            _Controls.Remove(primitive);
            if (Parent.Controls.Contains(primitive)) Parent.Remove(primitive);
        }

        public void Ranking()
        {
            _Controls = _Controls.OrderByDescending(x => x.Enabled).ToList();
            APoint point = _Location + new APoint(0, 0);

            _Controls[0].Location = point;

            for (int i = 1, row = 0; i < _Controls.Count; i++)
            {
                if (point.X + _Controls[i].Width + 10 < Width) point.X += _Controls[i].Width + 10;
                else if (point.Y + _Controls[i].Height < Height)
                {
                    row++;
                    point = new APoint(10, 10 + (_Controls[i].Height + 10) * row);
                }
                else break;
                _Controls[i].Location = point;
            }
        }

    }
}
