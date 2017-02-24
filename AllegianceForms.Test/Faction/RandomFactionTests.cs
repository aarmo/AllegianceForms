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

        [TestMethod]
        public void MoreRandomIsDifferent()
        {
            var f = AllegianceForms.Engine.Factions.Faction.Random(30);

            var b1 = _target.Bonuses;
            var b2 = f.Bonuses;

            var different = (b1.FireRate != b1.FireRate 
                || b1.MiningCapacity != b2.MiningCapacity 
                || b1.MiningSpeed != b2.MiningSpeed 
                || b1.Regneration != b2.Regneration 
                || b1.ResearchCost != b2.ResearchCost 
                || b1.ResearchTime != b2.ResearchTime 
                || b1.ScanRange != b2.ScanRange 
                || b1.Signature != b2.Signature 
                || b1.Speed != b2.Speed
                || b1.Health != b2.Health)
                && _target.Name != f.Name;

            different.ShouldBe(true);

        }

    }
}
