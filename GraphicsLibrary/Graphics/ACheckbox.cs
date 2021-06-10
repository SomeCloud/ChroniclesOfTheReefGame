using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class ACheckbox : APrimitive, IPrimitive
    {

        public delegate void OnValueChange(bool value);

        public event OnValueChange ValueChange;

        private IPrimitiveTexture ActiveTexture;
        private IPrimitiveTexture NotActiveTexture;

        public bool _Value;
        public bool Value => _Value;

        public ACheckbox(Texture2D active, Texture2D notActive) : base(GraphicsExtension.DefaultCheckboxSize)
        {

            ActiveTexture = new ARectangleTexture(active.GraphicsDevice, Size) { IsDraw = true, IsFill = true };
            ActiveTexture.FillBySource(active);

            NotActiveTexture = new ARectangleTexture(notActive.GraphicsDevice, Size) { IsDraw = true, IsFill = true };
            NotActiveTexture.FillBySource(notActive);

            MouseClickEvent += (state, mstate) =>
            {
                if (_Value) Deactivate();
                else Activate();
            };

        }

        public void Activate()
        {
            _Value = true;
            ValueChange?.Invoke(_Value);
            Texture = ActiveTexture;
        }
        
        public void Deactivate()
        {
            _Value = false;
            ValueChange?.Invoke(_Value);
            Texture = NotActiveTexture;
        }

        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
            Texture = NotActiveTexture;
        }

        public override void OnLocationChangeProcess()
        {
            //nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }

    }
}
