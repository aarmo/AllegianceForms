using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Rocks;
using AllegianceForms.Orders;
using System;
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
        public TimeSpan ShootingDuration { get; }
        public TimeSpan ShootingDelay { get; }
        public ResourceAsteroid Target { get; set; }

        public const int MineDistance = 100;
        public const int MineAmount = 10;
        public const int MaxResourceCapacity = 100;

        private DateTime _shootingStop = DateTime.MaxValue;
        private DateTime _shootingNextTime = DateTime.MinValue;
        private int _lastHealth = int.MinValue;
        private DateTime _nextHealthCheckTime = DateTime.MinValue;
        private TimeSpan _healthCheckDelay = new TimeSpan(0, 0, 1);
        private DateTime _callNext = DateTime.MinValue;

        public MinerShip(string imageFilename, int width, int height, Color teamColor, int team, int health, int sectorId)
            : base(imageFilename, width, height, teamColor, team, health, 0, sectorId)
        {
            Type = EShipType.Miner;
            Resources = 0;

            MinePen = new Pen(Brushes.CornflowerBlue, 2);
            ShootingDuration = new TimeSpan(0, 0, 0, 0, 1000);
            ShootingDelay = new TimeSpan(0, 0, 0, 0, 250);
            Shooting = false;
        }

        public override void Damage(int amount)
        {
            base.Damage(amount);

            if (Team == 1 && !Docked && Type == EShipType.Miner && Health < 0.5f * MaxHealth && DateTime.Now > _callNext)
            {
                SoundEffect.Play(ESounds.vo_sal_minercritical, true);
                OrderShip(new DockOrder(this));
                _callNext = DateTime.Now.AddSeconds(4);
            }

            if (Team == 1 && !Docked && DateTime.Now > _callNext)
            {
                SoundEffect.Play(ESounds.vo_miner_underattack, true);
                OrderShip(new DockOrder(this));
                _callNext = DateTime.Now.AddSeconds(4);
            }
        }

        public override void Update()
        {
            if (!Active) return;
            base.Update();

            if (!(CurrentOrder is MineOrder)) Mining = false;

            if (Target != null && !StrategyGame.WithinDistance(CenterX, CenterY, Target.CenterX, Target.CenterY, MineDistance))
            {
                Mining = false;
            }

            if (!Shooting && Mining && _shootingNextTime <= DateTime.Now && Target != null && Target.AvailableResources > 0)
            {
                Shooting = true;
                _shootingStop = DateTime.Now + ShootingDuration;
            }

            if (Shooting && _shootingStop <= DateTime.Now)
            {
                if (Target != null)
                {
                    Resources += Target.Mine(MineAmount);
                }
                Shooting = false;
                _shootingNextTime = DateTime.Now + ShootingDelay;
            }

            // If we are full, Dock then come back!
            if (Mining && Resources >= MaxResourceCapacity)
            {
                Mining = false;
                Target.BeingMined = false;
                Target = null;
                var backToWork = new MineOrder(CurrentOrder.OrderSectorId, CurrentOrder.OrderPosition, CurrentOrder.Offset);
                var refinery = StrategyGame.ClosestDistance<Base>(CenterX, CenterY, StrategyGame.AllBases.Where(_ => _.Active && _.Team == Team && _.SectorId == SectorId));

                if (refinery != null)
                {
                    OrderShip(new RefineOrder(this, refinery));
                    OrderShip(backToWork, true);
                }
                else
                {
                    Stop();
                    OrderShip(new DockOrder(this));
                }
            }

            if (_lastHealth == int.MinValue)
            {
                _lastHealth = Health;
                _nextHealthCheckTime = DateTime.Now + _healthCheckDelay;
            }
            else if (_nextHealthCheckTime < DateTime.Now)
            {
                if (Health < _lastHealth)
                {
                    _lastHealth = Health;
                    // Under attack - Dock now!

                    if (Team == 1) SoundEffect.Play(ESounds.vo_miner_dontgetpaid, true);
                    OrderShip(new DockOrder(this));

                    return;
                }
                _nextHealthCheckTime = DateTime.Now + _healthCheckDelay;
            }
        }

        public override void Draw(Graphics g)
        {
            if (!Active) return;
            base.Draw(g);

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
            StrategyGame.AddResources(Team, Resources);
            Resources = 0;
        }
    }
}
