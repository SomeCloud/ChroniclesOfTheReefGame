using System;
using System.Collections.Generic;

using GraphicsLibrary.Interfaces;

namespace ArtemisChroniclesOfTheReefGame.Page
{
    public class APage: IPage
    {

        private List<IPrimitive> _Controls;
        private IPrimitive _Parent;
        private bool _Visible;
        public IReadOnlyList<IPrimitive> Controls { get => _Controls; }
        public IPrimitive Parent { get => _Parent; }
        public bool Visible
        {
            get => _Visible;
            set
            {
                _Visible = value;
                SetVisiable(_Visible);
            }
        }

        public APage(IPrimitive parent) => (_Parent, _Controls) = (parent, new List<IPrimitive>());

        public void Add(IPrimitive primitive)
        {
            primitive.Parent = _Parent;
            _Controls.Add(primitive);
        }
        public void Remove(IPrimitive primitive)
        {
            primitive.Parent.Remove(primitive);
            _Controls.Remove(primitive);
        }
        public void Clear()
        {
            foreach (IPrimitive primitive in _Controls) primitive.Parent.Remove(primitive);
            _Controls.Clear();
        }

        private void SetVisiable(bool visible) { foreach(IPrimitive primitive in _Controls) primitive.Enabled = visible; }

    }
}
