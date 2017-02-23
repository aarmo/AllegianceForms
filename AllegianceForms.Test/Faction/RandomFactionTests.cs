using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.Faction
{
    [TestClass]
    public class RandomFactionTests
    {
        private AllegianceForms.Engine.Factions.Faction _target;

        [TestInitialize]
        public void Setup()
        {
            _target = AllegianceForms.Engine.Factions.Faction.Random();
        }

        [TestMethod]
        public void RandomHasAName()
        {
            _target.Name.ShouldNotBeNullOrEmpty();
        }

        [TestMethod]
        public void RandomHasRandomBonuses()
        {
            var b = _target.Bonuses;
            var different = (b.FireRate != 0 || b.MiningCapacity != 0 || b.MiningSpeed != 0 || b.Regneration != 0 || b.ResearchCost != 0 || b.ResearchTime != 0 || b.ScanRange != 0 || b.Signature != 0 || b.Speed != 0);

            different.ShouldBe(true);
        }

        [TestMethod]
        public void RandomIsBalanced()
        {
            _target.Bonuses.IsBalanced().ShouldBe(true);
        }


    }
}
