using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllegianceForms.Test.Balance
{
    [TestClass]
    public class BaseBalanceTests
    {
        [TestInitialize]
        public void Setup()
        {
            StrategyGame.LoadData();
            StrategyGame.ResetGame(GameSettings.Default());
        }

        [TestMethod]
        public void BaseCostsAreBalanced()
        {
            var bases = StrategyGame.Bases.Bases;
            var tech = StrategyGame.TechTree[0].TechItems;
            var results = new List<float>();

            foreach (var b in bases)
            {
                var name = b.Type.ToString() + " Constructor";
                var con = tech.FirstOrDefault(_ => _.Type == ETechType.Construction && _.Name == name);
                if (con == null) continue; 

                var factor = (1f * b.Health + b.ScanRange + b.ScanRange) / (con.Cost + con.DurationSec);
                results.Add(factor);
            }

            var diff = results.Max() - results.Min();

            diff.ShouldBeLessThan(0.1f);
        }
    }
}
