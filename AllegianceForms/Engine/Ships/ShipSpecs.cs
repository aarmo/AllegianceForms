using AllegianceForms.Engine.Weapons;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System;

namespace AllegianceForms.Engine.Ships
{
    public class ShipSpecs
    {
        public Dictionary<ERaceType, List<ShipSpec>> RaceShips { get; set; }
        private StrategyGame _game;

        private ShipSpecs(StrategyGame game, Dictionary<ERaceType, List<ShipSpec>> raceShipSpecs)
        {
            RaceShips = raceShipSpecs;
            _game = game;
        }

        public static ShipSpecs LoadShipSpecs(StrategyGame game)
        {
            var cfg = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                AllowComments = true
            };

            var specs = new Dictionary<ERaceType, List<ShipSpec>>();

            foreach(ERaceType race in Enum.GetValues(typeof(ERaceType)))
            { 
                var shipFile = Path.Combine(StrategyGame.DataDir, $"Ships-{race}.txt");

                using (var textReader = File.OpenText(shipFile))
                {
                    var csv = new CsvReader(textReader, cfg);

                    var records = csv.GetRecords<ShipSpec>().ToList();

                    foreach (var r in records)
                    {
                        r.Initialise(game);
                    }

                    specs.Add(race, records);
                }
            }
            return new ShipSpecs(game, specs);
        }

        public CombatShip CreateShip(string name, int team, Color teamColour, int sectorId)
        {
            var unlockedIds = _game.TechTree[team - 1].CompletedTechIds();
            var type = (EShipType)Enum.Parse(typeof(EShipType), name.Replace(" ", string.Empty));
            var race = _game.Faction[team - 1].Race;

            var spec = (from s in RaceShips[race]
                        where s.Type == type
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateCombatShip(int team, Color teamColour, int sectorId)
        {
            var unlockedIds = _game.TechTree[team - 1].CompletedTechIds();
            var race = _game.Faction[team - 1].Race;
            var keys = new[] { "F", "I", "G", "T" };

            // Get the most advanced ship for any of these keys
            var spec = (from s in RaceShips[race]
                        where keys.Contains(s.Key)
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                       && _game.CanLaunchShip(team, s.NumPilots, s.Type)
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateBomberShip(int team, Color teamColour, int sectorId)
        {
            var unlockedIds = _game.TechTree[team - 1].CompletedTechIds();
            var race = _game.Faction[team - 1].Race;
            var keys = new[] { "B", "O", "X" };

            // Get the most advanced ship for any of these keys
            var spec = (from s in RaceShips[race]
                        where keys.Contains(s.Key)
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                       && _game.CanLaunchShip(team, s.NumPilots, s.Type)
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateTowerShip(EShipType type, int team, Color teamColour, int sectorId)
        {
            var unlockedIds = _game.TechTree[team - 1].CompletedTechIds();
            var race = _game.Faction[team - 1].Race;

            // Get the most advanced tower ship
            var spec = (from s in RaceShips[race]
                        where (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                        && s.Type == type
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateCombatShip(Keys k, int team, Color teamColour, int sectorId)
        {
            var unlockedIds = _game.TechTree[team - 1].CompletedTechIds();
            var race = _game.Faction[team - 1].Race;

            // Get the most advanced ship for this key
            var spec = (from s in RaceShips[race]
                        where s.Key == k.ToString()
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                       && _game.CanLaunchShip(team, s.NumPilots, s.Type)
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateCombatShip(EShipType[] types, int team, Color teamColour, int sectorId)
        {
            var unlockedIds = _game.TechTree[team - 1].CompletedTechIds();
            var race = _game.Faction[team - 1].Race;

            // Get the most advanced ship for these types
            var spec = (from s in RaceShips[race]
                        where types.Contains(s.Type)
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                       && _game.CanLaunchShip(team, s.NumPilots, s.Type)
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateShip(ShipSpec spec, int team, Color teamColour, int sectorId)
        {
            var t = team - 1;
            var faction = _game.Faction[t];
            var race = faction.Race;
            var research = _game.TechTree[t].ResearchedUpgrades;
            var settings = _game.GameSettings;
            var alliance = (t < 0) ? -1 : settings.TeamAlliance[t];
            var raceSettings = _game.RaceSettings[race];

            var health = spec.Health * research[EGlobalUpgrade.ShipHull] * settings.ShipHealthMultiplier[spec.Type] * faction.Bonuses.Health * raceSettings.HullMultiplier;

            var ship = new CombatShip(_game, StrategyGame.IconPicDir + spec.Image, spec.Width, spec.Height, teamColour, team, alliance
                    , health, spec.NumPilots, spec.Type, sectorId);

            ship.MaxShield = ship.Shield *= raceSettings.ShieldMultiplier;
            ship.Speed = spec.Speed * research[EGlobalUpgrade.ShipSpeed] * settings.ShipSpeedMultiplier[spec.Type] * faction.Bonuses.Speed * settings.GameSpeed * raceSettings.SpeedMultiplier;

            ship.ScanRange = spec.ScanRange * research[EGlobalUpgrade.ScanRange] * faction.Bonuses.ScanRange;
            ship.Signature = spec.Signature * research[EGlobalUpgrade.ShipSignature] * settings.ShipSignatureMultiplier[spec.Type] * faction.Bonuses.Signature;

            if (spec.Weapons != null)
            {
                foreach (var w in spec.Weapons)
                {
                    if (w is NanLaserWeapon nl)
                    {
                        var clone = new NanLaserWeapon(_game, nl.LaserPen.Width
                            , nl.ShootingTicks
                            , (int)(nl.ShootingDelayTicks / (settings.NanWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate))
                            , nl.WeaponRange * settings.NanWeaponRangeMultiplier
                            , nl.WeaponDamage * settings.NanWeaponHealingMultiplier * research[EGlobalUpgrade.RepairHealing]
                            , ship, nl.FireOffset);
                        ship.Weapons.Add(clone);
                        continue;
                    }

                    if (w is ShieldChargeWeapon cl)
                    {
                        var clone = new ShieldChargeWeapon(_game, cl.LaserPen.Width
                            , cl.ShootingTicks
                            , (int)(cl.ShootingDelayTicks / (settings.NanWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate))
                            , cl.WeaponRange * settings.NanWeaponRangeMultiplier
                            , cl.WeaponDamage * settings.NanWeaponHealingMultiplier * research[EGlobalUpgrade.RepairHealing]
                            , ship, cl.FireOffset);
                        ship.Weapons.Add(clone);
                        continue;
                    }

                    if (w is ShipLaserWeapon sl)
                    {
                        var c = sl.LaserPen.Color;
                        if (c.Name == "0") c = teamColour;
                        var clone = new ShipLaserWeapon(_game, c, sl.LaserPen.Width
                            , sl.ShootingTicks
                            , (int)(sl.ShootingDelayTicks / (settings.AntiShipWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate * research[EGlobalUpgrade.LaserFireRate]))
                            , sl.WeaponRange * settings.AntiShipWeaponRangeMultiplier
                            , sl.WeaponDamage * settings.AntiShipWeaponDamageMultiplier * research[EGlobalUpgrade.WeaponDamage] * research[EGlobalUpgrade.LaserDamage]
                            , ship, sl.FireOffset);
                        ship.Weapons.Add(clone);
                        continue;
                    }

                    if (w is MineWeapon mw)
                    {
                        var c = mw.Colour;
                        if (c.Name == "0") c = teamColour;
                        var clone = new MineWeapon(_game, mw.Width, (int)mw.Duration
                            , (int)(mw.ShootingDelayTicks / (settings.AntiShipWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate))
                            , mw.WeaponRange * settings.AntiShipWeaponRangeMultiplier
                            , mw.WeaponDamage * settings.AntiShipWeaponDamageMultiplier * research[EGlobalUpgrade.WeaponDamage]
                            , ship, mw.FireOffset, c);
                        ship.Weapons.Add(clone);
                        continue;
                    }

                    if (w is ShipMissileWeapon ml)
                    {
                        var clone = new ShipMissileWeapon(_game, ml.Width
                            , ml.Speed * settings.MissileWeaponSpeedMultiplier * research[EGlobalUpgrade.MissileSpeed] * faction.Bonuses.MissileSpeed
                            , ml.Tracking * settings.MissileWeaponTrackingMultiplier * research[EGlobalUpgrade.MissileTracking] * faction.Bonuses.MissileTracking
                            , ml.ShootingTicks
                            , (int)(ml.ShootingDelayTicks / (settings.MissileWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate * research[EGlobalUpgrade.MissileFireRate]))
                            , ml.WeaponRange * settings.MissileWeaponRangeMultiplier
                            , ml.WeaponDamage * settings.MissileWeaponDamageMultiplier * research[EGlobalUpgrade.WeaponDamage] * research[EGlobalUpgrade.MissileDamage]
                            , ship, Point.Empty, new SolidBrush(teamColour));
                        ship.Weapons.Add(clone);
                        continue;
                    }

                    if (w is BaseLaserWeapon bl)
                    {
                        var c = bl.LaserPen.Color;
                        if (c.Name == "0") c = teamColour;
                        var clone = new BaseLaserWeapon(_game, c, bl.LaserPen.Width
                            , bl.ShootingTicks
                            , (int)(bl.ShootingDelayTicks / (settings.AntiBaseWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate))
                            , bl.WeaponRange * settings.AntiBaseWeaponRangeMultiplier
                            , bl.WeaponDamage * settings.AntiBaseWeaponDamageMultiplier * research[EGlobalUpgrade.WeaponDamage]
                            , ship, bl.FireOffset);
                        ship.Weapons.Add(clone);
                        continue;
                    }

                    if (w is CarrierDroneWeapon cd)
                    {
                        var clone = new CarrierDroneWeapon(_game
                            , cd.ShootingTicks
                            , cd.ShootingDelayTicks
                            , cd.WeaponRange
                            , cd.WeaponDamage
                            , ship, cd.FireOffset, cd.DroneShipId, cd.DroneCost);
                        ship.Weapons.Add(clone);
                        continue;
                    }
                }
            }

            return ship;
        }

        public BuilderShip CreateBuilderShip(EBaseType baseType, int team, Color teamColour, int sectorId)
        {
            var race = _game.Faction[team - 1].Race;
            var spec = RaceShips[race].FirstOrDefault(_ => _.BaseType == baseType && _.Type == EShipType.Constructor);
            if (spec == null) return null;

            var t = team - 1;
            var faction = _game.Faction[t];
            var research = _game.TechTree[t].ResearchedUpgrades;
            var settings = _game.GameSettings;
            var alliance = (t < 0) ? -1 : settings.TeamAlliance[t];
            var raceSettings = _game.RaceSettings[race];

            var health = spec.Health * research[EGlobalUpgrade.ShipHull] * settings.ShipHealthMultiplier[spec.Type] * faction.Bonuses.Health * raceSettings.HullMultiplier;

            var ship = new BuilderShip(_game, StrategyGame.IconPicDir + spec.Image, spec.Width, spec.Height, teamColour, team, alliance
                , health, baseType, sectorId);

            ship.MaxShield = ship.Shield *= raceSettings.ShieldMultiplier;
            ship.Speed = spec.Speed * research[EGlobalUpgrade.ShipSpeed] * settings.ShipSpeedMultiplier[spec.Type] * faction.Bonuses.Speed * settings.GameSpeed * raceSettings.SpeedMultiplier;

            ship.ScanRange = spec.ScanRange * research[EGlobalUpgrade.ScanRange] * faction.Bonuses.ScanRange;
            ship.Signature = spec.Signature * research[EGlobalUpgrade.ShipSignature] * settings.ShipSignatureMultiplier[spec.Type] * faction.Bonuses.Signature;

            ship.Name = baseType.ToString();
            ship.TextOffsetY = -15;
            ship.TextBrush = new SolidBrush(teamColour);

            _game.GameStats.TotalConstructorsBuilt[t]++;

            return ship;
        }

        public MinerShip CreateMinerShip(int team, Color teamColour, int sectorId)
        {
            var race = _game.Faction[team - 1].Race;
            var spec = RaceShips[race].FirstOrDefault(_ => _.Type == EShipType.Miner);
            if (spec == null) return null;

            var t = team - 1;
            var faction = _game.Faction[t];
            var research = _game.TechTree[t].ResearchedUpgrades;
            var settings = _game.GameSettings;
            var alliance = (t < 0) ? -1 : settings.TeamAlliance[t];
            var raceSettings = _game.RaceSettings[race];

            var health = spec.Health * research[EGlobalUpgrade.ShipHull] * settings.ShipHealthMultiplier[spec.Type] * faction.Bonuses.Health * raceSettings.HullMultiplier;


            var ship = new MinerShip(_game, StrategyGame.IconPicDir + spec.Image, spec.Width, spec.Height, teamColour, team, alliance
                , health, sectorId);

            ship.MaxShield = ship.Shield *= raceSettings.ShieldMultiplier;
            ship.Speed = spec.Speed * research[EGlobalUpgrade.ShipSpeed] * settings.ShipSpeedMultiplier[spec.Type] * faction.Bonuses.Speed * settings.GameSpeed * raceSettings.SpeedMultiplier;

            ship.ScanRange = spec.ScanRange * research[EGlobalUpgrade.ScanRange] * faction.Bonuses.ScanRange;
            ship.Signature = spec.Signature * research[EGlobalUpgrade.ShipSignature] * settings.ShipSignatureMultiplier[spec.Type] * faction.Bonuses.Signature;
         
            ship.MaxResourceCapacity = (int)(ship.MaxResourceCapacity * research[EGlobalUpgrade.MinerCapacity] * settings.MinersCapacityMultiplier * faction.Bonuses.MiningCapacity * settings.GameSpeed);

            _game.GameStats.TotalMinersBuilt[t]++;

            return ship;
        }

        public Ship CreateLifepod(int team, Color teamColour, int sectorId)
        {
            var race = _game.Faction[team - 1].Race;
            var spec = RaceShips[race].FirstOrDefault(_ => _.Type == EShipType.Lifepod);
            if (spec == null) return null;

            var t = team - 1;
            var faction = _game.Faction[t];
            var research = _game.TechTree[t].ResearchedUpgrades;
            var settings = _game.GameSettings;
            var alliance = (t < 0) ? -1 : settings.TeamAlliance[t];

            var ship = new Ship(_game, StrategyGame.IconPicDir + spec.Image, spec.Width, spec.Height, teamColour, team, alliance
                , spec.Health * research[EGlobalUpgrade.ShipHull] * settings.ShipHealthMultiplier[spec.Type] * faction.Bonuses.Health, 1, sectorId);
            ship.Type = EShipType.Lifepod;

            ship.ScanRange = spec.ScanRange * research[EGlobalUpgrade.ScanRange] * faction.Bonuses.ScanRange;
            ship.Signature = spec.Signature * research[EGlobalUpgrade.ShipSignature] * settings.ShipSignatureMultiplier[spec.Type] * faction.Bonuses.Signature;
            ship.Speed = spec.Speed * research[EGlobalUpgrade.ShipSpeed] * settings.ShipSpeedMultiplier[spec.Type] * faction.Bonuses.Speed;

            return ship;
        }
    }

    public class ShipSpec
    {
        public int Id { get; set; }
        public EShipType Type { get; set; }
        public string Key { get; set; }
        public string Image { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Health { get; set; }
        public int NumPilots { get; set; }
        public float Speed { get; set; }
        public int ScanRange { get; set; }
        public float Signature { get; set; }
        public string WeaponData { get; set; }
        public EBaseType BaseType { get; set; }
        public string PreReqTechIds { get; set; }
        public int[] DependsOnTechIds { get; set; }
        public List<Weapon> Weapons { get; set; }

        public void Initialise(StrategyGame game)
        {
            if (string.IsNullOrEmpty(WeaponData)) return;
            var weaps = WeaponData.Split('>');
            Weapons = new List<Weapon>();

            foreach (var w in weaps)
            {
                if (string.IsNullOrWhiteSpace(w)) continue;
                var data = w.Split('|');

                // Type|Width|FireTicks|RefireTicks|Range|Damage|OffsetX|OffsetY>(repeating...)
                switch (data[0])
                {
                    case "0":
                    case "ship":
                        Weapons.Add(new ShipLaserWeapon(game, Color.Empty, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "1":
                    case "base":
                        Weapons.Add(new BaseLaserWeapon(game, Color.Empty, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "2":
                    case "nan":
                        Weapons.Add(new NanLaserWeapon(game, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "3":
                    case "missile":
                        Weapons.Add(new ShipMissileWeapon(game, int.Parse(data[1]), float.Parse(data[8]), float.Parse(data[9]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7])), new SolidBrush(Color.Empty)));
                        break;

                    case "4":
                    case "mine":
                        Weapons.Add(new MineWeapon(game, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7])), Color.Empty));
                        break;

                    case "5":
                    case "shield":
                        Weapons.Add(new ShieldChargeWeapon(game, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "6":
                    case "drones":
                        Weapons.Add(new CarrierDroneWeapon(game, int.Parse(data[1]), int.Parse(data[2]), float.Parse(data[3]), float.Parse(data[4]), null, new PointF(int.Parse(data[5]), int.Parse(data[6])), int.Parse(data[7]), int.Parse(data[8])));
                        break;

                }
            }

            if (!string.IsNullOrEmpty(PreReqTechIds))
            {
                DependsOnTechIds = PreReqTechIds.Split('|').Select(int.Parse).ToArray();
            }
            WeaponData = string.Empty;
        }
    }
}
