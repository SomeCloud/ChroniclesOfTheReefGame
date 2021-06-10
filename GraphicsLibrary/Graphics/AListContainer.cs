using System;
using System.Collections.Generic;
using System.Linq;

using GraphicsLibrary.Interfaces;
using CommonPrimitivesLibrary;

namespace GraphicsLibrary.Graphics
{
    public class AListContainer
    {

        private List<IPrimitive> _Controls;
        public APoint Location;
        public IPrimitive Parent;

        public int _Distance;
        public int Distance { 
            get => _Distance;
            set {
                _Distance = value;
                Update();
            }
        }

        public AListContainer(APoint location, IPrimitive parent, int distance = 10)
        {
            Location = location;
            Parent = parent;
            _Distance = 10;
            _Controls = new List<IPrimitive>();
        }

        public void Add(IPrimitive primitive)
        {
            _Controls.Add(primitive);
            if (!Parent.Controls.Contains(primitive))
            {
                Parent.Add(primitive);
                Update();
            }
        }

        public void Remove(IPrimitive primitive)
        {
            _Controls.Remove(primitive);
            if (Parent.Controls.Contains(primitive)) Parent.Remove(primitive);
        }

        public void Clear()
        {
            foreach (IPrimitive primitive in _Controls) _Controls.Remove(primitive);
            _Controls.Clear();
        }

        public void Update()
        {

            APoint last = Location + 0;

            foreach (IPrimitive primitive in _Controls)
            {
                primitive.Location = last;
                last += new APoint(0, primitive.Height + 10);
            }

        }

    }
}
