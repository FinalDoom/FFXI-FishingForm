using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FFACETools;
using NUnit.Framework;

namespace Fishing.Test
{
    internal class PopulateFFACEResources : TestActionAttribute
    {
        public override void BeforeTest(TestDetails testDetails)
        {
            base.BeforeTest(testDetails);
            string dir = Path.Combine(Util.AssemblyDirectory, "resources");
            Assert.IsTrue(Directory.Exists(dir),
                "resources directory ({0}) must exist with Windower resources files (or other tests will fail).", dir);
            FFACE.WindowerPath = Util.AssemblyDirectory;
       }

        public ActionTargets Targets
        {
            get { return ActionTargets.Suite; }
        }
    }
}
