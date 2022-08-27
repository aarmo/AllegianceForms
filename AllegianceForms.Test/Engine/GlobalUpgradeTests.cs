using AllegianceForms.Engine;
using AllegianceForms.Engine.Tech;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.IO;
using System.Linq;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class GlobalUpgradeTests
    {
        private TechTree _target;

        [TestInitialize]
        public void Setup()
        {
            var game = new StrategyGame();
            var techFile = Path.Combine(StrategyGame.DataDir, $"Tech-Amanni.txt");
            _target = TechTree.LoadTechTree(game, techFile, 1);
        }

        [TestMethod]
        public void CheckInitialGlobalUpgrades()
        {
            var values = (EGlobalUpgrade[])Enum.GetValues(typeof(EGlobalUpgrade));

            _target.ResearchedUpgrades.Keys.Count.ShouldBe(values.Length);

            foreach (var e in values)
            {
                _target.ResearchedUpgrades[e].ShouldBe(1);
            }
        }

        [TestMethod]
        public void CheckGlobalUpgradesAreRecognised()
        {
            var upgrades = _target.TechItems.Where(_ => _.Name.Contains("%")).ToList();

            foreach (var e in upgrades)
            {
                TechItem.IsGlobalUpgrade(e.Name).ShouldBe(true);
            }
        }

        [TestMethod]
        public void CheckPositiveGlobalUpgradesAreAllApplied()
        {
            var upgrades = _target.TechItems.Where(_ => _.Name.Contains("+") && _.Name.Contains("%")).ToList();

            foreach (var e in upgrades)
            {
                e.ApplyGlobalUpgrade(_target);

                var type = TechItem.GetGlobalUpgradeType(e.Name);
                var amount = TechItem.GetGlobalUpgradeAmount(e.Name);
                
                _target.ResearchedUpgrades[type].ShouldBe(1 + amount);
            }
        }

        [TestMethod]
        public void CheckNegativeGlobalUpgradesAreAllApplied()
        {
            var upgrades = _target.TechItems.Where(_ => _.Name.Contains("-") && _.Name.Contains("%")).ToList();
            
            foreach (var e in upgrades)
            {
                e.ApplyGlobalUpgrade(_target);

                var type = TechItem.GetGlobalUpgradeType(e.Name);
                var amount = TechItem.GetGlobalUpgradeAmount(e.Name);

                _target.ResearchedUpgrades[type].ShouldBe(1 + amount);
            }
        }
    }
}
