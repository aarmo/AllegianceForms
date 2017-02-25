using AllegianceForms.AI;
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

namespace AllegianceForms
{
    public partial class Sector : Form
    {        
        private readonly Bitmap _frame;
        private readonly Image _bg;
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
        private Color _colourTeam1;
        private Color _colourTeam2;
        private Random _rnd = new Random();
        private int _lastCredits = 0;
        private int _lastPilots = 0;
        private int _alertSectorId = -1;
        private DateTime _alertExpire = DateTime.MinValue;
        private TimeSpan _alertDuration = new TimeSpan(0, 0, 0, 3);
        
        private CommanderAI _ai;
        
        public Sector(GameSettings settings)
        {
            InitializeComponent();
            StrategyGame.ResetGame(settings);

            Width = StrategyGame.ScreenWidth;
            Height = StrategyGame.ScreenHeight;
            _frame = new Bitmap(Width, Height);
            _bg = Image.FromFile(".\\Art\\Backgrounds\\stars.png");
            _bgBrush = new TextureBrush(_bg);
            var centerPos = new Point(Width / 2, Height / 2);

            _selectionPen = new Pen(Color.LightGray, 1F) {DashStyle = DashStyle.Dot};
            _colourTeam1 = Color.FromArgb(settings.Team1ColourARBG);
            _colourTeam2 = Color.FromArgb(settings.Team2ColourARBG);
            _sensorPen = new Pen(StrategyGame.NewAlphaColour(20, _colourTeam1), 1F) { DashStyle = DashStyle.Dash };
            _sensorBrush = new SolidBrush(StrategyGame.NewAlphaColour(5, _colourTeam1));
            
            StrategyGame.Map = GameMaps.LoadMap(settings.MapName);
            _currentSector = StrategyGame.Map.Sectors.First(_ => _.StartingSector);
            Text = "Allegiance Forms - Conquest: " + _currentSector.Name;

            StrategyGame.LoadData();
            _shipKeys = StrategyGame.Ships.Ships.Select(_ => _.Key).ToList();

            // Friendlies
            StrategyGame.DockedPilots[0] = settings.NumPilots;
            var b1 = StrategyGame.Bases.CreateBase(EBaseType.Starbase, 1, _colourTeam1, _currentSector.Id);
            b1.CenterX = 100;
            b1.CenterY = 100;
            b1.BaseEvent += B_BaseEvent;
            StrategyGame.AddBase(b1);
            StrategyGame.GameStats.TotalBasesBuilt[0] = 1;

            /*
            // Test Ship!
            var testShip = StrategyGame.Ships.CreateCombatShip(Keys.F, 1, _colourTeam1, _currentSector.Id);
            testShip.Weapons.Add(new ShipMissileWeapon(8, 5, 250, 2000, 400, 10, testShip, Point.Empty, new SolidBrush(_colourTeam1)));
            testShip.MaxHealth = testShip.Health = 1000;
            testShip.CenterX = b1.CenterX + 100;
            testShip.CenterY = b1.CenterY + 80;
            testShip.ShipEvent += F_ShipEvent;
            StrategyGame.AddUnit(testShip);
            */

            for (var i = 0; i < settings.MinersInitial; i++)
            {
                var startingMiner = StrategyGame.Ships.CreateMinerShip(1, _colourTeam1, _currentSector.Id);
                startingMiner.CenterX = b1.CenterX + 100 + (i * 30);
                startingMiner.CenterY = b1.CenterY + 40;
                startingMiner.ShipEvent += F_ShipEvent;
                StrategyGame.AddUnit(startingMiner);
            }

            // Enemies
            var enemySector = StrategyGame.Map.Sectors.Last(_ => _.StartingSector);
            var b2 = StrategyGame.Bases.CreateBase(EBaseType.Starbase, 2, _colourTeam2, enemySector.Id);
            b2.CenterX = Width - 300;
            b2.CenterY = Height - 300;
            b2.BaseEvent += B_BaseEvent;
            StrategyGame.AddBase(b2);
            StrategyGame.GameStats.TotalBasesBuilt[1] = 1;
            for (var i = 0; i < settings.MinersInitial; i++)
            {
                var startingMiner = StrategyGame.Ships.CreateMinerShip(2, _colourTeam2, enemySector.Id);
                startingMiner.CenterX = b2.CenterX - 100 - (i * 30);
                startingMiner.CenterY = b2.CenterY - 40;
                startingMiner.ShipEvent += F_ShipEvent;
                StrategyGame.AddUnit(startingMiner);
            }
            _ai = new CommanderAI(2, _colourTeam2, this, true);
            StrategyGame.AICommanders[1] = _ai;
            _ai.SetDifficulty(settings.AiDifficulty);
            StrategyGame.DockedPilots[1] = (int)(settings.NumPilots * _ai.CheatAdditionalPilots);

            _debugForm = new DebugAI(_ai);
            //_ai.Enabled = false;

#if DEBUG
            // Testing Setup: Crazy money, fast tech, map visible
            StrategyGame.AddResources(1, 100000);
            settings.WormholesVisible = true;
            settings.ResearchTimeMultiplier = 0.25f;
            settings.ResearchCostMultiplier = 0.25f;

            enemyAIDebugToolStripMenuItem_Click(null, null);
#endif

            // Regular Setup:
            for (var t = 1; t <= StrategyGame.NumTeams; t++)
            {
                StrategyGame.AddResources(t, (int)(StrategyGame.ResourcesInitial * settings.ResourcesStartingMultiplier));
                StrategyGame.Map.SetVisibilityToTeam(t, settings.WormholesVisible);
                var faction = StrategyGame.Faction[t - 1];

                foreach (var tech in StrategyGame.TechTree[t-1].TechItems)
                {
                    tech.Cost = (int)(tech.Cost * settings.ResearchCostMultiplier * (1 + (1 - faction.Bonuses.ResearchCost)));
                    tech.DurationSec = (int)(tech.DurationSec * settings.ResearchTimeMultiplier * (1 + (1 - faction.Bonuses.ResearchTime)));
                }
            }

            // Setup explosions
            var explosionFrames = new string[10];
            for (var i = 0; i < 10; i++)
            {
                explosionFrames[i] = $".\\Art\\Animations\\Explode\\bubble_explo{i + 1}.png";
            }

            for (var i = 0; i < 30; i++)
            {
                var a = new Animation(explosionFrames, 0, 0, 16, 16, new TimeSpan(0, 0, 0, 0, 100), false);
                _explosions.Add(a);
            }

            // Final setup
            _currentSector.VisibleToTeam[0] = true;
            StrategyGame.UpdateVisibility(true);
            StrategyGame.GameEvent += StrategyGame_GameEvent;
            miniMapToolStripMenuItem_Click(null, null);
        }

        private void StrategyGame_GameEvent(object sender, EGameEventType e)
        {
            if (e == EGameEventType.DroneBuilt)
            {
                var tech = sender as TechItem;
                if (tech == null) return;

                var b1 = StrategyGame.AllBases.Where(_ => _.Team == tech.Team && _.Type == EBaseType.Starbase).LastOrDefault();
                if (b1 == null) return;
                Ship drone;

                var colour = tech.Team == 1 ? _colourTeam1 : _colourTeam2;
                if (tech.Name == "Miner")
                {
                    drone = StrategyGame.Ships.CreateMinerShip(tech.Team, colour, b1.SectorId);
                    if (drone == null) return;

                    if (tech.Team == 1) SoundEffect.Play(ESounds.vo_miner_report4duty);
                }
                else if (tech.Type == ETechType.ShipyardConstruction)
                {
                    b1 = StrategyGame.AllBases.Where(_ => _.Team == tech.Team && _.Type == EBaseType.Shipyard).LastOrDefault();
                    if (b1 == null) return;

                    drone = StrategyGame.Ships.CreateShip(tech.Name, tech.Team, colour, b1.SectorId);
                    if (drone == null) return;
                }
                else
                {
                    var bType = TechItem.GetBaseType(tech.Name);

                    drone = StrategyGame.Ships.CreateBuilderShip(bType, tech.Team, colour, b1.SectorId);
                    if (drone == null) return;
                    var builder = drone as BuilderShip;
                    if (builder == null) return;

                    if (tech.Team == 1 && BaseSpecs.IsTower(builder.BaseType))
                    {
                        SoundEffect.Play(ESounds.vo_request_tower);
                    }
                    else if (tech.Team == 1)
                    {
                        switch (builder.TargetRockType)
                        {
                            case EAsteroidType.Resource:
                                SoundEffect.Play(ESounds.vo_request_builderhelium);
                                break;
                            case EAsteroidType.Rock:
                                SoundEffect.Play(ESounds.vo_request_buildergeneric);
                                break;
                            case EAsteroidType.TechCarbon:
                                SoundEffect.Play(ESounds.vo_request_buildercarbon);
                                break;
                            case EAsteroidType.TechSilicon:
                                SoundEffect.Play(ESounds.vo_request_buildersilicon);
                                break;
                            case EAsteroidType.TechUranium:
                                SoundEffect.Play(ESounds.vo_request_builderuranium);
                                break;
                        }
                    }
                }
                
                drone.CenterX = b1.CenterX;
                drone.CenterY = b1.CenterY;
                drone.ShipEvent += F_ShipEvent;
                drone.OrderShip(new MoveOrder(b1.SectorId, b1.GetNextBuildPosition(), Point.Empty));
                
                StrategyGame.AddUnit(drone);
            }
            else if (e == EGameEventType.ResearchComplete)
            {
                var tech = sender as TechItem;
                if (tech == null) return;
                if (TechItem.IsGlobalUpgrade(tech.Name)) tech.ApplyGlobalUpgrade(StrategyGame.TechTree[tech.Team - 1]);

                if (tech.Team == 1) SoundEffect.Play(ESounds.vo_sal_researchcomplete);
            }
            else if (e == EGameEventType.SectorLeftClicked)
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
        }

        private void B_BaseEvent(Base sender, EBaseEventType e)
        {
            if (e == EBaseEventType.BaseDamaged)
            {
                // Bases have small explosions when damaged!
                CreateExplosion(new RectangleF(sender.CenterX - 8, sender.CenterY - 8, 16, 16), sender.SectorId);
            }
            else if (e == EBaseEventType.BaseDestroyed)
            {                
                var b = sender.Bounds;
                var p = new PointF(sender.Left, sender.Top);

                // Bases explode with multiple explosions!
                for (var i = 0; i < 4; i++)
                {
                    CreateExplosion(new RectangleF(p.X, p.Y, b.Width / 2, b.Height / 2), sender.SectorId);
                    
                    if ((i+1) % 2 == 0)
                    {
                        p.X = (int)sender.Left;
                        p.Y += b.Width / 2;
                    }
                    else
                    {
                        p.X += b.Width / 2;
                    }
                }

                if (sender.Team == 1 && !StrategyGame.AllBases.Any(_ => _.Active && _.Team == 1 && _.SectorId == sender.SectorId && _.CanLaunchShips()))
                {
                    SoundEffect.Play(ESounds.vo_sal_sectorlost, true);
                }
                SoundEffect.Play(ESounds.big_explosion, true);

                StrategyGame.GameStats.TotalBasesDestroyed[sender.Team - 1]++;

                switch (sender.Type)
                {
                    case (EBaseType.Expansion):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_destroy_enemyexpansion : ESounds.vo_destroy_expansion, true);
                        break;

                    case (EBaseType.Supremacy):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_destroy_enemysupremecy : ESounds.vo_destroy_supremecy, true);
                        break;

                    case (EBaseType.Outpost):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_destroy_enemyoutpost : ESounds.vo_destroy_outpost, true);
                        break;

                    case (EBaseType.Starbase):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_destroy_enemygarrison : ESounds.vo_destroy_garrison, true);
                        break;

                    case (EBaseType.Tactical):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_destroy_enemytactical : ESounds.vo_destroy_tactical, true);
                        break;

                    case (EBaseType.Refinery):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_destroy_enemyrefinery : ESounds.vo_destroy_refinery, true);
                        break;

                    case (EBaseType.Shipyard):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_destroy_enemyshipyard : ESounds.vo_destroy_shipyard, true);
                        break;
                }
                
                CheckForGameOver();
            }
            else if (e == EBaseEventType.BaseCaptured)
            {
                switch (sender.Type)
                {
                    case (EBaseType.Expansion):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_capture_expansion : ESounds.vo_capture_enemyexpansion, true);
                        break;

                    case (EBaseType.Supremacy):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_capture_supremecy : ESounds.vo_capture_enemysupremecy, true);
                        break;

                    case (EBaseType.Outpost):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_capture_outpost : ESounds.vo_capture_enemyoutpost, true);
                        break;

                    case (EBaseType.Starbase):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_capture_garrison : ESounds.vo_capture_enemygarrison, true);
                        break;
                        
                    case (EBaseType.Tactical):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_capture_tactical : ESounds.vo_capture_enemytactical, true);
                        break;

                    case (EBaseType.Shipyard):
                        SoundEffect.Play(sender.Team == 2 ? ESounds.vo_capture_shipyard : ESounds.vo_capture_enemyshipyard, true);
                        break;
                }

                if (sender.Team == 2)
                {
                    if (!StrategyGame.AllBases.Any(_ => _.Active && _.Team == 1 && _.SectorId == sender.SectorId && _.CanLaunchShips()))
                    {
                        SoundEffect.Play(ESounds.vo_sal_sectorlost, true);
                    }
                }
                
                CheckForGameOver();
            }
        }

        private void CheckForGameOver()
        {
            var gameOver = false;

            if (!StrategyGame.AllBases.Any(_ => _.Team == 1 && _.Active && _.CanLaunchShips()))
            {
                WinLose.Text = "You Lose!";
                gameOver = true;
            }
            else if (!StrategyGame.AllBases.Any(_ => _.Team == 2 && _.Active && _.CanLaunchShips()))
            {
                WinLose.Text = "You Win!";
                gameOver = true;
            }

            if (gameOver)
            {
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
        }

        internal void F_ShipEvent(Ship sender, EShipEventType e)
        {
            if (e == EShipEventType.ShipDestroyed)
            {
                CreateExplosion(sender.Bounds, sender.SectorId);

                if (sender.Team == 1 && sender.Type == EShipType.Miner) SoundEffect.Play(ESounds.vo_destroy_miner, true);
                if (sender.Type == EShipType.Miner) StrategyGame.GameStats.TotalMinersDestroyed[sender.Team - 1]++;
                if (sender.Type == EShipType.Constructor) StrategyGame.GameStats.TotalConstructorsDestroyed[sender.Team - 1]++;

                // Launch a Lifepod for each pilot
                var lifepods = new List<Ship>();
                for (var i = 0; i < sender.NumPilots; i++)
                {
                    var lifepod = StrategyGame.Ships.CreateLifepod(sender.Team, sender.Colour, sender.SectorId);
                    lifepod.CenterX = sender.CenterX;
                    lifepod.CenterY = sender.CenterY;
                    lifepod.ShipEvent += F_ShipEvent;
                    lifepods.Add(lifepod);
                }
                sender.NumPilots = 0;

                if (lifepods.Count > 1)
                {
                    StrategyGame.SpreadOrderEvenly<MoveOrder>(lifepods, sender.SectorId, sender.CenterPoint);
                }
                lifepods.ForEach(_ => _.OrderShip(new PodDockOrder(_, true), true));
                
                StrategyGame.AddUnits(lifepods);
            }
            if (e == EShipEventType.BuildingStarted)
            {
                var b = sender as BuilderShip;
                if (b != null && BaseSpecs.IsTower(b.BaseType))
                {
                    var type = (EShipType)Enum.Parse(typeof(EShipType), b.BaseType.ToString());
                    var tower = StrategyGame.Ships.CreateTowerShip(type, b.Team, b.Colour, b.SectorId);
                    if (tower == null) return;

                    tower.CenterX = b.CenterX;
                    tower.CenterY = b.CenterY;
                    tower.ShipEvent += F_ShipEvent;

                    StrategyGame.AddUnit(tower);
                    b.Active = false;
                }
            }
            if (e == EShipEventType.BuildingFinished)
            {
                var b = sender as BuilderShip;
                if (b != null)
                {
                    b.Target.BuildingComplete();
                    StrategyGame.BuildableAsteroids.Remove(b.Target);
                    StrategyGame.AllAsteroids.Remove(b.Target);
                    
                    var newBase = b.GetFinishedBase();
                    newBase.BaseEvent += B_BaseEvent;

                    var secured = (sender.Team == 1 && newBase.CanLaunchShips() && !StrategyGame.AllBases.Any(_ => _.Active && _.SectorId == sender.SectorId && _.CanLaunchShips()));

                    StrategyGame.AddBase(newBase);
                    StrategyGame.GameStats.TotalBasesBuilt[sender.Team - 1]++;
                    StrategyGame.UnlockTech(newBase.Type, newBase.Team);
                    _researchForm.RefreshItems();

                    if (newBase.Team == 1)
                    {
                        switch (newBase.Type)
                        {
                            case EBaseType.Outpost:
                                SoundEffect.Play(ESounds.vo_builder_outpost, true);
                                break;
                            case EBaseType.Refinery:
                                SoundEffect.Play(ESounds.vo_builder_refinery, true);
                                break;
                            case EBaseType.Starbase:
                                SoundEffect.Play(ESounds.vo_builder_garrison, true);
                                break;
                            case EBaseType.Supremacy:
                                SoundEffect.Play(ESounds.vo_builder_supremecy, true);
                                break;
                            case EBaseType.Tactical:
                                SoundEffect.Play(ESounds.vo_builder_tactical, true);
                                break;
                            case EBaseType.Expansion:
                                SoundEffect.Play(ESounds.vo_builder_expansion, true);
                                break;
                            case EBaseType.Shipyard:
                                SoundEffect.Play(ESounds.vo_builder_shipyard, true);
                                break;
                        }

                        if (secured) SoundEffect.Play(ESounds.vo_sal_sectorsecured, true);
                    }
                }
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
            SoundEffect.Play(ESounds.small_explosion);
        }

        private void UpdateFrame()
        {
            var g = Graphics.FromImage(_frame);
            //g.Clear(BackColor);
            g.FillRectangle(_bgBrush, 0, 0, Width, Height);
            StrategyGame.Map.DrawSector(g, _currentSector.Id);

            if (_shiftDown)
            {
                foreach (var u in _selectedUnits)
                {
                    if (!u.Selected || u.CurrentOrder == null) continue;

                    u.CurrentOrder.Draw(g, u.CenterPoint, _currentSector.Id);
                    var lastPos = u.CurrentOrder.OrderPosition;

                    // don't show orders after a navigate (another sector!)...
                    if (typeof(NavigateOrder) == u.CurrentOrder.GetType()) continue;

                    foreach (var o in u.Orders)
                    {
                        o.Draw(g, lastPos, _currentSector.Id);
                        lastPos = o.OrderPosition;
                        if (typeof(NavigateOrder) == o.GetType()) break;                        
                    }
                }
            }

            if (_ctrlDown)
            {
                foreach (var u in _selectedUnits)
                {
                    if (u.SectorId != _currentSector.Id) continue;

                    var range = u.ScanRange*2;
                    var rangeHalf = u.ScanRange;
                    g.FillEllipse(_sensorBrush, u.CenterX - rangeHalf, u.CenterY - rangeHalf, range, range);

                    range /= 2;
                    rangeHalf /= 2;
                    g.DrawEllipse(_sensorPen, u.CenterX - rangeHalf, u.CenterY - rangeHalf, range, range);
                }

                foreach (var u in _selectedBases)
                {
                    if (u.SectorId != _currentSector.Id) continue;

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
                    if (u.SectorId != _currentSector.Id) continue;
                    u.Draw(g);
                }
            }

            lock (StrategyGame.AllUnits)
            {
                foreach (var u in StrategyGame.AllUnits)
                {
                    if (u.SectorId != _currentSector.Id) continue;
                    u.Draw(g);
                }
            }
            
            foreach (var a in _animations)
            {
                a.Draw(g, a.SectorId == _currentSector.Id);
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
            for (var i = 0; i < StrategyGame.AllUnits.Count; i++)
            {
                var u = StrategyGame.AllUnits[i];
                u.Update();
            }

            for (var i = 0; i < StrategyGame.AllBases.Count; i++)
            {
                var u = StrategyGame.AllBases[i];
                u.Update();
            }

            StrategyGame.AllUnits.RemoveAll(_ => !_.Active);
            StrategyGame.AllBases.RemoveAll(_ => !_.Active);
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
            StrategyGame.UpdateVisibility(false, _currentSector.Id);

            if (_mapForm.Visible)
                _mapForm.UpdateMap(_currentSector.Id);

            for (var t = 0; t < StrategyGame.NumTeams; t++)
            {
                var items = (from i in StrategyGame.TechTree[t].TechItems
                             where !i.Completed
                             && i.AmountInvested > 0
                             select i).ToList();

                if (items.Count > 0)
                {
                    var researchableBefore = _researchForm.ShownResearchableItems(t);

                    items.ForEach(_ => _.UpdateEachSecond(timer.Interval));
                    _researchForm.CheckForCompletedItems(researchableBefore, t);

                    if (t == 0)
                    {
                        RefreshCommandText();

                        if (_researchForm.Visible)
                        {
                            _researchForm.UpdateItems(items);
                        }
                    }
                }
                StrategyGame.AddResources(t+1, (int)(StrategyGame.ResourceRegularAmount * StrategyGame.GameSettings.ResourcesEachTickMultiplier), false);
            }

            _ai.Update();
            if (_debugForm.Visible)
            {
                _debugForm.UpdateDebugInfo();
            }
            
            if (_pilotList.Visible)
            {
                _pilotList.RefreshPilotList();
            }

            if (AlertMessage.Visible && DateTime.Now >= _alertExpire)
            {
                AlertMessage.Visible = false;
            }
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
