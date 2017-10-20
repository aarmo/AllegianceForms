using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Test.Art
{
    [TestClass]
    public class SoundTests
    {
        [TestMethod]
        public void ExtraSoundsDontExist()
        {
            var allItems = new List<string>();
            var extraItems = new List<string>();
            var e = (ESounds[])Enum.GetValues(typeof(ESounds));

            allItems.AddRange(e.Select(_ => _.ToString().ToUpper()));

            var files = Directory.GetFiles(StrategyGame.SoundsDir, "*.ogg");

            foreach (var f in files)
            {
                var i = f.Replace(StrategyGame.SoundsDir, string.Empty).Replace(".ogg", string.Empty).ToUpper();

                if (!allItems.Contains(i))
                    extraItems.Add(i);
            }

            extraItems.Count.ShouldBe(0, $"Unexpected sounds found: {string.Join("\n", extraItems)}");
        }

        [TestMethod]
        public void AllReferencedSoundFilesExist()
        {
            var sounds = Enum.GetValues(typeof(ESounds));

            foreach (var s in sounds)
            { 
                var file = SoundEffect.GetSoundFile((ESounds)s);
                var exists = File.Exists(file);
                exists.ShouldBe(true, "File doesn't exist: " + file);
            }
        }
    }
}
