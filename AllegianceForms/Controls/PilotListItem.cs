using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.Controls
{
    public partial class PilotListItem : UserControl
    {
        public Ship Pilot { get; set; }

        public PilotListItem(Ship ship)
        {
            InitializeComponent();
            Pilot = ship;

            RefreshPilot();
        }

        public void RefreshPilot()
        {
            if (Pilot == null) return;

            var m = Pilot as MinerShip;
            var b = Pilot as BuilderShip;

            var info = m != null ? string.Format(" ({0:P})", m.Resources * 1.0f / m.MaxResourceCapacity) : string.Empty;
            if (Pilot.Docked) info += "\n[Docked]";

            Image.Image = Pilot.Image;

            ShipType.Text = Pilot.Type.ToString();
            if (b != null) ShipType.Text += string.Format("\n[{0}]", b.BaseType);

            Sector.Text = string.Format("[{0}] {1}", Pilot.SectorId+1, StrategyGame.Map.Sectors[Pilot.SectorId].Name);            
            Info.Text = string.Format("{0:P} {1}", Pilot.Health * 1.0f / Pilot.MaxHealth, info);
        }

        private void Info_MouseEnter(object sender, System.EventArgs e)
        {
            BackColor = Color.DarkGreen;
            ForeColor = Color.White;

            SoundEffect.Play(ESounds.mouseover);
        }

        private void Info_MouseLeave(object sender, System.EventArgs e)
        {
            BackColor = Color.Black;
            ForeColor = Color.Silver;
        }

        private void Info_DoubleClick(object sender, System.EventArgs e)
        {
            StrategyGame.OnGameEvent(Pilot, EGameEventType.ShipClicked);
            SoundEffect.Play(ESounds.mousedown);
        }
    }
}
