using AllegianceForms.Engine.Map;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AllegianceForms.Engine.Generation
{
    public class RandomMap
    {
        public static GameMap Generate(EMapSize size, bool symetric, RandomString sectorNames)
        {
            // position some random sectors, add a starting base, then flip either X or Y
            sectorNames.Reset();

            var map = new GameMap { Size = size };

            var num = 3 + (int)size;
            var overlapTolerance = 4;

            if (size == EMapSize.Massive) num++;

            var flipX = StrategyGame.Random.NextDouble() > 0.5;

            var startX = 40;
            var startY = 40;
            MapSector sect = null;
            MapSector lastSect = null;
            MapSector firstSect = null;

            for (var i = 0; i < num; i++)
            {
                lastSect = sect;
                sect = new MapSector(i, sectorNames.NextString, new Point(startX, startY));
                map.Sectors.Add(sect);

                if (lastSect != null)
                {
                    map.Wormholes.Add(new Wormhole(sect, lastSect));
                }
                else
                {
                    firstSect = sect;
                }

                var newPos = new Point();

                do
                {
                    newPos.X = startX + (StrategyGame.Random.Next(3, 5) * 2 * (StrategyGame.Random.NextDouble() > 0.5 ? -1 : 1));
                    newPos.Y = startY + (StrategyGame.Random.Next(3, 5) * 2 * (StrategyGame.Random.NextDouble() > 0.5 ? -1 : 1));

                } while (!map.Sectors.TrueForAll(_ => !StrategyGame.WithinDistance(_.MapPosition.X, _.MapPosition.Y, newPos.X, newPos.Y, overlapTolerance)));

                startX = newPos.X;
                startY = newPos.Y;
            }
            lastSect = sect;

            var leftMostX = map.GetMinX();
            var rightMostX = map.GetMaxX();
            var topMostY = map.GetMinY();
            var bottomMostY = map.GetMaxY();

            // Choose a random sector as a Starting base.
            MapSector start;
            if (flipX)
            {
                do
                {
                    start = map.Sectors[StrategyGame.Random.Next(map.Sectors.Count)];
                } while (start.MapPosition.X == rightMostX);
            }
            else
            {
                do
                {
                    start = map.Sectors[StrategyGame.Random.Next(map.Sectors.Count)];
                } while (start.MapPosition.Y == bottomMostY);
            }
            start.StartingSector = true;

            // Link the end nodes with some others nearby
            var endLinks = map.Sectors.Count / 2 - 1;
            if (endLinks > 0)
            {
                AddLinksToClosest(map, map.Sectors, firstSect, endLinks, overlapTolerance);
                AddLinksToClosest(map, map.Sectors, lastSect, endLinks, overlapTolerance);
            }
            
            // now mirror!
            sect = null;
            lastSect = null;

            var mirrorSectors = new List<MapSector>(map.Sectors);
            var mirroredSectors = new List<MapSector>();
            foreach (var m in mirrorSectors)
            {
                lastSect = sect;
                sect = new MapSector(0, sectorNames.NextString, Point.Empty);
                
                sect.StartingSector = m.StartingSector;
                Point pos;

                if (flipX)
                {
                    pos = new Point(rightMostX + (rightMostX - m.MapPosition.X) + overlapTolerance, m.MapPosition.Y);

                    if (symetric)
                        pos.Y = bottomMostY + (bottomMostY - m.MapPosition.Y);
                }
                else
                {
                    pos = new Point(m.MapPosition.X, bottomMostY + (bottomMostY - m.MapPosition.Y) + overlapTolerance);

                    if (symetric)
                        pos.X = rightMostX + (rightMostX - m.MapPosition.X);
                }

                sect.MapPosition = pos;
                mirroredSectors.Add(sect);
                map.Sectors.Add(sect);

                if (lastSect != null)
                {
                    map.Wormholes.Add(new Wormhole(sect, lastSect));
                }
                else
                {
                    firstSect = sect;
                }
            }

            lastSect = sect;

            // Link the end nodes with some others nearby
            if (endLinks > 0)
            {
                AddLinksToClosest(map, mirroredSectors, firstSect, endLinks, overlapTolerance);
                AddLinksToClosest(map, mirroredSectors, lastSect, endLinks, overlapTolerance);
            }

            // Join mirrors!
            var numFlipLinks = Math.Max(1, (map.Sectors.Count + 1) / 2);

            for (var i = 0; i < numFlipLinks; i++)
            {
                var checkLoop = 100;
                do
                {
                    checkLoop--;
                    if (checkLoop <= 0)
                    {
                        sect = null;
                        break;
                    }
                    sect = mirrorSectors[StrategyGame.Random.Next(mirrorSectors.Count)];
                    lastSect = mirroredSectors[mirrorSectors.IndexOf(sect)];
                } while (sect.StartingSector || map.HasASectorCloseToLine(sect, lastSect, map.Sectors, overlapTolerance));

                if (sect != null && !map.AreSectorsLinkedDirectly(sect, lastSect)) map.Wormholes.Add(new Wormhole(sect, lastSect));
            }

            map.AdjustBoundsTopLeft(4);

            // retry :(
            if (!map.IsValid())
            {
                return Generate(size, symetric, sectorNames);
            }

            return map;
        }

        private static void AddLinksToClosest(GameMap map, List<MapSector> sectors, MapSector sector, int numLinks, float overlapTolerance)
        {
            var closestSectors = new List<MapSector>();

            foreach (var s in sectors)
            {
                if (s != sector && !map.AreSectorsLinkedDirectly(s, sector))
                {
                    closestSectors.Add(s);
                }
            }

            closestSectors.Sort(new MapDistanceComparer(sector));
                        
            var count = 0;

            foreach (var s in closestSectors)
            {
                if (count >= numLinks) return;

                if (!map.HasASectorCloseToLine(sector, s, sectors, overlapTolerance) && !map.AreSectorsLinkedDirectly(s, sector))
                {
                    map.Wormholes.Add(new Wormhole(sector, s));
                    count++;
                }
            }
        }
    }
}
