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
        public EMapSize Size { get; set; }
        
        private Pen _sectorPen = new Pen(Color.DarkBlue, 4);
        private Pen _currentSectorPen = new Pen(Color.DarkGreen, 4);
        private Pen _wormholePen = new Pen(Color.DarkGray, 4);
        private Pen _criticalAlertPen = new Pen(Color.Yellow, 2);
        private Pen _conflictPen = new Pen(Color.Red, 2);

        private PathfindingGraph<MapSector> _pathfinding;

        public GameMap()
        {
            GridImage = Image.FromFile(".\\Art\\Backgrounds\\Grid.png");
            Sectors = new List<MapSector>();
            Wormholes = new List<Wormhole>();
        }

        public void Draw(Graphics g, int sectorId)
        {
            foreach(var w in Wormholes)
            {
                if (!w.End1.VisibleToTeam[0] || !w.End2.VisibleToTeam[0]) continue;

                var x1 = w.Sector1.MapPosition.X * GameMaps.SectorSpacing + GameMaps.SectorRadius;
                var y1 = w.Sector1.MapPosition.Y * GameMaps.SectorSpacing + GameMaps.SectorRadius;
                var x2 = w.Sector2.MapPosition.X * GameMaps.SectorSpacing + GameMaps.SectorRadius;
                var y2 = w.Sector2.MapPosition.Y * GameMaps.SectorSpacing + GameMaps.SectorRadius;

                g.DrawLine(_wormholePen, x1, y1, x2, y2);
            }

            foreach(var s in Sectors)
            {
                if (!s.VisibleToTeam[0]) continue;
                var xs = s.MapPosition.X * GameMaps.SectorSpacing;
                var ys = s.MapPosition.Y * GameMaps.SectorSpacing;
                var pen = sectorId == s.Id ? _currentSectorPen : _sectorPen;

                g.FillEllipse(s.Colour1, xs, ys, GameMaps.SectorDiameter, GameMaps.SectorDiameter);
                if (s.Colour2Set)
                    g.FillEllipse(s.Colour2, xs+GameMaps.SectorHalfRadius, ys+ GameMaps.SectorHalfRadius, GameMaps.SectorRadius, GameMaps.SectorRadius);

                if (s.CriticalAlert) g.DrawEllipse(_criticalAlertPen, xs-4, ys-4, GameMaps.SectorDiameter+8, GameMaps.SectorDiameter+8);
                if (s.Conflict) g.DrawEllipse(_conflictPen, xs-2, ys-2, GameMaps.SectorDiameter+4, GameMaps.SectorDiameter+4);
                g.DrawEllipse(pen, xs, ys, GameMaps.SectorDiameter, GameMaps.SectorDiameter);
            }
        }

        public void DrawSector(Graphics g, int sectorId)
        {
            g.DrawImage(GridImage, 0, 0);

            foreach (var u in StrategyGame.AllAsteroids)
            {
                if (u.SectorId != sectorId) continue;

                u.Draw(g);
            }

            foreach (var w in Wormholes)
            {
                if (w.Sector1.Id == sectorId)
                {
                    w.End1.Draw(g);
                    continue;
                }
                if (w.Sector2.Id == sectorId)
                {
                    w.End2.Draw(g);
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
                    w.End1.Signature *= StrategyGame.GameSettings.WormholesSignatureMultiplier;
                    w.End2.Signature *= StrategyGame.GameSettings.WormholesSignatureMultiplier;

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
            var settings = StrategyGame.GameSettings;
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
                    var a = new Asteroid(rnd, rockSize, rockSize, sector.Id);
                    a.CenterX = rnd.Next(centerPos.X - radiusX, centerPos.X + radiusX);
                    a.CenterY = rnd.Next(centerPos.Y - radiusY, centerPos.Y + radiusY);
                    for (var t = 0; t < StrategyGame.NumTeams; t++)
                    {
                        a.VisibleToTeam[t] = settings.RocksVisible;
                    }
                    asteroids.Add(a);
                    StrategyGame.BuildableAsteroids.Add(a);
                }

                // Tech
                rockSize = 70;
                radiusX = 200;
                radiusY = 100;
                var rockOptionsFull = new List<Asteroid>
                {
                    new TechCarbonAsteroid(rnd, rockSize, rockSize, sector.Id),
                    new TechSiliconAsteroid(rnd, rockSize, rockSize, sector.Id),
                    new TechUraniumAsteroid(rnd, rockSize, rockSize, sector.Id)
                };
                var rockOptions = new List<Asteroid>(rockOptionsFull);
                for (var i = 0; i < settings.RocksPerSectorTech; i++)
                {
                    var a = rockOptions[rnd.Next(0, rockOptions.Count)];
                    a.CenterX = rnd.Next(centerPos.X - radiusX, centerPos.X + radiusX);
                    a.CenterY = rnd.Next(centerPos.Y - radiusY, centerPos.Y + radiusY);
                    asteroids.Add(a);
                    for (var t = 0; t < StrategyGame.NumTeams; t++)
                    {
                        a.VisibleToTeam[t] = settings.RocksVisible;
                    }
                    StrategyGame.BuildableAsteroids.Add(a);
                    rockOptions.Remove(a);

                    if (rockOptions.Count == 0) rockOptions.AddRange(rockOptionsFull);
                }

                // Resources
                rockSize = 40;
                radiusX = 300;
                radiusY = 200;
                for (var i = 0; i < settings.RocksPerSectorResource; i++)
                {
                    var a = new ResourceAsteroid(rnd, rockSize, rockSize, sector.Id);
                    a.CenterX = rnd.Next(centerPos.X - radiusX, centerPos.X + radiusX);
                    a.CenterY = rnd.Next(centerPos.Y - radiusY, centerPos.Y + radiusY);
                    for (var t = 0; t < StrategyGame.NumTeams; t++)
                    {
                        a.VisibleToTeam[t] = settings.RocksVisible;
                    }
                    StrategyGame.ResourceAsteroids.Add(a);
                    asteroids.Add(a);
                }                
            }
            StrategyGame.AllAsteroids.AddRange(asteroids);
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
