using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Rocks;
using AllegianceForms.Orders;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Ships
{
    public class MinerShip : Ship
    {
        public int Resources { get; set; }
        public bool Mining { get; set; }
        public bool Shooting { get; private set; }
        public Pen MinePen { get; set; }
        public int ShootingDurationTicks { get; }
        public int ShootingDelayTicks { get; }
        public ResourceAsteroid Target { get; set; }

        public const int MineDistance = 100;
        public const int MineAmount = 100;

        public int MaxResourceCapacity { get; set; }

        private int _shootingStop = int.MaxValue;
        private int _shootingNext = 0;
        private float _lastHealth = float.MinValue;
        private int _nextHealthCheck = 0;
        private int _healthCheckDelay = 20;
        private int _callNext = 80;

        public MinerShip(StrategyGame game, string imageFilename, int width, int height, Color teamColor, int team, int alliance, float health, int sectorId)
            : base(game, imageFilename, width, height, teamColor, team, alliance, health, 0, sectorId)
        {
            Type = EShipType.Miner;
            Resources = 0;

            MinePen = new Pen(Brushes.CornflowerBlue, 2);
            ShootingDurationTicks = 20;
            ShootingDelayTicks = 5;
            Shooting = false;

            MaxResourceCapacity = 1000;
        }

        public override void Damage(float amount, int senderTeam)
        {
            base.Damage(amount, senderTeam);

            if (!Docked && Type == EShipType.Miner && Health < 0.65f * MaxHealth && _callNext <= 0)
            {
                if (Team == 1) SoundEffect.Play(ESounds.vo_sal_minercritical, true);
                OrderShip(new DockOrder(_game, this));
                _callNext = 80;
            }

            if (!Docked && _callNext <= 0)
            {
                _callNext = 80;
                if (Team == 1)
                {
                    _game.OnGameEvent(new GameAlert(SectorId, $"{Type} under attack in {_game.Map.Sectors[SectorId]}!"), EGameEventType.ImportantMessage);
                    SoundEffect.Play(ESounds.vo_miner_underattack, true);
                }
            }
        }

        public override void Update()
        {
            if (!Active) return;
            base.Update();

            _shootingNext--;
            _shootingStop--;
            _nextHealthCheck--;
            _callNext--;

            if (!(CurrentOrder is MineOrder)) Mining = false;

            if (Target != null && !StrategyGame.WithinDistance(CenterX, CenterY, Target.CenterX, Target.CenterY, MineDistance))
            {
                Mining = false;
            }

            if (!Shooting && Mining && _shootingNext <= 0 && Target != null && Target.AvailableResources > 0)
            {
                Shooting = true;
                _shootingStop = ShootingDurationTicks;
            }

            if (Shooting && _shootingStop <= 0)
            {
                if (Target != null)
                {
                    Resources += Target.Mine(MineAmount);
                }
                Shooting = false;
                _shootingNext = ShootingDelayTicks;
            }

            // If we are full, Dock then come back!
            if (Mining && Resources >= MaxResourceCapacity)
            {
                Mining = false;
                Target.BeingMined = false;
                Target = null;
                var backToWork = new MineOrder(_game, CurrentOrder.OrderSectorId, CurrentOrder.OrderPosition, CurrentOrder.Offset);
                var refinery = StrategyGame.ClosestDistance<Base>(CenterX, CenterY, _game.AllBases.Where(_ => _.Active && _.Team == Team && _.SectorId == SectorId));

                if (refinery != null)
                {
                    OrderShip(new RefineOrder(_game, this, refinery));
                    OrderShip(backToWork, true);
                }
                else
                {
                    Stop();
                    OrderShip(new DockOrder(_game, this));
                }
            }

            if (_lastHealth == int.MinValue)
            {
                _lastHealth = Health;
                _nextHealthCheck = _healthCheckDelay;
            }
            else if (_nextHealthCheck <= 0)
            {
                if (Health < _lastHealth)
                {
                    _lastHealth = Health;

                    // Under attack - Dock now!
                    if (Team == 1 && _callNext <= 0)
                    {
                        _callNext = 80;
                        SoundEffect.Play(ESounds.vo_miner_dontgetpaid, true);
                    }

                    OrderShip(new DockOrder(_game, this));
                    return;
                }
                _nextHealthCheck = _healthCheckDelay;
            }
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active || SectorId != currentSectorId) return;
            base.Draw(g, currentSectorId);

            if (Shooting && Target != null && VisibleToTeam[0]) g.DrawLine(MinePen, _centerX, _centerY, Target.CenterX, Target.CenterY);
        }

        public override void Stop()
        {
            base.Stop();
            Mining = false;

            if (Target != null)
            {
                Target.BeingMined = false;
                Target = null;
            }
        }

        public override void Dock()
        {
            base.Dock();
            Refine();
        }

        public virtual void Refine()
        {
            _game.AddResources(Team, Resources);
            Resources = 0;
        }
    }
}
