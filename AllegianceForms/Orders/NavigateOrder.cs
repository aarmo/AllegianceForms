using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Orders
{
    public class NavigateOrder : MoveOrder
    {
        private int _team;
        private GameEntity _nextStep;
        private GameEntity _otherEnd;
        private int _targetSectorId;

        public NavigateOrder(Ship ship, int targetSectorId) : base(ship.SectorId)
        {
            OrderPen.Color = Color.CornflowerBlue;
            _team = ship.Team;
            _targetSectorId = targetSectorId;
            FindNextWormhole(ship.Team, ship.SectorId);
        }

        public override void Update(Ship ship)
        {
            if (OrderSectorId != ship.SectorId)
            {
                FindNextWormhole(ship.Team, ship.SectorId);
            }

            base.Update(ship);

            if (OrderComplete && _nextStep != null)
            {
                ship.SectorId = _otherEnd.SectorId;
                ship.CenterX = _otherEnd.CenterX;
                ship.CenterY = _otherEnd.CenterY;

                if (ship.SectorId != _targetSectorId)
                {
                    OrderComplete = false;
                    FindNextWormhole(ship.Team, ship.SectorId);
                }
            }
        }

        private void FindNextWormhole(int team, int sectorId)
        {
            _nextStep = StrategyGame.NextWormholeEnd(team, sectorId, _targetSectorId, out _otherEnd);

            if (_nextStep != null && _otherEnd != null)
            {
                OrderPosition = _nextStep.CenterPoint;
                OrderSectorId = sectorId;
            }
            else
            {
                OrderComplete = true;
            }
        }
    }
}
