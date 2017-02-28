using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class PilotList : Form
    {
        public PilotList()
        {
            InitializeComponent();
        }

        private void PilotList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6 || e.KeyCode == Keys.Escape)
            {
                SoundEffect.Play(ESounds.windowslides);
                Hide();
            }
        }

        public void RefreshPilotList()
        {
            var ships = StrategyGame.AllUnits.Where(_ => _.Active && _.Team == 1).ToList();
            var currentShips = new List<Ship>();
            for (var i = 0; i < PilotItems.Controls.Count; i++)
            {
                var item = PilotItems.Controls[i] as PilotListItem;

                if (item == null) continue;

                if (!ships.Contains(item.Pilot))
                {
                    PilotItems.Controls.RemoveAt(i);
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
                PilotItems.Controls.Add(new PilotListItem(s));
            }
        }
    }
}
