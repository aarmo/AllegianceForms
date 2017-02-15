using AllegianceForms.Engine;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Drawing;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;

namespace AllegianceForms.AI.Missions
{
    public class MinerOffenseMission : CommanderMission
    {
        private const int LastTargetExpireSeconds = 30;

        private int _lastTargetSectorId;
        private PointF _lastPos;
        private DateTime _lastTargetExpire;

        public MinerOffenseMission(CommanderAI ai, Sector ui) : base(ai, ui)
        {
            CheckForNextTargetSector();
        }

        private void CheckForNextTargetSector()
        {
            var bs = StrategyGame.AllUnits.Where(_ => _.Team != AI.Team && _.Active && _.VisibleToTeam[AI.Team - 1] && !_.Docked && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner)).ToList();
            if (bs.Count == 0) return;

            var s = bs[StrategyGame.Random.Next(bs.Count)];
            _lastTargetSectorId = s.SectorId;
            _lastPos = s.CenterPoint;
            _lastTargetExpire = DateTime.Now.AddSeconds(LastTargetExpireSeconds);
        }

        public override bool RequireMorePilots()
        {
            var numPilots = (int)(StrategyGame.GameSettings.NumPilots * 0.8f) + 1;

            return IncludedShips.Sum(_ => _.NumPilots) < numPilots;
        }

        public override void AddMorePilots()
        {            
            var launchBase = StrategyGame.ClosestSectorWithBase(AI.Team, _lastTargetSectorId);
            if (launchBase == null)
            {
                return;
            }

            Ship ship = null;

            // Launch at least 1 scout first, followed by our best pilotable combat ships
            if (IncludedShips.Count < 2)
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
            CheckForNextTargetSector();

            // If we haven't seen a target for some time, abort!
            if (DateTime.Now > _lastTargetExpire)
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
                    i.OrderShip(new NavigateOrder(i, _lastTargetSectorId));
                    LogOrder();
                    i.OrderShip(new MoveOrder(_lastTargetSectorId, _lastPos), true);
                    LogOrder();
                }
                else
                {
                    // Then find the closest random miner here to attack!
                    var m = StrategyGame.ClosestDistance(i.CenterX, i.CenterY, StrategyGame.AllUnits.Where(_ => _.Team != AI.Team && _.Active && _.SectorId == i.SectorId && !_.Docked && _.VisibleToTeam[AI.Team - 1] && (_.Type == EShipType.Constructor || _.Type == EShipType.Miner || _.CanAttackBases())));
                    if (m != null)
                    {
                        i.OrderShip(new MoveOrder(m.SectorId, m.CenterPoint));
                        LogOrder();
                    }
                    else
                    {
                        // attack anything!
                        var ens = StrategyGame.AllUnits.Where(_ => _.Team != AI.Team && _.Active && !_.Docked && _.SectorId == i.SectorId && _.VisibleToTeam[AI.Team - 1] && _.Type != EShipType.Lifepod).ToList();
                        if (ens.Count > 0)
                        {
                            var tar = ens[StrategyGame.Random.Next(ens.Count)];
                            i.OrderShip(new MoveOrder(tar.SectorId, tar.CenterPoint));
                            LogOrder();
                        }
                        else
                        {
                            if (i.Bounds.Contains(_lastPos)) continue;

                            i.OrderShip(new MoveOrder(_lastTargetSectorId, _lastPos));
                            LogOrder();
                        }
                    }
                }
            }
        }
    }
}
