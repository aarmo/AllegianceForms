using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class PathfindingTests
    {
        private GameMap _target;

        [TestInitialize]
        public void Setup()
        {
            StrategyGame.SetupGame(GameSettings.Default());
            _target = GameMaps.PinWheel(2);
            _target.SetVisibilityToTeam(1, true);
        }

        [TestMethod]
        public void TestPathBetweenStartingSectors()
        {
            var start = _target.Sectors.First(_ => _.StartingSector);
            var end = _target.Sectors.Last(_ => _.StartingSector);

            var path = _target.ShortestPath(1, start.Id, end.Id);
            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

        [TestMethod]
        public void TestPathBetweenSameSector()
        {
            var path = _target.ShortestPath(1, 0, 0);
            path.ShouldNotBeNull();
            path.Count.ShouldBe(0);
        }

        [TestMethod]
        public void TestPathToNextSector()
        {
            var path = _target.ShortestPath(1, 0, 1);
            path.ShouldNotBeNull();
            path.Count.ShouldBe(1);
        }

        [TestMethod]
        public void FindShortestPath()
        {
            var g = new PathfindingGraph<char>();
            g.AddVertex('A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } });
            g.AddVertex('B', new Dictionary<char, int>() { { 'A', 7 }, { 'F', 2 } });
            g.AddVertex('C', new Dictionary<char, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 4 } });
            g.AddVertex('D', new Dictionary<char, int>() { { 'F', 8 } });
            g.AddVertex('E', new Dictionary<char, int>() { { 'H', 1 } });
            g.AddVertex('F', new Dictionary<char, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 }, { 'H', 3 } });
            g.AddVertex('G', new Dictionary<char, int>() { { 'C', 4 }, { 'F', 9 } });
            g.AddVertex('H', new Dictionary<char, int>() { { 'E', 1 }, { 'F', 3 } });

            var path = g.ShortestPath(0, 'A', 'H');
            path.ShouldNotBeNull();
            path.Count.ShouldBe(3);
        }

    }
}
