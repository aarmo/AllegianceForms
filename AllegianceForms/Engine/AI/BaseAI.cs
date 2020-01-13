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
            Alliance = (team < 0) ? -1 : game.GameSettings.TeamAlliance[_t];

            _game = game;
            TeamColour = teamColour;
            _shipHandler = shipHandler;
            Enabled = true;
        }

        // 0 to 7 Inactive(0) >> Normal(3) >> Insane(7)
        public virtual void SetDifficulty(float i)
        {
            // Don't go lower than -1 (or inactive)
            i = i - 3;            

            CheatAdditionalPilots = (int)System.Math.Max(1 + i, 0);
            // 0..8 extra pilots
            _limitActionsPerMinute = (int)System.Math.Max((i+1) * 60, 20);
            // 20..480 apm
            _limitActionsTickDelay = (60 * 4 / _limitActionsPerMinute);

            // <= 0 (will never cheat)
            _cheatCreditsChance = 0.025f * i; 
            // 0..17.5% chance to credit cheat!
            _cheatCreditsLastsTicks = (int)(100 * i);
            // 0..700 ticks of +3 credits per tick! (up to +$2.1k)

            _cheatVisibilityChance = 0.005f * i;
            // 0..3.5% chance to vision cheat!
            _cheatVisibilityLastsTicks = (int)(40 * i);
            // 0..280 ticks of free all vision! (up to 14s)
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
