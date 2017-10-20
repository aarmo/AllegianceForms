using System;
using System.Linq;

namespace AllegianceForms.Engine
{
    public class CampaignGame
    {
        public static string DefaultCampaignFile = ".\\Data\\CurrentCampaign.dat";

        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int TotalPoints { get; set; }
        public int UnspentPoints { get; set; }
        public GameSettings CurrentSettings { get; set; }

        public static CampaignGame NewGame()
        {
            return new CampaignGame()
            {
                CurrentSettings = GameSettings.CampaignStart()
            };
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

            SaveGame();
        }
    }
}
