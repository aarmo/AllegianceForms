﻿using System;
using System.Linq;

namespace AllegianceForms.Engine.Tech
{
    public class TechItem
    {
        public int Id { get; set; }
        public ETechType Type { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int AmountInvested { get; set; }
        public bool Completed { get; set; }
        public bool Active { get; set; }
        public bool Unlocked { get; set; }
        public string PreReqsIds { get; set; }
        public int[] DependsOnIds { get; set; }
        public int Team { get; set; }
        public int ResearchedTicks { get; set; }
        public int DurationTicks { get; set; }
        public string Icon { get; set; }
        public string ShortcutKey { get; set; }

        private StrategyGame _game;

        public void Initialise(StrategyGame game, int team)
        {
            Active = true;
            Team = team;
            _game = game;

            if (!string.IsNullOrEmpty(PreReqsIds))
            {
                DependsOnIds = PreReqsIds.Split('|').Select(int.Parse).ToArray();
            }
        }
        
        public void Update()
        {
            if (Completed || AmountInvested == 0) return;

            // Each second increment if researching!
            var investedPerc = 1.0 * AmountInvested / Cost;
            var researchPerc = 1.0 * ResearchedTicks / DurationTicks;

            if (researchPerc < investedPerc)
            {
                ResearchedTicks++;
            }

            if (ResearchedTicks >= DurationTicks && AmountInvested >= Cost)
            {
                Completed = true;
            }
        }

        public void Reset()
        {
            AmountInvested = 0;
            ResearchedTicks = 0;
            Completed = false;
            Active = true;
            Unlocked = false;
        }

        public bool CanBuild()
        {
            if (_game == null) return true;

            return (Type != ETechType.Construction && Type != ETechType.ShipyardConstruction)
                || (Type == ETechType.ShipyardConstruction && Name.Contains("Shipyard"))
                || (Type == ETechType.ShipyardConstruction && !Name.Contains("Shipyard") && _game.NumberOfActiveShips(Team, Name) < _game.Faction[Team-1].CapitalMaxDrones)
                || (Name.Contains("Miner") && _game.NumberOfMinerDrones(Team) < _game.GameSettings.MinersMaxDrones)
                || (Name.Contains("Drone") && _game.NumberOfActiveShips(Team, Name) < _game.GameSettings.MaximumPilots)
                || (Name.Contains("Tower") && _game.NumberOfConstructionDrones(Name, Team) < _game.GameSettings.ConstructorsMaxTowerDrones)
                || (Name.Contains("Constructor") && _game.NumberOfConstructionDrones(Name, Team) < _game.GameSettings.ConstructorsMaxDrones
                    && _game.AllAsteroids.Count(_ => _.IsVisibleToTeam(Team - 1) && _.Type == GetAsteroidType(Name)) > 0);
        }

        public static EBaseType GetBaseType(string name)
        {
            const string conName = "Constructor";
            if (!name.Contains(conName)) return EBaseType.None;

            var bName = name.Replace(conName, string.Empty).Replace(" ", string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(bName)) return EBaseType.None;

            return (EBaseType)Enum.Parse(typeof(EBaseType), bName);
        }

        public static EAsteroidType GetAsteroidType(string name)
        {
            if (name.Contains("Expansion")) return EAsteroidType.Uranium;
            if (name.Contains("Tactical")) return EAsteroidType.Silicon;
            if (name.Contains("Supremacy")) return EAsteroidType.Carbon;

            return EAsteroidType.Generic;
        }

        public static bool IsGlobalUpgrade(string name)
        {
            return name.Contains("%") && (name.Contains("+") || name.Contains("-"));
        }

        public static EGlobalUpgrade GetGlobalUpgradeType(string name)
        {
            var start = name.IndexOfAny(new[] {'+', '-'});

            var n = name.Substring(0, start).Replace(" ", string.Empty).Trim();
            return (EGlobalUpgrade)Enum.Parse(typeof(EGlobalUpgrade), n);
        }

        public static float GetGlobalUpgradeAmount(string name)
        {
            return Convert.ToSingle(name.Substring(name.Length - 4).Replace("%", string.Empty).Trim()) / 100;
        }

        public void ApplyGlobalUpgrade(TechTree tech)
        {
            var type = GetGlobalUpgradeType(Name);
            var amount = GetGlobalUpgradeAmount(Name);

            tech.ResearchedUpgrades[type] = 1 + amount;
        }

        public bool IsConstructionType()
        {
            return Type == ETechType.Construction || (Type == ETechType.ShipyardConstruction && !Name.Contains("Shipyard"));
        }

        static readonly string[] ShipTypes = { "Scout", "Fighter", "Gunship", "Bomber", "Interceptor", "Transport" };

        public bool IsShipType()
        {
            return ShipTypes.Any(_ => Name.Contains(_));
        }
    }
}
