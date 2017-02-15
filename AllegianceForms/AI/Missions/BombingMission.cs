using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Orders;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AllegianceForms.AI.Missions
{
    public class BombingMission : CommanderMission
    {
        Base _targetBase;
        Base _launchBase;

        public BombingMission(CommanderAI ai, Sector ui) : base(ai, ui)
        { }

        public override void UpdateMission()
        {
            base.UpdateMission();
            if (_completed) return;
            if (_targetBase == null) return;

            var distanceFromCenter = 100;
            var angleInDegrees = 360 / IncludedShips.Count;
            var currentAngle = StrategyGame.Random.Next(360);

            foreach (var i in IncludedShips)
            {
                if (i.CurrentOrder != null) continue;

                // Aim to surround the base!
                var angleAsRadians = (currentAngle * Math.PI) / 180.0;
                var x = _targetBase.CenterX + Math.Cos(angleAsRadians) * distanceFromCenter;
                var y = _targetBase.CenterY + Math.Sin(angleAsRadians) * distanceFromCenter;
                var append = false;

                if (i.SectorId != _targetBase.SectorId)
                {
                    i.OrderShip(new NavigateOrder(i, _targetBase.SectorId));
                    LogOrder();
                    append = true;
                }

                i.OrderShip(new MoveOrder(_targetBase.SectorId, new PointF((float)x, (float)y)), append);
                LogOrder();
            }
        }

        public override bool RequireMorePilots()
        {
            return true;
        }

        public override void AddMorePilots()
        {
            // launch a bomber if possible
            if (_launchBase == null || _targetBase == null)
                _targetBase = StrategyGame.ClosestEnemyBase(AI.Team, out _launchBase);
            if (_launchBase == null) return;

            var ship = StrategyGame.Ships.CreateCombatShip(Keys.B, AI.Team, AI.TeamColour, _launchBase.SectorId);
            if (ship == null) return;
            
            ship.CenterX = _launchBase.CenterX;
            ship.CenterY = _launchBase.CenterY;

            var pos = _launchBase.GetNextBuildPosition();
            ship.ShipEvent += UI.F_ShipEvent;
            ship.OrderShip(new MoveOrder(_launchBase.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            StrategyGame.LaunchShip(ship);
        }

        public override bool MissionComplete()
        {
            // If we have no more bombers in this mission, abort!
            if (!IncludedShips.Exists(_ => _.Active && _.CanAttackBases()))
            {
                return true;
            }

            // If we have no more visible bases to attack or launch from (we loose), abort!
            if (_targetBase == null || !_targetBase.Active || _launchBase == null || !_launchBase.Active)
            {
                _targetBase = StrategyGame.ClosestEnemyBase(AI.Team, out _launchBase);

                if (_targetBase == null || _launchBase == null) return true;
            }

            return false;
        }
    }
}
