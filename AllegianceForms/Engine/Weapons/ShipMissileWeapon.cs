using AllegianceForms.Engine.Ships;
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

        public ShipMissileWeapon(int width, float missileSpeed, float missileTracking, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset, SolidBrush teamColour) 
            : base(fireTicks, refireTicks, range, damage, shooter, offset)
        {
            _weaponSound = ESounds.sidewinder;
            Speed = missileSpeed;
            Tracking = missileTracking;
            _damageOnShotEnd = false;
            Missiles = new List<MissileProjectile>();
            TeamColour = teamColour;
            Smoke1 = new Pen(Color.Orange, width/2);
            Smoke2 = new Pen(Color.Gray, 1);
            Width = width;
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            foreach (var m in Missiles)
            {
                if (m.SectorId != currentSectorId) continue;

                m.Draw(g);
            }
        }

        public override void Update(int currentSectorId)
        {
            Missiles.RemoveAll(_ => !_.Active);

            if (!Shooting && Firing && _shootingNext <= 1 && Target != null)
            {
                var heading = (float) StrategyGame.AngleBetweenPoints(Shooter.CenterPoint, Target.CenterPoint);
                var pos = new PointF(Shooter.CenterPoint.X + FireOffset.X, Shooter.CenterPoint.Y + FireOffset.Y);
                Missiles.Add(new MissileProjectile(Shooter.SectorId, Width, Speed, Tracking, heading, WeaponDamage, 60, pos, TeamColour, Smoke1, Smoke2, (Ship)Target));
            }
            base.Update(currentSectorId);

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
