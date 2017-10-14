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
        private CampaignGame _campaignGame;
        private GameSettings _customSettings = GameSettings.Default();
        private StarField _starfield = new StarField();

        public Menu()
        {
            InitializeComponent();
            SoundEffect.Init();
            SoundEffect.Play(ESounds.windowslides);

            AppVersion.Text = string.Format("(ALPHA) v{0}", Assembly.GetEntryAssembly().GetName().Version);
            _campaignGame = CampaignGame.LoadOrDefault();
            if (_campaignGame.GamesPlayed > 0) PlayCampaign.Text = "Continue Campaign";

            _starfield.Init(Width, Height);
            animateStars.Enabled = true;
        }

        private void Dogfight_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            var f = new ChanceGame();
            animateStars.Enabled = false;
            f.ShowDialog(this);
            animateStars.Enabled = true;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            Application.Exit();
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mouseover);
            var b = sender as Button;
            if (b != null) b.BackColor = Color.DarkGreen;
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b != null) b.BackColor = Color.Black;
        }

        private void CustomGame_Click(object sender, EventArgs e)
        {
            Faction.FactionNames.Reset();
            SoundEffect.Play(ESounds.mousedown);
            var f = new CustomiseSetttings();
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

        private void MapDesigner_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            var f = new MapDesigner();
            f.ShowDialog(this);
        }

        private void QuickPlay_Click(object sender, EventArgs e)
        {
            var settings = GameSettings.Default();
            _gamescreen = new Sector(settings);
            _gamescreen.FormClosed += Game_FormClosed;
            if (!_gamescreen.IsDisposed) _gamescreen.Show();
            animateStars.Enabled = false;
        }

        private void PlayCampaign_Click(object sender, EventArgs e)
        {
            _gamescreen = new Sector(_campaignGame.CurrentSettings);
            _gamescreen.FormClosed += Game_FormClosed;
            if (!_gamescreen.IsDisposed) _gamescreen.Show();
            animateStars.Enabled = false;
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            var f = sender as Sector;
            if (f == null) return;

            f.Visible = false;
            animateStars.Enabled = true;

            var game = f.StrategyGame;
            if (game.GameSettings.GameType == EGameType.Campaign)
            {
                _campaignGame.UpdateResults(game);

                var c = new CampaignEnd(_campaignGame);
                if (c.ShowDialog(this) == DialogResult.OK)
                {
                    // Save & play again!
                    _campaignGame.SaveGame();
                    _gamescreen = new Sector(c.Settings);
                    _gamescreen.FormClosed += Game_FormClosed;

                    if (!_gamescreen.IsDisposed) _gamescreen.Show();
                    animateStars.Enabled = false;
                }
            }
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
