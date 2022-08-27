﻿using AllegianceForms.Engine.Generation;
using System;

namespace AllegianceForms.Engine.Factions
{
    public class Faction
    {
        public string Name { get; set; }
        public string PictureCode { get; set; }
        public string CommanderName { get; set; }
        
        public FactionBonus Bonuses { get; set; }

        public int CapitalMaxDrones { get; set; }

        public ERaceType Race { get; set;}

        public Faction()
        {
        }

        public Faction(string name, string commanderName, GameSettings settings)
        {
            PictureCode = Name = name;
            CommanderName = commanderName;
            Bonuses = new FactionBonus();
            Race = ERaceType.Amanni;

            CapitalMaxDrones = settings.InitialCapitalMaxDrones;
        }

        public override string ToString()
        {
            return Name;
        }

        public static RandomPartString FactionNames = new RandomPartString(".\\Data\\FactionNameParts.json");

        public static Faction Default(GameSettings settings)
        {
            return new Faction("Default", "Player1", settings);
        }
        
        public static Faction Random(GameSettings settings, int min = 30)
        {
            var name = FactionNames.NextString;
            var f = new Faction(name, StrategyGame.RandomName.GetRandomName(name), settings);
            
            var races = Enum.GetValues(typeof(ERaceType));
            f.Race = (ERaceType)races.GetValue(StrategyGame.Random.Next(races.Length));

            f.Bonuses.Randomise(min);

            return f;
        }
    }
}
