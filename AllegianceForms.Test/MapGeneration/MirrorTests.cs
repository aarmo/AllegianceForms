using AllegianceForms.Engine.Generation;
using AllegianceForms.Engine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;

namespace AllegianceForms.Test.MapGeneration
{
    [TestClass]
    public class MirrorTests
    {
        [TestMethod]
        public void Horizontal_Zero()
        {
            var s = new Point(0, 0);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Horizontal);
            
            m.X.ShouldBe(6);
            m.Y.ShouldBe(s.Y);
        }

        [TestMethod]
        public void Horizontal_Left()
        {
            var s = new Point(2, 0);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Horizontal);
            
            m.X.ShouldBe(4);
            m.Y.ShouldBe(s.Y);
        }

        [TestMethod]
        public void Horizontal_Mid()
        {
            var s = new Point(3, 0);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Horizontal);
            
            m.X.ShouldBe(3);
            m.Y.ShouldBe(s.Y);
        }

        [TestMethod]
        public void Horizontal_Right()
        {
            var s = new Point(4, 0);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Horizontal);

            m.X.ShouldBe(2);
            m.Y.ShouldBe(s.Y);
        }

        [TestMethod]
        public void Horizontal_FarRight()
        {
            var s = new Point(6, 0);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Horizontal);

            m.X.ShouldBe(0);
            m.Y.ShouldBe(s.Y);
        }

        [TestMethod]
        public void Vertical_Zero()
        {
            var s = new Point(0, 0);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Vertical);
            
            m.X.ShouldBe(s.X);
            m.Y.ShouldBe(6);
        }

        [TestMethod]
        public void Vertical_Top()
        {
            var s = new Point(0, 2);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Vertical);
            
            m.X.ShouldBe(s.X);
            m.Y.ShouldBe(4);
        }

        [TestMethod]
        public void Vertical_Mid()
        {
            var s = new Point(0, 3);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Vertical);
            
            m.X.ShouldBe(s.X);
            m.Y.ShouldBe(3);
        }

        [TestMethod]
        public void Vertical_Bottom()
        {
            var s = new Point(0, 4);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Vertical);

            m.X.ShouldBe(s.X);
            m.Y.ShouldBe(2);
        }

        [TestMethod]
        public void Horizontal_FarBottom()
        {
            var s = new Point(0, 6);
            var m = RandomMap.MirrorPoint(s, EMirrorType.Vertical);

            m.X.ShouldBe(s.X);
            m.Y.ShouldBe(0);
        }
    }
}
