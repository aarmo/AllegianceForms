using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Orders
{
    public class StopOrder : ShipOrder
    {
        public StopOrder() : base(-1)
        {
        }

        public override void Update(Ship ship)
        {
            ship.Stop();
            OrderComplete = true;
        }
    }
}
