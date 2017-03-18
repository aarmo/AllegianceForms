using Newtonsoft.Json;
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
    }
}
