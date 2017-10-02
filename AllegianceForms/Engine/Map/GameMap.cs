using AllegianceForms.Engine.Rocks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Map
{    

    public class GameMap
    {
        public const int WormholeRadius = 325;
        
        public string Name { get; set; }
        public List<MapSector> Sectors { get; set; }        
        public List<Wormhole> Wormholes { get; set; }
        public Image GridImage { get; set; }
        public Rectangle MiniMapBounds { get; set; }

        private Pen _sectorPen = new Pen(Color.DarkBlue, 4);
        private Pen _currentSectorPen = new Pen(Color.DarkGreen, 4);
        private Pen _wormholePen = new Pen(Color.DarkGray, 4);
        private Pen _criticalAlertPen = new Pen(Color.Yellow, 2);
        private Pen _conflictPen = new Pen(Color.Red, 2);

        private PathfindingGraph<MapSector> _pathfinding;
        private StrategyGame _game;

        public GameMap(StrategyGame game)
        {
            GridImage = Image.FromFile(".\\Art\\Backgrounds\\Grid.png");
            Sectors = new List<MapSector>();
            Wormholes = new List<Wormhole>();
            _game = game;
        }

        public void Draw(Graphics g, int sectorId)
        {
            foreach(var w in Wormholes)
            {
                if (!w.End1.VisibleToTeam[0] || !w.End2.VisibleToTeam[0]) continue;

                var x1 = w.Sector1.MapPosition.X * GameMaps.SectorSpacing + GameMaps.SectorRadius + GameMaps.MapPadding;
                var y1 = w.Sector1.MapPosition.Y * GameMaps.SectorSpacing + GameMaps.SectorRadius + GameMaps.MapPadding;
                var x2 = w.Sector2.MapPosition.X * GameMaps.SectorSpacing + GameMaps.SectorRadius + GameMaps.MapPadding;
                var y2 = w.Sector2.MapPosition.Y * GameMaps.SectorSpacing + GameMaps.SectorRadius + GameMaps.MapPadding;

                g.DrawLine(_wormholePen, x1, y1, x2, y2);
            }

            foreach(var s in Sectors)
            {
                if (!s.VisibleToTeam[0]) continue;
                var xs = s.MapPosition.X * GameMaps.SectorSpacing + GameMaps.MapPadding;
                var ys = s.MapPosition.Y * GameMaps.SectorSpacing + GameMaps.MapPadding;
                var pen = sectorId == s.Id ? _currentSectorPen : _sectorPen;

                g.FillEllipse(s.Colour1, xs, ys, GameMaps.SectorDiameter, GameMaps.SectorDiameter);
                if (s.Colour2Set)
                    g.FillEllipse(s.Colour2, xs+GameMaps.SectorHalfRadius, ys+GameMaps.SectorHalfRadius, GameMaps.SectorRadius, GameMaps.SectorRadius);

                if (s.CriticalAlert) g.DrawEllipse(_criticalAlertPen, xs-4, ys-4, GameMaps.SectorDiameter+8, GameMaps.SectorDiameter+8);
                if (s.Conflict) g.DrawEllipse(_conflictPen, xs-2, ys-2, GameMaps.SectorDiameter+4, GameMaps.SectorDiameter+4);
                g.DrawEllipse(pen, xs, ys, GameMaps.SectorDiameter, GameMaps.SectorDiameter);
            }
        }
        
        private void SetupMiniMapBounds()
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;

            foreach (var s in Sectors)
            {
                var xs = s.MapPosition.X * GameMaps.SectorSpacing + GameMaps.MapPadding;
                var ys = s.MapPosition.Y * GameMaps.SectorSpacing + GameMaps.MapPadding;

                if (xs - GameMaps.SectorRadius < minX)
                    minX = xs - GameMaps.SectorDiameter;
                if (xs + GameMaps.SectorRadius > maxX)
                    maxX = xs + GameMaps.SectorDiameter;

                if (ys - GameMaps.SectorRadius < minY)
                    minY = ys - GameMaps.SectorDiameter;
                if (ys + GameMaps.SectorRadius > maxY)
                    maxY = ys + GameMaps.SectorDiameter;
            }

            MiniMapBounds = new Rectangle(0, 0, maxX + GameMaps.MapPadding, maxY + GameMaps.MapPadding);
        }

        public void DrawSector(Graphics g, int sectorId)
        {
            //g.DrawImage(GridImage, 0, 0, StrategyGame.ScreenWidth, StrategyGame.ScreenHeight);

            foreach (var u in _game.AllAsteroids)
            {
                u.Draw(g, sectorId);
            }

            foreach (var w in Wormholes)
            {
                if (w.Sector1.Id == sectorId)
                {
                    w.End1.Draw(g, sectorId);
                    continue;
                }
                if (w.Sector2.Id == sectorId)
                {
                    w.End2.Draw(g, sectorId);
                    continue;
                }
            }
        }

        public void SetVisibilityToTeam(int team, bool visible)
        {
            foreach (var w in Wormholes)
            {
                w.SetVisibleToTeam(team, visible);
            }
        }

        public void InitialiseMap()
        {
            ArrangeWormholes();
            SetupRocks();
            GenerateGraph();
            SetupMiniMapBounds();
        }

        public static GameMap FromSimpleMap(StrategyGame game, SimpleGameMap map, bool preview = false)
        {
            var m = new GameMap(game)
            {
                Name = map.Name,
                Sectors = map.Sectors.Select(_ => new MapSector(game, _.Id, GameMaps.SectorNames.NextString, _.MapPosition) { StartingSector = _.StartingSectorTeam}).ToList()
            };

            m.Wormholes = map.WormholeIds.Select(_ => new Wormhole(game, m.Sectors[_.FromSectorId], m.Sectors[_.ToSectorId])).ToList();

            if (preview)
            {
                foreach(var s in m.Sectors)
                {
                    s.VisibleToTeam[0] = true;
                    if (s.StartingSector > 0)
                    {
                        s.Colour1 = new SolidBrush(Color.FromArgb(GameSettings.DefaultTeamColours[s.StartingSector - 1]));
                    }
                }

                foreach ( var w in m.Wormholes)
                {
                    w.SetVisibleToTeam(1, true);
                }
            }

            m.InitialiseMap();
            return m;
        }

        public SimpleGameMap ToSimpleMap()
        {
            var m = new SimpleGameMap(Name);

            foreach (var s in Sectors)
            {
                m.Sectors.Add(new SimpleMapSector(s.Id, s.MapPosition) { StartingSectorTeam = s.StartingSector });
            }

            foreach (var w in Wormholes)
            {
                var s1 = w.Sector1.Id;
                var s2 = w.Sector2.Id;

                if (!m.WormholeIds.Any(_ => (_.FromSectorId == s1 || _.ToSectorId == s1) && (_.FromSectorId == s2 || _.ToSectorId == s2)))
                {
                    m.WormholeIds.Add(new WormholeId(s1, s2));
                }
            }
            return m;
        }

        private void ArrangeWormholes()
        {
            if (Wormholes.Count == 0) return;

            var centerX = StrategyGame.ScreenWidth / 2;
            var centerY = StrategyGame.ScreenHeight / 2;

            foreach (var s in Sectors)
            {
                var allW = (from w in Wormholes
                              where w.Sector1 == s
                              || w.Sector2 == s
                              select w).ToList();
                
                var angleInDegrees = 360 / allW.Count;
                var currentAngle = StrategyGame.Random.Next(360);

                foreach (var w in allW)
                {
                    w.End1.Signature *= _game.GameSettings.WormholesSignatureMultiplier;
                    w.End2.Signature *= _game.GameSettings.WormholesSignatureMultiplier;

                    var angleAsRadians = (currentAngle * Math.PI) / 180.0;
                    var x = centerX + Math.Cos(angleAsRadians) * WormholeRadius;
                    var y = centerY + Math.Sin(angleAsRadians) * WormholeRadius;

                    if (s == w.Sector1)
                    {
                        w.End1.CenterX = (int)x;
                        w.End1.CenterY = (int)y;
                    }
                    else
                    {
                        w.End2.CenterX = (int)x;
                        w.End2.CenterY = (int)y;
                    }

                    currentAngle += angleInDegrees;
                }                
            }
        }
        
        public void SetupRocks()
        {
            var rnd = StrategyGame.Random;
            var settings = _game.GameSettings;
            var asteroids = new List<Asteroid>();
            var centerPos = new Point(StrategyGame.ScreenWidth / 2, StrategyGame.ScreenHeight / 2);

            foreach (var sector in Sectors)
            {
                var rockSize = 60;
                var radiusX = 400;
                var radiusY = 300;
                // Rocks
                for (var i = 0; i < settings.RocksPerSectorGeneral; i++)
                {
                    var a = new Asteroid(_game, rnd, rockSize, rockSize, sector.Id);
                    a.CenterX = rnd.Next(centerPos.X - radiusX, centerPos.X + radiusX);
                    a.CenterY = rnd.Next(centerPos.Y - radiusY, centerPos.Y + radiusY);
                    for (var t = 0; t < _game.NumTeams; t++)
                    {
                        a.VisibleToTeam[t] = settings.RocksVisible;
                    }
                    asteroids.Add(a);
                    _game.BuildableAsteroids.Add(a);
                }

                // Tech
                rockSize = 70;
                radiusX = 200;
                radiusY = 100;
                List<Asteroid> rockOptions = new List<Asteroid>();
                for (var i = 0; i < settings.RocksPerSectorTech; i++)
                {
                    if (rockOptions.Count == 0)
                    {
                        rockOptions.Add(new TechCarbonAsteroid(_game, rnd, rockSize, rockSize, sector.Id));
                        rockOptions.Add(new TechSiliconAsteroid(_game, rnd, rockSize, rockSize, sector.Id));
                        rockOptions.Add(new TechUraniumAsteroid(_game, rnd, rockSize, rockSize, sector.Id));
                    }
                    var a = rockOptions[rnd.Next(0, rockOptions.Count)];

                    a.CenterX = rnd.Next(centerPos.X - radiusX, centerPos.X + radiusX);
                    a.CenterY = rnd.Next(centerPos.Y - radiusY, centerPos.Y + radiusY);
                    asteroids.Add(a);
                    for (var t = 0; t < _game.NumTeams; t++)
                    {
                        a.VisibleToTeam[t] = settings.RocksVisible;
                    }
                    _game.BuildableAsteroids.Add(a);
                    rockOptions.Remove(a);
                }

                // Resources
                rockSize = 40;
                radiusX = 300;
                radiusY = 200;
                for (var i = 0; i < settings.RocksPerSectorResource; i++)
                {
                    var a = new ResourceAsteroid(_game, rnd, rockSize, rockSize, sector.Id);
                    a.CenterX = rnd.Next(centerPos.X - radiusX, centerPos.X + radiusX);
                    a.CenterY = rnd.Next(centerPos.Y - radiusY, centerPos.Y + radiusY);
                    for (var t = 0; t < _game.NumTeams; t++)
                    {
                        a.VisibleToTeam[t] = settings.RocksVisible;
                    }
                    _game.ResourceAsteroids.Add(a);
                    asteroids.Add(a);
                }                
            }
            _game.AllAsteroids.AddRange(asteroids);
        }

        public void GenerateGraph()
        {
            _pathfinding = new PathfindingGraph<MapSector>();

            foreach (var s in Sectors)
            {
                var edges = new Dictionary<MapSector, int>();

                foreach (var w in Wormholes)
                {
                    if (w.End1.SectorId == s.Id)
                    {
                        var nextSector = Sectors[w.End2.SectorId];
                        edges[nextSector] = 1;
                    }
                    else if (w.End2.SectorId == s.Id)
                    {
                        var nextSector = Sectors[w.End1.SectorId];
                        edges[nextSector] = 1;
                    }
                }

                _pathfinding.AddVertex(s, edges);
            }
        }

        public List<MapSector> ShortestPath(int team, int fromSectorId, int toSectorId)
        {
            if (_pathfinding == null) return null;

            return _pathfinding.ShortestPath(team, Sectors[fromSectorId], Sectors[toSectorId]);
        }
    }
}
