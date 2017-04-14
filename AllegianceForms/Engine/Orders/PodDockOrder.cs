using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Orders
{
    public class PodDockOrder : DockOrder
    {
        protected Ship _dockPodTarget;

        public PodDockOrder(StrategyGame game, Ship ship, bool append = false) : this(game, ship, null, append)
        {
        }

        public PodDockOrder(StrategyGame game, Ship ship, Base bs, bool append) : base(game, ship, bs, append)
        {
            OrderPen.Color = Color.WhiteSmoke;
            _dockTarget = bs;

            if (_dockPodTarget == null)
            {
                FindClosestFriendlyShip(ship, append);
            }
            else
            { 
                OrderPosition = _dockPodTarget.CenterPoint;
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

            if (_dockPodTarget == null || !_dockPodTarget.Active || _dockPodTarget.SectorId != OrderSectorId)
            {
                FindClosestFriendlyShip(ship, false);
            }
            else
            {
                OrderPosition = _dockPodTarget.CenterPoint;
            }

            base.Update(ship);

            if (OrderComplete && (_dockTarget != null || _dockPodTarget != null))
            {
                // Offload miner, repair/rearm etc.
                ship.Dock();
            }
        }

        protected void FindClosestFriendlyShip(Ship ship, bool append)
        {
            // Find the closest Friendly ship we can dock at
            _dockPodTarget = StrategyGame.ClosestDistance(ship.CenterX, ship.CenterY, _game.AllUnits.Where(_ => _.Active && _.Team == ship.Team && _.SectorId == ship.SectorId && _.Type != EShipType.Lifepod && _.Type != EShipType.Constructor && _.Type != EShipType.Miner));

            if (_dockPodTarget != null)
            {
                OrderPosition = _dockPodTarget.CenterPoint;
                OrderSectorId = ship.SectorId;
            }
        }
    }
}
