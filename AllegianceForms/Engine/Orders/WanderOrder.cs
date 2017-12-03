using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Orders
{
    public class WanderOrder : MoveOrder
    {
        public WanderOrder(StrategyGame game, int sectorId) : this(game, sectorId, StrategyGame.RandomPosition(), PointF.Empty)
        {
        }

        public WanderOrder(StrategyGame game, int sectorId, PointF targetPosition, PointF offset) : base(game, sectorId, targetPosition, offset)
        {
            OrderPen.Color = Color.SkyBlue;
        }

        public override void Update(Ship ship)
        {
            base.Update(ship);

            // Keep repeating go to another random Pos
            if (OrderComplete)
            {
                OrderPosition = StrategyGame.RandomPosition();

                OrderComplete = false;
            }
        }
    }
}
