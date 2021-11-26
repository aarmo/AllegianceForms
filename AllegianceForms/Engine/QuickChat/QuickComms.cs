using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AllegianceForms.Engine.Map;
using AllegianceForms.Orders;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Engine.Bases;
using System.Drawing;

namespace AllegianceForms.Engine.QuickChat
{
    public class QuickComms
    {
        public List<QuickChatItem> QuickItems { get; set; }

        public static QuickComms LoadQuickChat(string dataFile)
        {
            var cfg = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true,
            };

            using (var textReader = File.OpenText(dataFile))
            {
                var csv = new CsvReader(textReader, cfg);

                return new QuickComms
                {
                     QuickItems = csv.GetRecords<QuickChatItem>().ToList()
                };
            }
        }

        public static void ProcessOrder(StrategyGame game, QuickChatItem cmd, MapSector sector, Ship.ShipEventHandler f_ShipEvent)
        {
            // TODO: Test!
            return;
            
            if (cmd == null || game == null || cmd.OrderAction == string.Empty) return;

            var order = cmd.OrderAction;
            var team = 1;
            var t = team - 1;
            var alliance = game.GameSettings.TeamAlliance[t];
            var pilotCount = game.DockedPilots[t];

            if (order.StartsWith("Attack"))
            {
                // All friendly combat ships in sector
                var ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.CanAttackShips()).ToList();
                if (ships.Count == 0) return;

                if (order.EndsWith("Base"))
                {
                    var targetBases = game.AllBases.Where(_ => _.Active && _.SectorId != sector.Id && _.Alliance != alliance).ToList();
                    if (targetBases.Count == 0) return;

                    var targetBase = StrategyGame.RandomItem(targetBases);
                    ships.ForEach(_ => _.OrderShip(new SurroundOrder(game, sector.Id, targetBase, PointF.Empty)));
                    return;
                }

                // if type in sector, attack it (rnd)
                var targetTypes = GetOrderTypes(order);
                var targets = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(t) && _.Alliance != alliance && _.SectorId == sector.Id && targetTypes.Contains(_.Type)).ToList();

                var append = false;
                Ship target = null;

                // if type spotted, navigate to it (rnd)
                if (targets.Count == 0)
                {
                    targets = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(t) && _.Alliance != alliance && targetTypes.Contains(_.Type)).ToList();
                    if (targets.Count == 0) return;

                    append = true;
                    target = StrategyGame.RandomItem(targets);
                    ships.ForEach(_ => _.OrderShip(new NavigateOrder(game, _, target.SectorId)));
                }
                if (target == null) target = StrategyGame.RandomItem(targets);

                ships.ForEach(_ => _.OrderShip(new InterceptOrder(game, target, sector.Id, true), append));
            }
            else if (order.StartsWith("Defend"))
            {
                // All friendly combat ships in sector
                var ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.Type != EShipType.Lifepod && _.CanAttackShips()).ToList();
                if (ships.Count == 0) return;

                if (order.EndsWith("Base"))
                {
                    var targetBases = game.AllBases.Where(_ => _.Active && _.SectorId != sector.Id && _.Alliance == alliance).ToList();
                    if (targetBases.Count == 0) return;

                    var targetBase = StrategyGame.RandomItem(targetBases);
                    ships.ForEach(_ => _.OrderShip(new SurroundOrder(game, sector.Id, targetBase, PointF.Empty)));
                    return;
                }

                // if type in sector, defend it (rnd)
                var targetTypes = GetOrderTypes(order);
                var targets = game.AllUnits.Where(_ => _.Active && _.Alliance == alliance && _.SectorId == sector.Id && targetTypes.Contains(_.Type)).ToList();

                if (targets.Count == 0) return;
                var target = StrategyGame.RandomItem(targets);
                ships.ForEach(_ => _.OrderShip(new InterceptOrder(game, target, sector.Id, true)));
            }
            else if (order.StartsWith("Launch"))
            {
                if (pilotCount == 0) return;

                // Get a base in sector (rnd)
                var bases = game.AllBases.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.CanLaunchShips()).ToList();
                Base launchBase = null;
                if (bases.Count == 0)
                {
                    // Get a base close to this sector (rnd)
                    launchBase = game.ClosestSectorWithBase(team, sector.Id);                    
                }
                else
                {
                    launchBase = StrategyGame.RandomItem(bases);
                }
                if (launchBase == null) return;

                // Launch this ship type
                var types = GetOrderTypes(order);
                do
                {
                    var s = LaunchShipType(game, types, launchBase, f_ShipEvent);
                    if (s == null) break;
                } while (game.DockedPilots[t] > pilotCount / 2);

            }
            else if (order.StartsWith("Hunt"))
            {
                // Get a base in sector (rnd)
                var bases = game.AllBases.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.CanLaunchShips()).ToList();
                Base launchBase = null;

                if (bases.Count == 0)
                {
                    // Get a base close to this sector (rnd)
                    launchBase = game.ClosestSectorWithBase(team, sector.Id);
                }
                else
                {
                    launchBase = StrategyGame.RandomItem(bases);
                }

                var targetTypes = GetOrderTypes(order);
                var targets = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(team) && _.Alliance != alliance && targetTypes.Contains(_.Type)).ToList();
                var target = StrategyGame.RandomItem(targets);

                // Launch fighter ship type
                var types = GetOrderTypes("Fighter");
                var ships = new List<CombatShip>();
                do
                {
                    var s = LaunchShipType(game, types, launchBase, f_ShipEvent);
                    if (s == null) break;
                    ships.Add(s);
                } while (game.DockedPilots[t] > pilotCount / 2);

                // If type spotted, navigate & attack it (rnd)
                if (target != null)
                {
                    if (launchBase.SectorId != target.SectorId)
                    {
                        ships.ForEach(_ => _.OrderShip(new NavigateOrder(game, _, target.SectorId), true));
                    }

                    ships.ForEach(_ => _.OrderShip(new InterceptOrder(game, target, _.SectorId, true), true));
                }
                else
                {
                    // Otherwise patrol randomly until spotted
                    ships.ForEach(_ => _.OrderShip(new HuntControlOrder(game, targetTypes), true));
                }
            }
            else if (order == "Scout")
            {
                // Launch up to 3 scouts to patrol randomly
                var bases = game.AllBases.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.CanLaunchShips()).ToList();
                if (bases.Count == 0) return;
                var launchBase = StrategyGame.RandomItem(bases);

                var types = GetOrderTypes("Scout");
                var ships = new List<CombatShip>();
                do
                {
                    var s = LaunchShipType(game, types, launchBase, f_ShipEvent);
                    if (s == null) break;
                    ships.Add(s);
                } while (game.DockedPilots[t] > 0 && ships.Count < 3);
                
                ships.ForEach(_ => _.OrderShip(new ScoutControlOrder(game), true));
            }
            else if (order == "Dock")
            {
                // All friendly combat ships in sector
                var ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.Type != EShipType.Lifepod && _.CanAttackShips()).ToList();
                if (ships.Count == 0) return;

                // Dock order
                ships.ForEach(_ => _.OrderShip(new DockOrder(game, _)));
            }
            else if (order == "Pause")
            {
                // All friendly combat ships in sector
                var ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.Type != EShipType.Lifepod && _.CanAttackShips()).ToList();
                if (ships.Count == 0) return;
                
                // Pause order
            }
            else if (order == "Resume")
            {
                // All friendly combat ships in sector
                var ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.Type != EShipType.Lifepod && _.CanAttackShips()).ToList();
                if (ships.Count == 0) return;
                
                // Unpause order
            }

        }

        private static CombatShip LaunchShipType(StrategyGame game, EShipType[] types, Base launchBase, Ship.ShipEventHandler f_ShipEvent)
        {
            var team = launchBase.Team;
            var colour = Color.FromArgb(game.GameSettings.TeamColours[team - 1]);
            var ship = game.Ships.CreateCombatShip(types, team, colour, launchBase.SectorId);
            if (ship == null) return null;
            if (!game.LaunchShip(ship)) return null;

            var pos = launchBase.GetNextBuildPosition();
            ship.CenterX = launchBase.CenterX;
            ship.CenterY = launchBase.CenterY;
            ship.ShipEvent += f_ShipEvent;
            ship.OrderShip(new MoveOrder(game, launchBase.SectorId, pos, Point.Empty));
            
            return ship;
        }

        private static EShipType[] GetOrderTypes(string order)
        {
            var type = order.Substring(order.IndexOf('_') + 1);
            
            switch(type)
            {
                case "Fighter":
                    return new[] { EShipType.Fighter, EShipType.Interceptor, EShipType.StealthFighter };

                case "Scout":
                    return new[] { EShipType.Scout };

                case "Bomber":
                    return new[] { EShipType.Bomber, EShipType.StealthBomber, EShipType.FighterBomber, EShipType.TroopTransport };

                case "Builder":
                    return new[] { EShipType.Constructor };

                case "Miner":
                    return new[] { EShipType.Miner };

                case "Tower":
                    return new[] { EShipType.Tower, EShipType.ShieldTower, EShipType.RepairTower, EShipType.MissileTower };
            }

            return new EShipType[0];
        }
    }

    public class QuickChatItem
    {
        public int MenuId { get; set; }
        public string QuickComms { get; set; }
        public string Key { get; set; }
        public string Filename { get; set; }
        public string OpenMenuId { get; set; }
        public string OrderAction { get; set; }
    }
}
