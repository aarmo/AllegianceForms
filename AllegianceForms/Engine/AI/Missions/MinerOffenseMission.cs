using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace AllegianceForms.Engine.AI.Missions
{
    public class MinerOffenseMission : CommanderMission
    {
        private const int LastTargetExpireTicks = 80;

        private int _lastTargetSectorId;
        private PointF _lastPos;
        private int _lastTargetExpire;

        public MinerOffenseMission(StrategyGame game, BaseAI ai, Ship.ShipEventHandler shipEvent) : base(game, ai, shipEvent)
        {
            CheckForNextTargetSector();
        }

        private void CheckForNextTargetSector()
        {
            var bs = _game.AllUnits.Where(_ => _.Alliance != AI.Alliance && _.Active && _.IsVisibleToTeam(AI.Team - 1) && !_.Docked && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner)).ToList();
            if (bs.Count == 0) return;

            var s = bs[StrategyGame.Random.Next(bs.Count)];
            _lastTargetSectorId = s.SectorId;
            _lastPos = s.CenterPoint;
            _lastTargetExpire = LastTargetExpireTicks;
        }

        public override bool RequireMorePilots()
        {
            var numPilots = (int)(_game.GameSettings.NumPilots * 0.8f) + 1;

            return IncludedShips.Count < numPilots;
        }

        public override bool AddMorePilots()
        {            
            var launchBase = _game.ClosestSectorWithBase(AI.Team, _lastTargetSectorId);
            if (launchBase == null)
            {
                return false;
            }

            Ship ship = null;

            // Launch at least 1 scout first, followed by our best pilotable combat ships
            if (IncludedShips.Count < 2)
            {
                ship = _game.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, launchBase.SectorId);
            }
            else
            {
                ship = _game.Ships.CreateCombatShip(AI.Team, AI.TeamColour, launchBase.SectorId);
            }
            if (ship == null) return false;
            if (!_game.LaunchShip(ship)) return false;

            ship.CenterX = launchBase.CenterX;
            ship.CenterY = launchBase.CenterY;

            var pos = launchBase.GetNextBuildPosition();
            ship.ShipEvent += _shipHandler;
            ship.OrderShip(new MoveOrder(_game, launchBase.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            return true;
        }

        public override bool MissionComplete()
        {
            CheckForNextTargetSector();

            // If we haven't seen a target for some time, abort!
            _lastTargetExpire--;
            if (_lastTargetExpire <= 0)
            {
                return true;
            }
            
            return false;
        }

        public override void UpdateMission()
        {
            base.UpdateMission();
            if (_completed) return;

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
                    // Then find the closest random miner/con/bomber here to attack!
                    var m = Utils.ClosestDistance(i.CenterX, i.CenterY, _game.AllUnits.Where(_ => _.Alliance != AI.Alliance && _.Active && _.SectorId == i.SectorId && !_.Docked && _.IsVisibleToTeam(AI.Team - 1) && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner || _.CanAttackBases())));
                    if (m != null)
                    {
                        i.OrderShip(new MoveOrder(_game, m.SectorId, m.CenterPoint));
                        LogOrder();
                    }
                    else
                    {
                        // Otherwise, attack anything!
                        var ens = _game.AllUnits.Where(_ => _.Active && !_.Docked && _.Alliance != AI.Alliance && _.SectorId == i.SectorId && _.IsVisibleToTeam(AI.Team - 1) && _.Type != EShipType.Lifepod).ToList();
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
}
