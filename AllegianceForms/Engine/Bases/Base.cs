using AllegianceForms.Engine.Ships;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AllegianceForms.Engine.Bases
{
    public class Base : GameEntity
    {
        public delegate void BaseEventHandler(Base sender, EBaseEventType e, int senderTeam);
        public event BaseEventHandler BaseEvent;

        public EBaseType Type { get; set; }
        public int Team { get; set; }
        public int Alliance { get; set; }
        public Brush TeamColor { get; set; }
        public bool Selected { get; set; }
        public Pen SelectedPen { get; set; }
        public Pen BorderPen { get; set; }
        public float Health { get; set; }
        public PointF BuildPosition { get; set; }
        public float ScanRange { get; set; }

        private float _maxHealth;
        private int _offsetCount = 0;
        private PointF _lastBuildPos = Point.Empty;
        private PointF _nextBuildOffset = Point.Empty;
        private DateTime _offsetClear = DateTime.MinValue;
        private TimeSpan _offsetDelay = new TimeSpan(0, 0, 2);
        private Brush _textColor;

        public Base(EBaseType type, int width, int height, Color teamColor, int team, int alliance, float health, int sectorId)
            : this(string.Empty, type, width, height, teamColor, team, alliance, health, sectorId)
        {
        }

        protected Base(string image, EBaseType type, int width, int height, Color teamColor, int team, int alliance, float health, int sectorId)
            : base(string.Empty, width, height, sectorId)
        {
            Type = type;
            _maxHealth = Health = health;
            Team = team;
            Alliance = alliance;
            VisibleToTeam[team - 1] = true;
            TeamColor = new SolidBrush(teamColor);
            _textColor = new SolidBrush(PerceivedBrightness(teamColor) > 130 ? Color.Black : Color.White);
            SelectedPen = new Pen(teamColor, 1) { DashStyle = DashStyle.Dot };
            BorderPen = new Pen(Color.Gray, 2);
            ScanRange = 500;
        }

        public PointF GetNextBuildPosition()
        {            
            if (_lastBuildPos != BuildPosition)
            {
                _nextBuildOffset = PointF.Empty;
                _offsetCount = 0;
                _lastBuildPos = BuildPosition;
            }

            _offsetClear = DateTime.Now + _offsetDelay;
            _offsetCount++;
            var nextPos = new PointF(BuildPosition.X + _nextBuildOffset.X, BuildPosition.Y + _nextBuildOffset.Y);

            if (_offsetCount % 4 == 0)
            {
                _nextBuildOffset.X = 0;
                _nextBuildOffset.Y += 24;
            }
            else
            {
                _nextBuildOffset.X += 24;

            }
            return nextPos;
        }

        public bool CanLaunchShips()
        {
            return Type != EBaseType.Refinery;
        }

        public void Capture(Ship capturedBy)
        {
            Team = capturedBy.Team;
            Alliance = capturedBy.Alliance;
            TeamColor = (Brush) capturedBy.TeamColor.Clone();
            SelectedPen = (Pen) capturedBy.SelectedPen.Clone();
            OnBaseEvent(EBaseEventType.BaseCaptured, capturedBy.Team);
        } 

        public virtual void Update(int currentSectorId)
        {
            if (!CanLaunchShips()) return;

            if (BuildPosition == Point.Empty)
            {
                var p = CenterPoint;
                _lastBuildPos = BuildPosition = new PointF(p.X + 100, p.Y);
            }

            if (DateTime.Now > _offsetClear)
            {
                _nextBuildOffset = Point.Empty;
                _offsetCount = 0;
                _offsetClear = DateTime.MaxValue;
            }
        }

        private int PerceivedBrightness(Color c)
        {
            return (int)Math.Sqrt(
            c.R * c.R * .299 +
            c.G * c.G * .587 +
            c.B * c.B * .114);
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active || !VisibleToTeam[0] || SectorId != currentSectorId) return;

            var b = BoundsI;
            g.FillRectangle(TeamColor, b);
            g.DrawRectangle(BorderPen, b);

            var rect = BoundsI;
            StrategyGame.DrawCenteredText(g, _textColor, Type.ToString(), rect);

            if (Selected)
            {
                g.DrawRectangle(SelectedPen, b.Left - 1, b.Top - 1, b.Width + 2, b.Height + 2);

                if (CanLaunchShips()) g.DrawLine(SelectedPen, CenterX, CenterY, BuildPosition.X, BuildPosition.Y);
            }
        }

        public virtual void Damage(float amount, int senderTeam)
        {
            if (Health - amount <= 0)
            {
                // Dead!
                Active = false;
                OnBaseEvent(EBaseEventType.BaseDestroyed, senderTeam);
            }
            else
            {
                Health -= amount;
                OnBaseEvent(EBaseEventType.BaseDamaged, senderTeam);
            }
        }

        protected void OnBaseEvent(EBaseEventType e, int senderTeam)
        {
            if (BaseEvent != null) BaseEvent(this, e, senderTeam);
        }
    }
}
