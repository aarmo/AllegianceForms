using AllegianceForms.Engine;
using AllegianceForms.Engine.Rocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.IO;

namespace AllegianceForms.Test.Art
{
    [TestClass]
    public class RockTests
    {
        [TestMethod]
        public void ExtraRocksDontExist()
        {
            var allItems = new List<string>();
            var extraItems = new List<string>();

            allItems.AddRange(Asteroid.Images);
            allItems.AddRange(ResourceAsteroid.Images);
            allItems.AddRange(TechCarbonAsteroid.Images);
            allItems.AddRange(TechSiliconAsteroid.Images);
            allItems.AddRange(TechUraniumAsteroid.Images);

            var files = Directory.GetFiles(StrategyGame.RockPicDir, "*.png");

            foreach (var f in files)
            {
                var i = f.Replace(StrategyGame.RockPicDir, string.Empty).ToLower();

                if (!allItems.Contains(i))
                    extraItems.Add(i);
            }

            extraItems.Count.ShouldBe(0, $"Unexpected rocks found: {string.Join("\n", extraItems)}");
        }

        [TestMethod]
        public void AsteroidFilesExist()
        {
            foreach (var file in Asteroid.Images)
            {
                var exists = File.Exists(StrategyGame.RockPicDir + file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void ResourceFilesExist()
        {
            foreach (var file in ResourceAsteroid.Images)
            {
                var exists = File.Exists(StrategyGame.RockPicDir + file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void TechCarbonFilesExist()
        {
            foreach (var file in TechCarbonAsteroid.Images)
            {
                var exists = File.Exists(StrategyGame.RockPicDir + file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void TechSiliconFilesExist()
        {
            foreach (var file in TechSiliconAsteroid.Images)
            {
                var exists = File.Exists(StrategyGame.RockPicDir + file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void TechUraniumFilesExist()
        {
            foreach (var file in TechUraniumAsteroid.Images)
            {
                var exists = File.Exists(StrategyGame.RockPicDir + file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }
    }
}
