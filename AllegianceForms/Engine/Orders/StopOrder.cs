using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Orders
{
    public class StopOrder : ShipOrder
    {
        public StopOrder(StrategyGame game) : base(game, -1)
        {
        }

        public override void Update(Ship ship)
        {
            ship.Stop();
            OrderComplete = true;
        }
    }
}
