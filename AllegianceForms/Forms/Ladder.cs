using AllegianceForms.Controls;
using AllegianceForms.Engine;
using AllegianceForms.Engine.Factions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class Ladder : Form
    {
        public LadderGame CurrentLadder;

        public Ladder()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            CurrentLadder.LadderStarted = true;
            SoundEffect.Play(ESounds.mousedown);
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
                
        public void LoadLadder(LadderGame l)
        {
            CurrentLadder = l;

            Difficulty.SelectedIndex = l.AiDifficulty;
            LadderType.SelectedIndex = (int)l.LadderType - 1;
            MapPool.Items.Clear();
            MapPool.Items.AddRange(l.MapPool);

            CommanderItems.Controls.Clear();

            var commanders = new List<Faction>(l.OtherPlayers);
            commanders.Add(l.Player);
            var orderedList = commanders.OrderByDescending(_ => _.CommanderRankPoints);

            foreach(var c in orderedList)
            {
                CommanderItems.Controls.Add(new LadderCommander(c));
            }

            PlayerCommander.Controls.Clear();
            PlayerCommander.Controls.Add(new LadderCommander(l.Player));
        }

        private void Abandon_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to abandon this ladder?", "Abandon Ladder?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                CurrentLadder.AbandonLadder();
                DialogResult = DialogResult.Cancel;
            }
        }

        private void Ladder_Load(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.windowslides);
        }
    }
}
