using System;

namespace AllegianceForms.Engine.Rocks
{
    public class TechCarbonAsteroid : Asteroid
    {
        public static new string[] Images = new[] { ".\\Art\\Rocks\\carbon_1.png", ".\\Art\\Rocks\\carbon_2.png", ".\\Art\\Rocks\\carbon_3.png", ".\\Art\\Rocks\\carbon_4.png" };

        public TechCarbonAsteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : base(game, Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.TechCarbon;
        }
    }

    public class TechSiliconAsteroid : Asteroid
    {
        public static new string[] Images = new[] { ".\\Art\\Rocks\\silicon_1.png", ".\\Art\\Rocks\\silicon_2.png", ".\\Art\\Rocks\\silicon_3.png", ".\\Art\\Rocks\\silicon_4.png" };

        public TechSiliconAsteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : base(game, Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.TechSilicon;
        }
    }

    public class TechUraniumAsteroid : Asteroid
    {
        public static new string[] Images = new[] { ".\\Art\\Rocks\\uranium_1.png", ".\\Art\\Rocks\\uranium_2.png", ".\\Art\\Rocks\\uranium_3.png", ".\\Art\\Rocks\\uranium_4.png" };

        public TechUraniumAsteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : base(game, Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.TechUranium;
        }
    }
}
