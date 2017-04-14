using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class PilotTests
    {
        private StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            _game = new StrategyGame();

            _game.SetupGame(GameSettings.Default());
            _game.LoadData();

            _game.DockedPilots[0] = 0;
            _game.DockedPilots[1] = 0;
        }

        [TestMethod]
        public void DockPilots()
        {
            _game.DockPilots(2, 10);

            _game.DockedPilots[0].ShouldBe(0);
            _game.DockedPilots[1].ShouldBe(10);
        }

        [TestMethod]
        public void CantLaunchWithNoPilots()
        {
            var result = _game.CanLaunchShip(2, 1, EShipType.Scout);

            result.ShouldBe(false);
        }

        [TestMethod]
        public void CantLaunchWithoutAllPilots()
        {
            _game.DockedPilots[1] = 1;
            var result = _game.CanLaunchShip(2, 2, EShipType.Scout);

            result.ShouldBe(false);
        }

        [TestMethod]
        public void CanLaunchWithEnoughPilots()
        {
            _game.DockedPilots[1] = 1;
            var result = _game.CanLaunchShip(2, 1, EShipType.Scout);

            result.ShouldBe(true);
            _game.DockedPilots[1].ShouldBe(1);
        }

        [TestMethod]
        public void CantLaunchAConstructor()
        {
            _game.DockedPilots[1] = 10;
            var result = _game.CanLaunchShip(2, 1, EShipType.Constructor);

            result.ShouldBe(false);
            _game.DockedPilots[1].ShouldBe(10);
        }

        [TestMethod]
        public void CantLaunchATower()
        {
            _game.DockedPilots[1] = 10;
            var result = _game.CanLaunchShip(2, 1, EShipType.Tower);

            result.ShouldBe(false);
            _game.DockedPilots[1].ShouldBe(10);
        }

        [TestMethod]
        public void CanLaunchWithMorePilots()
        {
            _game.DockedPilots[1] = 4;
            var result = _game.CanLaunchShip(2, 2, EShipType.Scout);

            result.ShouldBe(true);
            _game.DockedPilots[1].ShouldBe(4);
        }

        [TestMethod]
        public void LaunchWithoutAllPilots()
        {
            _game.DockedPilots[1] = 1;
            var ship = new Ship(_game, string.Empty, 10, 10, System.Drawing.Color.DimGray, 2, 1, 10, 2, 0);
            _game.LaunchShip(ship);
            _game.DockedPilots[1].ShouldBe(1);
        }

        [TestMethod]
        public void LaunchWithEnoughPilots()
        {
            _game.DockedPilots[1] = 1;
            var ship = new Ship(_game, string.Empty, 10, 10, System.Drawing.Color.DimGray, 2, 1, 10, 1, 0);
            _game.LaunchShip(ship);
            _game.DockedPilots[1].ShouldBe(0);
        }

        [TestMethod]
        public void LaunchWithMorePilots()
        {
            _game.DockedPilots[1] = 4;
            var ship = new Ship(_game, string.Empty, 10, 10, System.Drawing.Color.DimGray, 2, 1, 10, 3, 0);
            _game.LaunchShip(ship);
            _game.DockedPilots[1].ShouldBe(1);
        }
    }
}
