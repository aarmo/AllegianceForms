using AllegianceForms.Engine;
using AllegianceForms.Engine.Factions;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class FactionDetails : Form
    {
        public Faction Faction { get; set; }
        private Color _colour;
        private double _balance;
        private double _initBalance;
        private double _targetBalance;

        private const float BalanceIncrement = 0.1f;

        private GameSettings _settings;

        public FactionDetails(GameSettings settings, double balance = 0, double target = 0, bool locked = false)
        {
            InitializeComponent();

            _settings = settings;
            _initBalance = Math.Round(balance, 2);
            _targetBalance = Math.Round(target, 2);

            if (locked)
            {
                CustomPresets.Visible = false;
                LoadPreset.Visible = false;
                SavePreset.Visible = false;
                Random.Visible = false;
                RandomName.Visible = false;
                FactionName.Enabled = false;
                PlayerName.Enabled = false;
                RandomCommanderName.Visible = false;
                Default.Visible = false;
                FactionRace.Enabled = false;
            }
            else
            {
                InitialisePresets();
            }
        }

        private void InitialisePresets()
        {
            if (!Directory.Exists(StrategyGame.FactionPresetFolder)) return;

            var presetFiles = Directory.GetFiles(StrategyGame.FactionPresetFolder);
            var filenames = (from f in presetFiles
                             select f.Substring(f.LastIndexOf("\\") + 1)).ToArray();

            CustomPresets.Items.AddRange(filenames);

            foreach (ERaceType e in Enum.GetValues(typeof(ERaceType)))
            {
                FactionRace.Items.Add(e);
            }
        }

        public void LoadFaction(Faction f, Color c)
        {
            Faction = f;
            _colour = c;

            FactionName.Text = f.Name;
            ResearchTime.Text = f.Bonuses.ResearchTime.ToString("P0");
            ResearchCost.Text = Faction.Bonuses.ResearchCost.ToString("P0");
            Speed.Text = f.Bonuses.Speed.ToString("P0");
            Health.Text = f.Bonuses.Health.ToString("P0");
            ScanRange.Text = f.Bonuses.ScanRange.ToString("P0");
            Signature.Text = f.Bonuses.Signature.ToString("P0");
            FireRate.Text = f.Bonuses.FireRate.ToString("P0");
            MissileSpeed.Text = f.Bonuses.MissileSpeed.ToString("P0");
            MissileTracking.Text = f.Bonuses.MissileTracking.ToString("P0");
            MiningEfficiency.Text = f.Bonuses.MiningEfficiency.ToString("P0");
            MiningCapacity.Text = f.Bonuses.MiningCapacity.ToString("P0");

            FactionPicture.Image = Utils.GetAvatarImage(f.PictureCode);
            PlayerName.ForeColor = c;
            PlayerName.Text = Faction.CommanderName;

            FactionRace.SelectedItem = f.Race;

            RefreshBalance();
        }

        private void RefreshBalance()
        {
            SoundEffect.Play(ESounds.mousedown);

            var f = Faction;
            _balance = Math.Round(f.Bonuses.TotalBonus, 2);

            Speed.ForeColor = GetColour(f.Bonuses.Speed);
            ResearchTime.ForeColor = GetColour(f.Bonuses.ResearchTime, true);
            ResearchCost.ForeColor = GetColour(f.Bonuses.ResearchCost, true);
            Health.ForeColor = GetColour(f.Bonuses.Health);
            ScanRange.ForeColor = GetColour(f.Bonuses.ScanRange);
            Signature.ForeColor = GetColour(f.Bonuses.Signature, true);
            FireRate.ForeColor = GetColour(f.Bonuses.FireRate);
            MissileSpeed.ForeColor = GetColour(f.Bonuses.MissileSpeed);
            MissileTracking.ForeColor = GetColour(f.Bonuses.MissileTracking);
            MiningEfficiency.ForeColor = GetColour(f.Bonuses.MiningEfficiency);
            MiningCapacity.ForeColor = GetColour(f.Bonuses.MiningCapacity);

            var balanced = (_balance >= _initBalance && _balance <= _targetBalance);
            Done.Enabled = balanced;

            UpdateButtonsEnabled();

            if (balanced)
            {
                BalancedLabel.ForeColor = Color.Lime;
                BalancedLabel.Text = "OK";
            }
            else
            {
                BalancedLabel.ForeColor = Color.Red;

                if (_balance < _initBalance) BalancedLabel.Text = "Too Low";
                if (_balance > _targetBalance) BalancedLabel.Text = "Too High";
            }
        }

        private void UpdateButtonsEnabled()
        {
            ResearchTimeUp.Enabled = (Faction.Bonuses.ResearchTime < FactionBonus.MaxBonus);
            ResearchTimeDown.Enabled = (Faction.Bonuses.ResearchTime > FactionBonus.MinBonus);
            ResearchCostUp.Enabled = (Faction.Bonuses.ResearchCost < FactionBonus.MaxBonus);
            ResearchCostDown.Enabled = (Faction.Bonuses.ResearchCost > FactionBonus.MinBonus);
            SpeedUp.Enabled = (Faction.Bonuses.Speed < FactionBonus.MaxBonus);
            SpeedDown.Enabled = (Faction.Bonuses.Speed > FactionBonus.MinBonus);
            HealthUp.Enabled = (Faction.Bonuses.Health < FactionBonus.MaxBonus);
            HealthDown.Enabled = (Faction.Bonuses.Health > FactionBonus.MinBonus);
            ScanRangeUp.Enabled = (Faction.Bonuses.ScanRange < FactionBonus.MaxBonus);
            ScanRangeDown.Enabled = (Faction.Bonuses.ScanRange > FactionBonus.MinBonus);
            SignatureUp.Enabled = (Faction.Bonuses.Signature < FactionBonus.MaxBonus);
            SignatureDown.Enabled = (Faction.Bonuses.Signature > FactionBonus.MinBonus);
            FireRateUp.Enabled = (Faction.Bonuses.Signature < FactionBonus.MaxBonus);
            FireRateDown.Enabled = (Faction.Bonuses.Signature > FactionBonus.MinBonus);
            MiningCapacityUp.Enabled = (Faction.Bonuses.MiningCapacity < FactionBonus.MaxBonus);
            MiningCapacityDown.Enabled = (Faction.Bonuses.MiningCapacity > FactionBonus.MinBonus);
            MiningSpeedUp.Enabled = (Faction.Bonuses.MiningEfficiency < FactionBonus.MaxBonus);
            MiningSpeedDown.Enabled = (Faction.Bonuses.MiningEfficiency > FactionBonus.MinBonus);
            MissileSpeedUp.Enabled = (Faction.Bonuses.MissileSpeed < FactionBonus.MaxBonus);
            MissileSpeedDown.Enabled = (Faction.Bonuses.MissileSpeed > FactionBonus.MinBonus);
            MissileTrackingUp.Enabled = (Faction.Bonuses.MissileTracking < FactionBonus.MaxBonus);
            MissileTrackingDown.Enabled = (Faction.Bonuses.MissileTracking > FactionBonus.MinBonus);

            var canIncrease = (_balance < _targetBalance);
            SpeedUp.Enabled = ResearchTimeDown.Enabled = ResearchCostDown.Enabled
                = HealthUp.Enabled = ScanRangeUp.Enabled = SignatureDown.Enabled
                = FireRateUp.Enabled = MissileSpeedUp.Enabled = MissileTrackingUp.Enabled
                = MiningSpeedUp.Enabled = MiningCapacityUp.Enabled = canIncrease;
        }

        private void Random_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            CustomPresets.Text = string.Empty;
            LoadFaction(Faction.Random(_settings), _colour);
        }

        private void FactionDetails_Load(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.windowslides);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(StrategyGame.FactionPresetFolder)) Directory.CreateDirectory(StrategyGame.FactionPresetFolder);

            var filename = StrategyGame.FactionPresetFolder + "\\" + CustomPresets.Text;
            if (CustomPresets.Text == string.Empty || File.Exists(filename))
            {
                SoundEffect.Play(ESounds.outofammo);
                return;
            }

            SoundEffect.Play(ESounds.mousedown);
            Utils.SerialiseToFile(filename, Faction);
            if (!CustomPresets.Items.Contains(CustomPresets.Text)) CustomPresets.Items.Add(CustomPresets.Text);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            var filename = StrategyGame.FactionPresetFolder + "\\" + CustomPresets.Text;
            if (CustomPresets.Text == string.Empty || !File.Exists(filename))
            {
                SoundEffect.Play(ESounds.outofammo);
                return;
            }

            SoundEffect.Play(ESounds.mousedown);
            var f = Utils.DeserialiseFromFile<Faction>(filename);
            if (f == null) return;
            LoadFaction(f, _colour);
        }

        private void RandomName_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            FactionName.Text = Faction.Name = Faction.FactionNames.NextString;
        }
        
        private Color GetColour(float value, bool inverse = false)
        {
            var r = Math.Round(value, 2);
            return (r == 1) ? Color.DarkGreen : (r > 1 && !inverse) || (r < 1 && inverse) ? Color.Lime : Color.DarkRed;
        }

        private void ResearchTimeUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.ResearchTime += BalanceIncrement;
            ResearchTime.Text = Faction.Bonuses.ResearchTime.ToString("P0");
            RefreshBalance();
        }

        private void ResearchTimeDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.ResearchTime -= BalanceIncrement;
            ResearchTime.Text = Faction.Bonuses.ResearchTime.ToString("P0");
            RefreshBalance();
        }

        private void ResearchCostUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.ResearchCost += BalanceIncrement;
            ResearchCost.Text = Faction.Bonuses.ResearchCost.ToString("P0");
            RefreshBalance();
        }
        
        private void ResearchCostDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.ResearchCost -= BalanceIncrement;
            ResearchCost.Text = Faction.Bonuses.ResearchCost.ToString("P0");
            RefreshBalance();
        }

        private void SpeedUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.Speed += BalanceIncrement;
            Speed.Text = Faction.Bonuses.Speed.ToString("P0");
            RefreshBalance();
        }

        private void SpeedDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.Speed -= BalanceIncrement;
            Speed.Text = Faction.Bonuses.Speed.ToString("P0");
            RefreshBalance();
        }

        private void HealthUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.Health += BalanceIncrement;
            Health.Text = Faction.Bonuses.Health.ToString("P0");
            RefreshBalance();
        }

        private void ScanRangeUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.ScanRange += BalanceIncrement;
            ScanRange.Text = Faction.Bonuses.ScanRange.ToString("P0");
            RefreshBalance();
        }

        private void SignatureUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.Signature += BalanceIncrement;
            Signature.Text = Faction.Bonuses.Signature.ToString("P0");
            RefreshBalance();
        }

        private void FireRateUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.FireRate += BalanceIncrement;
            FireRate.Text = Faction.Bonuses.FireRate.ToString("P0");
            RefreshBalance();
        }

        private void MiningCapacityUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.MiningCapacity += BalanceIncrement;
            MiningCapacity.Text = Faction.Bonuses.MiningCapacity.ToString("P0");
            RefreshBalance();
        }

        private void MiningCapacityDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.MiningCapacity -= BalanceIncrement;
            MiningCapacity.Text = Faction.Bonuses.MiningCapacity.ToString("P0");
            RefreshBalance();
        }

        private void FireRateDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.FireRate -= BalanceIncrement;
            FireRate.Text = Faction.Bonuses.FireRate.ToString("P0");
            RefreshBalance();
        }

        private void SignatureDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.Signature -= BalanceIncrement;
            Signature.Text = Faction.Bonuses.Signature.ToString("P0");
            RefreshBalance();
        }

        private void ScanRangeDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.ScanRange -= BalanceIncrement;
            ScanRange.Text = Faction.Bonuses.ScanRange.ToString("P0");
            RefreshBalance();
        }

        private void HealthDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.Health -= BalanceIncrement;
            Health.Text = Faction.Bonuses.Health.ToString("P0");
            RefreshBalance();
        }

        private void MissileSpeedUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.MissileSpeed += BalanceIncrement;
            MissileSpeed.Text = Faction.Bonuses.MissileSpeed.ToString("P0");
            RefreshBalance();
        }

        private void MissileSpeedDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.MissileSpeed -= BalanceIncrement;
            MissileSpeed.Text = Faction.Bonuses.MissileSpeed.ToString("P0");
            RefreshBalance();
        }

        private void MissileTrackingUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.MissileTracking += BalanceIncrement;
            MissileTracking.Text = Faction.Bonuses.MissileTracking.ToString("P0");
            RefreshBalance();
        }

        private void MissileTrackingDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.MissileTracking -= BalanceIncrement;
            MissileTracking.Text = Faction.Bonuses.MissileTracking.ToString("P0");
            RefreshBalance();
        }

        private void MiningSpeedUp_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.MiningEfficiency += BalanceIncrement;
            MiningEfficiency.Text = Faction.Bonuses.MiningEfficiency.ToString("P0");
            RefreshBalance();
        }
        
        private void MiningSpeedDown_Click(object sender, EventArgs e)
        {
            Faction.Bonuses.MiningEfficiency -= BalanceIncrement;
            MiningEfficiency.Text = Faction.Bonuses.MiningEfficiency.ToString("P0");
            RefreshBalance();
        }

        private void RandomImage_Click(object sender, EventArgs e)
        {
            Faction.PictureCode = Utils.RandomString();
            FactionPicture.Image = Utils.GetAvatarImage(Faction.PictureCode);
        }

        private void RandomCommanderName_Click(object sender, EventArgs e)
        {
            Faction.CommanderName = StrategyGame.RandomName.GetRandomName(Utils.RandomString());
            PlayerName.Text = Faction.CommanderName;
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

        private void Default_Click(object sender, EventArgs e)
        {
            var f = Faction.Default(_settings);
            f.CommanderName = "Default";
            LoadFaction(f, _colour);
        }

        private void FactionRace_SelectedValueChanged(object sender, EventArgs e)
        {
            Faction.Race = (ERaceType)FactionRace.SelectedItem;
            RacePicture.Image = Utils.GetRaceImage(Faction.Race);
        }
    }
}
