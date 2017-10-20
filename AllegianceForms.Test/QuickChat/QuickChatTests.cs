using AllegianceForms.Engine;
using AllegianceForms.Engine.QuickChat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllegianceForms.Test.QuickChat
{
    [TestClass]
    public class QuickChatTests
    {
        [TestMethod]
        public void QuickCommsFileExists()
        {
            var exists = File.Exists(StrategyGame.QuickChatDataFile);
            exists.ShouldBe(true);
        }

        [TestMethod]
        public void CheckQuickComms()
        {
            var target = QuickComms.LoadQuickChat(StrategyGame.QuickChatDataFile);
            target.QuickItems.ShouldNotBeNull();
            target.QuickItems.Count.ShouldBeGreaterThan(0);
        }


        [TestMethod]
        public void AllQuickChatSoundFilesAreReferenced()
        {
            var target = QuickComms.LoadQuickChat(StrategyGame.QuickChatDataFile);
            var checkItems = new List<string>();

            foreach (var q in target.QuickItems)
            {
                if (string.IsNullOrWhiteSpace(q.Filename)) continue;

                checkItems.Add(q.Filename);
            }

            foreach (var i in checkItems)
            {
                ESounds e;
                var exists = Enum.TryParse(i, out e);
                
                exists.ShouldBe(true, "Sound isn't referenced: " + i);
            }
        }
    }
}
