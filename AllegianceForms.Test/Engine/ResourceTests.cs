using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class ResourceTests
    {
        [TestInitialize]
        public void Setup()
        {
            StrategyGame.Credits[0] = 0;
            StrategyGame.Credits[1] = 0;
            StrategyGame.GameStats = new GameStats();
            StrategyGame.GameSettings = GameSettings.Default();
            StrategyGame.LoadData();
        }

        [TestMethod]
        public void AddResources()
        {
            StrategyGame.AddResources(2, 100);

            StrategyGame.Credits[0].ShouldBe(0);
            StrategyGame.Credits[1].ShouldBe((int)(100 * StrategyGame.ConversionRate[1] * StrategyGame.Faction[1].Bonuses.MiningEfficiency));
        }
        
        [TestMethod]
        public void SpendNoCredits()
        {
            var spent = StrategyGame.SpendCredits(1, 100);
            spent.ShouldBe(0);

            StrategyGame.Credits[0].ShouldBe(0);
        }

        [TestMethod]
        public void SpendSomeCredits()
        {
            StrategyGame.Credits[1] = 100;
            var spent = StrategyGame.SpendCredits(2, 200);
            spent.ShouldBe(100);

            StrategyGame.Credits[1].ShouldBe(0);
        }

        [TestMethod]
        public void SpendEnoughCredits()
        {
            StrategyGame.Credits[1] = 100;
            var spent = StrategyGame.SpendCredits(2, 75);
            spent.ShouldBe(75);

            StrategyGame.Credits[1].ShouldBe(25);
        }

        [TestMethod]
        public void SpendAllCredits()
        {
            StrategyGame.Credits[1] = 100;
            var spent = StrategyGame.SpendCredits(2, 100);
            spent.ShouldBe(100);

            StrategyGame.Credits[1].ShouldBe(0);
        }
    }
}
