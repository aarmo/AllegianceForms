using AllegianceForms.Engine.Map;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AllegianceForms
{
    public class Utils
    {
        public static void SerialiseToFile(string filename, object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(filename, json);
        }

        public static T DeserialiseFromFile<T>(string filename)
        {
            var json = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static GameMap CloneMap(GameMap map)
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                SerialiseToFile(tempFile, map);
                return LoadMapFromFile(tempFile);
            }
            finally
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception) { }
            }
        }

        public static GameMap LoadMapFromFile(string filename)
        {
            var map = DeserialiseFromFile<GameMap>(filename);

            // Re-associate the wormholes
            foreach (var w in map.Wormholes)
            {
                var oldS1 = w.Sector1;
                var oldS2 = w.Sector2;

                if (oldS1 != null)
                {
                    var newS1 = map.Sectors.Find(s => s.MapPosition.Equals(oldS1.MapPosition));
                    w.Sector1 = newS1;
                }

                if (oldS2 != null)
                {
                    var newS2 = map.Sectors.Find(s => s.MapPosition.Equals(oldS2.MapPosition));
                    w.Sector2 = newS2;
                }
            }

            return map;
        }
    }
}
