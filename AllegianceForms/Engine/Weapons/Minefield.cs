using System.Drawing;
using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Engine.Weapons
{
    public class Minefield
    {
        public bool Active { get; set; }
        public RectangleF Bounds { get; set; }
        public int SectorId { get; set; }
        public int Alliance { get; set; }
        public int Team { get; set; }
        public float Damage { get; set; }

        private float _expireTicks;

        public Image Image { get; set; }


        public Minefield(Ship shooter, PointF offset, float width, float duration, Image image, float damage)
        {
            Active = true;
            Alliance = shooter.Alliance;
            Team = shooter.Team;
            Damage = damage;
            SectorId = shooter.SectorId;
            _expireTicks = duration;

            var center = new PointF(shooter.CenterPoint.X + offset.X, shooter.CenterPoint.Y + offset.Y);
            Bounds = new RectangleF(center.X - width / 2, center.Y - width / 2, width, width);

            Image = image;
        }

        public virtual void Draw(Graphics g, int currentSectorId)
        {
            g.DrawImage(Image, Bounds);
        }

        public virtual void Update()
        {
            if (_expireTicks <= 0)
            {
                Active = false;
                return;
            }
            _expireTicks--;
        }
    }
}
