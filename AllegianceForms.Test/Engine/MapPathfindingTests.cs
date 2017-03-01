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
            StrategyGame.SetupGame(GameSettings.Default());
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
            var target = GameMaps.DoubleRing(2);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(1);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_DoubleRing3()
        {
            var target = GameMaps.DoubleRing(3);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var mid = target.Sectors.Where(_ => _.StartingSector).Skip(1).First();
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path1 = target.ShortestPath(1, start.Id, end.Id);
            path1.ShouldNotBeNull();
            path1.Count.ShouldBe(1);

            var path2 = target.ShortestPath(1, start.Id, mid.Id);
            path2.ShouldNotBeNull();
            path2.Count.ShouldBe(1);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_DoubleRing4()
        {
            var target = GameMaps.DoubleRing(4);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var mid2 = target.Sectors.Where(_ => _.StartingSector).Skip(1).First();
            var mid3 = target.Sectors.Where(_ => _.StartingSector).Skip(2).First();
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path1 = target.ShortestPath(1, start.Id, end.Id);
            path1.ShouldNotBeNull();
            path1.Count.ShouldBe(1);

            var path2 = target.ShortestPath(1, start.Id, mid2.Id);
            path2.ShouldNotBeNull();
            path2.Count.ShouldBe(1);

            var path3 = target.ShortestPath(1, start.Id, mid3.Id);
            path3.ShouldNotBeNull();
            path3.Count.ShouldBe(2);
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
            var target = GameMaps.SingleRing(2);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(2);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_SingleRing3()
        {
            var target = GameMaps.SingleRing(3);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var mid = target.Sectors.Where(_ => _.StartingSector).Skip(1).First();
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path1 = target.ShortestPath(1, start.Id, end.Id);
            path1.ShouldNotBeNull();
            path1.Count.ShouldBe(2);

            var path2 = target.ShortestPath(1, start.Id, mid.Id);
            path2.ShouldNotBeNull();
            path2.Count.ShouldBe(2);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_SingleRing4()
        {
            var target = GameMaps.SingleRing(4);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var mid2 = target.Sectors.Where(_ => _.StartingSector).Skip(1).First();
            var mid3 = target.Sectors.Where(_ => _.StartingSector).Skip(2).First();
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path1 = target.ShortestPath(1, start.Id, end.Id);
            path1.ShouldNotBeNull();
            path1.Count.ShouldBe(2);

            var path2 = target.ShortestPath(1, start.Id, mid2.Id);
            path2.ShouldNotBeNull();
            path2.Count.ShouldBe(2);

            var path3 = target.ShortestPath(1, start.Id, mid3.Id);
            path3.ShouldNotBeNull();
            path3.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_PinWheel()
        {
            var target = GameMaps.PinWheel(2);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_PinWheel3()
        {
            var target = GameMaps.PinWheel(3);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var mid = target.Sectors.Where(_ => _.StartingSector).Skip(1).First();
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path1 = target.ShortestPath(1, start.Id, end.Id);
            path1.ShouldNotBeNull();
            path1.Count.ShouldBe(4);

            var path2 = target.ShortestPath(1, start.Id, mid.Id);
            path2.ShouldNotBeNull();
            path2.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_PinWheel4()
        {
            var target = GameMaps.PinWheel(4);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector);
            var mid2 = target.Sectors.Where(_ => _.StartingSector).Skip(1).First();
            var mid3 = target.Sectors.Where(_ => _.StartingSector).Skip(2).First();
            var end = target.Sectors.Last(_ => _.StartingSector);

            var path1 = target.ShortestPath(1, start.Id, end.Id);
            path1.ShouldNotBeNull();
            path1.Count.ShouldBe(3);

            var path2 = target.ShortestPath(1, start.Id, mid2.Id);
            path2.ShouldNotBeNull();
            path2.Count.ShouldBe(3);

            var path3 = target.ShortestPath(1, start.Id, mid3.Id);
            path3.ShouldNotBeNull();
            path3.Count.ShouldBe(4);
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
