using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Engine.AI
{
    public class BaseAI
    {
        public int Team { get; set; }
        public int Alliance { get; set; }
        public Color TeamColour { get; set; }
        public bool Enabled { get; set; }
        public bool CheatVisibility { get; set; }
        public bool CheatCredits { get; set; }

        public float CheatAdditionalPilots = 1.5f;
        public int CheatCreditAmout = 3;

        protected int _t;
        protected int _limitActionsPerMinute = 300;
        protected int _limitActionsTickDelay;
        protected int _nextActionAllowed = 0;

        protected float _cheatCreditsChance = 0.05f;
        protected int _cheatCreditsLastsTicks = 200;
        protected float _cheatVisibilityChance = 0.025f;
        protected int _cheatVisibilityLastsTicks = 100;
        protected int _cheatVisibilityExpires = 0;
        protected int _cheatCreditExpires = 0;
        protected Ship.ShipEventHandler _shipHandler;

        public BaseAI(int team, Color teamColour, Ship.ShipEventHandler shipHandler)
        {
            Team = team;
            _t = team - 1;
            Alliance = StrategyGame.GameSettings.TeamAlliance[_t];
            TeamColour = teamColour;
            _shipHandler = shipHandler;
            Enabled = true;
        }

        // 0-3 (Easy - Very Hard)
        public virtual void SetDifficulty(int i)
        {
            CheatAdditionalPilots = 1 + (0.25f * i);
            _limitActionsPerMinute = 60 + (i * 60);
            _limitActionsTickDelay = (60 * 4 / _limitActionsPerMinute);

            _cheatCreditsChance = 0.025f * i;
            _cheatCreditsLastsTicks = 5 * 20 * i;
            _cheatVisibilityChance = 0.005f * i;
            _cheatVisibilityLastsTicks = 2 * 20 * i;
        }

        public virtual void Update()
        {
        }

        public virtual void UpdateCheats()
        {
            _cheatVisibilityExpires--;
            _cheatCreditsLastsTicks--;

            if (StrategyGame.Random.NextDouble() <= _cheatVisibilityChance)
            {
                CheatVisibility = true;
                _cheatVisibilityExpires = _cheatVisibilityLastsTicks;
            }
            if (CheatVisibility && _cheatVisibilityExpires <= 0)
            {
                CheatVisibility = false;
            }

            if (StrategyGame.Random.NextDouble() <= _cheatCreditsChance)
            {
                CheatCredits = true;
                _cheatCreditExpires = _cheatCreditsLastsTicks;
            }
            if (CheatCredits && _cheatCreditsLastsTicks <= 0)
            {
                CheatCredits = false;
            }

            if (CheatCredits) StrategyGame.AddResources(Team, CheatCreditAmout);
        }
    }
}
