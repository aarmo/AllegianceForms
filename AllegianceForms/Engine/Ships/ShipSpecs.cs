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

        public CombatShip CreateCombatShip(int team, Color teamColour, int sectorId)
        {
            var unlockedIds = StrategyGame.TechTree[team - 1].CompletedTechIds();
            var keys = new[] { "F", "I", "G", "T" };

            // Get the most advanced ship for any of these keys
            var spec = (from s in Ships
                        where keys.Contains(s.Key)
                       && (s.DependsOnTechIds == null || s.DependsOnTechIds.All(unlockedIds.Contains))
                       && StrategyGame.CanLaunchShip(team, s.NumPilots)
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
                       && StrategyGame.CanLaunchShip(team, s.NumPilots)
                        orderby s.Id descending
                        select s).FirstOrDefault();
            if (spec == null) return null;

            return CreateShip(spec, team, teamColour, sectorId);
        }

        private CombatShip CreateShip(ShipSpec spec, int team, Color teamColour, int sectorId)
        {
            var ship = new CombatShip(spec.Image, spec.Width, spec.Height, teamColour, team, spec.Health, spec.NumPilots, spec.Type, sectorId);
            ship.ScanRange = spec.ScanRange;
            ship.Signature = spec.Signature;
            ship.Speed = spec.Speed;

            if (spec.Weapons != null)
            {
                foreach (var w in spec.Weapons)
                {
                    var clone = w.ShallowCopy();

                    clone.LaserPen = (Pen)w.LaserPen.Clone();
                    if (w.LaserPen.Color.Name == "0")
                        clone.LaserPen.Color = teamColour;

                    clone.Shooter = ship;
                    clone.Target = null;
                    ship.Weapons.Add(clone);
                }
            }

            return ship;
        }

        public BuilderShip CreateBuilderShip(EBaseType baseType, int team, Color teamColour, int sectorId)
        {        
            var spec = Ships.FirstOrDefault(_ => _.BaseType == baseType && _.Type == EShipType.Constructor);
            if (spec == null) return null;
            
            var ship = new BuilderShip(spec.Image, spec.Width, spec.Height, teamColour, team, spec.Health, baseType, sectorId);
            ship.ScanRange = spec.ScanRange;
            ship.Signature = spec.Signature;
            ship.Speed = spec.Speed;
            ship.Name = baseType.ToString();
            ship.TextOffsetY = -15;
            ship.TextBrush = new SolidBrush(teamColour);

            StrategyGame.GameStats.TotalConstructorsBuilt[ship.Team - 1]++;

            return ship;
        }

        public MinerShip CreateMinerShip(int team, Color teamColour, int sectorId)
        {
            var spec = Ships.FirstOrDefault(_ => _.Type == EShipType.Miner);
            if (spec == null) return null;

            var ship = new MinerShip(spec.Image, spec.Width, spec.Height, teamColour, team, spec.Health, sectorId);
            ship.ScanRange = spec.ScanRange;
            ship.Signature = spec.Signature;
            ship.Speed = spec.Speed;

            StrategyGame.GameStats.TotalMinersBuilt[ship.Team - 1]++;

            return ship;
        }

        public Ship CreateLifepod(int team, Color teamColour, int sectorId)
        {
            var spec = Ships.FirstOrDefault(_ => _.Type == EShipType.Lifepod);
            if (spec == null) return null;

            var ship = new Ship(spec.Image, spec.Width, spec.Height, teamColour, team, spec.Health, 1, sectorId);
            ship.Type = EShipType.Lifepod;
            ship.Speed = spec.Speed;
            ship.ScanRange = spec.ScanRange;
            ship.Signature = spec.Signature;
            
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

        public List<LaserWeapon> Weapons { get; set; }


        public void Initialise()
        {
            if (string.IsNullOrEmpty(WeaponData)) return;
            var weaps = WeaponData.Split('>');
            Weapons = new List<LaserWeapon>();

            foreach (var w in weaps)
            {
                if (string.IsNullOrWhiteSpace(w)) continue;
                var data = w.Split('|');

                // Type|Width|FireTime|RefireDelay|Range|Damage|OffsetX|OffsetY>(repeating...)
                switch (data[0])
                {
                    case "0":
                    case "ship":
                        Weapons.Add(new ShipLaserWeapon(Color.Empty, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new Point(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "1":
                    case "base":
                        Weapons.Add(new BaseLaserWeapon(Color.Empty, float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new Point(int.Parse(data[6]), int.Parse(data[7]))));
                        break;

                    case "2":
                    case "nan":
                        Weapons.Add(new NanLaserWeapon(float.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), null, new Point(int.Parse(data[6]), int.Parse(data[7]))));
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
