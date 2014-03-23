using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Fishing.Test
{
    static class Util
    {
        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        
        public static MethodInfo GetMethod(string className, string methodName)
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

        public static object InvokeMethod(MethodInfo method, object thisObj, params object[] parameters)
        {
            return method.Invoke(thisObj, parameters);
        }
    }
}
