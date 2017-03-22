using AllegianceForms.Engine.Generation;
using System.Collections.Generic;
using System.Drawing;

namespace AllegianceForms.Engine.Map
{
    public static class GameMaps
    {
        public static RandomString SectorNames = new RandomString(".\\Data\\Names-Sector.txt");

        public const int SectorHalfRadius = 5;
        public const int SectorRadius = 10;
        public const int SectorDiameter = 20;
        public const int SectorSpacing = 30;

        public static string[] AvailableMaps(int teams)
        {
            if (teams == 2) return new[] { "Grid", "HiHigher", "HiLo", "PinWheel", "DoubleRing", "SingleRing", "Star", "Brawl" };
            if (teams == 3 || teams == 4) return new[] { "PinWheel", "DoubleRing", "SingleRing"};

            return new[] { "" };
        }
        
        public static string RandomName(int team)
        {
            var maps = AvailableMaps(team);
            return maps[StrategyGame.Random.Next(maps.Length)];
        }

        public static GameMap LoadMap(string name, int teams = 2)
        {
            switch (name)
            {
                case "Grid": return Grid();
                case "HiHigher": return HiHigher();
                case "HiLo": return HiLo();
                case "PinWheel": return PinWheel(teams);
                case "DoubleRing": return DoubleRing(teams);
                case "SingleRing": return SingleRing(teams);
                case "Star": return Star();
            }

            return Brawl();
        }

        public static GameMap Brawl()
        {
            var map = new GameMap
            {
                Name = "Brawl",
                Sectors = new List<MapSector> { new MapSector(0, SectorNames.NextString, new Point(0, 0)) { StartingSector = true } },
                Wormholes = new List<Wormhole>()
            };
            map.InitialiseMap();
            return map;
        }

        public static GameMap DoubleRing(int teams)
        {
            if (teams == 3) return DoubleRing3();
            if (teams == 4) return DoubleRing4();

            var map = new GameMap
            {
                Name = "DoubleRing",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 1)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(1, 0)),
                    new MapSector (2, SectorNames.NextString, new Point(2, 1)) { StartingSector = true },
                    new MapSector (3, SectorNames.NextString, new Point(1, 2)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[2]),
                new Wormhole (map.Sectors[0], map.Sectors[3]),

                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[1], map.Sectors[3]),

                new Wormhole (map.Sectors[2], map.Sectors[3]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap DoubleRing3()
        {

            var map = new GameMap
            {
                Name = "DoubleRing",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 0)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(2, 1)),
                    new MapSector (2, SectorNames.NextString, new Point(4, 1)) { StartingSector = true },
                    new MapSector (3, SectorNames.NextString, new Point(2, 2)),
                    new MapSector (4, SectorNames.NextString, new Point(1, 4)) { StartingSector = true },
                    new MapSector (5, SectorNames.NextString, new Point(1, 2)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[2]),
                new Wormhole (map.Sectors[0], map.Sectors[4]),
                new Wormhole (map.Sectors[0], map.Sectors[5]),

                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[1], map.Sectors[3]),
                new Wormhole (map.Sectors[1], map.Sectors[5]),

                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[4]),

                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[3], map.Sectors[5]),

                new Wormhole (map.Sectors[4], map.Sectors[5]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap DoubleRing4()
        {

            var map = new GameMap
            {
                Name = "DoubleRing",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(3, 0)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(4, 2)),
                    new MapSector (2, SectorNames.NextString, new Point(6, 3)) { StartingSector = true },
                    new MapSector (3, SectorNames.NextString, new Point(4, 4)),
                    new MapSector (4, SectorNames.NextString, new Point(3, 6)) { StartingSector = true },
                    new MapSector (5, SectorNames.NextString, new Point(2, 4)),
                    new MapSector (6, SectorNames.NextString, new Point(0, 3)) { StartingSector = true },
                    new MapSector (7, SectorNames.NextString, new Point(2, 2)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[2]),
                new Wormhole (map.Sectors[0], map.Sectors[6]),
                new Wormhole (map.Sectors[0], map.Sectors[7]),

                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[1], map.Sectors[3]),
                new Wormhole (map.Sectors[1], map.Sectors[7]),

                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[4]),

                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[3], map.Sectors[5]),

                new Wormhole (map.Sectors[4], map.Sectors[5]),
                new Wormhole (map.Sectors[4], map.Sectors[6]),

                new Wormhole (map.Sectors[5], map.Sectors[6]),
                new Wormhole (map.Sectors[5], map.Sectors[7]),

                new Wormhole (map.Sectors[6], map.Sectors[7]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap SingleRing(int teams)
        {
            if (teams == 3) return SingleRing3();
            if (teams == 4) return SingleRing4();

            var map = new GameMap
            {
                Name = "SingleRing",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 1)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(1, 0)),
                    new MapSector (2, SectorNames.NextString, new Point(2, 1)) { StartingSector = true },
                    new MapSector (3, SectorNames.NextString, new Point(1, 2)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[3]),
                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[2], map.Sectors[3]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap SingleRing3()
        {
            var map = new GameMap
            {
                Name = "SingleRing",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 0)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(1, 1)),
                    new MapSector (2, SectorNames.NextString, new Point(2, 2)) { StartingSector = true },
                    new MapSector (3, SectorNames.NextString, new Point(1, 3)),
                    new MapSector (4, SectorNames.NextString, new Point(0, 4)) { StartingSector = true },
                    new MapSector (5, SectorNames.NextString, new Point(0, 2)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[5]),
                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[4], map.Sectors[5]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap SingleRing4()
        {
            var map = new GameMap
            {
                Name = "SingleRing",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(2, 0)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(3, 1)),
                    new MapSector (2, SectorNames.NextString, new Point(4, 2)) { StartingSector = true },
                    new MapSector (3, SectorNames.NextString, new Point(3, 3)),
                    new MapSector (4, SectorNames.NextString, new Point(2, 4)) { StartingSector = true },
                    new MapSector (5, SectorNames.NextString, new Point(1, 3)),
                    new MapSector (6, SectorNames.NextString, new Point(0, 2)) { StartingSector = true },
                    new MapSector (7, SectorNames.NextString, new Point(1, 1)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[7]),
                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[1], map.Sectors[5]),
                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[3], map.Sectors[7]),
                new Wormhole (map.Sectors[4], map.Sectors[5]),
                new Wormhole (map.Sectors[5], map.Sectors[6]),
                new Wormhole (map.Sectors[6], map.Sectors[7]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap Star()
        {
            var map = new GameMap
            {
                Name = "Star",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 1)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(1, 0)),
                    new MapSector (2, SectorNames.NextString, new Point(3, 0)),
                    new MapSector (3, SectorNames.NextString, new Point(5, 0)),
                    new MapSector (4, SectorNames.NextString, new Point(6, 1)) { StartingSector = true },

                    new MapSector (5, SectorNames.NextString, new Point(1, 1)),
                    new MapSector (6, SectorNames.NextString, new Point(2, 1)),
                    new MapSector (7, SectorNames.NextString, new Point(4, 1)),
                    new MapSector (8, SectorNames.NextString, new Point(5, 1)),

                    new MapSector (9, SectorNames.NextString, new Point(1, 2)),
                    new MapSector (10, SectorNames.NextString, new Point(3, 2)),
                    new MapSector (11, SectorNames.NextString, new Point(5, 2)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[9]),
                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[1], map.Sectors[5]),
                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[5]),
                new Wormhole (map.Sectors[2], map.Sectors[8]),
                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[3], map.Sectors[8]),
                new Wormhole (map.Sectors[4], map.Sectors[11]),
                new Wormhole (map.Sectors[5], map.Sectors[6]),
                new Wormhole (map.Sectors[5], map.Sectors[9]),
                new Wormhole (map.Sectors[5], map.Sectors[10]),
                new Wormhole (map.Sectors[6], map.Sectors[7]),
                new Wormhole (map.Sectors[7], map.Sectors[8]),
                new Wormhole (map.Sectors[8], map.Sectors[10]),
                new Wormhole (map.Sectors[8], map.Sectors[11]),
                new Wormhole (map.Sectors[9], map.Sectors[10]),
                new Wormhole (map.Sectors[10], map.Sectors[11]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap Grid()
        {
            var map = new GameMap
            {
                Name = "Grid",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 0)),
                    new MapSector (1, SectorNames.NextString, new Point(6, 0)),
                    new MapSector (2, SectorNames.NextString, new Point(2, 1)),
                    new MapSector (3, SectorNames.NextString, new Point(4, 1)),
                    new MapSector (4, SectorNames.NextString, new Point(1, 2)) { StartingSector = true },
                    new MapSector (5, SectorNames.NextString, new Point(5, 2)) { StartingSector = true },
                    new MapSector (6, SectorNames.NextString, new Point(2, 3)),
                    new MapSector (7, SectorNames.NextString, new Point(4, 3)),
                    new MapSector (8, SectorNames.NextString, new Point(0, 4)),
                    new MapSector (9, SectorNames.NextString, new Point(6, 4)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[2]),
                new Wormhole (map.Sectors[0], map.Sectors[4]),
                new Wormhole (map.Sectors[0], map.Sectors[8]),

                new Wormhole (map.Sectors[1], map.Sectors[3]),
                new Wormhole (map.Sectors[1], map.Sectors[5]),
                new Wormhole (map.Sectors[1], map.Sectors[9]),
                
                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[4]),
                new Wormhole (map.Sectors[2], map.Sectors[6]),

                new Wormhole (map.Sectors[3], map.Sectors[5]),
                new Wormhole (map.Sectors[3], map.Sectors[7]),

                new Wormhole (map.Sectors[4], map.Sectors[8]),
                new Wormhole (map.Sectors[4], map.Sectors[6]),

                new Wormhole (map.Sectors[5], map.Sectors[7]),
                new Wormhole (map.Sectors[5], map.Sectors[9]),

                new Wormhole (map.Sectors[6], map.Sectors[7]),
                new Wormhole (map.Sectors[6], map.Sectors[8]),

                new Wormhole (map.Sectors[7], map.Sectors[9]),

                new Wormhole (map.Sectors[8], map.Sectors[9]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap HiHigher()
        {
            var map = new GameMap
            {
                Name = "HiHigher",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 2)) {StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(1, 0)),
                    new MapSector (2, SectorNames.NextString, new Point(3, 0)),
                    new MapSector (3, SectorNames.NextString, new Point(4, 2)) {StartingSector = true },
                    new MapSector (4, SectorNames.NextString, new Point(1, 1)),
                    new MapSector (5, SectorNames.NextString, new Point(2, 1)),
                    new MapSector (6, SectorNames.NextString, new Point(3, 1)),
                    new MapSector (7, SectorNames.NextString, new Point(2, 2)),
                    new MapSector (8, SectorNames.NextString, new Point(1, 3)),
                    new MapSector (9, SectorNames.NextString, new Point(2, 3)),
                    new MapSector (10, SectorNames.NextString, new Point(3, 3)),
                    new MapSector (11, SectorNames.NextString, new Point(1, 4)),
                    new MapSector (12, SectorNames.NextString, new Point(3, 4)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[4]),
                new Wormhole (map.Sectors[0], map.Sectors[8]),
                new Wormhole (map.Sectors[0], map.Sectors[11]),
                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[1], map.Sectors[4]),
                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[6]),
                new Wormhole (map.Sectors[3], map.Sectors[6]),
                new Wormhole (map.Sectors[3], map.Sectors[10]),
                new Wormhole (map.Sectors[3], map.Sectors[12]),
                new Wormhole (map.Sectors[4], map.Sectors[5]),
                new Wormhole (map.Sectors[5], map.Sectors[6]),
                new Wormhole (map.Sectors[5], map.Sectors[7]),
                new Wormhole (map.Sectors[7], map.Sectors[9]),
                new Wormhole (map.Sectors[8], map.Sectors[9]),
                new Wormhole (map.Sectors[8], map.Sectors[11]),
                new Wormhole (map.Sectors[9], map.Sectors[10]),
                new Wormhole (map.Sectors[10], map.Sectors[12]),
                new Wormhole (map.Sectors[11], map.Sectors[12]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap HiLo()
        {
            var map = new GameMap
            {
                Name = "HiLo",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 0)),
                    new MapSector (1, SectorNames.NextString, new Point(2, 0)),
                    new MapSector (2, SectorNames.NextString, new Point(0, 2)) {StartingSector = true },
                    new MapSector (3, SectorNames.NextString, new Point(1, 1)),
                    new MapSector (4, SectorNames.NextString, new Point(2, 2)) {StartingSector = true },
                    new MapSector (5, SectorNames.NextString, new Point(1, 2)),
                    new MapSector (6, SectorNames.NextString, new Point(1, 3)),
                    new MapSector (7, SectorNames.NextString, new Point(0, 4)),
                    new MapSector (8, SectorNames.NextString, new Point(2, 4)),
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[0], map.Sectors[2]),
                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[6]),
                new Wormhole (map.Sectors[2], map.Sectors[7]),
                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[3], map.Sectors[5]),
                new Wormhole (map.Sectors[4], map.Sectors[1]),
                new Wormhole (map.Sectors[4], map.Sectors[6]),
                new Wormhole (map.Sectors[4], map.Sectors[8]),
                new Wormhole (map.Sectors[5], map.Sectors[6]),
                new Wormhole (map.Sectors[7], map.Sectors[8]),
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap PinWheel(int teams)
        {
            if (teams == 3) return PinWheel3();
            if (teams == 4) return PinWheel4();

            var map = new GameMap
            {
                Name = "PinWheel",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 0)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(1, 1)),
                    new MapSector (2, SectorNames.NextString, new Point(2, 2)),
                    new MapSector (3, SectorNames.NextString, new Point(3, 1)),
                    new MapSector (4, SectorNames.NextString, new Point(4, 0)) { StartingSector = true },
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[1], map.Sectors[3])
            };

            map.InitialiseMap();

            return map;
        }
        
        public static GameMap PinWheel3()
        {
            var map = new GameMap
            {
                Name = "PinWheel",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 0)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(1, 1)),
                    new MapSector (2, SectorNames.NextString, new Point(2, 2)),
                    new MapSector (3, SectorNames.NextString, new Point(3, 1)),
                    new MapSector (4, SectorNames.NextString, new Point(4, 0)) { StartingSector = true },
                    new MapSector (5, SectorNames.NextString, new Point(3, 3)),
                    new MapSector (6, SectorNames.NextString, new Point(4, 4)) { StartingSector = true },
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[1], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[5]),
                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[3], map.Sectors[5]),
                new Wormhole (map.Sectors[5], map.Sectors[6])
            };

            map.InitialiseMap();

            return map;
        }

        public static GameMap PinWheel4()
        {
            var map = new GameMap
            {
                Name = "PinWheel",
                Sectors = new List<MapSector> {
                    new MapSector (0, SectorNames.NextString, new Point(0, 0)) { StartingSector = true },
                    new MapSector (1, SectorNames.NextString, new Point(1, 1)),
                    new MapSector (2, SectorNames.NextString, new Point(2, 2)),
                    new MapSector (3, SectorNames.NextString, new Point(3, 1)),
                    new MapSector (4, SectorNames.NextString, new Point(4, 0)) { StartingSector = true },
                    new MapSector (5, SectorNames.NextString, new Point(3, 3)),
                    new MapSector (6, SectorNames.NextString, new Point(4, 4)) { StartingSector = true },
                    new MapSector (7, SectorNames.NextString, new Point(1, 3)),
                    new MapSector (8, SectorNames.NextString, new Point(0, 4)) { StartingSector = true },
                }
            };

            map.Wormholes = new List<Wormhole> {
                new Wormhole (map.Sectors[0], map.Sectors[1]),
                new Wormhole (map.Sectors[1], map.Sectors[2]),
                new Wormhole (map.Sectors[1], map.Sectors[3]),
                new Wormhole (map.Sectors[1], map.Sectors[7]),

                new Wormhole (map.Sectors[2], map.Sectors[3]),
                new Wormhole (map.Sectors[2], map.Sectors[5]),
                new Wormhole (map.Sectors[2], map.Sectors[7]),

                new Wormhole (map.Sectors[3], map.Sectors[4]),
                new Wormhole (map.Sectors[3], map.Sectors[5]),

                new Wormhole (map.Sectors[5], map.Sectors[6]),
                new Wormhole (map.Sectors[5], map.Sectors[7]),

                new Wormhole (map.Sectors[7], map.Sectors[8]),

            };

            map.InitialiseMap();

            return map;
        }
    }
}
