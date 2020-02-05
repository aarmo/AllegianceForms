using AllegianceForms.Engine;
using AllegianceForms.Engine.Factions;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class Menu : Form
    {
        private Sector _gamescreen;
        private GameSettings _customSettings = GameSettings.Default();
        private StarField _starfield = new StarField();

        public Menu()
        {
            InitializeComponent();
            SoundEffect.Init();
            SoundEffect.Play(ESounds.windowslides);

            AppVersion.Text = string.Format("(ALPHA) v{0}", Assembly.GetEntryAssembly().GetName().Version);
            
            _starfield.Init(Width, Height);
            animateStars.Enabled = false;
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            animateStars.Enabled = true;
            SoundEffect.Play(ESounds.mouseover);
            var b = sender as Button;
            if (b != null) b.BackColor = Color.DarkGreen;
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            animateStars.Enabled = false;
            var b = sender as Button;
            if (b != null) b.BackColor = Color.Black;
        }

        private void CustomGame_Click(object sender, EventArgs e)
        {
            Faction.FactionNames.Reset();
            SoundEffect.Play(ESounds.mousedown);
            using (var f = new CustomiseSetttings())
            { 
                f.LoadSettings(_customSettings);

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    _customSettings = f.Settings;
                    _gamescreen = new Sector(_customSettings);
                    _gamescreen.FormClosed += Game_FormClosed;
                    if (!_gamescreen.IsDisposed) _gamescreen.Show();
                    animateStars.Enabled = false;
                }
            }
        }

        private void MapDesigner_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            using (var f = new MapDesigner())
            {
                f.ShowDialog(this);
            }
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            var f = sender as Sector;
            if (f == null) return;

            f.Visible = false;
            animateStars.Enabled = false;
        }        

        private void animateStars_Tick(object sender, EventArgs e)
        {
            _starfield.UpdateFrame();
            Invalidate();
        }

        private void Menu_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (_starfield.Frame != null) g.DrawImage(_starfield.Frame, 0, 0);
        }
    }
}
