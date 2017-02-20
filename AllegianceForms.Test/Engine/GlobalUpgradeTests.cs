using AllegianceForms.Engine;
using AllegianceForms.Engine.Tech;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
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
            _target = TechTree.LoadTechTree(StrategyGame.TechDataFile, 1);
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
            var check = new List<EGlobalUpgrade>();

            foreach (var e in upgrades)
            {
                e.ApplyGlobalUpgrade(_target);
                check.Add(TechItem.GetGlobalUpgradeType(e.Name));
            }
            
            foreach (var e in check)
            {
                _target.ResearchedUpgrades[e].ShouldBe(1.2f);
            }
        }

        [TestMethod]
        public void CheckNegativeGlobalUpgradesAreAllApplied()
        {
            var upgrades = _target.TechItems.Where(_ => _.Name.Contains("-") && _.Name.Contains("%")).ToList();
            var check = new List<EGlobalUpgrade>();

            foreach (var e in upgrades)
            {
                e.ApplyGlobalUpgrade(_target);
                check.Add(TechItem.GetGlobalUpgradeType(e.Name));
            }

            foreach (var e in check)
            {
                _target.ResearchedUpgrades[e].ShouldBe(0.8f);
            }
        }
    }
}
