using AllegianceForms.Engine;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class CampaignEnd : Form
    {
        public GameSettings Settings;
        private int _points;
        public CampaignEnd(StrategyGame game)
        {
            InitializeComponent();
            _points = game.TotalCampaignPoints(1);

            Points.Text = _points.ToString();
            Settings = game.GameSettings;
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
            var f = Settings.TeamFactions[0].Clone();
            var c = Settings.TeamColours[0];
            var b = Math.Round(f.Bonuses.TotalBonus, 2);
            var extra = (_points / 10) * 0.1f;
            var form = new FactionDetails(b, b + extra);

            form.LoadFaction(f, Color.FromArgb(c));

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Settings.TeamFactions[0] = form.Faction;
            }
        }
    }
}
