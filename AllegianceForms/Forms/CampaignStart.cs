using AllegianceForms.Engine;
using AllegianceForms.Engine.Factions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class CampaignStart : Form
    {
        public CampaignStart()
        {
            InitializeComponent();
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

        private void RandomCommanderName_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            PlayerName.Text = StrategyGame.RandomName.GetRandomName(Utils.RandomString());
        }

        private void RandomName_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            FactionName.Text = Faction.FactionNames.NextString;

        }

        private void FactionName_Enter(object sender, EventArgs e)
        {
            FactionName.SelectAll();
        }

        private void PlayerName_Enter(object sender, EventArgs e)
        {
            PlayerName.SelectAll();
        }

        private void FactionName_MouseDown(object sender, MouseEventArgs e)
        {
            FactionName.SelectAll();
        }

        private void PlayerName_MouseDown(object sender, MouseEventArgs e)
        {
            PlayerName.SelectAll();
        }
    }
}
