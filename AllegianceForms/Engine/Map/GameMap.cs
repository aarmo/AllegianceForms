using AllegianceForms.Engine.Rocks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Map
{
    public class GameMap
    {
        private SolidBrush _sectorBrush = new SolidBrush(Color.DimGray);
        private Pen _sectorPen = new Pen(Color.DarkBlue, 4);
        private Pen _currentSectorPen = new Pen(Color.DarkGreen, 4);
        private Pen _wormholePen = new Pen(Color.DarkGray, 4);

        public string Name { get; set; }
        public List<MapSector> Sectors { get; set; }
        public List<Wormhole> Wormholes { get; set; }
        public Image GridImage { get; set; }

        public EMapSize Size { get; set; }

        public GameMap()
        {
            GridImage = Image.FromFile(".\\Art\\Backgrounds\\Grid.png");
            Sectors = new List<MapSector>();
            Wormholes = new List<Wormhole>();
        }
        
        public void Clear()
        {
            Sectors.Clear();
            Wormholes.Clear();
        }

        public GameMap Clone()
        {
            return Utils.CloneMap(this);
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

                g.FillEllipse(_sectorBrush, xs, ys, GameMaps.SectorDiameter, GameMaps.SectorDiameter);
                g.DrawEllipse(pen, xs, ys, GameMaps.SectorDiameter, GameMaps.SectorDiameter);
            }
        }

        public void DrawSector(Graphics g, int sectorId)
        {
            /*
            var s = Sectors.First(_ => _.Id == sectorId);
            g.DrawImage(s.Image, 0, 0);
            */

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

        public void ArrangeWormholes(int distanceFromCenter)
        {
            var centerX = StrategyGame.ScreenWidth / 2;
            var centerY = StrategyGame.ScreenHeight/ 2;

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
                    var x = centerX + Math.Cos(angleAsRadians) * distanceFromCenter;
                    var y = centerY + Math.Sin(angleAsRadians) * distanceFromCenter;

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

        public void SetVisibilityToTeam(int team, bool visible)
        {
            foreach(var w in Wormholes)
            {
                w.SetVisibleToTeam(team, visible);
            }
        }

        public void SetupRocks(Point centerPos)
        {
            var rnd = StrategyGame.Random;
            var settings = StrategyGame.GameSettings;
            var asteroids = new List<Asteroid>();

            foreach (var sector in StrategyGame.Map.Sectors)
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
                    StrategyGame.ResourceAsteroids.Add(a);
                    asteroids.Add(a);
                }                
            }
            StrategyGame.AllAsteroids.AddRange(asteroids);
        }

        public int GetMinX()
        {
            return Sectors.Min(_ => _.MapPosition.X);
        }

        public int GetMinY()
        {
            return Sectors.Min(_ => _.MapPosition.Y);
        }
        public int GetMaxX()
        {
            return Sectors.Max(_ => _.MapPosition.X);
        }

        public int GetMaxY()
        {
            return Sectors.Max(_ => _.MapPosition.Y);
        }

        public void AdjustBoundsTopLeft(int buffer = 0)
        {
            // Bounds
            var leftMostX = GetMinX() - buffer;
            var topMostY = GetMinY() - buffer;

            foreach (var s in Sectors)
            {
                s.MapPosition = new Point(s.MapPosition.X - leftMostX, s.MapPosition.Y - topMostY);
            }
        }
        public bool HasASectorCloseToLine(MapSector s1, MapSector s2, List<MapSector> checkSectors, float minDistance)
        {
            foreach (var s in checkSectors)
            {
                if (s == s1 || s == s2) continue;

                if (LineContainsPoint(s1.MapPosition, s2.MapPosition, s.MapPosition, minDistance)) return true;
            }

            return false;
        }

        private bool LineContainsPoint(Point start, Point end, Point point, double fuzziness)
        {
            Point leftPoint;
            Point rightPoint;

            // Normalize start/end to left right to make the offset calc simpler.
            if (start.X <= end.X)
            {
                leftPoint = start;
                rightPoint = end;
            }
            else
            {
                leftPoint = end;
                rightPoint = start;
            }

            // If point is out of bounds, no need to do further checks.                  
            if (point.X + fuzziness < leftPoint.X || rightPoint.X < point.X - fuzziness)
                return false;
            else if (point.Y + fuzziness < Math.Min(leftPoint.Y, rightPoint.Y) || Math.Max(leftPoint.Y, rightPoint.Y) < point.Y - fuzziness)
                return false;

            double deltaX = rightPoint.X - leftPoint.X;
            double deltaY = rightPoint.Y - leftPoint.Y;

            // If the line is straight, the earlier boundary check is enough to determine that the point is on the line.
            // Also prevents division by zero exceptions.
            if (deltaX == 0 || deltaY == 0)
                return true;

            double slope = deltaY / deltaX;
            double offset = leftPoint.Y - leftPoint.X * slope;
            double calculatedY = point.X * slope + offset;

            // Check calculated Y matches the points Y coord with some easing.
            bool lineContains = point.Y - fuzziness <= calculatedY && calculatedY <= point.Y + fuzziness;

            return lineContains;
        }

        public bool AreSectorsLinkedDirectly(MapSector start, MapSector end)
        {
            var w = Wormholes.Find(_ => (_.Sector1 == start && _.Sector2 == end)
                                || (_.Sector1 == end && _.Sector2 == start));

            return w != null;
        }

        public bool IsValid()
        {
            var startSectors = Sectors.FindAll(s => s.StartingSector);
            if (startSectors.Count == 0) return false;

            return startSectors.TrueForAll(CanReachAllSectors);
        }

        public bool CanReachAllSectors(MapSector fromSector)
        {
            var visited = new List<MapSector>();

            var sectorsTravelled = TravelSector(fromSector, visited);

            return (Sectors.Count == sectorsTravelled);
        }

        private int TravelSector(MapSector start, ICollection<MapSector> visited)
        {
            if (visited.Contains(start)) return 0;

            visited.Add(start);
            var traveled = 1;

            var links = Wormholes.FindAll(w => w.LinksTo(start));

            foreach (var l in links)
            {
                traveled += TravelSector(l.Sector1, visited);
                traveled += TravelSector(l.Sector2, visited);
            }

            return traveled;
        }
    }
}
