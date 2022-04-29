using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public abstract class Weapon
    {
        public bool Firing { get; set; }
        public bool Shooting { get; protected set; }
        public int ShootingTicks { get; protected set; }
        public int ShootingDelayTicks { get; protected set; }
        public float WeaponRange { get; set; }
        public float WeaponDamage { get; set; }
        public GameEntity Target { get; set; }
        public Ship Shooter { get; set; }
        public PointF FireOffset { get; set; }
        public ESounds WeaponSound { get; set; }

        protected bool _damageOnShotEnd = true;
        protected int _shootingStop = int.MaxValue;
        protected int _shootingNext = 0;
        protected StrategyGame _game;
                
        protected Weapon(StrategyGame game, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset)
        {
            ShootingTicks = _shootingStop = fireTicks;
            ShootingDelayTicks = refireTicks;
            Shooting = false;
            Shooter = shooter;
            WeaponRange = range;
            WeaponDamage = damage;
            FireOffset = offset;
            WeaponSound = ESounds.plasmaac1;
            _game = game;
        }

        public virtual void Update(float boostedAmount)
        {
            _shootingStop--;
            _shootingNext--;

            if (!Shooting && Firing && _shootingNext <= 0 && Target != null && Target.Active)
            {
                Shooting = true;
                _shootingStop = ShootingTicks;
                if (Shooter.SectorId == _game.PlayerCurrentSectorId) SoundEffect.Play(WeaponSound);
            }

            if (Shooting && _shootingStop <= 0)
            {
                if (_damageOnShotEnd) DamageTarget(boostedAmount);
                Shooting = false;
                _shootingNext = ShootingDelayTicks;
            }

            CheckForANewTarget();
        }

        public abstract void Draw(Graphics g, int currentSectorId, bool boosted);

        public abstract void DamageTarget(float boostedAmount = 1f);

        public abstract void CheckForANewTarget();

    }
}
