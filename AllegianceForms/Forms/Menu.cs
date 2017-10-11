using AllegianceForms.Engine;
using AllegianceForms.Engine.Factions;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class Menu : Form
    {
        private GameSettings _gameSettings = GameSettings.Default();

        public Menu()
        {
            InitializeComponent();
            SoundEffect.Init();
            SoundEffect.Play(ESounds.windowslides);

            AppVersion.Text = string.Format("(ALPHA) v{0}", Assembly.GetEntryAssembly().GetName().Version);
        }

        private void Dogfight_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            var f = new ChanceGame();
            f.ShowDialog(this);
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
            f.LoadSettings(_gameSettings);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                _gameSettings = f.Settings;
                var f2 = new Sector(_gameSettings);
                if (!f2.IsDisposed) f2.Show();
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
            var f2 = new Sector(_gameSettings);
            if (!f2.IsDisposed) f2.Show();
        }

        private Sector _gamescreen;

        private void PlayCampaign_Click(object sender, EventArgs e)
        {
            var settings = GameSettings.CampaignStart();
            _gamescreen = new Sector(settings);
            _gamescreen.FormClosed += Game_FormClosed;

            if (!_gamescreen.IsDisposed) _gamescreen.Show();
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            var f = sender as Sector;
            if (f == null) return;

            var game = f.StrategyGame;
            if (game.GameSettings.GameType == EGameType.Campaign)
            {
                var c = new CampaignEnd(game);
                if (c.ShowDialog() != DialogResult.OK) return;

                _gamescreen = new Sector(c.Settings);
                _gamescreen.FormClosed += Game_FormClosed;

                if (!_gamescreen.IsDisposed) _gamescreen.Show();            }
            
        }
    }
}
