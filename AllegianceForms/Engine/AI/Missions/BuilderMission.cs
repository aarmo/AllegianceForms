﻿using AllegianceForms.Engine.Rocks;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.AI.Missions
{
    public class BuilderMission : CommanderMission
    {
        private List<Asteroid> _chosenRocks = new List<Asteroid>();

        public BuilderMission(BaseAI ai, Ship.ShipEventHandler shipEvent) : base(ai, shipEvent)
        {
        }
        
        public override void UpdateMission()
        {
            base.UpdateMission();

            var t = AI.Team - 1;

            // Check our constructor ships
            var ships = StrategyGame.AllUnits.Where(_ => _.Active && _.Team == AI.Team && _.Type == EShipType.Constructor).ToList();
            var maxHops = StrategyGame.Map.Wormholes.Count + 4;
            
            foreach (var s in ships)
            {
                var b = s as BuilderShip;
                if (b == null) continue;
                if (b.CurrentOrder != null || b.Target != null) continue;

                // Order this builder somewhere smart...
                var possibleRocks = StrategyGame.AllAsteroids.Where(_ => _.Active && _.VisibleToTeam[t] && _.Type == b.TargetRockType && !_chosenRocks.Contains(_)).ToList();

                // Score one rock per sector: close to our home, without an enemy or friendly base
                var sectorChecked = new List<int>();
                var bestScore = int.MaxValue;
                Asteroid bestRock = null;
                foreach (var r in possibleRocks)
                {
                    if (sectorChecked.Contains(r.SectorId)) continue;
                    sectorChecked.Add(r.SectorId);

                    var hasEnemyBase = StrategyGame.AllBases.Any(_ => _.Active && _.VisibleToTeam[t] && _.SectorId == r.SectorId && _.Alliance != AI.Alliance);
                    var hasFriendlyBase = StrategyGame.AllBases.Any(_ => _.Active && _.SectorId == r.SectorId && _.Alliance == AI.Alliance && _.CanLaunchShips());

                    var path = StrategyGame.Map.ShortestPath(AI.Team, r.SectorId, b.SectorId);
                    var newHops = path == null ? int.MaxValue : path.Count();

                    var score = newHops + (hasEnemyBase ? 4 : 0) + (hasFriendlyBase ? 2 : 0);
                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestRock = r;
                    }
                }

                if (bestRock == null) continue;

                _chosenRocks.Add(bestRock);
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
