using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using System.Linq;

namespace AllegianceForms.Orders
{
    public class HuntControlOrder : ShipOrder
    {
        private EShipType[] _targetTypes;

        public HuntControlOrder(StrategyGame game, EShipType[] targetTypes) : base(game, -1)
        {
            _targetTypes = targetTypes;
        }

        public override void Update(Ship ship)
        {
            OrderComplete = true;
            
            // Atack any targets in this sector
            var targets = _game.AllUnits.Where(_ => _.Active && _.IsVisibleToTeam(ship.Team - 1) && _.SectorId == ship.SectorId && _.Alliance != ship.Alliance && _targetTypes.Contains(_.Type)).ToList();
            if (targets.Count() > 0)
            {
                var target = StrategyGame.RandomItem(targets);
                ship.OrderShip(new InterceptOrder(_game, target, ship.SectorId, true));
            }
            else
            {
                // Otherwise, continue the hunt in another sector!
                var visibleSectors = _game.Map.Sectors.Where(_ => _.IsVisibleToTeam(ship.Team - 1)).ToList();
                if (visibleSectors.Count < 2) return;

                var randomSectorId = StrategyGame.RandomItem(visibleSectors).Id;

                ship.OrderShip(new NavigateOrder(_game, ship, randomSectorId), true);
                ship.OrderShip(new HuntControlOrder(_game, _targetTypes), true);
            }
        }
    }
}
