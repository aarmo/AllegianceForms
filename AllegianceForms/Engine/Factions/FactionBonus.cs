using System;

namespace AllegianceForms.Engine.Factions
{
    public class FactionBonus
    {
        public const float MaxBonus = 1.5f;
        public const float MinBonus = 0.5f;

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

        public float TotalBonus
        {
            get
            {
                return (1 - ResearchTime)
                    + (1 - ResearchCost)
                    + (Speed - 1)
                    + (Health - 1)
                    + (ScanRange - 1)
                    + (1 - Signature)
                    + (FireRate - 1)
                    + (MissileTracking - 1)
                    + (MissileSpeed - 1)
                    + (MiningCapacity - 1)
                    + (MiningEfficiency - 1);
            }
        }
        
        public void Reset()
        {
            ResearchTime = 1;
            ResearchCost = 1;
            Speed = 1;
            Health = 1;
            ScanRange = 1;
            Signature = 1;
            FireRate = 1;
            MissileTracking = 1;
            MissileSpeed = 1;
            MiningEfficiency = 1;
            MiningCapacity = 1;
        }

        public bool IsBalanced() => Math.Round(TotalBonus, 2) == 0;
        private bool BonusOutsideBounds(float bonus) => bonus < MinBonus || bonus > MaxBonus;

        public void Randomise(int min)
        {
            Reset();

            var iterations = min + StrategyGame.Random.Next(min);

            for (var i = 0; i < iterations; i++)
            {
                if (IsBalanced()) 
                    IncreaseRandomBonus(-1);

                if (Math.Round(TotalBonus, 2) == -0.1) 
                    IncreaseRandomBonus(1);
            }

            if (!IsBalanced()) Reset();
        }

        private void IncreaseRandomBonus(int inc)
        {
            // Increase a random bonus, but not beyond a reasonable min/max bonuses
            // Recommend keeping the number of calls low - there is no infinite loop protection(!) 

            var amount = inc / 10f;

            switch (StrategyGame.Random.Next(NumBonuses))
            {
                // Check and try again if outside the bounds...
                case 0:
                    if (BonusOutsideBounds(ResearchTime - amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    ResearchTime -= amount;
                    break;
                case 1:
                    if (BonusOutsideBounds(ResearchCost - amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    ResearchCost -= amount;
                    break;
                case 2:
                    if (BonusOutsideBounds(Speed + amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    Speed += amount;
                    break;
                case 3:
                    if (BonusOutsideBounds(Health + amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    Health += amount;
                    break;
                case 4:
                    if (BonusOutsideBounds(ScanRange + amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    ScanRange += amount;
                    break;
                case 5:
                    if (BonusOutsideBounds(Signature - amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    Signature -= amount;
                    break;
                case 6:
                    if (BonusOutsideBounds(FireRate + amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    FireRate += amount;
                    break;
                case 7:
                    if (BonusOutsideBounds(MissileTracking + amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    MissileTracking += amount;
                    break;
                case 8:
                    if (BonusOutsideBounds(MissileSpeed + amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    MissileSpeed += amount;
                    break;
                case 9:
                    if (BonusOutsideBounds(MiningCapacity + amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    MiningCapacity += amount;
                    break;
                case 10:
                    if (BonusOutsideBounds(MiningEfficiency + amount))
                    {
                        IncreaseRandomBonus(inc);
                        return;
                    }
                    MiningEfficiency += amount;
                    break;
            }
        }
    }
}
