using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLibrary.Technology
{
    [Serializable]
    public class ATechnologyTree
    {

        private Dictionary<ATechnologyType, ITechnology> _Technologies;

        public IReadOnlyDictionary<ATechnologyType, ITechnology> Technologies => _Technologies;

        public ATechnologyTree()
        {
            _Technologies = new Dictionary<ATechnologyType, ITechnology>();
            Initialize();
            _Technologies.Values.First().Increase(1);
        }

        protected void Initialize()
        {

            foreach (ATechnologyType technology in Enum.GetValues(typeof(ATechnologyType)).Cast<ATechnologyType>()) _Technologies.Add(technology, new ATechnology(technology));
            /*
            ITechnology PrimitiveSociety = new ATechnology(ATechnologyType.PrimitiveSociety);
            ITechnology HuntingAndGathering = new ATechnology(ATechnologyType.HuntingAndGathering);
            ITechnology Paganism = new ATechnology(ATechnologyType.Paganism);
            ITechnology Agriculture = new ATechnology(ATechnologyType.Agriculture);
            ITechnology Fishing = new ATechnology(ATechnologyType.Fishing);
            ITechnology Masonry = new ATechnology(ATechnologyType.Masonry);
            ITechnology Phytotherapy = new ATechnology(ATechnologyType.Phytotherapy);
            ITechnology Priesthood = new ATechnology(ATechnologyType.Priesthood);
            ITechnology WoodProcessing = new ATechnology(ATechnologyType.WoodProcessing);
            ITechnology StoneProcessing = new ATechnology(ATechnologyType.StoneProcessing);
            ITechnology Mining = new ATechnology(ATechnologyType.Mining);
            ITechnology Medicine = new ATechnology(ATechnologyType.Medicine);
            ITechnology AnimalHusbandry = new ATechnology(ATechnologyType.AnimalHusbandry);
            ITechnology BronzeProcessing = new ATechnology(ATechnologyType.BronzeProcessing);
            ITechnology Writing = new ATechnology(ATechnologyType.Writing);
            ITechnology Handicraft = new ATechnology(ATechnologyType.Handicraft);
            ITechnology CastingMetals = new ATechnology(ATechnologyType.CastingMetals);
            ITechnology Alphabet = new ATechnology(ATechnologyType.Alphabet);
            ITechnology MilitaryTraditions = new ATechnology(ATechnologyType.MilitaryTraditions);
            ITechnology Mathematics = new ATechnology(ATechnologyType.Mathematics);
            ITechnology TheatricalArt = new ATechnology(ATechnologyType.TheatricalArt);
            ITechnology Сurrency = new ATechnology(ATechnologyType.Сurrency);
            ITechnology Building = new ATechnology(ATechnologyType.Building);
            ITechnology Banking = new ATechnology(ATechnologyType.Banking);
            ITechnology CodeOfLaws = new ATechnology(ATechnologyType.CodeOfLaws);
            ITechnology Construction = new ATechnology(ATechnologyType.Construction);
            ITechnology Education = new ATechnology(ATechnologyType.Education);
            */
        }

        public void Increase(ATechnologyType technologyType, int count) { if (_Technologies[technologyType].RequiredTechnologies.All(x => _Technologies[x].IsCompleted)) _Technologies[technologyType].Increase(count); }

    }
}
