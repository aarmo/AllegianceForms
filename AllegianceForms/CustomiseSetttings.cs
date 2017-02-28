using AllegianceForms.Engine;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms
{
    public partial class CustomiseSetttings : Form
    {
        public GameSettings Settings { get; set; }
        public const string PresetFolder = ".\\Data\\CustomPresets";

        public CustomiseSetttings()
        {
            InitializeComponent();

            if (!Directory.Exists(PresetFolder)) return;

            var presetFiles = Directory.GetFiles(PresetFolder);
            var filenames = (from f in presetFiles
                             select f.Substring(f.LastIndexOf("\\") + 1)).ToArray();

            CustomPresets.Items.AddRange(filenames);

            foreach (EShipType e in Enum.GetValues(typeof(EShipType)))
            {
                ShipType.Items.Add(e);
            }
            foreach (EBaseType e in Enum.GetValues(typeof(EBaseType)))
            {
                if (e == EBaseType.None || e.ToString().Contains("Tower")) continue;

                BaseType.Items.Add(e);
            }
        }

        public void LoadSettings(GameSettings s)
        {
            Settings = s;

            MapList.Text = s.MapName;
            Pilots.Value = s.NumPilots;
            Difficulty.SelectedIndex = s.AiDifficulty;
            Team1Colour.BackColor = Color.FromArgb(s.TeamColours[0]);
            Team2Colour.BackColor = Color.FromArgb(s.TeamColours[1]);
            Team1Faction.Text = s.TeamFactions[0].Name;
            Team2Faction.Text = s.TeamFactions[1].Name;

            ResearchCost.Text = s.ResearchCostMultiplier.ToString("P0");
            ResearchTime.Text = s.ResearchTimeMultiplier.ToString("P0");

            AsteroidsTech.Text = s.RocksPerSectorTech.ToString();
            AsteroidResource.Text = s.RocksPerSectorResource.ToString();
            AsteroidGeneral.Text = s.RocksPerSectorGeneral.ToString();

            AllowTechExp.Checked = s.RocksAllowedTech.Contains(EAsteroidType.TechUranium);
            AllowTechSup.Checked = s.RocksAllowedTech.Contains(EAsteroidType.TechCarbon);
            AllowTechTac.Checked = s.RocksAllowedTech.Contains(EAsteroidType.TechSilicon);

            ShipWeaponRange.Text = s.AntiShipWeaponRangeMultiplier.ToString("P0");
            ShipWeaponDamage.Text = s.AntiShipWeaponDamageMultiplier.ToString("P0");
            ShipWeaponFireRate.Text = s.AntiShipWeaponFireRateMultiplier.ToString("P0");

            BaseWeaponRange.Text = s.AntiBaseWeaponRangeMultiplier.ToString("P0");
            BaseWeaponDamage.Text = s.AntiBaseWeaponDamageMultiplier.ToString("P0");
            BaseWeaponFireRate.Text = s.AntiBaseWeaponFireRateMultiplier.ToString("P0");
            
            NanWeaponRange.Text = s.NanWeaponRangeMultiplier.ToString("P0");
            NanWeaponHealing.Text = s.NanWeaponHealingMultiplier.ToString("P0");
            NanWeaponFireRate.Text = s.NanWeaponFireRateMultiplier.ToString("P0");
            
            WormholeSig.Text = s.WormholesSignatureMultiplier.ToString("P0");
            WormholesVisible.Checked = s.WormholesVisible;
            RocksVisible.Checked = s.RocksVisible;

            MinersInitial.Text = s.MinersInitial.ToString();
            MinersMax.Text = s.MinersMaxDrones.ToString();
            MinerCapacity.Text = s.MinersCapacityMultiplier.ToString("P0");

            ShipType.Text = "Scout";
            BaseType.Text = "Outpost";

            ResourcesStarting.Text = s.ResourcesStartingMultiplier.ToString("P0");
            ResourcesPerRock.Text = s.ResourcesPerRockMultiplier.ToString("P0");
            ResourceConversion.Text = s.ResourceConversionRateMultiplier.ToString("P0");
            ResourcesEachTick.Text = s.ResourcesEachTickMultiplier.ToString("P0");

            ConstructorsMax.Text = s.ConstructorsMaxDrones.ToString();
            MaxTowerDrones.Text = s.ConstructorsMaxTowerDrones.ToString();
            CustomPresets.Text = string.Empty;

            MissilesDamage.Text = s.MissileWeaponDamageMultiplier.ToString("P0");
            MissilesFireRate.Text = s.MissileWeaponFireRateMultiplier.ToString("P0");
            MissilesRange.Text = s.MissileWeaponRangeMultiplier.ToString("P0");
            MissilesSpeed.Text = s.MissileWeaponSpeedMultiplier.ToString("P0");
            MissilesTracking.Text = s.MissileWeaponTrackingMultiplier.ToString("P0");
        }

        private void CustomiseSetttings_Load(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.windowslides);
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
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
        
        private void ShipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sType = (EShipType)Enum.Parse(typeof(EShipType), ShipType.Text);

            ShipHealth.Text = Settings.ShipHealthMultiplier[sType].ToString("P0");
            ShipSig.Text = Settings.ShipSignatureMultiplier[sType].ToString("P0");
            ShipSpeed.Text = Settings.ShipSpeedMultiplier[sType].ToString("P0");
            CustomPresets.Text = string.Empty;
        }

        private void BaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var bType = (EBaseType)Enum.Parse(typeof(EBaseType), BaseType.Text);

            BaseHealth.Text = Settings.StationHealthMultiplier[bType].ToString("P0");
            BaseSig.Text = Settings.StationSignatureMultiplier[bType].ToString("P0");
            CustomPresets.Text = string.Empty;
        }

        private void Pilots_ValueChanged(object sender, EventArgs e)
        {
            Settings.NumPilots = (int) Pilots.Value;
            CustomPresets.Text = string.Empty;
        }

        private void AsteroidsTech_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;

            Settings.RocksPerSectorTech = int.Parse(s.Text);
            CustomPresets.Text = string.Empty;
        }

        private void AsteroidResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;

            Settings.RocksPerSectorResource = int.Parse(s.Text);
            CustomPresets.Text = string.Empty;
        }

        private void AsteroidGeneral_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;

            Settings.RocksPerSectorGeneral = int.Parse(s.Text);
            CustomPresets.Text = string.Empty;
        }

        private void AllowTechExp_CheckedChanged(object sender, EventArgs e)
        {
            var s = sender as CheckBox;
            if (s == null) return;
            var t = EAsteroidType.TechUranium;

            if (!s.Checked)
                Settings.RocksAllowedTech.Remove(t);
            else if (!Settings.RocksAllowedTech.Contains(t))
                Settings.RocksAllowedTech.Add(t);

            CustomPresets.Text = string.Empty;
        }

        private void AllowTechTac_CheckedChanged(object sender, EventArgs e)
        {
            var s = sender as CheckBox;
            if (s == null) return;
            var t = EAsteroidType.TechSilicon;

            if (!s.Checked)
                Settings.RocksAllowedTech.Remove(t);
            else if (!Settings.RocksAllowedTech.Contains(t))
                Settings.RocksAllowedTech.Add(t);

            CustomPresets.Text = string.Empty;
        }

        private void AllowTechSup_CheckedChanged(object sender, EventArgs e)
        {
            var s = sender as CheckBox;
            if (s == null) return;
            var t = EAsteroidType.TechCarbon;

            if (!s.Checked)
                Settings.RocksAllowedTech.Remove(t);
            else if (!Settings.RocksAllowedTech.Contains(t))
                Settings.RocksAllowedTech.Add(t);

            CustomPresets.Text = string.Empty;
        }

        private void ShipWeaponRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.AntiShipWeaponRangeMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ShipWeaponFireRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.AntiShipWeaponFireRateMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ShipWeaponDamage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.AntiShipWeaponDamageMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void BaseWeaponRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.AntiBaseWeaponRangeMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void BaseWeaponFireRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.AntiBaseWeaponFireRateMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void BaseWeaponDamage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.AntiBaseWeaponDamageMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void NanWeaponRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.NanWeaponRangeMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void NanWeaponFireRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.NanWeaponFireRateMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void NanWeaponHealing_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.NanWeaponHealingMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void WormholesVisible_CheckedChanged(object sender, EventArgs e)
        {
            Settings.WormholesVisible = WormholesVisible.Checked;
            CustomPresets.Text = string.Empty;
        }

        private void WormholeSig_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.WormholesSignatureMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void MinersInitial_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;

            Settings.MinersInitial = int.Parse(s.Text);
            CustomPresets.Text = string.Empty;
        }

        private void MinersMax_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;

            Settings.MinersMaxDrones = int.Parse(s.Text);
            CustomPresets.Text = string.Empty;
        }

        private void ConstructorsMax_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;

            Settings.ConstructorsMaxDrones = int.Parse(s.Text);
            CustomPresets.Text = string.Empty;
        }

        private void MinerCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.MinersCapacityMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ResourcesStarting_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.ResourcesStartingMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ResourcesPerRock_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.ResourcesPerRockMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ResourceConversion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.ResourceConversionRateMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ResourcesEachTick_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.ResourcesEachTickMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ShipHealth_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);
            var sType = (EShipType)Enum.Parse(typeof(EShipType), ShipType.Text);

            Settings.ShipHealthMultiplier[sType] = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ShipSig_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);
            var sType = (EShipType)Enum.Parse(typeof(EShipType), ShipType.Text);

            Settings.ShipSignatureMultiplier[sType] = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ShipSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);
            var sType = (EShipType)Enum.Parse(typeof(EShipType), ShipType.Text);

            Settings.ShipSpeedMultiplier[sType] = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void BaseHealth_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);
            var bType = (EBaseType)Enum.Parse(typeof(EBaseType), BaseType.Text);

            Settings.StationHealthMultiplier[bType] = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void BaseSig_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);
            var bType = (EBaseType)Enum.Parse(typeof(EBaseType), BaseType.Text);

            Settings.StationSignatureMultiplier[bType] = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void Difficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;

            Settings.AiDifficulty = s.SelectedIndex;
            CustomPresets.Text = string.Empty;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(PresetFolder)) Directory.CreateDirectory(PresetFolder);

            var filename = PresetFolder + "\\" + CustomPresets.Text;
            if (CustomPresets.Text == string.Empty || File.Exists(filename))
            {
                SoundEffect.Play(ESounds.outofammo);
                return;
            }

            SoundEffect.Play(ESounds.mousedown);
            Utils.SerialiseToFile(filename, Settings);
            if (!CustomPresets.Items.Contains(CustomPresets.Text)) CustomPresets.Items.Add(CustomPresets.Text);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            var filename = PresetFolder + "\\" + CustomPresets.Text;
            if (CustomPresets.Text == string.Empty || !File.Exists(filename))
            {
                SoundEffect.Play(ESounds.outofammo);
                return;
            }

            SoundEffect.Play(ESounds.mousedown);
            var s = Utils.DeserialiseFromFile<GameSettings>(filename);
            if (s == null) return;
            LoadSettings(s);
        }

        private void Team1Colour_Click(object sender, EventArgs e)
        {
            var s = sender as Button;
            if (s == null) return;

            colorDialog.Color = s.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                s.BackColor = colorDialog.Color;
                Settings.TeamColours[0] = s.BackColor.ToArgb();
            }
        }

        private void Team2Colour_Click(object sender, EventArgs e)
        {
            var s = sender as Button;
            if (s == null) return;

            colorDialog.Color = s.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                s.BackColor = colorDialog.Color;
                Settings.TeamColours[1] = s.BackColor.ToArgb();
            }
        }

        private void ResearchCost_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.ResearchCostMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ResearchTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.ResearchTimeMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void ResetPreset_Click(object sender, EventArgs e)
        {
            LoadSettings(GameSettings.Default());
        }

        private void MapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.MapName = MapList.Text;
        }

        private void MaxTowerDrones_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            
            Settings.ConstructorsMaxTowerDrones = int.Parse(s.Text);
            CustomPresets.Text = string.Empty;
        }

        private void RocksVisible_CheckedChanged(object sender, EventArgs e)
        {
            Settings.RocksVisible = RocksVisible.Checked;
            CustomPresets.Text = string.Empty;
        }

        private void MissilesSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.MissileWeaponSpeedMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void MissilesRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.MissileWeaponRangeMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void MissilesFireRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.MissileWeaponFireRateMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void MissilesDamage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.MissileWeaponDamageMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void MissilesTracking_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ComboBox;
            if (s == null) return;
            var p = s.Text.Replace("%", string.Empty);

            Settings.MissileWeaponTrackingMultiplier = float.Parse(p) / 100f;
            CustomPresets.Text = string.Empty;
        }

        private void Team1Faction_Click(object sender, EventArgs e)
        {
            var f = Settings.TeamFactions[0].Clone();

            var form = new FactionDetails();
            form.LoadFaction(f);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Settings.TeamFactions[0] = form.Faction;
                Team1Faction.Text = Settings.TeamFactions[0].Name;
            }
        }

        private void Team2Faction_Click(object sender, EventArgs e)
        {
            var f = Settings.TeamFactions[1].Clone();

            var form = new FactionDetails();
            form.LoadFaction(f);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Settings.TeamFactions[1] = form.Faction;
                Team2Faction.Text = Settings.TeamFactions[1].Name;
            }
        }
    }
}
