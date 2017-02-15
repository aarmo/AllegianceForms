using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class PositionTests
    {
        GameEntity _target;
        RectangleF _bounds;

        [TestInitialize]
        public void TestSetup()
        {
            _target = new GameEntity(string.Empty, 12, 14, 0);
            _bounds = _target.Bounds;
        }

        [TestMethod]
        public void InitialBoundTop()
        {
            _bounds.Top.ShouldBe(_target.Top);
        }

        [TestMethod]
        public void InitialBoundLeft()
        {
            _bounds.Left.ShouldBe(_target.Left);
        }

        [TestMethod]
        public void InitialBoundWidth()
        {
            _bounds.Width.ShouldBe(12);
        }

        [TestMethod]
        public void InitialBoundHeight()
        {
            _bounds.Height.ShouldBe(14);
        }

        [TestMethod]
        public void InitialCenterX()
        {
            _target.CenterX.ShouldBe(_bounds.Width / 2);
        }

        [TestMethod]
        public void InitialCenterY()
        {
            _target.CenterY.ShouldBe(_bounds.Height / 2);
        }

        [TestMethod]
        public void InitialTop()
        {
            _target.Top.ShouldBe(0);
        }

        [TestMethod]
        public void InitialLeft()
        {
            _target.Left.ShouldBe(0);
        }

        [TestMethod]
        public void TestMovementX()
        {
            _target.CenterX += 10;
            
            _target.Left.ShouldBe(10);
            _target.CenterX.ShouldBe(10 + _bounds.Width / 2);

            _target.Top.ShouldBe(0);
            _target.CenterY.ShouldBe(_bounds.Height / 2);
        }

        [TestMethod]
        public void TestMovementY()
        {
            _target.CenterY += 10;

            _target.Left.ShouldBe(0);
            _target.CenterX.ShouldBe(_bounds.Width / 2);

            _target.Top.ShouldBe(10);
            _target.CenterY.ShouldBe(10 + _bounds.Height / 2);
        }
    }
}
