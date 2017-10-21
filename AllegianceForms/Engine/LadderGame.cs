using AllegianceForms.Engine.Factions;
using AllegianceForms.Engine.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine
{
    public class LadderGame
    {
        public const int NumPlayersPerDivision = 12;
        public const int MinRankPointsPerGame = 12;
        public const int MaxRankPointsPerDivision = 100;
        public const int NumDivisions = 5;
        public const float MaxSkillDifForMaxPoints = 50f;

        public bool LadderStarted { get; set; }

        public ELadderType LadderType { get; set; }
        public Faction Player { get; set; }
        public Faction[] OtherPlayers { get; set; }
        public int AiDifficulty { get; set; }
        public string[] MapPool { get; set; }
        public GameSettings GameSettings { get; set; }

        public static int TotalOtherPlayers
        {
            get
            {
                return NumPlayersPerDivision - 1;
            }
        }

        public bool Abandoned
        {
            get
            {
                return Player == null;
            }
        }

        public static string DefaultLadderFile = ".\\Data\\DefaultLadder.lad";


        public void AbandonLadder()
        {
            File.Delete(DefaultLadderFile);
            Player = null;
        }

        public void SaveLadder()
        {
            Utils.SerialiseToFile(DefaultLadderFile, this);
        }

        public void UpdateCommanderRanks(Faction[] winners, Faction[] losers)
        {
            var totalWinnerRank = winners.Sum(_ => _.CommanderRankPoints);
            var totalLoserRank = losers.Sum(_ => _.CommanderRankPoints);
            var maxDiff = MaxSkillDifForMaxPoints * winners.Length;

            var diff = totalLoserRank - totalWinnerRank;
            var x = (int) Math.Round(diff / maxDiff * MinRankPointsPerGame);
            var points = (MinRankPointsPerGame + x + 1);

            for (var i = 0; i < winners.Length; i++)
            {
                var w = winners[i];
                w.CommanderRankPoints += points;
                w.LadderGamesPlayed++;
                w.LadderGamesWon++;

                var l = losers[i];
                l.CommanderRankPoints -= points;
                l.LadderGamesPlayed++;
                l.LadderGamesLost++;
            }
        }

        public static bool IsInPlacement(Faction commander)
        {
            return (commander.LeagueTier == ELadderTier.Unranked && commander.LadderGamesPlayed < 10);
        }

        public bool PlaceInTier(Faction commander)
        {
            if (!FinishedPlacement(commander)) return false;

            //var winPerc = 1f * commander.LadderGamesWon / commander.LadderGamesPlayed;
            //var numDivisions = NumDivisions * 3; // Bronze/Silver/Gold
            
            return true;
        }

        public static bool FinishedPlacement(Faction commander)
        {
            return (commander.LeagueTier == ELadderTier.Unranked && commander.LadderGamesPlayed >= 10);
        }

        public static bool ShouldStartDemotion(Faction commander)
        {
            return (commander.LeagueTier != ELadderTier.Bronze && commander.LeagueTier != ELadderTier.Unranked && commander.CommanderRankPoints <= 0);
        }

        public static bool ShouldEndDemotion(Faction commander)
        {
            return (commander.DemotionGamesRunning && commander.PromotionGamesPlayed >= 3);
        }

        public static bool ShouldStartPromotion(Faction commander)
        {
            return (commander.LeagueTier != ELadderTier.Challenger && commander.LeagueTier != ELadderTier.Unranked && commander.CommanderRankPoints >= 100);
        }

        public static bool ShouldEndPromotion(Faction commander)
        {
            return (commander.PromotionGamesRunning && commander.PromotionGamesPlayed >= 3);
        }

        public static bool ShouldBePromoted(Faction commander)
        {
            return (ShouldEndPromotion(commander) && commander.PromotionGamesWon >= 2);
        }

        public static bool ShouldBeDemoted(Faction commander)
        {
            return (ShouldEndDemotion(commander) && commander.PromotionGamesWon < 2);
        }

        public void Promote(Faction commander)
        {
            if (commander.LeagueDivision == 1)
            { 
                commander.LeagueTier = (ELadderTier)((int)commander.LeagueTier + 1);
                commander.LeagueDivision = 5;
            }
            else
            {
                commander.LeagueDivision--;
            }
            commander.CommanderRankPoints = LadderGame.MaxRankPointsPerDivision / 2;

            ReplaceCommander(commander);
        }

        public void Deomote(Faction commander)
        {
            if (commander.LeagueDivision == 5)
            {
                commander.LeagueTier = (ELadderTier)((int)commander.LeagueTier - 1);
                commander.LeagueDivision = 1;
            }
            else
            {
                commander.LeagueDivision++;
            }
            commander.CommanderRankPoints = LadderGame.MaxRankPointsPerDivision / 2;

            ReplaceCommander(commander);
        }

        public void ReplaceCommander(Faction commander)
        {
            if (commander == Player)
            {
                // Generate new AIs
                var newOthers = new List<Faction>();
                Faction.FactionNames.Reset();

                for (var i = 0; i < NumPlayersPerDivision; i++)
                {
                    var f = Faction.Random();
                    f.CommanderRankPoints = StrategyGame.Random.Next(1, 101);
                    f.LeagueTier = commander.LeagueTier;
                    f.LeagueDivision = commander.LeagueDivision;
                    f.LadderGamesPlayed = commander.LadderGamesPlayed;
                    f.LadderGamesWon = StrategyGame.Random.Next(0, f.LadderGamesPlayed + 1);
                    f.LadderGamesLost = f.LadderGamesPlayed - f.LadderGamesWon;

                    newOthers.Add(f);
                }
            }
            else
            {
                // Replace this AI
                var f = Faction.Random();
                f.CommanderRankPoints = 1;
                f.LeagueTier = commander.LeagueTier;
                f.LeagueDivision = commander.LeagueDivision;
                f.LadderGamesPlayed = commander.LadderGamesPlayed;
                f.LadderGamesWon = StrategyGame.Random.Next(0, f.LadderGamesPlayed + 1);
                f.LadderGamesLost = f.LadderGamesPlayed - f.LadderGamesWon;
            }
        }


        List<Faction> _availableCommanders = new List<Faction>();

        public void PlayGamesForAllOtherPlayers()
        {
            if (_availableCommanders.Count() == 0)
            {
                _availableCommanders.AddRange(OtherPlayers);
            }

            var reqCommanders = (int)LadderType * 2;

            while (_availableCommanders.Count() >= reqCommanders)
            {
                var team1 = new List<Faction>{ _availableCommanders[0]};
                if (LadderType == ELadderType.Twos)
                {
                    team1.Add(_availableCommanders[1]);
                }
                _availableCommanders.RemoveAll(team1.Contains);

                var t1 = team1.ToArray();
                var t2 = GetCommandersToFight(t1);

                var winners = GetWinnersRandomly(t1, t2);
                UpdateCommanderRanks(winners, (winners == t1 ? t2 : t1));
            }
        }


        public Faction[] GetWinnersRandomly(Faction[] team1, Faction[] team2)
        {
            var totalRank1 = team1.Sum(_ => _.CommanderRankPoints);
            var totalRank2 = team2.Sum(_ => _.CommanderRankPoints);

            // Favour the team with the highest combined points
            var chance = (1f * totalRank1 / totalRank2) - 0.5f;

            // Who Win?
            var team1Won = StrategyGame.RandomChance(chance);

            return (team1Won ? team1 : team2);
        }

        public Faction GetAllyForPlayer()
        {
            if (LadderType == ELadderType.Ones) return null;

            if (_availableCommanders.Count == 0)
            {
                _availableCommanders.AddRange(OtherPlayers);
            }

            var ally = _availableCommanders[StrategyGame.Random.Next(_availableCommanders.Count)];
            _availableCommanders.Remove(ally);

            return ally;
        }

        public Faction[] GetCommandersForPlayerToFight(ref Faction[] a)
        {
            var allies = new List<Faction> { Player };
            var ally = GetAllyForPlayer();
            if (ally != null) allies.Add(ally);

            a = allies.ToArray();

            return GetCommandersToFight(a);
        }

        public Faction[] GetCommandersToFight(Faction[] allies)
        {
            var enemies = new List<Faction>();

            if (_availableCommanders.Count == 0)
            {
                _availableCommanders.AddRange(OtherPlayers.Except(allies));
            }

            var avgRank = 1f * allies.Sum(_ => _.CommanderRankPoints) / allies.Length;

            var available = (from c in _availableCommanders
                             orderby Math.Abs(c.CommanderRankPoints - avgRank)
                             select c).ToList();

            enemies.AddRange(available.Take((int)LadderType));
            _availableCommanders.RemoveAll(enemies.Contains);

            return enemies.ToArray();
        }


        public static LadderGame LoadOrDefault()
        {
            var l = Utils.DeserialiseFromFile<LadderGame>(DefaultLadderFile);
            if (l == null)
            {
                l = Default();
                Utils.SerialiseToFile(DefaultLadderFile, l);
            }

            return l;
        }

        public static LadderGame Default()
        {
            var l = new LadderGame
            {
                LadderStarted = false,
                AiDifficulty = 1,
                LadderType = ELadderType.Ones,
                OtherPlayers = new Faction[TotalOtherPlayers],
                Player = Faction.Default()
            };

            var maps = new List<string>();
            var chosenMaps = new List<string>();
            maps.AddRange(GameMaps.AvailableMaps(2));
            
            for (var i = 0; i < 4; i++)
            {
                var m = maps[StrategyGame.Random.Next(maps.Count)];
                maps.Remove(m);
                chosenMaps.Add(m);
            }
            l.MapPool = chosenMaps.ToArray();

            for (var i = 0; i < l.OtherPlayers.Length; i++)
            {
                l.OtherPlayers[i] = Faction.Random();
            }

            l.GameSettings = GameSettings.Default();

            return l;
        }
    }
}
