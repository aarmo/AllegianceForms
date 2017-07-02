using AllegianceForms.Engine.Factions;
using AllegianceForms.Engine.Map;
using AllegianceForms.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine
{
    public class LadderGame
    {
        public const int NumPlayersPerDivision = 5;
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
                var numTiers = Enum.GetValues(typeof(ELadderTier)).Length;
                return NumPlayersPerDivision * NumDivisions * numTiers - 1;
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
            var tier = allies[0].LeagueTier;
            var division = allies[0].LeagueDivision;

            var available = (from c in _availableCommanders
                             where c.LeagueTier == tier
                             && c.LeagueDivision == division
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
