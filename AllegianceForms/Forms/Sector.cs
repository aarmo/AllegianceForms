using AllegianceForms.Engine.AI;
using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Map;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AllegianceForms.Engine.Factions;
using AllegianceForms.Engine.QuickChat;
using AllegianceForms.Engine.Generation;

namespace AllegianceForms.Forms
{
    public partial class Sector : Form
    {
        public delegate void GameOverHandler(object sender);
        public event GameOverHandler GameOverEvent;

        private readonly Bitmap _frame;
        private readonly List<Base> _selectedBases = new List<Base>();
        private readonly List<Ship> _selectedUnits = new List<Ship>();
        private readonly List<Animation> _explosions = new List<Animation>();
        private readonly List<Animation> _animations = new List<Animation>();
        private readonly List<string> _shipKeys;

        private Research _researchForm;
        private PilotList _pilotList;
        private Map _mapForm;
        private DebugAI _debugForm;
        private MapSector _currentSector;
        private List<QuickChatItem> _currentQuickList;
        private List<QuickChatItem> _currentQuickSubList;

        private readonly Pen _selectionPen;
        private readonly Brush _sensorBrush;
        private readonly Pen _sensorPen;
        private Point _selectionStart;
        private Point _selectionEnd;
        private Rectangle _selection;
        private bool _mouseDown;
        private bool _mouseDouble;
        private bool _shiftDown;
        private bool _ctrlDown;
        private EOrderType _orderType;
        private Random _rnd = new Random();
        private int _lastCredits = 0;
        private int _lastPilots = 0;
        private int _alertSectorId = -1;
        private DateTime _alertExpire = DateTime.MinValue;
        private TimeSpan _alertDuration = new TimeSpan(0, 0, 0, 3);
        private Color _colourTeam1;

        public StrategyGame StrategyGame = new StrategyGame();
        
        public Sector(GameSettings settings)
        {
            InitializeComponent();
            SetupScreenSize();

            _researchForm = new Research(StrategyGame);
            _pilotList = new PilotList(StrategyGame);

            StrategyGame.SetupGame(settings);
            StrategyGame.LoadData();
            StrategyGame.Map = GameMaps.LoadMap(StrategyGame, settings.MapName);
            _mapForm = new Map(StrategyGame);
            UpdateWinnersAndLoosers(false);

            var startSectors = (from s in StrategyGame.Map.Sectors
                                where s.StartingSector != 0
                                orderby s.StartingSector
                                select s).ToList();
            if (StrategyGame.Map.Name == "Brawl") startSectors.Add(startSectors[0]);

            if (startSectors.Count < StrategyGame.NumTeams && (StrategyGame.Map.Name != "Brawl" || StrategyGame.NumTeams > 2))
            {
                MessageBox.Show($"Sorry, the map '{StrategyGame.Map.Name}' doesn't support {StrategyGame.NumTeams} teams...", "Setup Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            StrategyGame.InitialiseGame();

            _frame = new Bitmap(Width, Height);
            _selectionPen = new Pen(Color.LightGray, 1F) {DashStyle = DashStyle.Dot};
            _colourTeam1 = Color.FromArgb(settings.TeamColours[0]);
            _sensorPen = new Pen(StrategyGame.NewAlphaColour(20, _colourTeam1), 1F) { DashStyle = DashStyle.Dash };
            _sensorBrush = new SolidBrush(StrategyGame.NewAlphaColour(5, _colourTeam1));
            _shipKeys = StrategyGame.Ships.Ships.Select(_ => _.Key).ToList();
            _currentSector = startSectors[0];
            StrategyGame.PlayerCurrentSectorId = _currentSector.Id;
            GetCurrentSectorBounds();

            SectorLabel.Text = _currentSector.Name;
            LoadQuickMenu(0);

            // Friendy & enemy team setup:
            for (var t = 0; t < StrategyGame.NumTeams; t++)
            {
                var team = t + 1;
                var startingSector = startSectors[t];
                var teamColour = Color.FromArgb(StrategyGame.GameSettings.TeamColours[t]);
                var aiPlayer = t != 0;

                var b1 = StrategyGame.Bases.CreateBase(EBaseType.Starbase, team, teamColour, startingSector.Id, false);
                b1.CenterX = Width / 2 + (aiPlayer ?  300 : -300);
                b1.CenterY = Height / 2 + (aiPlayer ? 300 : -300);
                b1.BaseEvent += B_BaseEvent;
                StrategyGame.AddBase(b1);
                StrategyGame.GameStats.TotalBasesBuilt[t] = 1;

                for (var i = 0; i < settings.MinersInitial; i++)
                {
                    var startingMiner = StrategyGame.Ships.CreateMinerShip(team, teamColour, startingSector.Id);
                    startingMiner.CenterX = b1.CenterX + (aiPlayer ?  -100 - (i * 30) : 100 + (i * 30));
                    startingMiner.CenterY = b1.CenterY + (aiPlayer ?  -40 : 40);
                    startingMiner.ShipEvent += F_ShipEvent;
                    StrategyGame.AddUnit(startingMiner);
                }

                if (aiPlayer)
                {
                    // AI setup:
                    BaseAI ai;

                    if (settings.VariantAi)
                    {
                        ai = new VariantAI(StrategyGame, team, teamColour, F_ShipEvent);
                    }
                    else
                    {
                        ai = new CommanderAI(StrategyGame, team, teamColour, F_ShipEvent, true);
                    }

                    StrategyGame.AICommanders[t] = ai;
                    StrategyGame.AICommanders[t].SetDifficulty(settings.AiDifficulty);
                    StrategyGame.DockedPilots[t] = (int)(settings.NumPilots * StrategyGame.AICommanders[t].CheatAdditionalPilots);

                    if (t == 1 && !settings.VariantAi)
                    {
                        var comAi = ai as CommanderAI;
                        _debugForm = new DebugAI(StrategyGame, comAi);
                    }

                    //ai.Enabled = false;
                    //enemyAIDebugToolStripMenuItem_Click(null, null);
                }
            }

            // Explosions!
            var explosionFrames = StrategyGame.GetExplosionFrames();

            for (var i = 0; i < 30; i++)
            {
                var a = new Animation(explosionFrames, 0, 0, 16, 16, 1, false);
                _explosions.Add(a);
            }

            // Final setup:
            _currentSector.VisibleToTeam[0] = true;
            StrategyGame.UpdateVisibility(true);
            StrategyGame.GameEvent += StrategyGame_GameEvent;

            miniMapToolStripMenuItem_Click(null, null);

            timer.Enabled = tick.Enabled = true;
        }

        private void LoadQuickMenu(int m)
        {
            QuickItems.Controls.Clear();
            _currentQuickList = StrategyGame.QuickChat.QuickItems.Where(_ => _.MenuId == m).ToList();
            foreach (var q in _currentQuickList)
            {
                QuickItems.Controls.Add(new Controls.QuickChatItem(q));
            }
        }

        private void LoadQuickSubMenu(int m)
        {
            QuickItems2.Controls.Clear();
            _currentQuickSubList = StrategyGame.QuickChat.QuickItems.Where(_ => _.MenuId == m).ToList();
            foreach (var q in _currentQuickSubList)
            {
                QuickItems2.Controls.Add(new Controls.QuickChatItem(q));
            }
        }

        private void SetupScreenSize()
        {
            var scr = Screen.FromControl(this);
            var area = scr.WorkingArea;

            StrategyGame.ScreenWidth = Width = area.Width + StrategyGame.ScreenPositionOffset_Width;
            StrategyGame.ScreenHeight = Height = area.Height + StrategyGame.ScreenPositionOffset_Height;
            Top = StrategyGame.ScreenPositionOffset_Top;
            Left = StrategyGame.ScreenPositionOffset_Left;
        }

        private void StrategyGame_GameEvent(object sender, EGameEventType e)
        {
            if (e == EGameEventType.SectorLeftClicked)
            {
                var s = sender as MapSector;
                if (s == null) return;

                SoundEffect.Play(ESounds.text);
                SwitchSector(s.Id+1);
            }
            else if (e == EGameEventType.SectorRightClicked)
            {
                var s = sender as MapSector;
                if (s == null || _selectedUnits.Count == 0) return;

                // Order selected units to navigate to the clicked sector
                _selectedUnits.ForEach(_ => _.OrderShip(new NavigateOrder(StrategyGame, _, s.Id), _shiftDown));
                PlayOrderSound();

                ClearSelected();
                Focus();
            }
            else if (e == EGameEventType.ShipClicked)
            {
                var s = sender as Ship;
                if (s == null) return;
                
                SwitchSector(s.SectorId+1);

                s.Selected = true;
                _selectedUnits.Add(s);
                RefreshCommandText();
            }
            else if (e == EGameEventType.MissileHit)
            {
                var s = sender as Ship;
                if (s == null) return;

                CreateExplosion(s.Bounds, s.SectorId);
                if (s.SectorId == _currentSector.Id) SoundEffect.Play(ESounds.explosion_tiny);
            }
            else if (e == EGameEventType.ImportantMessage)
            {
                var s = sender as GameAlert;
                if (s == null) return;

                _alertSectorId  = s.SectorId;
                _alertExpire = DateTime.Now + _alertDuration;
                AlertMessage.Text = s.Message;
                AlertMessage.Visible = true;
            }
            else if (e == EGameEventType.GameLost)
            {
                GameOver(false);
            }
            else if (e == EGameEventType.GameWon)
            {
                GameOver(true);
            }
            else
            {
                StrategyGame.ProcessGameEvent(sender, e, F_ShipEvent);
            }
        }

        private void B_BaseEvent(Base sender, EBaseEventType e, int senderTeam)
        {
            StrategyGame.ProcessBaseEvent(sender, e, senderTeam);

            if (e == EBaseEventType.BaseDamaged)
            {
                // Bases have small explosions when damaged!
                CreateExplosion(new RectangleF(sender.CenterX - 8, sender.CenterY - 8, 16, 16), sender.SectorId);
                if (sender.SectorId == _currentSector.Id) SoundEffect.Play(ESounds.explosion_tiny);
            }
            else if (e == EBaseEventType.BaseDestroyed)
            {
                if (sender.SectorId == _currentSector.Id) SoundEffect.Play(ESounds.final_explosion_large, true);

                var b = sender.Bounds;
                var p = new PointF(sender.Left, sender.Top);

                // Bases explode with multiple explosions!
                for (var i = 0; i < 4; i++)
                {
                    CreateExplosion(new RectangleF(p.X, p.Y, b.Width / 2, b.Height / 2), sender.SectorId);

                    if ((i + 1) % 2 == 0)
                    {
                        p.X = (int)sender.Left;
                        p.Y += b.Width / 2;
                    }
                    else
                    {
                        p.X += b.Width / 2;
                    }
                }
            }
        }

        private void UpdateWinnersAndLoosers(bool playerWon)
        {
            var winners = new List<Faction>();
            var loosers = new List<Faction>();
            var playerTeam = StrategyGame.GameSettings.TeamAlliance[0];

            for (var i = 0; i < StrategyGame.Faction.Length; i++)
            {
                if (playerWon)
                {
                    if (StrategyGame.GameSettings.TeamAlliance[i] == playerTeam)
                        winners.Add(StrategyGame.Faction[i]);
                    else
                        loosers.Add(StrategyGame.Faction[i]);
                }
                else
                {
                    if (StrategyGame.GameSettings.TeamAlliance[i] != playerTeam)
                        winners.Add(StrategyGame.Faction[i]);
                    else
                        loosers.Add(StrategyGame.Faction[i]);                    
                }
            }
            StrategyGame.Winners = winners.ToArray();
            StrategyGame.Loosers = loosers.ToArray();
        }

        private void GameOver(bool playerWon)
        {
            UpdateWinnersAndLoosers(playerWon);
            if (playerWon)
            {
                WinLose.Text = "You Win!";
            }
            else
            {
                WinLose.Text = "You Lose!";
            }            

            GameOverPanel.Left = Width / 2 - GameOverPanel.Width / 2;
            GameOverPanel.Top = Height / 2 - GameOverPanel.Height / 2;
            SoundEffect.Play((_rnd.Next(2) == 0) ? ESounds.static1 : ESounds.static2);
            GameOverPanel.Visible = true;

            TotalBases1.Text = StrategyGame.GameStats.TotalBasesBuilt[0].ToString();
            TotalBases2.Text = StrategyGame.GameStats.TotalBasesBuilt[1].ToString();
            TotalBasesDestroyed1.Text = StrategyGame.GameStats.TotalBasesDestroyed[0].ToString();
            TotalBasesDestroyed2.Text = StrategyGame.GameStats.TotalBasesDestroyed[1].ToString();
            TotalConstructors1.Text = StrategyGame.GameStats.TotalConstructorsBuilt[0].ToString();
            TotalConstructors2.Text = StrategyGame.GameStats.TotalConstructorsBuilt[1].ToString();
            TotalConstructorsDestroyed1.Text = StrategyGame.GameStats.TotalConstructorsDestroyed[0].ToString();
            TotalConstructorsDestroyed2.Text = StrategyGame.GameStats.TotalConstructorsDestroyed[1].ToString();
            TotalMined1.Text = StrategyGame.GameStats.TotalResourcesMined[0].ToString();
            TotalMined2.Text = StrategyGame.GameStats.TotalResourcesMined[1].ToString();
            TotalMiners1.Text = StrategyGame.GameStats.TotalMinersBuilt[0].ToString();
            TotalMiners2.Text = StrategyGame.GameStats.TotalMinersBuilt[1].ToString();
            TotalMinersDestroyed1.Text = StrategyGame.GameStats.TotalMinersDestroyed[0].ToString();
            TotalMinersDestroyed2.Text = StrategyGame.GameStats.TotalMinersDestroyed[1].ToString();

            tick.Enabled = false;
            timer.Enabled = false;
        }

        internal void F_ShipEvent(Ship sender, EShipEventType e)
        {
            StrategyGame.ProcessShipEvent(sender, e, F_ShipEvent, B_BaseEvent);

            if (e == EShipEventType.ShipDestroyed)
            {
                CreateExplosion(sender.Bounds, sender.SectorId);

                if (sender.SectorId == _currentSector.Id)
                {
                    if (Ship.IsCapitalShip(sender.Type))
                    {
                        SoundEffect.Play(ESounds.final_explosion_medium);
                    }
                    else
                    {
                        SoundEffect.Play(ESounds.final_explosion_small);
                    }
                }

                if (sender.Team == 1 && sender.Type == EShipType.Miner) SoundEffect.Play(ESounds.vo_destroy_miner, true);
            }
            else if (e == EShipEventType.BuildingFinished)
            {
                if (sender.Team == 1 && _researchForm.Visible) _researchForm.RefreshItems();
            }
        }

        private void CreateExplosion(RectangleF b, int sectorId)
        {
            var exp = _explosions.FirstOrDefault(_ => !_.Enabled);
            if (exp != null)
            {
                exp.Resize(b.Width, b.Height);

                exp.SectorId = sectorId;
                exp.TopLeft.X = b.Left;
                exp.TopLeft.Y = b.Top;
                exp.Start();
                _animations.Add(exp);
            }
        }

        private void GetCurrentSectorBounds()
        {
            var maxWidth = StrategyGame.Map.Background.Width;
            var maxHeight = StrategyGame.Map.Background.Height;
            if (maxWidth <= Width || maxHeight <= Height) return;

            var extraWidth = maxWidth - Width;
            var extraHeight = maxHeight - Height;

            var mapPosStep = new Point(extraWidth / RandomMap.MaxWidth, extraHeight / RandomMap.MaxHeight);

            _currentSectorBounds = new Rectangle(_currentSector.MapPosition.X * mapPosStep.X, _currentSector.MapPosition.Y * mapPosStep.Y, Width, Height);
        }

        private Rectangle _currentSectorBounds;
        private void UpdateFrame()
        {
            var g = Graphics.FromImage(_frame);
            var currentSectorId = _currentSector.Id;

            g.DrawImage(StrategyGame.Map.Background, 0, 0, _currentSectorBounds, GraphicsUnit.Pixel);
            StrategyGame.Map.DrawSector(g, currentSectorId);

            if (_shiftDown)
            {
                foreach (var u in _selectedUnits)
                {
                    if (!u.Selected || u.CurrentOrder == null) continue;

                    u.CurrentOrder.Draw(g, u.CenterPoint, currentSectorId);
                    var lastPos = u.CurrentOrder.OrderPosition;

                    // don't show orders after a navigate (another sector!)...
                    if (typeof(NavigateOrder) == u.CurrentOrder.GetType()) continue;

                    foreach (var o in u.Orders)
                    {
                        o.Draw(g, lastPos, currentSectorId);
                        lastPos = o.OrderPosition;
                        if (typeof(NavigateOrder) == o.GetType()) break;                        
                    }
                }
            }

            if (_ctrlDown)
            {
                foreach (var u in _selectedUnits)
                {
                    if (u.SectorId != currentSectorId) continue;

                    var range = u.ScanRange*2;
                    var rangeHalf = u.ScanRange;
                    g.FillEllipse(_sensorBrush, u.CenterX - rangeHalf, u.CenterY - rangeHalf, range, range);

                    range /= 2;
                    rangeHalf /= 2;
                    g.DrawEllipse(_sensorPen, u.CenterX - rangeHalf, u.CenterY - rangeHalf, range, range);
                }

                foreach (var u in _selectedBases)
                {
                    if (u.SectorId != currentSectorId) continue;

                    var range = u.ScanRange * 2;
                    var rangeHalf = u.ScanRange;
                    g.FillEllipse(_sensorBrush, u.CenterX - rangeHalf, u.CenterY - rangeHalf, range, range);

                    range /= 2;
                    rangeHalf /= 2;
                    g.DrawEllipse(_sensorPen, u.CenterX - rangeHalf, u.CenterY - rangeHalf, range, range);
                }
            }

            lock (StrategyGame.AllBases)
            {
                foreach (var u in StrategyGame.AllBases)
                {
                    u.Draw(g, currentSectorId);
                }
            }

            lock (StrategyGame.AllUnits)
            {
                foreach (var u in StrategyGame.AllUnits)
                {
                    u.Draw(g, currentSectorId);
                }
            }

            StrategyGame.DrawMinefields(g, currentSectorId);
            StrategyGame.DrawMissiles(g, currentSectorId);

            foreach (var a in _animations)
            {
                a.Draw(g, a.SectorId == currentSectorId);
            }

            if (_mouseDown)
            {
                g.DrawRectangle(_selectionPen, _selection);
            }

            if (StrategyGame.Credits[0] != _lastCredits)
            {
                _lastCredits = StrategyGame.Credits[0];
                CreditsLabel.Text = "Credits: $" + _lastCredits;
            }

            if (StrategyGame.DockedPilots[0] != _lastPilots)
            {
                _lastPilots = StrategyGame.DockedPilots[0];
                PilotsLabel.Text = "Docked Pilots: " + _lastPilots;
            }

            Invalidate();
        }
        
        private void tick_Tick(object sender, EventArgs e)
        {
            StrategyGame.Tick();

            _selectedUnits.RemoveAll(_ => !_.Active);
            _selectedBases.RemoveAll(_ => !_.Active);
            _animations.RemoveAll(_ => !_.Enabled);

            if (_selectedUnits.Count == 0 && _selectedBases.Count == 0) CommandsLabel.Text = string.Empty;

            UpdateFrame();
        }
        
        private void Sector_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (_frame != null) g.DrawImage(_frame, 0, 0);
        }

        private void Sector_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDouble = false;
            var mousePos = PointToClient(MousePosition);

            if (e.Button == MouseButtons.Right)
            {
                if (_selectedUnits.Count > 0)
                {
                    GiveMoveOrder(mousePos);
                    PlayOrderSound();
                }
                if (_selectedBases.Count > 0)
                {
                    _selectedBases.ForEach(_ => _.BuildPosition = mousePos);
                }
                return;
            }
            else if (e.Button == MouseButtons.Left)
            {
                _selectionStart = mousePos;
                _selectionEnd = mousePos;
                _selection = new Rectangle();
                _mouseDown = true;
            }
        }

        private void Sector_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;

            if (!_mouseDouble && e.Button == MouseButtons.Left)
            {
                SetSelectionRect();
                GetSelectedUnits();
            }
        }

        private void GetSelectedUnits()
        {
            if (!_shiftDown)
            {
                ClearSelected();
            }

            var units = StrategyGame.AllUnits.Where(_ => _.Active && _.Team == 1).ToList();
            foreach (var u in units)
            {
                if (_selectedUnits.Contains(u)) continue;

                if (u.SectorId == _currentSector.Id 
                    && (u.Bounds.Contains(_selectionStart) 
                        || (_selection.Contains((int)u.CenterX, (int)u.CenterY) && u.Type != EShipType.Lifepod)))
                {
                    u.Selected = true;
                    _selectedUnits.Add(u);
                }
            }

            var bases = StrategyGame.AllBases.Where(_ => _.Active && _.Team == 1).ToList();
            foreach (var b in bases)
            {
                if (_selectedBases.Contains(b)) continue;

                if (b.SectorId == _currentSector.Id && (b.Bounds.Contains(_selectionStart) || _selection.Contains((int)b.CenterX, (int)b.CenterY)))
                {
                    b.Selected = true;
                    _selectedBases.Add(b);
                }
            }

            RefreshCommandText();
        }

        private void RefreshCommandText()
        {
            if (_selectedUnits.Count > 0)
            {
                _orderType = EOrderType.Ship;
                CommandsLabel.Text = "Attack:[A]  Stop:[S]  Dock:[D]  Patrol:[R]";
                if (_selectedUnits.Any(_ => _.Type == EShipType.Miner))
                {
                    CommandsLabel.Text += "  Mine:[E]";
                }

                if (_selectedUnits.Any(_ => _.Type == EShipType.Constructor))
                {
                    CommandsLabel.Text += "  Build:[B]";
                }

                if (_selectedUnits.Any(_ => _.Type == EShipType.TroopTransport))
                {
                    CommandsLabel.Text += "  Capture:[C]";
                }
            }
            else if (_selectedBases.Any(_ => _.CanLaunchShips()))
            {
                _orderType = EOrderType.Base;
                CommandsLabel.Text = "Scout:[S]  Fighter:[F]";

                if (StrategyGame.TechTree[0].HasResearchedShipType(EShipType.Bomber))
                    CommandsLabel.Text += "  Bomber:[B]";

                if (StrategyGame.TechTree[0].HasResearchedShipType(EShipType.Gunship))
                    CommandsLabel.Text += "  Gunship:[G]";

                if (StrategyGame.TechTree[0].HasResearchedShipType(EShipType.Interceptor))
                    CommandsLabel.Text += "  Interceptor:[I]";

                if (StrategyGame.TechTree[0].HasResearchedShipType(EShipType.StealthFighter))
                    CommandsLabel.Text += "  Stealth Fighter:[T]";

                if (StrategyGame.TechTree[0].HasResearchedShipType(EShipType.StealthBomber))
                    CommandsLabel.Text += "  Stealth Bomber:[O]";

                if (StrategyGame.TechTree[0].HasResearchedShipType(EShipType.FighterBomber))
                    CommandsLabel.Text += "  Fighter Bomber:[X]";

                if (StrategyGame.TechTree[0].HasResearchedShipType(EShipType.TroopTransport))
                    CommandsLabel.Text += "  Troop Transport:[P]";
            }
            else if (_selectedUnits.Count == 0 && _selectedBases.Count == 0)
            {
                _orderType = EOrderType.None;
                CommandsLabel.Text = string.Empty;
            }
        }

        private void Sector_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDown) return;

            _selectionEnd = PointToClient(MousePosition);
            SetSelectionRect();
        }

        private void SetSelectionRect()
        {
            var x = _selectionStart.X > _selectionEnd.X ? _selectionEnd.X : _selectionStart.X;
            var y = _selectionStart.Y > _selectionEnd.Y ? _selectionEnd.Y : _selectionStart.Y;

            var width = _selectionStart.X > _selectionEnd.X ? _selectionStart.X - _selectionEnd.X : _selectionEnd.X - _selectionStart.X;
            var height = _selectionStart.Y > _selectionEnd.Y ? _selectionStart.Y - _selectionEnd.Y : _selectionEnd.Y - _selectionStart.Y;

            _selection = new Rectangle(x, y, width, height);
        }

        private int[] _mappedSectors = new[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

        private void Sector_KeyDown(object sender, KeyEventArgs e)
        {
            _shiftDown = e.Shift;
            _ctrlDown = e.Control;

            if (e.KeyCode == Keys.F3)
            {
                miniMapToolStripMenuItem_Click(sender, null);
                return;
            }
            else if (e.KeyCode == Keys.F5)
            {
                researchToolStripMenuItem_Click(sender, null);
                return;
            }
            else if (e.KeyCode == Keys.F6)
            {
                pilotListToolStripMenuItem_Click(sender, null);
                return;
            }
            else if (e.KeyCode == Keys.F12)
            {
                enemyAIDebugToolStripMenuItem_Click(sender, null);
                return;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (_researchForm.Visible) researchToolStripMenuItem_Click(sender, null);
                if (_pilotList.Visible) pilotListToolStripMenuItem_Click(sender, null);
                QuickItems.Visible = false;
                QuickItems2.Visible = false;
            }
            else if (e.KeyCode == Keys.Space)
            {
                if (_alertSectorId > -1)
                {
                    var lastSectorId = _currentSector.Id;
                    SwitchSector(_alertSectorId + 1);
                    _alertSectorId = lastSectorId;
                    return;
                }
            }
            else if (e.KeyCode == Keys.Oemtilde)
            {
                SoundEffect.Play(ESounds.text);
                QuickItems.Visible = !QuickItems.Visible;
                QuickItems2.Visible = false;
            }
            else if (QuickItems2.Visible)
            {
                if (ProcessQuickItem(_currentQuickSubList, e))
                {
                    QuickItems.Visible = false;
                    QuickItems2.Visible = false;
                }
            }
            else if (QuickItems.Visible)
            {
                if (ProcessQuickItem(_currentQuickList, e))
                {
                    QuickItems.Visible = false;
                    QuickItems2.Visible = false;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.D0:
                    case Keys.D1:
                    case Keys.D2:
                    case Keys.D3:
                    case Keys.D4:
                    case Keys.D5:
                    case Keys.D6:
                    case Keys.D7:
                    case Keys.D8:
                    case Keys.D9:
                        var i = Convert.ToInt32(e.KeyCode.ToString().Replace("D", string.Empty));

                        if (e.Control)
                        {
                            SoundEffect.Play(ESounds.mousedown);
                            _mappedSectors[i] = _currentSector.Id;
                        }
                        else
                        {
                            var sectorId = _mappedSectors[i];
                            if (sectorId > -1)
                            {
                                SoundEffect.Play(ESounds.text);
                                SwitchSector(sectorId + 1);
                            }
                        }
                        return;
                }

                switch (_orderType)
                {
                    case EOrderType.Ship:
                        GiveShipOrders(e);
                        break;

                    case EOrderType.Base:
                        GiveBaseOrders(e);
                        break;
                }
            }
        }

        private bool ProcessQuickItem(List<QuickChatItem> quickList, KeyEventArgs e)
        {
            var key = e.KeyCode.ToString();
            if (key.Length > 1) key = key.Substring(1, 1);

            var i = quickList.FirstOrDefault(_ => _.Key == key);
            if (i == null) return false;

            if (i.OpenMenuId == string.Empty)
            {
                if (i.Filename == string.Empty) return false;

                ESounds s;
                if (Enum.TryParse(i.Filename, out s))
                {
                    SoundEffect.Play(s);
                }
                return true;
            }
            else
            {
                int m;
                if (int.TryParse(i.OpenMenuId, out m))
                {
                    // Load the other menu;
                    SoundEffect.Play(ESounds.text);
                    LoadQuickSubMenu(m);
                    QuickItems2.Visible = true;
                    QuickItems2.Left = QuickItems.Left + QuickItems.Width;
                }
                return false;
            }
        }

        private void SwitchSector(int i)
        {
            if (i < 1 || i > StrategyGame.Map.Sectors.Count) return;

            ClearSelected();

            var s = StrategyGame.Map.Sectors[i-1];
            _currentSector = s;
            StrategyGame.PlayerCurrentSectorId = _currentSector.Id;
            GetCurrentSectorBounds();

            SectorLabel.Text = _currentSector.Name;
            if (_mapForm.Visible) _mapForm.UpdateMap(_currentSector.Id);            
            Focus();
        }

        private void GiveBaseOrders(KeyEventArgs e)
        {
            if (_selectedBases.Count == 0 || e.KeyCode == Keys.C) return;

            var b = _selectedBases.FirstOrDefault(_ => _.CanLaunchShips());
            if (b == null) return;

            var ship = StrategyGame.Ships.CreateCombatShip(e.KeyCode, 1, _colourTeam1, b.SectorId);
            if (ship == null)
            { 
                if (_shipKeys.Contains(e.KeyCode.ToString())) SoundEffect.Play(ESounds.outofammo);
                return;
            }

            var pos = b.GetNextBuildPosition();
            ship.CenterX = b.CenterX;
            ship.CenterY = b.CenterY;
            ship.ShipEvent += F_ShipEvent;
            ship.OrderShip(new MoveOrder(StrategyGame, b.SectorId, pos, Point.Empty));

            StrategyGame.LaunchShip(ship);
            SoundEffect.Play(ESounds.text);
        }

        private void GiveShipOrders(KeyEventArgs e)
        {
            if (_selectedUnits.Count == 0) return;
            var centerPos = PointToClient(MousePosition);

            var s = StrategyGame.AllUnits.FirstOrDefault(_ => _.Active && _.Team == 1 && _.SectorId == _currentSector.Id && _.Type != EShipType.Lifepod && _.Bounds.Contains(centerPos));
            var lifepods = _selectedUnits.Where(_ => _.Active && _.Type == EShipType.Lifepod && _.SectorId == _currentSector.Id).ToList();
            if (s!= null && lifepods.Count > 0)
            {
                foreach (var pod in lifepods)
                {
                    pod.OrderShip(new InterceptOrder(StrategyGame, s, pod.SectorId));
                    _selectedUnits.Remove(pod);
                    pod.Selected = false;
                }
            }

            var playKey = false;

            switch (e.KeyCode)
            {
                case Keys.S:
                    _selectedUnits.ForEach(_ => _.OrderShip(new StopOrder(StrategyGame), _shiftDown));
                    playKey = true;
                    break;
                case Keys.R:
                    GivePatrolOrder(centerPos);
                    playKey = true;
                    break;
                case Keys.A:
                    GiveMoveOrder(centerPos);
                    playKey = true;
                    break;
                case Keys.D:
                    _selectedUnits.ForEach(_ => _.OrderShip(new DockOrder(StrategyGame, _, _shiftDown), _shiftDown));
                    playKey = true;
                    break;
                case Keys.C:
                    GiveCaptureOrder();
                    playKey = true;
                    break;
                case Keys.E:
                    GiveMineOrder(centerPos);
                    break;
                case Keys.B:
                    GiveBuildOrder(centerPos);
                    playKey = true;
                    break;
            }

            if (playKey) PlayOrderSound();
        }

        private const int AffirmativeSoundDelay = 12;
        private int _afirmativeSoundNext = AffirmativeSoundDelay;

        private ESounds[] _affirmativeSounds = { ESounds.vo_player_affirmative, ESounds.vo_player_roger, ESounds.vo_player_onmyway, ESounds.vo_player_acknowledged };
        private void PlayOrderSound()
        {
            var sound = ESounds.text;

            if (_afirmativeSoundNext <= 0)
            {
                sound = _affirmativeSounds[_rnd.Next(_affirmativeSounds.Length)];
                _afirmativeSoundNext = AffirmativeSoundDelay;
            }

            SoundEffect.Play(sound);
        }

        private void GiveMoveOrder(Point orderPosition)
        {
            var b = StrategyGame.AllBases.FirstOrDefault(_ => _.Active && _.Team == 1 && _.SectorId == _currentSector.Id && _.Bounds.Contains(orderPosition));
            if (b != null)
            {
                foreach (var u in _selectedUnits)
                {
                    if (u.SectorId != _currentSector.Id) continue;
                    u.OrderShip(new DockOrder(StrategyGame, u, b, _shiftDown), _shiftDown);
                }
                return;
            }

            var w = StrategyGame.Map.Wormholes.FirstOrDefault(_ => (_.End1.SectorId == _currentSector.Id && _.End1.Bounds.Contains(orderPosition))
                                                                    || (_.End2.SectorId == _currentSector.Id && _.End2.Bounds.Contains(orderPosition)));
            if (w != null)
            {
                foreach (var u in _selectedUnits)
                {
                    if (u.SectorId != _currentSector.Id) continue;
                    u.OrderShip(new WarpOrder(StrategyGame, u, w), _shiftDown);
                }
                return;
            }

            if (_selectedUnits.Any(_ => _.Type == EShipType.TroopTransport))
            {
                var e = StrategyGame.AllBases.FirstOrDefault(_ => _.Active && _.Team == 2 && _.SectorId == _currentSector.Id && _.Bounds.Contains(orderPosition));
                if (e != null)
                {
                    foreach (var u in _selectedUnits)
                    {
                        if (u.SectorId != _currentSector.Id || u.Type != EShipType.TroopTransport) continue;
                        u.OrderShip(new CaptureOrder(StrategyGame, u, b), _shiftDown);
                    }

                    _selectedUnits.RemoveAll(_ => _.Type == EShipType.TroopTransport);
                }
            }            

            if (_selectedUnits.Count > 1)
            {
                StrategyGame.SpreadOrderEvenly<MoveOrder>(StrategyGame, _selectedUnits, _currentSector.Id, orderPosition, _shiftDown);
            }
            else if (_selectedUnits.Count == 1 && _selectedUnits[0].SectorId == _currentSector.Id)
            {
                _selectedUnits[0].OrderShip(new MoveOrder(StrategyGame, _currentSector.Id, orderPosition, Point.Empty), _shiftDown);
            }
        }

        private void GivePatrolOrder(Point orderPosition)
        {
            if (_selectedUnits.Count > 1)
            {
                StrategyGame.SpreadOrderEvenly<PatrolOrder>(StrategyGame, _selectedUnits, _currentSector.Id, orderPosition, _shiftDown);
            }
            else if (_selectedUnits.Count == 1 && _selectedUnits[0].SectorId == _currentSector.Id)
            {
                _selectedUnits[0].OrderShip(new PatrolOrder(StrategyGame, _currentSector.Id, orderPosition, Point.Empty), _shiftDown);
            }
        }

        private void GiveMineOrder(Point orderPosition)
        {
            var minerUnits = _selectedUnits.Where(_ => _.Type == EShipType.Miner).ToList();

            if (minerUnits.Count > 1)
            {
                StrategyGame.SpreadOrderEvenly<MineOrder>(StrategyGame, minerUnits, _currentSector.Id, orderPosition, _shiftDown);
            }
            else if (minerUnits.Count == 1 && minerUnits[0].SectorId == _currentSector.Id)
            {
                minerUnits[0].OrderShip(new MineOrder(StrategyGame, _currentSector.Id, orderPosition, Point.Empty), _shiftDown);
            }
        }

        private void GiveCaptureOrder()
        {
            var transportUnit = _selectedUnits.Where(_ => _.Type == EShipType.TroopTransport 
                                                    && (_.CurrentOrder == null || _.CurrentOrder as CaptureOrder == null)
                                                    ).FirstOrDefault();
            if (transportUnit != null)
            {
                transportUnit.OrderShip(new CaptureOrder(StrategyGame, transportUnit), _shiftDown);
            }
        }

        private void GiveBuildOrder(Point orderPosition)
        {
            var builderUnit = _selectedUnits.Where(_ => _.Type == EShipType.Constructor
                                                    && (_.CurrentOrder == null || _.CurrentOrder as BuildOrder == null)
                                                    ).FirstOrDefault();
            if (builderUnit != null)
            {
                builderUnit.OrderShip(new BuildOrder(StrategyGame, _currentSector.Id, orderPosition, Point.Empty), _shiftDown);
            }
        }

        private void Sector_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Shift) _shiftDown = false;
            if (!e.Control) _ctrlDown = false;
        }

        private void ClearSelected()
        {
            _selectedUnits.ForEach(_ => _.Selected = false);
            _selectedUnits.Clear();
            _selectedBases.ForEach(_ => _.Selected = false);
            _selectedBases.Clear();
        }

        private void Sector_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Select all units of this type
            _mouseDouble = true;
            if (!_shiftDown)
            {
                ClearSelected();
            }

            var pos = e.Location;
            var t = EShipType.None;
            GameEntity wormHoldEnd = null;

            var units = StrategyGame.AllUnits.Where(_ => _.Active && _.Team == 1).ToList();
            foreach (var u in units)
            {
                if (u.SectorId == _currentSector.Id && u.Bounds.Contains(pos))
                {
                    t = u.Type;
                    break;
                }
            }

            foreach (var w in StrategyGame.Map.Wormholes)
            {
                if (w.Sector1.Id == _currentSector.Id)
                {
                    if (w.End1.Bounds.Contains(pos))
                    {
                        wormHoldEnd = w.End2;
                        break;
                    }
                    continue;
                }
                if (w.Sector2.Id == _currentSector.Id)
                {
                    if (w.End2.Bounds.Contains(pos))
                    {
                        wormHoldEnd = w.End1;
                        break;
                    }
                    continue;
                }
            }

            if (t != EShipType.None)
            {
                _selectedUnits.AddRange(units.Where(_ => _.Active && _.SectorId == _currentSector.Id && _.Type == t));
                _selectedUnits.ForEach(_ => _.Selected = true);
            }

            if (wormHoldEnd != null)
            {
                var newSector = StrategyGame.Map.Sectors.First(_ => _.Id == wormHoldEnd.SectorId);
                StrategyGame_GameEvent(newSector, EGameEventType.SectorLeftClicked);
            }
        }
        
        private void researchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.windowslides);

            if (_researchForm.Visible)
            {
                _researchForm.Hide();
            }
            else
            {
                if (_researchForm.IsDisposed)
                {
                    _researchForm = new Research(StrategyGame);
                }

                _researchForm.RefreshItems();
                _researchForm.Show(this);

                _researchForm.Top = Top + Height / 2 - _researchForm.Height / 2;
                _researchForm.Left = Left + Width / 2 - _researchForm.Width / 2;
                Focus();
            }
        }

        private void miniMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.windowslides);

            if (_mapForm.Visible)
            {
                _mapForm.Hide();
            }
            else
            {
                if (_mapForm.IsDisposed)
                {
                    _mapForm = new Map(StrategyGame);
                }
                _mapForm.UpdateMap(_currentSector.Id);
                _mapForm.Show(this);

                _mapForm.Top = Top;
                _mapForm.Left = Left + Width - 5;
                Focus();
            }
        }

        private void enemyAIDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_debugForm == null) return;

            SoundEffect.Play(ESounds.windowslides);
            if (_debugForm.Visible)
            {
                _debugForm.Hide();
            }
            else
            {
                _debugForm.Show(this);

                _debugForm.Top = Top + Height - _debugForm.Height - 10;
                _debugForm.Left = Left + Width - 5;
                Focus();
            }
        }

        private void Sector_Move(object sender, EventArgs e)
        {
            if (_researchForm.Visible)
            {
                _researchForm.Top = Top + Height / 2 - _researchForm.Height / 2;
                _researchForm.Left = Left + Width / 2 - _researchForm.Width / 2;
            }
            if (_mapForm.Visible)
            {
                _mapForm.Top = Top;
                _mapForm.Left = Left + Width - 5;
            }
            if (_debugForm != null && _debugForm.Visible)
            {
                _debugForm.Top = Top + Height - _debugForm.Height - 10;
                _debugForm.Left = Left + Width - 5;
            }
            if (_pilotList.Visible)
            {
                _pilotList.Top = Top + Height / 2 - _pilotList.Height / 2;
                _pilotList.Left = Left + Width / 2 - _pilotList.Width / 2;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // The Game's slow update!
            StrategyGame.SlowTick();
            _afirmativeSoundNext--;

            if (_mapForm.Visible) _mapForm.UpdateMap(_currentSector.Id);
            
            if (_debugForm != null && _debugForm.Visible) _debugForm.UpdateDebugInfo();
                        
            //if (_pilotList.Visible) _pilotList.RefreshPilotList();

            if (AlertMessage.Visible && DateTime.Now >= _alertExpire) AlertMessage.Visible = false;

            if (_researchForm.Visible) _researchForm.UpdateItems();

            RefreshCommandText();
        }

        private void pilotListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.windowslides);
            if (_pilotList.Visible)
            {
                _pilotList.Hide();
            }
            else
            {
                if (_pilotList.IsDisposed)
                {
                    _pilotList = new PilotList(StrategyGame);
                }

                _pilotList.RefreshPilotList();
                _pilotList.Show(this);

                _pilotList.Top = Top + Height / 2 - _pilotList.Height / 2;
                _pilotList.Left = Left + Width / 2 - _pilotList.Width / 2;
                Focus();
            }
        }

        private void Sector_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tick.Enabled)
            {
                tick.Enabled =  timer.Enabled = false;
                if (MessageBox.Show("A game is running, are you sure you want to quit?", "Quit Game?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    tick.Enabled = timer.Enabled = true;
                }
            }

            if (!e.Cancel)
            {
                TopMost = false;
                if (GameOverEvent != null) GameOverEvent(this);
            }
        }

        private void Done_Click(object sender, EventArgs e)
        {
            Close();
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
}
