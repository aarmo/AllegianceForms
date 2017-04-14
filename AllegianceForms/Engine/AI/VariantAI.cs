using AllegianceForms.Engine.AI.Missions;
using AllegianceForms.Engine.Ships;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;
using AllegianceForms.Engine.Tech;

namespace AllegianceForms.Engine.AI
{
    public class VariantAI : BaseAI
    {
        private ScoutingMission _scouting;
        private MinerOffenseMission _minerOffense;
        private MinerDefenseMission _minerDefense;
        private BuilderMission _building;
        private BombingMission _baseOffense;

        private bool _flagBuiltTech;
        private bool _flagFoundEnemyBase;
        private bool _flagHaveBombers;
        
        public VariantAI(int team, Color teamColour, Ship.ShipEventHandler shipHandler) : base(team, teamColour, shipHandler)
        {
            _scouting = new ScoutingMission(this, _shipHandler);
            _minerOffense = new MinerOffenseMission(this, shipHandler);
            _building = new BuilderMission(this, shipHandler);
            _baseOffense = new BombingMission(this, shipHandler);
            _minerDefense = new MinerDefenseMission(this, shipHandler);
        }
        
        public override void Update()
        {
            if (!Enabled) return;
            _nextActionAllowed--;
            if (_nextActionAllowed > 0) return;

            UpdateMissions();
            UpdateBuild();
            UpdateResearch();
            UpdateCheats();

            // Later vary this more:
            // ** STYLE
            // Aggressive: standard scout (sectors/3)+1, push cons into enemy sectors, flood enemy sectors with miner and base offense
            // Expansive: more scout (sectors/2)+1, push cons next to enemy sectors, 50% pilots on constant miner and base offense, 25% on miner defense, rest in reserve
            // Defensive: less scount (sectors/4)+1, push conds next to our sectors, 25% pilots on constant miner and base offense, 50% on miner defense, rest in reserve

            // ** MINERS
            // All: try to get friendly sectors with resources * 2) if a miner is docked mine in a random friendly sector with resources.

            // ** TECH
            // Caps: SY & outpost cons. Build full caps & bombers & SY upgrades. Then Starbase scouts & upgrades. Then other tech for upgrades only
            // 1/2/3 techs - start with home tech. Follow research focus (even/single starting with first). Then SY & outpost cons.

            // ** END GAME
            // Caps: When [CapsBeforeInvading] split into [FinalTechGroups] groups, send all pilots to support caps. Constantly build caps & send.
            // Techs: When end game reached, split into [FinalTechGroups] groups, send all pilots to support groups. Build caps & switch when [CapsBeforeInvading] reached.
        }

        private void UpdateMissions()
        {
            _minerOffense.UpdateMission();
            _baseOffense.UpdateMission();
            _building.UpdateMission();
            _scouting.UpdateMission();
            _minerDefense.UpdateMission();

            if (StrategyGame.DockedPilots[_t] == 0) return;

            if (!_flagHaveBombers || !_flagFoundEnemyBase || !_flagBuiltTech)
            {
                _flagFoundEnemyBase = _flagFoundEnemyBase || StrategyGame.AllBases.Exists(_ => _.VisibleToTeam[_t] && _.Active && _.Alliance != Alliance);
                _flagHaveBombers = _flagHaveBombers  || StrategyGame.TechTree[_t].HasResearchedShipType(EShipType.Bomber);
                _flagBuiltTech = _flagBuiltTech || StrategyGame.AllBases.Exists(_ => _.Active && _.Team == Team && _.IsTechBase());
            }

            if (_flagHaveBombers && _flagFoundEnemyBase)
            {
                while (StrategyGame.DockedPilots[_t] > 0 && _baseOffense.RequireMorePilots())
                {
                    _baseOffense.AddMorePilots();
                }
            }

            if (_flagFoundEnemyBase)
            { 
                while (StrategyGame.DockedPilots[_t] > 0 && _minerOffense.RequireMorePilots())
                {
                    _minerOffense.AddMorePilots();
                }
            }

            if (_building.IncludedShips.Count > 0 && StrategyGame.RandomChance(0.15f))
            {
                while (StrategyGame.DockedPilots[_t] > 0 && _minerDefense.RequireMorePilots())
                {
                    _minerDefense.AddMorePilots();
                }
            }

            while (StrategyGame.DockedPilots[_t] > 0 && _scouting.RequireMorePilots())
            {
                _scouting.AddMorePilots();
            }
        }

        private void UpdateResearch()
        {
            // Research all available tech randomly!
            var tech = (from t in StrategyGame.TechTree[_t].ResearchableItemsNot(ETechType.Construction)
                        orderby t.Cost - t.AmountInvested, t.Id descending
                        select t).Take(5).ToList();
            
            InvestInRandomTech(tech);
        }

        private int _gamePhase = 0;

        private void UpdateBuild()
        {
            var consBuilding = (from c in StrategyGame.TechTree[_t].ResearchableItems(ETechType.Construction)
                                where c.AmountInvested == c.Cost
                                select c).ToList();
            
            var consCanBuild = (from c in StrategyGame.TechTree[_t].ResearchableItems(ETechType.Construction)
                                where c.CanBuild() && c.AmountInvested < c.Cost
                                orderby c.Cost - c.AmountInvested
                                select c).ToList();
            var ourSectors = (from b in StrategyGame.AllBases
                              where b.Active && b.Team == Team && b.CanLaunchShips()
                              select b.SectorId).Distinct().ToList();

            // Build cons randomly
            // Early/Late build up to 5 cons at once, always a refinery if we can
            if (_gamePhase == 0 || _gamePhase == 2)
            {   
                if (consBuilding.Count < 5 && consCanBuild.Count > 0)
                {
                    TryToInvestInRefinery(consCanBuild, ourSectors);
                    TryToInvestInBases(consCanBuild, ourSectors);
                    InvestInRandomTech(consCanBuild);
                }

                // Transition once we get or first Tech Base or loose our last
                if (StrategyGame.AllBases.Exists(_ => _.Active && _.Team == Team && _.IsTechBase()))
                {
                    if (_gamePhase == 0) _gamePhase = 1;
                }
                else if (_gamePhase == 2)
                {
                    _gamePhase = 0;
                }
            }

            // Mid (researching) - only build 1 con at a time
            else if (_gamePhase == 1)
            {
                if (consBuilding.Count < 1)
                {
                    TryToInvestInRefinery(consCanBuild, ourSectors);
                    InvestInRandomTech(consCanBuild);
                }

                // Transition once we can't research anymore
                if (StrategyGame.TechTree[_t].ResearchableItemsNot(ETechType.Construction).Count == 0)
                {
                    _gamePhase = 2;
                }
            }
        }

        private void TryToInvestInBases(List<TechItem> consCanBuild, List<int> ourSectors)
        {
            if (ourSectors.Count == StrategyGame.Map.Sectors.Count) return;

            var techCons = consCanBuild.Where(_ => _.Name.Contains("Expansion") || _.Name.Contains("Supremacy") || _.Name.Contains("Tactical") || _.Name.Contains("Shipyard")).ToList();
            var baseCons = consCanBuild.Where(_ => _.Name.Contains("Outpost") || _.Name.Contains("Starbase")).ToList();
            if (techCons.Count == 0 && baseCons.Count == 0) return;

            // Build an Outpost/Starbase/Techbase/Shipyard
            TechItem con = null;
            if (!_flagBuiltTech && techCons.Count > 0)
            {
                con = techCons[StrategyGame.Random.Next(techCons.Count)];
            }
            else
            {
                con = (StrategyGame.RandomChance(0.5f) && baseCons.Count > 0) ? baseCons[0] : techCons[0];
            }
            if (con == null) return;

            var remaining = con.Cost - con.AmountInvested;
            if (remaining <= 0) return;

            var invested = StrategyGame.SpendCredits(Team, remaining);
            con.AmountInvested += invested;
        }

        private void TryToInvestInRefinery(List<TechItem> consCanBuild, List<int> ourSectors)
        {            
            var con = consCanBuild.FirstOrDefault(_ => _.Name.Contains("Refinery"));
            if (con == null) return;

            var numResourceRocks = StrategyGame.ResourceAsteroids.Count(_ => _.Active && _.VisibleToTeam[_t] && ourSectors.Contains(_.SectorId));
            if (numResourceRocks == 0) return;
            
            // Build a refinery!
            var remaining = con.Cost - con.AmountInvested;
            if (remaining <= 0) return;

            var invested = StrategyGame.SpendCredits(Team, remaining);
            con.AmountInvested += invested;
        }

        private void InvestInRandomTech(List<Tech.TechItem> items)
        {
            var tech = items[StrategyGame.Random.Next(items.Count)];
            if (tech.Completed || !tech.Active) return;

            var remaining = tech.Cost - tech.AmountInvested;
            if (remaining <= 0) return;

            var invested = StrategyGame.SpendCredits(Team, remaining);
            tech.AmountInvested += invested;
        }
    }
}
