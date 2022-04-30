using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Orders
{
    public class ResumeControlOrder : ShipOrder
    {
        public ResumeControlOrder(StrategyGame game) : base(game, -1)
        {
        }

        public override void Update(Ship ship)
        {
            // Remove any pause orders
            if (ship.CurrentOrder != null && ship.CurrentOrder.GetType() == typeof(PauseControlOrder))
            {
                ship.CurrentOrder.Cancel(ship);
            }
            ship.Orders.RemoveAll(_ => _.GetType() == typeof(PauseControlOrder));

            OrderComplete = true;
        }
    }
}
