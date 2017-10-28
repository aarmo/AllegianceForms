using AllegianceForms.Engine.Map;
using AllegianceForms.Engine.Tech;
using System;
using System.Linq;

namespace AllegianceForms.Engine
{
    public class CampaignGame
    {
        public static string DefaultCampaignFile = ".\\Data\\CurrentCampaign.dat";
        public static int NumAvailableMaps = 20;

        public bool Setup { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int TotalPoints { get; set; }
        public int UnspentPoints { get; set; }
        public GameSettings CurrentSettings { get; set; }
        public TechTree TechTree { get; set; }
        public string[] RemainingMaps { get; set; }

        public static CampaignGame NewGame()
        {
            var c = new CampaignGame()
            {
                CurrentSettings = GameSettings.CampaignStart(),
                Setup = false,
                UnspentPoints = 50,
            };

            c.RemainingMaps = Utils.Shuffle(GameMaps.AvailableMaps(2, false), StrategyGame.Random).Take(NumAvailableMaps).ToArray();

            c.TechTree = TechTree.LoadTechTree(null, StrategyGame.TechDataFile, 1);
            foreach (var i in c.TechTree.TechItems)
            {
                i.DurationTicks = 1;
                i.Cost /= 100;
                i.Unlocked = c.CurrentSettings.RestrictTechToIds[0].Contains(i.Id);
            }
            return c;
        }

        public static CampaignGame LoadOrDefault()
        {
            var l = Utils.DeserialiseFromFile<CampaignGame>(DefaultCampaignFile);
            if (l == null)
            {
                l = NewGame();
                l.SaveGame();
            }

            return l;
        }

        public void SaveGame()
        {
            Utils.SerialiseToFile(DefaultCampaignFile, this);
        }

        public void UpdateResults(StrategyGame game)
        {
            var won = game.Winners.Contains(game.Faction[0]);
            var p = game.TotalCampaignPoints(1);

            GamesPlayed++;
            if (won) GamesWon++;

            TotalPoints += p;
            UnspentPoints += p;

            RemainingMaps = RemainingMaps.Skip(1).ToArray();

            SaveGame();
        }

        public void SetupGame(string playerName, string factionName)
        {
            CurrentSettings.TeamFactions[0].CommanderName = playerName;
            CurrentSettings.TeamFactions[0].Name = factionName;
            Setup = true;
            SaveGame();
        }
    }
}
