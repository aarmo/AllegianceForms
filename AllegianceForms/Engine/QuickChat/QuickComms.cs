using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AllegianceForms.Engine.Map;
using System;
using AllegianceForms.Orders;
using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Engine.QuickChat
{
    public class QuickComms
    {
        public List<QuickChatItem> QuickItems { get; set; }

        public static QuickComms LoadQuickChat(string dataFile)
        {
            var cfg = new CsvConfiguration()
            {
                WillThrowOnMissingField = false,
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

        public static void ProcessOrder(StrategyGame game, QuickChatItem cmd, MapSector sector)
        {
            // TODO: Complete!
            return;

            if (cmd == null || game == null || cmd.OrderAction == string.Empty) return;

            var order = cmd.OrderAction;
            var team = 1;
            var alliance = game.GameSettings.TeamAlliance[0];
            
            if (order.StartsWith("Attack"))
            {
                // All friendly combat ships in sector
                var ships = game.AllUnits.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.Type != EShipType.Lifepod && _.CanAttackShips()).ToList();
                if (ships.Count == 0) return;

                // if type in sector, attack it (rnd)
                var targetTypes = GetOrderTypes(order);
                var targets = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(team) && _.Alliance != alliance && _.SectorId == sector.Id && targetTypes.Contains(_.Type)).ToList();

                var append = false;
                Ship target = null;

                // if type spotted, navigate to it (rnd)
                if (targets.Count == 0)
                {
                    targets = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(team) && _.Alliance != alliance && targetTypes.Contains(_.Type)).ToList();
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

                // if type in sector, defend it (rnd)
                var targetTypes = GetOrderTypes(order);
                var targets = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(team) && _.Alliance == alliance && _.SectorId == sector.Id && targetTypes.Contains(_.Type)).ToList();

                if (targets.Count == 0) return;
                var target = StrategyGame.RandomItem(targets);
                ships.ForEach(_ => _.OrderShip(new InterceptOrder(game, target, sector.Id, true)));
            }
            else if (order.StartsWith("Launch"))
            {
                // Get a base in sector (rnd)
                var bases = game.AllBases.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.CanLaunchShips()).ToList();
                if (bases.Count == 0) return;
                
                // Get a base close to this sector (rnd)
                // Launch this ship type
                
            }
            else if (order.StartsWith("Hunt"))
            {
                // Get a base in sector (rnd)
                var bases = game.AllBases.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.CanLaunchShips()).ToList();
                if (bases.Count == 0) return;

                var targetTypes = GetOrderTypes(order);
                var targets = game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(team) && _.Alliance != alliance && targetTypes.Contains(_.Type)).ToList();
                
                // Get a base close to this sector (rnd)
                // Launch fighter ship type

                // If type spotted, navigate & attack it (rnd)
                // Otherwise patrol randomly until spotted
            }
            else if (order == "Scout")
            {
                // Launch up to min(50%, 3) scouts to patrol randomly
                var bases = game.AllBases.Where(_ => _.Active && _.Team == team && _.SectorId == sector.Id && _.CanLaunchShips()).ToList();

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
