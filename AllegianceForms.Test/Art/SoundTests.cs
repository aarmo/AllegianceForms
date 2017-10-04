using AllegianceForms.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.IO;

namespace AllegianceForms.Test.Art
{
    [TestClass]
    public class SoundTests
    {
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
