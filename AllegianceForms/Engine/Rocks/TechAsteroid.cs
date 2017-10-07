using System;

namespace AllegianceForms.Engine.Rocks
{
    public class TechCarbonAsteroid : Asteroid
    {
        public static new string[] Images = new[] { "carbon_1.png", "carbon_2.png", "carbon_3.png", "carbon_4.png" };

        public TechCarbonAsteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : base(game, StrategyGame.RockPicDir + Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.Carbon;
        }
    }

    public class TechSiliconAsteroid : Asteroid
    {
        public static new string[] Images = new[] { "silicon_1.png", "silicon_2.png", "silicon_3.png", "silicon_4.png" };

        public TechSiliconAsteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : base(game, StrategyGame.RockPicDir + Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.Silicon;
        }
    }

    public class TechUraniumAsteroid : Asteroid
    {
        public static new string[] Images = new[] { "uranium_1.png", "uranium_2.png", "uranium_3.png", "uranium_4.png" };

        public TechUraniumAsteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : base(game, StrategyGame.RockPicDir + Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.Uranium;
        }
    }
}
