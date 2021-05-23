using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary.Interfaces
{


    public delegate void OnTimeEvent();

    public delegate void OnMouseEvent(IPrimitive primitive, AMouseState mouseState);
    public delegate void OnKeyEvent(IPrimitive primitive, AKeyboardState keyboardState);
    public delegate void OnSizeChangeEvent(IPrimitive primitive, ASize size);

    public interface IPrimitive: IDisposable
    {

        public event OnMouseEvent MouseClickEvent;
        public event OnMouseEvent MouseEnterEvent;
        public event OnMouseEvent MouseIntoEvent;
        public event OnMouseEvent MouseIntoChildEvent;
        public event OnMouseEvent MouseOverEvent;
        public event OnMouseEvent MouseButtonDownEvent;
        public event OnMouseEvent MouseButtonUpEvent;

        public event OnKeyEvent KeyUpEvent;
        public event OnKeyEvent KeyPressEvent;
        public event OnKeyEvent KeyDownEvent;

        public event OnTimeEvent TimeEvent;

        public event OnSizeChangeEvent SizeChangeEvent;

        public ASize Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public APoint Location { get; set; }
        public APoint GlobalLocation { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public ARectangle VisibleArea { get; }

        public int ZIndex { get; set; }

        public bool Enabled { get; set; }
        public bool Active { get; set; }
        public bool ActiveCollider { get; set; }
        public bool ForcedActive { get; set; }
        public bool DragAndDrop { get; set; }
        public bool IsDarkened { get; set; }
        public bool IsInteraction { get; set; }
        public bool IsCounting { get; set; }

        public IPrimitive Parent { get; set; }
        public List<IPrimitive> Controls { get; }

        public ICollider Collider { get; set; }
        public ITexture Texture { get; set; }
        public GraphicsDevice GraphicsDevice { get; }
        public ATextLabel TextLabel { get; }
        public string Text { get; set; }

        public abstract void Initialize();
        public abstract bool PreLocationChangeProcess(APoint point);
        public abstract void OnLocationChangeProcess();

        public void Add(IPrimitive primitive);
        public void Remove(IPrimitive primitive);
        public void Clear();
        public void ControlsUpdateByZIndex();

        public void Draw(SpriteBatch spriteBatch);

        public void ProcessKey(AKeyboardState keyboardState);
        public bool InCollider(AMouseState mouseState, out IPrimitive activeChild);

        public void InvokeMouseOverEvent(AMouseState mouseState);

        public void OnForcedMouseButtonDown(AMouseState mouseState);
        public void OnForcedMouseButtonUp(AMouseState mouseState);
        public void OnForcedMouseOver(AMouseState mouseState);
        public void OnForcedMouseInto(AMouseState mouseState);

        public void UpdateVisibleArea(ARectangle rectangle);

        public new void Dispose();

    }
}
