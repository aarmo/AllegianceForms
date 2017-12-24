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
                var targets = _game.AllUnits.Where(_ => _.Active && _.SectorId == ship.SectorId && _.Alliance != ship.Alliance && _.Type == _target.Type).ToList();
                if (targets.Count == 0)
                {
                    OrderComplete = true;
                    return;
                }

                _target = StrategyGame.RandomItem(targets);
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
