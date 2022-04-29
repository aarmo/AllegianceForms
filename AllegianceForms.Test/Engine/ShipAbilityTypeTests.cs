using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class ShipAbilityTypeTests
    {
        private StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            _game = new StrategyGame();
            _game.SetupGame(GameSettings.Default());
            _game.LoadData();
        }

        [TestMethod]
        public void CheckScoutAbility()
        {
            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Scout, 0);

            ship.Abilities.Count.ShouldBe(1);
            ship.Abilities.ShouldContainKey(EAbilityType.ScanBoost);
        }

        [TestMethod]
        public void CheckAdvScoutAbilities()
        {
            _game.TechTree[0].RecordCompleted("Advanced Scouts");
            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Scout, 0);

            ship.Abilities.Count.ShouldBe(2);
            ship.Abilities.ShouldContainKey(EAbilityType.ScanBoost);
            ship.Abilities.ShouldContainKey(EAbilityType.RapidFire);
        }

        [TestMethod]
        public void CheckHvyScoutAbilities()
        {
            _game.TechTree[0].RecordCompleted("Advanced Scouts");
            _game.TechTree[0].RecordCompleted("Heavy Scouts");
            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Scout, 0);

            ship.Abilities.Count.ShouldBe(3);
            ship.Abilities.ShouldContainKey(EAbilityType.ScanBoost);
            ship.Abilities.ShouldContainKey(EAbilityType.RapidFire);
            ship.Abilities.ShouldContainKey(EAbilityType.ShieldBoost);
        }

        [TestMethod]
        public void CheckCapShipAbilities()
        {
            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Frigate, 0);

            ship.Abilities.Count.ShouldBe(5);
            ship.Abilities.ShouldContainKey(EAbilityType.EngineBoost);
            ship.Abilities.ShouldContainKey(EAbilityType.HullRepair);
            ship.Abilities.ShouldContainKey(EAbilityType.RapidFire);
            ship.Abilities.ShouldContainKey(EAbilityType.WeaponBoost);
            ship.Abilities.ShouldContainKey(EAbilityType.ShieldBoost);
        }

        [TestMethod]
        public void CheckUnlockedShipAbilities()
        {
            _game.TechTree[0].RecordCompleted("Unlock Engine Boost");
            _game.TechTree[0].RecordCompleted("Unlock Weapon Boost");
            _game.TechTree[0].RecordCompleted("Unlock Shield Boost");
            _game.TechTree[0].RecordCompleted("Unlock Rapid Fire");
            _game.TechTree[0].RecordCompleted("Unlock Hull Repair");

            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Scout, 0);

            ship.Abilities.Count.ShouldBe(6);
            ship.Abilities.ShouldContainKey(EAbilityType.ScanBoost);
            ship.Abilities.ShouldContainKey(EAbilityType.EngineBoost);
            ship.Abilities.ShouldContainKey(EAbilityType.HullRepair);
            ship.Abilities.ShouldContainKey(EAbilityType.RapidFire);
            ship.Abilities.ShouldContainKey(EAbilityType.WeaponBoost);
            ship.Abilities.ShouldContainKey(EAbilityType.ShieldBoost);
        }

        [TestMethod]
        public void CheckScanBoost()
        {
            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Scout, 0)
            {
                ScanRange = 1
            };

            ship.UseAbility(EAbilityType.ScanBoost);
            ship.ScanRange.ShouldBe(1.5f, 0.001);
        }

        [TestMethod]
        public void CheckStealthBoost()
        {
            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.StealthFighter, 0)
            {
                Signature = 1
            };

            ship.UseAbility(EAbilityType.StealthBoost);
            ship.Signature.ShouldBe(0.666f, 0.001);
        }

        [TestMethod]
        public void CheckStealthBoostWithMaxUpgrade()
        {
            var effect = _game.TechTree[0].TechItems.First(_ => _.Name == "Ability Effect +30%");
            effect.ApplyGlobalUpgrade(_game.TechTree[0]);

            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.StealthFighter, 0)
            {
                Signature = 1
            };

            ship.UseAbility(EAbilityType.StealthBoost);
            ship.Signature.ShouldBe(0.512f, 0.001);
        }

        [TestMethod]
        public void CheckShieldBoost()
        {
            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Bomber, 0)
            {
                MaxShield = 100,
                Shield = 10
            };

            ship.UseAbility(EAbilityType.ShieldBoost);
            ship.Shield.ShouldBe(60f, 0.001);
        }

        [TestMethod]
        public void CheckEngineBoost()
        {
            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Frigate, 0)
            {
                Speed = 1
            };

            ship.UseAbility(EAbilityType.EngineBoost);
            ship.Speed.ShouldBe(1.5f, 0.001);
            Thread.Sleep(6000);

            ship.AbilityIsActive(EAbilityType.EngineBoost).ShouldBe(false);
            ship.Speed.ShouldBe(1f, 0.001);
        }

        [TestMethod]
        public void CheckEngineBoostWithMaxUpgrade()
        {
            var effect = _game.TechTree[0].TechItems.First(_ => _.Name == "Ability Effect +30%");
            effect.ApplyGlobalUpgrade(_game.TechTree[0]);

            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Frigate, 0)
            {
                Speed = 1
            };

            var expectedSpeed = 1 * 1.5f * 1.3f;

            ship.UseAbility(EAbilityType.EngineBoost);
            ship.Speed.ShouldBe(expectedSpeed, 0.001);
        }

        [TestMethod]
        public void CheckCooldownUpgrade()
        {
            var effect = _game.TechTree[0].TechItems.First(_ => _.Name == "Ability Cooldown -30%");
            effect.ApplyGlobalUpgrade(_game.TechTree[0]);

            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Frigate, 0);

            var ability = ship.Abilities[EAbilityType.WeaponBoost];
            var expectedCd = 30 * 0.7f;

            ability.CooldownDuration.ShouldBe(expectedCd, 0.001); 
            ship.UseAbility(EAbilityType.WeaponBoost);
            ability.IsActive().ShouldBeTrue();

            var cooldown = ability.AvailableAfter - DateTime.Now;
            cooldown.TotalSeconds.ShouldBeLessThanOrEqualTo(expectedCd);
        }

        [TestMethod]
        public void CheckDurationUpgrade()
        {
            var effect = _game.TechTree[0].TechItems.First(_ => _.Name == "Ability Duration +30%");
            effect.ApplyGlobalUpgrade(_game.TechTree[0]);

            var ship = new CombatShip(_game, string.Empty, 10, 10, Color.White, 1, 1, 100, 1, EShipType.Frigate, 0);

            var ability = ship.Abilities[EAbilityType.WeaponBoost];
            var expectedDuration = 5 * 1.3f;

            ability.AbilityDuration.ShouldBe(expectedDuration, 0.001);
            ship.UseAbility(EAbilityType.WeaponBoost);
            ability.IsActive().ShouldBeTrue();

            var duration = ability.InActiveAfter - DateTime.Now;
            duration.TotalSeconds.ShouldBeLessThanOrEqualTo(expectedDuration);
        }
    }
}
