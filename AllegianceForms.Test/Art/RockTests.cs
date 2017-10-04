using AllegianceForms.Engine.Rocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;

namespace AllegianceForms.Test.Art
{
    [TestClass]
    public class RockTests
    {
        [TestMethod]
        public void AsteroidFilesExist()
        {
            foreach (var file in Asteroid.Images)
            {
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void ResourceFilesExist()
        {
            foreach (var file in ResourceAsteroid.Images)
            {
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void TechCarbonFilesExist()
        {
            foreach (var file in TechCarbonAsteroid.Images)
            {
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void TechSiliconFilesExist()
        {
            foreach (var file in TechSiliconAsteroid.Images)
            {
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void TechUraniumFilesExist()
        {
            foreach (var file in TechUraniumAsteroid.Images)
            {
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }
    }
}
