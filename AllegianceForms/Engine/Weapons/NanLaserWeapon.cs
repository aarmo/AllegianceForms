using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Weapons
{
    public class NanLaserWeapon : ShipLaserWeapon
    {
        public NanLaserWeapon(StrategyGame game, float laserWidth, int fireTicks, int refireTicks, float range, float healing, Ship shooter, PointF offset)
            : base(game, Color.Aqua, laserWidth, fireTicks, refireTicks, range, healing, shooter, offset)
        {
            WeaponSound = ESounds.sniperlaser1pwrup;
        }

        public override void CheckForANewTarget()
        {
            // Always check for friendly targets in range and FIRE!
            if (Shooter == null || !Shooter.Active || Shooting) return;

            var t = Target as Ship;
            if (t == null || !t.Active || t.SectorId != Shooter.SectorId || t.Docked || t.Health == t.MaxHealth || t.Alliance != Shooter.Alliance || !Utils.WithinDistance(Shooter.CenterX, Shooter.CenterY, Target.CenterX, Target.CenterY, WeaponRange))
            {
                var friendsInRange = _game.AllUnits.Where(_ => _.Active && _.Alliance == Shooter.Alliance && !_.Docked && Shooter.SectorId == _.SectorId && _.Health < _.MaxHealth && _ != Shooter && _.Type != EShipType.Lifepod && Utils.WithinDistance(Shooter.CenterX, Shooter.CenterY, _.CenterX, _.CenterY, WeaponRange)).ToList();
                if (friendsInRange.Count > 1)
                {
                    Target = friendsInRange[StrategyGame.Random.Next(friendsInRange.Count)];
                    Firing = true;
                }
                else if (friendsInRange.Count == 1)
                {
                    Target = friendsInRange[0];
                    Firing = true;
                }
                else
                {
                    Target = _game.AllBases.FirstOrDefault(_ => _.Active && _.Alliance == Shooter.Alliance && _.SectorId == Shooter.SectorId && _.Health < _.MaxHealth && Utils.WithinDistance(Shooter.CenterX, Shooter.CenterY, _.CenterX, _.CenterY, WeaponRange));
                    Firing = Target != null;
                }
            }
        }
    }
}