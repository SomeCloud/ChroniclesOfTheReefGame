using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class AGraphics: APrimitive, IPrimitive
    {

        private IPrimitive ActivePrimitive;

        public AGraphics(ASize size, GraphicsDevice graphicsDevice): base(size)
        {
            _GraphicsDevice = graphicsDevice;
        }

        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
        }

        public override void OnLocationChangeProcess()
        {
            // nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            return true;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {

            foreach (IPrimitive primitive in Controls.OrderBy(x => x.ZIndex).ToList())
            {
                primitive.Draw(spriteBatch);
            }

        }

        public new void ProcessKey(AKeyboardState keyboardState)
        {
            if (ActivePrimitive is object)
            {
                ActivePrimitive.ProcessKey(keyboardState);
            }
        }

        public void ProcessClick(AMouseState mouseState)
        {
            IPrimitive Temp = null;
            if (!(ActivePrimitive is null) && ActivePrimitive.ForcedActive)
            {
                
                if (ActivePrimitive.Enabled)
                {
                    ActivePrimitive.InCollider(mouseState, out Temp);
                }
            }
            else
            {

                foreach (IPrimitive e in Controls.Where(e => e.Enabled).OrderByDescending(x => x.ZIndex).ToList())
                {
                    if (e.Enabled && e.InCollider(mouseState, out Temp))
                    {
                        if (!(ActivePrimitive is null) && ActivePrimitive != Temp)
                        {                  
                            ActivePrimitive.InvokeMouseOverEvent(mouseState);
                        }
                        break;
                    }
                }

            }

            ActivePrimitive = Temp;

        }

    }
}
