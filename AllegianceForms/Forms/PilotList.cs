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

        private void FilterCons_Click(object sender, System.EventArgs e)
        {
            _showCons = !_showCons;
            UpdateFilters();
        }

        private void UpdateFilters()
        {
            FilterCons.BackColor = _showCons ? Color.DarkGreen : Color.Black;
            FilterMiners.BackColor = _showMiners ? Color.DarkGreen : Color.Black;
            FilterSmall.BackColor = _showNonCaps ? Color.DarkGreen : Color.Black;
            FilterCap.BackColor = _showCaps ? Color.DarkGreen : Color.Black;

            RefreshPilotList();
        }

        private void FilterMiners_Click(object sender, System.EventArgs e)
        {
            _showMiners = !_showMiners;
            UpdateFilters();
        }

        private void FilterCap_Click(object sender, System.EventArgs e)
        {
            _showCaps = !_showCaps;
            UpdateFilters();
        }

        private void FilterSmall_Click(object sender, EventArgs e)
        {
            _showNonCaps = !_showNonCaps;
            UpdateFilters();
        }

        private void FilterMiners_DoubleClick(object sender, EventArgs e)
        {
            if (_showMiners)
            {
                SelectAll();
                return;
            }

            _showMiners = true;
            _showCons = _showCaps = _showNonCaps = false;
            UpdateFilters();
        }

        private void FilterSmall_DoubleClick(object sender, EventArgs e)
        {
            if (_showNonCaps)
            {
                SelectAll();
                return;
            }

            _showNonCaps = true;
            _showCons = _showCaps = _showMiners = false;
            UpdateFilters();
        }

        private void FilterCons_DoubleClick(object sender, EventArgs e)
        {
            if (_showCons)
            {
                SelectAll();
                return;
            }

            _showCons = true;
            _showNonCaps = _showCaps = _showMiners = false;
            UpdateFilters();
        }

        private void FilterCap_DoubleClick(object sender, EventArgs e)
        {
            if (_showCaps) 
            {
                SelectAll(); 
                return;
            }

            _showCaps = true;
            _showNonCaps = _showCons = _showMiners = false;
            UpdateFilters();
        }

        private void SelectAll()
        {
            _showCaps = _showNonCaps = _showCons = _showMiners = true;
            UpdateFilters();
        }
    }
}
