using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.Ships
{
    public class Ability
    {
        public delegate void AbilityEventHandler(Ability sender);
        
        public event AbilityEventHandler AbilityStarted;
        public event AbilityEventHandler AbilityFinished;

        public EAbilityType AbilityEffect { get; private set; }
        public bool Active { get; private set; } = false;
        public DateTime AvailableAfter { get; private set; } = DateTime.MinValue;
        public DateTime InActiveAfter { get; private set; } = DateTime.MinValue;

        public float CooldownDuration { get; private set; }
        public float AbilityEffectMultiplier { get; private set; }
        public float AbilityDuration { get; private set; }

        public Ability(EAbilityType type, float cooldownDuration = 30f, float abilityEffectMultiplier = 1.5f, float abilityDuration = 5f)
        {
            AbilityEffect = type;
            CooldownDuration = cooldownDuration;
            AbilityEffectMultiplier = abilityEffectMultiplier;
            AbilityDuration = abilityDuration;
        }

        public bool IsActive()
        {
            if (!Active) return false;

            if (Active && DateTime.Now > InActiveAfter)
            {
                Active = false;

                if (AbilityFinished != null) AbilityFinished(this);
                return false;
            }

            return true;
        }

        public bool IsReady()
        {
            if (Active || DateTime.Now < AvailableAfter) return false;

            return true;
        }

        public bool Activate()
        {
            if (!IsReady()) return false;

            Active = true;
            AvailableAfter = DateTime.Now.AddSeconds(CooldownDuration);
            InActiveAfter = DateTime.Now.AddSeconds(AbilityDuration);

            if (AbilityStarted != null) AbilityStarted(this);
            return true;
        }

        internal static Dictionary<EAbilityType, AbilityDataItem> LoadAbilitData(string dataFile)
        {
            var cfg = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true,
            };

            using (var textReader = File.OpenText(dataFile))
            {
                var csv = new CsvReader(textReader, cfg);

                return csv.GetRecords<AbilityDataItem>().ToDictionary(_ => _.AbilityType);
            }
        }

        internal static Dictionary<EShipType, List<DefaultShipAbilityItem>> LoadEnabledAbilities(string dataFile)
        {
            var cfg = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true,
            };

            using (var textReader = File.OpenText(dataFile))
            {
                var csv = new CsvReader(textReader, cfg);
                var data = csv.GetRecords<DefaultShipAbilityItem>().ToList();
                data.ForEach(_ => _.LoadAbilities());

                var result = new Dictionary<EShipType, List<DefaultShipAbilityItem>>();

                foreach (var g in data.GroupBy(_ => _.ShipType))
                {
                    result.Add(g.Key, g.ToList());
                }

                return result;
            }
        }
    }

    public class AbilityDataItem
    {
        public EAbilityType AbilityType { get; set; }
        public float CooldownDuration { get; set; }
        public float AbilityEffectMultiplier { get; set; }
        public float AbilityDuration { get; set; }
    }

    public class DefaultShipAbilityItem
    {
        public EShipType ShipType { get; set; }
        public string RequiresResearch { get; set; }
        public string IncludeAbilities { get; set; }

        public EAbilityType[] ParsedAbilities { get; set; }

        public void LoadAbilities()
        {
            ParsedAbilities = IncludeAbilities.Split('|').Select(_ => (EAbilityType)Enum.Parse(typeof(EAbilityType), _)).ToArray();
        }
    }
}