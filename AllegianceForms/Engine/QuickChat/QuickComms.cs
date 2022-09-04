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
            if (cmd == null || game == null || cmd.OrderAction == string.Empty) return;

            var targetSectorId = sector.Id;
            var launchSectorId = sector.Id;

            var team = 1;
            var t = 0;
            var alliance = game.GameSettings.TeamAlliance[t];
            var pilotCount = game.DockedPilots[t];
            var order = cmd.OrderAction;
            List<Ship> ships;

            if (order == "Scout")
            {
                var idealNumOfShips = 3;
                var orderTypes = GetOrderTypes(order);

                // Prefer ships already in sector
                ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == launchSectorId && orderTypes.Contains(_.Type)).Take(idealNumOfShips).ToList();

                if (ships.Count < idealNumOfShips && pilotCount > 0)
                {
                    // Launch more scouts if needed
                    var launchBase = game.ClosestSectorWithBase(team, sector.Id);
                    if (launchBase != null)
                    {
                        ships.AddRange(LaunchShips(game, idealNumOfShips - ships.Count, orderTypes, launchBase, f_ShipEvent));
                    }
                }
                if (ships.Count == 0) return;

                ships.ForEach(_ => _.OrderShip(new ScoutControlOrder(game), true));
            }
            else if (order == "Dock")
            {
                // All our combat ships in this sector should dock immediately
                ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.Type != EShipType.Lifepod && _.Type != EShipType.CarrierDrone && _.CanAttackShips() && !Ship.IsCapitalShip(_.Type)).ToList();
                if (ships.Count == 0) return;

                ships.ForEach(_ => _.OrderShip(new DockOrder(game, _)));
            }
            else if (order == "Pause")
            {
                // Interrupt the current order for all our combat ships in this sector
                ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.Type != EShipType.Lifepod && _.Type != EShipType.CarrierDrone && _.CanAttackShips()).ToList();
                if (ships.Count == 0) return;

                ships.ForEach(_ => _.InsertOrder(new PauseControlOrder(game)));
            }
            else if (order == "Resume")
            {
                // Resume the order queue for all our combat ships in this sector
                ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.Type != EShipType.Lifepod && _.Type != EShipType.CarrierDrone && _.CanAttackShips()).ToList();
                if (ships.Count == 0) return;

                ships.ForEach(_ => _.InsertOrder(new ResumeControlOrder(game)));
            }
            else if (order.StartsWith("Launch"))
            {
                if (pilotCount < 2) return;
                var orderTypes = GetOrderTypes(order);
                var idealNumOfShips = pilotCount / 2;

                var launchBase = game.ClosestSectorWithBase(team, sector.Id);
                if (launchBase == null) return;
                launchSectorId = launchBase.SectorId;

                ships = LaunchShips(game, idealNumOfShips, orderTypes, launchBase, f_ShipEvent);
                if (ships.Count == 0) return;

                if (sector.Id != launchSectorId)
                {
                    ships.ForEach(_ => _.OrderShip(new NavigateOrder(game, _, sector.Id)));
                    ships.ForEach(_ => _.OrderShip(new MoveOrder(game, launchBase.SectorId, StrategyGame.ScreenCenter, Point.Empty), true));
                }
            }
            else if (order.StartsWith("Defend") || order.StartsWith("Attack") || order.StartsWith("Hunt"))
            {
                var idealNumOfScouts = 2;
                var idealNumOfShips = game.TotalPilots[t] / 2;
                var shipTypes = GetOrderTypes("Fighter");
                var targetTypes = GetOrderTypes(order);

                Base targetBase = null;
                Ship targetShip = null;

                var defend = order.StartsWith("Defend");
                var hunt = order.StartsWith("Hunt");

                if (order.EndsWith("Base"))
                {
                    idealNumOfScouts = 0;

                    // Only bases in this sector
                    var targetBases = game.AllBases.Where(_ => _.Active && _.IsVisibleToTeam(t) && (_.Alliance != alliance || defend) && _.SectorId == sector.Id).ToList();                    
                    if (targetBase == null) return;
                    targetSectorId = targetBase.SectorId;
                }
                else
                { 
                    // Prefer targest in this sector
                    var targetShips = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(t) && (_.Alliance != alliance || defend) && _.SectorId == sector.Id && targetTypes.Contains(_.Type)).ToList();
                    if (targetShips.Count == 0)
                    {
                        targetShips = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(t) && (_.Alliance != alliance || defend) && targetTypes.Contains(_.Type)).ToList();
                    }

                    targetShip = StrategyGame.RandomItem(targetShips);

                    // Abort if we have no targets and we are not hunting
                    if (targetShip == null && !hunt) return;

                    targetSectorId = targetShip != null ? targetShip.SectorId : sector.Id;
                }

                var launchBase = game.ClosestSectorWithBase(team, targetSectorId);
                if (launchBase != null) launchSectorId = launchBase.SectorId;

                // Prefer scouts already in sector
                if (idealNumOfScouts > 0)
                {
                    ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == targetSectorId && _.Type == EShipType.Scout).Take(idealNumOfScouts).ToList();

                    if (ships.Count < idealNumOfScouts && pilotCount > 0)
                    {
                        // Launch more scouts if needed
                        if (launchBase != null)
                        {
                            ships.AddRange(LaunchShips(game, idealNumOfScouts - ships.Count, new[] { EShipType.Scout }, launchBase, f_ShipEvent));
                        }
                    }
                }
                else
                {
                    ships = new List<Ship>();
                }

                // Prefer fighters already in sector
                ships.AddRange(game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == targetSectorId && shipTypes.Contains(_.Type)).Take(idealNumOfShips).ToList());
                if (ships.Count < idealNumOfShips && pilotCount > 0)
                {
                    // Launch more fighters if needed
                    if (launchBase != null)
                    {
                        ships.AddRange(LaunchShips(game, idealNumOfShips - ships.Count, shipTypes, launchBase, f_ShipEvent));
                    }
                }
                if (ships.Count == 0) return;

                // Get to the target's sector, if needed
                ships.ForEach(_ => _.OrderShip(new NavigateOrder(game, _, targetSectorId)));

                if (targetBase != null)
                {
                    ships.ForEach(_ => _.OrderShip(new SurroundOrder(game, targetSectorId, targetBase), true));
                }
                else if (targetShip != null)
                {
                    ships.ForEach(_ => _.OrderShip(new InterceptOrder(game, targetShip, targetSectorId), true));
                }
                else if (hunt)
                { 
                    // Otherwise, patrol randomly to hunt for the target type
                    ships.ForEach(_ => _.OrderShip(new HuntControlOrder(game, targetTypes)));
                }
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

        private static List<Ship> LaunchShips(StrategyGame game, int numShips, EShipType[] shipTypes, Base launchBase, Ship.ShipEventHandler f_ShipEvent)
        {
            var ships = new List<Ship>();
            var t = launchBase.Team-1;

            do
            {
                var s = LaunchShipType(game, shipTypes, launchBase, f_ShipEvent);
                if (s == null) break;
                ships.Add(s);
            } while (game.DockedPilots[t] > 0 && ships.Count < numShips);

            return ships;
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

                case "Capital":
                    return new[] { EShipType.Corvette, EShipType.Destroyer, EShipType.Frigate, EShipType.Devastator, EShipType.Cruiser, EShipType.Battleship, EShipType.Battlecruiser, EShipType.Support, EShipType.AdvancedSupport, EShipType.HeavySupport, EShipType.SupportCarrier, EShipType.SuperCarrier, EShipType.Megalodon};

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
