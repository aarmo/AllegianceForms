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
        public const string RandomMapName_Small = "Random-Small2";
        public const string RandomMapName_Normal = "Random2";
        public const string RandomMapName_Large = "Random-Large2";

        private static string[][] GetMapFiles()
        {
            var files = new string[3][];

            if (!Directory.Exists(StrategyGame.MapFolder))
                return files;

            var presetFiles = Directory.GetFiles(StrategyGame.MapFolder, "*.map");
            var filenames = (from f in presetFiles
                             select f.Substring(f.LastIndexOf("\\") + 1).Replace(".map", string.Empty)).ToList();

            filenames.Add(RandomMapName_Normal);
            filenames.Add(RandomMapName_Small);
            filenames.Add(RandomMapName_Large);

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
            SimpleGameMap map;
            switch (name)
            {
                case RandomMapName_Small:
                    map = RandomMap.GenerateSimpleMap(EMapSize.Small);
                    break;
                case RandomMapName_Normal:
                    map = RandomMap.GenerateSimpleMap(EMapSize.Normal);
                    break;
                case RandomMapName_Large:
                    map = RandomMap.GenerateSimpleMap(EMapSize.Large);
                    break;
                default:
                    map = Utils.DeserialiseFromFile<SimpleGameMap>(StrategyGame.MapFolder + "\\" + name + ".map");
                    break;
            }

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
