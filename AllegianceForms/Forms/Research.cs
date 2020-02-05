using AllegianceForms.Engine;
using AllegianceForms.Controls;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using AllegianceForms.Engine.Tech;

namespace AllegianceForms.Forms
{
    public partial class Research : Form
    {
        public int Currency { get; set; }

        private ETechType _type = ETechType.Construction;

        private readonly Color _backColorType = Color.DimGray;
        private readonly Color _backColorNotType = Color.Black;
        private readonly Color _foreColorActive = Color.White;
        private readonly Color _foreColorInActive = Color.Gray;

        private Dictionary<string, TechTreeItem> _techTreeItems = new Dictionary<string, TechTreeItem>();

        private StrategyGame _game;
        private TechTree _tree;

        public Research(StrategyGame game)
        {
            InitializeComponent();

            _game = game;
            _tree = game.TechTree[0];
            RefreshItems();
        }

        public Research(TechTree tree, int[] unlockedTechIds, int currency)
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            OkButton.Visible = CancelSetButton.Visible = CurrencyPoints.Visible = CurrencyTitle.Visible = true;

            _game = null;
            _tree = tree;
            Currency = currency;
            CurrencyPoints.Text = $"{Currency}";
            RefreshItems();
        }

        public void RefreshItems()
        {
            ResearchItems.Controls.Clear();

            ConstructionButton.BackColor = (_type == ETechType.Construction) ? _backColorType : _backColorNotType;
            if (_game == null)
            {
                StarbaseButton.ForeColor = SupremacyButton.ForeColor = TacticalButton.ForeColor = ExpansionButton.ForeColor = ShipyardButton.ForeColor = _foreColorActive;
            }
            else
            {
                StarbaseButton.ForeColor = (_game.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Starbase)) ? _foreColorActive : _foreColorInActive;
                SupremacyButton.ForeColor = (_game.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Supremacy)) ? _foreColorActive : _foreColorInActive;
                TacticalButton.ForeColor = (_game.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Tactical)) ? _foreColorActive : _foreColorInActive;
                ExpansionButton.ForeColor = (_game.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Expansion)) ? _foreColorActive : _foreColorInActive;
                ShipyardButton.ForeColor = (_game.AllBases.Any(_ => _.Active && _.Team == 1 && _.Type == EBaseType.Shipyard)) ? _foreColorActive : _foreColorInActive;
            }

            StarbaseButton.BackColor = (_type == ETechType.Starbase) ? _backColorType : _backColorNotType;
            SupremacyButton.BackColor = (_type == ETechType.Supremacy) ? _backColorType : _backColorNotType;
            TacticalButton.BackColor = (_type == ETechType.Tactical) ? _backColorType : _backColorNotType;
            ExpansionButton.BackColor = (_type == ETechType.Expansion) ? _backColorType : _backColorNotType;
            ShipyardButton.BackColor = (_type == ETechType.ShipyardConstruction) ? _backColorType : _backColorNotType;

            var items = _tree.ResearchableItems(_type);
            foreach (var i in items)
            {
                TechTreeItem c;

                // Reuse the Tech Tree control if it is still researchable
                if (_techTreeItems.ContainsKey(i.Name))
                {
                    c = _techTreeItems[i.Name];
                    c.SetInfo(i);
                }
                else
                {
                    c = new TechTreeItem(_game, this);
                    c.SetInfo(i);
                    _techTreeItems.Add(i.Name, c);
                }                

                ResearchItems.Controls.Add(c);
            }
        }

        public int SpendCurrency(int amount)
        {
            var spentAmount = Math.Min(amount, Currency);
            Currency -= spentAmount;
            CurrencyPoints.Text = $"{Currency}";

            return spentAmount;
        }

        private void Research_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F4 && e.Alt) || e.KeyCode == Keys.F5 || e.KeyCode == Keys.Escape)
            {
                SoundEffect.Play(ESounds.windowslides);
                Hide();
                GC.Collect();
                return;
            }

            var ks = e.KeyCode.ToString();
            foreach (var c in ResearchItems.Controls)
            {
                var t = c as TechTreeItem;
                if (t == null) continue;

                if (t.Item.ShortcutKey == ks)
                {
                    t.Invest();
                    return;
                }
            }

            switch (e.KeyCode)
            {
                case Keys.D1:
                    ConstructionButton_Click(sender, null);
                    break;
                case Keys.D2:
                    StarbaseButton_Click(sender, null);
                    break;
                case Keys.D3:
                    SupremacyButton_Click(sender, null);
                    break;
                case Keys.D4:
                    TacticalButton_Click(sender, null);
                    break;
                case Keys.D5:
                    ExpansionButton_Click(sender, null);
                    break;
                case Keys.D6:
                    ShipyardButton_Click(sender, null);
                    break;
            }
        }

        public void UpdateItems()
        {
            var activeItems = _tree.ResearchableItems(_type);
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
                var ui = new TechTreeItem(_game, this);
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
            b.BackColor = Color.DarkGreen;
            SoundEffect.Play(ESounds.mouseover);
        }

        private void ShipyardButton_Click(object sender, EventArgs e)
        {
            if (ShipyardButton.ForeColor != _foreColorActive) return;
            SoundEffect.Play(ESounds.mousedown);
            _type = ETechType.ShipyardConstruction;
            RefreshItems();
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

        private void ConstructionButton_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;

            b.BackColor = _type == ETechType.Construction ? _backColorType : _backColorNotType;
        }

        private void StarbaseButton_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;

            b.BackColor = _type == ETechType.Starbase ? _backColorType : _backColorNotType;
        }

        private void SupremacyButton_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;

            b.BackColor = _type == ETechType.Supremacy ? _backColorType : _backColorNotType;
        }

        private void TacticalButton_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;

            b.BackColor = _type == ETechType.Tactical ? _backColorType : _backColorNotType;
        }

        private void ExpansionButton_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;

            b.BackColor = _type == ETechType.Expansion ? _backColorType : _backColorNotType;
        }

        private void ShipyardButton_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;

            b.BackColor = _type == ETechType.ShipyardConstruction ? _backColorType : _backColorNotType;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            DialogResult = DialogResult.OK;
        }

        private void CancelSetButton_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            DialogResult = DialogResult.Cancel;
        }
    }
}
