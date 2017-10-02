using System.Drawing;
using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Engine.Weapons
{
    public class Minefield
    {
        public bool Active { get; set; }
        public RectangleF Bounds { get; set; }
        public int SectorId { get; set; }
        
        private float _expireTicks;

        public Image Image { get; set; }

        public Minefield(Ship shooter, PointF offset, float width, float duration, Image image)
        {
            Active = true;
            _expireTicks = duration;
            SectorId = shooter.SectorId;

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
