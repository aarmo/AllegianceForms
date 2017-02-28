using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using AllegianceForms.Engine.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class FactionAppliedTests
    {
        private GameSettings _settings;

        [TestInitialize]
        public void Setup()
        {
            _settings = GameSettings.Default();
            StrategyGame.SetupGame(_settings);
            StrategyGame.LoadData();
            StrategyGame.Map = GameMaps.LoadMap(_settings.MapName);
        }

        [TestMethod]
        public void CheckFactionResearchTime()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.ResearchTime = multiplier;
            StrategyGame.InitialiseGame(false);

            for (var i = 0; i < StrategyGame.TechTree[0].TechItems.Count; i++)
            {
                var item1 = StrategyGame.TechTree[0].TechItems[i];
                var item2 = StrategyGame.TechTree[1].TechItems[i];

                item2.DurationSec.ShouldBe((int)(multiplier * item1.DurationSec));
            }
        }

        [TestMethod]
        public void CheckFactionResearchCost()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.ResearchCost = multiplier;
            StrategyGame.InitialiseGame(false);

            for (var i = 0; i < StrategyGame.TechTree[0].TechItems.Count; i++)
            {
                var item1 = StrategyGame.TechTree[0].TechItems[i];
                var item2 = StrategyGame.TechTree[1].TechItems[i];

                item2.Cost.ShouldBe((int)(multiplier * item1.Cost));
            }
        }

        [TestMethod]
        public void CheckFactionSpeed()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.Speed = multiplier;
            StrategyGame.InitialiseGame(false);

            var builder1 = StrategyGame.Ships.CreateBuilderShip(EBaseType.Outpost, 1, Color.White, 1);
            var builder2 = StrategyGame.Ships.CreateBuilderShip(EBaseType.Outpost, 2, Color.White, 1);
            builder2.Speed.ShouldBe(multiplier * builder1.Speed);

            var combat1 = StrategyGame.Ships.CreateCombatShip(1, Color.White, 1);
            var combat2 = StrategyGame.Ships.CreateCombatShip(2, Color.White, 1);
            combat2.Speed.ShouldBe(multiplier * combat1.Speed);

            var miner1 = StrategyGame.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = StrategyGame.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.Speed.ShouldBe(multiplier * miner1.Speed);

            var tower1 = StrategyGame.Ships.CreateTowerShip(EShipType.Tower, 1, Color.White, 1);
            var tower2 = StrategyGame.Ships.CreateTowerShip(EShipType.Tower, 2, Color.White, 1);
            tower2.Speed.ShouldBe(multiplier * tower1.Speed);
        }

        [TestMethod]
        public void CheckFactionHealth()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.Health = multiplier;
            StrategyGame.InitialiseGame(false);

            var builder1 = StrategyGame.Ships.CreateBuilderShip(EBaseType.Outpost, 1, Color.White, 1);
            var builder2 = StrategyGame.Ships.CreateBuilderShip(EBaseType.Outpost, 2, Color.White, 1);
            builder2.Health.ShouldBe(multiplier * builder1.Health);

            var combat1 = StrategyGame.Ships.CreateCombatShip(1, Color.White, 1);
            var combat2 = StrategyGame.Ships.CreateCombatShip(2, Color.White, 1);
            combat2.Health.ShouldBe(multiplier * combat1.Health);

            var miner1 = StrategyGame.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = StrategyGame.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.Health.ShouldBe(multiplier * miner1.Health);

            var tower1 = StrategyGame.Ships.CreateTowerShip(EShipType.Tower, 1, Color.White, 1);
            var tower2 = StrategyGame.Ships.CreateTowerShip(EShipType.Tower, 2, Color.White, 1);
            tower2.Health.ShouldBe(multiplier * tower1.Health);
        }

        [TestMethod]
        public void CheckFactionScanRange()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.ScanRange = multiplier;
            StrategyGame.InitialiseGame(false);

            var builder1 = StrategyGame.Ships.CreateBuilderShip(EBaseType.Outpost, 1, Color.White, 1);
            var builder2 = StrategyGame.Ships.CreateBuilderShip(EBaseType.Outpost, 2, Color.White, 1);
            builder2.ScanRange.ShouldBe(multiplier * builder1.ScanRange);

            var combat1 = StrategyGame.Ships.CreateCombatShip(1, Color.White, 1);
            var combat2 = StrategyGame.Ships.CreateCombatShip(2, Color.White, 1);
            combat2.ScanRange.ShouldBe(multiplier * combat1.ScanRange);

            var miner1 = StrategyGame.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = StrategyGame.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.ScanRange.ShouldBe(multiplier * miner1.ScanRange);

            var tower1 = StrategyGame.Ships.CreateTowerShip(EShipType.Tower, 1, Color.White, 1);
            var tower2 = StrategyGame.Ships.CreateTowerShip(EShipType.Tower, 2, Color.White, 1);
            tower2.ScanRange.ShouldBe(multiplier * tower1.ScanRange);
        }

        [TestMethod]
        public void CheckFactionSignature()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.Signature = multiplier;
            StrategyGame.InitialiseGame(false);

            var builder1 = StrategyGame.Ships.CreateBuilderShip(EBaseType.Outpost, 1, Color.White, 1);
            var builder2 = StrategyGame.Ships.CreateBuilderShip(EBaseType.Outpost, 2, Color.White, 1);
            builder2.Signature.ShouldBe(multiplier * builder1.Signature);

            var combat1 = StrategyGame.Ships.CreateCombatShip(1, Color.White, 1);
            var combat2 = StrategyGame.Ships.CreateCombatShip(2, Color.White, 1);
            combat2.Signature.ShouldBe(multiplier * combat1.Signature);

            var miner1 = StrategyGame.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = StrategyGame.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.Signature.ShouldBe(multiplier * miner1.Signature);

            var tower1 = StrategyGame.Ships.CreateTowerShip(EShipType.Tower, 1, Color.White, 1);
            var tower2 = StrategyGame.Ships.CreateTowerShip(EShipType.Tower, 2, Color.White, 1);
            tower2.Signature.ShouldBe(multiplier * tower1.Signature);
        }

        [TestMethod]
        public void CheckFactionFireRate()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.FireRate = multiplier;
            StrategyGame.InitialiseGame(false);

            var tower1 = StrategyGame.Ships.CreateTowerShip(EShipType.MissileTower, 1, Color.White, 1);
            var tower2 = StrategyGame.Ships.CreateTowerShip(EShipType.MissileTower, 2, Color.White, 1);
            var tm1 = tower1.Weapons[0];
            var tm2 = tower2.Weapons[0];
            tm1.ShootingDelay.TotalMilliseconds.ShouldBe(multiplier * tm2.ShootingDelay.TotalMilliseconds);
            tm1.ShootingDuration.TotalMilliseconds.ShouldBe(tm2.ShootingDuration.TotalMilliseconds);

            var fig1 = StrategyGame.Ships.CreateCombatShip(Keys.F, 1, Color.White, 1);
            var fig2 = StrategyGame.Ships.CreateCombatShip(Keys.F, 2, Color.White, 1);
            var fm1 = fig1.Weapons[0];
            var fm2 = fig2.Weapons[0];
            fm1.ShootingDelay.TotalMilliseconds.ShouldBe(multiplier * fm2.ShootingDelay.TotalMilliseconds);
            fm1.ShootingDuration.TotalMilliseconds.ShouldBe(fm2.ShootingDuration.TotalMilliseconds);

            var nan1 = StrategyGame.Ships.CreateCombatShip(Keys.S, 1, Color.White, 1);
            var nan2 = StrategyGame.Ships.CreateCombatShip(Keys.S, 2, Color.White, 1);
            var nm1 = fig1.Weapons[0];
            var nm2 = fig2.Weapons[0];
            nm1.ShootingDelay.TotalMilliseconds.ShouldBe(multiplier * nm2.ShootingDelay.TotalMilliseconds);
            nm1.ShootingDuration.TotalMilliseconds.ShouldBe(nm2.ShootingDuration.TotalMilliseconds);
        }

        [TestMethod]
        public void CheckFactionMissileTracking()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.MissileTracking = multiplier;
            StrategyGame.InitialiseGame(false);

            var tower1 = StrategyGame.Ships.CreateTowerShip(EShipType.MissileTower, 1, Color.White, 1);
            var tower2 = StrategyGame.Ships.CreateTowerShip(EShipType.MissileTower, 2, Color.White, 1);
            var tm1 = tower1.Weapons[0] as ShipMissileWeapon;
            var tm2 = tower2.Weapons[0] as ShipMissileWeapon;
            tm2.Tracking.ShouldBe(multiplier * tm1.Tracking);

            var fig1 = StrategyGame.Ships.CreateCombatShip(Keys.F, 1, Color.White, 1);
            var fig2 = StrategyGame.Ships.CreateCombatShip(Keys.F, 2, Color.White, 1);
            var fm1 = fig1.Weapons[1] as ShipMissileWeapon;
            var fm2 = fig2.Weapons[1] as ShipMissileWeapon;
            fm2.Tracking.ShouldBe(multiplier * fm1.Tracking);
        }

        [TestMethod]
        public void CheckFactionMissileSpeed()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.MissileSpeed = multiplier;
            StrategyGame.InitialiseGame(false);

            var tower1 = StrategyGame.Ships.CreateTowerShip(EShipType.MissileTower, 1, Color.White, 1);
            var tower2 = StrategyGame.Ships.CreateTowerShip(EShipType.MissileTower, 2, Color.White, 1);
            var m1 = tower1.Weapons[0] as ShipMissileWeapon;
            var m2 = tower2.Weapons[0] as ShipMissileWeapon;
            m2.Speed.ShouldBe(multiplier * m1.Speed);
            
            var fig1 = StrategyGame.Ships.CreateCombatShip(Keys.F, 1, Color.White, 1);
            var fig2 = StrategyGame.Ships.CreateCombatShip(Keys.F, 2, Color.White, 1);
            var fm1 = fig1.Weapons[1] as ShipMissileWeapon;
            var fm2 = fig2.Weapons[1] as ShipMissileWeapon;
            fm2.Speed.ShouldBe(multiplier * fm1.Speed);
        }

        [TestMethod]
        public void CheckFactionMiningCapacity()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.MiningCapacity = multiplier;
            StrategyGame.InitialiseGame(false);
            
            var miner1 = StrategyGame.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = StrategyGame.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.MaxResourceCapacity.ShouldBe((int)(multiplier * miner1.MaxResourceCapacity));
        }

        [TestMethod]
        public void CheckFactionMiningEfficiency()
        {
            var multiplier = 0.5f;
            StrategyGame.Faction[1].Bonuses.MiningEfficiency = multiplier;
            StrategyGame.InitialiseGame(false);

            StrategyGame.AddResources(1, 100, false);
            StrategyGame.AddResources(2, 100, false);

            var creds1 = StrategyGame.Credits[0];
            var creds2 = StrategyGame.Credits[1];

            creds2.ShouldBe((int)(multiplier * creds1));
        }
    }
}
