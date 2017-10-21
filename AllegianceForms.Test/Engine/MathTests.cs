using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class MathTests
    {
        [TestMethod]
        public void CheckDistance()
        {
            var p1 = new Point(2, 2);
            var p2 = new Point(1, 1);

            var d = Utils.DistanceBetween(p1, p2);
            d.ShouldBe(1.414, 0.01);
        }

        [TestMethod]
        public void CheckAngleSamePoint()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(1, 1);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(0, 0.01);
        }

        [TestMethod]
        public void CheckAngle0()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(2, 1);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(0, 0.01);
        }

        [TestMethod]
        public void CheckMoveAngle0()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(2, 1);

            var t = Utils.GetNewPoint(p1, 1, 0);
            t.ShouldBe(p2);
        }

        [TestMethod]
        public void CheckAngle45()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(2, 2);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(45, 0.01);
        }

        [TestMethod]
        public void CheckMoveAngle45()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(2, 2);

            var t = Utils.GetNewPoint(p1, 1.414f, 45);
            t.X.ShouldBe(p2.X, 0.01);
            t.Y.ShouldBe(p2.Y, 0.01);
        }

        [TestMethod]
        public void CheckAngle90()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(1, 2);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(90, 0.01);
        }

        [TestMethod]
        public void CheckMoveAngle90()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(1, 2);

            var t = Utils.GetNewPoint(p1, 1, 90);
            t.X.ShouldBe(p2.X, 0.01);
            t.Y.ShouldBe(p2.Y, 0.01);
        }

        [TestMethod]
        public void CheckAngle135()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(0, 2);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(135, 0.01);
        }

        [TestMethod]
        public void CheckMoveAngle135()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(0, 2);

            var t = Utils.GetNewPoint(p1, 1.414f, 135);
            t.X.ShouldBe(p2.X, 0.01);
            t.Y.ShouldBe(p2.Y, 0.01);
        }

        [TestMethod]
        public void CheckAngle180()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(0, 1);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(180, 0.01);
        }

        [TestMethod]
        public void CheckMoveAngle180()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(0, 1);

            var t = Utils.GetNewPoint(p1, 1, 180);
            t.X.ShouldBe(p2.X, 0.01);
            t.Y.ShouldBe(p2.Y, 0.01);
        }

        [TestMethod]
        public void CheckAngleNeg45()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(2, 0);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(-45, 0.01);
        }

        [TestMethod]
        public void CheckMoveAngleNeg45()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(2, 0);

            var t = Utils.GetNewPoint(p1, 1.414f, -45);
            t.X.ShouldBe(p2.X, 0.01);
            t.Y.ShouldBe(p2.Y, 0.01);
        }

        [TestMethod]
        public void CheckAngleNeg90()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(1, 0);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(-90, 0.01);
        }

        [TestMethod]
        public void CheckMoveAngleNeg90()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(1, 0);

            var t = Utils.GetNewPoint(p1, 1, -90);
            t.X.ShouldBe(p2.X, 0.01);
            t.Y.ShouldBe(p2.Y, 0.01);
        }

        [TestMethod]
        public void CheckAngleNeg135()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(0, 0);

            var d = Utils.AngleBetweenPoints(p1, p2);
            d.ShouldBe(-135, 0.01);
        }

        [TestMethod]
        public void CheckMoveAngleNeg135()
        {
            var p1 = new PointF(1, 1);
            var p2 = new PointF(0, 0);

            var t = Utils.GetNewPoint(p1, 1.414f, -135);
            t.X.ShouldBe(p2.X, 0.01);
            t.Y.ShouldBe(p2.Y, 0.01);
        }
    }
}
