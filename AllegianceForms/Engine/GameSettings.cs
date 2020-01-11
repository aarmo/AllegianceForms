using AllegianceForms.Engine.Factions;
using AllegianceForms.Engine.Map;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AllegianceForms.Engine
{
    public class GameSettings
    {
        public static int[] DefaultTeamColours = new int[] 
        {
            Color.FromArgb(0, 159, 225).ToArgb(),
            Color.FromArgb(202, 178, 0).ToArgb(),
            Color.FromArgb(175, 30, 126).ToArgb(),
            Color.FromArgb(73, 161, 36).ToArgb()
        };

        public EGameType GameType { get; set; }
        public int NumTeams { get; set; }
        public int NumPilots { get; set; }
        public string MapName { get; set; }
        public int[] TeamColours { get; set; }
        public Faction[] TeamFactions { get; set; }
        public int[][] RestrictTechToIds;
        public int[] TeamAlliance { get; set; }
        public bool WormholesVisible { get; set; }
        public float WormholesSignatureMultiplier { get; set; }
        public int MinersInitial { get; set; }
        public int MinersMaxDrones { get; set; }
        public float MinersCapacityMultiplier { get; set; }
        public int ConstructorsMaxDrones { get; set; }
        public int ConstructorsMaxTowerDrones { get; set; }
        public int CapitalMaxDrones { get; set; }
        public Dictionary<EShipType, float> ShipSpeedMultiplier { get; set; }
        public Dictionary<EShipType, float> ShipHealthMultiplier { get; set; }
        public Dictionary<EShipType, float> ShipSignatureMultiplier { get; set; }
        public Dictionary<EBaseType, float> StationHealthMultiplier { get; set; }
        public Dictionary<EBaseType, float> StationSignatureMultiplier { get; set; }
        public float ResourcesStartingMultiplier { get; set; }
        public float ResourcesPerRockMultiplier { get; set; }
        public float ResourceConversionRateMultiplier { get; set; }
        public float ResourcesEachTickMultiplier { get; set; }
        public int RocksPerSectorTech { get; set; }
        public int RocksPerSectorResource { get; set; }
        public int RocksPerSectorGeneral { get; set; }
        public List<EAsteroidType> RocksAllowedTech { get; set; }
        public bool RocksVisible { get; set; }
        public float AntiShipWeaponRangeMultiplier { get; set; }
        public float AntiShipWeaponFireRateMultiplier { get; set; }
        public float AntiShipWeaponDamageMultiplier { get; set; }
        public float NanWeaponRangeMultiplier { get; set; }
        public float NanWeaponFireRateMultiplier { get; set; }
        public float NanWeaponHealingMultiplier { get; set; }
        public float AntiBaseWeaponRangeMultiplier { get; set; }
        public float AntiBaseWeaponFireRateMultiplier { get; set; }
        public float AntiBaseWeaponDamageMultiplier { get; set; }
        public float MissileWeaponRangeMultiplier { get; set; }
        public float MissileWeaponFireRateMultiplier { get; set; }
        public float MissileWeaponDamageMultiplier { get; set; }
        public float MissileWeaponSpeedMultiplier { get; set; }
        public float MissileWeaponTrackingMultiplier { get; set; }
        public int AiDifficulty { get; set; }
        public bool VariantAi { get; set; }
        public float ResearchTimeMultiplier { get; set; }
        public float ResearchCostMultiplier { get; set; }
        public float AlienChance { get; set; }
        public int MinAliensPerSector { get; set; }
        public int MaxAliensPerSector { get; set; }
        public int MinAlienBasesPerSector { get; set; }
        public int MaxAlienBasesPerSector { get; set; }
        public int WaveShipsPerBase { get; set; }
        public int InitialWaveDelay { get; set; }
        public int DecreaseWaveDelay { get; set; }
        public EWaveTargetType AlientWaveTargetType { get; set; }
        public float GameSpeed { get; set; }

        public static GameSettings Default()
        {
            var s = new GameSettings
            {
                NumTeams = 2,
                GameType = EGameType.Skirmish,
                MapName = GameMaps.RandomName(2, false),
                WormholesVisible = true,
                RocksVisible = false,

                TeamFactions = new[] { Faction.Default(), Faction.Random() },
                TeamColours = new[] { DefaultTeamColours[0], DefaultTeamColours[1] },
                TeamAlliance = new[] { 1, 2 },
                RestrictTechToIds = new int[2][],
                NumPilots = 16,
                AiDifficulty = 3,
                VariantAi = true,

                WormholesSignatureMultiplier = 1,

                MinersInitial = 1,
                MinersMaxDrones = 4,
                MinersCapacityMultiplier = 1,

                ConstructorsMaxDrones = 1,
                ConstructorsMaxTowerDrones = 4,
                CapitalMaxDrones = 2,

                ResourcesStartingMultiplier = 1,
                ResourcesPerRockMultiplier = 1,
                ResourcesEachTickMultiplier = 1,
                ResourceConversionRateMultiplier = 1,

                ResearchCostMultiplier = 1,
                ResearchTimeMultiplier = 1,

                RocksPerSectorTech = 2,
                RocksPerSectorResource = 4,
                RocksPerSectorGeneral = 8,
                RocksAllowedTech = new List<EAsteroidType> { EAsteroidType.Carbon, EAsteroidType.Silicon, EAsteroidType.Uranium },

                StationHealthMultiplier = new Dictionary<EBaseType, float>(),
                StationSignatureMultiplier = new Dictionary<EBaseType, float>(),

                ShipSpeedMultiplier = new Dictionary<EShipType, float>(),
                ShipHealthMultiplier = new Dictionary<EShipType, float>(),
                ShipSignatureMultiplier = new Dictionary<EShipType, float>(),

                AntiShipWeaponRangeMultiplier = 1,
                AntiShipWeaponFireRateMultiplier = 1,
                AntiShipWeaponDamageMultiplier = 1,

                NanWeaponRangeMultiplier = 1,
                NanWeaponFireRateMultiplier = 1,
                NanWeaponHealingMultiplier = 1,

                AntiBaseWeaponRangeMultiplier = 1,
                AntiBaseWeaponFireRateMultiplier = 1,
                AntiBaseWeaponDamageMultiplier = 1,

                MissileWeaponDamageMultiplier = 1,
                MissileWeaponFireRateMultiplier = 1,
                MissileWeaponRangeMultiplier = 1,
                MissileWeaponSpeedMultiplier = 1,
                MissileWeaponTrackingMultiplier = 1,

                AlienChance = 0f,
                MinAliensPerSector = 2,
                MaxAliensPerSector = 4,
                MinAlienBasesPerSector = 1,
                MaxAlienBasesPerSector = 2,
                WaveShipsPerBase = 1,
                InitialWaveDelay = 45 * 4,
                DecreaseWaveDelay = 1 * 4,

                AlientWaveTargetType = EWaveTargetType.Everyone,

                GameSpeed = 0.5f
            };

            foreach (EBaseType e in Enum.GetValues(typeof(EBaseType)))
            {
                s.StationHealthMultiplier.Add(e, 1);
                s.StationSignatureMultiplier.Add(e, 1);
            }

            foreach (EShipType e in Enum.GetValues(typeof(EShipType)))
            {
                s.ShipSpeedMultiplier.Add(e, 1);
                s.ShipHealthMultiplier.Add(e, 1);
                s.ShipSignatureMultiplier.Add(e, 1);
            }

/*#if DEBUG
            // Testing setup: fast cheap tech, map visible
            //StrategyGame.AddResources(1, 100000, false);
            //settings.ResearchCostMultiplier = 0.25f;
            s.ResearchCostMultiplier = 0.25f;
            s.ResearchTimeMultiplier = 0.25f;
#endif*/
            return s;
        }
    }
}
