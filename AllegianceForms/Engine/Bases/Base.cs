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

        public Base(StrategyGame game, EBaseType type, int width, int height, Color teamColor, int team, int alliance, float health, int sectorId)
            : this(game, string.Empty, type, width, height, teamColor, team, alliance, health, sectorId)
        {
        }

        protected Base(StrategyGame game, string image, EBaseType type, int width, int height, Color teamColor, int team, int alliance, float health, int sectorId)
            : base(game, string.Empty, width, height, health, sectorId, team)
        {
            Type = type;
            Alliance = alliance;
            if (team > 0) VisibleToTeam[team - 1] = true;
            ScanRange = 500;
        }

        public bool CanBeCaptured()
        {
            return Shield <= MaxShield / 100f;
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
        
        protected const int LimitResourcesTickDelay = 4;
        protected int _nextResourcesAllowed = LimitResourcesTickDelay;

        public bool CanGenerateIncome()
        {
            if (Type == EBaseType.Resource)
            {
                _nextResourcesAllowed--;
                if (_nextResourcesAllowed<=0)
                {
                    _nextResourcesAllowed = LimitResourcesTickDelay;
                    return true;
                }
            }
            return false;
        }

        public void Capture(Ship capturedBy)
        {
            _game.Bases.CaptureBase(Type, Team, capturedBy.Team);

            Team = capturedBy.Team;
            Alliance = capturedBy.Alliance;

            _healthBrush = _game.TeamBrushes[Team - 1];

            OnBaseEvent(EBaseEventType.BaseCaptured, capturedBy.Team);
        } 

        public override void Update()
        {
            if (!Active) return;
            base.Update();

            if (CanGenerateIncome())
            {
                _game.AddResources(Team, (int)(StrategyGame.ResourceRegularAmount * _game.GameSettings.ResourcesEachTickMultiplier), false);
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
            var br = (Team > 0) ? _game.TeamBrushes[t] : StrategyGame.AlienBrush;
            var tbr = (Team > 0) ? _game.TextBrushes[t] : Brushes.Black;

            g.FillRectangle(br, b);
            g.DrawRectangle(StrategyGame.BaseBorderPen, b);
            Utils.DrawCenteredText(g, tbr, Type.ToString(), b);
            
            DrawHealthBar(g, b);

            if (t == 0 && Selected)
            {
                g.DrawRectangle(_game.SelectedPens[t], b.Left - 1, b.Top - 1, b.Width + 2, b.Height + 2);
                if (CanLaunchShips()) g.DrawLine(_game.SelectedPens[t], CenterX, CenterY, BuildPosition.X, BuildPosition.Y);
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

        public bool IsTechBase()
        {
            return Type == EBaseType.Shipyard 
                || Type == EBaseType.Supremacy 
                || Type == EBaseType.Expansion 
                || Type == EBaseType.Tactical;
        }
    }
}
