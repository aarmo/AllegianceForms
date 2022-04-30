using AllegianceForms.Engine.Map;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace AllegianceForms.Engine.Generation
{
    public class RandomMap
    {
        public const int MaxWidth = 7;
        public const int MaxHeight = 7;

        private static int MirrorDimension(int maxDim, int p) => maxDim - p - 1;

        public static SimpleGameMap GenerateMirroredMap(EMapSize size)
        {
            var rnd = StrategyGame.Random;
            var iSize = (int)size;
            
            var numTeams = rnd.Next(1,3) * 2;

            // Lock Large/Small maps to 2/4 teams for less variation
            if (size == EMapSize.Small) numTeams = 2;
            if (size == EMapSize.Large) numTeams = 4;

            var map = new SimpleGameMap($"{size} Random{numTeams}");

            // 0:hor/1:vert/2:hor&vert
            var mirrorType = numTeams == 2 ? rnd.Next(3) : 2; 
            var mirrorIncrement = mirrorType == 2 ? 4 : 2;

            // Determine num sectors based on size & num teams
            // small: (min = num teams*2, max = num teams*4)
            // normal: (min = num teams*3, max = num teams*5)
            // large: (min = num teams*4, max = num teams*6)
            var minNumSectors = numTeams * (iSize + 2);
            var maxNumSectors = numTeams * (iSize + 4);

            var xDim = 7;
            var yDim = 7;
            var nextSectorId = 0;

            // Add sectors in all positions
            for (var x = 0; x < xDim; x++)
            {
                for (var y = 0; y < yDim; y++)
                {
                    var s = new SimpleMapSector(nextSectorId++, new Point(x, y));
                    map.Sectors.Add(s);
                }
            }

            var maxChances = map.Sectors.Count * 3;
            var l = 0;
            // Loop with a decreasing chance to remove sectors (mirrored)
            var removeChance = 0.97;
            while ((l++ < maxChances || map.Sectors.Count - mirrorIncrement > maxNumSectors) && map.Sectors.Count - mirrorIncrement > minNumSectors)
            {
                var s = StrategyGame.RandomItem(map.Sectors);
                if (rnd.NextDouble() < removeChance)
                {
                    removeChance -= 0.03;
                    map.Sectors.Remove(s);

                    // Horiz mirrored or both
                    if (mirrorType == 0 || mirrorType == 2)
                    {
                        var mX = MirrorDimension(xDim, s.MapPosition.X);
                        var mY = s.MapPosition.Y;

                        var s2 = map.Sectors.FirstOrDefault(_ => _.MapPosition.X == mX && _.MapPosition.Y == mY);
                        if (s2 != null) map.Sectors.Remove(s2);
                    }

                    // Vert mirrored or both
                    if (mirrorType == 1 || mirrorType == 2)
                    {
                        var mX = s.MapPosition.X;
                        var mY = MirrorDimension(yDim, s.MapPosition.Y); 

                        var s2 = map.Sectors.FirstOrDefault(_ => _.MapPosition.X == mX && _.MapPosition.Y == mY);
                        if (s2 != null) map.Sectors.Remove(s2);
                    }

                    // Mirrored for both dimensions
                    if (mirrorType == 2)
                    {
                        var mX = MirrorDimension(xDim, s.MapPosition.X);
                        var mY = MirrorDimension(yDim, s.MapPosition.Y);

                        var s2 = map.Sectors.FirstOrDefault(_ => _.MapPosition.X == mX && _.MapPosition.Y == mY);
                        if (s2 != null) map.Sectors.Remove(s2);
                    }
                }
            }

            // Fix SectorIDs after removing, they actually need to be indexes...
            l = 0;
            foreach (var s in map.Sectors)
            {
                s.Id = l++;
            }

            // Triangulate, loop with a small chance to remove an edges if that sector has > 2 connections
            var vertices = new List<Vertex<SimpleMapSector>>();
            foreach (var s in map.Sectors)
            {
                var p = s.MapPosition;
                vertices.Add(new Vertex<SimpleMapSector>(new Vector2(p.X, p.Y), s));
            }

            var delaunay = Delaunay2D<SimpleMapSector>.Triangulate(vertices);
            var mapEdges = new List<Edge<SimpleMapSector>>();

            foreach (var edge in delaunay.Edges)
            {
                if (!map.WormholeExists(edge.U.Item.Id, edge.V.Item.Id))
                {
                    map.WormholeIds.Add(new WormholeId(edge.U.Item.Id, edge.V.Item.Id));
                }
            }

            // Find Starting Positions (mirrored)
            var teamsAdded = 0;
            {
                var startPosMinX = 0;
                var startPosMinY = 0;

                var startPosMaxX = mirrorType == 1 ? xDim : xDim / 2 - 1;
                var startPosMaxY = mirrorType == 0 ? yDim : yDim / 2 - 1;

                var startSectors = map.Sectors
                    .Where(_ => _.MapPosition.X >= startPosMinX && _.MapPosition.X <= startPosMaxX
                                && _.MapPosition.Y >= startPosMinY && _.MapPosition.Y <= startPosMaxY)
                    .ToList();
                var s = StrategyGame.RandomItem(startSectors);

                // Safety check
                if (s == null)
                    return GenerateMirroredMap(size);

                s.StartingSectorTeam = ++teamsAdded;

                // Mirrored for both dimensions
                if (mirrorType == 2)
                {
                    var mX = MirrorDimension(xDim, s.MapPosition.X);
                    var mY = MirrorDimension(yDim, s.MapPosition.Y);

                    var s2 = map.Sectors.FirstOrDefault(_ => _.MapPosition.X == mX && _.MapPosition.Y == mY);
                    if (s2 != null) s2.StartingSectorTeam = ++teamsAdded;
                }

                // Horiz mirrored or both
                if (mirrorType == 0 || (mirrorType == 2 && numTeams > 2))
                {
                    var mX = MirrorDimension(xDim, s.MapPosition.X);
                    var mY = s.MapPosition.Y;

                    var s2 = map.Sectors.FirstOrDefault(_ => _.MapPosition.X == mX && _.MapPosition.Y == mY);
                    if (s2 != null) s2.StartingSectorTeam = ++teamsAdded;
                }

                // Vert mirrored or both
                if (mirrorType == 1 || (mirrorType == 2 && numTeams > 2))
                {
                    var mX = s.MapPosition.X;
                    var mY = MirrorDimension(yDim, s.MapPosition.Y);

                    var s2 = map.Sectors.FirstOrDefault(_ => _.MapPosition.X == mX && _.MapPosition.Y == mY);
                    if (s2 != null) s2.StartingSectorTeam = ++teamsAdded;
                }
            }

            // Final Checks
            if (teamsAdded < numTeams || !map.IsValid())
                return GenerateMirroredMap(size);

            return map;
        }
    }
}
