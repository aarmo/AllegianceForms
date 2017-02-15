using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Weapons
{
    public class ShipLaserWeapon : LaserWeapon
    {
        public ShipLaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, int range, int damage, Ship shooter)
            : base(laserColor, laserWidth, fireTimeMS, refireDelayMS, range, damage, shooter)
        {
        }

        public ShipLaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, int range, int damage, Ship shooter, Point offset)
            : base(laserColor, laserWidth, fireTimeMS, refireDelayMS, range, damage, shooter, offset)
        {
        }

        public override void DamageTarget()
        {
            var ship = Target as Ship;
            if (ship != null && Shooter.SectorId == Target.SectorId)
            {
                ship.Damage(WeaponDamage);
                if (!ship.Active) Target = null;
            }
        }

        public override void CheckForANewTarget()
        {
            // Always be checking for targets in range and FIRE!
            if (Shooter == null || !Shooter.Active || Shooting) return;

            var t = Target as Ship;
            if (t == null || !t.Active || t.SectorId != Shooter.SectorId || t.Docked || !StrategyGame.WithinDistance(Shooter.CenterX, Shooter.CenterY, Target.CenterX, Target.CenterY, WeaponRange))
            {
                var enemyInRange = StrategyGame.AllUnits.FirstOrDefault(_ => _.Active && _.Team != Shooter.Team && !_.Docked && Shooter.SectorId == _.SectorId && _.VisibleToTeam[Shooter.Team - 1] && _.Type != EShipType.Lifepod && StrategyGame.WithinDistance(Shooter.CenterX, Shooter.CenterY, _.CenterX, _.CenterY, WeaponRange));
                if (enemyInRange != null)
                {
                    Target = enemyInRange;
                    Firing = true;
                }
                else
                {
                    Target = null;
                    Firing = false;
                }
            }
        }
    }
}