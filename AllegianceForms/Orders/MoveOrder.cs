using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AllegianceForms.Orders
{
    public class MoveOrder : ShipOrder
    {
        protected Pen OrderPen = new Pen(Brushes.PaleVioletRed, 1);

        public MoveOrder(int sectorId) : this(sectorId, Point.Empty, Point.Empty)
        {
        }

        public MoveOrder(int sectorId, PointF targetPosition) : this(sectorId, targetPosition, Point.Empty)
        {
        }

        public MoveOrder(int sectorId, PointF targetPosition, PointF offset) : base(sectorId, targetPosition, offset)
        {
            OrderPen.DashStyle = DashStyle.Dash;
            OrderPen.DashCap = DashCap.Round;
        }

        public override void Update(Ship ship)
        {
            var shipBounds = ship.Bounds;

            if (OrderPosition.X < ship.Left)
            {
                ship.HorizontalDir = EHorDir.West;
            }
            else if (OrderPosition.X > ship.Left + shipBounds.Width)
            {
                ship.HorizontalDir = EHorDir.East;
            }
            else
            {
                ship.HorizontalDir = EHorDir.None;
            }

            if (OrderPosition.Y < ship.Top)
            {
                ship.VerticalDir = EVertDir.North;
            }
            else if (OrderPosition.Y > ship.Top + shipBounds.Height)
            {
                ship.VerticalDir = EVertDir.South;
            }
            else
            {
                ship.VerticalDir = EVertDir.None;
            }

            if (ship.VerticalDir == EVertDir.None
                && ship.HorizontalDir == EHorDir.None)
            {
                OrderComplete = true;
            }
        }
        public override void Draw(Graphics g, PointF fromPos, int fromSectorId)
        {
            if (OrderSectorId != fromSectorId) return;

            g.DrawLine(OrderPen, fromPos, OrderPosition);
        }
    }
}
