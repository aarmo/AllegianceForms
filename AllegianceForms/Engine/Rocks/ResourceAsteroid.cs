using System;

namespace AllegianceForms.Engine.Rocks
{

    public class ResourceAsteroid : Asteroid
    {
        public bool BeingMined { get; set; }
        public static new string[] Images = new[] { "helium_1.png", "helium_2.png", "helium_3.png", "helium_4.png" };

        public int AvailableResources { get; set; }
        public const int MaxResources = 500;

        public ResourceAsteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : base(game, StrategyGame.RockPicDir + Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.Resource;
            AvailableResources = (int)(MaxResources * _game.GameSettings.ResourcesPerRockMultiplier);
        }

        public int Mine(int amount)
        {
            if (AvailableResources - amount < 0)
            {
                    amount = AvailableResources;
            }

            AvailableResources -= amount;
            return amount;
        }

        public void Regenerate(int amount)
        {
            if (AvailableResources == MaxResources) return;

            if (AvailableResources + amount < MaxResources)
            {
                AvailableResources += amount;
            }
            else
            {
                AvailableResources = MaxResources;
            }
        }
    }
}
