using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class TechItemTests
    {
        [TestInitialize]
        public void Setup()
        {
            StrategyGame.SetupGame(GameSettings.Default());
            StrategyGame.LoadData();
            StrategyGame.Map = GameMaps.PinWheel(2);
        }

        [TestMethod]
        public void CheckUpdateWithNoInvestment()
        {
            var target = StrategyGame.TechTree[0].TechItems.First(_ => _.Active && !_.Completed);

            target.AmountInvested = 0;
            target.Update();

            target.ResearchedTicks.ShouldBe(0);
        }

        [TestMethod]
        public void CheckUpdateWhenCompleted()
        {
            var target = StrategyGame.TechTree[0].TechItems.First(_ => _.Active && !_.Completed);

            target.AmountInvested = target.Cost;
            target.ResearchedTicks = target.DurationTicks;
            target.Completed = true;
            target.Update();

            target.ResearchedTicks.ShouldBe(target.DurationTicks);
        }

        [TestMethod]
        public void CheckUpdateIncompleteInvestment()
        {
            var target = StrategyGame.TechTree[0].TechItems.First(_ => _.Active && !_.Completed);

            target.AmountInvested = target.Cost-10;
            target.ResearchedTicks = target.DurationTicks-1;
            target.Completed = false;
            target.Update();

            target.Completed.ShouldBe(false);
        }

        [TestMethod]
        public void CheckUpdateIncompleteTime()
        {
            var target = StrategyGame.TechTree[0].TechItems.First(_ => _.Active && !_.Completed);

            target.AmountInvested = target.Cost;
            target.ResearchedTicks = target.DurationTicks - 10;
            target.Completed = false;
            target.Update();

            target.Completed.ShouldBe(false);
            target.ResearchedTicks.ShouldBe(target.DurationTicks - 9);
        }

        [TestMethod]
        public void CheckUpdateCompleted()
        {
            var target = StrategyGame.TechTree[0].TechItems.First(_ => _.Active && !_.Completed);

            target.AmountInvested = target.Cost;
            target.ResearchedTicks = target.DurationTicks - 1;
            target.Completed = false;
            target.Update();

            target.Completed.ShouldBe(true);
            target.Active.ShouldBe(true);
            target.ResearchedTicks.ShouldBe(target.DurationTicks);
            target.AmountInvested.ShouldBe(target.Cost);
        }
    }
}
