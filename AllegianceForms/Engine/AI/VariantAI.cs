using AllegianceForms.Engine.AI.Missions;
using AllegianceForms.Engine.Ships;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AllegianceForms.Engine.Tech;

namespace AllegianceForms.Engine.AI
{
    public class VariantAI : BaseAI
    {
        private ScoutingMission _scouting;
        private MinerOffenseMission _minerOffense;
        private MinerMission _mining;
        private MinerDefenseMission _minerDefense;
        private BuilderMission _building;
        private BombingMission _baseOffense;
        private BaseDefenseMission _baseDefense;

        private bool _flagBuiltTech;
        private bool _flagFoundEnemyBase;
        private bool _flagFoundEnemyBombers;
        private bool _flagHaveBombers;
        
        public VariantAI(StrategyGame game, int team, Color teamColour, Ship.ShipEventHandler shipHandler) : base(game, team, teamColour, shipHandler)
        {
            _scouting = new ScoutingMission(game, this, _shipHandler);
            _minerOffense = new MinerOffenseMission(game, this, shipHandler);
            _mining = new MinerMission(game, this, shipHandler);
            _building = new BuilderMission(game, this, shipHandler);
            _baseOffense = new BombingMission(game, this, shipHandler);
            _minerDefense = new MinerDefenseMission(game, this, shipHandler);
            _baseDefense = new BaseDefenseMission(game, this, shipHandler);
        }
        
        public override void Update()
        {
            if (!Enabled) return;
            _nextActionAllowed--;
            if (_nextActionAllowed > 0) return;
            _nextActionAllowed = _limitActionsTickDelay;

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
            _mining.UpdateMission();
            _minerOffense.UpdateMission();
            _baseOffense.UpdateMission();
            _building.UpdateMission();
            _scouting.UpdateMission();
            _minerDefense.UpdateMission();
            _baseDefense.UpdateMission();

            var idleShips = _game.AllUnits.Where(_ => _.Active && !_.Docked && _.Team == Team && _.CurrentOrder == null && _.Type != EShipType.Constructor && _.Type != EShipType.Miner && _.Type != EShipType.Lifepod).ToList();
            if (_game.DockedPilots[_t] == 0 && idleShips.Count == 0) return;

            if (!_flagHaveBombers || !_flagFoundEnemyBase || !_flagBuiltTech || !_flagFoundEnemyBombers)
            {
                _flagFoundEnemyBase = _flagFoundEnemyBase || _game.AllBases.Exists(_ => _.VisibleToTeam[_t] && _.Active && _.Alliance != Alliance);
                _flagHaveBombers = _flagHaveBombers || _game.TechTree[_t].HasResearchedShipType(EShipType.Bomber);
                _flagBuiltTech = _flagBuiltTech || _game.AllBases.Exists(_ => _.Active && _.Team == Team && _.IsTechBase());
                _flagFoundEnemyBombers = _flagFoundEnemyBombers || _game.AllUnits.Exists(_ => _.Active && _.VisibleToTeam[_t] && _.Alliance != Alliance && _.CanAttackBases());
            }

            var addedPilots = true;

            if (_flagHaveBombers && _flagFoundEnemyBase)
            {
                if (idleShips.Count > 0)
                {
                    _baseOffense.IncludedShips.AddRange(idleShips);
                    idleShips.Clear();
                }

                while (addedPilots && _game.DockedPilots[_t] > 0 && _baseOffense.RequireMorePilots())
                {
                    addedPilots = _baseOffense.AddMorePilots();
                }
            }
            
            if (_flagFoundEnemyBombers)
            {
                if (_game.DockedPilots[_t] == 0) _minerDefense.ReducePilots(0.5f);
                if (idleShips.Count > 0)
                {
                    _baseDefense.IncludedShips.AddRange(idleShips);
                    idleShips.Clear();
                }

                addedPilots = true;
                while (addedPilots && _game.DockedPilots[_t] > 0 && _baseDefense.RequireMorePilots())
                {
                    addedPilots = _baseDefense.AddMorePilots();
                }
            }

            if (_flagFoundEnemyBase)
            {
                if (_game.DockedPilots[_t] == 0) _minerDefense.ReducePilots(0.5f);
                if (idleShips.Count > 0)
                {
                    _minerOffense.IncludedShips.AddRange(idleShips);
                    idleShips.Clear();
                }

                addedPilots = true;
                while (addedPilots && _game.DockedPilots[_t] > 0 && _minerOffense.RequireMorePilots())
                {
                    addedPilots = _minerOffense.AddMorePilots();
                }
            }

            addedPilots = true;
            while (addedPilots && _game.DockedPilots[_t] > 0 && _scouting.RequireMorePilots())
            {
                addedPilots = _scouting.AddMorePilots();
            }

            if (_building.IncludedShips.Count > 0 || _mining.IncludedShips.Count > 0)
            {
                addedPilots = true;
                while (addedPilots && _game.DockedPilots[_t] > 0 && _minerDefense.RequireMorePilots())
                {
                    addedPilots = _minerDefense.AddMorePilots();
                }
            }
        }

        private void UpdateResearch()
        {
            if (!_flagBuiltTech || _game.Credits[_t] <= 100) return;

            // Research all available tech randomly!
            var tech = (from t in _game.TechTree[_t].ResearchableItemsNot(ETechType.Construction)
                        where t.CanBuild()
                        orderby t.Cost - t.AmountInvested, t.Id descending
                        select t).Take(3).ToList();
            
            if (!_flagHaveBombers)
            {
                var bbr = (from t in _game.TechTree[_t].TechItems
                           where t.Active
                           && !t.Completed
                           && t.Name.Contains("Bombers")
                           select t).FirstOrDefault();
                if (bbr != null) tech.Add(bbr);
            }

            InvestInRandomTech(tech);
        }

        private int _gamePhase = 0;

        private void UpdateBuild()
        {
            if (_game.Credits[_t] <= 100) return;

            var consBuilding = (from c in _game.TechTree[_t].ResearchableItems(ETechType.Construction)
                                where c.AmountInvested == c.Cost
                                select c).ToList();
            
            var consCanBuild = (from c in _game.TechTree[_t].ResearchableItems(ETechType.Construction)
                                where c.CanBuild() && c.AmountInvested < c.Cost
                                orderby c.Cost - c.AmountInvested, c.Id descending
                                select c).ToList();

            var ourSectors = (from b in _game.AllBases
                              where b.Active && b.Team == Team && b.CanLaunchShips()
                              select b.SectorId).Distinct().ToList();

            // Build cons randomly
            // Early/Late build up to 5 cons at once, always a refinery if we can
            if (_gamePhase == 0 || _gamePhase == 2)
            {   
                if (consBuilding.Count < 5 && consCanBuild.Count > 0)
                {
                    TryToInvestInResources(consCanBuild, ourSectors);
                    TryToInvestInBases(consCanBuild, ourSectors);
                    InvestInRandomTech(consCanBuild);
                }

                // Transition once we get or first Tech Base or loose our last
                if (_flagBuiltTech)
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
                    TryToInvestInResources(consCanBuild, ourSectors);
                    TryToInvestInBases(consCanBuild, ourSectors);
                }

                // Transition once we can't research anymore
                if (_game.TechTree[_t].ResearchableItemsNot(ETechType.Construction).Count == 0)
                {
                    _gamePhase = 2;
                }
            }
        }

        private void TryToInvestInBases(List<TechItem> consCanBuild, List<int> ourSectors)
        {
            if (ourSectors.Count >= _game.Map.Sectors.Count / 2) return;

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
                if (StrategyGame.RandomChance(0.5f) && baseCons.Count > 0)
                    con = baseCons[0];
                else if (techCons.Count > 0)
                    con = techCons[0];
            }
            if (con == null) return;

            var remaining = con.Cost - con.AmountInvested;
            if (remaining <= 0) return;

            var invested = _game.SpendCredits(Team, remaining);
            con.AmountInvested += invested;
        }

        private void TryToInvestInResources(List<TechItem> consCanBuild, List<int> ourSectors)
        {            
            var con = consCanBuild.FirstOrDefault(_ => _.Name.Contains("Resource") || _.Name.Contains("Miner"));
            if (con == null) return;

            var numResourceRocks = _game.ResourceAsteroids.Count(_ => _.Active && _.VisibleToTeam[_t] && ourSectors.Contains(_.SectorId));
            if (numResourceRocks == 0) return;
            
            // Build a refinery!
            var remaining = con.Cost - con.AmountInvested;
            if (remaining <= 0) return;

            var invested = _game.SpendCredits(Team, remaining);
            con.AmountInvested += invested;
        }

        private void InvestInRandomTech(List<TechItem> items)
        {
            if (items.Count == 0) return;
            var tech = items[StrategyGame.Random.Next(items.Count)];

            if (tech.Completed || !tech.Active) return;

            var remaining = tech.Cost - tech.AmountInvested;
            if (remaining <= 0) return;

            var invested = _game.SpendCredits(Team, remaining);
            tech.AmountInvested += invested;
        }
    }
}
