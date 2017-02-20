using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Weapons
{
    public class NanLaserWeapon : ShipLaserWeapon
    {
        public NanLaserWeapon(float laserWidth, int fireTimeMS, int refireDelayMS, float range, float healing, Ship shooter, Point offset)
            : base(Color.Aqua, laserWidth, fireTimeMS, refireDelayMS, range, healing, shooter, offset)
        {
            _weaponSound = ESounds.sniperlaser1pwrup;
        }

        public override void CheckForANewTarget()
        {
            // Always be checking for friendly targets in range and FIRE!
            if (Shooter == null || !Shooter.Active || Shooting) return;

            var t = Target as Ship;
            if (t == null || !t.Active || t.SectorId != Shooter.SectorId || t.Docked || t.Health == t.MaxHealth || !StrategyGame.WithinDistance(Shooter.CenterX, Shooter.CenterY, Target.CenterX, Target.CenterY, WeaponRange))
            {
                var friendInRange = StrategyGame.AllUnits.FirstOrDefault(_ => _.Active && _.Team == Shooter.Team && !_.Docked && Shooter.SectorId == _.SectorId && _.Health < _.MaxHealth && _ != Shooter && _.Type != EShipType.Lifepod && StrategyGame.WithinDistance(Shooter.CenterX, Shooter.CenterY, _.CenterX, _.CenterY, WeaponRange));
                if (friendInRange != null)
                {
                    Target = friendInRange;
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