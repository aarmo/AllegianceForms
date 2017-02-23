using AllegianceForms.Engine.Generation;

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

        public static Faction Random()
        {
            var f = new Faction(FactionNames.NextString);
            f.Bonuses.Randomise();

            return f;
        }
    }
}
