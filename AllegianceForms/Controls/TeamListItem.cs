using System.Drawing;
using System.Windows.Forms;
using AllegianceForms.Engine.Factions;
using AllegianceForms.Forms;
using AllegianceForms.Engine;
using System;

namespace AllegianceForms.Controls
{
    public partial class TeamListItem : UserControl
    {
        public delegate void TeamChangedHandler(TeamListItem sender);
        public event TeamChangedHandler TeamChangedEvent;

        public int Index { get; set; }
        public int AllianceIndex { get; set; }
        public int ColourArgb { get; set; }
        public Faction Faction { get; set; }

        public TeamListItem(int index, int colour, Faction faction, int alliance)
        {
            InitializeComponent();
            Index = index;
            ColourArgb = colour;
            Faction = faction;
            AllianceIndex = alliance;

            RefreshTeam();
        }

        private void RefreshTeam()
        {
            TeamNumber.Text = $"Team {Index}:";
            TeamColour.BackColor = Color.FromArgb(ColourArgb);
            TeamFaction.Text = Faction.Name;
            AllianceGroup.SelectedIndex = AllianceIndex - 1;
        }

        private void TeamColour_Click(object sender, System.EventArgs e)
        {
            var s = sender as Button;
            if (s == null) return;

            colorDialog.Color = s.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                s.BackColor = colorDialog.Color;
                ColourArgb = s.BackColor.ToArgb();
                OnTeamChanged();
            }
        }

        private void TeamFaction_Click(object sender, System.EventArgs e)
        {
            var f = Faction.Clone();

            var form = new FactionDetails();
            form.LoadFaction(f, Color.FromArgb(ColourArgb));

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Faction = form.Faction;
                TeamFaction.Text = Faction.Name;
                OnTeamChanged();
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mouseover);
            var b = sender as Button;
            if (b != null && b.Text != "Change") b.BackColor = Color.DarkGreen;
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b != null && b.Text != "Change") b.BackColor = Color.Black;
        }

        private void OnTeamChanged()
        {
            if (TeamChangedEvent != null) TeamChangedEvent(this);
        }

        private void AllianceGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            AllianceIndex = AllianceGroup.SelectedIndex + 1;
            OnTeamChanged();
        }
    }
}
