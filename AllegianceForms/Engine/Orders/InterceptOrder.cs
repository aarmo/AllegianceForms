using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Orders
{
    public class InterceptOrder : MoveOrder
    {
        private Ship _target;
        public InterceptOrder(StrategyGame game, Ship targetShip, int sectorId) : base(game, sectorId)
        {
            OrderPen.Color = Color.LightGray;
            _target = targetShip;
            OrderPosition = _target.CenterPoint;
        }

        public override void Update(Ship ship)
        {
            if (_target != null && _target.Active && _target.SectorId == ship.SectorId)
            {
                OrderPosition = _target.CenterPoint;
                OrderSectorId = _target.SectorId;
            }
            else
            {
                OrderComplete = true;
                return;
            }

            base.Update(ship);

            if (OrderComplete && ship.Type == EShipType.Lifepod)
            {
                ship.Dock(null);
            }
        }

        public override void Draw(Graphics g, PointF fromPos, int fromSectorId)
        {
            if (fromSectorId != _target.SectorId) return;

            OrderPosition = _target.CenterPoint;
            base.Draw(g, fromPos, fromSectorId);
        }
    }
}
