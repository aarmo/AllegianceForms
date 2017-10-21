using AllegianceForms.Engine.Ships;
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

        public ShipMissileWeapon(StrategyGame game, int width, float missileSpeed, float missileTracking, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset, SolidBrush teamColour) 
            : base(game, fireTicks, refireTicks, range, damage, shooter, offset)
        {
            WeaponSound = ESounds.sidewinder;
            Speed = missileSpeed;
            Tracking = missileTracking;
            _damageOnShotEnd = false;
            TeamColour = teamColour;
            Smoke1 = new Pen(Color.Orange, width/2);
            Smoke2 = new Pen(Color.Gray, 1);
            Width = width;
        }
        
        public override void Update()
        {
            if (!Shooting && Firing && _shootingNext <= 1 && Target != null)
            {
                var heading = (float)Utils.AngleBetweenPoints(Shooter.CenterPoint, Target.CenterPoint);
                var pos = new PointF(Shooter.CenterPoint.X + FireOffset.X, Shooter.CenterPoint.Y + FireOffset.Y);
                _game.Missiles.Add(new MissileProjectile(_game, Shooter.SectorId, Width, Speed, Tracking, heading, WeaponDamage, 60, pos, TeamColour, Smoke1, Smoke2, (Ship)Target, Shooter.Team, Shooter.Alliance));
            }
            base.Update();
        }

        public override void Draw(Graphics g, int currentSectorId)
        { }
    }
}
