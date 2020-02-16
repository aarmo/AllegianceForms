using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Shouldly;
using System.Drawing;
using AllegianceForms.Engine;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class BaseTests
    {
        StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            _game = new StrategyGame();
            _game.SetupGame(GameSettings.Default());
            _game.LoadData();
        }

        [TestMethod]
        public void CheckStarbasesAddPilots()
        {
            var pilots = _game.DockedPilots[0];
            var newPilots = _game.Bases.Bases.First(_ => _.Type == EBaseType.Starbase).Pilots;

            var b1 = _game.Bases.CreateBase(EBaseType.Starbase, 1, Color.White, 1);

            _game.DockedPilots[0].ShouldBe(pilots + newPilots);
        }

        [TestMethod]
        public void CheckStarbaseDestroyedRemovesPilots()
        {
            var pilots = _game.DockedPilots[0];
            var newPilots = _game.Bases.Bases.First(_ => _.Type == EBaseType.Starbase).Pilots;
            var b1 = _game.Bases.CreateBase(EBaseType.Starbase, 1, Color.White, 1);

            _game.Bases.DestroyBase(EBaseType.Starbase, 1);

            _game.DockedPilots[0].ShouldBe(pilots);
        }

        [TestMethod]
        public void CheckResourcesGenerateIncome()
        {
            var res = _game.Credits[0];
            var b1 = _game.Bases.CreateBase(EBaseType.Resource, 1, Color.White, 1);

            b1.Update();
            b1.Update();
            b1.Update();
            b1.Update();

            _game.Credits[0].ShouldBe(res + 4);
        }
        
        [TestMethod]
        public void CheckShipyardsAddMaxCapDrones()
        {
            _game.Bases.CreateBase(EBaseType.Shipyard, 1, Color.White, 1);

            _game.Faction[0].CapitalMaxDrones.ShouldBe(_game.GameSettings.InitialCapitalMaxDrones*2);
        }

        [TestMethod]
        public void CheckShipyardDestroyedRemoveMaxCapDrones()
        {
            _game.Bases.CreateBase(EBaseType.Shipyard, 1, Color.White, 1);
            _game.Bases.DestroyBase(EBaseType.Shipyard, 1);

            _game.Faction[0].CapitalMaxDrones.ShouldBe(_game.GameSettings.InitialCapitalMaxDrones);
        }
    }
}
