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
            _target.AvailableResources.ShouldBe(ResourceAsteroid.MaxResources);
        }

        [TestMethod]
        public void MineTooManyResources()
        {
            var mined = _target.Mine(ResourceAsteroid.MaxResources + 100);

            mined.ShouldBe(ResourceAsteroid.MaxResources);
            _target.AvailableResources.ShouldBe(0);
        }
        
        [TestMethod]
        public void MineSomeResources()
        {
            var mined = _target.Mine(100);

            mined.ShouldBe(100);
            _target.AvailableResources.ShouldBe(ResourceAsteroid.MaxResources - 100);
        }

        [TestMethod]
        public void MineNoResources()
        {
            var mined = _target.Mine(0);

            mined.ShouldBe(0);
            _target.AvailableResources.ShouldBe(ResourceAsteroid.MaxResources);
        }

        [TestMethod]
        public void RegenerateNoResources()
        {
            _target.Mine(100);
            _target.Regenerate(0);
            _target.AvailableResources.ShouldBe(ResourceAsteroid.MaxResources - 100);
        }

        [TestMethod]
        public void RegenerateSomeResources()
        {
            _target.Mine(100);
            _target.Regenerate(10);
            _target.AvailableResources.ShouldBe(ResourceAsteroid.MaxResources - 90);
        }

        [TestMethod]
        public void RegenerateTooManyResources()
        {
            _target.Mine(100);
            _target.Regenerate(200);
            _target.AvailableResources.ShouldBe(ResourceAsteroid.MaxResources);
        }
    }
}
