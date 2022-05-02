﻿using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.Bases
{
    public class BaseSpecs
    {
        public List<BaseSpec> Bases { get; set; }
        private StrategyGame _game;

        private BaseSpecs(StrategyGame game, IEnumerable<BaseSpec> items)
        {
            Bases = items.ToList();
            _game = game;
        }

        public static BaseSpecs LoadBaseSpecs(StrategyGame game, string baseFile)
        {
            var cfg = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                AllowComments = true
            };

            using (var textReader = File.OpenText(baseFile))
            {
                var csv = new CsvReader(textReader, cfg);

                var records = csv.GetRecords<BaseSpec>().ToList();

                return new BaseSpecs(game, records);
            }
        }

        public static bool IsTower(EBaseType type)
        {
            return type == EBaseType.MissileTower || type == EBaseType.Tower || type == EBaseType.RepairTower || type == EBaseType.Minefield || type == EBaseType.ShieldTower;
        }

        public Base CreateBase(EBaseType baseType, int team, Color teamColour, int sectorId)
        {
            var spec = Bases.FirstOrDefault(_ => _.Type == baseType);
            if (spec == null) return null;

            var t = team - 1;
            var faction = _game.Faction[t];
            var research = _game.TechTree[t].ResearchedUpgrades;
            var settings = _game.GameSettings;
            var alliance = (t < 0) ? -1 : settings.TeamAlliance[t];

            if (baseType == EBaseType.Shipyard)
            {
                faction.CapitalMaxDrones += settings.InitialCapitalMaxDrones;
            }

            var bs = new Base(_game, baseType, spec.Width, spec.Height, teamColour, team, alliance, spec.Health * settings.StationHealthMultiplier[spec.Type] * faction.Bonuses.Health, sectorId)
            {
                Spec = spec,
                ScanRange = spec.ScanRange * research[EGlobalUpgrade.ScanRange] * faction.Bonuses.ScanRange,
                Signature = spec.Signature * research[EGlobalUpgrade.ShipSignature] * settings.StationSignatureMultiplier[spec.Type] * faction.Bonuses.Signature
            };

            return bs;
        }

        public void DestroyBase(Base bs)
        {
            var team = bs.Team;
            if (team <= 0 || bs.Destroyed) return;
            var spec = bs.Spec;
            if (spec == null) return;

            lock(_game)
            { 
                var t = team - 1;

                var faction = _game.Faction[t];
                var settings = _game.GameSettings;
                if (bs.Type == EBaseType.Shipyard)
                {
                    faction.CapitalMaxDrones -= settings.InitialCapitalMaxDrones;
                }
                bs.Destroyed = true;

                _game.UpdateTotalPilots(bs.Team);
            }
        }

        public void CaptureBase(Base bs, int oldTeam, int newTeam)
        {
            if (oldTeam == 0 || newTeam == 0 || oldTeam == newTeam) return;
            var spec = bs.Spec;
            if (spec == null) return;

            lock (_game)
            {
                _game.UpdateTotalPilots(oldTeam);

                _game.UpdateTotalPilots(newTeam);
            }
        }
    }

    public class BaseSpec
    {
        public int Id { get; set; }
        public EBaseType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Health { get; set; }
        public int ScanRange { get; set; }
        public float Signature { get; set; }
        public int Pilots { get; set; }
    }
}
