using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.Tech
{
    public class TechTree
    {
        public List<TechItem> TechItems { get; set; }

        public Dictionary<EGlobalUpgrade, float> ResearchedUpgrades { get; set; }

        public static TechTree LoadTechTree(string techFile, int team)
        {
            var cfg = new CsvConfiguration()
            {
                WillThrowOnMissingField = false,
                IgnoreBlankLines = true,
            };
            
            using (var textReader = File.OpenText(techFile))
            {
                var csv = new CsvReader(textReader, cfg);

                var records = csv.GetRecords<TechItem>().ToList();

                foreach (var r in records)
                {
                    r.Initialise(team);
                }
                return new TechTree(records);
            }
        }

        private TechTree(IEnumerable<TechItem> items)
        {
            TechItems = items.ToList();
            ResearchedUpgrades = new Dictionary<EGlobalUpgrade, float>();

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
    }
}
