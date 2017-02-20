using AllegianceForms.Engine.Ships;
using System;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Weapons
{
    public abstract class Weapon
    {
        public bool Firing { get; set; }
        public bool Shooting { get; protected set; }
        public TimeSpan ShootingDuration { get; protected set; }
        public TimeSpan ShootingDelay { get; protected set; }
        public float WeaponRange { get; set; }
        public float WeaponDamage { get; set; }
        public GameEntity Target { get; set; }
        public Ship Shooter { get; set; }
        public Point FireOffset { get; set; }

        protected bool _damageOnShotEnd = true;
        protected bool _playSoundAlways = true;
        protected ESounds _weaponSound = ESounds.plasmaac1;
        protected DateTime _shootingStop = DateTime.MaxValue;
        protected DateTime _shootingNextTime = DateTime.MinValue;
        
        protected Weapon(int fireTimeMS, int refireDelayMS, float range, float damage, Ship shooter, Point offset)
        {
            ShootingDuration = new TimeSpan(0, 0, 0, 0, fireTimeMS);
            ShootingDelay = new TimeSpan(0, 0, 0, 0, refireDelayMS);
            Shooting = false;
            Shooter = shooter;
            WeaponRange = range;
            WeaponDamage = damage;
            FireOffset = offset;
        }

        public virtual void Update()
        {
            if (!Shooting && Firing && _shootingNextTime <= DateTime.Now && Target != null && Target.Active)
            {
                Shooting = true;
                _shootingStop = DateTime.Now + ShootingDuration;
                if (_playSoundAlways || Shooter.Team == 1) SoundEffect.Play(_weaponSound);
            }

            if (Shooting && _shootingStop <= DateTime.Now)
            {
                if (_damageOnShotEnd) DamageTarget();
                Shooting = false;
                _shootingNextTime = DateTime.Now + ShootingDelay;
            }

            CheckForANewTarget();
        }

        public abstract void Draw(Graphics g);

        public abstract void DamageTarget();

        public abstract void CheckForANewTarget();

    }
}
