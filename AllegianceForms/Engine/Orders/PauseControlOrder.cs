using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Orders
{
    public class PauseControlOrder : ShipOrder
    {
        public PauseControlOrder(StrategyGame game) : base(game, -1)
        {
        }

        public override void Cancel(Ship ship)
        {
            OrderComplete = true;
        }

        public override void Update(Ship ship)
        {
            // Do nothing - hold the queue
            ;
        }
    }
}
