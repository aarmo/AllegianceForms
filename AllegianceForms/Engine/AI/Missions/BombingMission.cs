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
        private const int DistanceFromCenter = 100;
        private const int NewTargetCheckDelay = 200;
        private int _checkForNewTarget = NewTargetCheckDelay;

        public BombingMission(StrategyGame game, BaseAI ai, Ship.ShipEventHandler shipHandler) : base(game, ai, shipHandler)
        { }

        public override void UpdateMission()
        {
            base.UpdateMission();
            if (_completed) return;

            _checkForNewTarget--;
            if (_checkForNewTarget < 0) CheckForNewTarget();

            if (_targetBase == null) return;

            var firstBomber = IncludedShips.FirstOrDefault(_ => _.Active && _.CanAttackBases());

            foreach (var i in IncludedShips)
            {
                if (i.CurrentOrder != null) continue;
                var append = false;

                if (i.Type == EShipType.Scout)
                {
                    if (firstBomber == null)
                    {
                        i.OrderShip(new DockOrder(_game, i));
                        LogOrder();
                        continue;
                    }

                    if (i.SectorId != firstBomber.SectorId)
                    {
                        i.OrderShip(new NavigateOrder(_game, i, _targetBase.SectorId));
                        LogOrder();
                        append = true;
                    }

                    i.OrderShip(new MoveOrder(_game, firstBomber.SectorId, firstBomber.CenterPoint), append);
                    LogOrder();
                }
                else
                {
                    // Bombers and escorts: Aim to surround the base!
                    if (i.SectorId != _targetBase.SectorId)
                    {
                        i.OrderShip(new NavigateOrder(_game, i, _targetBase.SectorId));
                        LogOrder();
                        append = true;
                    }

                    var currentAngle = StrategyGame.Random.Next(360);
                    var angleAsRadians = (currentAngle * Math.PI) / 180.0;
                    var x = _targetBase.CenterX + Math.Cos(angleAsRadians) * DistanceFromCenter;
                    var y = _targetBase.CenterY + Math.Sin(angleAsRadians) * DistanceFromCenter;

                    i.OrderShip(new MoveOrder(_game, _targetBase.SectorId, new PointF((float)x, (float)y)), append);
                    LogOrder();
                }
            }
        }
        
        private void CheckForNewTarget()
        {
            _targetBase = _game.RandomEnemyBase(AI.Team, out _launchBase);
        }

        public override bool RequireMorePilots()
        {
            return true;
        }

        public override bool AddMorePilots()
        {
            if (_launchBase == null || _targetBase == null || !_launchBase.Active || !_targetBase.Active || _targetBase.Alliance == AI.Alliance || _launchBase.Team != AI.Team)
                CheckForNewTarget();
            if (_launchBase == null || _targetBase == null) return false;
            
            // Send any cap ships for support
            var capships = _game.AllUnits.Where(_ => _.Active && _.Team == AI.Team && Ship.IsCapitalShip(_.Type) && _.CurrentOrder == null).ToList();
            IncludedShips.AddRange(capships);
            
            Ship ship;
            var t = AI.Team - 1;

            ship = _game.Ships.CreateBomberShip(AI.Team, AI.TeamColour, _launchBase.SectorId);
            if (ship == null)
            {
                if (StrategyGame.RandomChance(0.5f))
                {
                    ship = _game.Ships.CreateCombatShip(AI.Team, AI.TeamColour, _launchBase.SectorId);
                }
                else
                {
                    ship = _game.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, _launchBase.SectorId);
                }
            }
            if (ship == null) return false;

            ship.CenterX = _launchBase.CenterX;
            ship.CenterY = _launchBase.CenterY;

            var pos = _launchBase.GetNextBuildPosition();
            ship.ShipEvent += _shipHandler;
            ship.OrderShip(new MoveOrder(_game, _launchBase.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            _game.LaunchShip(ship);
            return true;
        }

        public override bool MissionComplete()
        {
            // If we have no more visible bases to attack or launch from (we loose), stop this.
            if (_targetBase == null || !_targetBase.Active || _targetBase.Alliance == AI.Alliance || _launchBase == null || !_launchBase.Active || _launchBase.Team != AI.Team)
            {
                CheckForNewTarget();
                if (_targetBase == null || _launchBase == null) return true;
            }

            return false;
        }
    }
}
