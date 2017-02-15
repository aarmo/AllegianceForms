using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.Bases
{
    public class BaseSpecs
    {
        public List<BaseSpec> Bases { get; set; }

        private BaseSpecs(IEnumerable<BaseSpec> items)
        {
            Bases = items.ToList();
        }

        public static BaseSpecs LoadBaseSpecs(string baseFile)
        {
            var cfg = new CsvConfiguration()
            {
                WillThrowOnMissingField = false,
                IgnoreBlankLines = true,
                AllowComments = true
            };

            using (var textReader = File.OpenText(baseFile))
            {
                var csv = new CsvReader(textReader, cfg);

                var records = csv.GetRecords<BaseSpec>().ToList();

                return new BaseSpecs(records);
            }
        }

        public Base CreateBase(EBaseType baseType, int team, Color teamColour, int sectorId)
        {
            var spec = Bases.FirstOrDefault(_ => _.Type == baseType);
            if (spec == null) return null;

            var bse = new Base(baseType, spec.Width, spec.Height, teamColour, team, spec.Health, sectorId);
            bse.Signature = spec.Signature;
            bse.ScanRange = spec.ScanRange;

            return bse;
        }
    }

    public class BaseSpec
    {
        public int Id { get; set; }
        public EBaseType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Health { get; set; }
        public int ScanRange { get; set; }
        public float Signature { get; set; }
    }
    }
