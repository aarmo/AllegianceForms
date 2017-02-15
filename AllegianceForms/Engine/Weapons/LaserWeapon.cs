using AllegianceForms.Engine.Ships;
using System;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public abstract class LaserWeapon
    {
        public Pen LaserPen { get; set; }
        public bool Firing { get; set; }
        public bool Shooting { get; private set; }
        public TimeSpan ShootingDuration { get; }
        public TimeSpan ShootingDelay { get; }
        public int WeaponRange { get; set; }
        public int WeaponDamage { get; set; }
        public GameEntity Target { get; set; }
        public Ship Shooter { get; set; }
        public Point FireOffset { get; set; }

        protected ESounds _weaponSound = ESounds.plasmaac1;
        private DateTime _shootingStop = DateTime.MaxValue;
        private DateTime _shootingNextTime = DateTime.MinValue;

        protected LaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, int range, int damage, Ship shooter)
            : this (laserColor, laserWidth, fireTimeMS, refireDelayMS, range, damage, shooter, Point.Empty)
        {            
        }

        protected LaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, int range, int damage, Ship shooter, Point offset)
        {
            LaserPen = new Pen(laserColor, laserWidth);
            ShootingDuration = new TimeSpan(0, 0, 0, 0, fireTimeMS);
            ShootingDelay = new TimeSpan(0, 0, 0, 0, refireDelayMS);
            Shooting = false;
            Shooter = shooter;
            WeaponRange = range;
            WeaponDamage = damage;
            FireOffset = offset;
        }

        public LaserWeapon ShallowCopy()
        {
            return (LaserWeapon)MemberwiseClone();
        }

        public virtual void Update()
        {
            if (!Shooting && Firing && _shootingNextTime <= DateTime.Now && Target != null && Target.Active)
            {
                Shooting = true;
                _shootingStop = DateTime.Now + ShootingDuration;
                SoundEffect.Play(_weaponSound);
            }

            if (Shooting && _shootingStop <= DateTime.Now)
            {
                DamageTarget();
                Shooting = false;
                _shootingNextTime = DateTime.Now + ShootingDelay;
            }

            CheckForANewTarget();
        }

        public virtual void Draw(Graphics g)
        {
            if (Shooting && Shooter != null && Target != null && Shooter.SectorId == Target.SectorId) g.DrawLine(LaserPen, Shooter.CenterX + FireOffset.X, Shooter.CenterY + FireOffset.Y, Target.CenterX, Target.CenterY);
        }

        public abstract void DamageTarget();
        public abstract void CheckForANewTarget();
    }
}
