using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.AI.Missions
{
    public class BaseDefenseMission : CommanderMission
    {
        private int _lastTargetSectorId = -1;
        private PointF _lastPos;

        public BaseDefenseMission(StrategyGame game, BaseAI ai, Ship.ShipEventHandler shipHandler) : base(game, ai, shipHandler)
        {
        }

        public override bool RequireMorePilots()
        {
            return (StrategyGame.RandomChance(0.75f));
        }

        public override bool AddMorePilots()
        {
            if (_lastTargetSectorId == -1) return false;
            var launchBase = _game.ClosestSectorWithBase(AI.Team, _lastTargetSectorId);
            if (launchBase == null) return false;

            // Create our best combat ship
            Ship ship = _game.Ships.CreateCombatShip(AI.Team, AI.TeamColour, launchBase.SectorId);
            if (ship == null) return false;
            
            ship.CenterX = launchBase.CenterX;
            ship.CenterY = launchBase.CenterY;

            var pos = launchBase.GetNextBuildPosition();
            ship.ShipEvent += _shipHandler;
            ship.OrderShip(new MoveOrder(_game, launchBase.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            _game.LaunchShip(ship);
            return true;
        }
        
        public override bool MissionComplete()
        {
            CheckForNextTargetSector();

            return false;
        }

        private void CheckForNextTargetSector()
        {
            var bs = _game.AllUnits.Where(_ => _.Active && _.Alliance != AI.Alliance && _.IsVisibleToTeam(AI.Team-1) && (_.CanAttackBases() || _.Type == EShipType.Miner || _.Type == EShipType.Constructor)).ToList();
            if (bs.Count == 0) 
            {
                _lastTargetSectorId = -1;
                return;
            }

            var s = bs[StrategyGame.Random.Next(bs.Count)];
            _lastTargetSectorId = s.SectorId;
            _lastPos = s.CenterPoint;
        }

        public override void UpdateMission()
        {
            base.UpdateMission();
            if (_completed || _lastTargetSectorId == -1) return;

            foreach (var i in IncludedShips)
            {
                if (i.CurrentOrder != null) continue;

                // Make sure we are in the same sector as the target
                if (i.SectorId != _lastTargetSectorId)
                {
                    i.OrderShip(new NavigateOrder(_game, i, _lastTargetSectorId));
                    LogOrder();
                    i.OrderShip(new MoveOrder(_game, _lastTargetSectorId, _lastPos), true);
                    LogOrder();
                }
                else
                {
                    // Then find a random enemy here to attack!
                    var ens = _game.AllUnits.Where(_ => _.Alliance != AI.Alliance && _.Active && _.SectorId == i.SectorId && _.IsVisibleToTeam(AI.Team - 1) && _.Type != EShipType.Lifepod).ToList();
                    if (ens.Count > 0)
                    {
                        var tar = ens[StrategyGame.Random.Next(ens.Count)];
                        i.OrderShip(new MoveOrder(_game, tar.SectorId, tar.CenterPoint));
                        LogOrder();
                    }
                    else
                    {
                        if (i.Bounds.Contains(_lastPos)) continue;

                        i.OrderShip(new MoveOrder(_game, _lastTargetSectorId, _lastPos));
                        LogOrder();
                    }
                }
            }
        }
    }
}
