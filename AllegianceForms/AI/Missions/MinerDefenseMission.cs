using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.AI.Missions
{
    public class MinerDefenseMission : CommanderMission
    {
        private const int LastTargetExpireSeconds = 30;

        private float _numPilots;
        private int _lastTargetSectorId;
        private PointF _lastPos;

        public MinerDefenseMission(CommanderAI ai, Sector ui) : base(ai, ui)
        {
            _numPilots = StrategyGame.GameSettings.NumPilots * 0.8f + 1f;
        }
        
        public override bool RequireMorePilots()
        {
            var currentPilots = IncludedShips.Sum(_ => _.NumPilots);
            return currentPilots < _numPilots;
        }
        
        public override void AddMorePilots()
        {
            var bs = StrategyGame.AllUnits.Where(_ => _.Team == AI.Team && _.Active && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner)).ToList();
            if (bs.Count == 0) return;

            var s = bs[StrategyGame.Random.Next(bs.Count)];
            var launchBase = StrategyGame.ClosestSectorWithBase(AI.Team, s.SectorId);
            if (launchBase == null) return;

            Ship ship = null;
            // launch at least 2 scouts first, followed by our best pilotable combat ships
            if (IncludedShips.Count < 3)
            {
                ship = StrategyGame.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, launchBase.SectorId);
            }
            else
            {
                ship = StrategyGame.Ships.CreateCombatShip(AI.Team, AI.TeamColour, launchBase.SectorId);
            }
            if (ship == null) return;

            ship.CenterX = launchBase.CenterX;
            ship.CenterY = launchBase.CenterY;

            var pos = launchBase.GetNextBuildPosition();
            ship.ShipEvent += UI.F_ShipEvent;
            ship.OrderShip(new MoveOrder(launchBase.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            StrategyGame.LaunchShip(ship);
        }
        
        public override bool MissionComplete()
        {
            // If we haven't anything to defend, abort!
            var bs = StrategyGame.AllUnits.Where(_ => _.Team == AI.Team && !_.Docked && _.Active && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner)).ToList();
            return (bs.Count == 0);
        }

        private void CheckForNextTargetSector()
        {
            var bs = StrategyGame.AllUnits.Where(_ => _.Team == AI.Team && !_.Docked && _.Active && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner)).ToList();
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
                    i.OrderShip(new NavigateOrder(i, _lastTargetSectorId));
                    LogOrder();
                    i.OrderShip(new MoveOrder(_lastTargetSectorId, _lastPos), true);
                    LogOrder();
                }
                else
                {
                    // Then find a random enemy here to attack!
                    var ens = StrategyGame.AllUnits.Where(_ => _.Team != AI.Team && _.Active && !_.Docked && _.SectorId == i.SectorId && _.VisibleToTeam[AI.Team - 1]).ToList();
                    if (ens.Count > 0)
                    {
                        var tar = ens[StrategyGame.Random.Next(ens.Count)];
                        i.OrderShip(new MoveOrder(tar.SectorId, tar.CenterPoint));
                        LogOrder();
                    }
                    else
                    {
                        i.OrderShip(new MoveOrder(_lastTargetSectorId, _lastPos));
                        LogOrder();
                    }
                }
            }
        }
    }
}
