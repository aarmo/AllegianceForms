using System;
using System.Drawing;
using AllegianceForms.Engine.Ships;
using System.Collections.Generic;

namespace AllegianceForms.Engine.Weapons
{
    public class MineWeapon : ShipWeapon
    {
        public float Width { get; set; }
        public float Duration { get; set; }
        public Color Colour { get; set; }

        public List<Minefield> Minefields { get; set; }
        
        public Image Image { get; set; }
        private const string MinefieldImage = ".\\Art\\minefield.png";

        //(StrategyGame game, float laserWidth, int fireTicks, int refireTicks, float range, float healing, Ship shooter, PointF offset)
        public MineWeapon(StrategyGame game, float width, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset, Color teamColour)
            : base(game, 2, refireTicks, range, damage, shooter, offset)
        {
            // FireTicks => Duration
            Duration = fireTicks;
            Width = width;
            Colour = teamColour;

            _damageOnShotEnd = false;
            WeaponSound = ESounds.dropmine;
            Minefields = new List<Minefield>();
            
            var i = Image.FromFile(MinefieldImage);
            var bmp = new Bitmap(i, (int)Width, (int)Width);
            Image = bmp;
            Utils.ReplaceColour(bmp, Colour);
        }

        public override void Update()
        {
            base.Update();
            Minefields.RemoveAll(_ => !_.Active);

            // Create a minefield here.
            if (Shooting)
            {
                Minefields.Add(new Minefield(Shooter, FireOffset, Width, Duration, Image));
            }

            // Apply damage to all ships within the bounds
            foreach (var m in Minefields)
            {
                m.Update();

                var hits = _game.AllUnits.FindAll(_ => _.Active && _.Type != EShipType.Lifepod && m.SectorId == _.SectorId && _.Alliance != Shooter.Alliance && m.Bounds.Contains(_.Bounds));
                hits.ForEach(_ => _.Damage(WeaponDamage / 2f, Shooter.Team));
            }
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            foreach (var m in Minefields)
            {
                if (!m.Active || m.SectorId != currentSectorId) continue;

                m.Draw(g, currentSectorId);
            }
        }
    }
}
