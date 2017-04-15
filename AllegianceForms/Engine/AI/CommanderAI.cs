using AllegianceForms.Engine.AI.Missions;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Engine.Tech;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.AI
{
    public class CommanderAI : BaseAI
    {
        public Dictionary<EAiCreditPriorities, float> CreditPriorities { get; private set; }
        public Dictionary<EAiPilotPriorities, float> PilotPriorities { get; private set; }
        public bool Scouting { get; set; }
        public bool MinerOffence { get; set; }
        public bool MinerDefence{ get; set; }
        public bool BaseOffence { get; set; }
        public bool BaseDefence { get; set; }
        public bool CreditsForExpansion { get; set; }
        public bool CreditsForOffence { get; set; }
        public bool CreditsForDefence { get; set; }
        public bool ScoutingMission { get; set; }
        public bool BuildingMission { get; set; }
        public bool MiningMission { get; set; }
        public bool MinerOffenceMission { get; set; }
        public bool MinerDefenceMission { get; set; }
        public bool BaseOffenceMission { get; set; }
        public bool BaseDefenceMission { get; set; }
        public List<Ship> Miners { get; set; }
        public List<Ship> Builders{ get; set; }
        public List<Base> Bases { get; set; }
        public TechItem NextTech { get; set; }

        internal ScoutingMission _scouting;
        internal BuilderMission _building;
        internal MinerMission _mining;
        internal BombingMission _bombing;
        internal BaseDefenseMission _defense;
        internal MinerOffenseMission _minerO;
        internal MinerDefenseMission _minerD;
        
        public const float DegradeAmount = 0.98f;
        public const float MaxPriorityValue = 100;
        public const float MinActionAmount = 0.3f;

        private float _scoutFocus = 1;
        private float _baseDefenseFocus = 8;
        private float _minerOffenseFocus = 8;
        private float _minerDefenseFocus = 8;

        public CommanderAI(StrategyGame game, int team, Color teamColour, Ship.ShipEventHandler shipHandler, bool randomise = false) : base(game, team, teamColour, shipHandler)
        {
            CreditPriorities = new Dictionary<EAiCreditPriorities, float>();
            foreach (var e in (EAiCreditPriorities[])Enum.GetValues(typeof(EAiCreditPriorities)))
            {
                CreditPriorities.Add(e, 0);
            }

            PilotPriorities = new Dictionary<EAiPilotPriorities, float>();
            foreach (var e in (EAiPilotPriorities[])Enum.GetValues(typeof(EAiPilotPriorities)))
            {
                PilotPriorities.Add(e, 0);
            }

            Scouting = ScoutingMission = true;
            MinerOffence = MinerOffenceMission = true;
            MinerDefence = MinerDefenceMission = true;
            BaseDefence = BaseDefenceMission = true;
            BaseOffence = BaseOffenceMission = true;
            MiningMission = true;
            BuildingMission = true;
            CreditsForDefence = CreditsForOffence = CreditsForExpansion = true;

            _scouting = new ScoutingMission(_game, this, _shipHandler);
            _building = new BuilderMission(_game, this, _shipHandler);
            _mining = new MinerMission(_game, this, _shipHandler);
            _bombing = new BombingMission(_game, this, _shipHandler);
            _defense = new BaseDefenseMission(_game, this, _shipHandler);
            _minerO = new MinerOffenseMission(_game, this, _shipHandler);
            _minerD = new MinerDefenseMission(_game, this, _shipHandler);

            if (randomise)
            {
                var rnd = StrategyGame.Random;
                _scoutFocus = rnd.Next(40, 100) / 10f;
                _minerOffenseFocus = rnd.Next(40, 100) / 10f;
                _baseDefenseFocus = rnd.Next(40, 100) / 10f;
                _minerDefenseFocus = rnd.Next(40, 100) / 10f;
            }
        }
        
        public override void Update()
        {
            if (!Enabled) return;
            _nextActionAllowed--;
            if (_nextActionAllowed > 0) return; 

            UpdatePriorities();
            UpdateCheats();

            var idleShips = _game.AllUnits.Where(_ => _.Active && !_.Docked && _.Team == Team && _.CurrentOrder == null && _.Type != EShipType.Constructor && _.Type != EShipType.Miner && _.Type != EShipType.Lifepod).ToList();
            
            // Add more pilots to missions
            foreach (var v in PilotPriorities.OrderByDescending(_ => _.Value))
            {
                if (v.Value < MinActionAmount) break;

                switch (v.Key)
                {
                    case EAiPilotPriorities.Scout:
                        if (Scouting && _scouting.RequireMorePilots())
                        {
                            _scouting.AddMorePilots();
                        }
                        break;
                    case EAiPilotPriorities.BaseOffense:
                        if (BaseOffence && _bombing.RequireMorePilots())
                        {
                            _bombing.AddMorePilots();
                            if (idleShips.Count > 0)
                            {
                                _bombing.IncludedShips.AddRange(idleShips);
                                idleShips.Clear();
                            }
                        }
                        break;
                    case EAiPilotPriorities.MinerOffense:
                        if (MinerOffence && _minerO.RequireMorePilots())
                        {
                            _minerO.AddMorePilots();
                            if (idleShips.Count > 0)
                            {
                                _bombing.IncludedShips.AddRange(idleShips);
                                idleShips.Clear();
                            }
                        }
                        break;
                    case EAiPilotPriorities.BaseDefense:
                        if (BaseDefence && _defense.RequireMorePilots())
                        {
                            _defense.AddMorePilots();
                        }
                        break;
                    case EAiPilotPriorities.MinerDefense:
                        if (MinerDefence && _minerD.RequireMorePilots())
                        {
                            _minerD.AddMorePilots();
                        }
                        break;
                }
            }
            UpdateMissions();

            // Control purchases
            if (CreditsForExpansion || CreditsForDefence || CreditsForOffence)
            {
                var c = CreditPriorities
                    .Where(_ => (_.Key == EAiCreditPriorities.Expansion && CreditsForExpansion) ||
                                (_.Key == EAiCreditPriorities.Offense && CreditsForOffence) ||
                                (_.Key == EAiCreditPriorities.Defense && CreditsForDefence))
                    .OrderByDescending(_ => _.Value).FirstOrDefault();
                if (!c.IsDefault())
                {
                    var tech = _game.TechTree[_t].ResearchableItemsNot(ETechType.Construction).OrderByDescending(_ => _.Id).ToList();
                    var cons = _game.TechTree[_t].ResearchableItems(ETechType.Construction).OrderByDescending(_ => _.Id).ToList();
                    var funds = _game.Credits[_t];
                    
                    var hasExpTech = Bases.Any(_ => _.Active && _.Team == Team && _.Type == EBaseType.Expansion);
                    var hasSupTech = Bases.Any(_ => _.Active && _.Team == Team && _.Type == EBaseType.Supremacy);
                    var hasTacTech = Bases.Any(_ => _.Active && _.Team == Team && _.Type == EBaseType.Tactical);
                    var hasCapTech = Bases.Any(_ => _.Active && _.Team == Team && _.Type == EBaseType.Shipyard);

                    if (funds > 0)
                    {
                        NextTech = null;
                        if (c.Key == EAiCreditPriorities.Expansion)
                        {
                            NextTech = cons.FirstOrDefault(_ => _.CanBuild() && _.AmountInvested < _.Cost);
                        }
                        if (c.Key == EAiCreditPriorities.Offense || (c.Key == EAiCreditPriorities.Expansion && NextTech == null))
                        {
                            NextTech = tech.FirstOrDefault(_ => _.Name.Contains("Bomber") && _.AmountInvested < _.Cost && 
                                ((_.Type == ETechType.Starbase)
                                || (_.Type == ETechType.Tactical && hasTacTech) 
                                || (_.Type == ETechType.Expansion && hasExpTech) 
                                || (_.Type == ETechType.Supremacy && hasSupTech)
                                ));
                        }
                        if (c.Key == EAiCreditPriorities.Defense || NextTech == null)
                        {
                            NextTech = tech.FirstOrDefault(_ => !_.Name.Contains("Bomber") && _.AmountInvested < _.Cost &&
                                ((_.Type == ETechType.Starbase)
                                    || (_.Type == ETechType.ShipyardConstruction && hasCapTech && _.CanBuild())
                                    || (_.Type == ETechType.Supremacy && hasSupTech)
                                    || (_.Type == ETechType.Tactical && hasTacTech) 
                                    || (_.Type == ETechType.Expansion && hasExpTech)
                                ));
                        }

                        if (NextTech != null)
                        {
                            var invested = _game.SpendCredits(Team, NextTech.Cost);
                            NextTech.AmountInvested += invested;
                        }
                    }
                }
            }

            _nextActionAllowed = _limitActionsTickDelay;
        }
        
        private void UpdateMissions()
        {
            if (MiningMission) _mining.UpdateMission();
            if (ScoutingMission) _scouting.UpdateMission();
            if (BuildingMission) _building.UpdateMission();

            if (BaseDefenceMission) _defense.UpdateMission();
            if (MinerDefenceMission) _minerD.UpdateMission();

            if (BaseOffenceMission) _bombing.UpdateMission();
            if (MinerOffenceMission) _minerO.UpdateMission();
        }

        private void UpdatePriorities()
        {
            // Degrade things a little
            foreach (var e in (EAiCreditPriorities[])Enum.GetValues(typeof(EAiCreditPriorities)))
            {
                CreditPriorities[e] *= DegradeAmount;
            }
            foreach (var e in (EAiPilotPriorities[])Enum.GetValues(typeof(EAiPilotPriorities)))
            {
                PilotPriorities[e] *= DegradeAmount;
            }

            if (Scouting && PilotPriorities[EAiPilotPriorities.Scout] < MaxPriorityValue)
                PilotPriorities[EAiPilotPriorities.Scout] += _scoutFocus;

            var enemyShips = _game.AllUnits.Where(_ => _.Active && _.Alliance != Alliance && _.VisibleToTeam[_t]).ToList();
            Miners = _game.AllUnits.Where(_ => _.Active && _.Alliance == Alliance && _.Type == EShipType.Miner).ToList();
            Builders = _game.AllUnits.Where(_ => _.Active && _.Alliance == Alliance && _.Type == EShipType.Constructor).ToList();

            foreach (var s in enemyShips)
            {
                switch (s.Type)
                {
                    case EShipType.Miner:
                    case EShipType.Constructor:
                        if (MinerOffence && PilotPriorities[EAiPilotPriorities.MinerOffense] < MaxPriorityValue) PilotPriorities[EAiPilotPriorities.MinerOffense] += _minerOffenseFocus;
                        if (CreditsForDefence && CreditPriorities[EAiCreditPriorities.Defense] < MaxPriorityValue) CreditPriorities[EAiCreditPriorities.Defense] += _baseDefenseFocus;
                        break;

                    case EShipType.Bomber:
                    case EShipType.FighterBomber:
                    case EShipType.StealthBomber:
                        if (BaseDefence && PilotPriorities[EAiPilotPriorities.BaseDefense] < MaxPriorityValue) PilotPriorities[EAiPilotPriorities.BaseDefense] += _baseDefenseFocus;
                        if (CreditsForDefence && CreditPriorities[EAiCreditPriorities.Defense] < MaxPriorityValue) CreditPriorities[EAiCreditPriorities.Defense] += _baseDefenseFocus;
                        break;

                    case EShipType.Fighter:
                    case EShipType.Interceptor:
                    case EShipType.StealthFighter:
                    case EShipType.Gunship:
                        if (Miners.Count > 0 || Builders.Count > 0)
                            if (MinerDefence && PilotPriorities[EAiPilotPriorities.MinerDefense] < MaxPriorityValue) PilotPriorities[EAiPilotPriorities.MinerDefense] += _minerDefenseFocus;

                        if (CreditsForDefence && CreditPriorities[EAiCreditPriorities.Defense] < MaxPriorityValue) CreditPriorities[EAiCreditPriorities.Defense] += _baseDefenseFocus;
                        break;
                }
            }

            if (MinerDefence && (Miners.Count > 0 || Builders.Count > 0) && PilotPriorities[EAiPilotPriorities.MinerDefense] < MaxPriorityValue)
                PilotPriorities[EAiPilotPriorities.MinerDefense] += _minerDefenseFocus;

            //TODO: Ineffiencient to do each second!
            Bases = _game.AllBases.Where(_ => _.Active && _.Team == Team).ToList();
            var enemyBases = _game.AllBases.Count(_ => _.Active && _.Alliance != Alliance && _.VisibleToTeam[_t]);
            var numOwnedSectors = _game.Map.Sectors.Count(s => Bases.Any(b => b.SectorId == s.Id));

            if (_game.TechTree[_t].HasResearchedShipType(EShipType.Bomber))
            {
                if (BaseOffence && PilotPriorities[EAiPilotPriorities.BaseOffense] < MaxPriorityValue) PilotPriorities[EAiPilotPriorities.BaseOffense] += (enemyBases * _baseDefenseFocus);                
            }

            if (CreditsForOffence && CreditPriorities[EAiCreditPriorities.Offense] < MaxPriorityValue) CreditPriorities[EAiCreditPriorities.Offense] += (enemyBases * _baseDefenseFocus);

            if (CreditsForExpansion && CreditPriorities[EAiCreditPriorities.Expansion] < MaxPriorityValue) CreditPriorities[EAiCreditPriorities.Expansion] += _game.Map.Sectors.Count - numOwnedSectors;
        }
    }
}
