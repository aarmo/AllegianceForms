using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class ShieldDamageTests
    {
        private Ship _target;
        private const int TestHealth = 100;

        [TestInitialize]
        public void Setup()
        {
            var game = new StrategyGame();
            game.SetupGame(GameSettings.Default());
            game.LoadData();

            _target = new Ship(game, string.Empty, 10, 10, Color.White, 1, 1, TestHealth, 1, 0);
        }

        [TestMethod]
        public void MaxShieldInitialised()
        {
            _target.MaxShield.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void ShieldDoesntOvercharge()
        {
            _target.Update();

            _target.MaxShield.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void ShieldRecharges()
        {
            _target.Shield -= 0.1f;
            _target.Update();

            _target.MaxShield.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth);
        }


        [TestMethod]
        public void ZeroDamage()
        {
            _target.Damage(0, 1);
            _target.Shield.ShouldBe(TestHealth);
            _target.Health.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void SomeDamageToShield()
        {
            _target.Damage(10, 1);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth - 10);
            _target.Health.ShouldBe(TestHealth);
            _target.Active.ShouldBe(true);
        }

        [TestMethod]
        public void OverDamageShield()
        {
            _target.Damage(110, 1);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(0);
            _target.Health.ShouldBe(90);
            _target.Active.ShouldBe(true);
        }

        [TestMethod]
        public void OverDamageAll()
        {
            _target.Damage(210, 1);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(0);
            _target.Health.ShouldBe(0);
            _target.Active.ShouldBe(false);
        }

        [TestMethod]
        public void HealingWhenFull()
        {
            _target.Damage(-10, 1);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void HealingWhenDamagedNoShield()
        {
            _target.Shield = 0;
            _target.Health = 90;
            _target.Damage(-5, 1);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(95);
        }

        [TestMethod]
        public void HealingWhenDamagedShield()
        {
            _target.Health = 90;
            _target.Damage(-5, 1);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth);
            _target.Health.ShouldBe(95);
        }
    }
}
