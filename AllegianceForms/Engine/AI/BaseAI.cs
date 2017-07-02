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
        public bool ForceVisible { get; set; }
        public bool CheatVisibility { get; set; }
        public bool CheatCredits { get; set; }

        public float CheatAdditionalPilots = 1.5f;
        public int CheatCreditAmout = 3;

        protected int _t;
        protected int _limitActionsPerMinute = 300;
        protected int _limitActionsTickDelay;
        protected int _nextActionAllowed = 0;
        protected StrategyGame _game;

        protected float _cheatCreditsChance = 0.05f;
        protected int _cheatCreditsLastsTicks = 200;
        protected float _cheatVisibilityChance = 0.025f;
        protected int _cheatVisibilityLastsTicks = 100;
        protected int _cheatVisibilityExpires = 0;
        protected int _cheatCreditExpires = 0;
        protected Ship.ShipEventHandler _shipHandler;

        public BaseAI(StrategyGame game, int team, Color teamColour, Ship.ShipEventHandler shipHandler)
        {
            Team = team;
            _t = team - 1;
            _game = game;
            Alliance = game.GameSettings.TeamAlliance[_t];
            TeamColour = teamColour;
            _shipHandler = shipHandler;
            Enabled = true;
        }

        // 0 to 7 (Inactive >> Normal >> Insane)
        public virtual void SetDifficulty(float i)
        {
            i = i - 3;

            // Small negative numbers will make it even easier
            // Don't go lower than -1 (or inactive)

            CheatAdditionalPilots = (int)System.Math.Max(1 + (0.25f * i), 0);
            _limitActionsPerMinute = (int)System.Math.Max((i+1) * 60, 20);
            _limitActionsTickDelay = (60 * 4 / _limitActionsPerMinute);

            // <= 0 (never cheats)
            _cheatCreditsChance = 0.025f * i;
            _cheatCreditsLastsTicks = (int)(5 * 20 * i);
            _cheatVisibilityChance = 0.005f * i;
            _cheatVisibilityLastsTicks = (int)(2 * 20 * i);
        }

        public virtual void Update()
        {
        }

        public virtual void UpdateCheats()
        {
            _cheatVisibilityExpires--;
            _cheatCreditExpires--;

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
            if (CheatCredits && _cheatCreditExpires <= 0)
            {
                CheatCredits = false;
            }

            if (CheatCredits) _game.AddResources(Team, CheatCreditAmout);
        }
    }
}
