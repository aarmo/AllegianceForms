namespace AllegianceForms.Engine
{
    public class GameStats
    {
        public int[] TotalResourcesMined;
        public int[] TotalBasesBuilt;
        public int[] TotalBasesDestroyed;
        public int[] TotalMinersBuilt;
        public int[] TotalMinersDestroyed;
        public int[] TotalConstructorsBuilt;
        public int[] TotalConstructorsDestroyed;
        public bool GameComplete;
        public int AllianceWon;

        public GameStats(int numTeams)
        {
            TotalResourcesMined = new int[numTeams];
            TotalBasesBuilt = new int[numTeams];
            TotalBasesDestroyed = new int[numTeams];
            TotalMinersBuilt = new int[numTeams];
            TotalMinersDestroyed = new int[numTeams];
            TotalConstructorsBuilt = new int[numTeams];
            TotalConstructorsDestroyed = new int[numTeams];

            GameComplete = false;
            AllianceWon = -1;
        }        
    }
}
