using AllegianceForms.Engine.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace AllegianceForms.Test.MapGeneration
{
    [TestClass]
    public class MapTests
    {
        [TestMethod]
        public void SmallCheck()
        {
            var m = RandomMap.GenerateSimpleMap(EMapSize.Small);
            
            m.Sectors.Count.ShouldBeGreaterThan(2);
        }

        [TestMethod]
        public void NormalCheck()
        {
            var m = RandomMap.GenerateSimpleMap(EMapSize.Normal);
            
            m.Sectors.Count.ShouldBeGreaterThan(4);
        }

        [TestMethod]
        public void LargeCheck()
        {
            var m = RandomMap.GenerateSimpleMap(EMapSize.Large);
            
            m.Sectors.Count.ShouldBeGreaterThan(6);
        }
    }
}
