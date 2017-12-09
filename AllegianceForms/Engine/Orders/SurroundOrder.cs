using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Orders
{
    public class SurroundOrder : MoveOrder
    {
        GameEntity _target;
        const int DistanceFromPoint = 100;

        public SurroundOrder(StrategyGame game, int sectorId, GameEntity target, PointF offset) : base(game, sectorId, target.CenterPoint, offset)
        {
            OrderPen.Color = Color.SkyBlue;
            _target = target;
        }

        public override void Update(Ship ship)
        {
            base.Update(ship);
            
            // Keep repeating and move around the target
            if (OrderComplete)
            {
                if (_target == null || !_target.Active)
                {
                    Base launchBase;
                    _target = _game.RandomEnemyBase(ship.Team, out launchBase);

                    if (_target == null)
                    {
                        OrderComplete = true;
                        return;
                    }

                    // Get to the target's sector
                    if (_target.SectorId != ship.SectorId)
                    {
                        ship.OrderShip(new NavigateOrder(_game, ship, _target.SectorId));
                        ship.OrderShip(new SurroundOrder(_game, _target.SectorId, _target, PointF.Empty), true);
                    }
                }

                if (_target != null)
                {
                    var pos = _target.CenterPoint;
                    var currentAngle = StrategyGame.Random.Next(360);
                    var angleAsRadians = (currentAngle * Math.PI) / 180.0;
                    var x = pos.X + Math.Cos(angleAsRadians) * DistanceFromPoint;
                    var y = pos.Y + Math.Sin(angleAsRadians) * DistanceFromPoint;

                    OrderPosition = new PointF((float)x, (float)y);
                    OrderComplete = false;
                }
            }
        }
    }
}
