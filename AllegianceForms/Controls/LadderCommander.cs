using System.Windows.Forms;
using AllegianceForms.Engine.Factions;

namespace AllegianceForms.Controls
{
    public partial class LadderCommander : UserControl
    {
        public Faction Commander;

        public LadderCommander(Faction f)
        {
            InitializeComponent();

            Commander = f;
            RefreshCommander();
        }
        
        private void RefreshCommander()
        {
            CommanderName.Text = Commander.CommanderName;
            RankNumber.Text = Commander.CommanderRankPoints.ToString();
            GamesPlayed.Text = $"{Commander.LadderGamesPlayed} ({Commander.LadderGamesWon} / {Commander.LadderGamesLost})";
            Avatar.Image = Utils.GetAvatarImage(Commander.PictureCode);
        }
    }
}
