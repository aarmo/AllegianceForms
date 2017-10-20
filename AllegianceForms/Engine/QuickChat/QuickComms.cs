using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Engine.QuickChat
{
    public class QuickComms
    {
        public List<QuickChatItem> QuickItems { get; set; }

        public static QuickComms LoadQuickChat(string dataFile)
        {
            var cfg = new CsvConfiguration()
            {
                WillThrowOnMissingField = false,
                IgnoreBlankLines = true,
            };

            using (var textReader = File.OpenText(dataFile))
            {
                var csv = new CsvReader(textReader, cfg);

                return new QuickComms
                {
                     QuickItems = csv.GetRecords<QuickChatItem>().ToList()
                };
            }
        }

    }

    public class QuickChatItem
    {
        public int MenuId { get; set; }
        public string QuickComms { get; set; }
        public string Key { get; set; }
        public string Filename { get; set; }
        public string OpenMenuId { get; set; }
    }
}
