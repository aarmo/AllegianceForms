using AllegianceForms.Engine;
using AllegianceForms.Engine.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
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
        public void ExtraIconsDontExist()
        {
            var allItems = new List<string>();
            var extraItems = new List<string>();

            foreach (ERaceType race in Enum.GetValues(typeof(ERaceType)))
            {
                foreach (var s in _game.Ships.RaceShips[race])
                {
                    if (string.IsNullOrWhiteSpace(s.Image)) continue;
                    if (allItems.Contains(s.Image.ToUpper())) continue;

                    allItems.Add(s.Image.ToUpper());
                }
            }

            foreach (var i in _game.TechTree[0].TechItems)
            {
                if (string.IsNullOrWhiteSpace(i.Icon)) continue;
                if (allItems.Contains(i.Icon.ToUpper())) continue;

                allItems.Add(i.Icon.ToUpper());
            }

            var files = Directory.GetFiles(StrategyGame.IconPicDir, "*.png");

            foreach (var f in files)
            {
                var i = f.Replace(StrategyGame.IconPicDir, string.Empty).ToUpper();

                if (!allItems.Contains(i))
                    extraItems.Add(i);
            }

            extraItems.Count.ShouldBe(0, $"Unexpected icons found: {string.Join("\n", extraItems)}");
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
        public void AnimationFramesExist()
        {
            var files = StrategyGame.GetExplosionFrames();
            foreach (var file in files)
            {
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }

        [TestMethod]
        public void MinefieldImageExists()
        {
            var file = MineWeapon.MinefieldImage;
            var exists = File.Exists(file);
            exists.ShouldBe(true, "File doesn't exist: " + file);            
        }

        [TestMethod]
        public void AllShipIconsExist()
        {
            foreach (ERaceType race in Enum.GetValues(typeof(ERaceType)))
            {
                foreach (var item in _game.Ships.RaceShips[race])
                {
                    if (string.IsNullOrWhiteSpace(item.Image)) continue;

                    var file = StrategyGame.IconPicDir + item.Image;
                    var exists = File.Exists(file);
                    exists.ShouldBe(true, "File doesn't exist: " + file);
                }
            }
        }
    }
}
