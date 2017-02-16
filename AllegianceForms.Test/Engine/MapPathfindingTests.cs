using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class MapPathfindingTests
    {
        [TestInitialize]
        public void Setup()
        {
            StrategyGame.ResetGame(GameSettings.Default());
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_Brawl()
        {
            var target = GameMaps.Brawl();
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldBeNull();
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_DoubleRing()
        {
            var target = GameMaps.DoubleRing();
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(1);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_Grid()
        {
            var target = GameMaps.Grid();
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_HiHigher()
        {
            var target = GameMaps.HiHigher();
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_HiLo()
        {
            var target = GameMaps.HiLo();
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(2);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_SingleRing()
        {
            var target = GameMaps.SingleRing();
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(2);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_PinWheel()
        {
            var target = GameMaps.PinWheel();
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_Star()
        {
            var target = GameMaps.Star();
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(4);
        }

        [TestMethod]
        public void TestNextWormholeBetweenStartingSectors_Star_Step1()
        {
            StrategyGame.Map = GameMaps.Star();
            StrategyGame.Map.SetVisibilityToTeam(1, true);

            GameEntity otherEnd;
            var next = StrategyGame.NextWormholeEnd(1, 0, 4, out otherEnd);

            next.ShouldNotBeNull();
            otherEnd.ShouldNotBeNull();

            next.SectorId.ShouldBe(0);
            otherEnd.SectorId.ShouldBe(1);
        }

        [TestMethod]
        public void TestNextWormholeBetweenStartingSectors_Star_Step2()
        {
            StrategyGame.Map = GameMaps.Star();
            StrategyGame.Map.SetVisibilityToTeam(1, true);

            GameEntity otherEnd;
            var next = StrategyGame.NextWormholeEnd(1, 1, 4, out otherEnd);

            next.ShouldNotBeNull();
            otherEnd.ShouldNotBeNull();

            next.SectorId.ShouldBe(1);
            otherEnd.SectorId.ShouldBe(2);
        }

        [TestMethod]
        public void TestNextWormholesWhenNotVisible()
        {
            StrategyGame.Map = GameMaps.Star();

            GameEntity otherEnd;
            var next = StrategyGame.NextWormholeEnd(1, 1, 4, out otherEnd);

            next.ShouldBeNull();
            otherEnd.ShouldBeNull();
        }

        [TestMethod]
        public void TestNextWormholesWhenSomeVisible()
        {
            StrategyGame.Map = GameMaps.Star();
            StrategyGame.Map.Sectors[0].VisibleToTeam[0] = true;
            StrategyGame.Map.Sectors[1].VisibleToTeam[0] = true;
            StrategyGame.Map.Sectors[2].VisibleToTeam[0] = true;

            GameEntity otherEnd;
            var next = StrategyGame.NextWormholeEnd(1, 0, 2, out otherEnd);

            next.ShouldNotBeNull();
            otherEnd.ShouldNotBeNull();

            next.SectorId.ShouldBe(0);
            otherEnd.SectorId.ShouldBe(1);
        }
    }
}
