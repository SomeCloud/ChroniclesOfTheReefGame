using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class ALabelList : AScrolleredPanel
    {

        private Dictionary<string, ALabel> Labels;

        public ALabelList(ASize size) : base(AScrollbarAlign.Vertical, size)
        {
            Labels = new Dictionary<string, ALabel>();
        }

        public override void Initialize()
        {

            base.Initialize();

            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

        }

        public new void Clear()
        {
            base.Clear();
            Labels.Clear();
        }

        public void Update(IEnumerable<string> labels)
        {
            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, -30);

            foreach (ALabel bt in Labels.Values) bt.Enabled = false;

            foreach (string player in labels)
            {

                ALabel label;

                if (Labels.Keys.Where(x => x.Equals(player)).Count() != 0)
                {
                    label = Labels[Labels.Keys.Where(x => x.Equals(player)).First()];
                    string text = player;
                    if (!label.Text.Equals(text)) label.Text = text;
                    label.Enabled = true;
                }
                else
                {
                    label = new ALabel(new ASize(ContentSize.Width - 20, 80));
                    Add(label);
                    Labels.Add(player, label);

                    label.Text = player;

                    label.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    label.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 14);

                }

                label.Location = last + new APoint(0, label.Height + 10);
                last = label.Location;

            }

            ContentSize = new ASize(ContentSize.Width, Labels.Count > 0 ? last.Y + 80 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

        }

    }
}
