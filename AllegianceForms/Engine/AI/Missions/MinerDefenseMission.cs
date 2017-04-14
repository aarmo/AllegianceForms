using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Collections.Generic;
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
        private List<Ship> _shipsToProtect;


        public MinerDefenseMission(StrategyGame game, BaseAI ai, Ship.ShipEventHandler shipEvent) : base(game, ai, shipEvent)
        {
            _numPilots = _game.GameSettings.NumPilots * 0.5f + 1f;
            _shipsToProtect = new List<Ship>();
        }
        
        public override bool RequireMorePilots()
        {
            _shipsToProtect = _game.AllUnits.Where(_ => _.Alliance == AI.Alliance && !_.Docked && _.Active && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner || _.CanAttackBases())).ToList();

            var currentPilots = IncludedShips.Sum(_ => _.NumPilots);
            return currentPilots < _shipsToProtect.Count * 3;
        }

        public override bool AddMorePilots()
        {
            if (_shipsToProtect.Count == 0) return false;

            var s = _shipsToProtect[StrategyGame.Random.Next(_shipsToProtect.Count)];
            var launchBase = _game.ClosestSectorWithBase(AI.Team, s.SectorId);
            if (launchBase == null) return false;

            Ship ship = null;
            // launch scouts and our best pilotable combat ships
            if (StrategyGame.RandomChance(0.5f))
            {
                ship = _game.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, launchBase.SectorId);
            }
            else
            {
                ship = _game.Ships.CreateCombatShip(AI.Team, AI.TeamColour, launchBase.SectorId);
            }
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
            // If we haven't anything to defend, abort!
            return (_shipsToProtect.Count == 0);
        }

        private void CheckForNextTargetSector()
        {
            if (_shipsToProtect.Count == 0) return;

            var s = _shipsToProtect[StrategyGame.Random.Next(_shipsToProtect.Count)];
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

                // Make sure we are in the same sector as a friendly con/miner/bomber
                if (i.SectorId != _lastTargetSectorId)
                {
                    i.OrderShip(new NavigateOrder(_game, i, _lastTargetSectorId));
                    LogOrder();
                }
                else
                {
                    // Then follow them                   
                    i.OrderShip(new MoveOrder(_game, _lastTargetSectorId, _lastPos));
                    LogOrder();
                }
            }
        }
    }
}
