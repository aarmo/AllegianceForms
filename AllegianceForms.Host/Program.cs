using AllegianceForms.Engine;
using AllegianceForms.Engine.AI;
using AllegianceForms.Engine.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;

namespace AllegianceForms.Host
{
    class Program
    {
        private static bool _running = true;
        private static List<StrategyGame> _games = new List<StrategyGame>();

        static void Main(string[] args)
        {
            using (var tickTimer = new Timer(TickCallback, null, 0, 50))
            {
                using (var slowTimer = new Timer(SlowTickCallback, null, 0, 50))
                {
                    while (_running)
                    {
                        // TODO: Wait for a game client to connect
                        _games.Add(SetupGame(GameSettings.Default()));

                        // Wait for the user to hit <Enter>
                        var command = Console.ReadLine();
                        ProcessServerCommand(command);
                    }
                }
            }
        }

        private static void ProcessServerCommand(string command)
        {
            //TODO: Process different commands
            _running = false;
        }

        private static StrategyGame SetupGame(GameSettings settings)
        {
            var StrategyGame = new StrategyGame();
            
            StrategyGame.SetupGame(settings);
            StrategyGame.LoadData();
            StrategyGame.Map = GameMaps.LoadMap(StrategyGame, settings.MapName);

            var startSectors = (from s in StrategyGame.Map.Sectors
                                where s.StartingSector != 0
                                orderby s.StartingSector
                                select s).ToList();

            StrategyGame.InitialiseGame();

            // Friendy & enemy team setup:
            for (var t = 0; t < StrategyGame.NumTeams; t++)
            {
                var team = t + 1;
                var startingSector = startSectors[t];
                var teamColour = Color.FromArgb(StrategyGame.GameSettings.TeamColours[t]);

                var b1 = StrategyGame.Bases.CreateBase(EBaseType.Starbase, team, teamColour, startingSector.Id, false);
                b1.CenterX = 100;
                b1.CenterY = 100;
                b1.BaseEvent += BaseEvent;
                StrategyGame.AddBase(b1);
                StrategyGame.GameStats.TotalBasesBuilt[t] = 1;

                for (var i = 0; i < settings.MinersInitial; i++)
                {
                    var startingMiner = StrategyGame.Ships.CreateMinerShip(team, teamColour, startingSector.Id);
                    startingMiner.CenterX = b1.CenterX + 100 + (i * 30);
                    startingMiner.CenterY = b1.CenterY + 40;
                    startingMiner.ShipEvent += ShipEvent;
                    StrategyGame.AddUnit(startingMiner);
                }

                if (t != 0)
                {
                    // AI setup:
                    var ai = new CommanderAI(StrategyGame, team, teamColour, ShipEvent, true);
                    StrategyGame.AICommanders[t] = ai;
                    ai.SetDifficulty(settings.AiDifficulty);
                    StrategyGame.DockedPilots[t] = (int)(settings.NumPilots * ai.CheatAdditionalPilots);
                }
            }

            // Final setup:
            startSectors[0].VisibleToTeam[0] = true;
            StrategyGame.UpdateVisibility(true);
            StrategyGame.GameEvent += GameEvent;

            return StrategyGame;
        }

        private static void GameEvent(object sender, EGameEventType e)
        {
        }

        private static void ShipEvent(Ship sender, EShipEventType e)
        {
            // TODO: Which Game is this for?
            foreach (var g in _games)
            {
                g.ProcessShipEvent(sender, e, ShipEvent, BaseEvent);
            }
        }

        private static void BaseEvent(Base sender, EBaseEventType e, int senderTeam)
        {
            // TODO: Which Game is this for?
            foreach (var g in _games)
            {
                g.ProcessBaseEvent(sender, e, senderTeam);
            }
        }

        private static void TickCallback(object state)
        {
            // TODO: Perform a game's simulation step
            foreach (var g in _games)
            {
                g.Tick();
            }
        }

        private static void SlowTickCallback(object state)
        {
            // TODO: Perform a game's slow step
            foreach (var g in _games)
            {
                g.SlowTick(null);
            }
        }
    }
}
