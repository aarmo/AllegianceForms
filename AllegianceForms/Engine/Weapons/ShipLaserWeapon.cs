using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public class ShipLaserWeapon : ShipWeapon
    {
        public Pen LaserPen { get; set; }
        
        public ShipLaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, float range, float damage, Ship shooter, Point offset)
            : base(fireTimeMS, refireDelayMS, range, damage, shooter, offset)
        {
            LaserPen = new Pen(laserColor, laserWidth);
        }

        public override void Draw(Graphics g)
        {
            if (Shooting && Shooter != null && Target != null && Shooter.SectorId == Target.SectorId)
                g.DrawLine(LaserPen, Shooter.CenterX + FireOffset.X, Shooter.CenterY + FireOffset.Y, Target.CenterX, Target.CenterY);
        }
    }
}
