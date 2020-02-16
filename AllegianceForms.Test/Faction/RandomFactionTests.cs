using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.Faction
{
    [TestClass]
    public class RandomFactionTests
    {
        private AllegianceForms.Engine.Factions.Faction _target;
        private GameSettings _settings;
        [TestInitialize]
        public void Setup()
        {
            _settings = GameSettings.Default();
            _target = AllegianceForms.Engine.Factions.Faction.Random(_settings);
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
            var different = (b.FireRate != 0 || b.MiningCapacity != 0 || b.MiningEfficiency != 0 || b.MissileSpeed != 0 || b.MissileTracking != 0 || b.ResearchCost != 0 || b.ResearchTime != 0 || b.ScanRange != 0 || b.Signature != 0 || b.Speed != 0);

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
            var f = AllegianceForms.Engine.Factions.Faction.Random(_settings, 11);

            var b1 = _target.Bonuses;
            var b2 = f.Bonuses;

            var different = (b1.FireRate != b1.FireRate 
                || b1.MiningCapacity != b2.MiningCapacity 
                || b1.MiningEfficiency != b2.MiningEfficiency
                || b1.MissileSpeed != b2.MissileSpeed
                || b1.MissileTracking != b2.MissileTracking
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
