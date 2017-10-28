using AllegianceForms.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class CampaignEnd : Form
    {
        CampaignGame _game;
        public GameSettings Settings;

        public const double MinFactionBonus = -2.2;
        public const double MaxFactionBonus = 2;

        public CampaignEnd(CampaignGame game)
        {
            InitializeComponent();

            _game = game;
            Settings = _game.CurrentSettings;

            UpdateForm();
        }

        private void UpdateForm()
        {
            var f = Settings.TeamFactions[0];
            var b = Math.Round(f.Bonuses.TotalBonus + Math.Abs(MinFactionBonus), 2) * 100;
            var bMax = (Math.Abs(MinFactionBonus) + MaxFactionBonus) * 100;
            var t = Settings.RestrictTechToIds[0].Length;
            var tMax = _game.TechTree.TechItems.Count;
            var m = _game.RemainingMaps.Length;
            var mMax = _game.TotalCampaignMaps;

            Points.Text = _game.UnspentPoints.ToString();
            CommanderName.Text = f.CommanderName;
            FactionName.Text = f.Name;

            FactionText.Text = $"{b} / {bMax}";
            FactionProgress.Maximum = (int)bMax;
            FactionProgress.Value = (int)b;

            TechText.Text = $"{t} / {tMax}";
            TechProgress.Maximum = tMax;
            TechProgress.Value = t;

            MapText.Text = $"{m} / {mMax}";
            MapsRemaining.Maximum = mMax;
            MapsRemaining.Value = m;
            
            toolTip1.SetToolTip(MapPreview, _game.RemainingMaps[0]);
            MapPreview.Image = Image.FromFile(StrategyGame.MapFolder + "\\" + _game.RemainingMaps[0] + ".png");

            if (_game.RemainingMaps.Length == 0) StartGame.Enabled = false;
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
            var s = _game.CurrentSettings;
            var f = Utils.CloneObject(s.TeamFactions[0]);
            var b = Math.Round(f.Bonuses.TotalBonus, 2);
            if (_game.UnspentPoints < 10 || b >= MaxFactionBonus) return;

            var c = s.TeamColours[0];
            var p = (_game.UnspentPoints / 10) * 0.1;
            SoundEffect.Play(ESounds.mousedown);
            var form = new FactionDetails(b, b + p, true);

            form.LoadFaction(f, Color.FromArgb(c));
            if (form.ShowDialog(this) != DialogResult.OK) return;

            var newF = form.Faction;
            var newB = Math.Round(newF.Bonuses.TotalBonus, 2);
            var spentPoints = (int)((newB - b) * 100);

            _game.UnspentPoints -= spentPoints;
            _game.CurrentSettings.TeamFactions[0] = newF;
            UpdateForm();
        }

        private void UnlockTech_Click(object sender, EventArgs e)
        {
            if (_game.UnspentPoints == 0) return;
            var t = Utils.CloneObject(_game.TechTree);

            SoundEffect.Play(ESounds.mousedown);
            var r = new Research(t, _game.CurrentSettings.RestrictTechToIds[0], _game.UnspentPoints);
            if (r.ShowDialog(this) != DialogResult.OK) return;

            _game.TechTree = t;
            var techItems = (from i in t.TechItems
                             where i.AmountInvested >= i.Cost || i.Unlocked
                             select i).ToList();
            techItems.ForEach(_ => _.Unlocked = true);

            _game.UnspentPoints = r.Currency;
            _game.CurrentSettings.RestrictTechToIds[0] = techItems.Select(_ => _.Id).ToArray();
            UpdateForm();
        }
    }
}
