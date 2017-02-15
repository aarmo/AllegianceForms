using System;

namespace AllegianceForms.Engine.Rocks
{

    public class ResourceAsteroid : Asteroid
    {
        public bool BeingMined { get; set; }
        private static string[] _images = new[] { ".\\Art\\Rocks\\helium_1.png", ".\\Art\\Rocks\\helium_2.png", ".\\Art\\Rocks\\helium_3.png", ".\\Art\\Rocks\\helium_4.png" };

        public int AvailableResources { get; set; }
        private int _maxResources = 500;

        public ResourceAsteroid(Random r, int width, int height, int sectorId)
            : base(_images[r.Next(0, _images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.Resource;
            AvailableResources = _maxResources;
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
    }
}
