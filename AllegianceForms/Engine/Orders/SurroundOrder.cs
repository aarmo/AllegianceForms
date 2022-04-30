using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System;
using System.Drawing;

namespace AllegianceForms.Orders
{
    public class SurroundOrder : MoveOrder
    {
        GameEntity _target;
        private int _distanceFromPoint = 100;
        private int _targetBaseTeam = -1;

        public SurroundOrder(StrategyGame game, int sectorId, GameEntity target, int distanceFromPoint = 100) : base(game, sectorId, target.CenterPoint, PointF.Empty)
        {
            OrderPen.Color = Color.SkyBlue;
            _target = target;

            var targetBase = target as Base;
            _targetBaseTeam = targetBase != null ? targetBase.Team : -1;
            _distanceFromPoint = distanceFromPoint;
        }

        public override void Update(Ship ship)
        {
            base.Update(ship);
            
            // Keep repeating and move around the target
            if (OrderComplete)
            {
                if (_target == null || !_target.Active && _targetBaseTeam > 0)
                {
                    _target = _game.RandomBase(_targetBaseTeam, ship.SectorId);
                    if (_target == null) 
                    {
                        // Find the next priority base to surround
                        _target = _game.RandomEnemyBase(ship.Team, out Base launchBase);

                        if (_target == null)
                        {
                            // Dock if none
                            ship.OrderShip(new DockOrder(_game, ship));
                            return;
                        }
                        else if (_target.SectorId != ship.SectorId)
                        {
                            // Get to the target's sector
                            ship.OrderShip(new NavigateOrder(_game, ship, _target.SectorId));
                            ship.OrderShip(new SurroundOrder(_game, _target.SectorId, _target, _distanceFromPoint), true);
                            return;
                        }
                    }
                }

                if (_target != null)
                {
                    var pos = _target.CenterPoint;
                    var currentAngle = StrategyGame.Random.Next(360);
                    var angleAsRadians = (currentAngle * Math.PI) / 180.0;
                    var x = pos.X + Math.Cos(angleAsRadians) * _distanceFromPoint;
                    var y = pos.Y + Math.Sin(angleAsRadians) * _distanceFromPoint;

                    OrderPosition = new PointF((float)x, (float)y);
                    OrderComplete = false;
                }
            }
        }
    }
}
