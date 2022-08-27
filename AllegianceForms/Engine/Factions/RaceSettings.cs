using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.Factions
{
    public class RaceSetting
    {
        public ERaceType Race { get; set; }
        public float ShieldMultiplier { get; set; }
        public float HullMultiplier { get; set; }
        public float SpeedMultiplier { get; set;}
        public string OnShipDestroy { get; set; }


        internal static Dictionary<ERaceType, RaceSetting> LoadRaceSettingData(string dataFile)
        {
            var cfg = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true,
            };

            using (var textReader = File.OpenText(dataFile))
            {
                var csv = new CsvReader(textReader, cfg);

                return csv.GetRecords<RaceSetting>().ToDictionary(_ => _.Race);
            }
        }

    }
}
