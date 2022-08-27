using System.Drawing;
using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Engine.Weapons
{
    public class MineWeapon : ShipWeapon
    {
        public float Width { get; set; }
        public float Duration { get; set; }
        public Color Colour { get; set; }
        
        public Image Image { get; set; }
        public const string MinefieldImage = ".\\Art\\minefield.png";

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
            
            var i = Image.FromFile(MinefieldImage);
            var bmp = new Bitmap(i, (int)Width, (int)Width);
            Image = bmp;
            Utils.ReplaceColour(bmp, Colour);
        }

        public override void Update(float boostedAmount)
        {
            base.Update(boostedAmount);

            // Create a minefield here.
            if (Shooting)
            {
                _game.DropMinefield(Shooter, Duration, 1f);
            }
        }

        public override void Draw(Graphics g, int currentSectorId, bool boosted)
        {
        }
    }
}
