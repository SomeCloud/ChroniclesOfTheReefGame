using System;
using System.Linq;
using System.Collections.Generic;

using Font = System.Drawing.Font;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.Graphics;

namespace GraphicsLibrary.StandartGraphicsPrimitives
{
    public abstract class APrimitive : IPrimitive, IDisposable
    {
        
        /*public delegate void OnMouseEvent(IPrimitive primitive, AMouseState mouseState);
        public delegate void OnKeyEvent(IPrimitive primitive, AKeyboardState keyboardState);
        public delegate void OnSizeChangeEvent(IPrimitive primitive, ASize size);*/

        public event OnMouseEvent MouseClickEvent;
        public event OnMouseEvent MouseEnterEvent;
        public event OnMouseEvent MouseIntoEvent;
        public event OnMouseEvent MouseIntoChildEvent;
        public event OnMouseEvent MouseOverEvent;
        public event OnMouseEvent MouseButtonDownEvent;
        public event OnMouseEvent MouseButtonUpEvent;

        private OnMouseEvent MouseButtonDown;
        private OnMouseEvent MouseButtonUp;
        private OnMouseEvent MouseOver;
        private OnMouseEvent MouseInto;

        public event OnKeyEvent KeyUpEvent;
        public event OnKeyEvent KeyPressEvent;
        public event OnKeyEvent KeyDownEvent;

        public event OnTimeEvent TimeEvent;

        public event OnSizeChangeEvent SizeChangeEvent;

        private ATextLabel.OnChangeEvent TextLabelChange;

        private ASize _Size;
        private APoint _Location;

        private AKeyboardState LastKey;
        private int _Counter;
        protected int Counter => _Counter;

        private bool _Enabled;
        private bool _Active;
        private bool _ActiveCollider;
        private bool _ForcedActive;
        private bool _DragAndDrop;
        private bool _IsDarkened;
        protected bool _IsDarkenedTexture;

        private ARectangle _VisibleArea;

        private IPrimitive _Parent;
        private List<IPrimitive> _Controls;
        private ICollider _Collider;
        private ITexture _Texture;
        private Texture2D _TextTexture;
        private ATextLabel _TextLabel;
        private string _Text;
        private APoint _TextOffset;

        protected APoint LastPoint;
        protected GraphicsDevice _GraphicsDevice;

        public ASize Size
        {
            get => _Size;
            set
            {
                _Size = value;
                if (!(Parent is null)) Initialize();
                UpdateVisibleArea();
                SizeChangeEvent?.Invoke(this, value);
            }
        }
        public int Width
        {
            get => _Size.Width;
            set
            {
                _Size.Width = value;
                if (!(Parent is null)) Initialize();
                UpdateVisibleArea();
                SizeChangeEvent?.Invoke(this, _Size);
            }
        }
        public int Height
        {
            get => _Size.Height;
            set
            {
                _Size.Height = value;
                if (!(Parent is null)) Initialize();
                UpdateVisibleArea();
                SizeChangeEvent?.Invoke(this, _Size);
            }
        }

        public APoint Location
        {
            get => _Location;
            set
            {
                if (PreLocationChangeProcess(value))
                {
                    _Location = value;
                    OnLocationChangeProcess();
                    UpdateVisibleArea();
                    //if (_Collider is object) _Collider.VisibleArea = _VisibleArea;
                }
            }
        }
        public APoint GlobalLocation { get => Location + (Parent is object ? Parent.GlobalLocation : new APoint()); }
        public int X
        {
            get => _Location.X;
            set
            {
                if (PreLocationChangeProcess(new APoint(value, Y)))
                {
                    _Location.X = value;
                    OnLocationChangeProcess();
                    UpdateVisibleArea();
                    if (_Collider is object) _Collider.VisibleArea = _VisibleArea;
                }
            }
        }
        public int Y
        {
            get => _Location.Y;
            set
            {
                if (PreLocationChangeProcess(new APoint(X, value)))
                {
                    _Location.Y = value;
                    OnLocationChangeProcess();
                    UpdateVisibleArea();
                    if (_Collider is object) _Collider.VisibleArea = _VisibleArea;
                }
            }
        }

        public ARectangle VisibleArea { get => _VisibleArea; }

        public int ZIndex { get; set; }

        public int DTimer { get; set; }

        public bool Enabled { get => _Enabled; set => _Enabled = value; }
        public bool Active { get => _Active; set => _Active = value; }
        public bool ActiveCollider { get => _ActiveCollider; set => _ActiveCollider = value; }
        public bool ForcedActive { get => _ForcedActive; set => _ForcedActive = value; }
        public bool DragAndDrop {
            get => _DragAndDrop;
            set
            {
                _DragAndDrop = value;
                if (value) InitializeDragAndDrop();
                else DeInitializeDragAndDrop();
            }
        }
        public bool IsDarkened {
            get => _IsDarkened;
            set
            {
                _IsDarkened = value;
            }
        }
        public bool IsDarkenedTexture { get => _IsDarkenedTexture; }
        public bool IsInteraction { get; set; }
        public bool IsCounting { get; set; }

        public IPrimitive Parent {
            get => _Parent; 
            set
            {
                if (_Parent is object)
                {
                    _Parent.Controls.Remove(this);
                }
                _Parent = value;
                if (value is object)
                {
                    ZIndex = _Parent.ZIndex + 1 + _Parent.Controls.Count;
                    _GraphicsDevice = value.GraphicsDevice;
                    if (_TextTexture is null) _TextTexture = new Texture2D(GraphicsDevice, 1, 1);
                    value.Controls.Add(this);
                    _Parent.ControlsUpdateByZIndex();
                }
                Initialize();
                Text = _Text;
                UpdateVisibleArea();
            }
        }
        public List<IPrimitive> Controls { get => _Controls; }

        public ICollider Collider
        {
            get => _Collider;
            set
            {
                _Collider = value;
                _Collider.VisibleArea = _VisibleArea;
            }
        }
        public ITexture Texture { get => _Texture; set => _Texture = value; }
        public GraphicsDevice GraphicsDevice { get => _GraphicsDevice; }
        public Texture2D TextTexture { get => _TextTexture; }
        public ATextLabel TextLabel { get => _TextLabel; }
        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                if (value.Length > 0 && !(GraphicsDevice is null))
                {
                    UpdateTextOffset();
                }
            }
        }

        public APrimitive(): this(GraphicsExtension.DefaultStandartPrimititveSize) { }

        public APrimitive(ASize size)
        {

            Size = size;
            Location = new APoint();

            _VisibleArea = new ARectangle(new APoint(), Size.ToAPoint());

            ZIndex = -1;
            Enabled = true;
            IsInteraction = true;
            _Controls = new List<IPrimitive>();
            _TextLabel = new ATextLabel();
            Text = "";

            DTimer = 10;

            TextLabelChange = new ATextLabel.OnChangeEvent(state => { if (Text.Length > 0) UpdateTextOffset(); });

            TextLabel.ChangeEvent += TextLabelChange;

        }

        public abstract void Initialize();
        public abstract bool PreLocationChangeProcess(APoint point);
        public abstract void OnLocationChangeProcess();

        public void Add(IPrimitive primitive)
        {
            primitive.Parent = this;
        }
        public void Remove(IPrimitive primitive)
        {
            primitive.ZIndex = 0;
            _Controls.Remove(primitive);
            _Controls = _Controls.OrderBy(x => x.ZIndex).ToList();
            for (int i = 0; i < _Controls.Count; i++)
            {
                _Controls[i].ZIndex = ZIndex + 1 + i;
            }
            ControlsUpdateByZIndex();
        }
        public void Clear()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].ZIndex = 0;
                Controls[i].Parent = null;
            }
            _Controls.Clear();
        }

        public void ControlsUpdateByZIndex()
        {
            _Controls = Controls.OrderByDescending(x => x.ZIndex).ToList();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //new Vector2(GlobalLocation.X + (Location.X < 0 ? VisibleArea.Location.X : 0), GlobalLocation.Y + (Location.Y < 0 ? VisibleArea.Location.Y : 0))
            if (Enabled)
            {
                if (IsCounting) 
                    if (_Counter - DateTime.Now.Second <= DTimer && _Counter - DateTime.Now.Second >= 0)
                    {
                        if (_Counter.Equals(DateTime.Now.Second))
                        {
                            TimeEvent?.Invoke();
                            _Counter = DateTime.Now.Second + DTimer;
                        }
                    }
                    else _Counter = DateTime.Now.Second + DTimer;

                spriteBatch.Draw(_IsDarkenedTexture ? Texture.DarkenedTexture : Texture.Texture, new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, /*scaling*/1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(_TextTexture, new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, /*scaling*/1f, SpriteEffects.None, 0f);

                foreach (IPrimitive primitive in Controls.OrderBy(x => x.ZIndex).ToList())
                {
                    primitive.Draw(spriteBatch);
                }
            }
        }

        public void ProcessKey(AKeyboardState keyboardState)
        {
            if (LastKey is object)
                if (keyboardState.KeyState.Equals(AKeyState.Undefined)) KeyUpEvent?.Invoke(this, keyboardState);
                else if (LastKey.KeyState.Equals(AKeyState.Key) && LastKey.KeyboardKey.Equals(keyboardState.KeyboardKey)) KeyPressEvent?.Invoke(this, keyboardState);
                else if (!LastKey.Equals(keyboardState))
                {
                    KeyUpEvent?.Invoke(this, keyboardState);
                    KeyDownEvent?.Invoke(this, keyboardState);
                }
            LastKey = keyboardState.Clone();
        }

        public bool InCollider(AMouseState mouseState, out IPrimitive activeChild)
        {
            activeChild = null;
            List<APoint> points = new List<APoint>(Collider.Points);
            for (int i = 0; i < points.Count; i++)
            {
                points[i] += GlobalLocation + Collider.Location;
            }

            if (Enabled && Collider.InCollider(mouseState.CursorPosition, points) || ForcedActive)
            {
                if (!ForcedActive) foreach (IPrimitive e in Controls.Where(e => e.Enabled).OrderByDescending(x => x.ZIndex).ToList())
                    {
                        if (e.Enabled && e.IsInteraction)
                        {
                            if (e.InCollider(mouseState, out activeChild))
                            {
                                InvokeMouseOverEvent(mouseState);
                                MouseIntoChildEvent?.Invoke(activeChild, mouseState);
                                return true;
                            }
                        }
                    }

                activeChild = this;

                return InvokeEvent(mouseState);
            }
            else
            {
                InvokeMouseOverEvent(mouseState);
            }

            return false;
        }

        public void InvokeMouseOverEvent(AMouseState mouseState)
        {     
            if ((ForcedActive && DragAndDrop) || (Active && !DragAndDrop) || ActiveCollider)
            {            
                ActiveCollider = false;
                Active = false;
                MouseOverEvent?.Invoke(this, mouseState);
                if (IsDarkened) _IsDarkenedTexture = false; // поздравляю, ты нашел костыль :)
                Active = false;
            }
        }

        protected bool InvokeEvent(AMouseState mouseState)
        {
            switch (mouseState.MouseButtonState)
            {
                case AMouseButtonState.Down:
                    ActiveCollider = true;
                    MouseEnterEvent?.Invoke(this, mouseState);
                    MouseButtonDownEvent?.Invoke(this, mouseState);
                    if (IsDarkened) _IsDarkenedTexture = true;
                    return true;
                case AMouseButtonState.Up:
                    if (ActiveCollider)
                    {
                        MouseClickEvent?.Invoke(this, mouseState);
                        if (IsDarkened) _IsDarkenedTexture = false;
                    }
                    ActiveCollider = false;
                    MouseButtonUpEvent?.Invoke(this, mouseState);
                    return true;
                case AMouseButtonState.Pressed:
                    if (ActiveCollider.Equals(false))
                    {
                        MouseEnterEvent?.Invoke(this, mouseState);
                    }
                    Active = true;
                    if (ActiveCollider)
                        MouseIntoEvent?.Invoke(this, mouseState);
                    return true;
            }
            return false;
        }

        private void InitializeDragAndDrop()
        {

            MouseButtonDown = new OnMouseEvent((state, mouseState) =>
            {
                OnForcedMouseButtonDown(mouseState);
            });

            MouseButtonUp = new OnMouseEvent((state, mouseState) =>
            {
                OnForcedMouseButtonUp(mouseState);
            });

            MouseOver = new OnMouseEvent((state, mouseState) =>
            {
                OnForcedMouseOver(mouseState);
            });

            MouseInto = new OnMouseEvent((state, mouseState) =>
            {
                OnForcedMouseInto(mouseState);
            });

            MouseButtonDownEvent += MouseButtonDown;

            MouseButtonUpEvent += MouseButtonUp;

            MouseOverEvent += MouseOver;

            MouseIntoEvent += MouseInto;

        }

        public void OnForcedMouseButtonDown(AMouseState mouseState)
        {
            if (DragAndDrop && !ForcedActive)
            {
                LastPoint = mouseState.CursorPosition;
                ForcedActive = true;
            }
            if (IsDarkened) _IsDarkenedTexture = true;
        }

        public void OnForcedMouseButtonUp(AMouseState mouseState = null)
        {
            if (DragAndDrop && ForcedActive)
            {
                LastPoint = new APoint();
                ForcedActive = false;
            }
            if (IsDarkened) _IsDarkenedTexture = false;
        }

        public void OnForcedMouseOver(AMouseState mouseState)
        {
            if (DragAndDrop && mouseState.MouseButtonState != AMouseButtonState.Up && ForcedActive)
            {
                ActiveCollider = true;
                //APoint dLocation = mouseState.CursorPosition - LastPoint;
                Location += mouseState.CursorPosition - LastPoint;
                LastPoint = mouseState.CursorPosition;
            }
            if (IsDarkened) _IsDarkenedTexture = true;
        }

        public void OnForcedMouseInto(AMouseState mouseState)
        {
            if (DragAndDrop && ForcedActive)
            {
                //APoint dLocation = mouseState.CursorPosition - LastPoint;
                Location += mouseState.CursorPosition - LastPoint;
                LastPoint = mouseState.CursorPosition;
            }
        }

        private void DeInitializeDragAndDrop()
        {

            MouseButtonDownEvent -= MouseButtonDown;

            MouseButtonUpEvent -= MouseButtonUp;

            MouseOverEvent -= MouseOver;

            MouseIntoEvent -= MouseInto;

            MouseButtonDown = null;
            MouseButtonUp = null;
            MouseOver = null;
            MouseInto = null;

        }

        protected void UpdateVisibleArea()
        {
            if (!(Parent is null))
            {

                ARectangle ParentR = _Parent.VisibleArea;//new ARectangle(Parent.GlobalLocation, Parent.GlobalLocation + Parent.Size.ToAPoint());
                ParentR.Location += _Parent.GlobalLocation;
                ParentR.EndPoint += _Parent.GlobalLocation;
                ARectangle ChildR = new ARectangle(GlobalLocation, GlobalLocation + Size.ToAPoint());
                ARectangle IntersectR = ParentR.Intersect(ChildR);

                _VisibleArea.Location = IntersectR.Location - GlobalLocation;
                _VisibleArea.EndPoint = IntersectR.EndPoint - GlobalLocation;

                //_Collider.VisibleArea = _VisibleArea;

                foreach (IPrimitive child in Controls)
                {
                    child.UpdateVisibleArea(IntersectR);
                }

            }
        }

        public void UpdateVisibleArea(ARectangle rectangle)
        {
            if (!(Parent is null))
            {

                ARectangle ChildR = new ARectangle(GlobalLocation, GlobalLocation + Size.ToAPoint());
                ARectangle IntersectR = rectangle.Intersect(ChildR);

                _VisibleArea.Location = IntersectR.Location - GlobalLocation;
                _VisibleArea.EndPoint = IntersectR.EndPoint - GlobalLocation;

                foreach (IPrimitive child in Controls)
                {
                    child.UpdateVisibleArea(IntersectR);
                }

            }
        }

        private ASize MeasureString(string text, Font font)
        {
            using (var gfx = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1, 1)))
            {
                var s = gfx.MeasureString(text, font, new System.Drawing.SizeF(Width, Height), System.Drawing.StringFormat.GenericTypographic);
                return new ASize((int)s.Width, (int)s.Height);
            }
        }

        public Texture2D DrawString(string text, Color color, Font font)
        {
            Texture2D texture;
            ASize measure = MeasureString(text, font) + 1;
            using (var bmp = new System.Drawing.Bitmap(measure.Width, measure.Height))
            {
                using (var gfx = System.Drawing.Graphics.FromImage(bmp))
                {
                    var textformat = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                    textformat.FormatFlags = System.Drawing.StringFormatFlags.MeasureTrailingSpaces;
                    textformat.Trimming = System.Drawing.StringTrimming.None;
                    textformat.FormatFlags |= System.Drawing.StringFormatFlags.NoClip;

                    gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    gfx.DrawString(text, font, new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B)), 0, 0, textformat);
                }
                var lck = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb); //Lock the bitmap in memory and give us the ability to extract data from it so we can load it into a Texture2D
                var data = new byte[Math.Abs(lck.Stride) * lck.Height]; //destination array for bitmap data, source for texture data
                System.Runtime.InteropServices.Marshal.Copy(lck.Scan0, data, 0, data.Length); //cool, data's in the destination array
                bmp.UnlockBits(lck);
                texture = new Texture2D(GraphicsDevice, bmp.Width, bmp.Height);
                for (int i = 0; i < data.Length; i += 4)
                {
                    byte r = data[i];
                    byte b = data[i + 2];
                    data[i] = b;
                    data[i + 2] = r;
                }
                texture.SetData(data);
            }
            return texture;
        }

        private void UpdateTextOffset()
        {

            Texture2D source = DrawString(_Text, _TextLabel.TextColor, _TextLabel.Font);

            switch (TextLabel.VerticalAlign)
            {
                case ATextVerticalAlign.Top:
                    _TextOffset.Y = 10;
                    break;
                case ATextVerticalAlign.Center:
                    _TextOffset.Y = (Height - source.Height) / 2;
                    break;
                case ATextVerticalAlign.Bottom:
                    _TextOffset.Y = Height - 10 - source.Height;
                    break;
            }

            switch (TextLabel.HorizontalAlign)
            {
                case ATextHorizontalAlign.Left:
                    _TextOffset.X = 10;
                    break;
                case ATextHorizontalAlign.Center:
                    _TextOffset.X = (Width - source.Width) / 2;
                    break;
                case ATextHorizontalAlign.Right:
                    _TextOffset.X = Width - 10 - source.Width;
                    break;
            }

            _TextTexture = new Texture2D(GraphicsDevice, Width, Height);

            Color[] colors = new Color[Width * Height];

            Color[] text = new Color[source.Width * source.Height];

            source.GetData(text);

            //Size = new ASize(Math.Max(source.Width, Width), Math.Max(source.Height, Height));

            for (int i = 0; i < Math.Min(source.Height, Height); i++)
                for (int j = 0; j < Math.Min(source.Width, Width); j++)
                {
                    if ((_TextOffset.Y + i) * Width + _TextOffset.X + j < colors.Length && i * source.Width + j < text.Length) 
                        if (_TextOffset.Y + i > 0 && _TextOffset.Y + i < Height && _TextOffset.X + j > 0 && _TextOffset.X + j < Width)
                            colors[(_TextOffset.Y + i) * Width + _TextOffset.X + j] = text[i * source.Width + j];
                    else break;
                }

            source.Dispose();
            _TextTexture.SetData(colors);

        }

        protected void SetLocation(APoint point)
        {
            _Location.X = point.X;
            _Location.Y = point.Y;
        }

        private static string AllignedString(string s, ATextHorizontalAlign align)
        {

            string[] strings = s.Split('\n');
            int max = s.Split('\n').Max(x => x.Length);
            string res = "";

            foreach (string e in strings)
            {
                switch (align)
                {
                    case ATextHorizontalAlign.Left:
                        res += e + "\n";
                        break;
                    case ATextHorizontalAlign.Center:
                        res += e.PadLeft(((max - e.Length) / 2) + e.Length, '_') + "\n";
                        break;
                    case ATextHorizontalAlign.Right:
                        res += e.PadLeft((max - e.Length) + e.Length) + "\n";
                        break;
                }
            }

            return res;
        }

        public void Dispose()
        {
            _Parent.Remove(this);
            if (DragAndDrop) DeInitializeDragAndDrop();
            TextLabel.ChangeEvent -= TextLabelChange;
            TextLabelChange = null;
            if (!(Collider is null)) Collider.Dispose();
            for (int i = 0; i < _Controls.Count; i++) _Controls[i].Dispose();
            Clear();
            Texture.Dispose();
            _TextTexture.Dispose();
            _Controls = null;
        }

    }
}
