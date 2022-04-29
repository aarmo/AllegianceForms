﻿using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.Tech
{
    public class TechTree
    {
        public TechTree()
        {
            TechItems = new List<TechItem>();
            ResearchedUpgrades = new Dictionary<EGlobalUpgrade, float>();
            CompletedTech = new Dictionary<string, bool>();
        }

        public List<TechItem> TechItems { get; set; }
        public Dictionary<EGlobalUpgrade, float> ResearchedUpgrades { get; set; }
        public Dictionary<string, bool> CompletedTech { get; set; }

        public static TechTree LoadTechTree(StrategyGame game, string techFile, int team)
        {
            var cfg = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                IgnoreBlankLines = true,
            };
            
            using (var textReader = File.OpenText(techFile))
            {
                var csv = new CsvReader(textReader, cfg);

                var records = csv.GetRecords<TechItem>().ToList();

                foreach (var r in records)
                {
                    r.Initialise(game, team);
                }
                return new TechTree(records);
            }
        }

        private TechTree(IEnumerable<TechItem> items)
        {
            TechItems = items.ToList();
            ResearchedUpgrades = new Dictionary<EGlobalUpgrade, float>();
            CompletedTech = new Dictionary<string, bool>();

            foreach (var e in (EGlobalUpgrade[])Enum.GetValues(typeof(EGlobalUpgrade)))
            {
                ResearchedUpgrades.Add(e, 1);
            }
        }
        
        public List<int> CompletedTechIds()
        {
            return (from i in TechItems
             where i.Completed
             select i.Id).ToList();
        }

        public List<TechItem> ResearchableItems(ETechType type)
        {
            var unlockedIds = CompletedTechIds();

            var items = (from i in TechItems
                         where !i.Completed
                         && i.Active
                         && i.Type == type
                         && (i.DependsOnIds == null || i.DependsOnIds.All(unlockedIds.Contains))
                         orderby i.Id ascending
                         select i).ToList();

            return items;
        }

        public List<TechItem> ResearchableItemsNot(ETechType type)
        {
            var unlockedIds = CompletedTechIds();

            var items = (from i in TechItems
                         where !i.Completed
                         && i.Active
                         && i.Type != type
                         && (i.DependsOnIds == null || i.DependsOnIds.All(unlockedIds.Contains))
                         orderby i.Id ascending
                         select i).ToList();

            return items;
        }

        public bool HasResearchedShipType(EShipType type)
        {
            var unlockedItems = (from i in TechItems
                                 where i.Completed
                               select i).ToList();

            switch (type)
            {
                case EShipType.Scout:
                case EShipType.Fighter:
                    return true;

                case EShipType.FighterBomber:
                case EShipType.Gunship:
                case EShipType.Bomber:
                case EShipType.StealthBomber:
                case EShipType.Interceptor:
                case EShipType.StealthFighter:
                case EShipType.TroopTransport:
                    return unlockedItems.Any(_ => _.Name.Replace(" ", string.Empty).Contains(type.ToString()));

                default:
                    return false;
            }
        }

        public bool HasResearchedTech(string name)
        {
            // Store the completed tech items in a hash list for faster lookup
            return CompletedTech.ContainsKey(name);
        }

        public void RecordCompleted(string name)
        {
            var t = TechItems.FirstOrDefault(_ => _.Name == name);
            if (t == null) return;

            RecordCompleted(t);
        }

        public void RecordCompleted(TechItem item)
        {
            item.Completed = true;

            if (!CompletedTech.ContainsKey(item.Name)) 
                CompletedTech.Add(item.Name, true);
            
        }
    }
}
