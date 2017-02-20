using AllegianceForms.AI;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Map;
using AllegianceForms.Engine.Rocks;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Engine.Tech;
using AllegianceForms.Orders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine
{
    
    public static class StrategyGame
    {
        public delegate void GameEventHandler(object sender, EGameEventType e);
        public static event GameEventHandler GameEvent;

        public const int NumTeams = 2;
        public const int ResourcesInitial = 4000;
        public const int ResourceRegularAmount = 2;

        public const string ShipDataFile = ".\\Data\\Ships.txt";
        public const string BaseDataFile = ".\\Data\\Bases.txt";
        public const string TechDataFile = ".\\Data\\Tech.txt";
        public const string IconPicDir = ".\\Art\\Trans\\";
        public static GameSettings GameSettings;
        
        public static double SqrtTwo = Math.Sqrt(2);

        public static int[] DockedPilots = new int[NumTeams];
        public static int[] Credits = new int[NumTeams];
        public static CommanderAI[] AICommanders = new CommanderAI[NumTeams];

        public static float[] ConversionRate = new float[] { 5f, 5f };
        public static TechTree[] TechTree = new TechTree[NumTeams];
        public static ShipSpecs Ships;
        public static BaseSpecs Bases;
        
        public static List<Ship> AllUnits = new List<Ship>();
        public static List<Base> AllBases = new List<Base>();

        public static List<Asteroid> AllAsteroids = new List<Asteroid>();
        public static List<ResourceAsteroid> ResourceAsteroids = new List<ResourceAsteroid>();

        public static double AngleBetweenPoints(PointF from, PointF to)
        {
            var deltaX = to.X - from.X;
            var deltaY = to.Y - from.Y;
            
            return Math.Atan2(deltaY, deltaX) * (180 / Math.PI);
        }

        public static PointF GetNewPoint(PointF p, float d, float angle)
        {
            var rad = (Math.PI / 180) * angle;
            return new PointF((float)(p.X + d * Math.Cos(rad)), (float)(p.Y + d * Math.Sin(rad)));
        }

        public static List<Asteroid> BuildableAsteroids = new List<Asteroid>();

        public static GameStats GameStats;

        public static Random Random = new Random();
        public static GameMap Map;

        public const int ScreenWidth = 1200;
        public const int ScreenHeight = 800;
        private static StringFormat _centeredFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };


        public static GameEntity NextWormholeEnd(int team, int fromSectorId, int toSectorId, out GameEntity _otherEnd)
        {
            // TODO: This no longer checks if the wormholes are visible!
            var path = Map.ShortestPath(team, fromSectorId, toSectorId);

            _otherEnd = null;
            if (path == null || path.Count == 0) return null;

            var nextSector = path[path.Count - 1];

            foreach (var w in Map.Wormholes)
            {
                if (w.End1.SectorId == fromSectorId && nextSector.Id == w.End2.SectorId)
                {
                    _otherEnd = w.End2;
                    return w.End1;
                }
                else if (w.End2.SectorId == fromSectorId && nextSector.Id == w.End1.SectorId)
                {
                    _otherEnd = w.End1;
                    return w.End2;
                }
            }

            return null;  
        }

        public static Base ClosestSectorWithBase(int team, int fromSectorId)
        {
            var t = team - 1;

            var thisSectorBase = AllBases.FirstOrDefault(_ => _.Active && _.Team == team && _.SectorId == fromSectorId && _.CanLaunchShips());
            if (thisSectorBase != null)
            {
                return thisSectorBase;
            }

            var otherSectorBases = AllBases.Where(_ => _.Active && _.Team == team && _.SectorId != fromSectorId && _.CanLaunchShips()).ToList();
            var minHops = int.MaxValue;
            Base targetBase = null;

            foreach ( var b in otherSectorBases)
            {
                var path = Map.ShortestPath(team, fromSectorId, b.SectorId);
                var newHops = path == null ? int.MaxValue : path.Count();

                if (newHops < minHops)
                {
                    minHops = newHops;
                    targetBase = b;
                }
            }

            return targetBase;
        }

        public static Base ClosestEnemyBase(int team, out Base launchingBase)
        {
            var t = team - 1;
            var maxHops = Map.Wormholes.Count + 4;
            var minHops = int.MaxValue;
            Base targetBase = null;
            launchingBase = null;

            var launchingBases = AllBases.Where(_ => _.Active && _.Team == team && _.CanLaunchShips());
            
            foreach (var l in launchingBases)
            {
                if (launchingBase == null)
                if (launchingBase != null && l.SectorId == launchingBase.SectorId) continue;

                var otherSectorBases = AllBases.Where(_ => _.Active && _.VisibleToTeam[t] && _.Team != team && _.SectorId != l.SectorId).ToList();
                foreach (var b in otherSectorBases)
                {
                    var path = Map.ShortestPath(team, l.SectorId, b.SectorId);
                    var newHops = path == null ? int.MaxValue : path.Count();

                    if (newHops < minHops)
                    {
                        minHops = newHops;
                        targetBase = b;
                        launchingBase = l;
                    }
                }
            }

            return targetBase;
        }
        
        public static int NumberOfMinerDrones(int team)
        {
            return (from c in AllUnits
                    where c.Active
                    && c.Team == team
                    && c.Type == EShipType.Miner
                    select c).Count();
        }

        public static int NumberOfConstructionDrones(string name, int team)
        {
            var bType = TechItem.GetBaseType(name);

            var cons = (from c in AllUnits
                        where c.Active
                        && c.Team == team
                        && c.Type == EShipType.Constructor
                        && c.GetType() == typeof(BuilderShip)
                        select c as BuilderShip).ToList();
            return cons.Count(_ => _.BaseType == bType);
        }

        public static void UpdateVisibility(bool init = false, int currentSectorId = -1)
        {
            var soundPlayed = false;
            var bbrSoundPlayed = false;
            var preVis = false;
            lock (AllUnits)
            {
                foreach (var s in AllUnits)
                {
                    for (var t = 0; t < NumTeams; t++)
                    {
                        var team = t + 1;
                        if (team == s.Team) continue;

                        var thisAi = AICommanders[t];
                        if (thisAi != null && thisAi.CheatVisibility)
                        {
                            s.VisibleToTeam[t] = true;
                            continue;
                        }

                        var thatAi = AICommanders[s.Team - 1];
                        if (thatAi != null && thatAi.ForceVisible)
                        {
                            s.VisibleToTeam[t] = true;
                            continue;
                        }

                        preVis = s.VisibleToTeam[t];
                        s.VisibleToTeam[t] = false;
                        if (IsVisibleToTeam(s, team))
                        {
                            if (!preVis && !soundPlayed && t == 0 && s.SectorId == currentSectorId)
                            {
                                SoundEffect.Play(ESounds.newtargetenemy);
                                soundPlayed = true;
                            }

                            if (!preVis && t == 0 && !bbrSoundPlayed && (s.Type == EShipType.Bomber || s.Type == EShipType.FighterBomber || s.Type == EShipType.StealthBomber))
                            {
                                SoundEffect.Play(ESounds.vo_sal_bombersighted, true);
                                bbrSoundPlayed = true;
                            }
                            s.VisibleToTeam[t] = true;
                        }
                    }
                }
            }

            lock (AllBases)
            {
                foreach (var s in AllBases)
                {
                    for (var t = 0; t < NumTeams; t++)
                    {
                        var team = t + 1;
                        // Once visible, bases are always visible!
                        if (team == s.Team || s.VisibleToTeam[t]) continue;

                        var thisAi = AICommanders[t];
                        if (thisAi != null && thisAi.CheatVisibility)
                        {
                            s.VisibleToTeam[t] = true;
                            continue;
                        }

                        var thatAi = AICommanders[s.Team - 1];
                        if (thatAi != null && thatAi.ForceVisible)
                        {
                            s.VisibleToTeam[t] = true;
                            continue;
                        }

                        if (IsVisibleToTeam(s, team))
                        {
                            if (!soundPlayed && t == 0 && s.SectorId == currentSectorId)
                            {
                                SoundEffect.Play(ESounds.newtargetenemy);
                                soundPlayed = true;
                            }
                            s.VisibleToTeam[t] = true;
                        }
                    }
                }
            }

            foreach (var s in AllAsteroids)
            {
                for (var t = 0; t < NumTeams; t++)
                {
                    var team = t + 1;
                    // Once visible, asteroids are always visible!
                    if (s.VisibleToTeam[t]) continue;
                    
                    var thisAi = AICommanders[t];
                    if (thisAi != null && thisAi.CheatVisibility)
                    {
                        s.VisibleToTeam[t] = true;
                        continue;
                    }

                    if (IsVisibleToTeam(s, team))
                    {
                        if (!soundPlayed && t == 0 && s.SectorId == currentSectorId)
                        {
                            if (!init) SoundEffect.Play(ESounds.noncriticalmessage);
                            soundPlayed = true;
                        }
                        s.VisibleToTeam[t] = true;
                    }
                }
            }

            foreach (var w in Map.Wormholes)
            {
                for (var t = 0; t < NumTeams; t++)
                {
                    var team = t + 1;
                    // Once visible, wormholes are always visible!
                    var s1 = w.End1;
                    var s2 = w.End2;
                    if (s1.VisibleToTeam[t] || s2.VisibleToTeam[t]) continue;

                    var thisAi = AICommanders[t];
                    if (thisAi != null && thisAi.CheatVisibility)
                    {
                        w.SetVisibleToTeam(team, true);
                        continue;
                    }

                    if (IsVisibleToTeam(s1, team) || IsVisibleToTeam(s2, team))
                    {
                        if (t == 0 && (s2.SectorId == currentSectorId || s1.SectorId == currentSectorId))
                        {
                            SoundEffect.Play(ESounds.newtargetneutral, true);
                            soundPlayed = true;
                        }
                        w.SetVisibleToTeam(team, true);
                    }
                }
            }

            foreach (var s in Map.Sectors)
            {
                s.UpdateColours();
            }
        }

        public static void AddUnit(Ship s)
        {
            lock (AllUnits)
            {
                AllUnits.Add(s);
            }
        }

        public static void AddUnits(ICollection<Ship> s)
        {
            lock (AllUnits)
            {
                AllUnits.AddRange(s);
            }
        }

        public static void AddBase(Base b)
        {
            lock (AllBases)
            {
                AllBases.Add(b);
            }
        }

        private static bool IsVisibleToTeam(GameEntity s, int team)
        {
            var teamBases = AllBases.Where(_ => _.Active && _.Team == team && s.SectorId == _.SectorId).ToList();
            var closestBase = ClosestDistance(s.CenterX, s.CenterY, teamBases);
            if (closestBase != null)
            {
                var requiredD = (int)(closestBase.ScanRange * s.Signature);
                if (WithinDistance(s.CenterX, s.CenterY, closestBase.CenterX, closestBase.CenterY, requiredD))
                {
                    return true;
                }
            }

            var teamShips = AllUnits.Where(_ => _.Active && _.Team == team && s.SectorId == _.SectorId).ToList();
            var closestShip = ClosestDistance(s.CenterX, s.CenterY, teamShips);
            if (closestShip != null)
            {
                var requiredD = (int)(closestShip.ScanRange * s.Signature);
                if (WithinDistance(s.CenterX, s.CenterY, closestShip.CenterX, closestShip.CenterY, requiredD))
                {
                    return true;
                }
            }

            return false;
        }

        public static void AddResources(int team, int resources, bool sound = true)
        {
            if (resources == 0) return;
            if (team == 1 && sound) SoundEffect.Play(ESounds.payday, true);

            var t = team - 1;
            var amount = (int)(resources * ConversionRate[t] * GameSettings.ResourceConversionRateMultiplier * TechTree[t].ResearchedUpgrades[EGlobalUpgrade.MinerEfficiency]);
            Credits[t] += amount;
            GameStats.TotalResourcesMined[t] += resources;
        }

        public static int SpendCredits(int team, int amount)
        {
            var spentAmount = Math.Min(amount, Credits[team - 1]);

            Credits[team - 1] -= spentAmount;

            return spentAmount;
        }

        public static void ResetGame(GameSettings settings)
        {
            DockedPilots = new int[NumTeams];
            Credits = new int[NumTeams];
            ConversionRate = new[] { 5f, 5f };
            GameStats = new GameStats();
            GameSettings = settings;

            AllUnits.Clear();
            AllBases.Clear();
            AllAsteroids.Clear();
            ResourceAsteroids.Clear();
            BuildableAsteroids.Clear();
        }

        public static void LoadData()
        {
            Ships = ShipSpecs.LoadShipSpecs(ShipDataFile);
            Bases = BaseSpecs.LoadBaseSpecs(BaseDataFile);

            for (var t = 0; t < NumTeams; t++)
            {
                TechTree[t] = Tech.TechTree.LoadTechTree(TechDataFile, t+1);

                var autoCompleted = TechTree[t].TechItems.Where(_ => _.Completed).ToList();
                foreach (var i in autoCompleted)
                {
                    i.Active = false;
                }
            }            
        }

        public static bool CanLaunchShip(int team, int pilotsRequired, EShipType type)
        {
            if (type == EShipType.Constructor || type == EShipType.Tower) return false;

            return DockedPilots[team - 1] >= pilotsRequired;
        }

        public static void LaunchShip(Ship ship)
        {
            var t = ship.Team - 1;
            if (DockedPilots[t] < ship.NumPilots) return;

            DockedPilots[t] -= ship.NumPilots;
            ship.Health = ship.MaxHealth;

            AddUnit(ship);

            OnGameEvent(ship, EGameEventType.ShipLaunched);
        }

        public static void DockShip(int team, int numPilots)
        {
            DockedPilots[team - 1] += numPilots;
        }

        public static void OnGameEvent(object sender, EGameEventType type)
        {
            if (GameEvent != null) GameEvent(sender, type);
        }

        public static bool WithinDistance(float x1, float y1, float x2, float y2, float d)
        {
            var dx = (x1 - x2);
            var dy = (y1 - y2);

            return (dx * dx + dy * dy) < d * d;
        }

        public static double DistanceBetween(Point p1, Point p2)
        {
            var dx = (p1.X - p2.X);
            var dy = (p1.Y - p2.Y);

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static T ClosestDistance<T>(float x, float y, IEnumerable<T> check) where T : GameEntity
        {
            return check.OrderBy(_ => ((x - _.CenterX) * (x - _.CenterX) + (y - _.CenterY) * (y - _.CenterY))).FirstOrDefault();
        }

        public static float Lerp(float firstFloat, float secondFloat, DateTime startTime, TimeSpan duration)
        {
            var by = (float)((DateTime.Now - startTime).TotalMilliseconds / duration.TotalMilliseconds);

            return firstFloat * by + secondFloat * (1 - by);
        }

        // Offset the order position evenly for these units...
        public static void SpreadOrderEvenly<T>(List<Ship> units, int currentSectorId, PointF centerPos, bool append = false) where T : ShipOrder
        {
            var columns = (int)Math.Round(Math.Sqrt(units.Count), 0);
            var offset = units.Max(_ => _.Image.Width) + 4;
            var origX = centerPos.X - (int)(columns / 2.0f * offset);
            var origY = centerPos.Y - (int)(columns / 2.0f * offset);
            var orderPos = new PointF(origX, origY);

            for (var i = 0; i < units.Count; i++)
            {
                var u = units[i];
                if (u.SectorId != currentSectorId) continue;

                ShipOrder order;

                if (typeof(T) == typeof(RefineOrder) || typeof(T) == typeof(DockOrder))
                    order = (T)Activator.CreateInstance(typeof(T), u);
                else
                    order = (T)Activator.CreateInstance(typeof(T), currentSectorId);

                order.OrderPosition = orderPos;
                order.Offset = new PointF(orderPos.X - origX, orderPos.Y - origY);
                u.OrderShip(order, append);

                if ((i + 1) % columns == 0)
                {
                    orderPos.X = origX;
                    orderPos.Y += offset;
                }
                else
                {
                    orderPos.X += offset;
                }
            }
        }

        public static void DrawCenteredText(Graphics g, Brush brush, string text, Rectangle rect)
        {
            g.DrawString(text, SystemFonts.SmallCaptionFont, brush, rect, _centeredFormat);
        }

        public static Color NewAlphaColour(int A, Color color)
        {
            return Color.FromArgb(A, color.R, color.G, color.B);
        }

        public static void UnlockTech(EBaseType type, int team)
        {
            var item = (from t in TechTree[team - 1].TechItems
                        where t.Name == type.ToString()
                        && !t.Completed
                        select t).FirstOrDefault();
            if (item == null) return;

            item.Completed = true;
        }
    }
}
