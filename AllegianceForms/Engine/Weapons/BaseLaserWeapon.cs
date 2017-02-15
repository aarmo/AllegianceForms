using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Weapons
{
    public class BaseLaserWeapon : LaserWeapon
    {
        public BaseLaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, int range, int damage, Ship shooter)
            : base(laserColor, laserWidth, fireTimeMS, refireDelayMS, range, damage, shooter)
        {
            _weaponSound = ESounds.plasmamini1;
        }
        
        public BaseLaserWeapon(Color laserColor, float laserWidth, int fireTimeMS, int refireDelayMS, int range, int damage, Ship shooter, Point offset)
            : base(laserColor, laserWidth, fireTimeMS, refireDelayMS, range, damage, shooter, offset)
        {
            _weaponSound = ESounds.plasmamini1;
        }

        public override void DamageTarget()
        {
            var b = Target as Base;
            if (b != null && Shooter.SectorId == Target.SectorId)
            {
                b.Damage(WeaponDamage);
            }
        }

        public override void CheckForANewTarget()
        {
            if (Shooter == null || !Shooter.Active) return;
            var t = Target as Base;

            // Always be checking for targets in range and FIRE!
            if (t == null || !t.Active || t.SectorId != t.SectorId || !StrategyGame.WithinDistance(Shooter.CenterX, Shooter.CenterY, Target.CenterX, Target.CenterY, WeaponRange))
            {
                var enemyInRange = StrategyGame.AllBases.FirstOrDefault(_ => _.Active && _.Team != Shooter.Team && _.SectorId == Shooter.SectorId && _.VisibleToTeam[Shooter.Team-1] && StrategyGame.WithinDistance(Shooter.CenterX, Shooter.CenterY, _.CenterX, _.CenterY, WeaponRange));
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