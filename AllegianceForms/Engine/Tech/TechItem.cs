using System;
using System.Linq;

namespace AllegianceForms.Engine.Tech
{
    public class TechItem
    {
        public int Id { get; set; }
        public ETechType Type { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int DurationSec { get; set; }
        public int AmountInvested { get; set; }
        public float TimeResearched { get; set; }
        public bool Completed { get; set; }
        public bool Active { get; set; }
        public string PreReqsIds { get; set; }
        public int[] DependsOnIds { get; set; }
        public int Team { get; set; }

        public void Initialise(int team)
        {
            Active = true;
            Team = team;

            if (!string.IsNullOrEmpty(PreReqsIds))
            {
                DependsOnIds = PreReqsIds.Split('|').Select(int.Parse).ToArray();
            }
        }

        public void UpdateEachSecond(float ms)
        {
            if (Completed || AmountInvested == 0) return;

            // Each second increment if researching!
            var investedPerc = 1.0 * AmountInvested / Cost;
            var researchPerc = 1.0 * TimeResearched / DurationSec;

            if (researchPerc < investedPerc)
            {
                TimeResearched += ms / 1000;
            }

            if (TimeResearched >= DurationSec && AmountInvested >= Cost)
            {
                Completed = true;
            }
        }

        public void Reset()
        {
            AmountInvested = 0;
            TimeResearched = 0;
            Completed = false;
            Active = true;
        }

        public bool CanBuild()
        {
            return (Type != ETechType.Construction)
                || (Name.Contains("Miner") && StrategyGame.NumberOfMinerDrones(Team) < StrategyGame.GameSettings.MinersMaxDrones)
                || (Name.Contains("Tower") && StrategyGame.NumberOfConstructionDrones(Name, Team) < StrategyGame.GameSettings.ConstructorsMaxTowerDrones)
                || (Name.Contains("Constructor") && StrategyGame.NumberOfConstructionDrones(Name, Team) < StrategyGame.GameSettings.ConstructorsMaxDrones
                    && StrategyGame.AllAsteroids.Count(_ => _.VisibleToTeam[Team - 1] && _.Type == TechItem.GetAsteroidType(Name)) > 0);
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
            if (name.Contains("Expansion")) return EAsteroidType.TechUranium;
            if (name.Contains("Tactical")) return EAsteroidType.TechSilicon;
            if (name.Contains("Supremacy")) return EAsteroidType.TechCarbon;

            return EAsteroidType.Rock;
        }

        public static bool IsGlobalUpgrade(string name)
        {
            return name.Contains("%") && (name.Contains("+") || name.Contains("-"));
        }

        public static EGlobalUpgrade GetGlobalUpgradeType(string name)
        {
            var n = name.Remove(name.Length - 4).Replace(" ", string.Empty).Trim();
            return (EGlobalUpgrade)Enum.Parse(typeof(EGlobalUpgrade), n);
        }

        public void ApplyGlobalUpgrade(TechTree tech)
        {
            var type = GetGlobalUpgradeType(Name);
            var amount = Convert.ToSingle(Name.Substring(Name.Length - 4).Replace("%", string.Empty).Trim()) / 100;

            tech.ResearchedUpgrades[type] = 1 + amount;
        }
    }
}
