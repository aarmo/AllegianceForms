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
            var target = GameMaps.LoadMap("DoubleRing2");
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
            var target = GameMaps.LoadMap("DoubleRing3");
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
            var target = GameMaps.LoadMap("DoubleRing4");
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
            var target = GameMaps.LoadMap("Grid2");
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
            var target = GameMaps.LoadMap("HiHigher2");
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
            var target = GameMaps.LoadMap("HiLo2");
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
            var target = GameMaps.LoadMap("SingleRing2");
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
            var target = GameMaps.LoadMap("SingleRing3");
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
            var target = GameMaps.LoadMap("SingleRing4");
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
            var target = GameMaps.LoadMap("PinWheel2");
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
            var target = GameMaps.LoadMap("PinWheel3");
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
            var target = GameMaps.LoadMap("PinWheel4");
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
            var target = GameMaps.LoadMap("Star2");
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
            StrategyGame.Map = GameMaps.LoadMap("Star2");
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
            StrategyGame.Map = GameMaps.LoadMap("Star2");
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
            StrategyGame.Map = GameMaps.LoadMap("Star2");

            GameEntity otherEnd;
            var next = StrategyGame.NextWormholeEnd(1, 1, 4, out otherEnd);

            next.ShouldBeNull();
            otherEnd.ShouldBeNull();
        }

        [TestMethod]
        public void TestNextWormholesWhenSomeVisible()
        {
            StrategyGame.Map = GameMaps.LoadMap("Star2");
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
