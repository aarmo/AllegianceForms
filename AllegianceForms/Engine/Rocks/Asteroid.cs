using System;

namespace AllegianceForms.Engine.Rocks
{
    public class Asteroid : GameEntity
    {
        private static string[] _images = new[] { ".\\Art\\Rocks\\rock_1.png", ".\\Art\\Rocks\\rock_2.png", ".\\Art\\Rocks\\rock_3.png", ".\\Art\\Rocks\\rock_4.png" };

        public EAsteroidType Type { get; set; }

        public Asteroid(Random r, int width, int height, int sectorId)
            : this(_images[r.Next(0, _images.Length)], width, height, sectorId)
        {
        }

        public Asteroid(string imageFilename, int width, int height, int sectorId)
            : base(imageFilename, width, height, sectorId)
        {
            Type = EAsteroidType.Rock;
        }

        public void BuildingComplete()
        {
            Active = false;
        }
    }
}
