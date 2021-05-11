using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

namespace GraphicsLibrary.Graphics
{
    public class AMapView : APrimitive, IPrimitive
    {
        private IPrimitiveTexture Source;
        private Color _Color;

        public Color Color
        {
            get => _Color;
            set
            {
                _Color = value;
                Source.FillColor = value;
            }
        }

        //public new readonly bool DragAndDrop;

        public AMapView(ASize size) : base(size)
        {
            //DragAndDrop = true;
        }

        public AMapView() : this(GraphicsExtension.DefaultMapViewSize) { }

        public override void Initialize()
        {
            Collider = new ARectangleCollider(Size);
            Texture = Source = new ARectangleTexture(GraphicsDevice, Size) { IsDraw = false, IsFill = false };
        }

        public override void OnLocationChangeProcess()
        {
            // nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            if (!(Parent is null) && (point.X + Width > GraphicsExtension.DefaultMapCellRadius && point.X < Parent.Width - GraphicsExtension.DefaultMapCellRadius) && (point.Y + Height > GraphicsExtension.DefaultMapCellRadius && point.Y < Parent.Height - GraphicsExtension.DefaultMapCellRadius)) return true;
            else return false;
        }
        
        public new bool InCollider(AMouseState mouseState, out IPrimitive activeChild)
        {
            activeChild = null;
            List<APoint> points = new List<APoint>(Collider.Points);
            for (int i = 0; i < points.Count; i++)
            {
                points[i] += GlobalLocation + Collider.Location;
            }

            if (Collider.InCollider(mouseState.CursorPosition, points))
            {
                if (!ForcedActive) foreach (IPrimitive e in Controls.OrderByDescending(x => x.ZIndex).ToList())
                    {
                        if (e.Enabled && e.IsInteraction)
                        {
                            if (e.InCollider(mouseState, out activeChild))
                            {
                                if (e.ForcedActive && mouseState.MouseButton.Equals(AMouseButton.Middle))
                                {
                                    e.OnForcedMouseButtonUp(mouseState);
                                    break;
                                }
                                else return true;
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
        
        public new void Dispose()
        {
            Source.Dispose();
            base.Dispose();
        }

    }
}
