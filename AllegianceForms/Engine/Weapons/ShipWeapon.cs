using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public abstract class ShipWeapon : Weapon
    {
        protected ShipWeapon(StrategyGame game, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset)
            : base(game, fireTicks, refireTicks, range, damage, shooter, offset)
        {
        }

        public override void DamageTarget()
        {
            var ship = Target as Ship;
            if (ship != null && Shooter.SectorId == Target.SectorId)
            {
                ship.Damage(WeaponDamage, Shooter.Team);
                if (!ship.Active) Target = null;
            }
        }

        public override void CheckForANewTarget()
        {
            // Always be checking for targets in range and FIRE!
            if (Shooter == null || !Shooter.Active || Shooting) return;

            var t = Target as Ship;
            var skipVis = Shooter.Team < 0;

            if (t == null || !t.Active || t.SectorId != Shooter.SectorId || t.Docked || t.Alliance == Shooter.Alliance || !t.IsVisibleToTeam(Shooter.Team - 1) || !Utils.WithinDistance(Shooter.CenterX, Shooter.CenterY, Target.CenterX, Target.CenterY, WeaponRange))
            {
                Target = _game.GetRandomEnemyInRange(Shooter.Team, Shooter.Alliance, Shooter.SectorId, Shooter.CenterPoint, WeaponRange);
                Firing = Target != null;
            }
        }
    }
}