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
        public ScoutingMission(StrategyGame game, BaseAI ai, Ship.ShipEventHandler shipEvent) : base(game, ai, shipEvent)
        { }
        
        public override bool RequireMorePilots()
        {
            return IncludedShips.Count < 4;
        }

        public override bool AddMorePilots()
        {
            var bs = _game.AllBases.Where(_ => _.Team == AI.Team && _.Active && _.CanLaunchShips()).ToList();
            if (bs.Count == 0) return false;
            var b = bs[StrategyGame.Random.Next(bs.Count)];

            // launch a scout if possible
            var ship = _game.Ships.CreateCombatShip(Keys.S, AI.Team, AI.TeamColour, b.SectorId);
            if (ship == null) return false;

            ship.CenterX = b.CenterX;
            ship.CenterY = b.CenterY;

            var pos = b.GetNextBuildPosition();
            ship.ShipEvent += _shipHandler;
            ship.OrderShip(new MoveOrder(_game, b.SectorId, pos, Point.Empty));

            IncludedShips.Add(ship);
            _game.LaunchShip(ship);
            return true;
        }

        public override void UpdateMission()
        {
            base.UpdateMission();
            if (_completed) return;
            
            foreach (var i in IncludedShips)
            {
                var visibleSectors = _game.Map.Sectors.Where(_ => _.VisibleToTeam[AI.Team - 1]).ToList();
                if (visibleSectors.Count == 0) continue;
                if (i.CurrentOrder == null)
                {
                    var randomSectorId = visibleSectors[StrategyGame.Random.Next(visibleSectors.Count)].Id;
                    // If we have stopped, keep scouting
                    if (randomSectorId != i.SectorId)
                    {
                        i.OrderShip(new NavigateOrder(_game, i, randomSectorId));
                        LogOrder();
                        i.OrderShip(new MoveOrder(_game, randomSectorId, _centerPos, PointF.Empty), true);
                        LogOrder();
                    }
                }
            }
        }
    }
}
