using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Shouldly;
using System.Drawing;
using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class BaseTests
    {
        StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            var settings = GameSettings.Default();
            _game = new StrategyGame();
            _game.SetupGame(settings);
            _game.LoadData();
            _game.Map = GameMaps.LoadMap(_game, settings.MapName);
            _game.InitialiseGame();
        }

        [TestMethod]
        public void CheckStarbasesAddPilots()
        {
            var pilots = _game.DockedPilots[0];
            var newPilots = _game.Bases.Bases.First(_ => _.Type == EBaseType.Starbase).Pilots;

            var b1 = _game.Bases.CreateBase(EBaseType.Starbase, 1, Color.White, 1);
            _game.AddBase(b1);

            _game.DockedPilots[0].ShouldBe(pilots + newPilots);
            _game.TotalPilots[0].ShouldBe(pilots + newPilots);
        }


        [TestMethod]
        public void CheckStarbasesAddPilotsLimitedToMax()
        {
            _game.GameSettings.MaximumPilots = 5;
            _game.TotalPilots[0] = _game.DockedPilots[0] = 2;

            var newPilots = _game.Bases.Bases.First(_ => _.Type == EBaseType.Starbase).Pilots;

            var b1 = _game.Bases.CreateBase(EBaseType.Starbase, 1, Color.White, 1);
            _game.AddBase(b1);

            _game.DockedPilots[0].ShouldBe(5);
            _game.TotalPilots[0].ShouldBe(5);
        }


        [TestMethod]
        public void CheckStarbaseDestroyedRemovesPilots()
        {
            var totalPilots = _game.TotalPilots[0];
            var pilots = _game.DockedPilots[0];
            var newPilots = _game.Bases.Bases.First(_ => _.Type == EBaseType.Starbase).Pilots;
            var b1 = _game.Bases.CreateBase(EBaseType.Starbase, 1, Color.White, 1);
            _game.AddBase(b1);

            _game.Bases.DestroyBase(b1);

            _game.DockedPilots[0].ShouldBe(pilots);
            _game.TotalPilots[0].ShouldBe(totalPilots);
        }

        [TestMethod]
        public void CheckStarbaseDestroyedKeepsMaxed()
        {
            _game.GameSettings.MaximumPilots = 5;
            _game.TotalPilots[0] = _game.DockedPilots[0] = 5;

            var newPilots = _game.Bases.Bases.First(_ => _.Type == EBaseType.Starbase).Pilots;

            var b1 = _game.Bases.CreateBase(EBaseType.Starbase, 1, Color.White, 1);
            _game.AddBase(b1);
            var b2 = _game.Bases.CreateBase(EBaseType.Starbase, 1, Color.White, 1);
            _game.AddBase(b2);

            _game.Bases.DestroyBase(b1);

            _game.DockedPilots[0].ShouldBe(5);
            _game.TotalPilots[0].ShouldBe(5);
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
            var bs = _game.Bases.CreateBase(EBaseType.Shipyard, 1, Color.White, 1);
            _game.Bases.DestroyBase(bs);

            _game.Faction[0].CapitalMaxDrones.ShouldBe(_game.GameSettings.InitialCapitalMaxDrones);
        }
    }
}
