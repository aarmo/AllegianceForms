using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Engine.AI.Missions
{
    public class BombingMission : CommanderMission
    {
        Base _targetBase;
        Base _launchBase;

        public BombingMission(BaseAI ai, Ship.ShipEventHandler shipHandler) : base(ai, shipHandler)
        { }

        public override void UpdateMission()
        {
            base.UpdateMission();
            if (_completed) return;
            if (_targetBase == null) return;

            var distanceFromCenter = 100;
            
            var firstBomber = IncludedShips.FirstOrDefault(_ => _.Active && _.CanAttackBases());

            foreach (var i in IncludedShips)
            {
                if (i.CurrentOrder != null) continue;
                var append = false;

                if (i.Type == EShipType.Scout)
                {
                    if (firstBomber == null)
                    {
                        i.OrderShip(new DockOrder(i));
                        LogOrder();
                        continue;
                    }

                    if (i.SectorId != firstBomber.SectorId)
                    {
                        i.OrderShip(new NavigateOrder(i, _targetBase.SectorId));
                        LogOrder();
                        append = true;
                    }

                    i.OrderShip(new MoveOrder(firstBomber.SectorId, firstBomber.CenterPoint), append);
                    LogOrder();
                }
                else
                {
                    // Bombers and escorts: Aim to surround the base!
                    if (i.SectorId != _targetBase.SectorId)
                    {
                        i.OrderShip(new NavigateOrder(i, _targetBase.SectorId));
                        LogOrder();
                        append = true;
                    }

                    var currentAngle = StrategyGame.Random.Next(360);
                    var angleAsRadians = (currentAngle * Math.PI) / 180.0;
                    var x = _targetBase.CenterX + Math.Cos(angleAsRadians) * distanceFromCenter;
                    var y = _targetBase.CenterY + Math.Sin(angleAsRadians) * distanceFromCenter;

                    i.OrderShip(new MoveOrder(_targetBase.SectorId, new PointF((float)x, (float)y)), append);
                    LogOrder();
                }
            }
        }

        public override bool RequireMorePilots()
        {
            return true;
        }

        public override void AddMorePilots()
        {
            if (_launchBase == null || _targetBase == null || !_launchBase.Active || _launchBase.Team != AI.Team)
                _targetBase = StrategyGame.ClosestEnemyBase(AI.Team, out _launchBase);
            if (_launchBase == null) return;
            
            // send any cap ships for support
            var capships = StrategyGame.AllUnits.Where(_ => _.Active && _.Team == AI.Team && Ship.IsCapitalShip(_.Type) && _.CurrentOrder == null).ToList();
            IncludedShips.AddRange(capships);
            
            // launch scouts to assist the bomber if we have "enough"
            var enoughBombers = IncludedShips.Count(_ => _.Active && _.CanAttackBases()) > 1;
            Ship ship;

            if (enoughBombers)
            {
                ship = StrategyGame.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, _launchBase.SectorId);
            }
            else
            {
                // launch a bomber if possible
                ship = StrategyGame.Ships.CreateCombatShip(Keys.B, AI.Team, AI.TeamColour, _launchBase.SectorId);
            }
            if (ship == null) return;

            ship.CenterX = _launchBase.CenterX;
            ship.CenterY = _launchBase.CenterY;

            var pos = _launchBase.GetNextBuildPosition();
            ship.ShipEvent += _shipHandler;
            ship.OrderShip(new MoveOrder(_launchBase.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            StrategyGame.LaunchShip(ship);
        }

        public override bool MissionComplete()
        {
            // If we have no more visible bases to attack or launch from (we loose), abort!
            if (_targetBase == null || !_targetBase.Active || _targetBase.Team == AI.Team || _launchBase == null || !_launchBase.Active || _launchBase.Team != AI.Team)
            {
                _targetBase = StrategyGame.ClosestEnemyBase(AI.Team, out _launchBase);

                if (_targetBase == null || _launchBase == null) return true;
            }

            return false;
        }
    }
}
