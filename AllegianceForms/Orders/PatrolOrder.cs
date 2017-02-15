using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Orders
{
    public class PatrolOrder : MoveOrder
    {
        private PointF _originalPosition = PointF.Empty;

        public PatrolOrder(int sectorId) : this(sectorId, PointF.Empty, PointF.Empty)
        {
        }

        public PatrolOrder(int sectorId, PointF targetPosition, PointF offset) : base(sectorId, targetPosition, offset)
        {
            OrderPen.Color = Color.SkyBlue;
        }

        public override void Update(Ship ship)
        {
            // Remember our current position when we start patrolling
            if (_originalPosition == Point.Empty)
            {
                _originalPosition = ship.CenterPoint;
            }

            base.Update(ship);

            // Keep repeating - back to our original position
            if (OrderComplete)
            {
                var nextPos = _originalPosition;
                _originalPosition = OrderPosition;
                OrderPosition = nextPos;

                OrderComplete = false;
            }
        }

        public override void Draw(Graphics g, PointF fromPos, int fromSectorId)
        {
            base.Draw(g, fromPos, fromSectorId);

            if (_originalPosition != Point.Empty && fromSectorId == OrderSectorId)
            {
                g.DrawLine(OrderPen, OrderPosition, _originalPosition);
                g.DrawLine(OrderPen, _originalPosition, fromPos);
            }
        }
    }
}
