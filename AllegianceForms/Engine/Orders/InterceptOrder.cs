using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Orders
{
    public class InterceptOrder : MoveOrder
    {
        private Ship _target;
        private bool _changeTarget;

        public InterceptOrder(StrategyGame game, Ship targetShip, int sectorId, bool changeTarget = false) : base(game, sectorId)
        {
            OrderPen.Color = Color.LightGray;
            _target = targetShip;
            _changeTarget = changeTarget;
            OrderPosition = _target.CenterPoint;
        }

        public override void Update(Ship ship)
        {
            if (_target != null && _target.Active && _target.SectorId == ship.SectorId)
            {
                OrderPosition = _target.CenterPoint;
                OrderSectorId = _target.SectorId;
            }
            else if (_changeTarget)
            {
                var targets = _game.AllUnits.Where(_ => _.Active && _.SectorId == ship.SectorId && _.Alliance == _target.Alliance && _.Type == _target.Type).ToList();
                if (targets.Count == 0)
                {
                    OrderComplete = true;
                    return;
                }

                _target = StrategyGame.RandomItem(targets);
            }
            else if (_target != null && _target.Active && _target.SectorId != ship.SectorId)
            {
                // Follow the target to another sector then resume
                ship.InsertOrder(new NavigateOrder(_game, ship, _target.SectorId));
            }
            else
            {
                OrderComplete = true;
                return;
            }

            base.Update(ship);

            if (OrderComplete && (ship.Type == EShipType.Lifepod || ship.Type == EShipType.CarrierDrone))
            {
                ship.Dock(null);

                if (ship.Type == EShipType.CarrierDrone) ship.Active = false;
            }
            else if (OrderComplete && _target != null && _target.Active && _target.SectorId == ship.SectorId)
            {
                OrderComplete = false;
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
