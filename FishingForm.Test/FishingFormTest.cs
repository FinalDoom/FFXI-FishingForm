using System;
using System.Reflection;
using FFACETools;
using Fishing.Test;
using NUnit.Framework;

namespace Fishing
{
    [TestFixture, PopulateFFACEResources]
    public class FishingFormTest
    {
        [Test]
        public void TestGetFishName()
        {
            Assert.AreEqual("Crocodilos", FishUtils.GetFishName("a Crocodilos."));
            Assert.AreEqual("Dil", FishUtils.GetFishName("a Dil."));
            Assert.AreEqual("Quus x2", FishUtils.GetFishName("2 Quus!"));
            Assert.AreEqual("Bastore Sardine x3", FishUtils.GetFishName("3 Bastore Sardine!"));
        }

        [Test]
        public void TestZoneResolution()
        {
            Assert.AreEqual("Ship bound for Selbina", FishUtils.GetZoneName(Zone.Ferry_between_Mhaura__Selbina));
            Assert.AreEqual("Ship bound for Selbina (Pirates)", FishUtils.GetZoneName(Zone.Ferry_between_Mhaura__Selbina_Pirates));
            Assert.AreEqual("Ship bound for Mhaura", FishUtils.GetZoneName(Zone.Ferry_between_Selbina__Mhaura));
            Assert.AreEqual("Ship bound for Mhaura (Pirates)", FishUtils.GetZoneName(Zone.Ferry_between_Selbina__Mhaura_Pirates));
        }
    }
}
