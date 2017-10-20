using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using AllegianceForms.Engine.Rocks;
using AllegianceForms.Engine.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class SettingsAppliedTests
    {
        private GameSettings _settings;
        private StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            _settings = GameSettings.Default();
            _settings.TeamFactions[1] = AllegianceForms.Engine.Factions.Faction.Default();
            _game = new StrategyGame();
        }
        
        private void LoadSettings()
        {
            _game.SetupGame(_settings);
            _game.LoadData();
            _game.Map = GameMaps.LoadMap(_game, _settings.MapName);
            _game.InitialiseGame(false);
        }

        [TestMethod]
        public void CheckSettingsNumPilots()
        {
            _settings.NumPilots = 8;
            LoadSettings();

            _game.DockedPilots[0].ShouldBe(8);
        }

        [TestMethod]
        public void CheckSettingWormholeVisible()
        {
            _settings.WormholesVisible = true;
            LoadSettings();
            foreach (var w in _game.Map.Wormholes)
            {
                w.Sector1.VisibleToTeam[0].ShouldBe(true);
                w.Sector1.VisibleToTeam[1].ShouldBe(true);

                w.Sector2.VisibleToTeam[0].ShouldBe(true);
                w.Sector2.VisibleToTeam[1].ShouldBe(true);
            }
        }

        [TestMethod]
        public void CheckSettingsWormholeSignature()
        {
            var value = 0.5f;
            _settings.WormholesSignatureMultiplier = value;
            LoadSettings();
            foreach (var w in _game.Map.Wormholes)
            {
                w.End1.Signature.ShouldBe(value * value);
                w.End2.Signature.ShouldBe(value * value);
            }
        }

        [TestMethod]
        public void CheckSettingsShipSpeed_Scout()
        {
            var value = 0.5f;
            _settings.ShipSpeedMultiplier[EShipType.Scout] = value;
            LoadSettings();
            
            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Scout);
            if (tech == null) return;
            var scout = _game.Ships.CreateShip("Scout", 1, Color.White, 1);

            scout.Speed.ShouldBe(tech.Speed * value);
        }

        [TestMethod]
        public void CheckSettingShipHealth_Scout()
        {
            var value = 0.5f;
            _settings.ShipHealthMultiplier[EShipType.Scout] = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Scout);
            if (tech == null) return;
            var scout = _game.Ships.CreateShip("Scout", 1, Color.White, 1);

            scout.Health.ShouldBe(tech.Health * value);
        }

        [TestMethod]
        public void CheckSettingShipSignature_Scout()
        {
            var value = 0.5f;
            _settings.ShipSignatureMultiplier[EShipType.Scout] = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Scout);
            if (tech == null) return;
            var scout = _game.Ships.CreateShip("Scout", 1, Color.White, 1);

            scout.Signature.ShouldBe(tech.Signature * value);
        }

        [TestMethod]
        public void CheckSettingStationHealth_Outpost()
        {
            var value = 0.5f;
            _settings.StationHealthMultiplier[EBaseType.Outpost] = value;
            LoadSettings();

            var tech = _game.Bases.Bases.FirstOrDefault(_ => _.Type == EBaseType.Outpost);
            if (tech == null) return;
            var outpost = _game.Bases.CreateBase(EBaseType.Outpost, 1, Color.White, 1);

            outpost.Health.ShouldBe(tech.Health * value);
        }

        [TestMethod]
        public void CheckSettingStationSignature_Outpost()
        {
            var value = 0.5f;
            _settings.StationSignatureMultiplier[EBaseType.Outpost] = value;
            LoadSettings();

            var tech = _game.Bases.Bases.FirstOrDefault(_ => _.Type == EBaseType.Outpost);
            if (tech == null) return;
            var outpost = _game.Bases.CreateBase(EBaseType.Outpost, 1, Color.White, 1);

            outpost.Signature.ShouldBe(tech.Signature * value);
        }

        [TestMethod]
        public void CheckSettingResourcesStarting()
        {
            var value = 0.5f;
            _settings.ResourcesStartingMultiplier = value;
            LoadSettings();

            _game.Credits[0].ShouldBe((int)(StrategyGame.ResourcesInitial * StrategyGame.BaseConversionRate * value));
            _game.Credits[1].ShouldBe((int)(StrategyGame.ResourcesInitial * StrategyGame.BaseConversionRate * value));
        }

        [TestMethod]
        public void CheckSettingsResourcesPerRock()
        {
            var value = 0.5f;
            _settings.ResourcesPerRockMultiplier = value;
            LoadSettings();

            foreach (var r in _game.ResourceAsteroids)
            {
                r.AvailableResources.ShouldBe((int)(ResourceAsteroid.MaxResources * value));
            }
        }

        [TestMethod]
        public void CheckSettingsResourceConversion()
        {
            var value = 0.5f;
            _settings.ResourceConversionRateMultiplier = value;
            LoadSettings();

            _game.Credits[0].ShouldBe((int)(StrategyGame.ResourcesInitial * StrategyGame.BaseConversionRate * value));
            _game.Credits[1].ShouldBe((int)(StrategyGame.ResourcesInitial * StrategyGame.BaseConversionRate * value));
        }

        [TestMethod]
        public void CheckSettingRocksPerSector_Tech()
        {
            var value = 5;
            _settings.RocksPerSectorTech = value;
            LoadSettings();

            foreach (var s in _game.Map.Sectors)
            {
                var rocks = _game.BuildableAsteroids.Count(_ => _.SectorId == s.Id && _.Type != EAsteroidType.Generic);
                rocks.ShouldBe(value);
            }
        }

        [TestMethod]
        public void CheckSettingsRocksPerSector_Resource()
        {
            var value = 5;
            _settings.RocksPerSectorResource = value;
            LoadSettings();

            foreach (var s in _game.Map.Sectors)
            {
                var rocks = _game.ResourceAsteroids.Count(_ => _.SectorId == s.Id);
                rocks.ShouldBe(value);
            }
        }

        [TestMethod]
        public void CheckSettingsRocksVisible()
        {
            _settings.RocksVisible = true;
            LoadSettings();

            _game.AllAsteroids.ShouldAllBe(_ => _.VisibleToTeam[0] && _.VisibleToTeam[1]);
        }

        [TestMethod]
        public void CheckSettingsAntiShipWeaponRange()
        {
            var value = 0.5f;
            _settings.AntiShipWeaponRangeMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Fighter);
            if (tech == null) return;
            var fig = _game.Ships.CreateShip("Fighter", 1, Color.White, 1);

            var w = fig.Weapons[0];
            var tw = tech.Weapons[0];

            w.WeaponRange.ShouldBe(tw.WeaponRange * value);
        }

        [TestMethod]
        public void CheckSettingsAntiShipWeaponFireRate()
        {
            var value = 0.5f;
            _settings.AntiShipWeaponFireRateMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Fighter);
            if (tech == null) return;
            var fig = _game.Ships.CreateShip("Fighter", 1, Color.White, 1);

            var w = fig.Weapons[0];
            var tw = tech.Weapons[0];

            w.ShootingDelayTicks.ShouldBe((int)(tw.ShootingDelayTicks / value));
        }

        [TestMethod]
        public void CheckSettingsAntiShipWeaponDamage()
        {
            var value = 0.5f;
            _settings.AntiShipWeaponDamageMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Fighter);
            if (tech == null) return;
            var fig = _game.Ships.CreateShip("Fighter", 1, Color.White, 1);

            var w = fig.Weapons[0];
            var tw = tech.Weapons[0];

            w.WeaponDamage.ShouldBe(tw.WeaponDamage * value);
        }

        [TestMethod]
        public void CheckSettingsNanWeaponRange()
        {
            var value = 0.5f;
            _settings.NanWeaponRangeMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Scout);
            if (tech == null) return;
            var ship = _game.Ships.CreateShip("Scout", 1, Color.White, 1);

            var w = ship.Weapons[0];
            var tw = tech.Weapons[0];

            w.WeaponRange.ShouldBe(tw.WeaponRange * value);
        }

        [TestMethod]
        public void CheckSettingsNanWeaponFireRate()
        {
            var value = 0.5f;
            _settings.NanWeaponFireRateMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Scout);
            if (tech == null) return;
            var ship = _game.Ships.CreateShip("Scout", 1, Color.White, 1);

            var w = ship.Weapons[0];
            var tw = tech.Weapons[0];

            w.ShootingDelayTicks.ShouldBe((int)(tw.ShootingDelayTicks / value));
        }

        [TestMethod]
        public void CheckSettingsNanWeaponHealing()
        {
            var value = 0.5f;
            _settings.NanWeaponHealingMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Scout);
            if (tech == null) return;
            var ship = _game.Ships.CreateShip("Scout", 1, Color.White, 1);

            var w = ship.Weapons[0];
            var tw = tech.Weapons[0];

            w.WeaponDamage.ShouldBe(tw.WeaponDamage * value);
        }

        [TestMethod]
        public void CheckSettingsMissileWeaponRange()
        {
            var value = 0.5f;
            _settings.MissileWeaponRangeMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Fighter);
            if (tech == null) return;
            var fig = _game.Ships.CreateShip("Fighter", 1, Color.White, 1);

            var w = fig.Weapons[1];
            var tw = tech.Weapons[1];

            w.WeaponRange.ShouldBe(tw.WeaponRange * value);
        }

        [TestMethod]
        public void CheckSettingsMissileWeaponFireRate()
        {
            var value = 0.5f;
            _settings.MissileWeaponFireRateMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Fighter);
            if (tech == null) return;
            var fig = _game.Ships.CreateShip("Fighter", 1, Color.White, 1);

            var w = fig.Weapons[1];
            var tw = tech.Weapons[1];

            w.ShootingDelayTicks.ShouldBe((int)(tw.ShootingDelayTicks / value));
        }

        [TestMethod]
        public void CheckSettingsMissileWeaponDamage()
        {
            var value = 0.5f;
            _settings.MissileWeaponDamageMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Fighter);
            if (tech == null) return;
            var fig = _game.Ships.CreateShip("Fighter", 1, Color.White, 1);

            var w = fig.Weapons[1];
            var tw = tech.Weapons[1];

            w.WeaponDamage.ShouldBe(tw.WeaponDamage * value);
        }

        [TestMethod]
        public void CheckSettingsMissileWeaponSpeed()
        {
            var value = 0.5f;
            _settings.MissileWeaponSpeedMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Fighter);
            if (tech == null) return;
            var fig = _game.Ships.CreateShip("Fighter", 1, Color.White, 1);

            var w = fig.Weapons[1] as ShipMissileWeapon;
            var tw = tech.Weapons[1] as ShipMissileWeapon;

            w.Speed.ShouldBe(tw.Speed * value);
        }

        [TestMethod]
        public void CheckSettingsMissileWeaponTracking()
        {
            var value = 0.5f;
            _settings.MissileWeaponTrackingMultiplier = value;
            LoadSettings();

            var tech = _game.Ships.Ships.FirstOrDefault(_ => _.Type == EShipType.Fighter);
            if (tech == null) return;
            var fig = _game.Ships.CreateShip("Fighter", 1, Color.White, 1);

            var w = fig.Weapons[1] as ShipMissileWeapon;
            var tw = tech.Weapons[1] as ShipMissileWeapon;

            w.Tracking.ShouldBe(tw.Tracking * value);
        }

        [TestMethod]
        public void CheckSettingsResearchTime()
        {
            var value = 0.5f;
            _settings.ResearchTimeMultiplier = value;
            LoadSettings();
            var techData = AllegianceForms.Engine.Tech.TechTree.LoadTechTree(_game, StrategyGame.TechDataFile, 0);

            for (var t = 0; t < _game.TechTree.Length; t++)
            {
                for (var i = 0; i < _game.TechTree[t].TechItems.Count; i++)
                {
                    var item = _game.TechTree[t].TechItems[i];
                    var original = techData.TechItems[i];

                    item.DurationTicks.ShouldBe((int)(original.DurationTicks * value));
                }
            }
        }

        [TestMethod]
        public void CheckSettingResearchCost()
        {
            var value = 0.5f;
            _settings.ResearchCostMultiplier = value;
            LoadSettings();
            var techData = AllegianceForms.Engine.Tech.TechTree.LoadTechTree(_game, StrategyGame.TechDataFile, 0);

            for (var t = 0; t < _game.TechTree.Length; t++)
            {
                for (var i = 0; i < _game.TechTree[t].TechItems.Count; i++)
                {
                    var item = _game.TechTree[t].TechItems[i];
                    var original = techData.TechItems[i];

                    item.Cost.ShouldBe((int)(original.Cost * value));
                }
            }
        }
    }
}
