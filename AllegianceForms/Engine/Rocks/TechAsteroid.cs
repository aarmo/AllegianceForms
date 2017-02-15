using System;

namespace AllegianceForms.Engine.Rocks
{
    public class TechCarbonAsteroid : Asteroid
    {
        private static string[] _images = new[] { ".\\Art\\Rocks\\carbon_1.png", ".\\Art\\Rocks\\carbon_2.png", ".\\Art\\Rocks\\carbon_3.png", ".\\Art\\Rocks\\carbon_4.png" };

        public TechCarbonAsteroid(Random r, int width, int height, int sectorId)
            : base(_images[r.Next(0, _images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.TechCarbon;
        }
    }

    public class TechSiliconAsteroid : Asteroid
    {
        private static string[] _images = new[] { ".\\Art\\Rocks\\silicon_1.png", ".\\Art\\Rocks\\silicon_2.png", ".\\Art\\Rocks\\silicon_3.png", ".\\Art\\Rocks\\silicon_4.png" };

        public TechSiliconAsteroid(Random r, int width, int height, int sectorId)
            : base(_images[r.Next(0, _images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.TechSilicon;
        }
    }

    public class TechUraniumAsteroid : Asteroid
    {
        private static string[] _images = new[] { ".\\Art\\Rocks\\uranium_1.png", ".\\Art\\Rocks\\uranium_2.png", ".\\Art\\Rocks\\uranium_3.png", ".\\Art\\Rocks\\uranium_4.png" };

        public TechUraniumAsteroid(Random r, int width, int height, int sectorId)
            : base(_images[r.Next(0, _images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.TechUranium;
        }
    }
}
