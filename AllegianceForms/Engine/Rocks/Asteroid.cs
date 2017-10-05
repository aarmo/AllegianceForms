using System;

namespace AllegianceForms.Engine.Rocks
{
    public class Asteroid : GameEntity
    {
        public static string[] Images = new[] {  "rock_1.png", "rock_2.png", "rock_3.png", "rock_4.png" };

        public EAsteroidType Type { get; set; }

        public Asteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : this(game, StrategyGame.RockPicDir + Images[r.Next(0, Images.Length)], width, height, sectorId)
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
