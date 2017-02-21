using AllegianceForms.Engine.Ships;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public class ShipMissileWeapon : ShipWeapon
    {
        public float Speed { get; set; }
        public float Tracking { get; set; }
        public SolidBrush TeamColour { get; set; }
        public int Width { get; set; }
        public Pen Smoke1 { get; set; }
        public Pen Smoke2 { get; set; }
        public List<MissileProjectile> Missiles { get; set; }

        public ShipMissileWeapon(int width, float missileSpeed, float missileTracking, int fireTimeMS, int refireDelayMS, float range, float damage, Ship shooter, PointF offset, SolidBrush teamColour) 
            : base(fireTimeMS, refireDelayMS, range, damage, shooter, offset)
        {
            _weaponSound = ESounds.missilelock;
            Speed = missileSpeed;
            Tracking = missileTracking;
            _damageOnShotEnd = false;
            _playSoundAlways = false;
            Missiles = new List<MissileProjectile>();
            TeamColour = teamColour;
            Smoke1 = new Pen(Color.Orange, width/2);
            Smoke2 = new Pen(Color.Gray, 1);
            Width = width;
        }

        public override void Draw(Graphics g)
        {
            foreach (var m in Missiles)
            {
                m.Draw(g);
            }
        }

        public override void Update()
        {
            Missiles.RemoveAll(_ => !_.Active);

            if (!Shooting && Firing && _shootingNextTime <= DateTime.Now && Target != null)
            {
                var heading = (float) StrategyGame.AngleBetweenPoints(Shooter.CenterPoint, Target.CenterPoint);
                var pos = new PointF(Shooter.CenterPoint.X + FireOffset.X, Shooter.CenterPoint.Y + FireOffset.Y);
                Missiles.Add(new MissileProjectile(Width, Speed, Tracking, heading, WeaponDamage, 3000, pos, TeamColour, Smoke1, Smoke2, (Ship)Target));
            }
            base.Update();

            foreach (var m in Missiles)
            {
                if (m.Target == null || !m.Target.Active)
                {
                    m.Target = (Ship)Target;
                }

                m.Update();
            }
        }
    }
}
