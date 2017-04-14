using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class ResourceTests
    {
        StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            _game = new StrategyGame();
            _game.SetupGame(GameSettings.Default());
            _game.LoadData();

            _game.Credits[0] = 0;
            _game.Credits[1] = 0;
        }

        [TestMethod]
        public void AddResources()
        {
            _game.AddResources(2, 100);

            _game.Credits[0].ShouldBe(0);
            _game.Credits[1].ShouldBe((int)(100 * StrategyGame.BaseConversionRate * _game.Faction[1].Bonuses.MiningEfficiency));
        }
        
        [TestMethod]
        public void SpendNoCredits()
        {
            var spent = _game.SpendCredits(1, 100);
            spent.ShouldBe(0);

            _game.Credits[0].ShouldBe(0);
        }

        [TestMethod]
        public void SpendSomeCredits()
        {
            _game.Credits[1] = 100;
            var spent = _game.SpendCredits(2, 200);
            spent.ShouldBe(100);

            _game.Credits[1].ShouldBe(0);
        }

        [TestMethod]
        public void SpendEnoughCredits()
        {
            _game.Credits[1] = 100;
            var spent = _game.SpendCredits(2, 75);
            spent.ShouldBe(75);

            _game.Credits[1].ShouldBe(25);
        }

        [TestMethod]
        public void SpendAllCredits()
        {
            _game.Credits[1] = 100;
            var spent = _game.SpendCredits(2, 100);
            spent.ShouldBe(100);

            _game.Credits[1].ShouldBe(0);
        }
    }
}
