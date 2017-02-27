using AllegianceForms.AI.Missions;
using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Engine.Tech;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.AI
{

    public class CommanderAI
    {
        public int Team { get; set; }
        public Color TeamColour { get; set; }
        public Dictionary<EAiCreditPriorities, float> CreditPriorities { get; private set; }
        public Dictionary<EAiPilotPriorities, float> PilotPriorities { get; private set; }
        public bool ForceVisible { get; set; }
        public bool Enabled { get; set; }
        public bool CheatVisibility { get; set; }
        public bool CheatCredits { get; set; }
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

        private int _t;

        public const float DegradeAmount = 0.98f;
        public const float MaxPriorityValue = 100;
        public const float MinActionAmount = 0.3f;
        public const int CheatCreditAmout = 3;

        public float CheatAdditionalPilots = 1.5f;
        private float _cheatCreditsChance = 0.05f;        
        private int _cheatCreditsLastsSeconds = 10;
        private float _cheatVisibilityChance = 0.025f;
        private int _cheatVisibilityLastsSeconds = 5;
        private DateTime _cheatVisibilityExpires = DateTime.MinValue;
        private DateTime _cheatCreditExpires = DateTime.MinValue;

        private float _scoutFocus = 1;
        private float _baseDefenseFocus = 8;
        private float _minerOffenseFocus = 8;
        private float _minerDefenseFocus = 8;

        public CommanderAI(int team, Color teamColour, Sector ui, bool randomise = false)
        {
            Team = team;
            _t = team - 1;
            TeamColour = teamColour;

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

            Enabled = true;
            Scouting = ScoutingMission = true;
            MinerOffence = MinerOffenceMission = true;
            MinerDefence = MinerDefenceMission = true;
            BaseDefence = BaseDefenceMission = true;
            BaseOffence = BaseOffenceMission = true;
            MiningMission = true;
            BuildingMission = true;
            CreditsForDefence = CreditsForOffence = CreditsForExpansion = true;

            _scouting = new ScoutingMission(this, ui);
            _building = new BuilderMission(this, ui);
            _mining = new MinerMission(this, ui);
            _bombing = new BombingMission(this, ui);
            _defense = new BaseDefenseMission(this, ui);
            _minerO = new MinerOffenseMission(this, ui);
            _minerD = new MinerDefenseMission(this, ui);

            if (randomise)
            {
                var rnd = StrategyGame.Random;
                _scoutFocus = rnd.Next(40, 100) / 10f;
                _minerOffenseFocus = rnd.Next(40, 100) / 10f;
                _minerDefenseFocus = rnd.Next(40, 100) / 10f;
            }
        }

        public void SetDifficulty(int i)
        {
            _cheatCreditsChance = 0.025f*i;
            _cheatCreditsLastsSeconds = 5*i;
            _cheatVisibilityChance = 0.005f*i;
            _cheatVisibilityLastsSeconds = 2*i;
            CheatAdditionalPilots = 1 + (0.25f * i);
        }
        
        public void Update()
        {
            if (!Enabled) return;
            UpdatePriorities();
            UpdateCheats();
            if (CheatCredits) StrategyGame.AddResources(Team, CheatCreditAmout);
            
            // Add more pilots to missions
            foreach (var v in PilotPriorities.OrderByDescending(_ => _.Value))
            {
                if (StrategyGame.DockedPilots[_t] == 0 || v.Value < MinActionAmount)
                    break;

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
                        }
                        break;
                    case EAiPilotPriorities.BaseDefense:
                        if (BaseDefence && _defense.RequireMorePilots())
                        {
                            _defense.AddMorePilots();
                        }
                        break;
                    case EAiPilotPriorities.MinerOffense:
                        if (MinerOffence && _minerO.RequireMorePilots())
                        {
                            _minerO.AddMorePilots();
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
                    var tech = StrategyGame.TechTree[_t].ResearchableItemsNot(ETechType.Construction).OrderByDescending(_ => _.Id).ToList();
                    var cons = StrategyGame.TechTree[_t].ResearchableItems(ETechType.Construction).OrderByDescending(_ => _.Id).ToList();
                    var funds = StrategyGame.Credits[_t];
                    
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
                                ((_.Type == ETechType.Starbase) || (_.Type == ETechType.Tactical && hasTacTech) || (_.Type == ETechType.Expansion && hasExpTech) || (_.Type == ETechType.Supremacy && hasSupTech)
                                ));
                        }
                        if (c.Key == EAiCreditPriorities.Defense || NextTech == null)
                        {
                            NextTech = tech.FirstOrDefault(_ => !_.Name.Contains("Bomber") && _.AmountInvested < _.Cost &&
                                ((_.Type == ETechType.Starbase)
                                    || (_.Type == ETechType.ShipyardConstruction && hasCapTech)
                                    || (_.Type == ETechType.Supremacy && hasSupTech)
                                    || (_.Type == ETechType.Tactical && hasTacTech) 
                                    || (_.Type == ETechType.Expansion && hasExpTech)
                                ));
                        }

                        if (NextTech != null)
                        {
                            var invested = StrategyGame.SpendCredits(Team, NextTech.Cost);
                            NextTech.AmountInvested += invested;
                        }
                    }
                }
            }
        }

        private void UpdateCheats()
        {
            if (StrategyGame.Random.NextDouble() <= _cheatVisibilityChance)
            {
                CheatVisibility = true;
                _cheatVisibilityExpires = DateTime.Now.AddSeconds(_cheatVisibilityLastsSeconds);
            }
            if (CheatVisibility && DateTime.Now > _cheatVisibilityExpires)
            {
                CheatVisibility = false;
            }

            if (StrategyGame.Random.NextDouble() <= _cheatCreditsChance)
            {
                CheatCredits = true;
                _cheatCreditExpires = DateTime.Now.AddSeconds(_cheatCreditsLastsSeconds);
            }
            if (CheatCredits && DateTime.Now > _cheatCreditExpires)
            {
                CheatCredits = false;
            }
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

            var enemyShips = StrategyGame.AllUnits.Where(_ => _.Active && _.Team != Team && _.VisibleToTeam[_t]).ToList();
            Miners = StrategyGame.AllUnits.Where(_ => _.Active && _.Team == Team && _.Type == EShipType.Miner).ToList();
            Builders = StrategyGame.AllUnits.Where(_ => _.Active && _.Team == Team && _.Type == EShipType.Constructor).ToList();

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

            var enemyBases = StrategyGame.AllBases.Where(_ => _.Active && _.Team != Team && _.VisibleToTeam[_t]).ToList();
            Bases = StrategyGame.AllBases.Where(_ => _.Active && _.Team == Team).ToList();
            if (!StrategyGame.TechTree[_t].HasResearchedShipType(EShipType.Bomber))
            {
                if (CreditsForOffence && CreditPriorities[EAiCreditPriorities.Offense] < MaxPriorityValue) CreditPriorities[EAiCreditPriorities.Offense] += (enemyBases.Count * _baseDefenseFocus);
            }
            else
            {
                if (BaseOffence && PilotPriorities[EAiPilotPriorities.BaseOffense] < MaxPriorityValue) PilotPriorities[EAiPilotPriorities.BaseOffense] += (enemyBases.Count * _baseDefenseFocus);
            }

            var numOwnedSectors = StrategyGame.Map.Sectors.Count(s => Bases.Any(b => b.SectorId == s.Id));
            if (CreditsForExpansion && CreditPriorities[EAiCreditPriorities.Expansion] < MaxPriorityValue) CreditPriorities[EAiCreditPriorities.Expansion] += StrategyGame.Map.Sectors.Count - numOwnedSectors;
        }
    }
}
