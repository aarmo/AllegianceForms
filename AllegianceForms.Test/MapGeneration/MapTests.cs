using AllegianceForms.Engine.Generation;
using AllegianceForms.Engine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.MapGeneration
{
    [TestClass]
    public class MapTests
    {
        [TestMethod]
        public void AvailableMaps_Unrestricted()
        {
            var maps = GameMaps.AvailableMaps(2, true);

            maps.Length.ShouldBeGreaterThan(0);
            maps.ShouldContain("Brawl2");
            maps.ShouldContain(GameMaps.RandomMapName_Small);
            maps.ShouldContain(GameMaps.RandomMapName_Normal);
            maps.ShouldContain(GameMaps.RandomMapName_Large);
        }

        [TestMethod]
        public void AvailableMaps_Restricted()
        {
            var maps = GameMaps.AvailableMaps(2, false);

            maps.Length.ShouldBeGreaterThan(0);
            maps.ShouldNotContain("Brawl2");
            maps.ShouldNotContain(GameMaps.RandomMapName_Small);
            maps.ShouldNotContain(GameMaps.RandomMapName_Normal);
            maps.ShouldNotContain(GameMaps.RandomMapName_Large);
        }

        [TestMethod]
        public void SmallCheck()
        {
            var m = RandomMap.GenerateMirroredMap(EMapSize.Small);
            
            m.Sectors.Count.ShouldBeGreaterThan(2);
        }

        [TestMethod]
        public void NormalCheck()
        {
            var m = RandomMap.GenerateMirroredMap(EMapSize.Normal);
            
            m.Sectors.Count.ShouldBeGreaterThan(4);
        }

        [TestMethod]
        public void LargeCheck()
        {
            var m = RandomMap.GenerateMirroredMap(EMapSize.Large);
            
            m.Sectors.Count.ShouldBeGreaterThan(6);
        }
    }
}
