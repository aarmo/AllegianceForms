using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public class BaseLaserWeapon : BaseWeapon
    {
        public Pen LaserPen { get; set; }
                
        public BaseLaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, float range, float damage, Ship shooter, PointF offset)
            : base(fireTimeMS, refireDelayMS, range, damage, shooter, offset)
        {
            _weaponSound = ESounds.plasmamini1;
            LaserPen = new Pen(laserColor, laserWidth);
        }
        
        public override void Draw(Graphics g, int currentSectorId)
        {
            if (Shooting && Shooter != null && Target != null && Shooter.SectorId == currentSectorId)
                g.DrawLine(LaserPen, Shooter.CenterX + FireOffset.X, Shooter.CenterY + FireOffset.Y, Target.CenterX, Target.CenterY);
        }
    }
}