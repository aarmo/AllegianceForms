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
        [TestInitialize]
        public void Setup()
        {
            StrategyGame.SetupGame(GameSettings.Default());
            StrategyGame.LoadData();
        }

        [TestMethod]
        public void CheckStarbasesAddPilots()
        {
            var pilots = StrategyGame.DockedPilots[0];
            var newPilots = StrategyGame.Bases.Bases.First(_ => _.Type == EBaseType.Starbase).Pilots;

            var b1 = StrategyGame.Bases.CreateBase(EBaseType.Starbase, 1, Color.White, 1);

            StrategyGame.DockedPilots[0].ShouldBe(pilots + newPilots);
        }

        [TestMethod]
        public void CheckResourcesGenerateIncome()
        {
            var res = StrategyGame.Credits[0];
            var b1 = StrategyGame.Bases.CreateBase(EBaseType.Resource, 1, Color.White, 1);
            b1.Update(0);

            StrategyGame.Credits[0].ShouldBe(res + 10);
        }
    }
}
