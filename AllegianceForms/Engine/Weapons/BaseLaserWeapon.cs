using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public class BaseLaserWeapon : BaseWeapon
    {
        public Pen LaserPen { get; set; }
                
        public BaseLaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, int range, int damage, Ship shooter, Point offset)
            : base(fireTimeMS, refireDelayMS, range, damage, shooter, offset)
        {
            _weaponSound = ESounds.plasmamini1;
            LaserPen = new Pen(laserColor, laserWidth);
        }
        
        public override void Draw(Graphics g)
        {
            if (Shooting && Shooter != null && Target != null && Shooter.SectorId == Target.SectorId)
                g.DrawLine(LaserPen, Shooter.CenterX + FireOffset.X, Shooter.CenterY + FireOffset.Y, Target.CenterX, Target.CenterY);
        }

    }
}