using AllegianceForms.Engine.Ships;
using System;
using System.Drawing;

namespace AllegianceForms.Engine.Bases
{
    public class Base : GameUnit
    {
        public delegate void BaseEventHandler(Base sender, EBaseEventType e, int senderTeam);
        public event BaseEventHandler BaseEvent;

        public EBaseType Type { get; set; }
        public int Alliance { get; set; }
        public bool Selected { get; set; }
        public PointF BuildPosition { get; set; }
        public float ScanRange { get; set; }
                
        private int _offsetCount = 0;
        private PointF _lastBuildPos = Point.Empty;
        private PointF _nextBuildOffset = Point.Empty;
        private DateTime _offsetClear = DateTime.MinValue;
        private TimeSpan _offsetDelay = new TimeSpan(0, 0, 2);

        public Base(EBaseType type, int width, int height, Color teamColor, int team, int alliance, float health, int sectorId)
            : this(string.Empty, type, width, height, teamColor, team, alliance, health, sectorId)
        {
        }

        protected Base(string image, EBaseType type, int width, int height, Color teamColor, int team, int alliance, float health, int sectorId)
            : base(string.Empty, width, height, health, sectorId, team)
        {
            Type = type;
            Alliance = alliance;
            VisibleToTeam[team - 1] = true;
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
            return Type != EBaseType.Resource;
        }

        public bool CanGenerateIncome()
        {
            return Type == EBaseType.Resource;
        }

        public void Capture(Ship capturedBy)
        {
            Team = capturedBy.Team;
            Alliance = capturedBy.Alliance;
            OnBaseEvent(EBaseEventType.BaseCaptured, capturedBy.Team);
        } 

        public override void Update(int currentSectorId)
        {
            if (!Active) return;
            base.Update(currentSectorId);

            if (CanGenerateIncome())
            {
                StrategyGame.AddResources(Team, (int)(StrategyGame.ResourceRegularAmount * StrategyGame.GameSettings.ResourcesEachTickMultiplier), false);
            }

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

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active || !VisibleToTeam[0] || SectorId != currentSectorId) return;

            var b = BoundsI;
            var t = Team - 1;
            g.FillRectangle(StrategyGame.TeamBrushes[t], b);
            g.DrawRectangle(StrategyGame.BaseBorderPen, b);
            StrategyGame.DrawCenteredText(g, StrategyGame.TextBrushes[t], Type.ToString(), b);
            
            DrawHealthBar(g, t, b);

            if (Selected)
            {
                g.DrawRectangle(StrategyGame.SelectedPens[t], b.Left - 1, b.Top - 1, b.Width + 2, b.Height + 2);
                if (CanLaunchShips()) g.DrawLine(StrategyGame.SelectedPens[t], CenterX, CenterY, BuildPosition.X, BuildPosition.Y);
            }
        }

        public override void Damage(float amount, int senderTeam)
        {
            base.Damage(amount, senderTeam);

            if (!Active)
            {
                // Dead!
                OnBaseEvent(EBaseEventType.BaseDestroyed, senderTeam);
            }
            else if (senderTeam != Team)
            {
                OnBaseEvent(EBaseEventType.BaseDamaged, senderTeam);
            }
        }

        protected void OnBaseEvent(EBaseEventType e, int senderTeam)
        {
            if (BaseEvent != null) BaseEvent(this, e, senderTeam);
        }
    }
}
