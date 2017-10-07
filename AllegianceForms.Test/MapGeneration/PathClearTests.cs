using AllegianceForms.Engine.Generation;
using AllegianceForms.Engine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;

namespace AllegianceForms.Test.MapGeneration
{
    [TestClass]
    public class PathClearTests
    {
        private SimpleGameMap _target;
        [TestInitialize]
        public void Setup()
        {
            _target = new SimpleGameMap("Test");
        }

        [TestMethod]
        public void SamePoint()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(3, 3), new Point(3, 4));
            clear.ShouldBe(true);
        }

        [TestMethod]
        public void OnHorizontalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(0, 3), new Point(6, 3));
            clear.ShouldBe(false);
        }

        [TestMethod]
        public void JustOffHorizontalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(0, 3), new Point(3, 4));
            clear.ShouldBe(true);
        }

        [TestMethod]
        public void JustOffHorizontalLine2()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(1, 1)));

            var clear = RandomMap.IsPathClear(_target, new Point(0, 0), new Point(3, 1));
            clear.ShouldBe(true);
        }

        [TestMethod]
        public void JustOnHorizontalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(1, 0)));

            var clear = RandomMap.IsPathClear(_target, new Point(0, 0), new Point(3, 1));
            clear.ShouldBe(false);
        }

        [TestMethod]
        public void JustOnHorizontalLine2()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(2, 1)));

            var clear = RandomMap.IsPathClear(_target, new Point(0, 0), new Point(3, 1));
            clear.ShouldBe(false);
        }

        [TestMethod]
        public void OnVerticalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(3, 0), new Point(3, 6));
            clear.ShouldBe(false);
        }

        [TestMethod]
        public void JustOffVerticalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(3, 0), new Point(4, 3));
            clear.ShouldBe(true);
        }

        [TestMethod]
        public void OnDiagonalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(0, 0), new Point(6, 6));
            clear.ShouldBe(false);
        }

        [TestMethod]
        public void BeforeHorizonalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(0, 3), new Point(2, 3));
            clear.ShouldBe(true);
        }

        [TestMethod]
        public void AfterHorizonalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(4, 3), new Point(6, 3));
            clear.ShouldBe(true);
        }

        [TestMethod]
        public void BeforeVerticalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(3, 0), new Point(3, 2));
            clear.ShouldBe(true);
        }

        [TestMethod]
        public void AfterVerticalLine()
        {
            _target.Sectors.Add(new SimpleMapSector(0, new Point(3, 3)));

            var clear = RandomMap.IsPathClear(_target, new Point(3, 4), new Point(3, 6));
            clear.ShouldBe(true);
        }
    }
}
