using AllegianceForms.Engine.Generation;
using Newtonsoft.Json;

namespace AllegianceForms.Engine.Factions
{
    public class Faction
    {
        public string Name { get; set; }
        public FactionBonus Bonuses { get; set; }

        public Faction(string name)
        {
            Name = name;
            Bonuses = new FactionBonus();
        }

        public override string ToString()
        {
            return Name;
        }

        public static RandomString FactionNames = new RandomString(".\\Data\\Names-Faction.txt");

        public static Faction Default()
        {
            return new Faction("Default");
        }

        public static Faction CreateFaction(string name)
        {
            switch (name)
            {
                case "Default":
                    return Default();
                default:
                    return Random();
            }
        }

        public static Faction Random(int min = 10)
        {
            var f = new Faction(FactionNames.NextString);
            f.Bonuses.Randomise(min);

            return f;
        }

        public Faction Clone()
        {
            var json = JsonConvert.SerializeObject(this); 
            return JsonConvert.DeserializeObject<Faction>(json);
        }
    }
}
