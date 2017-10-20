using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Engine.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class MineWeaponTests
    {
        private StrategyGame _game;
        private CombatShip _target;

        [TestInitialize]
        public void Setup()
        {
            _game = new StrategyGame();
            _game.SetupGame(GameSettings.Default());
            _game.LoadData();

            _target = _game.Ships.CreateShip("Scout", 1, Color.White, 0);
        }

        [TestMethod]
        public void MineWeaponCreated()
        {
            _target.Weapons.Count.ShouldBe(2);
            var w = _target.Weapons.FirstOrDefault(_ => _.GetType() == typeof(MineWeapon));
            w.ShouldNotBeNull();
        }
    }
}
