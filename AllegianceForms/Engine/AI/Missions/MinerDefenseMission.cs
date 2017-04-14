using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Engine.AI.Missions
{
    public class MinerDefenseMission : CommanderMission
    {
        private const int LastTargetExpireSeconds = 30;

        private float _numPilots;
        private int _lastTargetSectorId;
        private PointF _lastPos;
        
        public MinerDefenseMission(StrategyGame game, BaseAI ai, Ship.ShipEventHandler shipEvent) : base(game, ai, shipEvent)
        {
            _numPilots = _game.GameSettings.NumPilots * 0.5f + 1f;
        }
        
        public override bool RequireMorePilots()
        {
            var currentPilots = IncludedShips.Sum(_ => _.NumPilots);
            return currentPilots < _numPilots;
        }
        
        public override void AddMorePilots()
        {
            var bs = _game.AllUnits.Where(_ => _.Alliance == AI.Alliance && !_.Docked && _.Active && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner)).ToList();
            if (bs.Count == 0) return;

            var s = bs[StrategyGame.Random.Next(bs.Count)];
            var launchBase = _game.ClosestSectorWithBase(AI.Team, s.SectorId);
            if (launchBase == null) return;

            Ship ship = null;
            // launch at least 2 scouts first, followed by our best pilotable combat ships
            if (IncludedShips.Count < 3)
            {
                ship = _game.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, launchBase.SectorId);
            }
            else
            {
                ship = _game.Ships.CreateCombatShip(AI.Team, AI.TeamColour, launchBase.SectorId);
            }
            if (ship == null) return;

            ship.CenterX = launchBase.CenterX;
            ship.CenterY = launchBase.CenterY;

            var pos = launchBase.GetNextBuildPosition();
            ship.ShipEvent += _shipHandler;
            ship.OrderShip(new MoveOrder(_game, launchBase.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            _game.LaunchShip(ship);
        }
        
        public override bool MissionComplete()
        {
            // If we haven't anything to defend, abort!
            var bs = _game.AllUnits.Where(_ => _.Alliance == AI.Alliance && !_.Docked && _.Active && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner)).ToList();
            return (bs.Count == 0);
        }

        private void CheckForNextTargetSector()
        {
            var bs = _game.AllUnits.Where(_ => _.Alliance == AI.Alliance && !_.Docked && _.Active && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner)).ToList();
            if (bs.Count == 0) return;

            var s = bs[StrategyGame.Random.Next(bs.Count)];
            _lastTargetSectorId = s.SectorId;
            _lastPos = s.CenterPoint;
        }

        public override void UpdateMission()
        {
            base.UpdateMission();
            if (_completed) return;

            CheckForNextTargetSector();

            foreach (var i in IncludedShips)
            {
                if (i.CurrentOrder != null) continue;

                // Make sure we are in the same sector as a friendly con/miner
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
                    var ens = _game.AllUnits.Where(_ => _.Alliance != AI.Alliance && _.Active && !_.Docked && _.SectorId == i.SectorId && _.VisibleToTeam[AI.Team - 1]).ToList();
                    if (ens.Count > 0)
                    {
                        var tar = ens[StrategyGame.Random.Next(ens.Count)];
                        i.OrderShip(new MoveOrder(_game, tar.SectorId, tar.CenterPoint));
                        LogOrder();
                    }
                    else
                    {
                        i.OrderShip(new MoveOrder(_game, _lastTargetSectorId, _lastPos));
                        LogOrder();
                    }
                }
            }
        }
    }
}
