using AllegianceForms.Engine.AI;
using AllegianceForms.Engine;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class DebugAI : Form
    {
        private CommanderAI _ai;
        private DateTime _techTimeout;

        public DebugAI(CommanderAI ai)
        {
            InitializeComponent();

            _ai = ai;
            UpdateDebugInfo();
        }

        public void UpdateDebugInfo()
        {
            CheatVisible.Checked = _ai.CheatVisibility;
            CheatCredit.Checked = _ai.CheatCredits;
            EnableAi.Checked = _ai.Enabled;
            ForceVisible.Checked = _ai.ForceVisible;

            var maxCredit = _ai.CreditPriorities.Values.Max();
            var maxPilot = _ai.PilotPriorities.Values.Max();

            MaxValueCredit.Text = maxCredit.ToString("F");
            MaxValuePilot.Text = maxPilot.ToString("F");

            ScoutingPrior.Maximum = (int)maxPilot;
            ScoutingPrior.Value = (int)_ai.PilotPriorities[EAiPilotPriorities.Scout];
            Scouting.Checked = _ai.Scouting;

            MinerOPrior.Maximum = (int)maxPilot;
            MinerOPrior.Value = (int)_ai.PilotPriorities[EAiPilotPriorities.MinerOffense];
            MinerO.Checked = _ai.MinerOffence;

            MinerDPrior.Maximum = (int)maxPilot;
            MinerDPrior.Value = (int)_ai.PilotPriorities[EAiPilotPriorities.MinerDefense];
            MinerD.Checked = _ai.MinerDefence;

            BaseOPrior.Maximum = (int)maxPilot;
            BaseOPrior.Value = (int)_ai.PilotPriorities[EAiPilotPriorities.BaseOffense];
            BaseO.Checked = _ai.BaseOffence;

            BaseDPrior.Maximum = (int)maxPilot;
            BaseDPrior.Value = (int)_ai.PilotPriorities[EAiPilotPriorities.BaseDefense];
            BaseD.Checked = _ai.BaseDefence;

            ExpansionPrior.Maximum = (int)maxCredit;
            ExpansionPrior.Value = (int)_ai.CreditPriorities[EAiCreditPriorities.Expansion];
            Expansion.Checked = _ai.CreditsForExpansion;

            OffencePrior.Maximum = (int)maxCredit;
            OffencePrior.Value = (int)_ai.CreditPriorities[EAiCreditPriorities.Offense];
            Offence.Checked = _ai.CreditsForOffence;

            DefencePrior.Maximum = (int)maxCredit;
            DefencePrior.Value = (int)_ai.CreditPriorities[EAiCreditPriorities.Defense];
            Defence.Checked = _ai.CreditsForDefence;

            ScoutingMission.Checked = _ai.ScoutingMission;
            ScoutingShips.Text = _ai._scouting.IncludedShips.Count().ToString();
            ScoutingRequireMore.Text = _ai._scouting.RequireMorePilots().ToString();
            ScoutingRecentOrders.Text = _ai._scouting.RecentOrders.Count.ToString();

            BuildingMission.Checked = _ai.BuildingMission;
            BuildingShips.Text = _ai._building.IncludedShips.Count().ToString();
            BuildingRequireMore.Text = _ai._building.RequireMorePilots().ToString();
            BuildingRecentOrders.Text = _ai._building.RecentOrders.Count.ToString();

            MiningMission.Checked = _ai.MiningMission;
            MiningShips.Text = (_ai.Miners == null) ? "0" : _ai.Miners.Count().ToString();
            MiningRequireMore.Text = _ai._mining.RequireMorePilots().ToString();
            MiningRecentOrders.Text = _ai._mining.RecentOrders.Count.ToString();

            MinerOMission.Checked = _ai.MinerOffenceMission;
            MinerOShips.Text = _ai._minerO.IncludedShips.Count().ToString();
            MinerORequireMore.Text = _ai._minerO.RequireMorePilots().ToString();
            MinerORecentOrders.Text = _ai._minerO.RecentOrders.Count.ToString();

            MinerDMission.Checked = _ai.MinerDefenceMission;
            MinerDShips.Text = _ai._minerD.IncludedShips.Count().ToString();
            MinerDRequireMore.Text = _ai._minerD.RequireMorePilots().ToString();
            MinerDRecentOrders.Text = _ai._minerD.RecentOrders.Count.ToString();

            BaseOMission.Checked = _ai.BaseOffenceMission;
            BaseOShips.Text = _ai._bombing.IncludedShips.Count().ToString();
            BaseORequireMore.Text = _ai._bombing.RequireMorePilots().ToString();
            BaseORecentOrders.Text = _ai._bombing.RecentOrders.Count.ToString();

            BaseDMission.Checked = _ai.BaseDefenceMission;
            BaseDShips.Text = _ai._defense.IncludedShips.Count().ToString();
            BaseDRequireMore.Text = _ai._defense.RequireMorePilots().ToString();
            BaseDRecentOrders.Text = _ai._defense.RecentOrders.Count.ToString();

            MinerCount.Text = (_ai.Miners == null) ? "0" : _ai.Miners.Count.ToString();
            ConCount.Text = (_ai.Builders == null) ? "0" : _ai.Builders.Count.ToString();

            ScoutCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.Scout).ToString();
            FigCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.Fighter).ToString();
            SfCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.StealthFighter).ToString();
            SbCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.StealthBomber).ToString();
            IntCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.Interceptor).ToString();
            TtCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.TroopTransport).ToString();
            BbrCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.Bomber).ToString();
            GsCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.Gunship).ToString();
            FbCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.FighterBomber).ToString();
            LifepodCount.Text = StrategyGame.AllUnits.Count(_ => _.Team == _ai.Team && _.Type == EShipType.Lifepod).ToString();

            DockedPilots.Text = StrategyGame.DockedPilots[_ai.Team - 1].ToString();
            Credits.Text = StrategyGame.Credits[_ai.Team - 1].ToString();

            if (_ai.NextTech != null)
            {
                NextTech.Text = string.Format("{0}... ({1:P} - {2:F}s)", _ai.NextTech.Name, _ai.NextTech.AmountInvested * 1.0f / _ai.NextTech.Cost, _ai.NextTech.DurationSec - _ai.NextTech.TimeResearched);
                _techTimeout = DateTime.Now.AddSeconds(5);
            }
            else if (DateTime.Now > _techTimeout)
            {
                NextTech.Text = string.Empty;
            }
        }

        private void CheatVisible_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.CheatVisibility = CheatVisible.Checked;
        }

        private void CheatCredit_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.CheatCredits = CheatCredit.Checked;
        }

        private void EnableAi_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.Enabled = EnableAi.Checked;
        }

        private void ForceVisible_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (var ai in StrategyGame.AICommanders)
            {
                if (ai != null) ai.ForceVisible = ForceVisible.Checked;
            }
        }

        private void Scouting_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.Scouting = Scouting.Checked;
        }

        private void MinerO_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.MinerOffence = MinerO.Checked;
        }

        private void MinerD_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.MinerDefence = MinerD.Checked;
        }

        private void BaseO_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.BaseOffence = BaseO.Checked;
        }

        private void BaseD_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.BaseDefence = BaseD.Checked;
        }

        private void Expansion_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.CreditsForExpansion = Expansion.Checked;
        }

        private void Offence_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.CreditsForOffence = Offence.Checked;
        }

        private void Defence_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.CreditsForDefence = Defence.Checked;
        }

        private void ScoutingMission_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.ScoutingMission = ScoutingMission.Checked;
        }

        private void BuildingMission_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.BuildingMission = BuildingMission.Checked;
        }

        private void MiningMission_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.MiningMission = MiningMission.Checked;
        }

        private void MinerOMission_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.MinerOffenceMission = MinerOMission.Checked;
        }

        private void MinerDMission_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.MinerDefenceMission = MinerDMission.Checked;
        }

        private void BaseOMission_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.BaseOffenceMission = BaseOMission.Checked;
        }

        private void BaseDMission_CheckedChanged(object sender, System.EventArgs e)
        {
            _ai.BaseDefenceMission = BaseDMission.Checked;
        }
    }
}
