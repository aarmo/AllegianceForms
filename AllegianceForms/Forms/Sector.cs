using AllegianceForms.Engine.AI;
using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Map;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Engine.Tech;
using AllegianceForms.Orders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class Sector : Form
    {        
        private readonly Bitmap _frame;
        private readonly List<Base> _selectedBases = new List<Base>();
        private readonly List<Ship> _selectedUnits = new List<Ship>();
        private readonly List<Animation> _explosions = new List<Animation>();
        private readonly List<Animation> _animations = new List<Animation>();
        private readonly List<string> _shipKeys;

        private Research _researchForm = new Research();
        private PilotList _pilotList = new PilotList();
        private Map _mapForm = new Map();
        private DebugAI _debugForm;
        private MapSector _currentSector;

        private readonly Pen _selectionPen;
        private readonly Brush _sensorBrush;
        private readonly TextureBrush _bgBrush;
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

        public Sector(GameSettings settings)
        {
            InitializeComponent();

            StrategyGame.SetupGame(settings);
            StrategyGame.LoadData();
            StrategyGame.Map = GameMaps.LoadMap(settings.MapName);

            var startSectors = StrategyGame.Map.Sectors.Where(_ => _.StartingSector).ToList();
            if (StrategyGame.Map.Name == "Brawl") startSectors.Add(startSectors[0]);

            if (startSectors.Count < StrategyGame.NumTeams && (StrategyGame.Map.Name != "Brawl" || StrategyGame.NumTeams > 2))
            {
                MessageBox.Show($"Sorry, the map '{StrategyGame.Map.Name}' doesn't support {StrategyGame.NumTeams} teams...", "Setup Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

#if DEBUG
            // Testing setup: crazy money, fast tech, map visible
            StrategyGame.AddResources(1, 100000, false);
            settings.WormholesVisible = true;
            settings.ResearchTimeMultiplier = 0.25f;
            settings.ResearchCostMultiplier = 0.25f;
#endif
            StrategyGame.InitialiseGame();

            Width = StrategyGame.ScreenWidth;
            Height = StrategyGame.ScreenHeight;
            _frame = new Bitmap(Width, Height);
            _bgBrush = new TextureBrush(Image.FromFile(".\\Art\\Backgrounds\\stars.png"));
            _selectionPen = new Pen(Color.LightGray, 1F) {DashStyle = DashStyle.Dot};
            _colourTeam1 = Color.FromArgb(settings.TeamColours[0]);
            _sensorPen = new Pen(StrategyGame.NewAlphaColour(20, _colourTeam1), 1F) { DashStyle = DashStyle.Dash };
            _sensorBrush = new SolidBrush(StrategyGame.NewAlphaColour(5, _colourTeam1));
            _shipKeys = StrategyGame.Ships.Ships.Select(_ => _.Key).ToList();
            _currentSector = StrategyGame.Map.Sectors.First(_ => _.StartingSector);
            Text = "Allegiance Forms - Conquest: " + _currentSector.Name;


            // Friendy & enemy team setup:
            for (var t = 0; t < StrategyGame.NumTeams; t++)
            {
                var team = t + 1;
                var startingSector = startSectors[t];
                if (StrategyGame.NumTeams == 2 && startSectors.Count == 4 && t == 1) startingSector = startSectors[2];
                var teamColour = Color.FromArgb(StrategyGame.GameSettings.TeamColours[t]);
                var aiPlayer = t != 0;

                var b1 = StrategyGame.Bases.CreateBase(EBaseType.Starbase, team, teamColour, startingSector.Id);
                b1.CenterX = (aiPlayer ? Width - 300 : 100);
                b1.CenterY = (aiPlayer ? Height - 300 : 100);
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
                    var ai = new CommanderAI(team, teamColour, F_ShipEvent, true);
                    StrategyGame.AICommanders[t] = ai;
                    ai.SetDifficulty(settings.AiDifficulty);
                    StrategyGame.DockedPilots[t] = (int)(settings.NumPilots * ai.CheatAdditionalPilots);

                    if (t == 1) _debugForm = new DebugAI(ai);

                    //ai.Enabled = false;
                    //enemyAIDebugToolStripMenuItem_Click(null, null);
                }
            }

            // Explosions!
            var explosionFrames = new string[10];
            for (var i = 0; i < 10; i++)
            {
                explosionFrames[i] = $".\\Art\\Animations\\Explode\\bubble_explo{i + 1}.png";
            }

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

        private void StrategyGame_GameEvent(object sender, EGameEventType e)
        {
            if (e == EGameEventType.SectorLeftClicked)
            {
                var s = sender as MapSector;
                if (s == null) return;

                SoundEffect.Play(ESounds.text);
                SwitchSector(s.Id+1);
                Focus();
            }
            else if (e == EGameEventType.SectorRightClicked)
            {
                var s = sender as MapSector;
                if (s == null) return;

                // Order selected units to navigate to the clicked sector
                _selectedUnits.ForEach(_ => _.OrderShip(new NavigateOrder(_, s.Id), _shiftDown));
                _selectedUnits.ForEach(_ => _.Selected = false);
                _selectedUnits.Clear();
                Focus();
            }
            else if (e == EGameEventType.ShipClicked)
            {
                var s = sender as Ship;
                if (s == null) return;

                SwitchSector(s.SectorId+1);

                _selectedUnits.ForEach(_ => _.Selected = false);
                _selectedUnits.Clear();
                _selectedBases.ForEach(_ => _.Selected = false);
                _selectedBases.Clear();

                s.Selected = true;
                _selectedUnits.Add(s);
                RefreshCommandText();

                Focus();
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

        private void GameOver(bool won)
        {
            if (won)
            {
                WinLose.Text = "You Win!";
            }
            else
            {
                WinLose.Text = "You Lose!";
            }
            
            GameOverPanel.Left = Width / 2 - GameOverPanel.Width / 2;
            GameOverPanel.Top = Height / 2 - GameOverPanel.Height / 2;
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

        private void UpdateFrame()
        {
            var g = Graphics.FromImage(_frame);
            var currentSectorId = _currentSector.Id;

            //g.Clear(BackColor);
            g.FillRectangle(_bgBrush, 0, 0, Width, Height);
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
            StrategyGame.Tick(_currentSector.Id);

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
                _selectedUnits.ForEach(_ => _.Selected = false);
                _selectedUnits.Clear();

                _selectedBases.ForEach(_ => _.Selected = false);
                _selectedBases.Clear();
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
            if (_selectedBases.Any(_ => _.CanLaunchShips()))
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
            else if (_selectedUnits.Count > 0)
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

        private void Sector_KeyDown(object sender, KeyEventArgs e)
        {
            _shiftDown = e.Shift;
            _ctrlDown = e.Control;
            
            if (e.KeyCode == Keys.F3)
            {
                miniMapToolStripMenuItem_Click(sender, null);
                return;
            }
            else if(e.KeyCode == Keys.F5)
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
            }
            else if (e.KeyCode == Keys.Space)
            {
                if (_alertSectorId > -1)
                {
                    var lastSectorId = _currentSector.Id;
                    SwitchSector(_alertSectorId+1);
                    _alertSectorId = lastSectorId;
                    Focus();
                    return;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.D1:
                    case Keys.D2:
                    case Keys.D3:
                    case Keys.D4:
                    case Keys.D5:
                    case Keys.D6:
                    case Keys.D7:
                    case Keys.D8:
                    case Keys.D9:
                        SoundEffect.Play(ESounds.text);
                        SwitchSector(Convert.ToInt32(e.KeyCode.ToString().Replace("D", string.Empty)));
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

        private void SwitchSector(int i)
        {
            if (i <= 0 || i > StrategyGame.Map.Sectors.Count) return;

            _selectedBases.Clear();
            _selectedUnits.Clear();

            var s = StrategyGame.Map.Sectors[i-1];
            _currentSector = s;

            Text = "Allegiance Forms - Conquest: " + _currentSector.Name;
            if (_mapForm.Visible) _mapForm.UpdateMap(_currentSector.Id);
        }

        private void GiveBaseOrders(KeyEventArgs e)
        {
            if (_selectedBases.Count == 0) return;

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
            ship.OrderShip(new MoveOrder(b.SectorId, pos, Point.Empty));

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
                    pod.OrderShip(new InterceptOrder(s, pod.SectorId));
                    _selectedUnits.Remove(pod);
                    pod.Selected = false;
                }
            }

            var playKey = false;

            switch (e.KeyCode)
            {
                case Keys.S:
                    _selectedUnits.ForEach(_ => _.OrderShip(new StopOrder(), _shiftDown));
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
                    _selectedUnits.ForEach(_ => _.OrderShip(new DockOrder(_, _shiftDown), _shiftDown));
                    playKey = true;
                    break;
                case Keys.C:
                    GiveCaptureOrder();
                    playKey = true;
                    break;
                case Keys.E:
                    GiveMineOrder(centerPos);
                    playKey = true;
                    break;
                case Keys.B:
                    GiveBuildOrder(centerPos);
                    playKey = true;
                    break;
            }

            if (playKey) SoundEffect.Play(ESounds.text);
        }

        private void GiveMoveOrder(Point orderPosition)
        {
            var b = StrategyGame.AllBases.FirstOrDefault(_ => _.Active && _.Team == 1 && _.SectorId == _currentSector.Id && _.Bounds.Contains(orderPosition));
            if (b != null)
            {
                foreach (var u in _selectedUnits)
                {
                    if (u.SectorId != _currentSector.Id) continue;
                    u.OrderShip(new DockOrder(u, b, _shiftDown), _shiftDown);
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
                    u.OrderShip(new WarpOrder(u, w), _shiftDown);
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
                        u.OrderShip(new CaptureOrder(u, b), _shiftDown);
                    }

                    _selectedUnits.RemoveAll(_ => _.Type == EShipType.TroopTransport);
                }
            }            

            if (_selectedUnits.Count > 1)
            {
                StrategyGame.SpreadOrderEvenly<MoveOrder>(_selectedUnits, _currentSector.Id, orderPosition, _shiftDown);
            }
            else if (_selectedUnits.Count == 1 && _selectedUnits[0].SectorId == _currentSector.Id)
            {
                _selectedUnits[0].OrderShip(new MoveOrder(_currentSector.Id, orderPosition, Point.Empty), _shiftDown);
            }
        }

        private void GivePatrolOrder(Point orderPosition)
        {
            if (_selectedUnits.Count > 1)
            {
                StrategyGame.SpreadOrderEvenly<PatrolOrder>(_selectedUnits, _currentSector.Id, orderPosition, _shiftDown);
            }
            else if (_selectedUnits.Count == 1 && _selectedUnits[0].SectorId == _currentSector.Id)
            {
                _selectedUnits[0].OrderShip(new PatrolOrder(_currentSector.Id, orderPosition, Point.Empty), _shiftDown);
            }
        }

        private void GiveMineOrder(Point orderPosition)
        {
            var minerUnits = _selectedUnits.Where(_ => _.Type == EShipType.Miner).ToList();

            if (minerUnits.Count > 1)
            {
                StrategyGame.SpreadOrderEvenly<MineOrder>(minerUnits, _currentSector.Id, orderPosition, _shiftDown);
            }
            else if (minerUnits.Count == 1 && minerUnits[0].SectorId == _currentSector.Id)
            {
                minerUnits[0].OrderShip(new MineOrder(_currentSector.Id, orderPosition, Point.Empty), _shiftDown);
            }
        }

        private void GiveCaptureOrder()
        {
            var transportUnit = _selectedUnits.Where(_ => _.Type == EShipType.TroopTransport).FirstOrDefault();

            if (transportUnit != null)
            {
                transportUnit.OrderShip(new CaptureOrder(transportUnit), _shiftDown);
            }
        }

        private void GiveBuildOrder(Point orderPosition)
        {
            var builderUnit = _selectedUnits.Where(_ => _.Type == EShipType.Constructor).FirstOrDefault();

            if (builderUnit != null)
            {
                builderUnit.OrderShip(new BuildOrder(_currentSector.Id, orderPosition, Point.Empty), _shiftDown);
            }
        }

        private void Sector_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Shift) _shiftDown = false;
            if (!e.Control) _ctrlDown = false;
        }

        private void Sector_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Select all units of this type
            _mouseDouble = true;
            if (!_shiftDown)
            {
                _selectedUnits.ForEach(_ => _.Selected = false);
                _selectedUnits.Clear();
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
                _mapForm.UpdateMap(_currentSector.Id);
                _mapForm.Show(this);

                _mapForm.Top = Top;
                _mapForm.Left = Left + Width - 5;
                Focus();
            }
        }

        private void enemyAIDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            if (_debugForm.Visible)
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
            StrategyGame.SlowTick(_currentSector.Id);

            if (_mapForm.Visible) _mapForm.UpdateMap(_currentSector.Id);
            
            if (_debugForm.Visible) _debugForm.UpdateDebugInfo();
                        
            if (_pilotList.Visible) _pilotList.RefreshPilotList();

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
        }
    }
}
