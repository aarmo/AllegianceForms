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
        private StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            _game = new StrategyGame();
            _game.SetupGame(GameSettings.Default());
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_Brawl()
        {
            var target = GameMaps.Brawl(_game);
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldBeNull();
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_DoubleRing()
        {
            var target = GameMaps.LoadMap(_game, "DoubleRing2");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(1);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_DoubleRing3()
        {
            var target = GameMaps.LoadMap(_game, "DoubleRing3");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var mid = target.Sectors.Where(_ => _.StartingSector != 0).Skip(1).First();
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

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
            var target = GameMaps.LoadMap(_game, "DoubleRing4");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var mid2 = target.Sectors.Where(_ => _.StartingSector != 0).Skip(1).First();
            var mid3 = target.Sectors.Where(_ => _.StartingSector != 0).Skip(2).First();
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

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
            var target = GameMaps.LoadMap(_game, "Grid2");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_HiHigher()
        {
            var target = GameMaps.LoadMap(_game, "HiHigher2");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_HiLo()
        {
            var target = GameMaps.LoadMap(_game, "HiLo2");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(2);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_SingleRing()
        {
            var target = GameMaps.LoadMap(_game, "SingleRing2");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(2);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_SingleRing3()
        {
            var target = GameMaps.LoadMap(_game, "SingleRing3");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var mid = target.Sectors.Where(_ => _.StartingSector != 0).Skip(1).First();
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

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
            var target = GameMaps.LoadMap(_game, "SingleRing4");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var mid2 = target.Sectors.Where(_ => _.StartingSector != 0).Skip(1).First();
            var mid3 = target.Sectors.Where(_ => _.StartingSector != 0).Skip(2).First();
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

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
            var target = GameMaps.LoadMap(_game, "PinWheel2");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors_PinWheel3()
        {
            var target = GameMaps.LoadMap(_game, "PinWheel3");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var mid = target.Sectors.Where(_ => _.StartingSector != 0).Skip(1).First();
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

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
            var target = GameMaps.LoadMap(_game, "PinWheel4");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var mid2 = target.Sectors.Where(_ => _.StartingSector != 0).Skip(1).First();
            var mid3 = target.Sectors.Where(_ => _.StartingSector != 0).Skip(2).First();
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

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
            var target = GameMaps.LoadMap(_game, "Star2");
            target.SetVisibilityToTeam(1, true);

            var start = target.Sectors.First(_ => _.StartingSector != 0);
            var end = target.Sectors.Last(_ => _.StartingSector != 0);

            var path = target.ShortestPath(1, start.Id, end.Id);

            path.ShouldNotBeNull();
            path.Count.ShouldBe(4);
        }

        [TestMethod]
        public void TestNextWormholeBetweenStartingSectors_Star_Step1()
        {
            _game.Map = GameMaps.LoadMap(_game, "Star2");
            _game.Map.SetVisibilityToTeam(1, true);

            GameEntity otherEnd;
            var next = _game.NextWormholeEnd(1, 0, 4, out otherEnd);

            next.ShouldNotBeNull();
            otherEnd.ShouldNotBeNull();

            next.SectorId.ShouldBe(0);
            otherEnd.SectorId.ShouldBe(1);
        }

        [TestMethod]
        public void TestNextWormholeBetweenStartingSectors_Star_Step2()
        {
            _game.Map = GameMaps.LoadMap(_game, "Star2");
            _game.Map.SetVisibilityToTeam(1, true);

            GameEntity otherEnd;
            var next = _game.NextWormholeEnd(1, 1, 4, out otherEnd);

            next.ShouldNotBeNull();
            otherEnd.ShouldNotBeNull();

            next.SectorId.ShouldBe(1);
            otherEnd.SectorId.ShouldBe(2);
        }

        [TestMethod]
        public void TestNextWormholesWhenNotVisible()
        {
            _game.Map = GameMaps.LoadMap(_game, "Star2");

            GameEntity otherEnd;
            var next = _game.NextWormholeEnd(1, 1, 4, out otherEnd);

            next.ShouldBeNull();
            otherEnd.ShouldBeNull();
        }

        [TestMethod]
        public void TestNextWormholesWhenSomeVisible()
        {
            _game.Map = GameMaps.LoadMap(_game, "Star2");
            _game.Map.Sectors[0].SetVisibleToTeam(0, true);
            _game.Map.Sectors[1].SetVisibleToTeam(0, true);
            _game.Map.Sectors[2].SetVisibleToTeam(0, true);

            GameEntity otherEnd;
            var next = _game.NextWormholeEnd(1, 0, 2, out otherEnd);

            next.ShouldNotBeNull();
            otherEnd.ShouldNotBeNull();

            next.SectorId.ShouldBe(0);
            otherEnd.SectorId.ShouldBe(1);
        }
    }
}
