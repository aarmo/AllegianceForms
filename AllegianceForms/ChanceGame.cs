using AllegianceForms.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AllegianceForms
{
    public partial class ChanceGame : Form
    {
        private Dictionary<EShipType, ChanceGameRule> _rules;
        private string _keyLines;
        private bool _gameRunning;
        private bool _rulesShown;
        private int _timerState;
        private int[] _score = new int[2];
        private Random _rnd = new Random();
        private Keys _playersKey = Keys.None;


        private const string StartGameText = "Start Game";
        private const string StopGameText = "Stop Game";

        private const string TitleText = "Dogfight";
        private const string RuleText = "Rules";
        private const string GetReadyText = "Get Ready";
        private const string KeyPressText = "Fight!";
        private const string NoneText = "Too Slow";
        private const string WrongText = "Wrong Key";
        private const string ErrorText = "Error!";
        private const string TieText = "Tie!";
        private const string WinText = "You Win!";
        private const string LoseText = "You Lose!";

        private const int StarSize = 100;
        private const int MaxScore = 5;
        private const int MaxTimerState = 5;

        public ChanceGame()
        {
            InitializeComponent();

            _rules = new Dictionary<EShipType, ChanceGameRule>();

            // Scout DETECTS Stealth Fighter
            // Scout OUT RUNS Fighter
            _rules.Add(EShipType.Scout, new ChanceGameRule
            {
                ShipCenter = new PointF(Width / 2, Height / 2 - StarSize),
                ShipType = EShipType.Scout,
                ShipChar = "S",
                WinLabel1 = "DETECTS",
                DefeatsType1 = EShipType.StealthFighter,
                WinLabel2 = "OUT RUNS",
                DefeatsType2 = EShipType.Fighter,
            });
            // Stealth Fighter SNEAKS UP ON Interceptor
            // Stealth Fighter DEFEATS Gunship
            _rules.Add(EShipType.StealthFighter, new ChanceGameRule
            {
                ShipCenter = new PointF(Width / 2 + StarSize, Height / 2),
                ShipType = EShipType.StealthFighter,
                ShipChar = "T",
                WinLabel1 = "OUT RANGES",
                DefeatsType1 = EShipType.Interceptor,
                WinLabel2 = "DEFEATS",
                DefeatsType2 = EShipType.Gunship,
            });
            // Interceptor DESTROYS Fighter
            // Interceptor CATCHES Scout
            _rules.Add(EShipType.Interceptor, new ChanceGameRule
            {
                ShipCenter = new PointF(Width / 2 + StarSize / 2, Height / 2 + StarSize),
                ShipType = EShipType.Interceptor,
                ShipChar = "I",
                WinLabel1 = "DESTROYS",
                DefeatsType1 = EShipType.Fighter,
                WinLabel2 = "CATCHES",
                DefeatsType2 = EShipType.Scout,
            });
            // Fighters OVERWHELMS Gunship
            // Fighter DESTROYS Stealth Fighter
            _rules.Add(EShipType.Fighter, new ChanceGameRule
            {
                ShipCenter = new PointF(Width / 2 - StarSize / 2, Height / 2 + StarSize),
                ShipType = EShipType.Fighter,
                ShipChar = "F",
                WinLabel1 = "OVERWHELMS",
                DefeatsType1 = EShipType.Gunship,
                WinLabel2 = "DESTROYS",
                DefeatsType2 = EShipType.StealthFighter,
            });
            // Gunship OUT RANGES Scout
            // Gunship DESTROYS Interceptor
            _rules.Add(EShipType.Gunship, new ChanceGameRule
            {
                ShipCenter = new PointF(Width / 2 - StarSize, Height / 2),
                ShipType = EShipType.Gunship,
                ShipChar = "G",
                WinLabel1 = "OUT RANGES",
                DefeatsType1 = EShipType.Scout,
                WinLabel2 = "DESTROYS",
                DefeatsType2 = EShipType.Interceptor,
            });


            var sb = new StringBuilder();
            foreach (var k in _rules.Keys)
            {
                var v = _rules[k];
                sb.AppendLine(string.Format("{0}: {1}", v.ShipType, v.ShipChar));
            }

            _keyLines = sb.ToString();
            SoundEffect.Init(0.1f);
            SoundEffect.Play(ESounds.windowslides);
        }

        private void GameRules_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);

            if (_rulesShown)
            {
                WinLose.Text = TitleText;
                GameInfo.Text = "Good luck!";
                _rulesShown = false;
                return;
            }

            WinLose.Text = RuleText;
            var sb = new StringBuilder(Text);
            var sb2 = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine();

            foreach (var k in _rules.Keys)
            {
                var v = _rules[k];
                sb.AppendLine(string.Format("{0} **{1}** {2}", v.ShipType, v.WinLabel1, v.DefeatsType1));
                sb.AppendLine(string.Format("{0} **{1}** {2}", v.ShipType, v.WinLabel2, v.DefeatsType2));
            }
            sb.AppendLine();
            sb.AppendLine("CONTROLS:");
            sb.AppendLine(_keyLines);
            sb.AppendLine();
            sb.AppendLine("First to " + MaxScore);
            sb.AppendLine("Good luck!");
            sb.AppendLine(":)");

            GameInfo.Text = sb.ToString();
            _rulesShown = true;
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mousedown);
            if (StartGame.Text == StartGameText)
            {
                StartNewGame();
            }
            else
            {
                StopGame();
            }
        }

        private void StartNewGame()
        {
            SoundEffect.Play(ESounds.payday, true);
            _rulesShown = false;
            GameRules.Enabled = false;

            StartGame.Text = StopGameText;
            _score[0] = 0;
            _score[1] = 0;
            Score.Text = string.Format("{0} : {1}", _score[0], _score[1]);

            _gameRunning = true;
            _timerState = MaxTimerState+1;
            WinLose.Text = GetReadyText;

            GameInfo.Text = _keyLines;
        }

        private void StopGame()
        {
            _rulesShown = false;

            GameRules.Enabled = true;
            StartGame.Text = StartGameText;
            _gameRunning = false;
        }

        private void AddScore(int i, int points)
        {
            _score[i] += points;
            Score.Text = string.Format("{0} : {1}", _score[0], _score[1]);
        }

        private void CheckForWin()
        {
            var over = false;
            for (var i = 0; i < _score.Length; i++)
            {
                if (_score[i] >= MaxScore)
                {
                    StopGame();
                    over = true;

                    if (i == 0) GameInfo.Text = "\n\nCONGRATULATIONS\n\nYou reached the MAX SCORE\nWell done!\n\nThanks for playing\n:)";
                    if (i == 1) GameInfo.Text = "\n\nSORRY\n\nThe enemy reached the MAX SCORE\nBetter luck next time!\n\nThanks for playing\n:)";
                }
            }

            if (over)
            {
                SoundEffect.Play((_rnd.Next(2) == 0) ? ESounds.static1 : ESounds.static2);
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (!_gameRunning) return;

            _timerState--;

            switch(_timerState)
            {
                case MaxTimerState:
                    WinLose.Text = GetReadyText;
                    GameInfo.Text = _keyLines;
                    break;
                case MaxTimerState - 1:
                    WinLose.Text = "2";
                    SoundEffect.Play(ESounds.text);
                    break;
                case MaxTimerState - 2:
                    WinLose.Text = "1";
                    SoundEffect.Play(ESounds.text);
                    break;
                case MaxTimerState - 3:
                    WinLose.Text = KeyPressText;
                    _playersKey = Keys.None;
                    SoundEffect.Play(ESounds.newtarget);
                    break;
                case MaxTimerState - 4:
                    if (_playersKey == Keys.None)
                    {
                        SoundEffect.Play(ESounds.newtargetneutral);
                        WinLose.Text = NoneText;
                        GameInfo.Text = "Please press a key next time";
                        return;
                    }
                    CheckGameRules();
                    break;
                case MaxTimerState - 5:
                    _timerState = MaxTimerState + 1;
                    CheckForWin();
                    break;

            }

        }

        private void CheckGameRules()
        {
            var playerShip = _rules.Values.FirstOrDefault(_ => _.ShipChar == _playersKey.ToString());
            if (playerShip == null)
            {
                SoundEffect.Play(ESounds.newtargetneutral);
                WinLose.Text = WrongText;
                GameInfo.Text = "Please press the right key next time";
                return;
            }

            var keys = _rules.Keys.ToArray();
            var enemyShip = _rules[keys[_rnd.Next(keys.Length)]];
            var playerScores = new bool[_score.Length];
            var info = string.Empty;
            
            if (playerShip.ShipType == enemyShip.ShipType)
            {
                info = string.Format("( Both {0}s **EXPLODE** )", playerShip.ShipType);
                playerScores[0] = true;
                playerScores[1] = true;
            }
            else if (playerShip.DefeatsType1 == enemyShip.ShipType)
            {
                playerScores[0] = true;
                info = string.Format("( Your {0} **{1}** the enemy {2} )", playerShip.ShipType, playerShip.WinLabel1, enemyShip.ShipType);
            }
            else if (playerShip.DefeatsType2 == enemyShip.ShipType)
            {
                playerScores[0] = true;
                info = string.Format("( Your {0} **{1}** the enemy {2} )", playerShip.ShipType, playerShip.WinLabel2, enemyShip.ShipType);
            }
            else if (enemyShip.DefeatsType1 == playerShip.ShipType)
            {
                playerScores[1] = true;
                info = string.Format("( The enemy {0} **{1}** your {2} )", enemyShip.ShipType, enemyShip.WinLabel1, playerShip.ShipType);
            }
            else if (enemyShip.DefeatsType2 == playerShip.ShipType)
            {
                playerScores[1] = true;
                info = string.Format("( The enemy {0} **{1}** your {2} )", enemyShip.ShipType, enemyShip.WinLabel2, playerShip.ShipType);
            }
            else
            {
                WinLose.Text = "Error!";
                GameInfo.Text = "Please check the rules - unknown state!";
                return;
            }

            GameInfo.Text = info;

            for (var i = 0; i < _score.Length; i++)
            {
                if (playerScores[i]) AddScore(i, 1);
            }

            if (playerScores[0] && playerScores[1])
            {
                WinLose.Text = TieText;
                SoundEffect.Play(ESounds.small_explosion);
            }
            else if (playerScores[0])
            {
                WinLose.Text = WinText;
                SoundEffect.Play(ESounds.plasmaac1);
            }
            else
            {
                WinLose.Text = LoseText;
                SoundEffect.Play(ESounds.small_explosion);
            }
            
        }

        private void ChanceGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (WinLose.Text == KeyPressText)
            {
                SoundEffect.Play(ESounds.accept);
                _playersKey = e.KeyCode;
            }
            else
            {
                SoundEffect.Play(ESounds.outofammo);
            }
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
    }

    public class ChanceGameRule
    {
        public PointF ShipCenter { get; set; }
        public EShipType ShipType { get; set; }
        public string ShipChar { get; set; }
        public EShipType DefeatsType1 { get; set; }
        public string WinLabel1 { get; set; }
        public EShipType DefeatsType2 { get; set; }
        public string WinLabel2 { get; set; }
    }
}
