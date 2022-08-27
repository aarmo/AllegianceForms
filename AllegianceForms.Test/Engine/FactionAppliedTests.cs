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
        private StrategyGame _game;
        private GameSettings _settings;

        [TestInitialize]
        public void Setup()
        {
            _settings = GameSettings.Default();
            _game = new StrategyGame();

            _game.SetupGame(_settings);
            _game.LoadData();
            _game.Map = GameMaps.LoadMap(_game, _settings.MapName);
        }

        [TestMethod]
        public void CheckFactionResearchTime()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.ResearchTime = multiplier;
            _game.InitialiseGame(false);

            for (var i = 0; i < _game.TechTree[0].TechItems.Count; i++)
            {
                var item1 = _game.TechTree[0].TechItems[i];
                var item2 = _game.TechTree[1].TechItems[i];

                item2.DurationTicks.ShouldBe((int)(multiplier * item1.DurationTicks));
            }
        }

        [TestMethod]
        public void CheckFactionResearchCost()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.ResearchCost = multiplier;
            _game.InitialiseGame(false);

            for (var i = 0; i < _game.TechTree[0].TechItems.Count; i++)
            {
                var item1 = _game.TechTree[0].TechItems[i];
                var item2 = _game.TechTree[1].TechItems[i];

                item2.Cost.ShouldBe((int)(multiplier * item1.Cost));
            }
        }

        [TestMethod]
        public void CheckFactionSpeed()
        {
            var multiplier = 0.5f;
            var faction = _game.Faction[1];
            var race = faction.Race;
            var raceSpeed = _game.RaceSettings[race].SpeedMultiplier;
            faction.Bonuses.Speed = multiplier;

            _game.InitialiseGame(false);


            var builder1 = _game.Ships.CreateBuilderShip(EBaseType.Outpost, 1, Color.White, 1);
            var builder2 = _game.Ships.CreateBuilderShip(EBaseType.Outpost, 2, Color.White, 1);
            builder2.Speed.ShouldBe(multiplier * builder1.Speed * raceSpeed);

            var combat1 = _game.Ships.CreateCombatShip(1, Color.White, 1);
            var combat2 = _game.Ships.CreateCombatShip(2, Color.White, 1);
            combat2.Speed.ShouldBe(multiplier * combat1.Speed * raceSpeed);

            var miner1 = _game.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = _game.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.Speed.ShouldBe(multiplier * miner1.Speed * raceSpeed);

            var tower1 = _game.Ships.CreateTowerShip(EShipType.Tower, 1, Color.White, 1);
            var tower2 = _game.Ships.CreateTowerShip(EShipType.Tower, 2, Color.White, 1);
            tower2.Speed.ShouldBe(multiplier * tower1.Speed * raceSpeed);
        }

        [TestMethod]
        public void CheckFactionHealth()
        {
            var multiplier = 0.5f;
            var faction = _game.Faction[1];
            var race = faction.Race;
            var raceHealth = _game.RaceSettings[race].HullMultiplier;
            faction.Bonuses.Health = multiplier;

            _game.InitialiseGame(false);

            var builder1 = _game.Ships.CreateBuilderShip(EBaseType.Outpost, 1, Color.White, 1);
            var builder2 = _game.Ships.CreateBuilderShip(EBaseType.Outpost, 2, Color.White, 1);
            builder2.Health.ShouldBe(multiplier * builder1.Health * raceHealth);

            var combat1 = _game.Ships.CreateCombatShip(1, Color.White, 1);
            var combat2 = _game.Ships.CreateCombatShip(2, Color.White, 1);
            combat2.Health.ShouldBe(multiplier * combat1.Health * raceHealth);

            var miner1 = _game.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = _game.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.Health.ShouldBe(multiplier * miner1.Health * raceHealth);

            var tower1 = _game.Ships.CreateTowerShip(EShipType.Tower, 1, Color.White, 1);
            var tower2 = _game.Ships.CreateTowerShip(EShipType.Tower, 2, Color.White, 1);
            tower2.Health.ShouldBe(multiplier * tower1.Health * raceHealth);
        }

        [TestMethod]
        public void CheckFactionScanRange()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.ScanRange = multiplier;
            _game.InitialiseGame(false);

            var builder1 = _game.Ships.CreateBuilderShip(EBaseType.Outpost, 1, Color.White, 1);
            var builder2 = _game.Ships.CreateBuilderShip(EBaseType.Outpost, 2, Color.White, 1);
            builder2.ScanRange.ShouldBe(multiplier * builder1.ScanRange);

            var combat1 = _game.Ships.CreateCombatShip(1, Color.White, 1);
            var combat2 = _game.Ships.CreateCombatShip(2, Color.White, 1);
            combat2.ScanRange.ShouldBe(multiplier * combat1.ScanRange);

            var miner1 = _game.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = _game.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.ScanRange.ShouldBe(multiplier * miner1.ScanRange);

            var tower1 = _game.Ships.CreateTowerShip(EShipType.Tower, 1, Color.White, 1);
            var tower2 = _game.Ships.CreateTowerShip(EShipType.Tower, 2, Color.White, 1);
            tower2.ScanRange.ShouldBe(multiplier * tower1.ScanRange);
        }

        [TestMethod]
        public void CheckFactionSignature()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.Signature = multiplier;
            _game.InitialiseGame(false);

            var builder1 = _game.Ships.CreateBuilderShip(EBaseType.Outpost, 1, Color.White, 1);
            var builder2 = _game.Ships.CreateBuilderShip(EBaseType.Outpost, 2, Color.White, 1);
            builder2.Signature.ShouldBe(multiplier * builder1.Signature);

            var combat1 = _game.Ships.CreateCombatShip(1, Color.White, 1);
            var combat2 = _game.Ships.CreateCombatShip(2, Color.White, 1);
            combat2.Signature.ShouldBe(multiplier * combat1.Signature);

            var miner1 = _game.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = _game.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.Signature.ShouldBe(multiplier * miner1.Signature);

            var tower1 = _game.Ships.CreateTowerShip(EShipType.Tower, 1, Color.White, 1);
            var tower2 = _game.Ships.CreateTowerShip(EShipType.Tower, 2, Color.White, 1);
            tower2.Signature.ShouldBe(multiplier * tower1.Signature);
        }

        [TestMethod]
        public void CheckFactionFireRate()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.FireRate = multiplier;
            _game.InitialiseGame(false);

            var tower1 = _game.Ships.CreateTowerShip(EShipType.MissileTower, 1, Color.White, 1);
            var tower2 = _game.Ships.CreateTowerShip(EShipType.MissileTower, 2, Color.White, 1);
            var tm1 = tower1.Weapons[0];
            var tm2 = tower2.Weapons[0];
            tm1.ShootingDelayTicks.ShouldBe((int)(multiplier * tm2.ShootingDelayTicks));
            tm1.ShootingTicks.ShouldBe(tm2.ShootingTicks);

            var fig1 = _game.Ships.CreateCombatShip(Keys.F, 1, Color.White, 1);
            var fig2 = _game.Ships.CreateCombatShip(Keys.F, 2, Color.White, 1);
            var fm1 = fig1.Weapons[0];
            var fm2 = fig2.Weapons[0];
            fm1.ShootingDelayTicks.ShouldBe((int)(multiplier * fm2.ShootingDelayTicks));
            fm1.ShootingTicks.ShouldBe(fm2.ShootingTicks);

            var nan1 = _game.Ships.CreateCombatShip(Keys.S, 1, Color.White, 1);
            var nan2 = _game.Ships.CreateCombatShip(Keys.S, 2, Color.White, 1);
            var nm1 = fig1.Weapons[0];
            var nm2 = fig2.Weapons[0];
            nm1.ShootingDelayTicks.ShouldBe((int)(multiplier * nm2.ShootingDelayTicks));
            nm1.ShootingTicks.ShouldBe(nm2.ShootingTicks);
        }

        [TestMethod]
        public void CheckFactionMissileTracking()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.MissileTracking = multiplier;
            _game.InitialiseGame(false);

            var tower1 = _game.Ships.CreateTowerShip(EShipType.MissileTower, 1, Color.White, 1);
            var tower2 = _game.Ships.CreateTowerShip(EShipType.MissileTower, 2, Color.White, 1);
            var tm1 = tower1.Weapons[0] as ShipMissileWeapon;
            var tm2 = tower2.Weapons[0] as ShipMissileWeapon;
            tm2.Tracking.ShouldBe(multiplier * tm1.Tracking);

            var fig1 = _game.Ships.CreateCombatShip(Keys.F, 1, Color.White, 1);
            var fig2 = _game.Ships.CreateCombatShip(Keys.F, 2, Color.White, 1);
            var fm1 = fig1.Weapons[1] as ShipMissileWeapon;
            var fm2 = fig2.Weapons[1] as ShipMissileWeapon;
            fm2.Tracking.ShouldBe(multiplier * fm1.Tracking);
        }

        [TestMethod]
        public void CheckFactionMissileSpeed()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.MissileSpeed = multiplier;
            _game.InitialiseGame(false);

            var tower1 = _game.Ships.CreateTowerShip(EShipType.MissileTower, 1, Color.White, 1);
            var tower2 = _game.Ships.CreateTowerShip(EShipType.MissileTower, 2, Color.White, 1);
            var m1 = tower1.Weapons[0] as ShipMissileWeapon;
            var m2 = tower2.Weapons[0] as ShipMissileWeapon;
            m2.Speed.ShouldBe(multiplier * m1.Speed);
            
            var fig1 = _game.Ships.CreateCombatShip(Keys.F, 1, Color.White, 1);
            var fig2 = _game.Ships.CreateCombatShip(Keys.F, 2, Color.White, 1);
            var fm1 = fig1.Weapons[1] as ShipMissileWeapon;
            var fm2 = fig2.Weapons[1] as ShipMissileWeapon;
            fm2.Speed.ShouldBe(multiplier * fm1.Speed);
        }

        [TestMethod]
        public void CheckFactionMiningCapacity()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.MiningCapacity = multiplier;
            _game.InitialiseGame(false);
            
            var miner1 = _game.Ships.CreateMinerShip(1, Color.White, 1);
            var miner2 = _game.Ships.CreateMinerShip(2, Color.White, 1);
            miner2.MaxResourceCapacity.ShouldBe((int)(multiplier * miner1.MaxResourceCapacity));
        }

        [TestMethod]
        public void CheckFactionMiningEfficiency()
        {
            var multiplier = 0.5f;
            _game.Faction[1].Bonuses.MiningEfficiency = multiplier;
            _game.InitialiseGame(false);

            _game.AddResources(1, 100, false);
            _game.AddResources(2, 100, false);

            var creds1 = _game.Credits[0];
            var creds2 = _game.Credits[1];

            creds2.ShouldBe((int)(multiplier * creds1));
        }
    }
}
