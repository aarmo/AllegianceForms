using System.Drawing;

namespace AllegianceForms.Engine
{
    public class GameEntity
    {
        public bool[] VisibleToTeam { get; set; }
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

        public GameEntity(string imageFilename, int width, int height, int sectorId)
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
            TextOffsetY = -35;
            VisibleToTeam = new bool[StrategyGame.NumTeams];

            Top = 0;
            Left = 0;
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
                StrategyGame.DrawCenteredText(g, TextBrush, Name, rect);
            }
        }
    }
}
