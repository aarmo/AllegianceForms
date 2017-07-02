using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class LadderSettingsTests
    {
        LadderGame _ladder;

        [TestInitialize]
        public void Setup()
        {
            _ladder = LadderGame.Default();
        }

        [TestMethod]
        public void CheckDefaultOnesSettings()
        {
            AllegianceForms.Engine.Factions.Faction[] team1 = null;
            var team2 = _ladder.GetCommandersForPlayerToFight(ref team1);

            var invalidTeams = (team1 == null || team2 == null || team1.Length != team2.Length);
            invalidTeams.ShouldBe(false);
            
            var settings = GameSettings.LadderDefault(_ladder, team1, team2);

            settings.ShouldNotBeNull();
            settings.NumTeams.ShouldBe(2);
            settings.TeamFactions[0].ShouldBe(team1[0]);
            settings.TeamFactions[1].ShouldBe(team2[0]);
        }

        [TestMethod]
        public void CheckDefaultTwosSettings()
        {
            _ladder.LadderType = ELadderType.Twos;
            AllegianceForms.Engine.Factions.Faction[] team1 = null;
            var team2 = _ladder.GetCommandersForPlayerToFight(ref team1);

            var invalidTeams = (team1 == null || team2 == null || team1.Length != team2.Length);
            invalidTeams.ShouldBe(false);
            
            var settings = GameSettings.LadderDefault(_ladder, team1, team2);

            settings.ShouldNotBeNull();
            settings.NumTeams.ShouldBe(4);
            settings.TeamFactions[0].ShouldBe(team1[0]);
            settings.TeamAlliance[0].ShouldBe(1);
            settings.TeamFactions[1].ShouldBe(team1[1]);
            settings.TeamAlliance[1].ShouldBe(1);

            settings.TeamFactions[2].ShouldBe(team2[0]);
            settings.TeamAlliance[2].ShouldBe(2);
            settings.TeamFactions[3].ShouldBe(team2[1]);
            settings.TeamAlliance[3].ShouldBe(2);
        }
    }
}
