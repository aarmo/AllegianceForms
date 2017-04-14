using AllegianceForms.Engine.Generation;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.Map
{
    public static class GameMaps
    {
        public static RandomString SectorNames = new RandomString(".\\Data\\Names-Sector.txt");
        private static string[][] _availableMaps = GetMapFiles();

        public const int SectorHalfRadius = 5;
        public const int SectorRadius = 10;
        public const int SectorDiameter = 20;
        public const int SectorSpacing = 30;
        public const int MapPadding = 10;

        private static string[][] GetMapFiles()
        {
            var files = new string[3][];

            if (!Directory.Exists(StrategyGame.MapFolder))
                return files;

            var presetFiles = Directory.GetFiles(StrategyGame.MapFolder);
            var filenames = (from f in presetFiles
                             select f.Substring(f.LastIndexOf("\\") + 1).Replace(".map", string.Empty)).ToList();

            files[0] = filenames.Where(_ => _.EndsWith("2") || _.EndsWith("4")).ToArray();
            files[1] = filenames.Where(_ => _.EndsWith("3")).ToArray();
            files[2] = filenames.Where(_ => _.EndsWith("4")).ToArray();

            return files;
        }

        public static string[] AvailableMaps(int teams)
        {
            if (teams < 2 || teams > 4)
                return new[] { "" };

            return _availableMaps[teams - 2];
        }
        
        public static string RandomName(int team)
        {
            var maps = AvailableMaps(team);
            return maps[StrategyGame.Random.Next(maps.Length)];
        }

        public static GameMap LoadMap(StrategyGame game, string name)
        {
            var map = Utils.DeserialiseFromFile<SimpleGameMap>(StrategyGame.MapFolder + "\\" + name + ".map");
            if (map == null) return Brawl(game);

            return GameMap.FromSimpleMap(game, map);
        }

        public static GameMap Brawl(StrategyGame game)
        {
            var map = new GameMap(game)
            {
                Name = "Brawl",
                Sectors = new List<MapSector> { new MapSector(game, 0, SectorNames.NextString, new Point(0, 0)) { StartingSector = 1 } },
                Wormholes = new List<Wormhole>()
            };
            map.InitialiseMap();
            return map;
        }
    }
}
