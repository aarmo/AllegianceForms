using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Engine.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class ShieldDamageTests
    {
        private Ship _target;
        private ShipLaserWeapon _testLaser;
        private ShipMissileWeapon _testMissileLauncher;
        private const int TestHealth = 100;

        [TestInitialize]
        public void Setup()
        {
            var game = new StrategyGame();
            game.SetupGame(GameSettings.Default());
            game.LoadData();

            _target = new Ship(game, string.Empty, 10, 10, Color.White, 1, 1, TestHealth, 1, 0);
            _testLaser = new ShipLaserWeapon(null, Color.White, 1, 1, 1, 1, 1, null, PointF.Empty);
            _testMissileLauncher = new ShipMissileWeapon(null, 1, 1, 1, 1, 1, 1, 1, null, PointF.Empty, null);
        }

        [TestMethod]
        public void MaxShieldWhenInitialised()
        {
            _target.MaxShield.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void MaxHealthWhenInitialised()
        {
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(TestHealth);
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
        public void ZeroDamageDoesNothing()
        {
            _target.Damage(0, null);
            _target.Shield.ShouldBe(TestHealth);
            _target.Health.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void SomeDamageToShield()
        {
            _target.Damage(10, null);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth - 10);
            _target.Health.ShouldBe(TestHealth);
            _target.Active.ShouldBe(true);
        }

        [TestMethod]
        public void LasersDoBonusDamageToShield()
        {
            _target.Damage(10, _testLaser);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth - 13);
            _target.Health.ShouldBe(TestHealth);
            _target.Active.ShouldBe(true);
        }

        [TestMethod]
        public void MissilesDoBonusDamageToHealthWithoutShields()
        {
            _target.Shield = 0;
            _target.Damage(10, _testMissileLauncher);

            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(0);
            _target.Health.ShouldBe(TestHealth - 13);
            _target.Active.ShouldBe(true);
        }

        [TestMethod]
        public void MissilesDoBonusDamageToHealthAfterShields()
        {
            _target.Shield = 5;
            _target.Damage(10, _testMissileLauncher);

            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(0);
            _target.Health.ShouldBe(TestHealth - 6.5f);
            _target.Active.ShouldBe(true);
        }

        [TestMethod]
        public void OverDamageShieldHurtsHealth()
        {
            _target.Damage(110, null);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(0);
            _target.Health.ShouldBe(90);
            _target.Active.ShouldBe(true);
        }

        [TestMethod]
        public void OverDamageShieldWithLasersIsSame()
        {
            OverDamageShieldHurtsHealth();
        }

        [TestMethod]
        public void OverDamageShieldsAndHealthKills()
        {
            _target.Damage(210, null);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(0);
            _target.Health.ShouldBe(0);
            _target.Active.ShouldBe(false);
        }

        [TestMethod]
        public void MissilesDoBonusDamageAndKillsAfterShields()
        {
            _target.Damage(177, _testMissileLauncher);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(0);
            _target.Health.ShouldBe(0);
            _target.Active.ShouldBe(false);
        }

        [TestMethod]
        public void HealingWhenFullDoesNothing()
        {
            _target.Damage(-10, null);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth);
        }

        [TestMethod]
        public void HealingWhenDamagedAndNoShield()
        {
            _target.Shield = 0;
            _target.Health = 90;
            _target.Damage(-5, null);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Health.ShouldBe(95);
        }

        [TestMethod]
        public void HealingWhenDamagedWithShield()
        {
            _target.Health = 90;
            _target.Damage(-5, null);
            _target.MaxHealth.ShouldBe(TestHealth);
            _target.Shield.ShouldBe(TestHealth);
            _target.Health.ShouldBe(95);
        }
    }
}
