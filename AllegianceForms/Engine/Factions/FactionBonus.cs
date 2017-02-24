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
        public float Regneration { get; set; }
        public float MiningSpeed { get; set; }
        public float MiningCapacity { get; set; }

        public void IncreaseRandomBonus(int inc)
        {
            var amount = inc / 10f;

            switch (StrategyGame.Random.Next(10))
            {
                case 0:
                    ResearchTime += amount;
                    break;
                case 1:
                    ResearchCost += amount;
                    break;
                case 2:
                    Speed += amount;
                    break;
                case 3:
                    ScanRange += amount;
                    break;
                case 4:
                    Signature += amount;
                    break;
                case 5:
                    FireRate += amount;
                    break;
                case 6:
                    Regneration += amount;
                    break;
                case 7:
                    MiningSpeed += amount;
                    break;
                case 8:
                    MiningCapacity += amount;
                    break;
                case 9:
                    Health += amount;
                    break;
            }
        }

        public float TotalBonus
        {
            get
            {
                return ResearchTime + ResearchCost + Speed + ScanRange + Signature + FireRate + Regneration + MiningSpeed + MiningCapacity + Health;
            }
        }
        
        public void Reset()
        {
            ResearchTime = 0;
            ResearchCost = 0;
            Speed = 0;
            ScanRange = 0;
            Signature = 0;
            FireRate = 0;
            Regneration = 0;
            MiningSpeed = 0;
            MiningCapacity = 0;
            Health = 0;
        }

        public bool IsBalanced()
        {
            return Math.Round(TotalBonus) == 0;
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
