using System.Drawing;

namespace AllegianceForms.Engine
{
    public class GameEntity
    {
        protected bool[] VisibleToTeam { get; set; }
        public float Signature { get; set; } 
        public int SectorId { get; set; }
        public string Name { get; set; }
        public Brush TextBrush { get; set; }
        public int TextOffsetY { get; set; }
        public bool Active { get; set; }
        public Image Image { get; set; }
        public float Top
        {
            get { return _top; }
            set
            {
                _top = value;
                _centerY = value + _halfHeight;
                _topLeft.Y = (int)_top;
            }
        }
        public float CenterY
        {
            get { return _centerY; }
            set
            {
                _centerY = value;
                _top = value - _halfHeight;
                _topLeft.Y = (int)_top;
            }
        }
        public float Left
        {
            get { return _left; }
            set
            {
                _left = value;
                _centerX = value + _halfWidth;
                _topLeft.X = (int)_left;
            }
        }
        public float CenterX
        {
            get { return _centerX; }
            set
            {
                _centerX = value;
                _left = value - _halfWidth;
                _topLeft.X = (int)_left;
            }
        }
        public RectangleF Bounds => new RectangleF(_left, _top, _halfWidth * 2.0f, _halfHeight * 2.0f);
        public Rectangle BoundsI => new Rectangle((int)_left, (int)_top, (int)(_halfWidth * 2.0f), (int)(_halfHeight * 2.0f));
        public PointF CenterPoint => new PointF(_centerX, _centerY);
        public Point CenterPointI => new Point((int)_centerX, (int)_centerY);

        protected float _top;
        protected float _left;
        protected PointF _topLeft;
        protected float _centerX;
        protected float _centerY;
        protected readonly float _halfWidth;
        protected readonly float _halfHeight;
        protected StrategyGame _game;

        public GameEntity(StrategyGame game, string imageFilename, int width, int height, int sectorId)
        {
            Active = true;
            Signature = 1.0f; // 100%
            SectorId = sectorId;

            if (imageFilename != string.Empty)
            {
                var i = Image.FromFile(imageFilename);
                var bmp = new Bitmap(i, width, height);
                Image = bmp;
            }
            _halfWidth = width / 2.0f;
            _halfHeight = height / 2.0f;
            _game = game;
            TextOffsetY = -35;
            VisibleToTeam = new bool[game.NumTeams];

            Top = 0;
            Left = 0;
        }

        public bool IsVisibleToTeam(int t)
        {
            if (t < 0 || t >= _game.NumTeams) return true;

            return VisibleToTeam[t];
        }

        public void SetVisibleToTeam(int t, bool v)
        {
            if (t < 0 || t >= _game.NumTeams) return;
            VisibleToTeam[t] = v;
        }

        public virtual void Draw(Graphics g, int currentSectorId)
        {
            if (!Active || !VisibleToTeam[0] || SectorId != currentSectorId) return;
            g.DrawImage(Image, _topLeft);

            if (TextBrush != null)
            {
                var rect = BoundsI;
                rect.Width += 100;
                rect.Offset(-50, TextOffsetY);
                Utils.DrawCenteredText(g, TextBrush, Name, rect);
            }
        }
    }

    public class GameUnit : GameEntity
    {
        public int UnitId { get; protected set; }
        public int Team { get; protected set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Shield { get; set; }
        public float MaxShield { get; set; }
        public float ShieldRecharge { get; set; }

        protected Brush _healthBrush;

        public GameUnit(StrategyGame game, string imageFilename, int width, int height, float health, int sectorId, int team) : base(game, imageFilename, width, height, sectorId)
        {
            MaxHealth = Health = MaxShield = Shield = health;
            Team = team;
            UnitId = ++game.LastUnitId;

            var t = team - 1;
            if (t < 0)
            {
                _healthBrush = Brushes.DarkGreen;                
            }
            else
            {
                var research = game.TechTree[t].ResearchedUpgrades;
                MaxShield *= research[EGlobalUpgrade.MaxShield];
                ShieldRecharge = 0.1f * research[EGlobalUpgrade.ShieldRecharge];
                _healthBrush = _game.TeamBrushes[t];
            }
        }

        public virtual void Update()
        {
            if (!Active) return;
            if (Shield < MaxShield)
            {
                Shield += ShieldRecharge;
            }
        }

        protected virtual void DrawHealthBar(Graphics g, Rectangle b)
        {
            g.FillRectangle(StrategyGame.ShieldBrush, b.Left, b.Bottom + 3, (Shield / MaxShield) * b.Width, 3);
            g.FillRectangle(_healthBrush, b.Left, b.Bottom + 6, (Health / MaxHealth) * b.Width, 3);
            g.DrawRectangle(StrategyGame.HealthBorderPen, b.Left, b.Bottom + 3, b.Width, 6);
        }

        public virtual void Damage(float amount, Weapons.Weapon source)
        {
            if (!Active) return;

            if (amount > 0)
            {
                // Lasers do bonus shield damage
                var shieldDamage = amount;
                if (source is Weapons.BaseLaserWeapon || source is Weapons.ShipLaserWeapon) shieldDamage *= GameSettings.LaserShieldDamageMultiplier;

                if (Shield - shieldDamage < 0)
                {
                    amount -= Shield;
                    Shield = 0;
                }
                else
                {
                    Shield -= shieldDamage;
                    return;
                }

                // Missiles do bonus hull damage
                var hullDamage = amount;
                if (source is Weapons.ShipMissileWeapon) hullDamage *= GameSettings.MissileHullDamageMultiplier;

                if (Health - hullDamage <= 0)
                {
                    // Dead!
                    Health = 0;
                    Active = false;
                }
                else
                {
                    Health -= hullDamage;
                }
            }
            else
            {
                // Healing
                if (Health - amount >= MaxHealth)
                {
                    Health = MaxHealth;
                }
                else
                {
                    Health -= amount;
                }
            }
        }
    }
}