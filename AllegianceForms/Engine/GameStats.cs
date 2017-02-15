namespace AllegianceForms.Engine
{
    public class GameStats
    {
        public int[] TotalResourcesMined = new int[StrategyGame.NumTeams];
        public int[] TotalBasesBuilt = new int[StrategyGame.NumTeams];
        public int[] TotalBasesDestroyed = new int[StrategyGame.NumTeams];
        public int[] TotalMinersBuilt = new int[StrategyGame.NumTeams];
        public int[] TotalMinersDestroyed = new int[StrategyGame.NumTeams];
        public int[] TotalConstructorsBuilt = new int[StrategyGame.NumTeams];
        public int[] TotalConstructorsDestroyed = new int[StrategyGame.NumTeams];
    }
}
