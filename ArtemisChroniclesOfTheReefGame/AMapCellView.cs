using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;

using CommonPrimitivesLibrary;
using GraphicsLibrary.Interfaces;
using GraphicsLibrary.StandartGraphicsPrimitives;

using GameLibrary;
using GameLibrary.Map;
using GameLibrary.Unit.Main;
using GameLibrary.Extension;
using GameLibrary.Technology;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

namespace ArtemisChroniclesOfTheReefGame
{
    public class AMapCellView: APrimitive, IPrimitive
    {

        private ALabel Label;

        private IPrimitiveTexture Source;
        private AMapCell _MapCell;
        public AMapCell MapCell { get => _MapCell; }

        readonly new string Text;
        readonly new ATextLabel TextLabel;

        private AGame Game;

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

        public AMapCellView(int radius, AGame game, AMapCell mapCell, IPrimitiveTexture source) : base(new ASize(radius, radius))
        {
            Game = game;
            DragAndDrop = true;
            IsDarkened = true;
            _MapCell = mapCell;
            Source = source;
        }

        public AMapCellView(AMapCell mapCell, AGame game, APrimitiveTexture source) : this(GraphicsExtension.DefaultMapCellRadius, game, mapCell, source) { }

        public override void Initialize()
        {
            Label = new ALabel(new ASize(GraphicsExtension.DefaultMapCellRadius / 2, GraphicsExtension.DefaultTextLabelSize.Height)) { Parent = this, Location = new APoint(GraphicsExtension.DefaultMapCellRadius / 4, Height - GraphicsExtension.DefaultTextLabelSize.Height - 10), Text = _MapCell.IsSettlement ? /*"lvl.[" + _MapCell.Settlement.Level + "] " + */ _MapCell.Settlement.Name : "", IsInteraction = false };
            Label.TextLabel.Font = GraphicsExtension.DefaultConstructionFont;
            Label.TextLabel.VerticalAlign = ATextVerticalAlign.Bottom;
            Label.TextLabel.HorizontalAlign = ATextHorizontalAlign.Center;
            Collider = new AHexCollider(Size.Height);
            if (Source is null) Source = new AHexTexture(GraphicsDevice, Size.Height) { IsDraw = true, IsFill = true };
            Texture = Source;
        }

        public void ChangeSource(IPrimitiveTexture source)
        {
            Texture = Source = source;
        }

        public override void OnLocationChangeProcess()
        {
            //nothing
        }

        public override bool PreLocationChangeProcess(APoint point)
        {
            if (ForcedActive) return false;
            else return true;
        }

        public void UpdateLabel(bool isShowCost)
        {
            Label.Text = isShowCost ? 
                GameLocalization.Biomes[_MapCell.BiomeType] + "\nЦена: "/* + GameExtension.CellCostByBiome[_MapCell.BiomeType]*/ :
                _MapCell.IsSettlement ? 
                /*"ур. " + _MapCell.Settlement.Level + ", " +*/ _MapCell.Settlement.Name : 
                " ";
        }

        public new void Draw(SpriteBatch spriteBatch)
        {

            bool isUnit = Game.GetUnits(_MapCell.Location) is List<IUnit> units && units.Count > 0;

            if (MapCell.IsSettlement && !MapCell.Settlement.Name.Equals(Label.Text)) Label.Text = MapCell.Settlement.Name;
            Enabled = Game.ActivePlayer.ExploredTerritories.Contains(_MapCell.Location);

            _IsDarkenedTexture = Game.IsMapCellSelected && Game.SelectedMapCell.IsSettlement && Game.SelectedMapCell.Settlement.Territories.Contains(_MapCell);
            
            if (Enabled)
            {

                spriteBatch.Draw(IsDarkenedTexture ? TexturePack.Hex[_MapCell.OwnerId].DarkenedTexture : TexturePack.Hex[_MapCell.OwnerId].Texture, new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, /*scaling*/1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(TexturePack.Biome(_MapCell.BiomeType, isUnit, _MapCell.IsSettlement), new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, /*scaling*/1f, SpriteEffects.None, 0f);
                if (_MapCell.IsSettlement) spriteBatch.Draw(TexturePack.Construction(_MapCell.BiomeType, isUnit), new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, /*scaling*/1f, SpriteEffects.None, 0f);
                if (isUnit)
                {

                    if (!(_MapCell.ActiveUnit is null))
                    {
                        spriteBatch.Draw(TexturePack.Unit(_MapCell.ActiveUnit.UnitType), new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        spriteBatch.Draw(TexturePack.Banners[_MapCell.ActiveUnit.Owner.Id], new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }

                if (Game.ActivePlayer.IsResource(MapCell.ResourceType)) spriteBatch.Draw(TexturePack.Resource(_MapCell.ResourceType, _MapCell.IsMined), new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, /*scaling*/1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(TextTexture, new Vector2(GlobalLocation.X + VisibleArea.Location.X, GlobalLocation.Y + VisibleArea.Location.Y), new Rectangle(VisibleArea.Location.X, VisibleArea.Location.Y, VisibleArea.Size.Width, VisibleArea.Size.Height), Color.White, 0f, Vector2.Zero, /*scaling*/1f, SpriteEffects.None, 0f);

                foreach (IPrimitive primitive in Controls.OrderBy(x => x.ZIndex).ToList())
                {
                    primitive.Draw(spriteBatch);
                }
            }
        }

        public new bool InCollider(AMouseState mouseState, out IPrimitive activeChild)
        {
            activeChild = null;
            List<APoint> points = new List<APoint>(Collider.Points);
            for (int i = 0; i < points.Count; i++)
            {
                points[i] += GlobalLocation + Collider.Location;
            }

            if (!mouseState.MouseButton.Equals(AMouseButton.Middle))
            {
                if (Enabled && Collider.InCollider(mouseState.CursorPosition, points))
                {
                    if (!ForcedActive) foreach (IPrimitive e in Controls.OrderByDescending(x => x.ZIndex).ToList())
                        {
                            if (e.Enabled && e.IsInteraction)
                            {
                                if (e.InCollider(mouseState, out activeChild))
                                {
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
            }
            else if (!mouseState.CursorPosition.Equals(LastPoint)) OnForcedMouseButtonUp(mouseState);

            return false;
        }

        public new void Dispose()
        {
            base.Dispose();
        }

    }
}
