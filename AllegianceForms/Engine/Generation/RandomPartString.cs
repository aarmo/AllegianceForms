using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AllegianceForms.Engine.Generation
{
    public class RandomPartString
    {
        private Dictionary<string,List<string>> _samples;
        private readonly List<string> _used;
        private List<string> _formats;

        public RandomPartString(string filename)
        {
            var json = File.ReadAllText(filename);
            dynamic job = JsonConvert.DeserializeObject(json);
            var formats = job["Formats"].ToObject<List<string>>();
            var numParts = job["NumParts"].ToObject<int>();

            _formats = new List<string>(formats);
            _samples = new Dictionary<string, List<string>>();

            for (var i = 1; i <= numParts; i++)
            {
                _samples.Add(i.ToString(), job[i.ToString()].ToObject<List<string>>());
            }

            _used = new List<string>();
        }

        public void Reset()
        {
            _used.Clear();
        }

        /// <summary>
        /// Get the next random string
        /// </summary>
        public string NextString
        {
            get
            {
                var format = StrategyGame.RandomItem(_formats);
                var tokens = format.Split(' ');
                var sb = new StringBuilder();

                foreach (var t in tokens)
                {
                    if (_samples.ContainsKey(t)) 
                        sb.Append(StrategyGame.RandomItem(_samples[t]));
                    else 
                        sb.Append(t);

                    sb.Append(" ");
                }

                var s = sb.ToString().Trim();
                _used.Add(s);

                return s;
            }

        }
    }
}
