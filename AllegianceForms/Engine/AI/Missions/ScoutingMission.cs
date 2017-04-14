using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Engine.AI.Missions
{
    public class ScoutingMission : CommanderMission
    {
        private Dictionary<Ship, float> _lastHealth = new Dictionary<Ship, float>();
        
        public ScoutingMission(StrategyGame game, BaseAI ai, Ship.ShipEventHandler shipEvent) : base(game, ai, shipEvent)
        { }
        
        public override bool RequireMorePilots()
        {
            var numScouts = _game.Map.Sectors.Count / 3 + 1;

            return IncludedShips.Count < numScouts;
        }

        public override void AddMorePilots()
        {
            var bs = _game.AllBases.Where(_ => _.Team == AI.Team && _.Active && _.CanLaunchShips()).ToList();
            if (bs.Count == 0) return;
            var b = bs[StrategyGame.Random.Next(bs.Count)];

            // launch a scout if possible
            var ship = _game.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, b.SectorId);
            if (ship == null) return;

            ship.CenterX = b.CenterX;
            ship.CenterY = b.CenterY;

            var pos = b.GetNextBuildPosition();
            ship.ShipEvent += _shipHandler;
            ship.OrderShip(new MoveOrder(_game, b.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            _game.LaunchShip(ship);
            _lastHealth.Add(ship, ship.Health);
        }

        public override void UpdateMission()
        {
            var removeHealth = _lastHealth.Keys.Where(_ => !_.Active).ToList();
            removeHealth.ForEach(_ => _lastHealth.Remove(_));

            base.UpdateMission();
            if (_completed) return;
            
            foreach (var i in IncludedShips)
            {
                if (!_lastHealth.ContainsKey(i))
                {
                    _lastHealth.Add(i, i.Health);
                }
                else
                {
                    var visibleSectors = _game.Map.Sectors.Where(_ => _.VisibleToTeam[AI.Team - 1]).ToList();
                    if (visibleSectors.Count == 0) continue;
                    var randomSectorId = visibleSectors[StrategyGame.Random.Next(visibleSectors.Count)].Id;

                    if (i.Health < _lastHealth[i])
                    {
                        // Taking fire! Dock and continue
                        i.OrderShip(new DockOrder(_game, i));
                        LogOrder();
                        i.OrderShip(new NavigateOrder(_game, i, randomSectorId), true);
                        LogOrder();
                        i.OrderShip(new MoveOrder(_game, randomSectorId, _centerPos, PointF.Empty), true);
                        LogOrder();
                    }
                    else if (i.CurrentOrder == null)
                    {
                        // If we have stopped, keep scouting
                        if (randomSectorId != i.SectorId)
                        {
                            i.OrderShip(new NavigateOrder(_game, i, randomSectorId));
                            LogOrder();
                            i.OrderShip(new MoveOrder(_game, randomSectorId, centerPos, PointF.Empty), true);
                            LogOrder();
                        }
                    }
                }
            }
        }
    }
}
