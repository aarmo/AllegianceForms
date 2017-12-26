using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Orders
{
    public class ScoutControlOrder : ShipOrder
    {
        public ScoutControlOrder(StrategyGame game) : base(game, -1)
        {
        }

        public override void Update(Ship ship)
        {
            OrderComplete = true;

            var visibleSectors = _game.Map.Sectors.Where(_ => _.IsVisibleToTeam(ship.Team - 1)).ToList();
            if (visibleSectors.Count < 2) return;

            // If we have stopped, keep scouting
            var randomSectorId = StrategyGame.RandomItem(visibleSectors).Id;
            ship.OrderShip(new NavigateOrder(_game, ship, randomSectorId), true);
            ship.OrderShip(new MoveOrder(_game, randomSectorId, StrategyGame.ScreenCenter, PointF.Empty), true);
            ship.OrderShip(new ScoutControlOrder(_game), true);
        }
    }
}
