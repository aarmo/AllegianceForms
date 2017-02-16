using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.AI.Missions
{
    public class ScoutingMission : CommanderMission
    {
        Dictionary<Ship, int> _lastHealth = new Dictionary<Ship, int>();

        public ScoutingMission(CommanderAI ai, Sector ui) : base(ai, ui)
        { }
        
        public override bool RequireMorePilots()
        {
            var numScouts = StrategyGame.Map.Sectors.Count / 2 + 1;

            return IncludedShips.Count < numScouts;
        }

        public override void AddMorePilots()
        {
            var bs = StrategyGame.AllBases.Where(_ => _.Team == AI.Team && _.Active && _.CanLaunchShips()).ToList();
            if (bs.Count == 0) return;
            var b = bs[StrategyGame.Random.Next(bs.Count)];

            // launch a scout if possible
            var ship = StrategyGame.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, b.SectorId);
            if (ship == null) return;

            ship.CenterX = b.CenterX;
            ship.CenterY = b.CenterY;

            var pos = b.GetNextBuildPosition();
            ship.ShipEvent += UI.F_ShipEvent;
            ship.OrderShip(new MoveOrder(b.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            StrategyGame.LaunchShip(ship);
        }

        public override void UpdateMission()
        {
            base.UpdateMission();
            if (_completed) return;

            var centerPos = new PointF(StrategyGame.ScreenWidth / 2, StrategyGame.ScreenHeight / 2);

            foreach (var i in IncludedShips)
            {
                if (!_lastHealth.ContainsKey(i))
                {
                    _lastHealth.Add(i, i.Health);
                }
                else
                {
                    var visibleSectors = StrategyGame.Map.Sectors.Where(_ => _.VisibleToTeam[AI.Team - 1]).ToList();
                    var randomSectorId = visibleSectors[StrategyGame.Random.Next(visibleSectors.Count)].Id;

                    if (i.Health < _lastHealth[i])
                    {
                        // Taking fire! Dock and continue
                        i.OrderShip(new DockOrder(i));
                        LogOrder();
                        i.OrderShip(new NavigateOrder(i, randomSectorId), true);
                        LogOrder();
                        i.OrderShip(new MoveOrder(randomSectorId, centerPos, PointF.Empty), true);
                        LogOrder();
                    }
                    else if (i.CurrentOrder == null)
                    {
                        // If we have stopped, keep scouting
                        if (randomSectorId != i.SectorId)
                        {
                            i.OrderShip(new NavigateOrder(i, randomSectorId));
                            LogOrder();
                            i.OrderShip(new MoveOrder(randomSectorId, centerPos, PointF.Empty), true);
                            LogOrder();
                        }
                    }
                }
            }
        }
    }
}
