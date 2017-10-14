using AllegianceForms.Engine;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class CampaignEnd : Form
    {
        CampaignGame _game;
        public GameSettings Settings;

        public CampaignEnd(CampaignGame game)
        {
            InitializeComponent();

            _game = game;
            Settings = _game.CurrentSettings;

            Points.Text = _game.UnspentPoints.ToString();
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            DialogResult = DialogResult.OK;
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

        private void Ladder_Load(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.windowslides);
        }

        private void ChangeFaction_Click(object sender, EventArgs e)
        {
            if (_game.UnspentPoints < 10) return;

            var s = _game.CurrentSettings;
            var f = s.TeamFactions[0].Clone();
            var c = s.TeamColours[0];
            var b = Math.Round(f.Bonuses.TotalBonus, 2);
            var p = (_game.UnspentPoints / 10) * 0.1;           

            var form = new FactionDetails(b, b + p);

            form.LoadFaction(f, Color.FromArgb(c));

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                var newF = form.Faction;
                var newB = Math.Round(newF.Bonuses.TotalBonus, 2);
                var spentPoints = (int)((newB - b) * 100);

                _game.UnspentPoints -= spentPoints;
                Points.Text = _game.UnspentPoints.ToString();

                _game.CurrentSettings.TeamFactions[0] = newF;
            }
        }
    }
}
