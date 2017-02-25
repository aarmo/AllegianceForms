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
        public List<ShipSpec> Ships { get; set; }

        private ShipSpecs(IEnumerable<ShipSpec> items)
        {
            Ships = items.ToList();
        }

        public static ShipSpecs LoadShipSpecs(string shipFile)
        {
            var cfg = new CsvConfiguration()
            {
                WillThrowOnMissingField = false,
                IgnoreBlankLines = true,
                AllowComments = true
            };

            using (var textReader = File.OpenText(shipFile))
            {
                var csv = new CsvReader(textReader, cfg);

                var records = csv.GetRecords<ShipSpec>().ToList();

                foreach (var r in records)
                {
                    r.Initialise();
                }
                return new ShipSpecs(records);
            }
        }

        public CombatShip CreateShip(string name, int team, Color teamColour, int sectorId)
        {
            var unlockedIds = StrategyGame.TechTree[team - 1].CompletedTechIds();
            var type = (EShipType)Enum.Parse(typeof(EShipType), name);

            var spec = (from s in Ships
                        where s.Type == type
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateCombatShip(int team, Color teamColour, int sectorId)
        {
            var unlockedIds = StrategyGame.TechTree[team - 1].CompletedTechIds();
            var keys = new[] { "F", "I", "G", "T" };

            // Get the most advanced ship for any of these keys
            var spec = (from s in Ships
                        where keys.Contains(s.Key)
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                       && StrategyGame.CanLaunchShip(team, s.NumPilots, s.Type)
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateTowerShip(EShipType type, int team, Color teamColour, int sectorId)
        {
            var unlockedIds = StrategyGame.TechTree[team - 1].CompletedTechIds();
            
            // Get the most advanced tower ship
            var spec = (from s in Ships
                        where (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                        && s.Type == type
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        public CombatShip CreateCombatShip(Keys k, int team, Color teamColour, int sectorId)
        {
            var unlockedIds = StrategyGame.TechTree[team - 1].CompletedTechIds();

            // Get the most advanced ship for this key
            var spec = (from s in Ships
                        where s.Key == k.ToString()
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                       && StrategyGame.CanLaunchShip(team, s.NumPilots, s.Type)
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        private CombatShip CreateShip(ShipSpec spec, int team, Color teamColour, int sectorId)
        {
            var t = team - 1;
            var faction = StrategyGame.Faction[t];
            var research = StrategyGame.TechTree[t].ResearchedUpgrades;
            var settings = StrategyGame.GameSettings;

            var ship = new CombatShip(StrategyGame.IconPicDir + spec.Image, spec.Width, spec.Height, teamColour, team
                    , spec.Health * research[EGlobalUpgrade.ShipHull] * settings.ShipHealthMultiplier[spec.Type] * faction.Bonuses.Health, spec.NumPilots, spec.Type, sectorId);

            ship.ScanRange = spec.ScanRange * research[EGlobalUpgrade.ScanRange] * faction.Bonuses.ScanRange;
            ship.Signature = spec.Signature * research[EGlobalUpgrade.ShipSignature] * settings.ShipSignatureMultiplier[spec.Type] * faction.Bonuses.Signature;
            ship.Speed = spec.Speed * research[EGlobalUpgrade.ShipSpeed] * settings.ShipSpeedMultiplier[spec.Type] * faction.Bonuses.Speed;

            if (spec.Weapons != null)
            {
                foreach (var w in spec.Weapons)
                {
                    var nl = w as NanLaserWeapon;
                    if (nl != null)
                    {
                        var clone = new NanLaserWeapon(nl.LaserPen.Width
                            , (int)nl.ShootingDuration.TotalMilliseconds
                            , (int)(nl.ShootingDelay.TotalMilliseconds / (settings.NanWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate))
                            , nl.WeaponRange * settings.NanWeaponRangeMultiplier
                            , nl.WeaponDamage * settings.NanWeaponHealingMultiplier * research[EGlobalUpgrade.RepairHealing]
                            , ship, nl.FireOffset);
                        ship.Weapons.Add(clone);
                        continue;
                    }

                    var sl = w as ShipLaserWeapon;
                    if (sl != null)
                    {
                        var c = sl.LaserPen.Color;
                        if (c.Name == "0") c = teamColour;
                        var clone = new ShipLaserWeapon(c, sl.LaserPen.Width
                            , (int)sl.ShootingDuration.TotalMilliseconds
                            , (int)(sl.ShootingDelay.TotalMilliseconds / (settings.AntiShipWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate))
                            , sl.WeaponRange * settings.AntiShipWeaponRangeMultiplier
                            , sl.WeaponDamage * settings.AntiShipWeaponDamageMultiplier * research[EGlobalUpgrade.WeaponDamage]
                            , ship, sl.FireOffset);
                        ship.Weapons.Add(clone);
                        continue;
                    }
                    
                    var ml = w as ShipMissileWeapon;
                    if (ml != null)
                    {
                        var clone = new ShipMissileWeapon(ml.Width
                            , ml.Speed * settings.MissileWeaponSpeedMultiplier * research[EGlobalUpgrade.MissileSpeed] * faction.Bonuses.MissileSpeed
                            , ml.Tracking * settings.MissileWeaponTrackingMultiplier * research[EGlobalUpgrade.MissileTracking] * faction.Bonuses.MissileTracking
                            , (int)ml.ShootingDuration.TotalMilliseconds
                            , (int)(ml.ShootingDelay.TotalMilliseconds / (settings.MissileWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate))
                            , ml.WeaponRange * settings.MissileWeaponRangeMultiplier
                            , ml.WeaponDamage * settings.MissileWeaponDamageMultiplier * research[EGlobalUpgrade.WeaponDamage]
                            , ship, Point.Empty, new SolidBrush(teamColour));
                        ship.Weapons.Add(clone);
                        continue;
                    }

                    var bl = w as BaseLaserWeapon;
                    if (bl != null)
                    { 
                        var c = bl.LaserPen.Color;
                        if (c.Name == "0") c = teamColour;
                        var clone = new BaseLaserWeapon(c, bl.LaserPen.Width
                            , (int)bl.ShootingDuration.TotalMilliseconds
                            , (int)(bl.ShootingDelay.TotalMilliseconds / (settings.AntiBaseWeaponFireRateMultiplier * research[EGlobalUpgrade.WeaponFireRate] * faction.Bonuses.FireRate))
                            , bl.WeaponRange * settings.AntiBaseWeaponRangeMultiplier
                            , bl.WeaponDamage * settings.AntiBaseWeaponDamageMultiplier * research[EGlobalUpgrade.WeaponDamage]
                            , ship, bl.FireOffset);
                        ship.Weapons.Add(clone);
                        continue;
                    }
                }
            }

            return ship;
        }

        public BuilderShip CreateBuilderShip(EBaseType baseType, int team, Color teamColour, int sectorId)
        {        
            var spec = Ships.FirstOrDefault(_ => _.BaseType == baseType && _.Type == EShipType.Constructor);
            if (spec == null) return null;

            var t = team - 1;
            var faction = StrategyGame.Faction[t];
            var research = StrategyGame.TechTree[t].ResearchedUpgrades;
            var settings = StrategyGame.GameSettings;
            
            var ship = new BuilderShip(StrategyGame.IconPicDir + spec.Image, spec.Width, spec.Height, teamColour, team
                , spec.Health * research[EGlobalUpgrade.ShipHull] * settings.ShipHealthMultiplier[spec.Type] * faction.Bonuses.Health, baseType, sectorId);

            ship.ScanRange = spec.ScanRange * research[EGlobalUpgrade.ScanRange] * faction.Bonuses.ScanRange;
            ship.Signature = spec.Signature * research[EGlobalUpgrade.ShipSignature] * settings.ShipSignatureMultiplier[spec.Type] * faction.Bonuses.Signature;
            ship.Speed = spec.Speed * research[EGlobalUpgrade.ShipSpeed] * settings.ShipSpeedMultiplier[spec.Type] * faction.Bonuses.Speed;

            ship.Name = baseType.ToString();
            ship.TextOffsetY = -15;
            ship.TextBrush = new SolidBrush(teamColour);

            StrategyGame.GameStats.TotalConstructorsBuilt[t]++;

            return ship;
        }

        public MinerShip CreateMinerShip(int team, Color teamColour, int sectorId)
        {
            var spec = Ships.FirstOrDefault(_ => _.Type == EShipType.Miner);
            if (spec == null) return null;

            var t = team - 1;
            var faction = StrategyGame.Faction[t];
            var research = StrategyGame.TechTree[t].ResearchedUpgrades;
            var settings = StrategyGame.GameSettings;

            var ship = new MinerShip(StrategyGame.IconPicDir + spec.Image, spec.Width, spec.Height, teamColour, team
                , spec.Health * research[EGlobalUpgrade.ShipHull] * settings.ShipHealthMultiplier[spec.Type] * faction.Bonuses.Health, sectorId);

            ship.ScanRange = spec.ScanRange * research[EGlobalUpgrade.ScanRange] * faction.Bonuses.ScanRange;
            ship.Signature = spec.Signature * research[EGlobalUpgrade.ShipSignature] * settings.ShipSignatureMultiplier[spec.Type] * faction.Bonuses.Signature;
            ship.Speed = spec.Speed * research[EGlobalUpgrade.ShipSpeed] * settings.ShipSpeedMultiplier[spec.Type] * faction.Bonuses.Speed;

            ship.MaxResourceCapacity = (int)(ship.MaxResourceCapacity * research[EGlobalUpgrade.MinerCapacity] * settings.MinersCapacityMultiplier * faction.Bonuses.MiningCapacity);

            StrategyGame.GameStats.TotalMinersBuilt[t]++;

            return ship;
        }

        public Ship CreateLifepod(int team, Color teamColour, int sectorId)
        {
            var spec = Ships.FirstOrDefault(_ => _.Type == EShipType.Lifepod);
            if (spec == null) return null;

            var t = team - 1;
            var faction = StrategyGame.Faction[t];
            var research = StrategyGame.TechTree[t].ResearchedUpgrades;
            var settings = StrategyGame.GameSettings;

            var ship = new Ship(StrategyGame.IconPicDir + spec.Image, spec.Width, spec.Height, teamColour, team
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


        public void Initialise()
        {
            if (string.IsNullOrEmpty(WeaponData)) return;
            var weaps = WeaponData.Split('>');
            Weapons = new List<Weapon>();

            foreach (var w in weaps)
            {
                if (string.IsNullOrWhiteSpace(w)) continue;
                var data = w.Split('|');

                // Type|Width|FireTime|RefireDelay|Range|Damage|OffsetX|OffsetY>(repeating...)
                switch (data[0])
                {
                    case "0":
                    case "ship":
                        Weapons.Add(new ShipLaserWeapon(Color.Empty, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "1":
                    case "base":
                        Weapons.Add(new BaseLaserWeapon(Color.Empty, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "2":
                    case "nan":
                        Weapons.Add(new NanLaserWeapon(float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "3":
                    case "missile":
                        //new ShipMissileWeapon(8, 5, 250, 2000, 400, 10, testShip, Point.Empty, new SolidBrush(_colourTeam1)
                        Weapons.Add(new ShipMissileWeapon(int.Parse(data[1]), float.Parse(data[8]), float.Parse(data[9]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new PointF(int.Parse(data[6]), int.Parse(data[7])), new SolidBrush(Color.Empty)));
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
