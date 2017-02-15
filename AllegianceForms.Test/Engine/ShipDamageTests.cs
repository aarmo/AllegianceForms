using AllegianceForms.Engine.Ships;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class ShipDamageTests
    {
        private Ship _target;
        private const int TestHealth = 100;

        [TestInitialize]
        public void Setup()
        {
            _target = new Ship(string.Empty, 10, 10, Color.White, 1, TestHealth, 1, 0);
        }

        [TestMethod]
        public void MaxHealthInit()
        {
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void ZeroDamage()
        {
            _target.Damage(0);
            _target.Health.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void SomeDamage()
        {
            _target.Damage(10);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(TestHealth-10);
            _target.Active.ShouldBe(true);
        }

        [TestMethod]
        public void OverDamage()
        {
            _target.Damage(110);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(0);
            _target.Active.ShouldBe(false);
        }

        [TestMethod]
        public void HealingWhenFull()
        {
            _target.Damage(-10);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void HealingWhenDamaged()
        {
            _target.Health = 90;
            _target.Damage(-5);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(95);
        }
    }
}
