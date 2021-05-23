using System;
using System.Collections.Generic;
using System.Text;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;
using GraphicsLibrary.Interfaces;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    public interface IPage
    {

        public IReadOnlyList<IPrimitive> Controls { get; }
        public IPrimitive Parent { get; }
        public bool Visible { get; set; }

        public void Add(IPrimitive primitive);
        public void Remove(IPrimitive primitive);
        public void Clear();

        public void Update();

    }
}
