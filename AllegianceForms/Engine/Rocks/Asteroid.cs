using System;

namespace AllegianceForms.Engine.Rocks
{
    public class Asteroid : GameEntity
    {
        public static string[] Images = new[] { ".\\Art\\Rocks\\rock_1.png", ".\\Art\\Rocks\\rock_2.png", ".\\Art\\Rocks\\rock_3.png", ".\\Art\\Rocks\\rock_4.png" };

        public EAsteroidType Type { get; set; }

        public Asteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : this(game, Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
        }

        public Asteroid(StrategyGame game, string imageFilename, int width, int height, int sectorId)
            : base(game, imageFilename, width, height, sectorId)
        {
            Type = EAsteroidType.Rock;
        }

        public void BuildingComplete()
        {
            Active = false;
        }
    }
}
