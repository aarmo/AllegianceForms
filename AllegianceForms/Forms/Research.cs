using AllegianceForms.Engine;
using AllegianceForms.Engine.Tech;
using AllegianceForms.Controls;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace AllegianceForms.Forms
{
    public partial class Research : Form
    {
        private ETechType _type = ETechType.Starbase;

        private readonly Color _backColorType = Color.DimGray;
        private readonly Color _backColorNotType = Color.Black;
        private readonly Color _foreColorActive = Color.White;
        private readonly Color _foreColorInActive = Color.Gray;

        public Research()
        {
            InitializeComponent();
        }

        public void RefreshItems()
        {
            ResearchItems.Controls.Clear();

            ConstructionButton.BackColor = (_type == ETechType.Construction) ? _backColorType : _backColorNotType;

            StarbaseButton.ForeColor = (StrategyGame.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Starbase)) ? _foreColorActive : _foreColorInActive;
            StarbaseButton.BackColor = (_type == ETechType.Starbase) ? _backColorType : _backColorNotType;

            SupremacyButton.ForeColor = (StrategyGame.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Supremacy)) ? _foreColorActive : _foreColorInActive;
            SupremacyButton.BackColor = (_type == ETechType.Supremacy) ? _backColorType : _backColorNotType;

            TacticalButton.ForeColor = (StrategyGame.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Tactical)) ? _foreColorActive : _foreColorInActive;
            TacticalButton.BackColor = (_type == ETechType.Tactical) ? _backColorType : _backColorNotType;

            ExpansionButton.ForeColor = (StrategyGame.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Expansion)) ? _foreColorActive : _foreColorInActive;
            ExpansionButton.BackColor = (_type == ETechType.Expansion) ? _backColorType : _backColorNotType;

            ShipyardButton.ForeColor = (StrategyGame.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Shipyard)) ? _foreColorActive : _foreColorInActive;
            ShipyardButton.BackColor = (_type == ETechType.ShipyardConstruction) ? _backColorType : _backColorNotType;


            var items = StrategyGame.TechTree[0].ResearchableItems(_type);
            foreach (var i in items)
            {
                var c = new TechTreeItem();
                c.SetInfo(i);

                ResearchItems.Controls.Add(c);
            }
    }

        private void Research_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 || e.KeyCode == Keys.Escape)
            {
                SoundEffect.Play(ESounds.windowslides);
                Hide();
            }
        }

        public void UpdateItems()
        {
            var activeItems = StrategyGame.TechTree[0].ResearchableItems(_type);
            var removeControls = new List<Control>();

            // Update
            foreach (var c in ResearchItems.Controls)
            {
                var item = c as TechTreeItem;
                if (item == null) continue;

                if (item.Item.Type == _type) item.RefreshBackColour();

                if (activeItems.Contains(item.Item))
                {
                    item.UpdateTime();
                    activeItems.Remove(item.Item);
                }
                else
                {
                    removeControls.Add((Control)c);
                }
            }

            // Remove
            removeControls.ForEach(ResearchItems.Controls.Remove);

            // Insert
            foreach (var i in activeItems)
            {
                var ui = new TechTreeItem();
                ui.SetInfo(i);

                ResearchItems.Controls.Add(ui);
            }
        }

        private void ConstructionButton_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            _type = ETechType.Construction;
            RefreshItems();
        }

        private void StarbaseButton_Click(object sender, EventArgs e)
        {
            if (StarbaseButton.ForeColor != _foreColorActive) return;
            SoundEffect.Play(ESounds.mousedown);
            _type = ETechType.Starbase;
            RefreshItems();
        }

        private void SupremacyButton_Click(object sender, EventArgs e)
        {
            if (SupremacyButton.ForeColor != _foreColorActive) return;
            SoundEffect.Play(ESounds.mousedown);
            _type = ETechType.Supremacy;
            RefreshItems();
        }

        private void TacticalButton_Click(object sender, EventArgs e)
        {
            if (TacticalButton.ForeColor != _foreColorActive) return;
            SoundEffect.Play(ESounds.mousedown);
            _type = ETechType.Tactical;
            RefreshItems();
        }

        private void ExpansionButton_Click(object sender, EventArgs e)
        {
            if (ExpansionButton.ForeColor != _foreColorActive) return;
            SoundEffect.Play(ESounds.mousedown);
            _type = ETechType.Expansion;
            RefreshItems();
        }

        private void ConstructionButton_MouseEnter(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null || b.ForeColor != _foreColorActive) return;
            SoundEffect.Play(ESounds.mouseover);
        }

        private void ShipyardButton_Click(object sender, EventArgs e)
        {
            if (ShipyardButton.ForeColor != _foreColorActive) return;
            SoundEffect.Play(ESounds.mousedown);
            _type = ETechType.ShipyardConstruction;
            RefreshItems();
        }
    }
}
