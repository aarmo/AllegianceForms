using AllegianceForms.Engine.AI;
using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Drawing;

namespace AllegianceForms.Test.AI
{
    [TestClass]
    public class CommanderTests
    {
        private CommanderAI _target;
        private StrategyGame _game;

        [TestInitialize]
        public void Setup()
        {
            _game = new StrategyGame();

            _game.SetupGame(GameSettings.Default());
            _game.LoadData();
            _game.Map = GameMaps.LoadMap(_game, "PinWheel2");

            _target = new CommanderAI(_game, 2, Color.Red, null);
        }

        [TestMethod]
        public void InitialPriorities()
        {
            _target.PilotPriorities[EAiPilotPriorities.Scout].ShouldBe(0);
            _target.PilotPriorities[EAiPilotPriorities.BaseDefense].ShouldBe(0);
            _target.PilotPriorities[EAiPilotPriorities.BaseOffense].ShouldBe(0);
            _target.PilotPriorities[EAiPilotPriorities.MinerDefense].ShouldBe(0);
            _target.PilotPriorities[EAiPilotPriorities.MinerOffense].ShouldBe(0);
            _target.CreditPriorities[EAiCreditPriorities.Offense].ShouldBe(0);
            _target.CreditPriorities[EAiCreditPriorities.Defense].ShouldBe(0);
            _target.CreditPriorities[EAiCreditPriorities.Expansion].ShouldBe(0);
        }

        [TestMethod]
        public void FirstPriorities()
        {
            _target.Update();

            _target.PilotPriorities[EAiPilotPriorities.Scout].ShouldBe(1);
            _target.PilotPriorities[EAiPilotPriorities.BaseDefense].ShouldBe(0);
            _target.PilotPriorities[EAiPilotPriorities.BaseOffense].ShouldBe(0);
            _target.PilotPriorities[EAiPilotPriorities.MinerDefense].ShouldBe(0);
            _target.PilotPriorities[EAiPilotPriorities.MinerOffense].ShouldBe(0);

            _target.CreditPriorities[EAiCreditPriorities.Offense].ShouldBe(0);
            _target.CreditPriorities[EAiCreditPriorities.Defense].ShouldBe(0);
            _target.CreditPriorities[EAiCreditPriorities.Expansion].ShouldBe(_game.Map.Sectors.Count);
        }

        [TestMethod]
        public void DegradingPriorities()
        {
            foreach (var e in (EAiCreditPriorities[])Enum.GetValues(typeof(EAiCreditPriorities)))
            {
                _target.CreditPriorities[e] = 10;
            }
            foreach (var e in (EAiPilotPriorities[])Enum.GetValues(typeof(EAiPilotPriorities)))
            {
                _target.PilotPriorities[e] = 10;
            }

            _target.Update();
            var p = CommanderAI.DegradeAmount * 10;

            _target.PilotPriorities[EAiPilotPriorities.Scout].ShouldBe(p + 1);
            _target.PilotPriorities[EAiPilotPriorities.BaseDefense].ShouldBe(p);
            _target.PilotPriorities[EAiPilotPriorities.BaseOffense].ShouldBe(p);
            _target.PilotPriorities[EAiPilotPriorities.MinerDefense].ShouldBe(p);
            _target.PilotPriorities[EAiPilotPriorities.MinerOffense].ShouldBe(p);

            _target.CreditPriorities[EAiCreditPriorities.Offense].ShouldBe(p);
            _target.CreditPriorities[EAiCreditPriorities.Defense].ShouldBe(p);
            _target.CreditPriorities[EAiCreditPriorities.Expansion].ShouldBe(p + _game.Map.Sectors.Count);
        }
    }
}
