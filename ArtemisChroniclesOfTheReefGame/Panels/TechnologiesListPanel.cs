using System;
using System.Linq;
using System.Collections.Generic;

using GraphicsLibrary;
using GraphicsLibrary.Graphics;
using GraphicsLibrary.Interfaces;

using AScrollbarAlign = GraphicsLibrary.StandartGraphicsPrimitives.AScrollbarAlign;

using APoint = CommonPrimitivesLibrary.APoint;
using ASize = CommonPrimitivesLibrary.ASize;
using GameLibrary;
using GameLibrary.Technology;
using GameLibrary.Extension;

namespace ArtemisChroniclesOfTheReefGame.Panels
{
    public class TechnologiesListPanel: AScrolleredPanel
    {

        public delegate void OnSelect(ITechnology technology);
        public event OnSelect SelectEvent;

        private Dictionary<ATechnologyType, AButton> TechnologiesList;
        private Dictionary<AButton, OnMouseEvent> EventsList;

        public TechnologiesListPanel(ASize size): base(AScrollbarAlign.Vertical, size)
        {
            TechnologiesList = new Dictionary<ATechnologyType, AButton>();
            EventsList = new Dictionary<AButton, OnMouseEvent>();
        }

        public override void Initialize()
        {

            base.Initialize();

            TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
            TextLabel.VerticalAlign = ATextVerticalAlign.Top;
            TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

            Text = "Древо технологий";

        }

        public void Update(ATechnologyTree technologyTree)
        {

            Scrollbar.Value = Scrollbar.MinValue;

            APoint last = new APoint(10, 0);

            foreach (var bt in EventsList) bt.Key.MouseClickEvent -= bt.Value;
            EventsList.Clear();

            foreach (ITechnology technology in technologyTree.Technologies.Values)
            {
                AButton button;

                if (TechnologiesList.ContainsKey(technology.TechnologyType))
                {
                    button = TechnologiesList[technology.TechnologyType];
                    string text = technology.Name + " (" + (technology.IsCompleted ? "изучено" : "не изучено") + "): " + technology.StudyPoints + " / " + technology.RequiredStudyPoints + "\nТребуется: " + (technology.RequiredTechnologies.Count > 0? string.Join(", ", technology.RequiredTechnologies.Select(x => GameLocalization.Technologies[x])): "-");
                    if (!button.Text.Equals(text)) button.Text = text;
                    button.FillColor = technology.IsCompleted ? GraphicsExtension.ExtraColorGreen : technologyTree.Technologies.Values.Where(x => technology.RequiredTechnologies.Contains(x.TechnologyType)).All(x => x.IsCompleted) ? GraphicsExtension.ExtraColorYellow : GraphicsExtension.ExtraColorRed;
                    button.IsInteraction = !technology.IsCompleted;

                    EventsList.Add(button, (state, mstate) => { if (!technology.IsCompleted && technologyTree.Technologies.Where(x => technology.RequiredTechnologies.Contains(x.Key)).All(x => x.Value.IsCompleted)) SelectEvent?.Invoke(technology); });
                    button.MouseClickEvent += EventsList[button];

                }
                else
                {
                    button = new AButton(new ASize(ContentSize.Width - 20, 50));
                    button.Text = technology.Name + " (" + (technology.IsCompleted ? "изучено" : "не изучено") + "): " + technology.StudyPoints + " / " + technology.RequiredStudyPoints + "\nТребуется: " + (technology.RequiredTechnologies.Count > 0? string.Join(", ", technology.RequiredTechnologies.Select(x => GameLocalization.Technologies[x])): "-");
                    
                    Add(button);

                    TechnologiesList.Add(technology.TechnologyType, button);

                    button.FillColor = technology.IsCompleted ? GraphicsExtension.ExtraColorGreen : technologyTree.Technologies.Values.Where(x => technology.RequiredTechnologies.Contains(x.TechnologyType)).All(x => x.IsCompleted)? GraphicsExtension.ExtraColorYellow: GraphicsExtension.ExtraColorRed;

                    button.TextLabel.HorizontalAlign = ATextHorizontalAlign.Left;
                    button.TextLabel.Font = new System.Drawing.Font(GraphicsExtension.ExtraFontFamilyName, 10);

                    EventsList.Add(button, (state, mstate) => { if (!technology.IsCompleted && technologyTree.Technologies.Where(x => technology.RequiredTechnologies.Contains(x.Key)).All(x => x.Value.IsCompleted)) SelectEvent?.Invoke(technology); });

                    button.MouseClickEvent += EventsList[button];

                    button.IsInteraction = !technology.IsCompleted;
                }

                button.Location = last + new APoint(0, button.Height + 10);
                last = button.Location;
            }

            ContentSize = new ASize(ContentSize.Width, TechnologiesList.Count > 0 ? last.Y + 60 : Height);
            Scrollbar.MaxValue = Height < ContentSize.Height ? ContentSize.Height - Height + 10 : 0;

            Text = "Древо технологий (" + technologyTree.Technologies.Where(x => x.Value.IsCompleted).Count() + " / " + technologyTree.Technologies.Count + ")";

        }

    }
}
