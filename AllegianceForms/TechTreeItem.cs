using AllegianceForms.Engine;
using AllegianceForms.Engine.Tech;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms
{
    public partial class TechTreeItem : UserControl
    {
        public TechItem Item { get; set; }
        private readonly Color _normColour = Color.Black;
        private readonly Color _buildColour = Color.DarkGreen;
        private readonly Color _cantBuildColour = Color.DarkRed;

        public TechTreeItem()
        {
            InitializeComponent();
        }
        
        private void Name_MouseLeave(object sender, EventArgs e)
        {
            BackColor = _normColour;
        }

        private void Name_MouseEnter(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mouseover);
            RefreshBackColour(true);
        }

        public void RefreshBackColour(bool mouseOver = false)
        {
            if (!mouseOver && BackColor == _normColour) return;

            BackColor = (Item.CanBuild()) ? _buildColour : _cantBuildColour;
        }

        public void SetInfo(TechItem item)
        {
            Item = item;

            TechName.Text = item.Name;

            InvestmentProgress.Maximum = item.Cost;
            TimeProgress.Maximum = item.DurationSec;

            UpdateTime();
        }

        public void UpdateTime()
        {
            TimeProgress.Value = (int)Item.TimeResearched;
            InvestmentProgress.Value = Item.AmountInvested;
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
            
            var amount = StrategyGame.SpendCredits(1, (int)Math.Round(Item.Cost * perc));
            Item.AmountInvested += amount;

            InvestmentProgress.Value = Item.AmountInvested;
        }

        private void Name_DoubleClick(object sender, EventArgs e)
        {
            var me = e as MouseEventArgs;
            if (me == null || Item.AmountInvested == Item.Cost) return;
            if (me.Button != MouseButtons.Left) return;
            if (!Item.CanBuild()) return;
            
            SoundEffect.Play(ESounds.mousedown);

            // Double Click pays it out completely, or all you can afford
            var amount = StrategyGame.SpendCredits(1, Item.Cost - Item.AmountInvested);
            Item.AmountInvested += amount;

            InvestmentProgress.Value = Item.AmountInvested;
        }
    }
}
