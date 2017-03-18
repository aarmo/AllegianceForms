using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public class ShipLaserWeapon : ShipWeapon
    {
        public Pen LaserPen { get; set; }
        
        public ShipLaserWeapon(Color laserColor, float laserWidth, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset)
            : base(fireTicks, refireTicks, range, damage, shooter, offset)
        {
            LaserPen = new Pen(laserColor, laserWidth);
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (Shooting && Shooter != null && Target != null && Shooter.SectorId == currentSectorId)
                g.DrawLine(LaserPen, Shooter.CenterX + FireOffset.X, Shooter.CenterY + FireOffset.Y, Target.CenterX, Target.CenterY);
        }
    }
}
