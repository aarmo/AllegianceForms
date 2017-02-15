using AllegianceForms.Engine.Rocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Shouldly;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class MineAsteroidTests
    {
        private ResourceAsteroid _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new ResourceAsteroid(new Random(), 100, 100, 0);
        }

        [TestMethod]
        public void InitialResources()
        {
            _target.AvailableResources.ShouldBe(500);
        }

        [TestMethod]
        public void MineTooManyResources()
        {
            var mined = _target.Mine(600);

            mined.ShouldBe(500);
            _target.AvailableResources.ShouldBe(0);
        }
        
        [TestMethod]
        public void MineSomeResources()
        {
            var mined = _target.Mine(100);

            mined.ShouldBe(100);
            _target.AvailableResources.ShouldBe(400);
        }

        [TestMethod]
        public void MineNoResources()
        {
            var mined = _target.Mine(0);

            mined.ShouldBe(0);
            _target.AvailableResources.ShouldBe(500);
        }
    }
}
