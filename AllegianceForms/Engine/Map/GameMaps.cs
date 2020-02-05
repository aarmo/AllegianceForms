using AllegianceForms.Engine.Generation;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.Map
{
    public static class GameMaps
    {
        public static RandomString SectorNames = new RandomString(".\\Data\\SectorNames.txt");
        private static string[][] _allAvailableMaps = GetMapFiles(true);
        private static string[][] _availableMaps = GetMapFiles(false);

        public const int SectorHalfRadius = 5;
        public const int SectorRadius = 10;
        public const int SectorDiameter = 20;
        public const int SectorSpacing = 30;
        public const int MapPadding = 10;
        public const string RandomMapName_Small = "Random-Small2";
        public const string RandomMapName_Normal = "Random2";
        public const string RandomMapName_Large = "Random-Large2";

        private static string[][] GetMapFiles(bool allMaps)
        {
            var files = new string[3][];

            if (!Directory.Exists(StrategyGame.MapFolder))
                return files;

            var presetFiles = Directory.GetFiles(StrategyGame.MapFolder, "*.map");
            var filenames = (from f in presetFiles
                             select f.Substring(f.LastIndexOf("\\") + 1).Replace(".map", string.Empty)).ToList();

            if (allMaps)
            {
                filenames.Add(RandomMapName_Normal);
                filenames.Add(RandomMapName_Small);
                filenames.Add(RandomMapName_Large);
            }
            if (!allMaps)
            {
                filenames.RemoveAll(f => f.StartsWith("Brawl"));
            }

            files[0] = (from f in filenames
                        where f.EndsWith("2") || f.EndsWith("4")
                        select f).ToArray();
            files[1] = filenames.Where(_ => _.EndsWith("3")).ToArray();
            files[2] = filenames.Where(_ => _.EndsWith("4")).ToArray();

            return files;
        }

        public static string[] AvailableMaps(int teams, bool allMaps = true)
        {
            if (teams < 2 || teams > 4)
                return new[] { "" };

            return (allMaps ? _allAvailableMaps[teams - 2] : _availableMaps[teams - 2]);
        }
        
        public static string RandomName(int team, bool allMaps = true)
        {
            var maps = AvailableMaps(team, allMaps);
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
