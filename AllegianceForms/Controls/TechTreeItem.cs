using AllegianceForms.Engine;
using AllegianceForms.Engine.Tech;
using AllegianceForms.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.Controls
{
    public partial class TechTreeItem : UserControl
    {
        public TechItem Item { get; set; }

        private static readonly Color NormColour = Color.Black;
        private static readonly Color BuildColour = Color.DarkGreen;
        private static readonly Color UnlockedColour = Color.DarkGreen;
        private static readonly Color CantBuildColour = Color.DarkRed;
        private StrategyGame _game;
        private Research _researchForm;

        public TechTreeItem(StrategyGame game, Research ownerForm)
        {
            InitializeComponent();
            _game = game;
            _researchForm = ownerForm;
        }
        
        private void Name_MouseLeave(object sender, EventArgs e)
        {
            BackColor = Item.Unlocked ? UnlockedColour : NormColour;
        }

        private void Name_MouseEnter(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mouseover);
            RefreshBackColour(true);
        }

        public void RefreshBackColour(bool mouseOver = false)
        {
            if (!mouseOver && BackColor == NormColour) return;

            BackColor = (Item.CanBuild()) ? BuildColour : CantBuildColour;
        }

        public void SetInfo(TechItem item)
        {
            Item = item;

            TechName.Text = item.Name;
            TechAmount.Text = $"${item.Cost}";
            TechDuration.Text = $"({item.DurationTicks / 4}s)";
            Shortcut.Text = item.ShortcutKey;

            BackColor = NormColour;

            if (!string.IsNullOrWhiteSpace(item.Icon))
                TechIcon.Image = Image.FromFile(StrategyGame.IconPicDir + item.Icon);

            InvestmentProgress.Maximum = item.Cost;
            TimeProgress.Maximum = item.DurationTicks;
            UpdateTime();

            if (Item.Unlocked)
            {
                BackColor = UnlockedColour;
                InvestmentProgress.Value = InvestmentProgress.Maximum;
                TimeProgress.Value = TimeProgress.Maximum;
            }
        }

        public void UpdateTime()
        {
            TimeProgress.Value = Math.Min(Item.ResearchedTicks, TimeProgress.Maximum);
            InvestmentProgress.Value = Math.Min(Item.AmountInvested, InvestmentProgress.Maximum);
        }

        private void Name_Click(object sender, EventArgs e)
        {
            var me = e as MouseEventArgs;
            if (me == null || Item.AmountInvested == Item.Cost) return;
            if (me.Button != MouseButtons.Right) return;
            if (!Item.CanBuild()) return;
            
            SoundEffect.Play(ESounds.mousedown);
            // Left Click adds 20% or what you can afford if less, 5th adds 19%, 6th adds final 1%
            var perc = 0.2;
            var investedPerc = 1.0 * Item.AmountInvested / Item.Cost;
            if (investedPerc >= 0.99)
                perc = 1 - investedPerc;
            else if (investedPerc >= 0.8)
                perc = 1 - investedPerc - 0.01;

            var amount = (int)Math.Round(Item.Cost * perc);

            if (_game != null)
                amount = _game.SpendCredits(1, amount);
            else if (_researchForm != null)
                amount = _researchForm.SpendCurrency(amount);

            Item.AmountInvested += amount;
            InvestmentProgress.Value = Item.AmountInvested;
        }

        public void Invest()
        {
            Name_DoubleClick(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }

        private void Name_DoubleClick(object sender, EventArgs e)
        {
            var me = e as MouseEventArgs;
            if (me == null || Item.AmountInvested == Item.Cost) return;
            if (me.Button != MouseButtons.Left) return;
            if (!Item.CanBuild()) return;
            
            SoundEffect.Play(ESounds.mousedown);

            // Double Click pays it out completely, or all you can afford
            var amount = Item.Cost - Item.AmountInvested;

            if (_game != null)
                amount = _game.SpendCredits(1, amount);
            else if (_researchForm != null)
                amount = _researchForm.SpendCurrency(amount);

            Item.AmountInvested += amount;
            InvestmentProgress.Value = Item.AmountInvested;
        }
    }
}
