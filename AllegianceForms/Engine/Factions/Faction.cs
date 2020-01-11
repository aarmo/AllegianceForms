using AllegianceForms.Engine.Generation;

namespace AllegianceForms.Engine.Factions
{
    public class Faction
    {
        public string Name { get; set; }
        public string PictureCode { get; set; }
        public string CommanderName { get; set; }
        
        public FactionBonus Bonuses { get; set; }
        public bool PromotionGamesRunning { get; set; }
        public bool DemotionGamesRunning { get; set; }
        public int PromotionGamesPlayed { get; set; }
        public int PromotionGamesWon { get; set; }

        public Faction(string name, string commanderName)
        {
            PictureCode = Name = name;
            CommanderName = commanderName;
            Bonuses = new FactionBonus();
        }

        public override string ToString()
        {
            return Name;
        }

        public static RandomPartString FactionNames = new RandomPartString(".\\Data\\FactionNameParts.json");

        public static Faction Default()
        {
            return new Faction("Default", "Player1");
        }

        public static Faction CampaignStart(int powerLoss)
        {
            var dec = powerLoss * 0.1f;

            var f = new Faction("Campaign", "Player1");
            f.Bonuses.FireRate -= dec;
            f.Bonuses.Health -= dec;
            f.Bonuses.MiningCapacity -= dec;
            f.Bonuses.MiningEfficiency -= dec;
            f.Bonuses.MissileSpeed -= dec;
            f.Bonuses.MissileTracking -= dec;
            f.Bonuses.Speed -= dec;
            f.Bonuses.ScanRange -= dec;
            f.Bonuses.ResearchCost += dec;
            f.Bonuses.ResearchTime += dec;
            f.Bonuses.Signature += dec;

            return f;
        }

        public static Faction CreateFaction(string name)
        {
            switch (name)
            {
                case "Default":
                    return Default();
                default:
                    return Random();
            }
        }

        public static Faction Random(int min = 10)
        {
            var name = FactionNames.NextString;
            var f = new Faction(name, StrategyGame.RandomName.GetRandomName(name));
            f.Bonuses.Randomise(min);

            return f;
        }
    }
}
