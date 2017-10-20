using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Orders
{
    public class CaptureOrder : MoveOrder
    {
        Base _target;

        public CaptureOrder(StrategyGame game, Ship ship) : this(game, ship, null)
        {
        }

        public CaptureOrder(StrategyGame game, Ship ship, Base bs) : base(game, ship.SectorId)
        {
            OrderPen.Color = Color.OrangeRed;
            _target = bs;

            if (_target == null)
            {
                FindClosestEnemyBase(ship);
            }
            else
            {
                OrderPosition = _target.CenterPoint;
            }
        }

        private void FindClosestEnemyBase(Ship ship)
        {
            // Find the closest Enemy station we can capture (shield < 1%)
            _target = StrategyGame.ClosestDistance(ship.CenterX, ship.CenterY, 
                    _game.AllBases.Where(_ => _.Active && _.Team != ship.Team && _.SectorId == ship.SectorId && _.CanLaunchShips() && _.CanBeCaptured()));

            if (_target != null)
            {
                OrderPosition = _target.CenterPoint;
                OrderSectorId = ship.SectorId;
            }
            else
            {
                OrderComplete = true;
            }
        }
        public override void Update(Ship ship)
        {
            if (_target == null || !_target.Active || ship.SectorId != OrderSectorId)
            {
                FindClosestEnemyBase(ship);
            }
            base.Update(ship);

            if (OrderComplete && _target != null)
            {
                // Capture base! 
                _target.Capture(ship);
                ship.Dock(_target);
            }
        }
    }
}
