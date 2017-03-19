using AllegianceForms.Engine.Rocks;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.AI.Missions
{
    public class BuilderMission : CommanderMission
    {
        public BuilderMission(CommanderAI ai, Ship.ShipEventHandler shipEvent) : base(ai, shipEvent)
        {
        }

        public override void UpdateMission()
        {
            base.UpdateMission();

            var t = AI.Team - 1;

            // Check our constructor ships
            var ships = StrategyGame.AllUnits.Where(_ => _.Active && _.Team == AI.Team && _.Type == EShipType.Constructor).ToList();
            var maxHops = StrategyGame.Map.Wormholes.Count + 4;

            var chosenRocks = new List<Asteroid>();

            foreach (var s in ships)
            {
                var b = s as BuilderShip;
                if (b == null) continue;
                if (b.Target != null) chosenRocks.Add(b.Target);
                if (b.CurrentOrder != null || b.Target != null) continue;

                var possibleRocks = StrategyGame.BuildableAsteroids.Where(_ => _.Active && _.VisibleToTeam[t] && _.Type == b.TargetRockType).ToList();

                // Order this builder somewhere smart...
                // Score one rock per sector: close to our home, without an enemy or friendly base

                var sectorChecked = new List<int>();
                var bestScore = int.MaxValue;
                Asteroid bestRock = null;
                foreach (var r in possibleRocks)
                {
                    if (sectorChecked.Contains(r.SectorId) || chosenRocks.Contains(r)) continue;
                    sectorChecked.Add(r.SectorId);

                    var hasEnemyBase = StrategyGame.AllBases.Any(_ => _.Active && _.VisibleToTeam[t] && _.SectorId == r.SectorId && _.Team != AI.Team);
                    var hasFriendlyBase = StrategyGame.AllBases.Any(_ => _.Active && _.SectorId == r.SectorId && _.Team == AI.Team);

                    var path = StrategyGame.Map.ShortestPath(AI.Team, r.SectorId, b.SectorId);
                    var newHops = path == null ? int.MaxValue : path.Count();

                    var score = newHops + (hasEnemyBase ? 2 : 0) + (hasFriendlyBase ? 3 : 0);
                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestRock = r;
                    }
                }

                if (bestRock == null) continue;

                chosenRocks.Add(bestRock);
                b.Target = bestRock;

                if (bestRock.SectorId != b.SectorId)
                {
                    b.OrderShip(new NavigateOrder(b, bestRock.SectorId));

                    LogOrder();
                }

                b.OrderShip(new BuildOrder(bestRock.SectorId, bestRock.CenterPointI, Point.Empty), true);
                LogOrder();
            }
        }
    }
}
