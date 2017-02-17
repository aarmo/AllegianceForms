using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class PilotTests
    {
        [TestInitialize]
        public void Setup()
        {
            StrategyGame.DockedPilots[0] = 0;
            StrategyGame.DockedPilots[1] = 0;
        }

        [TestMethod]
        public void DockPilots()
        {
            StrategyGame.DockShip(2, 10);

            StrategyGame.DockedPilots[0].ShouldBe(0);
            StrategyGame.DockedPilots[1].ShouldBe(10);
        }

        [TestMethod]
        public void CantLaunchWithNoPilots()
        {
            var result = StrategyGame.CanLaunchShip(2, 1, EShipType.Scout);

            result.ShouldBe(false);
        }

        [TestMethod]
        public void CantLaunchWithoutAllPilots()
        {
            StrategyGame.DockedPilots[1] = 1;
            var result = StrategyGame.CanLaunchShip(2, 2, EShipType.Scout);

            result.ShouldBe(false);
        }

        [TestMethod]
        public void CanLaunchWithEnoughPilots()
        {
            StrategyGame.DockedPilots[1] = 1;
            var result = StrategyGame.CanLaunchShip(2, 1, EShipType.Scout);

            result.ShouldBe(true);
            StrategyGame.DockedPilots[1].ShouldBe(1);
        }

        [TestMethod]
        public void CantLaunchAConstructor()
        {
            StrategyGame.DockedPilots[1] = 10;
            var result = StrategyGame.CanLaunchShip(2, 1, EShipType.Constructor);

            result.ShouldBe(false);
            StrategyGame.DockedPilots[1].ShouldBe(10);
        }

        [TestMethod]
        public void CantLaunchATower()
        {
            StrategyGame.DockedPilots[1] = 10;
            var result = StrategyGame.CanLaunchShip(2, 1, EShipType.Tower);

            result.ShouldBe(false);
            StrategyGame.DockedPilots[1].ShouldBe(10);
        }

        [TestMethod]
        public void CanLaunchWithMorePilots()
        {
            StrategyGame.DockedPilots[1] = 4;
            var result = StrategyGame.CanLaunchShip(2, 2, EShipType.Scout);

            result.ShouldBe(true);
            StrategyGame.DockedPilots[1].ShouldBe(4);
        }

        [TestMethod]
        public void LaunchWithoutAllPilots()
        {
            StrategyGame.DockedPilots[1] = 1;
            var ship = new Ship(string.Empty, 10, 10, System.Drawing.Color.DimGray, 2, 10, 2, 0);
            StrategyGame.LaunchShip(ship);
            StrategyGame.DockedPilots[1].ShouldBe(1);
        }

        [TestMethod]
        public void LaunchWithEnoughPilots()
        {
            StrategyGame.DockedPilots[1] = 1;
            var ship = new Ship(string.Empty, 10, 10, System.Drawing.Color.DimGray, 2, 10, 1, 0);
            StrategyGame.LaunchShip(ship);
            StrategyGame.DockedPilots[1].ShouldBe(0);
        }

        [TestMethod]
        public void LaunchWithMorePilots()
        {
            StrategyGame.DockedPilots[1] = 4;
            var ship = new Ship(string.Empty, 10, 10, System.Drawing.Color.DimGray, 2, 10, 3, 0);
            StrategyGame.LaunchShip(ship);
            StrategyGame.DockedPilots[1].ShouldBe(1);
        }
    }
}
