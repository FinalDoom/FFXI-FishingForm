using System;
using System.Reflection;
using FFACETools;
using NUnit.Framework;

namespace Fishing
{
    [TestFixture]
    public class FishingFormTest
    {
        private static MethodInfo GetMethod(string className, string methodName)
        {
            if (string.IsNullOrEmpty(className))
            {
                Assert.Fail("className cannot be null or empty");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            var cls = Type.GetType(className);

            if (cls == null)
            {
                Assert.Fail(string.Format("{0} class not found.", className));
            }

            var method = cls.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }

        private static object InvokeMethod(MethodInfo method, object thisObj, params object[] parameters)
        {
            return method.Invoke(thisObj, parameters);
        }

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
