using AllegianceForms.Engine.Map;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Generation
{
    public class RandomMap
    {
        public const int MaxWidth = 7;
        public const int MaxHeight = 7;

        public static Point MirrorPoint(Point p, EMirrorType type)
        {
            switch (type)
            {
                case EMirrorType.Horizontal:
                    return new Point(MaxWidth - p.X - 1, p.Y);

                case EMirrorType.Vertical:
                    return new Point(p.X, MaxHeight - p.Y - 1);
            }

            return Point.Empty;
        }

        public static SimpleMapSector MirrorSector(SimpleMapSector sector, EMirrorType type)
        {
            switch (type)
            {
                case EMirrorType.Horizontal:
                    if (sector.MapPosition.X >= MaxWidth / 2) return null;
                    return new SimpleMapSector(0, MirrorPoint(sector.MapPosition, type));

                case EMirrorType.Vertical:
                    if (sector.MapPosition.Y >= MaxHeight / 2) return null;
                    return new SimpleMapSector(0, MirrorPoint(sector.MapPosition, type));
            }

            return null;
        }

        public static SimpleGameMap GenerateSimpleMap(EMapSize size)
        {
            var rnd = StrategyGame.Random;
            var map = new SimpleGameMap($"{size} Random");
            var mirror = (EMirrorType)rnd.Next(2);
            var maxX = (mirror == EMirrorType.Horizontal ? MaxWidth / 2 : MaxWidth);
            var maxY = (mirror == EMirrorType.Vertical ? MaxHeight / 2 : MaxHeight);
            var nextSectorId = 0;
            var numSectors = 2 + (int)size;

            var s1 = new SimpleMapSector(nextSectorId++, new Point(rnd.Next(maxX), rnd.Next(maxY)));
            s1.StartingSectorTeam = 1;
            map.Sectors.Add(s1);

            var s2 = new SimpleMapSector(nextSectorId++, MirrorPoint(s1.MapPosition, mirror));
            s2.StartingSectorTeam = 2;
            map.Sectors.Add(s2);

            for (var i = 0; i < numSectors; i++)
            {
                var attemptCount = 0;
                bool pathClear;
                Point p;
                SimpleMapSector linkedSector;
                var checkSectors = Utils.Shuffle(map.Sectors, rnd);
                do
                {
                    p = new Point(rnd.Next(maxX), rnd.Next(maxY));
                    pathClear = false;
                    linkedSector = null;
                    attemptCount++;

                    var existingSector = map.Sectors.FirstOrDefault(_ => _.MapPosition == p);
                    if (existingSector != null) continue;

                    // Checking if there is a clear path from this point to any other random sector, to add a wormhole                    
                    foreach (var s in checkSectors)
                    {
                        if (s.MapPosition == p) continue;

                        pathClear = IsPathClear(map, p, s.MapPosition, 2f);
                        if (pathClear)
                        {
                            linkedSector = s;
                            break;
                        }
                    }

                } while (!pathClear && attemptCount < 40);

                if (pathClear)
                {
                    var newS = new SimpleMapSector(nextSectorId++, p);
                    map.Sectors.Add(newS);

                    if (!map.WormholeExists(newS.Id, linkedSector.Id))
                        map.WormholeIds.Add(new WormholeId(newS.Id, linkedSector.Id));

                    var newS2 = new SimpleMapSector(nextSectorId++, MirrorPoint(newS.MapPosition, mirror));
                    map.Sectors.Add(newS2);

                    // Add wormhole from the mirrored sector to equivilant
                    var m2 = MirrorPoint(linkedSector.MapPosition, mirror);
                    var linkedSector2 = map.Sectors.FirstOrDefault(_ => _.MapPosition == m2);
                    if (linkedSector2 != null)
                    {
                        if (!map.WormholeExists(newS2.Id, linkedSector2.Id))
                            map.WormholeIds.Add(new WormholeId(newS2.Id, linkedSector2.Id));
                    }
                }
            }
            
            var c = 0;
            foreach (var a in map.Sectors)
            {
                var rndSectors = Utils.Shuffle(map.Sectors, rnd);
                foreach (var b in rndSectors)
                {
                    if (a.StartingSectorTeam != 0 && b.StartingSectorTeam != 0) continue;
                    if (map.WormholeExists(a.Id, b.Id)) continue;

                    if (c < 2 && IsPathClear(map, a.MapPosition, b.MapPosition, 2f))
                    {
                        map.WormholeIds.Add(new WormholeId(a.Id, b.Id));

                        // mirror wormhole!
                        var ma = MirrorPoint(a.MapPosition, mirror);
                        var mb = MirrorPoint(b.MapPosition, mirror);
                        var sa = map.Sectors.FirstOrDefault(_ => _.MapPosition == ma);
                        var sb = map.Sectors.FirstOrDefault(_ => _.MapPosition == mb);

                        if (sa != null && sb != null && !map.WormholeExists(sa.Id, sb.Id))
                        {
                            map.WormholeIds.Add(new WormholeId(sa.Id, sb.Id));
                        }
                        c++;
                        break;
                    }
                }
            }

            if (!map.IsValid()) return GenerateSimpleMap(size);

            return map;
        }

        public static bool IsPathClear(SimpleGameMap map, Point p1, Point p2, float tolerance = 1)
        {
            if (p1 == p2) return false;

            foreach (var s in map.Sectors)
            {
                if (s.MapPosition == p1 || s.MapPosition == p2) continue;
                if (Utils.IsPointOnLine(s.MapPosition, p1, p2, tolerance))
                {
                    return false;
                }
            }

            return true;
        }
        
    }
}
