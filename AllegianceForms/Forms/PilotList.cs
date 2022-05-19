using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace AllegianceForms.Forms
{
    public enum EFilterType
    {
        Miner, Constructor, Capital, None
    }

    public partial class PilotList : Form
    {
        StrategyGame _game;
        bool _showCons = true;
        bool _showCaps = true;
        bool _showNonCaps = true;
        bool _showMiners = true;
        bool _showDrones = true;

        public PilotList(StrategyGame game)
        {
            InitializeComponent();

            _game = game;
        }

        private void PilotList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F4 && e.Alt) || e.KeyCode == Keys.F6 || e.KeyCode == Keys.Escape)
            {
                SoundEffect.Play(ESounds.windowslides);
                Hide();
                GC.Collect();
            }
        }

        public void RefreshPilotList()
        {
            var ships = _game.AllUnits.Where(_ => _.Active && _.Team == 1 && !Ship.IsTower(_.Type) && _.Type != EShipType.Lifepod).ToList();

            if (!_showCaps) ships.RemoveAll(_ => Ship.IsCapitalShip(_.Type));
            if (!_showCons) ships.RemoveAll(_ => _.Type == EShipType.Constructor);
            if (!_showMiners) ships.RemoveAll(_ => _.Type == EShipType.Miner);
            if (!_showNonCaps) ships.RemoveAll(_ => Ship.IsNonCapitalShip(_.Type));
            if (!_showDrones) ships.RemoveAll(_ => Ship.IsDroneShip(_.Type));

            var currentShips = new List<Ship>();
            for (var i = 0; i < PilotItems.Controls.Count; i++)
            {
                var item = PilotItems.Controls[i] as PilotListItem;

                if (item == null) continue;

                if (!ships.Contains(item.Pilot))
                {
                    item.Dispose();
                    PilotItems.Controls.Remove(item);
                    i--;
                }
                else
                {
                    currentShips.Add(item.Pilot);
                    item.RefreshPilot();
                }
            }

            var newShips = ships.Except(currentShips).ToList();

            foreach (var s in newShips)
            {
                PilotItems.Controls.Add(new PilotListItem(_game, s));
            }
        }

        private void ToggleFilter(ref bool filter)
        {
            filter = !filter;
            UpdateFilters();
        }

        private void ToggleShowOnly(ref bool filter)
        {
            if (filter)
            {
                SelectAll();
                return;
            }

            _showCaps = _showNonCaps = _showCons = _showMiners = _showDrones = false;

            filter = true;
            UpdateFilters();
        }

        private void SelectAll()
        {
            _showCaps = _showNonCaps = _showCons = _showMiners = _showDrones = true;
            UpdateFilters();
        }

        private void UpdateFilters()
        {
            FilterCons.BackColor = _showCons ? Color.DarkGreen : Color.Black;
            FilterMiners.BackColor = _showMiners ? Color.DarkGreen : Color.Black;
            FilterSmall.BackColor = _showNonCaps ? Color.DarkGreen : Color.Black;
            FilterCap.BackColor = _showCaps ? Color.DarkGreen : Color.Black;
            FilterDrones.BackColor = _showDrones ? Color.DarkGreen : Color.Black;

            RefreshPilotList();
        }


        private void FilterCons_Click(object sender, System.EventArgs e)
        {
            ToggleFilter(ref _showCons);
        }

        private void FilterMiners_Click(object sender, System.EventArgs e)
        {
            ToggleFilter(ref _showMiners);
        }

        private void FilterCap_Click(object sender, System.EventArgs e)
        {
            ToggleFilter(ref _showCaps);
        }

        private void FilterSmall_Click(object sender, EventArgs e)
        {
            ToggleFilter(ref _showNonCaps);
        }

        private void FilterDrones_Click(object sender, EventArgs e)
        {
            ToggleFilter(ref _showDrones);
        }


        private void FilterMiners_DoubleClick(object sender, EventArgs e)
        {
            ToggleShowOnly(ref _showMiners);
        }

        private void FilterSmall_DoubleClick(object sender, EventArgs e)
        {
            ToggleShowOnly(ref _showNonCaps);
        }

        private void FilterCons_DoubleClick(object sender, EventArgs e)
        {
            ToggleShowOnly(ref _showCons);
        }

        private void FilterCap_DoubleClick(object sender, EventArgs e)
        {
            ToggleShowOnly(ref _showCaps);
        }

        private void FilterDrones_DoubleClick(object sender, EventArgs e)
        {
            ToggleShowOnly(ref _showDrones);
        }
    }
}
