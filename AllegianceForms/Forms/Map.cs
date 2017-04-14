using AllegianceForms.Engine;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class Map : Form
    {
        private readonly Bitmap _frame;
        private readonly StrategyGame _game;

        public Map(StrategyGame game)
        {
            InitializeComponent();
            _game = game;
            _frame = new Bitmap(Width, Height);
        }

        public void UpdateMap(int currentSectorId)
        {
            var g = Graphics.FromImage(_frame);
            g.Clear(BackColor);
            _game.Map.Draw(g, currentSectorId);

            Invalidate();
        }

        private void Research_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                Hide();
            }
        }

        private void Map_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (_frame != null) g.DrawImage(_frame, 0, 0);
        }

        private void Map_MouseClick(object sender, MouseEventArgs e)
        {
            var mousePos = PointToClient(MousePosition);

            foreach (var s in _game.Map.Sectors)
            {
                if (s.Bounds.Contains(mousePos))
                {
                    if (e.Button == MouseButtons.Left) _game.OnGameEvent(s, EGameEventType.SectorLeftClicked);
                    if (e.Button == MouseButtons.Right) _game.OnGameEvent(s, EGameEventType.SectorRightClicked);
                    return;
                }
            }
        }
    }
}
