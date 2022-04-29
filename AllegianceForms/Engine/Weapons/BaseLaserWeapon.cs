using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public class BaseLaserWeapon : BaseWeapon
    {
        public Pen LaserPen { get; set; }
        public Pen LaserPenBoosted { get; set; }

        public BaseLaserWeapon(StrategyGame game, Color laserColor, float laserWidth, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset)
            : base(game, fireTicks, refireTicks, range, damage, shooter, offset)
        {
            WeaponSound = ESounds.plasmamini1;
            LaserPen = new Pen(laserColor, laserWidth);
            LaserPenBoosted = new Pen(laserColor.Lighten(0.2f), laserWidth * 1.5f);
        }
        
        public override void Draw(Graphics g, int currentSectorId, bool boosted)
        {
            if (Shooting && Shooter != null && Target != null && Shooter.SectorId == currentSectorId && Target.SectorId == Shooter.SectorId)
                g.DrawLine(boosted ? LaserPenBoosted : LaserPen, Shooter.CenterX + FireOffset.X, Shooter.CenterY + FireOffset.Y, Target.CenterX, Target.CenterY);
        }
    }
}