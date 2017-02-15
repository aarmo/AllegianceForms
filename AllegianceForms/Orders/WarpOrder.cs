using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Orders
{
    public class WarpOrder : MoveOrder
    {
        protected Ship _ship;
        private GameEntity _otherEnd;

        public WarpOrder(Ship ship, Wormhole w) : base(ship.SectorId)
        {
            OrderPen.Color = Color.CornflowerBlue;
            _ship = ship;

            var e = (w.End1.SectorId == ship.SectorId) ? w.End1 : w.End2;
            _otherEnd = (w.End1.SectorId != ship.SectorId) ? w.End1 : w.End2;

            OrderPosition = e.CenterPoint;
        }

        public override void Update(Ship ship)
        {
            base.Update(ship);

            if (OrderComplete)
            {
                _ship.SectorId = _otherEnd.SectorId;
                _ship.CenterX = _otherEnd.CenterX;
                _ship.CenterY = _otherEnd.CenterY;
                _ship.Orders.Clear();
            }
        }
    }
}
