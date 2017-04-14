using System.Drawing;

namespace AllegianceForms.Engine.AI
{
    public class BaseAI
    {
        public int Team { get; set; }
        public int Alliance { get; set; }
        public Color TeamColour { get; set; }
        public bool Enabled { get; set; }
        public float CheatAdditionalPilots = 1.5f;

        protected int _t;
        protected int _limitActionsPerMinute = 300;
        protected int _limitActionsTickDelay;
        protected int _nextActionAllowed = 0;
        protected StrategyGame _game;

        public BaseAI(StrategyGame game, int team, Color teamColour)
        {
            Team = team;
            _t = team - 1;
            _game = game;
            Alliance = game.GameSettings.TeamAlliance[_t];
            TeamColour = teamColour;
            Enabled = true;
        }

        // 0-3 (Easy - Very Hard)
        public virtual void SetDifficulty(int i)
        {
            CheatAdditionalPilots = 1 + (0.25f * i);
            _limitActionsPerMinute = 60 + (i * 60);
            _limitActionsTickDelay = (60 * 4 / _limitActionsPerMinute);
        }

        public virtual void Update()
        {
        }
    }
}
