using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MindMate.Tests.Model
{
    [TestClass]
    public class ResourcesTests
    {
        [TestMethod]
        public void TraverseThroughAllProperties()
        {
            foreach(var p in typeof(MindMate.Properties.Resources).GetProperties())
            {
                p.GetValue(null);
            }
                
        }
    }
}
