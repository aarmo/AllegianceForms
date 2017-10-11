using AllegianceForms.Engine;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class CampaignEnd : Form
    {
        public GameSettings Settings;

        public CampaignEnd(StrategyGame game)
        {
            InitializeComponent();

            Points.Text = game.TotalCampaignPoints(1).ToString();
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
    }
}
