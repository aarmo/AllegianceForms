using AllegianceForms.Engine;
using AllegianceForms.Engine.Factions;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            SoundEffect.Init();
            SoundEffect.Play(ESounds.windowslides);

            AppVersion.Text = string.Format("(ALPHA) v{0}", Assembly.GetEntryAssembly().GetName().Version);

            if (_ladderGame.LadderStarted) Ladder.Text = "Continue Ladder";
        }

        private void Dogfight_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            var f = new ChanceGame();
            f.ShowDialog(this);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            Application.Exit();
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

        private GameSettings _gameSettings = GameSettings.Default();
        private LadderGame _ladderGame = LadderGame.LoadOrDefault();

        private void CustomGame_Click(object sender, EventArgs e)
        {
            Faction.FactionNames.Reset();
            SoundEffect.Play(ESounds.mousedown);
            var f = new CustomiseSetttings();
            f.LoadSettings(_gameSettings);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                _gameSettings = f.Settings;
                var f2 = new Sector(_gameSettings);
                if (!f2.IsDisposed) f2.Show();
            }
        }

        private void MapDesigner_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            var f = new MapDesigner();
            f.ShowDialog(this);
        }

        private void Ladder_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            
            var f = new Ladder();
            f.LoadLadder(_ladderGame);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                StartLadderGame();
            }
            else
            {
                // Reset
                if (_ladderGame.Abandoned)
                {
                    _ladderGame = LadderGame.Default();
                    _ladderGame.SaveLadder();
                }
            }                        
        }

        private void StartLadderGame()
        {
            // Get teams
            Faction[] team1 = null;
            var team2 = _ladderGame.GetCommandersForPlayerToFight(ref team1);
            if (team1 == null || team2 == null || team1.Length != team2.Length) return;

            // Play the game!
            var f2 = new Sector(GameSettings.LadderDefault(_ladderGame, team1, team2));
            f2.Show();

            f2.GameOverEvent += Ladder_GameOverEvent;
        }

        private void Ladder_GameOverEvent(object sender)
        {
            var f2 = sender as Sector;
            if (f2 == null) return;

            // If you leave early, you loose. Sorry :(
            _ladderGame.UpdateCommanderRanks(f2.StrategyGame.Winners, f2.StrategyGame.Loosers);

            // Update other players & save results
            _ladderGame.PlayGamesForAllOtherPlayers();
            _ladderGame.SaveLadder();

            // TODO: Promotions, tiers, demotions, etc.

            Ladder.Text = "Continue Ladder";
        }
    }
}
