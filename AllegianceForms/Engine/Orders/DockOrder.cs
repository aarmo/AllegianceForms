using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Orders
{
    public class DockOrder : MoveOrder
    {
        protected Base _dockTarget;

        public DockOrder(StrategyGame game, Ship ship, bool append = false) : this(game, ship, null, append)
        {
        }

        public DockOrder(StrategyGame game, Ship ship, Base bs, bool append) : base(game, ship.SectorId)
        {
            OrderPen.Color = Color.WhiteSmoke;
            _dockTarget = bs;

            if (_dockTarget == null)
            {
                FindClosestBase(ship, append);
            }
            else
            { 
                OrderPosition = _dockTarget.CenterPoint;
            }
        }

        public override void Update(Ship ship)
        {
            if (ship.Docked)
            {
                OrderComplete = true;
                return;
            }

            if (_dockTarget == null || !_dockTarget.Active || ship.SectorId != OrderSectorId)
            {
                FindClosestBase(ship, false);
            }
            base.Update(ship);

            if (OrderComplete && _dockTarget != null)
            {
                // Offload miner, repair/rearm etc.
                ship.Dock(_dockTarget);
            }
        }

        protected void FindClosestBase(Ship ship, bool append)
        {
            // Find the closest Friendly station we can dock at
            _dockTarget = StrategyGame.ClosestDistance(ship.CenterX, ship.CenterY, _game.AllBases.Where(_ => _.Active && _.Team == ship.Team && _.SectorId == ship.SectorId && _.CanLaunchShips()));

            if (_dockTarget != null)
            {
                OrderPosition = _dockTarget.CenterPoint;
                OrderSectorId = ship.SectorId;
            }
            else
            {
                // Look for a base in another sector...
                var otherBase = _game.ClosestSectorWithBase(ship.Team, ship.SectorId);

                OrderComplete = true;

                if (otherBase != null)
                {
                    ship.OrderShip(new NavigateOrder(_game, ship, otherBase.SectorId), append);
                    ship.OrderShip(new DockOrder(_game, ship, otherBase, false), true);
                }
            }
        }
    }
}
