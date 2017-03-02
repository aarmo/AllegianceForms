using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Weapons
{
    public abstract class BaseWeapon : Weapon
    {
        protected BaseWeapon(int fireTimeMS, int refireDelayMS, float range, float damage, Ship shooter, PointF offset)
            : base(fireTimeMS, refireDelayMS, range, damage, shooter, offset)
        {
        }

        public override void DamageTarget()
        {
            var b = Target as Base;
            if (b != null && b.Active && Shooter.SectorId == Target.SectorId)
            {
                b.Damage(WeaponDamage, Shooter.Team);
                if (!b.Active) Target = null;
            }
        }

        public override void CheckForANewTarget()
        {
            if (Shooter == null || !Shooter.Active) return;
            var t = Target as Base;

            // Always be checking for targets in range and FIRE!
            if (t == null || !t.Active || t.SectorId != t.SectorId || t.Team == Shooter.Team || !StrategyGame.WithinDistance(Shooter.CenterX, Shooter.CenterY, Target.CenterX, Target.CenterY, WeaponRange))
            {
                var enemyInRange = StrategyGame.AllBases.FirstOrDefault(_ => _.Active && _.Team != Shooter.Team && _.SectorId == Shooter.SectorId && _.VisibleToTeam[Shooter.Team - 1] && StrategyGame.WithinDistance(Shooter.CenterX, Shooter.CenterY, _.CenterX, _.CenterY, WeaponRange));
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