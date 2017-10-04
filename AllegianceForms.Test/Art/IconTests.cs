using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;

namespace AllegianceForms.Test.Art
{
    [TestClass]
    public class IconTests
    {
        private StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            _game = new StrategyGame();
            _game.SetupGame(GameSettings.Default());
            _game.LoadData();
        }

        [TestMethod]
        public void TechIconsExist()
        {
            var items = _game.TechTree[0].TechItems;

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Icon)) continue;

                var file = StrategyGame.IconPicDir + item.Icon;
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void ShipIconsExist()
        {
            var items = _game.Ships.Ships;

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Image)) continue;

                var file = StrategyGame.IconPicDir + item.Image;
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }
    }
}
