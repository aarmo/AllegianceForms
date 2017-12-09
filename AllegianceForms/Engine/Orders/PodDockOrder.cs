using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;
using System;

namespace AllegianceForms.Orders
{
    public class PodDockOrder : DockOrder
    {
        protected GameEntity _target;
        protected Ship _dockPodTarget;
        protected double _shipDistance = 0;
        protected double _baseDistance = 0;

        public PodDockOrder(StrategyGame game, Ship ship, bool append = false) : this(game, ship, null, append)
        {
        }

        public PodDockOrder(StrategyGame game, Ship ship, Base bs, bool append) : base(game, ship, bs, append)
        {
            OrderPen.Color = Color.WhiteSmoke;
            _dockTarget = bs;

            FindClosestFriendlyBaseOrShip(ship);
        }

        private void FindClosestFriendlyBaseOrShip(Ship ship)
        {
            _dockPodTarget = Utils.ClosestDistance(ship.CenterX, ship.CenterY, _game.AllUnits.Where(_ => _.Active && _.Team == ship.Team && _.SectorId == ship.SectorId && _.Type != EShipType.Lifepod && _.Type != EShipType.Constructor && _.Type != EShipType.Miner));

            if (_dockPodTarget != null)
            {
                _shipDistance = Utils.DistanceBetween(ship.CenterPointI, _dockPodTarget.CenterPointI);
            }

            _dockTarget = Utils.ClosestDistance(ship.CenterX, ship.CenterY, _game.AllBases.Where(_ => _.Active && _.Team == ship.Team && _.SectorId == ship.SectorId && _.CanLaunchShips()));
            if (_dockTarget != null)
            {
                _baseDistance = Utils.DistanceBetween(ship.CenterPointI, _dockTarget.CenterPointI);
            }

            if (_shipDistance > 0 && _baseDistance > 0)
            {
                if (_shipDistance < _baseDistance)
                    _target = _dockPodTarget;
                else
                    _target = _dockTarget;
            }
            else
            {
                if (_dockPodTarget != null && _shipDistance > 0) _target = _dockPodTarget;
                if (_dockTarget != null && _baseDistance > 0) _target = _dockTarget;
            }

            OrderSectorId = ship.SectorId;
            if (_target != null) OrderPosition = _target.CenterPoint;
        }

        public override void Update(Ship ship)
        {
            if (ship.Docked)
            {
                OrderComplete = true;
                return;
            }

            if (_target == null || !_target.Active || ship.SectorId != OrderSectorId)
            {
                FindClosestFriendlyBaseOrShip(ship);
            }
            else
            {
                OrderPosition = _target.CenterPoint;
            }

            base.Update(ship);

            if (OrderComplete && _target != null)
            {
                // Offload miner, repair/rearm etc.
                ship.Dock(_dockTarget);
            }
        }
    }
}
