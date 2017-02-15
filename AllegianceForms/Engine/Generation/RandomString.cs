using System.Collections.Generic;
using System.IO;

namespace AllegianceForms.Engine.Generation
{
    public class RandomString
    {
        private List<string> _samples;
        private readonly List<string> _used;

        public RandomString(string filename)
        {
            var strs = File.ReadAllLines(filename);

            _samples = new List<string>(strs);
            _used = new List<string>();
        }

        public void AddStrings(string filename)
        {
            var strs = File.ReadAllLines(filename);

            _samples.AddRange(strs);
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
                if (_used.Count == _samples.Count) return string.Empty;

                string s;
                do
                {
                    s = _samples[StrategyGame.Random.Next(_samples.Count)];
                } while (_used.Contains(s));
                _used.Add(s);

                return s;
            }

        }
    }
}
