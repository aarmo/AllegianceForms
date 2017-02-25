using System;

namespace AllegianceForms.Engine.Factions
{
    public class FactionBonus
    {
        public float ResearchTime { get; set; }
        public float ResearchCost { get; set; }

        public float Speed { get; set; }
        public float Health { get; set; }

        public float ScanRange { get; set; }
        public float Signature { get; set; }

        public float FireRate { get; set; }
        public float MissileTracking { get; set; }
        public float MissileSpeed { get; set; }

        public float MiningCapacity { get; set; }
        public float MiningEfficiency { get; set; }

        public const int NumBonuses = 11;

        public FactionBonus()
        {
            Reset();
        }

        public void IncreaseRandomBonus(int inc)
        {
            var amount = inc / 10f;

            switch (StrategyGame.Random.Next(NumBonuses))
            {
                case 0:
                    ResearchTime -= amount;
                    break;
                case 1:
                    ResearchCost -= amount;
                    break;
                case 2:
                    Speed += amount;
                    break;
                case 3:
                    Health += amount;
                    break;
                case 4:
                    ScanRange += amount;
                    break;
                case 5:
                    Signature -= amount;
                    break;
                case 6:
                    FireRate += amount;
                    break;
                case 7:
                    MissileTracking += amount;
                    break;
                case 8:
                    MissileSpeed += amount;
                    break;
                case 9:
                    MiningCapacity += amount;
                    break;
                case 10:
                    MiningEfficiency += amount;
                    break;
            }
        }

        public float TotalBonus
        {
            get
            {
                return (1 - ResearchTime)
                    + (1 - ResearchCost)
                    + (Speed - 1)
                    + (ScanRange - 1)
                    + (1 - Signature)
                    + (FireRate - 1)
                    + (MissileSpeed - 1)
                    + (MissileTracking - 1)
                    + (MiningCapacity - 1)
                    + (MiningEfficiency - 1)
                    + (Health - 1);
            }
        }
        
        public void Reset()
        {
            ResearchTime = 1;
            ResearchCost = 1;
            Speed = 1;
            ScanRange = 1;
            Signature = 1;
            FireRate = 1;
            MissileSpeed = 1;
            MissileTracking = 1;
            MiningEfficiency = 1;
            MiningCapacity = 1;
            Health = 1;
        }

        public bool IsBalanced()
        {
            return Math.Round(TotalBonus, 2) == 0;
        }

        public void Randomise(int min)
        {
            Reset();

            var iterations = min + StrategyGame.Random.Next(min);

            for (var i = 0; i < iterations; i++)
            {
                IncreaseRandomBonus(-1);
                IncreaseRandomBonus(1);
            }
        }
    }
}
