using System;
using System.Text;

namespace AllegianceForms.Engine.Generation
{
    public class RandomName
    {
        // 1 + 2
        string[] nm1_1 = { "Super", "Acid", "Aero", "After", "Air", "Ape", "Aqua", "Armor", "Astro", "Auto", "Avian", "Back", "Blow", "Blue", "Body", "Bomb", "Bone", "Botani", "Boulder", "Brake", "Break", "Broad", "Brush", "Bulk", "Bull", "Bullet", "Buzz", "Cannon", "Chain", "Chrome", "Cinder", "Cliff", "Cloud", "Crack", "Crank", "Cross", "Dark", "Deep", "Deft", "Depth", "Dino", "Dirt", "Dive", "Doom", "Double", "Dread", "Drop", "Dune", "Dust", "Fang", "Fiery", "Fire", "Fist", "Flame", "Flash", "Flat", "Flight", "Fly", "Free", "Freeze", "Frost", "Gear", "Gloom", "Gold", "Grand", "Grim", "Grind", "Grizz", "Groove", "Ground", "Growl", "Gun", "Hail", "Half", "Hammer", "Hang", "Hard", "Heavy", "Heli", "High", "Hook", "Hot", "Hydra", "Hydrau", "Hyper", "Ice", "Iron", "Jaw", "Jet", "Jolt", "Junk", "Kick", "Land", "Lazer", "Lead", "Leo", "Light", "Lock", "Long", "Lunar", "Magma", "Magna", "Mean", "Mecha", "Mega", "Melt", "Motor", "Neutro", "Night", "Oil", "Over", "Phase", "Photon", "Power", "Pyro", "Quick", "Rage", "Rapid", "Rat", "Razor", "Retro", "Rhi", "Rhino", "Rip", "Road", "Roll", "Rotor", "Rough", "Rumble", "Savage", "Scorch", "Scrap", "Sea", "Shade", "Shadow", "Shock", "Side", "Silver", "Sky", "Slam", "Slip", "Smoke", "Solar", "Sound", "Spark", "Speed", "Star", "Steel", "Stone", "Storm", "Strike", "Sun", "Swift", "Thunder", "Tiga", "Tiger", "Top", "Turbo", "Twin", "Vice", "Volt", "Wide", "Wild", "Wind", "Wolf", "Wreck" };
        string[] nm1_2 = { "back", "bang", "beam", "beast", "bird", "bite", "blade", "blades", "blast", "blaze", "blight", "blitz", "bolt", "boom", "bot", "brawl", "brawn", "burn", "burner", "burst", "buster", "button", "case", "cast", "charge", "charger", "circuit", "clash", "claw", "cloud", "clutch", "crack", "crush", "crusher", "cut", "cycle", "dash", "dealer", "dive", "dome", "drift", "drive", "feather", "fight", "fire", "fist", "flash", "flight", "flow", "foot", "frame", "glide", "glider", "glitch", "guard", "hammer", "head", "heap", "hide", "horn", "jaw", "jump", "kick", "kill", "lane", "lift", "light", "line", "load", "lock", "master", "mine", "point", "pounder", "punch", "quake", "raid", "raider", "rake", "raker", "ray", "razor", "runner", "scope", "scrap", "scraps", "scream", "shift", "shot", "side", "sight", "siren", "slide", "sling", "slinger", "snarl", "spike", "spin", "splitter", "spot", "stalker", "steel", "stop", "storm", "streak", "stream", "strike", "strong", "stuff", "sweep", "switch", "thing", "top", "track", "tracks", "trap", "tron", "twitch", "viper", "vortex", "war", "watch", "wave", "way", "ways", "wheels", "whip", "wing", "wire", "wise", "works" };
        
        // 1 + " " + 2
        string[] nm2_1 = { "barb", "blade", "bone", "chest", "cinder", "claw", "crag", "crest", "crook", "crystal", "dagger", "death", "dirge", "dust", "edge", "ember", "fang", "frost", "fuse", "gore", "hammer", "heart", "hook", "ice", "iron", "knife", "lance", "leather", "light", "meat", "molten", "pincer", "pyre", "rage", "ridge", "saber", "sabre", "scythe", "shade", "shadow", "shank", "sharp", "shiv", "silver", "skull", "slate", "solid", "spike", "spine", "steel", "tail", "talon", "thorn", "thunder", "tusk" };
        string[] nm2_2 = { "back", "basher", "blade", "blight", "blower", "bone", "breaker", "breath", "claw", "cleaver", "crest", "crusher", "cutter", "drifter", "eye", "eyes", "fang", "fangs", "fist", "flayer", "fury", "gazer", "hammer", "head", "heart", "hook", "hunter", "jaw", "lance", "mane", "mantle", "maul", "maw", "pelt", "reaper", "reaver", "ridge", "ripper", "snout", "spitter", "splitter", "stalker", "striker", "weaver" };

        string[] nm3_1 = { "Ace", "Adder", "Ancient", "Arachnid", "Arcadia", "Azure", "Barbarian", "Basilisk", "Battler", "Beard", "Beast", "Beelzebub", "Beryl", "Boar", "Bobcat", "Bohemian", "Bold", "Brawler", "Brilliant", "Bruiser", "Brute", "Butcher", "Canine", "Cardinal", "Carmine", "Catamount", "Centaur", "Cerulean", "Cherno", "Chinook", "Chrome", "Cobalt", "Cobra", "Cold", "Colossus", "Cosmic", "Cougar", "Coyote", "Crimson", "Dark", "Dastard", "Diablo", "Diligent", "Djinn", "Duke", "Dybbuk", "Ebon", "Echo", "Eden", "Edge", "Empyreal", "Enigma", "Epitome", "Exalted", "Feline", "Forsaken", "Fox", "Frankenstein", "Freak", "Frozen", "Fury", "Gargoyle", "Giant", "Gipsy", "Gladiator", "Glory", "Grand", "Grave", "Griffon", "Grim", "Guardian", "Harmony", "Heliacal", "Hellion", "Hermit", "Hollow", "Horizon", "Hound", "Hunger", "Hungry", "Hunter", "Hydra", "Hyena", "Imp", "Infinite", "Ironclad", "Ivory", "Jackal", "Jester", "Jigsaw", "Jinx", "Judge", "Juvenile", "Keen", "Knave", "Kraken", "Light", "Lucifer", "Lucky", "Mad", "Majestic", "Malachite", "Mammoth", "Maroon", "Matador", "Menace", "Mephistopheles", "Mercenary", "Muse", "Mute", "Nightowl", "Nomad", "Obsidian", "Ogre", "Onyx", "Oracle", "Ornate", "Ox", "Paladin", "Panther", "Paragon", "Patient", "Phoenix", "Pinnacle", "Primal", "Prime", "Prospect", "Puma", "Quiet", "Rattle", "Rebel", "Reckless", "Rhino", "Rogue", "Romeo", "Sanguine", "Savage", "Scoundral", "Scourge", "Secret", "Serenity", "Serpent", "Shangri-La", "Shoalin", "Silent", "Slayer", "Solar", "Soothsayer", "Spider", "Stalker", "Stark", "Stellar", "Striker", "Surgeon", "Tacit", "Tango", "Tanker", "Tarragon", "Titan", "Titanic", "Toreador", "Torero", "Treasure", "Tyrant", "Vagrant", "Valiant", "Viper", "Voodoo", "Vortex", "Vulcan", "Vulture", "Warlord", "Warmonger", "Warrior", "Watcher", "Weasel", "Werewolf", "Wicked", "Widow", "Wildcat", "Witch", "Wolf", "Wretched", "Wyvern", "Zingara" };
        string[] nm3_2 = { "Ace", "Adder", "Ancient", "Arachnid", "Assassin", "Barbarian", "Basilisk", "Battler", "Beast", "Beelzebub", "Boar", "Bobcat", "Bohemian", "Brawler", "Bruiser", "Brute", "Brutus", "Butcher", "Canine", "Centaur", "Chinook", "Cobra", "Colossus", "Cougar", "Coyote", "Danger", "Diablo", "Djinn", "Duke", "Echo", "Eden", "Edge", "Enigma", "Epitome", "Fox", "Frankenstein", "Freak", "Fury", "Gargoyle", "Giant", "Gladiator", "Glory", "Grave", "Griffon", "Guardian", "Gypsy", "Heliacal", "Hellion", "Hermit", "Horizon", "Hound", "Hunger", "Hunter", "Hydra", "Hyena", "Imp", "Jackal", "Jester", "Jigsaw", "Jinx", "Judge", "Juvenile", "Knave", "Kraken", "Light", "Lucifer", "Mammoth", "Maroon", "Matador", "Menace", "Mephistopheles", "Mercenary", "Muse", "Mute", "Nightowl", "Nomad", "Obsidian", "Ogre", "Onyx", "Oracle", "Ox", "Paladin", "Panther", "Paragon", "Patient", "Phoenix", "Pinnacle", "Primal", "Prime", "Prophet", "Prospect", "Puma", "Rebel", "Rhino", "Rogue", "Romeo", "Ronin", "Saber", "Savage", "Scoundrel", "Scourge", "Secret", "Serenity", "Serpent", "Shoalin", "Slayer", "Soothsayer", "Spider", "Stalker", "Stark", "Striker", "Surgeon", "Tango", "Tanker", "Tarragon", "Titan", "Titanic", "Toreador", "Torero", "Treasure", "Typhoon", "Tyrant", "Vagrant", "Viper", "Voodoo", "Vortex", "Vulcan", "Vulture", "Warlord", "Warmonger", "Warrior", "Watcher", "Weasel", "Werewolf", "Widow", "Wildcat", "Witch", "Wolf", "Wretched", "Wyvern", "Zingara" };
        
        // Full names
        string[] nm4 = { "Alligator", "Amida", "Ammit", "Amun", "Ape", "Arachnid", "Arbal", "Ash", "Asseg", "Asvin", "Atl", "Ax", "Babi", "Baboon", "Badger", "Ballis", "Barong", "Bayo", "Bayonit", "Bazoo", "Bear", "Beast", "Bicks", "Blade", "Blight", "Blits", "Blyght", "Brant", "Brawl", "Bruizer", "Brutus", "Buck", "Buster", "Butcher", "Bynd", "Cage", "Cane", "Cano", "Carkas", "Carn", "Carpse", "Carris", "Carse", "Carver", "Cast", "Cloud", "Cobra", "Combe", "Coyote", "Crane", "Croc", "Cudge", "Cyn", "Cynder", "Cypher", "Dagg", "Darum", "Dash", "Daver", "Davi", "Dino", "Draco", "Draegon", "Dragon", "Dragonfly", "Drake", "Dynamo", "Ebis", "Enigma", "Exxec", "Falcon", "Ferno", "Fiend", "Flame", "Frag", "Fume", "Glive", "Gorilla", "Grave", "Gritt", "Grym", "Gryme", "Guillo", "Hale", "Haros", "Hawk", "Helia", "Hippo", "Hitt", "Howitz", "Ibis", "Imp", "Izana", "Jackal", "Jag", "Kagu", "Kame", "Kangiten", "Kannon", "Komodo", "Kriz", "Kublai", "Laki", "Lance", "Leech", "Luce", "Machet", "Magnum", "Magnus", "Malyc", "Mammoth", "Manta", "Mantis", "Marat", "Massac", "Merce", "Mercer", "Mise", "Mongoose", "Mort", "Necros", "Nide", "Nightingale", "Nightowl", "Nuke", "Nunchu", "Oblivion", "Obsidian", "Onyx", "Oracle", "Osir", "Panther", "Pest", "Phoenix", "Pulse", "Pyre", "Quake", "Quaz", "Queen Bee", "Quelz", "Quiv", "Rackas", "Radic", "Raijin", "Raptor", "Rath", "Rattle", "Raze", "Razor", "Reaper", "Rex", "Rhino", "Riot", "Rudas", "Ruzh", "Ryze", "Sabe", "Sabre", "Samit", "Scarch", "Scarn", "Scatter", "Scimi", "Scourge", "Scrimm", "Scythe", "Sekh", "Semet", "Serpent", "Shav", "Shiver", "Silen", "Siris", "Skelt", "Skirm", "Skiv", "Skiver", "Slate", "Slayer", "Sliver", "Slyce", "Snyde", "Soot", "Spike", "Splinter", "Spyte", "Stal", "Stark", "Storm", "Surge", "Sybre", "Sylver", "Tank", "Tenac", "Tiger", "Torm", "Torren", "Tugs", "Vapor", "Viger", "Vissu", "Visus", "Weasel", "Wildcat", "Wolf", "Wolverine", "Wrangler", "Xerox", "Yce", "Zizor", "Zyn" };
        string[] nm5 = { "Abomination", "Ace", "Ares", "Aries", "Athena", "Augment", "Aurora", "Barbarian", "Barrage", "Beacon", "Beast", "Behemoth", "Blade", "Blitz", "Blitzkrieg", "Blockaide", "Brute", "Cascade", "Claw", "Coil", "Comet", "Compass", "Core", "Creature", "Critter", "Crossflare", "Crux", "Dagger", "Delirium", "Ditch", "Divebomb", "Dread", "Dynamite", "Dynamo", "Earthquake", "Eclipse", "Element", "Ember", "Enigma", "Error", "Feral", "Flinch", "Flow", "Flutter", "Flux", "Freak", "Fungus", "Fury", "Fuse", "Gadget", "Gleam", "Grease", "Growl", "Harness", "Havoc", "Hazard", "Hightop", "Hitch", "Honeybee", "Howler", "Hymn", "Icicle", "Inferno", "Influx", "Jeopardy", "Landslide", "Maniac", "Melody", "Mercy", "Meteoroid", "Nightlight", "Oracle", "Outburst", "Overboard", "Overflow", "Overload", "Paradox", "Particle", "Pest", "Pillage", "Posthaste", "Prodigy", "Pummel", "Pursuit", "Quake", "Quarrel", "Rabid", "Racer", "Rage", "Remix", "Requiem", "Residue", "Ricochet", "Riot", "Rodent", "Rubble", "Rumble", "Rush", "Salvo", "Savage", "Savvy", "Scourge", "Scratch", "Shamble", "Shift", "Sidearm", "Sideburns", "Sidelock", "Sidewire", "Sky-High", "Slide", "Smite", "Snake", "Snarl", "Snowdrift", "Sprocket", "Stampede", "Starblaster", "Starburst", "Stormrunner", "Sunblast", "Sunburst", "Switch", "Talon", "Tempest", "Thrust", "Thunder", "Torment", "Torrent", "Trailfire", "Trident", "Turbine", "Twinkle", "Typhoon", "Venture", "Vermin", "Vigor", "Virtue", "Volley", "Voltage", "Wallop", "Weasel", "Wheels", "Whistle", "Whiz", "Wrangle", "Wreckage", "Zephyr", "Zodiac" };
        string[] nm6 = { "Ache", "Aggressor", "Agitator", "Assaulter", "Austerity", "Battler", "Beast", "Brawler", "Bruiser", "Brute", "Bulldozer", "Bully", "Calamity", "Cataclysm", "Contender", "Curse", "Defiler", "Deserter", "Disrupter", "Dissenter", "Distress", "Doom", "Downfall", "Encroacher", "Fiend", "Gloom", "Grief", "Grievance", "Hardship", "Harrier", "Hazard", "Headache", "Hellion", "Infringer", "Injury", "Insurrector", "Intimidator", "Intruder", "Invader", "Jeopardy", "Misery", "Neglector", "Objector", "Opposer", "Oppressor", "Peril", "Radical", "Raider", "Rascal", "Rebel", "Reckoner", "Resister", "Revolter", "Rigor", "Rioter", "Ruffian", "Ruin", "Savage", "Scourge", "Scrapper", "Shirker", "Slugger", "Sorrow", "Squalor", "Stitch", "Striker", "Suffering", "Tanker", "Torment", "Tormenter", "Torture", "Transgressor", "Trespasser", "Tribulation", "Violator", "Woe", "Wreck", "Wreckage", "Wrecker" };

        string[] pre = { "i", "_", "@", "x", "xXx", "-", "+", "=", "#", "*", "|", "!"};
        string[] post = { "007", "#1", ".ai", ".cpu", "+1", "++", ".net", ".pro"};

        public string GetRandomName(string key)
        {
            var hash = key.GetHashCode();
            var rnd = new Random(hash);
            var sb = new StringBuilder();
            var postStr = string.Empty;

            if (rnd.NextDouble() < 0.3f)
            {
                postStr = pre[rnd.Next(pre.Length)];
                sb.Append(postStr);                
            }
            else if (rnd.NextDouble() < 0.3f)
            {
                postStr = rnd.Next(1981, 2007).ToString().Substring(2);
            }
            else if (rnd.NextDouble() < 0.2f)
            {
                postStr = post[rnd.Next(post.Length)];
            }
            else if (rnd.NextDouble() < 0.2f)
            {
                sb.Append(pre[rnd.Next(pre.Length)]);
            }            

            switch (rnd.Next(12))
            {
                case 0:
                    sb.Append(ToTitleCase(nm1_1[rnd.Next(nm1_1.Length)]));
                    sb.Append(ToTitleCase(nm1_2[rnd.Next(nm1_2.Length)]));
                    break;
                case 1:
                    sb.Append(ToTitleCase(nm2_1[rnd.Next(nm2_1.Length)]));
                    sb.Append(ToTitleCase(nm2_2[rnd.Next(nm2_2.Length)]));
                    break;
                case 2:
                    sb.Append(ToTitleCase(nm3_1[rnd.Next(nm3_1.Length)]));
                    sb.Append(ToTitleCase(nm3_2[rnd.Next(nm3_2.Length)]));
                    break;
                case 3:
                    sb.Append(ToTitleCase(nm4[rnd.Next(nm4.Length)]));
                    break;
                case 4:
                    sb.Append(ToTitleCase(nm5[rnd.Next(nm5.Length)]));
                    break;
                case 5:
                    sb.Append(ToTitleCase(nm6[rnd.Next(nm6.Length)]));
                    break;
                case 6:
                    sb.Append(ToTitleCase(nm1_1[rnd.Next(nm1_1.Length)]));
                    break;
                case 7:
                    sb.Append(ToTitleCase(nm1_2[rnd.Next(nm1_2.Length)]));
                    break;
                case 8:
                    sb.Append(ToTitleCase(nm2_1[rnd.Next(nm2_1.Length)]));
                    break;
                case 9:
                    sb.Append(ToTitleCase(nm2_1[rnd.Next(nm2_2.Length)]));
                    break;
                case 10:
                    sb.Append(ToTitleCase(nm3_1[rnd.Next(nm3_1.Length)]));
                    break;
                case 11:
                    sb.Append(ToTitleCase(nm3_2[rnd.Next(nm3_2.Length)]));
                    break;
            }
            
            sb.Append(postStr);
            return sb.ToString();
        }

        private static string ToTitleCase(string str)
        {
            var c = str.Substring(0, 1).ToUpper();

            return c + str.Substring(1);
        }
    }
}
