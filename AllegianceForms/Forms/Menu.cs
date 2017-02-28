using AllegianceForms.Engine;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            SoundEffect.Init();
            SoundEffect.Play(ESounds.windowslides);

            AppVersion.Text = string.Format("(ALPHA) v{0}", Assembly.GetEntryAssembly().GetName().Version);
        }

        private void Skirmish_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            _gameSettings = GameSettings.Default();
            var f = new Sector(_gameSettings);
            f.Show(this);
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

        private GameSettings _gameSettings = GameSettings.Default();
        private void CustomGame_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            var f = new CustomiseSetttings();
            f.LoadSettings(_gameSettings);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                _gameSettings = f.Settings;
                var f2 = new Sector(_gameSettings);
                if (!f2.IsDisposed) f2.Show(this);
            }
        }
    }
}
