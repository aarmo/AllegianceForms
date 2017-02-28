using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Rocks;
using AllegianceForms.Engine.Ships;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Orders
{
    public class BuildOrder : MoveOrder
    {
        private Asteroid _targetRock;

        public BuildOrder(int sectorId) : this(sectorId, Point.Empty, Point.Empty)
        {
        }

        public BuildOrder(int sectorId, Point targetPosition, Point offset) : base(sectorId, targetPosition, offset)
        {
            OrderPen.Color = Color.Blue;
        }

        public override void Update(Ship ship)
        {
            var builder = (BuilderShip)ship;
            if (builder.Building)
            {
                OrderComplete = true;
                return;
            }

            base.Update(ship);
            if (!BaseSpecs.IsTower(builder.BaseType) && _targetRock == null)
            {
                _targetRock = StrategyGame.ClosestDistance(OrderPosition.X, OrderPosition.Y, StrategyGame.BuildableAsteroids.Where(_ => _.Active && _.SectorId == ship.SectorId && _.VisibleToTeam[ship.Team-1] && _.Type == builder.TargetRockType));

                if (_targetRock != null)
                {
                    builder.Target = _targetRock;
                    OrderPosition = _targetRock.CenterPoint;
                }
            }

            if (OrderComplete && (BaseSpecs.IsTower(builder.BaseType) || _targetRock != null))
            {
                builder.Building = true;
            }
        }
    }
}
