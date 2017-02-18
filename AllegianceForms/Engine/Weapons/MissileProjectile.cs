using AllegianceForms.Engine.Ships;
using System;
using System.Drawing;

namespace AllegianceForms.Engine.Weapons
{
    public class MissileProjectile
    {
        public float Heading { get; set; }
        public float Speed { get; set; }
        public float Tracking { get; set; }
        public int Width { get; set; }
        public bool Active { get; set; }

        public PointF Center { get; set; }
        public PointF PreviousPoint { get; set; }
        public PointF LastPoint { get; set; }

        public SolidBrush TeamColour { get; set; }
        public Pen SmokePen1 { get; set; }
        public Pen SmokePen2 { get; set; }
        public Ship Target { get; set; }

        public int Damage { get; set; }

        public RectangleF Bounds => new RectangleF(Center.X-Width/2, Center.Y-Width/2, Width, Width);

        private DateTime _expireTime;

        public MissileProjectile(int width, float speed, float tracking, float heading, int damage, int expireMS, PointF start, SolidBrush fill, Pen smoke1, Pen smoke2, Ship target)
        {
            Heading = heading;
            Speed = speed;
            Tracking = tracking;
            Damage = damage;
            Width = width;
            _expireTime = DateTime.Now.AddMilliseconds(expireMS);
            Center = PreviousPoint = LastPoint = start;
            TeamColour = fill;
            SmokePen1 = smoke1;
            SmokePen2 = smoke2;
            Active = true;
            Target = target;
        }

        public virtual void Draw(Graphics g)
        {
            var b = Bounds;
            g.DrawLine(SmokePen2, LastPoint, PreviousPoint);
            g.DrawLine(SmokePen1, Center, PreviousPoint);

            g.FillEllipse(TeamColour, b);
        }

        public virtual void Update()
        {
            if (!Active) return;
            if (DateTime.Now > _expireTime)
            {
                Active = false;
                return;
            }
            
            if (Tracking > 0 && Target != null && Target.Active)
            {
                var newHeading = (float)StrategyGame.AngleBetweenPoints(Center, Target.CenterPoint);

                // adjust heading to cover 180 to -180 (<-)
                var adjustHeading = (Heading < -90);
                var adjustNewHeading = (newHeading < -90);
                var angle = Heading + (adjustHeading ? 360 : 0);
                var newAngle = newHeading + (adjustNewHeading ? 360 : 0);
                var diff = angle - newAngle;
                if (diff < 0)
                {
                    angle = angle + Tracking;
                    if (angle > newAngle) angle = newAngle;
                }
                else if (diff > 0)
                {
                    angle = angle - Tracking;
                    if (angle < newAngle) angle = newAngle;
                }

                // restore heading
                Heading = (adjustHeading ? angle - 360 : angle);
            }

            // move towards the target
            var newPoint = StrategyGame.GetNewPoint(Center, Speed, Heading);
            LastPoint = PreviousPoint;
            PreviousPoint = Center;
            Center = newPoint;

            // check for collisions
            if (Target != null && Target.Active && Target.Bounds.Contains(Center))
            {
                Target.Damage(Damage);
                StrategyGame.OnGameEvent(Target, EGameEventType.MissileHit);
                Active = false;
            }
        }
    }
}
