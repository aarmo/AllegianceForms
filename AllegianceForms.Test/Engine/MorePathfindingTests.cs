using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class MorePathfindingTests
    {
        [TestInitialize]
        public void Setup()
        {
            StrategyGame.ResetGame(GameSettings.Default());
        }

        [TestMethod]
        public void TestPathBetweenProblemSectors()
        {
            var target = GameMaps.Star();
            var path = target.ShortestPath(0, 4);

            path.ShouldNotBeNull();

            path.Count.ShouldBe(4);
        }

        [TestMethod]
        public void TestNextWormholesBetweenProblemSectors_Step1()
        {
            StrategyGame.Map = GameMaps.Star();

            GameEntity otherEnd;
            var next = StrategyGame.NextWormholeEnd(1, 0, 4, out otherEnd);

            next.ShouldNotBeNull();
            otherEnd.ShouldNotBeNull();

            next.SectorId.ShouldBe(0);
            otherEnd.SectorId.ShouldBe(1);
        }

        [TestMethod]
        public void TestNextWormholesBetweenProblemSectors_Step2()
        {
            StrategyGame.Map = GameMaps.Star();
            GameEntity otherEnd;
            var next = StrategyGame.NextWormholeEnd(1, 1, 4, out otherEnd);

            next.ShouldNotBeNull();
            otherEnd.ShouldNotBeNull();

            next.SectorId.ShouldBe(1);
            otherEnd.SectorId.ShouldBe(2);
        }
    }
}
