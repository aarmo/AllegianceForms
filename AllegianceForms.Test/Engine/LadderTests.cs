using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;

namespace AllegianceForms.Test.Engine
{
    [TestClass]
    public class LadderTests
    {
        LadderGame _ladder;

        [TestInitialize]
        public void Setup()
        {
            _ladder = LadderGame.Default();
        }

        [TestMethod]
        public void CheckInitialSetup()
        {
            LadderGame.TotalOtherPlayers.ShouldBe(8 * 5 * 5 - 1);

            _ladder.AiDifficulty.ShouldBe(1);
            _ladder.LadderType.ShouldBe(ELadderType.Ones);
            _ladder.OtherPlayers.Length.ShouldBe(LadderGame.TotalOtherPlayers);
            _ladder.MapPool.Length.ShouldBe(4);

            var p = _ladder.Player;
            p.ShouldNotBeNull();
            p.LeagueTier.ShouldBe(ELadderTier.Unranked);
            p.LeagueDivision.ShouldBe(5);
            p.CommanderRankPoints.ShouldBe(LadderGame.MaxRankPointsPerDivision / 2);
            
            for (var i = 0; i < _ladder.OtherPlayers.Length; i++)
            {
                var op = _ladder.OtherPlayers[i];

                op.ShouldNotBeNull();
                op.CommanderRankPoints.ShouldBe(LadderGame.MaxRankPointsPerDivision / 2);
                op.LeagueTier.ShouldBe(ELadderTier.Unranked);
                op.LeagueDivision.ShouldBe(5);
            }
        }

        [TestMethod]
        public void CheckPointsWithEqualRanks()
        {
            var winners = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var losers = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };

            _ladder.UpdateCommanderRanks(winners, losers);

            for (var i = 0; i < winners.Length; i++)
            {
                var w = winners[i];
                var l = losers[i];

                // Win/lost 13 points
                w.CommanderRankPoints.ShouldBe(LadderGame.MaxRankPointsPerDivision / 2 + LadderGame.MinRankPointsPerGame + 1);
                l.CommanderRankPoints.ShouldBe(LadderGame.MaxRankPointsPerDivision / 2 - LadderGame.MinRankPointsPerGame - 1);
            }
        }

        [TestMethod]
        public void CheckPointsWhenSlightlyFavoured()
        {
            var winners = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var losers = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            winners[0].CommanderRankPoints = 60;
            winners[1].CommanderRankPoints = 50;

            losers[0].CommanderRankPoints = 50;
            losers[1].CommanderRankPoints = 50;

            _ladder.UpdateCommanderRanks(winners, losers);

            // Win/lose 12 points
            winners[0].CommanderRankPoints.ShouldBe(72);
            winners[1].CommanderRankPoints.ShouldBe(62);

            losers[0].CommanderRankPoints.ShouldBe(38);
            losers[1].CommanderRankPoints.ShouldBe(38);
        }

        [TestMethod]
        public void CheckPointsWhenVerySlightlyFavoured()
        {
            var winners = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var losers = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            winners[0].CommanderRankPoints = 51;
            winners[1].CommanderRankPoints = 51;

            losers[0].CommanderRankPoints = 50;
            losers[1].CommanderRankPoints = 50;

            _ladder.UpdateCommanderRanks(winners, losers);

            // Win/lose 13 points
            winners[0].CommanderRankPoints.ShouldBe(64);
            winners[1].CommanderRankPoints.ShouldBe(64);

            losers[0].CommanderRankPoints.ShouldBe(37);
            losers[1].CommanderRankPoints.ShouldBe(37);
        }

        [TestMethod]
        public void CheckPointsWhenVerySlightlyUnFavoured()
        {
            var winners = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var losers = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            winners[0].CommanderRankPoints = 50;
            winners[1].CommanderRankPoints = 50;

            losers[0].CommanderRankPoints = 51;
            losers[1].CommanderRankPoints = 51;

            _ladder.UpdateCommanderRanks(winners, losers);

            // Win/lose 13 points
            winners[0].CommanderRankPoints.ShouldBe(63);
            winners[1].CommanderRankPoints.ShouldBe(63);

            losers[0].CommanderRankPoints.ShouldBe(38);
            losers[1].CommanderRankPoints.ShouldBe(38);
        }

        [TestMethod]
        public void CheckPointsWhenSlightlyUnFavoured()
        {
            var winners = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var losers = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            winners[0].CommanderRankPoints = 50;
            winners[1].CommanderRankPoints = 50;

            losers[0].CommanderRankPoints = 60;
            losers[1].CommanderRankPoints = 50;

            _ladder.UpdateCommanderRanks(winners, losers);

            // Win/lose a little more points
            winners[0].CommanderRankPoints.ShouldBe(64);
            winners[1].CommanderRankPoints.ShouldBe(64);

            losers[0].CommanderRankPoints.ShouldBe(46);
            losers[1].CommanderRankPoints.ShouldBe(36);
        }

        [TestMethod]
        public void CheckPointsWhenHeavilyFavoured()
        {
            var winners = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var losers = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            winners[0].CommanderRankPoints = 80;
            winners[1].CommanderRankPoints = 70;

            losers[0].CommanderRankPoints = 30;
            losers[1].CommanderRankPoints = 20;

            _ladder.UpdateCommanderRanks(winners, losers);

            // Win/lost min points (1)
            winners[0].CommanderRankPoints.ShouldBe(81);
            winners[1].CommanderRankPoints.ShouldBe(71);

            losers[0].CommanderRankPoints.ShouldBe(29);
            losers[1].CommanderRankPoints.ShouldBe(19);
        }

        [TestMethod]
        public void CheckPointsWhenHeavilyUnFavoured()
        {
            var winners = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var losers = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            winners[0].CommanderRankPoints = 30;
            winners[1].CommanderRankPoints = 20;

            losers[0].CommanderRankPoints = 80;
            losers[1].CommanderRankPoints = 70;

            _ladder.UpdateCommanderRanks(winners, losers);

            // Win/lost max points (25)
            winners[0].CommanderRankPoints.ShouldBe(55);
            winners[1].CommanderRankPoints.ShouldBe(45);

            losers[0].CommanderRankPoints.ShouldBe(55);
            losers[1].CommanderRankPoints.ShouldBe(45);
        }

        [TestMethod]
        public void CheckNoAlliesForOnes()
        {
            var ally = _ladder.GetAllyForPlayer();
            ally.ShouldBeNull();
        }

        [TestMethod]
        public void CheckAnAllyForTwos()
        {
            _ladder.LadderType = ELadderType.Twos;
            var ally = _ladder.GetAllyForPlayer();
            ally.ShouldNotBeNull();
        }

        [TestMethod]
        public void CheckEnemiesForOnes()
        {
            AllegianceForms.Engine.Factions.Faction[] playerAllies = null;
            var enemies = _ladder.GetCommandersForPlayerToFight(ref playerAllies);

            enemies.Length.ShouldBe(1);
            enemies[0].ShouldNotBeNull();

            playerAllies.Length.ShouldBe(1);
            playerAllies[0].ShouldNotBeNull();
            playerAllies[0].ShouldBe(_ladder.Player);
        }

        [TestMethod]
        public void CheckEnemiesForTwos()
        {
            AllegianceForms.Engine.Factions.Faction[] playerAllies = null;
            _ladder.LadderType = ELadderType.Twos;
            var enemies = _ladder.GetCommandersForPlayerToFight(ref playerAllies);

            enemies.Length.ShouldBe(2);
            enemies[0].ShouldNotBeNull();
            enemies[1].ShouldNotBeNull();
            enemies.Any(playerAllies.Contains).ShouldBe(false);

            playerAllies.Length.ShouldBe(2);
            playerAllies[0].ShouldNotBeNull();
            playerAllies[0].ShouldBe(_ladder.Player);
            playerAllies[1].ShouldNotBeNull();
            playerAllies.Any(enemies.Contains).ShouldBe(false);

            enemies.ShouldNotContain(_ladder.Player);
        }

        [TestMethod]
        public void CheckRandomWinnersEqual()
        {
            var team1 = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var team2 = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };

            var winners = _ladder.GetWinnersRandomly(team1, team2);

            winners.Length.ShouldBe(2);
        }

        [TestMethod]
        public void CheckRandomWinnersFavoured()
        {
            var team1 = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var team2 = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            team1[0].CommanderRankPoints = 60;

            var winners = _ladder.GetWinnersRandomly(team1, team2);

            winners.Length.ShouldBe(2);
        }

        [TestMethod]
        public void CheckRandomWinnersHeavilyFavoured()
        {
            var team1 = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var team2 = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            team1[0].CommanderRankPoints = 85;

            var winners = _ladder.GetWinnersRandomly(team1, team2);

            winners.Length.ShouldBe(2);
        }

        [TestMethod]
        public void CheckRandomWinnersUnFavoured()
        {
            var team1 = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var team2 = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            team2[0].CommanderRankPoints = 60;

            var winners = _ladder.GetWinnersRandomly(team1, team2);

            winners.Length.ShouldBe(2);
        }

        [TestMethod]
        public void CheckRandomWinnersHeavilyUnFavoured()
        {
            var team1 = new[] { _ladder.OtherPlayers[0], _ladder.OtherPlayers[1] };
            var team2 = new[] { _ladder.OtherPlayers[2], _ladder.OtherPlayers[3] };
            team2[0].CommanderRankPoints = 85;

            var winners = _ladder.GetWinnersRandomly(team1, team2);

            winners.Length.ShouldBe(2);
        }

        [TestMethod]
        public void CheckAllPlayersPlayAfterPlayerInOnes()
        {
            AllegianceForms.Engine.Factions.Faction[] t1 = null;
            var t2 = _ladder.GetCommandersForPlayerToFight(ref t1);

            var w1 = _ladder.GetWinnersRandomly(t1, t2);
            _ladder.UpdateCommanderRanks(w1, (w1 == t1 ? t2 : t1));

            _ladder.PlayGamesForAllOtherPlayers();

            _ladder.Player.LadderGamesPlayed.ShouldBe(1);

            var unplayedPlayers = _ladder.OtherPlayers.Count(_ => _.LadderGamesPlayed == 0);
            unplayedPlayers.ShouldBe(0);
        }

        [TestMethod]
        public void CheckAllPlayersPlayAfterPlayerInTwos()
        {
            _ladder.LadderType = ELadderType.Twos;

            AllegianceForms.Engine.Factions.Faction[] t1 = null;
            var t2 = _ladder.GetCommandersForPlayerToFight(ref t1);

            var w1 = _ladder.GetWinnersRandomly(t1, t2);
            _ladder.UpdateCommanderRanks(w1, (w1 == t1 ? t2 : t1));

            _ladder.PlayGamesForAllOtherPlayers();

            _ladder.Player.LadderGamesPlayed.ShouldBe(1);

            var unplayedPlayers = _ladder.OtherPlayers.Count(_ => _.LadderGamesPlayed == 0);
            unplayedPlayers.ShouldBe(0);
        }
    }
}
